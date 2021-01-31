using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ScoreManager : NetworkBehaviour
{
    public int maxScore = 5;
    public void AddScore(GameObject player, int points = 1)
    {
        if (isServer)
        {
            var targetPlayerScore = player.GetComponent<PlayerScore>();
            targetPlayerScore.score += points;
            if (targetPlayerScore.score >= maxScore)
            {
                print("A player has won!");
                StartCoroutine(InitializeGameReset());
            }
        }
    }

    public IEnumerator InitializeGameReset()
    {
        yield return new WaitForSeconds(3);
        RestartGame();
    }

    public void RestartGame()
    {
        var nm = FindObjectOfType<NetworkManager>();
        nm.ServerChangeScene("FirstZoo");
    }
}
