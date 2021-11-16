using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    /// <value>
    /// Property <c>TileValue</c> represents the number associated with the tile.
    /// </value>
    public int TileValue { get; set; }

    /// <value>
    /// Property <c>CurrentCell</c> represents the cell at which this tile is currently.
    /// </value>
    public Cell CurrentCell { get; set; }

    /// <value>
    /// Property <c>TargetCell</c> represents the cell to which this tile shall move next.
    /// If <c>TargetCell</c> equals <c>CurrentCell</c> then no further move is planned currently.
    /// </value>
    public Cell TargetCell { get; set; }

    private Direction _moveDirection;
    private Vector3 _moveDirectionVector3;
    private bool _doMove;
    private float _moveSpeed = 25;


    // Update is called once per frame
    void Update()
    {
        // shall this tile be moved?
        if (_doMove)
        {
            MoveTile();
        }
    }

    /// <summary>
    /// This method assigns sets <c>_tileValue</c> and updates the appearance of the tile game object
    /// (by updating the material).
    /// </summary>
    /// <param name="tileValue"></param>
    public void AssignTileValue(int tileValue)
    {
        TileValue = tileValue;
        UpdateMaterial();
    }

    /// <summary>
    /// This method updates the material of the tile game object, so that it fits <c>_tileValue</c>.
    /// </summary>
    public void UpdateMaterial()
    {
        gameObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>($"Materials/TileMaterial{TileValue}");
    }

    /// <summary>
    /// This method orders the tile to move in <c>direction</c> to <c>targetCell</c>.
    /// </summary>
    /// <param name="targetCell"></param>
    /// <param name="direction"></param>
    public void CommandMoveTile(Cell targetCell, Direction direction)
    {
        TargetCell = targetCell;
        _moveDirection = direction;
        _moveDirectionVector3 = DirectionToVector3(direction);
        _doMove = true;
        GameManager.CountUnfinishedTileMoves += 1;
    }

    /// <summary>
    /// This method moves the tile in <c>_moveDirection</c> until it reaches its target cell.
    /// </summary>
    public void MoveTile()
    {
        gameObject.transform.position += _moveDirectionVector3 * _moveSpeed * Time.deltaTime;

        if (IsTileBeyondTarget())
        {
            // tile was move too far -> set tile at position of "TargetCell"
            gameObject.transform.position = new Vector3(TargetCell.gameObject.transform.position.x,
                                                        gameObject.transform.position.y,
                                                        TargetCell.gameObject.transform.position.z);
            // tile finished its move to target cell
            CurrentCell = TargetCell;
            _doMove = false;
            CheckForMerge();
            GameManager.CountUnfinishedTileMoves -= 1;
        }
    }

    /// <summary>
    /// After a finished tile move, his method checks a merge of two tiles (including this one) shall be made.
    /// </summary>
    public void CheckForMerge()
    {
        if (TargetCell.Tile[0] != gameObject)
        {
            // this tile reached a cell with a similar tile -> merge

            // destroy one of merging tiles
            TargetCell.Tile.Remove(gameObject);
            Destroy(gameObject);
            // promote other merging tile to tile with higher value
            TargetCell.GetTileGameObject().GetComponent<Tile>().PromoteTile();
        }
    }

    /// <summary>
    /// The tile was merged and shall now represent the resulting tile of this merge. Thus this tile is updated.
    /// </summary>
    public void PromoteTile()
    {
        AssignTileValue(TileValue * 2);
        UpdateMaterial();
    }

    /// <param name="direction"></param>
    /// <returns>Returns a Vector3 that represents <c>direction</c>.</returns>
    public Vector3 DirectionToVector3(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return new Vector3(0, 0, -1);
            case Direction.Right:
                return new Vector3(-1, 0, 0);
            case Direction.Down:
                return new Vector3(0, 0, 1);
            default:  // case: Left
                return new Vector3(1, 0, 0);
        }
    }

    /// <returns>Returns <c>true</c> if shifted tile moved too far.</returns>
    public bool IsTileBeyondTarget()
    {
        switch (_moveDirection)
        {
            case Direction.Up:
                return gameObject.transform.position.z < TargetCell.gameObject.transform.position.z;
            case Direction.Right:
                return gameObject.transform.position.x < TargetCell.gameObject.transform.position.x;
            case Direction.Down:
                return gameObject.transform.position.z > TargetCell.gameObject.transform.position.z;
            default:  // case: Left
                return gameObject.transform.position.x > TargetCell.gameObject.transform.position.x;
        }
    }

}
