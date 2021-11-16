using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDrawer : MonoBehaviour
{
    public GameObject PlayFieldGameObject;

    private GameObject[,] _cellGameObjects;

    /// <summary>
    /// This method sets up the scene drawer so that it can draw the play field.
    /// </summary>
    public void InitializeSceneDrawer(int fieldSize)
    {
        List<GameObject> allCells = new List<GameObject>();
        foreach (Transform cell in PlayFieldGameObject.transform)
        {
            allCells.Add(cell.gameObject);
        }

        int i = 0;  // counts through "allCells"
        for (int x = 0; x < fieldSize; x++)
        {
            for (int y = 0; y < fieldSize; y++)
            {
                _cellGameObjects[y, x] = allCells[i];
                i++;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void UpdateSceneDrawer()
    {

    }



    /// <summary>
    /// This methods updates the visual representation of the play field.
    /// </summary>    
    public void DrawScene(Cell [,] playField, int fieldSize)
    {
        for (int x = 0; x < fieldSize; x++)
        {
            for (int y = 0; y < fieldSize; y++)
            {
                playField[y, x].DrawCell();
            }
        }
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
