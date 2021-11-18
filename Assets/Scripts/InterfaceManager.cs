using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    public TextMesh ScoreValue;
    public TextMesh BestScoreValue;

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
    }
}
