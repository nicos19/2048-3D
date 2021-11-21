using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    public TextMesh ScoreValue;
    public TextMesh BestScoreValue;
    public GameObject SettingsMenu;
    public GameObject SettingsButton;

    private GameManager GAME_MANAGER;

    // Start is called before the first frame update
    void Start()
    {
        GAME_MANAGER = gameObject.GetComponent<GameManager>();
    }

    /// <summary>
    /// This method updates the scoreboard so that it displays the correct current score and the best score.
    /// </summary>
    /// <param name="score"></param>
    /// <param name="bestScore"></param>
    public void UpdateScoreboard(int score, int bestScore)
    {
        ScoreValue.text = score.ToString();
        BestScoreValue.text = bestScore.ToString();

        if (BestScoreValue.color == Color.white && score == bestScore && score != 0)
        {
            // just reached new high score
            BestScoreValue.color = Color.yellow;
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
        Debug.Log("GAME OVER");
    }

    /// <summary>
    /// This method quits the application.
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }

    /// <summary>
    /// This methods starts a new 2048 game.
    /// </summary>
    public void Restart()
    {

    }

    /// <summary>
    /// This method open or closes the settings menu.
    /// </summary>
    public void Settings()
    {
        if (SettingsMenu.activeSelf)
        {
            // close settings menu if it is open
            SettingsMenu.SetActive(false);
            GAME_MANAGER.GamePaused = false;
        } 
        else
        {
            // open settings menu if it is closed
            SettingsMenu.SetActive(true);
            GAME_MANAGER.GamePaused = true;
        }
    }

}
