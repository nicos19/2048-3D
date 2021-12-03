using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject PlayFieldGameObject;
    public GameObject TilesGameObject;
    public GameObject TilePrefab;
    public const int FieldSize = 4;

    /// <value>
    /// Property <c>PlayField</c> represents the current state of the play field.
    /// </value>
    public Cell[,] PlayField { get; set; }

    /// <value>
    /// Property <c>CountUnfinishedMoves</c> represents the number of tile moves that are still executed right now.
    /// </value>
    public static int CountUnfinishedTileMoves { get; set; }

    /// <value>
    /// Property <c>MoveMadeThisRound</c> is set <c>true</c> when a player input results in an actual action on the play field.
    /// </value>
    public bool MoveMadeThisRound { get; set; }

    /// <value>
    /// Property <c>GamePaused</c> is <c>true</c> if game is paused due to open menu or game over.
    /// </value>
    public bool GamePaused { get; set; }

    /// <value>
    /// Property <c>ReadyForUserInput</c> is <c>true</c> if game is ready to receive next input by user.
    /// </value>
    public bool ReadyForUserInput { get; set; }

    private List<Cell> _allCells;
    private int _score;
    private int _bestScore;
    private bool _waitForUnfinishedTileMoves;
    private bool _gameWon;
    private InterfaceManager INTERFACE_MANAGER;
    private AudioManager AUDIO_MANAGER;
    private SavegameManager SAVEGAME_MANAGER;


    // Start is called before the first frame update
    void Start()
    {
        INTERFACE_MANAGER = gameObject.GetComponent<InterfaceManager>();
        AUDIO_MANAGER = gameObject.GetComponent<AudioManager>();
        SAVEGAME_MANAGER = gameObject.GetComponent<SavegameManager>();

        // load best score
        if (PlayerPrefs.HasKey("bestScore"))
        {
            _bestScore = PlayerPrefs.GetInt("bestScore");
        }

        // load game if savegame exists
        if (SAVEGAME_MANAGER.SavegameExists())
        {
            RestoreLoadedGame(SAVEGAME_MANAGER.LoadSavegame());
        }
        else
        {
            // no savegame found -> start new game
            StartNewGame();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_waitForUnfinishedTileMoves && CountUnfinishedTileMoves == 0)
        {
            // GameManager was waiting for tile moves to be finished & all moves are finished now
            _waitForUnfinishedTileMoves = false;
            NextTurn();
        } 
    }

    /// <summary>
    /// This method resets the game to initial state without any tiles.
    /// </summary>
    public void ResetGame()
    {
        GamePaused = false;
        _gameWon = false;
        AUDIO_MANAGER.ShiftSoundPlayedThisRound = false;
        _score = 0;
        MoveMadeThisRound = false;
        CountUnfinishedTileMoves = 0;

        PlayField = new Cell[FieldSize, FieldSize];
        _allCells = new List<Cell>();

        // get cell instances from "PlayFieldGameObject"
        foreach (Transform cell in PlayFieldGameObject.transform)
        {
            _allCells.Add(cell.gameObject.GetComponent<Cell>());
        }

        int i = 0;  // counts through "_allCells"
        // create play field with empty cells
        for (int y = 0; y < FieldSize; y++)
        {
            for (int x = 0; x < FieldSize; x++)
            {
                PlayField[y, x] = _allCells[i];
                PlayField[y, x].Tile = new List<GameObject>();
                i++;
            }
        }

        // destroy all tiles of old game
        DestroyAllTiles();
    }

    /// <summary>
    /// This method resets the game state to initial state and starts a new game.
    /// </summary>
    public void StartNewGame()
    {
        ResetGame();
        INTERFACE_MANAGER.UpdateScoreboard(_score, _bestScore);

        // create two initial tiles
        CreateRandomTile();
        CreateRandomTile();

        ReadyForUserInput = true;
    }

    /// <summary>
    /// This method restores the game state that fits <c>savegame</c>.
    /// </summary>
    /// <param name="savegame"></param>
    /// <param name="score"></param>
    public void RestoreLoadedGame(Savegame savegame)
    {
        ResetGame();

        _score = savegame.Score;
        INTERFACE_MANAGER.UpdateScoreboard(_score, _bestScore);

        foreach (SavegameTile savegameTile in savegame.SavegameTiles)
        {
            CreateTile(savegameTile.CellX, savegameTile.CellY, savegameTile.TileValue);
        }

        // check if game ended with game over or win
        _gameWon = savegame.GameWon;
        if (savegame.ActiveEndingScreen == "win")
        {
            GamePaused = true;
            INTERFACE_MANAGER.ShowWinScreen(_score, _bestScore);
            return;
        }
        else if (savegame.ActiveEndingScreen == "gameover")
        {
            GamePaused = true;
            INTERFACE_MANAGER.ShowGameOverScreen(_score, _bestScore);
        }

        ReadyForUserInput = true;
    }

    /// <summary>
    /// This method saves the current game state.
    /// </summary>
    public void Save()
    {
        SAVEGAME_MANAGER.CreateSavegameFile(_score, TilesGameObject, _gameWon, INTERFACE_MANAGER.ActiveEndingScreen());
    }

    /// <summary>
    /// This method creates a tile at cell <c>(tilePosX, tilePosY)</c> with value <c>tileValue</c>.
    /// </summary>
    /// <param name="tilePosX"></param>
    /// <param name="tilePosY"></param>
    /// <param name="tileValue"></param>
    public void CreateTile(int tilePosX, int tilePosY, int tileValue)
    {
        Vector3 newTileWorldPosition = new Vector3(PlayField[tilePosY, tilePosX].gameObject.transform.position.x,
                                                   TilePrefab.transform.position.y,
                                                   PlayField[tilePosY, tilePosX].gameObject.transform.position.z);
        // create new tile game object
        GameObject newTile = Instantiate(TilePrefab, newTileWorldPosition, TilePrefab.transform.rotation);
        newTile.transform.SetParent(TilesGameObject.transform, true);
        newTile.GetComponent<Tile>().AssignTileValue(tileValue);
        newTile.GetComponent<Tile>().UpdateMaterial();
        newTile.GetComponent<Tile>().CurrentCell = PlayField[tilePosY, tilePosX];
        newTile.GetComponent<Tile>().TargetCell = PlayField[tilePosY, tilePosX];
        newTile.GetComponent<Tile>().AUDIO_MANAGER = AUDIO_MANAGER;

        // assign tile to its current cell
        PlayField[tilePosY, tilePosX].AssignTileToCell(newTile);
    }

    /// <summary>
    /// This method creates a random tile with value of <c>2</c> (90%) or <c>4</c> (10%) in an empty cell.
    /// </summary>
    public void CreateRandomTile()
    {
        System.Random randomGenerator = new System.Random();


        int tilePosX = randomGenerator.Next(0, 4);
        int tilePosY = randomGenerator.Next(0, 4);
        while (PlayField[tilePosY, tilePosX].Tile.Count != 0)
        {
            // cell (tilePosX, tilePosY) is not empty -> try again
            tilePosX = randomGenerator.Next(0, 4);
            tilePosY = randomGenerator.Next(0, 4);
        }
        int tileValue;
        if (randomGenerator.Next(0, 10) == 0)
        {
            tileValue = 4;
        }
        else
        {
            tileValue = 2;
        }

        CreateTile(tilePosX, tilePosY, tileValue);
    }

    /// <summary>
    /// This method destroys all tile game objects currently existing in the scene.
    /// </summary>
    public void DestroyAllTiles()
    {
        foreach (Transform tile in TilesGameObject.transform)
        {
            Destroy(tile.gameObject);
        }
    }

    /// <summary>
    /// This method increases the score by <c>addedScore</c>.
    /// </summary>
    /// <param name="addedScore"></param>
    public void IncreaseScore(int addedScore)
    {
        _score += addedScore;
        if (_score > _bestScore)
        {
            // new high score
            _bestScore = _score;
        }
    }
    
    /// <summary>
    /// This method does the transition from current round to the next round of the game.
    /// It checks for win/game over and creates one new random tile if the game continues.
    /// </summary>
    public void NextTurn()
    {
        if (CountUnfinishedTileMoves != 0)
        {
            // there are still tile moves that are being executed right now -> wait until all moves are done
            _waitForUnfinishedTileMoves = true;
            return;
        }

        if (!AnyMoveMade())
        {
            // last user input did not cause any move or merge of any tile -> do not go to next round
            ReadyForUserInput = true;
            return;
        }

        // update displayed score
        INTERFACE_MANAGER.UpdateScoreboard(_score, _bestScore);

        // check winning condition
        for (int x = 0; x < FieldSize; x++)
        {
            for (int y = 0; y < FieldSize; y++)
            {
                PlayField[y, x].ResetHasMergedTile();
                if (PlayField[y, x].Tile.Count != 0 && PlayField[y, x].GetTileValue() == 2048)
                {
                    if (!_gameWon)
                    {
                        // player has just merged a 2048 tile -> game is won
                        GamePaused = true;
                        _gameWon = true;
                        INTERFACE_MANAGER.ShowWinScreen(_score, _bestScore);
                        return;
                    }
                }
            }
        }

        InitializeNextRound();
    }

    /// <summary>
    /// When a new round is started, this method executes the round's initialization. 
    /// Used by <c>NextTurn()</c> or <c>InterfaceManager.Continue()</c>.
    /// </summary>
    public void InitializeNextRound()
    {
        // next round begins -> new tile appears on the play field
        AUDIO_MANAGER.ShiftSoundPlayedThisRound = false;
        MoveMadeThisRound = false;
        CreateRandomTile();

        // check if any shift is possible
        if (!AnyShiftPossible())
        {
            // player cannot do any shift action -> game over
            GamePaused = true;
            INTERFACE_MANAGER.ShowGameOverScreen(_score, _bestScore);
        }

        ReadyForUserInput = true;
    }

    /// <summary>
    /// This method checks if any shift action can be executed.
    /// </summary>
    /// <returns>Returns <c>true</c> if any shift is possible, <c>false</c> otherwise. </returns>
    public bool AnyShiftPossible()
    {
        for (int x = 0; x < FieldSize; x++)
        {
            for (int y = 0; y < FieldSize; y++)
            {
                if (PlayField[y, x].Tile.Count == 0)
                {
                    // play field has an empty tile -> there is a possible shift action
                    return true;
                }
                if (TileHasEqualNeighbor(PlayField[y, x]))
                {
                    // there are two neighbored tiles with same value -> there is a possible shift (merge) action
                    return true;
                }
            }
        }
        return false;  // no possible shift action found
    }

    /// <summary>
    /// This method checks if any tile was moved or was merged during the current round.
    /// </summary>
    /// <returns>Returns <c>true</c> if any shift action was executed, 
    /// <c>false</c> otherwise.
    /// </returns>
    public bool AnyMoveMade()
    {
        return MoveMadeThisRound;
    }

    /// <summary>
    /// This method checks if the tile in <c>cell</c> has any neighbored tile with same value.
    /// </summary>
    /// <param name="cell"></param>
    /// <returns>Returns <c>true</c> if a neighbored tile with same value is found, <c>false</c> otherwise.</returns>
    public bool TileHasEqualNeighbor(Cell cell)
    {
        if (cell.Tile.Count == 0)
        {
            // cell has no tile
            return false;
        }

        Cell neighbor;
        // neighbor above
        if (cell.Y > 0)
        {
            neighbor = PlayField[cell.Y - 1, cell.X];
            if (neighbor.Tile.Count != 0 && cell.Tile[0].GetComponent<Tile>().TileValue == neighbor.Tile[0].GetComponent<Tile>().TileValue)
            {
                return true;
            }
        }
        // neighbor beneath
        if (cell.Y < FieldSize - 1)
        {
            neighbor = PlayField[cell.Y + 1, cell.X];
            if (neighbor.Tile.Count != 0 && cell.Tile[0].GetComponent<Tile>().TileValue == neighbor.Tile[0].GetComponent<Tile>().TileValue)
            {
                return true;
            }
        }
        // neighbor leftward
        if (cell.X > 0)
        {
            neighbor = PlayField[cell.Y, cell.X - 1];
            if (neighbor.Tile.Count != 0 && cell.Tile[0].GetComponent<Tile>().TileValue == neighbor.Tile[0].GetComponent<Tile>().TileValue)
            {
                return true;
            }
        }
        // neighbor rightward
        if (cell.X < FieldSize - 1)
        {
            neighbor = PlayField[cell.Y, cell.X + 1];
            if (neighbor.Tile.Count != 0 && cell.Tile[0].GetComponent<Tile>().TileValue == neighbor.Tile[0].GetComponent<Tile>().TileValue)
            {
                return true;
            }
        }

        // no neighbored tile with same value found
        return false;
    }

    

}
