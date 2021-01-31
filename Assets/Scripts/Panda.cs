using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Character;
using Scripts.Items;
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
    private void OnTriggerStay2D(Collider2D other)
    {

        var isPlayerHoldingFood = other.CompareTag("Player") && other.GetComponent<HoldObject>().heldItem == favoriteFood;
        var isFoodItem = other.CompareTag("Food Item") && other.GetComponent<ItemState>().item == favoriteFood;
        if (isPlayerHoldingFood || isFoodItem)
        {
            target = other.gameObject;
        }

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (target == other.gameObject)
        {
            target = null;
        }
    }
}
