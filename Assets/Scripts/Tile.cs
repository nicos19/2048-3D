using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // ------------------------------
    // ONLY RELEVANT FOR TILES > 2048
    // ------------------------------
    public GameObject Value4Digits;
    public GameObject Value5Digits;
    public GameObject BaseValue;
    public GameObject ExponentValue;
    // ------------------------------


    public AudioManager AUDIO_MANAGER;  // is set by GameManager when tile is created

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
    private void Update()
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
    /// This method updates the material of the tile game object, so that it fits <c>_tileValue</c> 
    /// and adjust height/y-pos of tile game object.
    /// </summary>
    public void UpdateMaterial()
    {
        gameObject.GetComponent<MeshRenderer>().material = GetTileMaterial();
        // also update height and y-position of tile game object -> higher "TileValue" causes larger tile size
        float newHeight = (float)(0.1 + Mathf.Log(TileValue, 2) * 0.05);
        if (newHeight > 0.75f)
        {
            // max height is 0.75
            newHeight = 0.75f;
        }

        float newPositionY = (float)(transform.position.y + (newHeight - transform.localScale.y) * 0.5);
        transform.localScale = new Vector3(transform.localScale.x, newHeight, transform.localScale.z);
        transform.position = new Vector3(transform.position.x, newPositionY, transform.position.z);
    }

    /// <summary>
    /// This method returns the proper material for the tile.
    /// </summary>
    /// <returns></returns>
    public Material GetTileMaterial()
    {
        Material material;

        if (TileValue <= 2048)
        {
            // material including tile value texture available
            return Resources.Load<Material>($"Materials/TileMaterial{TileValue}");
        }
        else if (TileValue <= 65536)
        {
            // material available
            material = Resources.Load<Material>($"Materials/TileMaterial{TileValue}");
            // write "TileValue" on tile
            if (TileValue <= 9999)
            {
                gameObject.GetComponent<Tile>().Value4Digits.SetActive(true);
                gameObject.GetComponent<Tile>().Value4Digits.GetComponent<TextMesh>().text = TileValue.ToString();
            }
            else
            {
                gameObject.GetComponent<Tile>().Value4Digits.SetActive(false);
                gameObject.GetComponent<Tile>().Value5Digits.SetActive(true);
                gameObject.GetComponent<Tile>().Value5Digits.GetComponent<TextMesh>().text = TileValue.ToString();
            }
            return material;
        }
        else
        {
            // no new material available -> stay with current material ("TileMaterial65536")
            material = gameObject.GetComponent<MeshRenderer>().material;
            // write "TileValue" on Tile
            gameObject.GetComponent<Tile>().Value5Digits.SetActive(false);
            gameObject.GetComponent<Tile>().BaseValue.SetActive(true);
            gameObject.GetComponent<Tile>().ExponentValue.SetActive(true);
            gameObject.GetComponent<Tile>().ExponentValue.GetComponent<TextMesh>().text = Mathf.Log(TileValue, 2).ToString();
            return material;
        }
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
        AUDIO_MANAGER.PlayShiftSoundEffect();
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
            GameManager.CountUnfinishedTileMoves -= 1;
            CheckForMerge();
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

    /// <returns>Returns <c>true</c> if shifted tile moved too far or exactly at target position.</returns>
    public bool IsTileBeyondTarget()
    {
        switch (_moveDirection)
        {
            case Direction.Up:
                return gameObject.transform.position.z <= TargetCell.gameObject.transform.position.z;
            case Direction.Right:
                return gameObject.transform.position.x <= TargetCell.gameObject.transform.position.x;
            case Direction.Down:
                return gameObject.transform.position.z >= TargetCell.gameObject.transform.position.z;
            default:  // case: Left
                return gameObject.transform.position.x >= TargetCell.gameObject.transform.position.x;
        }
    }

}
