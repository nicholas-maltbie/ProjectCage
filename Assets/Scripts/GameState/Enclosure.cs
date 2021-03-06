﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Scripts.Animals;

public class Enclosure : NetworkBehaviour
{
    // Requires Rigidbody2D, and a collider with isTrigger enabled.
    public AnimalSpecies enclosureSpecies;
    [SyncVar]
    public int numberOfSpecies;
    [SyncVar]
    public int numberCaptured = 0;

    private EnclosureManager enclosureManager;
    private void Start()
    {
        enclosureManager = FindObjectOfType<EnclosureManager>();
    }

    private bool IsCorrectTarget(Collider2D other)
    {
        bool isCorrectAnimal = other.GetComponent<Animal>() && other.GetComponent<Animal>().species == enclosureSpecies;
        return isCorrectAnimal && !other.isTrigger;
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (isServer && IsCorrectTarget(other))
        {
            other.GetComponent<Animal>().Consume();
        }
    }
}
