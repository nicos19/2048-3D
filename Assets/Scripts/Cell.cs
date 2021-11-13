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
    /// Empty cell is represented by <c>0</c>, actual tiles by <c>2</c>, <c>4</c>, <c>8</c>, ... or <c>2048</c>.
    /// </value>
    public int Tile { get; private set; }

    /// <value>
    /// Property <c>Color</c> represents the background color of the cell (and shall depend on <c>Tile</c> property).
    /// </value>
    public Color Color { get; private set; }

    /// <value>
    /// Property <c>HasMergedTile</c> shall be true if cell contains a tile just merged in this round.
    /// </value>
    public bool HasMergedTile { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        Color = Color.white;
        HasMergedTile = false;
    }

    /// <summary>
    /// This methods sets the <c>Tile</c> property of the cell. 
    /// Therefore a tile is assigned to the cell or the cell is marked as empty.
    /// </summary>
    /// <param name="tileValue"></param>
    public void AssignTileToCell(int tileValue)
    {
        Tile = tileValue;
    }

    /// <summary>
    /// This method updates the visual representation of the cell.
    /// </summary>
    public void DrawCell()
    {
        // TODO

        /*
        if (Tile == 0)
        {
            // cell is empty
            game.setCellValueEx(X, Y, Color, "");
            return;
        }

        Integer tileInteger = new Integer(Tile);
        game.setCellValueEx(X, Y, Color, tileInteger.toString(), UnityEngine.Color.BLACK);
        */
    }

    /// <summary>
    /// This method sets property <c>Color</c> so that it fits with property <c>Tile</c>.
    /// </summary>
    public void UpdateColor()
    {
        switch (Tile)
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
    }

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
