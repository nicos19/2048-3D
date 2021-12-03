using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SavegameManager : MonoBehaviour
{
    /// <summary>
    /// This method creates a savegame on disk that can be deserialized when savegame is loaded.
    /// </summary>
    public void CreateSavegameFile(int score, GameObject tilesGameObject, bool gameWon, string activeEndingScreen)
    {
        Savegame savegame = CreateSavegame(score, tilesGameObject, gameWon, activeEndingScreen);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savegame.save");
        bf.Serialize(file, savegame);
        file.Close();
    }

    /// <summary>
    /// This method creates a savegame object representing the game state to be saved.
    /// </summary>
    /// <returns></returns>
    public Savegame CreateSavegame(int score, GameObject tilesGameObject, bool gameWon, string activeEndingScreen)
    {
        return new Savegame(score, tilesGameObject, gameWon, activeEndingScreen);
    }

    /// <summary>
    /// This method loads (-> deserializes) a previous saved savegame file and returns it.
    /// </summary>
    /// <returns></returns>
    public Savegame LoadSavegame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/savegame.save", FileMode.Open);
        Savegame savegame = (Savegame)bf.Deserialize(file);
        file.Close();
        return savegame;
    }

    /// <returns>Returns <c>true</c> if a savegame file exists, <c>false</c> otherwise.</returns>
    public bool SavegameExists()
    {
        return File.Exists(Application.persistentDataPath + "/savegame.save");
    }

}
