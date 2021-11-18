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
    /// Property <c>Tiles</c> is a list with all tiles on the play field.
    /// </value>
    public List<GameObject> Tiles { get; set; }

    /// <value>
    /// Property <c>CountUnfinishedMoves</c> represents the number of tile moves that are still executed right now.
    /// </value>
    public static int CountUnfinishedTileMoves { get; set; }

    /// <value>
    /// Property <c>MoveMadeThisRound</c> is set <c>true</c> when a player input results in an actual action on the play field.
    /// </value>
    public bool MoveMadeThisRound { get; set; }

    private bool _gamePaused;
    private List<Cell> _allCells;
    private int _score;
    private int _bestScore;
    private bool _waitForUnfinishedTileMoves;
    private InterfaceManager INTERFACE_MANAGER;


    // Start is called before the first frame update
    void Start()
    {
        INTERFACE_MANAGER = gameObject.GetComponent<InterfaceManager>();
        CreateNewGame();
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
    /// resets the game state to start a new game
    /// </summary>
    public void CreateNewGame()
    {
        _gamePaused = false;
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

        // create two initial tiles
        CreateTile();
        CreateTile();
    }

    /// <summary>
    /// This method creates a random tile with value of <c>2</c> (90%) or <c>4</c> (10%) in an empty cell.
    /// </summary>
    public void CreateTile()
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

        Vector3 newTileWorldPosition = new Vector3(PlayField[tilePosY, tilePosX].gameObject.transform.position.x, 
                                                   TilePrefab.transform.position.y,
                                                   PlayField[tilePosY, tilePosX].gameObject.transform.position.z);
        // create new tile game object
        GameObject newTile = Instantiate(TilePrefab, newTileWorldPosition, TilePrefab.transform.rotation);
        newTile.GetComponent<Tile>().AssignTileValue(tileValue);
        newTile.GetComponent<Tile>().UpdateMaterial();
        newTile.GetComponent<Tile>().CurrentCell = PlayField[tilePosY, tilePosX];
        newTile.GetComponent<Tile>().TargetCell = PlayField[tilePosY, tilePosX];

        PlayField[tilePosY, tilePosX].AssignTileToCell(newTile);
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
    /// This method initializes the next round of the game. 
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
                    // player has a 2048 tile -> game is won
                    _gamePaused = true;
                    INTERFACE_MANAGER.ShowWinMessage();
                    return;
                }
            }
        }

        // next round begins -> new tile appears on the play field
        MoveMadeThisRound = false;
        CreateTile();

        // check if any shift is possible
        if (!AnyShiftPossible())
        {
            // player cannot do any shift action -> game over
            _gamePaused = true;
            INTERFACE_MANAGER.ShowGameOverMessage();
        }
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
        Cell neighbor;
        // neighbor above
        if (cell.Y > 0)
        {
            neighbor = PlayField[cell.Y - 1, cell.X];
            if (cell.Tile == neighbor.Tile)
            {
                return true;
            }
        }
        // neighbor beneath
        if (cell.Y < FieldSize - 1)
        {
            neighbor = PlayField[cell.Y + 1, cell.X];
            if (cell.Tile == neighbor.Tile)
            {
                return true;
            }
        }
        // neighbor leftward
        if (cell.X > 0)
        {
            neighbor = PlayField[cell.Y, cell.X - 1];
            if (cell.Tile == neighbor.Tile)
            {
                return true;
            }
        }
        // neighbor rightward
        if (cell.X < FieldSize - 1)
        {
            neighbor = PlayField[cell.Y, cell.X + 1];
            if (cell.Tile == neighbor.Tile)
            {
                return true;
            }
        }

        // no neighbored tile with same value found
        return false;
    }

    

}
