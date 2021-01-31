
using UnityEngine;

namespace Scripts.Items
{
    public enum Item
    {

        None = 0,
        Bamboo = 10,
        Steak = 20,
        Ham = 30,
        Fish = 40,
        Player = 100
    }

    public class ItemState : MonoBehaviour
    {
        public static bool IsThrowableItem(Item item)
        {
            return item > Item.None && item < Item.Player;
        }

        public Item item;
        public GameObject scoreCredit;
    }
}