
using UnityEngine;

namespace Scripts.Items
{
    public enum Item
    {
        None    = 0,
        Bamboo  = 10,
        Steak   = 20,
        Ham     = 30,
        Fish    = 40
    }

    public class ItemState : MonoBehaviour
    {
        public Item item;
    }
}