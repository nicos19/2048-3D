using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cell : MonoBehaviour
{
    /// <value> 
    /// <c>X</c> represents the cell's x-coordinate. 
    /// </value>
    public int X;

    /// <value> 
    /// <c>Y</c> represents the cell's y-coordinate. 
    /// </value>
    public int Y;

    /// <value> 
    /// Property <c>Tile</c> represents the tile contained by the cell.
    /// Empty list means that cell does not contain any tile. 
    /// List of length two means that this two tiles will merge at this cell.
    /// </value>
    public List<GameObject> Tile { get; set; }

    /// <value>
    /// Property <c>Color</c> represents the background color of the cell (and shall depend on <c>Tile</c> property).
    /// </value>
    public Color Color { get; private set; }

    /// <value>
    /// Property <c>HasMergedTile</c> shall be true if cell contains a tile just merged in this round.
    /// </value>
    public bool HasMergedTile { get; private set; }

    private float rotationSpeed;
    private bool doRotate = true;
    private Vector3 newRotation;


    // Start is called before the first frame update
    void Start()
    {
        Color = Color.white;
        HasMergedTile = false;
        rotationSpeed = 200f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// This methods assigns <c>tileGameObject</c> to <c>Tile</c>.
    /// </summary>
    /// <param name="tileValue"></param>
    public void AssignTileToCell(GameObject tileGameObject)
    {
        Tile.Add(tileGameObject);
    }

    /// <summary>
    /// This method returns the tile game object in this cell.
    /// </summary>
    /// <returns></returns>
    public GameObject GetTileGameObject()
    {
        // returns always first object in "Tile" since "Tile" has more than one element only temporarly
        return Tile[0];
    }

    /// <summary>
    /// This method returns the value of the tile the cell contains.
    /// </summary>
    /// <returns></returns>
    public int GetTileValue()
    {
        return Tile[0].GetComponent<Tile>().TileValue;
    }

    /// <summary>
    /// This method updates the visual representation of the cell.
    /// </summary>
    public void DrawCell()
    {
        
    }

    /*
    /// <summary>
    /// This method sets property <c>Color</c> so that it fits with property <c>Tile</c>.
    /// </summary>
    public void UpdateColor()
    {
        switch (Tile[0].TileValue)
        {
            case 0:
                Color = Color.white;
                break;
            case 2:
                Color = new Color(11, 164, 82);
                break;
            case 4:
                Color = new Color(17, 70, 24);
                break;
            case 8:
                Color = new Color(18, 111, 19);
                break;
            case 16:
                Color = new Color(0, 255, 0);
                break;
            case 32:
                Color = new Color(36, 243, 38);
                break;
            case 64:
                Color = new Color(224, 241, 85);
                break;
            case 128:
                Color = new Color(255, 255, 0);
                break;
            case 256:
                Color = new Color(255, 116, 0);
                break;
            case 512:
                Color = new Color(224, 51, 0);
                break;
            case 1024:
                Color = new Color(99, 0, 0);
                break;
            case 2048:
                Color = new Color(255, 0, 0);
                break;
        }
    }*/

    /// <summary>
    /// This methods sets the property <c>HasMergedTile</c> to <c>true</c>. It is called after two tiles merged.
    /// </summary>
    public void TileMerged()
    {
        HasMergedTile = true;
    }

    /// <summary>
    /// This method resets the property <c>HasMergedTile</c> back to <c>false</c>.
    /// </summary>
    public void ResetHasMergedTile()
    {
        HasMergedTile = false;
    }
}
