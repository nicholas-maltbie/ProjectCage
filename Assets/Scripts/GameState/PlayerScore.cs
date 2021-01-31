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
}
