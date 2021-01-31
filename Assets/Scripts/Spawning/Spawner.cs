using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public abstract class Spawner : NetworkBehaviour
{
    public GameObject spawnPrefab;
    
    public virtual GameObject SpawnObject(Vector2 position)
    {
        if(isServer)
        {
            var obj = Instantiate(spawnPrefab, position, Quaternion.identity);
            NetworkServer.Spawn(obj);
            return obj;
        }
        return null;
    }


}
