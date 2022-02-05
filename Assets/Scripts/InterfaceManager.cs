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
    public GameObject WinScreen;
    public GameObject WinScreenFinalScore;

    private bool _gameOverScreenDrawn;
    private bool _winScreenDrawn;

    private GameManager GAME_MANAGER;
    private AudioManager AUDIO_MANAGER;

    // Start is called before the first frame update
    void Start()
    {
        GAME_MANAGER = gameObject.GetComponent<GameManager>();
        AUDIO_MANAGER = gameObject.GetComponent<AudioManager>();
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
    public void ShowWinScreen(int finalScore, int bestScore)
    {
        DrawFinalScore(WinScreenFinalScore, finalScore, bestScore);
        WinScreen.SetActive(true);
        _winScreenDrawn = true;
    }

    /// <summary>
    /// This method does the same as <c>ShowWinScreen(int finalScore, int bestScore)</c> but is only used 
    /// when the win screen of this try had already been drawn previously.
    /// </summary>
    public void ShowWinScreen()
    {
        WinScreen.SetActive(true);
    }

    /// <summary>
    /// This method removes the win screen from the display.
    /// </summary>
    public void RemoveWinScreen()
    {
        WinScreen.SetActive(false);
    }

    /// <summary>
    /// This method draws a "Game over!" message including the <c>finalScore</c> of this try.
    /// </summary>
    public void ShowGameOverScreen(int finalScore, int bestScore)
    {
        DrawFinalScore(GameOverScreenFinalScore, finalScore, bestScore);
        GameOverScreen.SetActive(true);
        _gameOverScreenDrawn = true;
    }

    /// <summary>
    /// This method does the same as <c>ShowGameOverScreen(int finalScore, int bestScore)</c> but is only used 
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
    /// This method assigns the <c>finalScore</c> to the <c>finalScoreObject</c> and determines
    /// the correct color for the displayed score.
    /// </summary>
    /// <param name="finalScoreObject"></param>
    /// <param name="finalScore"></param>
    /// <param name="bestScore"></param>
    public void DrawFinalScore(GameObject finalScoreObject, int finalScore, int bestScore)
    {
        finalScoreObject.GetComponent<Text>().text = finalScore.ToString();
        if (finalScore == bestScore)
        {
            // this try occured in new high score
            finalScoreObject.GetComponent<Text>().color = Color.yellow;
        }
        else
        {
            // no new high score
            finalScoreObject.GetComponent<Text>().color = Color.white;
        }
    }

    /// <returns>
    /// Returns <c>"gameover"</c> if game over screen is shown, <c>"win"</c> if win screen is shown and <c>""</c> otherwise.
    /// </returns>
    public string ActiveEndingScreen()
    {
        if (_gameOverScreenDrawn)
        {
            // game over screen is shown
            return "gameover";
        }
        else if (_winScreenDrawn)
        {
            // win screen is shown
            return "win";
        }
        else
        {
            return "";
        }
    }

    /// <summary>
    /// This method quits the application.
    /// </summary>
    public void Quit()
    {
        if (!ButtonClickable())
        {
            return;
        }

        AUDIO_MANAGER.PlayButtonSoundEffect();
        Application.Quit();
        // game is saved before quitting by OnApplicationQuit() in GameManager
    }

    /// <summary>
    /// This methods starts a new 2048 game.
    /// </summary>
    public void Restart()
    {
        if (!ButtonClickable())
        {
            return;
        }

        RemoveGameOverScreen();
        RemoveWinScreen();
        _gameOverScreenDrawn = false;
        _winScreenDrawn = false;
        GAME_MANAGER.StartNewGame();

        AUDIO_MANAGER.PlayButtonSoundEffect();
    }

    /// <summary>
    /// This method continues a game that is already won.
    /// </summary>
    public void Continue()
    {
        RemoveWinScreen();
        _winScreenDrawn = false;
        GAME_MANAGER.GamePaused = false;
        GAME_MANAGER.InitializeNextRound();

        AUDIO_MANAGER.PlayButtonSoundEffect();
    }

    /// <summary>
    /// This method open or closes the settings menu.
    /// </summary>
    public void Settings()
    {
        if (!ButtonClickable())
        {
            return;
        }

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
            else if (_winScreenDrawn)
            {
                GAME_MANAGER.GamePaused = true;  // game is won
                ShowWinScreen();
            }
        } 
        else
        {
            // open settings menu if it is closed
            RemoveGameOverScreen();
            RemoveWinScreen();
            SettingsMenu.SetActive(true);
            GAME_MANAGER.GamePaused = true;
        }

        AUDIO_MANAGER.PlayButtonSoundEffect();
    }

    /// <returns>
    /// Returns <c>true</c> if player is allowed to use buttons, <c>false</c> otherwise.
    /// </returns>
    public bool ButtonClickable()
    {
        // quit-, qestart- and settings-buttons shall only be clickable if there are no moving tiles
        return GAME_MANAGER.ReadyForUserInput || _winScreenDrawn;
    }

}
