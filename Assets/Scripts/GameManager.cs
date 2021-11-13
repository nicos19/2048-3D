using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject PlayFieldGameObject;

    private const int FieldSize = 4;
    private bool _isGameStopped;
    private Cell[,] _playField;
    private List<Cell> _allCells;
    private int _score;
    private bool _moveMadeThisRound;


    // Start is called before the first frame update
    void Start()
    {
        CreateNewGame();
    }

    // Update is called once per frame
    void Update()
    {

    }


    /// <summary>
    /// resets the game state to start a new game
    /// </summary>
    public void CreateNewGame()
    {
        _isGameStopped = false;
        _moveMadeThisRound = false;
        _score = 0;

        _playField = new Cell[FieldSize, FieldSize];
        _allCells = new List<Cell>();

        // get cell instances from "PlayFieldGameObject"
        foreach (Transform cell in PlayFieldGameObject.transform)
        {
            _allCells.Add(cell.gameObject.GetComponent<Cell>());
        }

        int i = 0;  // counts through "_allCells"
        Cell nextCell;
        // create play field with empty cells
        for (int x = 0; x < FieldSize; x++)
        {
            for (int y = 0; y < FieldSize; y++)
            {
                nextCell =  _allCells[i];
                _playField[y, x] = nextCell;
                _playField[y, x].AssignTileToCell(0);
                i++;
            }
        }

        // create two initial tiles
        CreateTile();
        CreateTile();

        DrawScene();
    }

    /// <summary>
    /// This methods updates the visual representation of the play field.
    /// </summary>    
    public void DrawScene()
    {
        for (int x = 0; x < FieldSize; x++)
        {
            for (int y = 0; y < FieldSize; y++)
            {
                _playField[y, x].DrawCell();
            }
        }
    }

    // creates a random tile with value of 2 (90%) or 4 (10%) in an empty cell
    public void CreateTile()
    {
        System.Random randomGenerator = new System.Random();


        int tilePosX = randomGenerator.Next(0, 4);
        int tilePosY = randomGenerator.Next(0, 4);
        while (_playField[tilePosY, tilePosX].Tile != 0)
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
        _playField[tilePosY, tilePosX].AssignTileToCell(tileValue);
        _playField[tilePosY, tilePosX].UpdateColor();
    }

    /// <summary>
    /// This method executes a shift of all tiles in <c>direction</c> depending by player input.
    /// </summary>
    /// <param name="direction"></param>
    public void Shift(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                for (int x = 0; x < FieldSize; x++)
                {
                    for (int y = 0; y < FieldSize; y++)
                    {
                        MoveTile(_playField[y, x], direction);
                    }
                }
                break;
            case Direction.Right:
                for (int x = FieldSize - 1; x >= 0; x--)
                {
                    for (int y = 0; y < FieldSize; y++)
                    {
                        MoveTile(_playField[y, x], direction);
                    }
                }
                break;
            case Direction.Down:
                for (int x = 0; x < FieldSize; x++)
                {
                    for (int y = FieldSize - 1; y >= 0; y--)
                    {
                        MoveTile(_playField[y, x], direction);
                    }
                }
                break;
            case Direction.Left:
                for (int x = 0; x < FieldSize; x++)
                {
                    for (int y = 0; y < FieldSize; y++)
                    {
                        MoveTile(_playField[y, x], direction);
                    }
                }
                break;
        }
    }

    /// <summary>
    /// This method moves tile at <c>cell</c> in <c>direction</c> as far as possible.
    /// There is no action if <c>cell</c> does not contain a tile.
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="direction"></param>
    public void MoveTile(Cell cell, Direction direction)
    {
        if (cell.Tile == 0)
        {
            // no tile in this cell
            return;
        }

        Cell neighborCell;
        switch (direction)
        {
            case Direction.Up:
                if (cell.Y == 0)
                {
                    // tile cannot move farther
                    return;
                }
                neighborCell = _playField[cell.Y - 1, cell.X];
                break;
            case Direction.Right:
                if (cell.X == FieldSize - 1)
                {
                    // tile cannot move farther
                    return;
                }
                neighborCell = _playField[cell.Y, cell.X + 1];
                break;
            case Direction.Down:
                if (cell.Y == FieldSize - 1)
                {
                    // tile cannot move farther
                    return;
                }
                neighborCell = _playField[cell.Y + 1, cell.X];
                break;
            default:  // case: LEFT
                if (cell.X == 0)
                {
                    // tile cannot move farther
                    return;
                }
                neighborCell = _playField[cell.Y, cell.X - 1];
                break;
        }

        if (neighborCell.Tile == 0)
        {
            // nieghbor cell is empty -> move there and try to move in "direction" again
            neighborCell.AssignTileToCell(cell.Tile);
            neighborCell.UpdateColor();
            cell.AssignTileToCell(0);
            cell.UpdateColor();
            MoveTile(neighborCell, direction);
            _moveMadeThisRound = true;
        }
        else if (neighborCell.Tile == cell.Tile && !neighborCell.HasMergedTile)
        {
            // neighbor cell contains tile with same value as tile in "cell" -> merge both tiles
            // no merge if tile in neighbor cell is result of former merge during current round
            neighborCell.AssignTileToCell(2 * neighborCell.Tile);
            neighborCell.UpdateColor();
            neighborCell.TileMerged();
            cell.AssignTileToCell(0);
            cell.UpdateColor();
            _moveMadeThisRound = true;
            _score += neighborCell.Tile;
        }
    }

    /// <summary>
    /// This method initializes the next round of the game. 
    /// It checks for win/game over and creates one new random tile if the game continues.
    /// </summary>
    public void NextTurn()
    {
        if (!AnyMoveMade())
        {
            // last user input did not cause any move or merge of any tile -> do not go to next round
            return;
        }

        // check winning condition
        for (int x = 0; x < FieldSize; x++)
        {
            for (int y = 0; y < FieldSize; y++)
            {
                _playField[y, x].ResetHasMergedTile();
                if (_playField[y, x].Tile == 2048)
                {
                    // player has a 2048 tile -> game is won
                    _isGameStopped = true;
                    DrawScene();
                    ShowWinMessage();
                    return;
                }
            }
        }

        // next round begins -> new tile appears on the play field
        _moveMadeThisRound = false;
        CreateTile();
        DrawScene();

        // check if any shift is possible
        if (!AnyShiftPossible())
        {
            // player cannot do any shift action -> game over
            _isGameStopped = true;
            ShowGameOverMessage();
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
                if (_playField[y, x].Tile == 0)
                {
                    // play field has an empty tile -> there is a possible shift action
                    return true;
                }
                if (TileHasEqualNeighbor(_playField[y, x]))
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
        return _moveMadeThisRound;
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
            neighbor = _playField[cell.Y - 1, cell.X];
            if (cell.Tile == neighbor.Tile)
            {
                return true;
            }
        }
        // neighbor beneath
        if (cell.Y < FieldSize - 1)
        {
            neighbor = _playField[cell.Y + 1, cell.X];
            if (cell.Tile == neighbor.Tile)
            {
                return true;
            }
        }
        // neighbor leftward
        if (cell.X > 0)
        {
            neighbor = _playField[cell.Y, cell.X - 1];
            if (cell.Tile == neighbor.Tile)
            {
                return true;
            }
        }
        // neighbor rightward
        if (cell.X < FieldSize - 1)
        {
            neighbor = _playField[cell.Y, cell.X + 1];
            if (cell.Tile == neighbor.Tile)
            {
                return true;
            }
        }

        // no neighbored tile with same value found
        return false;
    }

    /// <summary>
    /// This method causes the game to show a "You win!" message.
    /// </summary>
    public void ShowWinMessage()
    {
        // TODO
    }

    /// <summary>
    /// This method causes the game to show a "Game over!" message.
    /// </summary>
    public void ShowGameOverMessage()
    {
        // TODO
    }

}
