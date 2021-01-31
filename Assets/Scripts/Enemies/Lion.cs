using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Animals;
using Scripts.Items;
using Scripts.Character;

public class Lion : Animal
{
    public Scripts.Items.Item favoriteFood2;
    public override void Start()
    {
        species = AnimalSpecies.Lion;
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

    protected override void OnTriggerStay2D(Collider2D other)
    {
        var isPlayer = other.CompareTag("Player");
        var isMeat = other.GetComponent<ItemState>().item == favoriteFood || other.GetComponent<ItemState>().item == favoriteFood2;
        var isFoodItem = other.CompareTag("Food Item") && isMeat;
        if (isPlayer || isFoodItem)
        {
            target = other.gameObject;
            if (isPlayer)
            {
                scoreCredit = target;
            }
            else // is food item
            {
                scoreCredit = target.GetComponent<ItemState>().scoreCredit;
            }
        }
    }
}
