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
        if(isLocalPlayer)
        {
            stt = FindObjectOfType<ScoreTrackerText>();
        }
    }

    public void ModifyScore(int change)
    {
        if(isLocalPlayer)
        {
            stt.UpdateScoreUI(score);
        }
    }
}
