using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavegameTile
{
    public int CellX { get; private set; }
    public int CellY { get; private set; }
    public int TileValue { get; private set; }

    /// <summary>
    /// A <c>SavegameTile</c> instance represents a tile at cell <c>(cellX, cellY)</c> with value <c>tileValue</c>.
    /// </summary>
    /// <param name="cellX"></param>
    /// <param name="cellY"></param>
    /// <param name="tileValue"></param>
    public SavegameTile(int cellX, int cellY, int tileValue)
    {
        CellX = cellX;
        CellY = cellY;
        TileValue = tileValue;
    }
}
