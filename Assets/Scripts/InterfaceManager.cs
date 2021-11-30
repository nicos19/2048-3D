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
    public GameObject GameOverScreen;
    public GameObject GameOverScreenFinalScore;

    private bool _gameOverScreenDrawn;

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

        if (score == bestScore && score != 0)
        {
            // current score is also the high score
            ScoreValue.color = Color.yellow;
            // remember high score
            PlayerPrefs.SetInt("bestScore", bestScore);
        } 
        else
        {
            // current score is lower than the high score
            ScoreValue.color = Color.white;
        }
    }

    /// <summary>
    /// This method causes the game to show the screen that tells the player the he/she has just won the game.
    /// </summary>
    public void ShowWinScreen()
    {
        // TODO
    }

    /// <summary>
    /// This method draws a "Game over!" message including the <c>finalScore</c> of this try.
    /// </summary>
    public void ShowGameOverScreen(int finalScore, int bestScore)
    {
        GameOverScreenFinalScore.GetComponent<Text>().text = finalScore.ToString();
        if (finalScore == bestScore)
        {
            // this try occured in new high score
            GameOverScreenFinalScore.GetComponent<Text>().color = Color.yellow;
        }
        else
        {
            // no new high score
            GameOverScreenFinalScore.GetComponent<Text>().color = Color.white;
        }
        GameOverScreen.SetActive(true);
        _gameOverScreenDrawn = true;
    }

    /// <summary>
    /// This method does the same as <c>ShowGameOverScreen(int finalScore)</c> but is only used 
    /// when the game over screen of this try had already been drawn previously.
    /// </summary>
    public void ShowGameOverScreen()
    {
        GameOverScreen.SetActive(true);
    }

    /// <summary>
    /// This method removes the game over screen from the display.
    /// </summary>
    public void RemoveGameOverScreen()
    {
        GameOverScreen.SetActive(false);
    }

    /// <summary>
    /// This method quits the application.
    /// </summary>
    public void Quit()
    {
        GAME_MANAGER.Save();
        Application.Quit();
    }

    /// <summary>
    /// This methods starts a new 2048 game.
    /// </summary>
    public void Restart()
    {
        RemoveGameOverScreen();
        _gameOverScreenDrawn = false;
        GAME_MANAGER.StartNewGame();
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
            if (_gameOverScreenDrawn)
            {
                GAME_MANAGER.GamePaused = true;  // game is over
                ShowGameOverScreen();
            }
        } 
        else
        {
            // open settings menu if it is closed
            RemoveGameOverScreen();
            SettingsMenu.SetActive(true);
            GAME_MANAGER.GamePaused = true;
        }
    }

}
