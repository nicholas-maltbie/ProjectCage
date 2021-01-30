using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panda : Animal
{
    protected override void Start() 
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            target = other.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        if(target == other.gameObject)
        {
            target = null;
        }
    }
}
