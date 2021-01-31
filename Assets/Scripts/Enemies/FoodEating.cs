using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using Mirror;
using Scripts.Character;
using Scripts.Animals;
using Scripts.Items;
using UnityEngine.SceneManagement;

public class FoodEating : NetworkBehaviour
{
    public List<Item> foodEat = new List<Item>();

    public float eatTime = 1.5f;

    public float eatElapsed = 0.0f;

    public GameObject eat;

    public void Update()
    {
        if (isServer && eat != null)
        {
            eatElapsed += Time.deltaTime;
            if (eatElapsed >= eatTime)
            {
                NetworkServer.Destroy(eat);
                eat = null;
            }
        }
    }
}
