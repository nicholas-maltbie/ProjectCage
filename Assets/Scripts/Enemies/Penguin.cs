﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Animals;

public class Penguin : Animal
{
    public override void Start()
    {
        species = AnimalSpecies.Penguin;
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
    }
}
