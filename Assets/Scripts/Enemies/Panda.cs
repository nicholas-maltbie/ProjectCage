using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Animals;

public class Panda : Animal
{
    public override void Start()
    {
        species = AnimalSpecies.Panda;
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        other.gameObject.SendMessage("KillCharacter", SendMessageOptions.DontRequireReceiver);
    }
}
