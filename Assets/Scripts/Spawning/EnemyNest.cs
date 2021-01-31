using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EnemyNest : Spawner
{
    [SyncVar]
    public int maxEnemies;
    public float radius = 1;

    private void Start() 
    {
        if (isServer)
        {
            for (int i = 0; i < maxEnemies; i++)
            {
                NestSpawn();
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void NestSpawn()
    {
        var xVectorComponent = Random.value * 2 - 1;
        var yVectorComponent = Random.value * 2 - 1;
        var vectorMagnitude = (Random.value * 2 - 1) * radius; 
        Vector2 spawnLocation = (Vector2)transform.position + new Vector2(xVectorComponent, yVectorComponent)*vectorMagnitude;
        SpawnObject(spawnLocation);
        
    }

    public override void SpawnObject(Vector2 position)
    {
        if(isServer)
        {
            var obj = Instantiate(spawnPrefab, position, Quaternion.Euler(0,0,0));
            obj.GetComponent<Animal>().nest = this;
            obj.GetComponent<Animal>().Start(); // This prevents the navmesh from forcing weird angular turns.
            NetworkServer.Spawn(obj);
        }
    }
}
