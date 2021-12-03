using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Savegame
{
    public int Score { get; private set; }
    public List<SavegameTile> SavegameTiles { get; private set; }
    public bool GameWon { get; private set; }
    public string ActiveEndingScreen { get; private set; }


    /// <summary>
    /// A <c>Savegame</c> instance represents the game state to be saved.
    /// </summary>
    /// <param name="score"></param>
    /// <param name="tilesGameObject"></param>
    public Savegame(int score, GameObject tilesGameObject, bool gameWon, string activeEndingScreen)
    {
        Score = score;
        SavegameTiles = new List<SavegameTile>();

        List<Tile> tiles = GetTileList(tilesGameObject);
        
        // save position and value of all tiles
        foreach (Tile tile in tiles)
        {
            SavegameTiles.Add(new SavegameTile(tile.CurrentCell.X, tile.CurrentCell.Y, tile.TileValue));
        }

        GameWon = gameWon;
        ActiveEndingScreen = activeEndingScreen;
    }

    /// <summary>
    /// This methods returns a list of <c>Tile</c> instances, one for each tile game object.
    /// </summary>
    /// <param name="tilesGameObject"></param>
    /// <returns></returns>
    public List<Tile> GetTileList(GameObject tilesGameObject)
    {
        List<Tile> tiles = new List<Tile>();
        foreach (Transform tile in tilesGameObject.transform)
        {
            tiles.Add(tile.gameObject.GetComponent<Tile>());
        }
        return tiles;
    }
}
