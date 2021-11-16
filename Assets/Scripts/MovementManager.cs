using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    private GameManager GAME_MANAGER;

    // Start is called before the first frame update
    void Start()
    {
        GAME_MANAGER = gameObject.GetComponent<GameManager>();
    }

    /// <summary>
    /// This method simulates the move of tile at <c>cell</c> in <c>direction</c> as far as possible.
    /// There is no action if <c>cell</c> does not contain a tile.
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="direction"></param>
    public void SimulateTileMove(GameObject cellGameObject, Direction direction)
    {
        Cell cell = cellGameObject.GetComponent<Cell>();

        if (cell.Tile.Count == 0)
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
                neighborCell = GAME_MANAGER.PlayField[cell.Y - 1, cell.X];
                break;
            case Direction.Right:
                if (cell.X == GameManager.FieldSize - 1)
                {
                    // tile cannot move farther
                    return;
                }
                neighborCell = GAME_MANAGER.PlayField[cell.Y, cell.X + 1];
                break;
            case Direction.Down:
                if (cell.Y == GameManager.FieldSize - 1)
                {
                    // tile cannot move farther
                    return;
                }
                neighborCell = GAME_MANAGER.PlayField[cell.Y + 1, cell.X];
                break;
            default:  // case: LEFT
                if (cell.X == 0)
                {
                    // tile cannot move farther
                    return;
                }
                neighborCell = GAME_MANAGER.PlayField[cell.Y, cell.X - 1];
                break;
        }

        if (neighborCell.Tile.Count == 0)
        {
            // nieghbor cell is empty -> move there and try to move in "direction" again
            neighborCell.AssignTileToCell(cell.GetTileGameObject());
            cell.Tile.Clear();
            SimulateTileMove(neighborCell.gameObject, direction);
            GAME_MANAGER.MoveMadeThisRound = true;
        }
        else if (neighborCell.GetTileGameObject().GetComponent<Tile>().TileValue == cell.GetTileGameObject().GetComponent<Tile>().TileValue && !neighborCell.HasMergedTile)
        {
            // neighbor cell contains tile with same value as tile in "cell" -> merge both tiles
            // no merge if tile in neighbor cell is result of former merge during current round
            neighborCell.AssignTileToCell(cell.GetTileGameObject());
            neighborCell.TileMerged();
            cell.Tile.Clear();
            GAME_MANAGER.MoveMadeThisRound = true;
            GAME_MANAGER.IncreaseScore(neighborCell.GetTileGameObject().GetComponent<Tile>().TileValue * 2);
        }
    }

    /// <summary>
    /// This method simulates a shift of all tiles in <c>direction</c> depending on player input.
    /// </summary>
    /// <param name="direction"></param>
    public void SimulateShift(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                for (int x = 0; x < GameManager.FieldSize; x++)
                {
                    for (int y = 0; y < GameManager.FieldSize; y++)
                    {
                        SimulateTileMove(GAME_MANAGER.PlayField[y, x].gameObject, direction);
                    }
                }
                break;
            case Direction.Right:
                for (int x = GameManager.FieldSize - 1; x >= 0; x--)
                {
                    for (int y = 0; y < GameManager.FieldSize; y++)
                    {
                        SimulateTileMove(GAME_MANAGER.PlayField[y, x].gameObject, direction);
                    }
                }
                break;
            case Direction.Down:
                for (int x = 0; x < GameManager.FieldSize; x++)
                {
                    for (int y = GameManager.FieldSize - 1; y >= 0; y--)
                    {
                        SimulateTileMove(GAME_MANAGER.PlayField[y, x].gameObject, direction);
                    }
                }
                break;
            case Direction.Left:
                for (int x = 0; x < GameManager.FieldSize; x++)
                {
                    for (int y = 0; y < GameManager.FieldSize; y++)
                    {
                        SimulateTileMove(GAME_MANAGER.PlayField[y, x].gameObject, direction);
                    }
                }
                break;
        }
    }

    /// <summary>
    /// This method orders a shift of all tiles in <c>direction</c> depending on player input.
    /// </summary>
    /// <param name="direction"></param>
    public void CommandShiftTiles(Direction direction)
    {
        SimulateShift(direction);

        for (int x = 0; x < GameManager.FieldSize; x++)
        {
            for (int y = 0; y < GameManager.FieldSize; y++)
            {
                Cell cell = GAME_MANAGER.PlayField[y, x];

                if (cell.Tile.Count == 0)
                {
                    // cell (x, y) is empty -> go on
                    continue;
                }
                
                foreach (GameObject tileGameObject in GAME_MANAGER.PlayField[y, x].Tile)
                {
                    tileGameObject.GetComponent<Tile>().TargetCell = cell;

                    if (tileGameObject.GetComponent<Tile>().TargetCell != tileGameObject.GetComponent<Tile>().CurrentCell)
                    {
                        // tile shall move!
                        tileGameObject.GetComponent<Tile>().CommandMoveTile(cell, direction);
                    }
                }
            }
        }
    }

}
