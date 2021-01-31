using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerScore : NetworkBehaviour
{
    [SyncVar]
    string playerName;
    [SyncVar]
    public int score;

    public ScoreTrackerText stt;

    private void Start() 
    {
        if(isClient)
        {
            stt = FindObjectOfType<ScoreTrackerText>();
        }
    }
    
    private void FixedUpdate() 
    {
        if(isLocalPlayer)
        {
            stt.UpdateScoreUI(score);
        }
    }
    
    public void RpcDisplayWinnerUI(string player)
    {
        stt.DisplayWinner(player);
    }
}
