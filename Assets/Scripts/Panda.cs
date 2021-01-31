using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Animals;

public class Panda : Animal
{
    protected override void Start()
    {
        species = AnimalSpecies.Panda;
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
    }
}
