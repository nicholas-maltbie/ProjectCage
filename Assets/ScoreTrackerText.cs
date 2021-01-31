using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreTrackerText : MonoBehaviour
{
    public Text scoreText;
    public Text winnerDisplayText;

    public void UpdateScoreUI(int score)
    {
        scoreText.text = score.ToString();
    }

    public void DisplayWinner(string name)
    {
        winnerDisplayText.text = $"The Winner is {name}!";
    }
}
