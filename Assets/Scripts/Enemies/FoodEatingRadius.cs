
using Scripts.Items;
using UnityEngine;

public class FoodEatingRadius : MonoBehaviour
{
    public FoodEating foodEating;

    public void OnTriggerEnter2D(Collider2D other)
    {
        ItemState state = other.gameObject.GetComponent<ItemState>();
        if (state != null && foodEating.foodEat.Contains(state.item))
        {
            foodEating.eat = state.gameObject;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == foodEating.eat)
        {
            foodEating.eat = null;
            foodEating.eatElapsed = 0.0f;
        }
    }
}