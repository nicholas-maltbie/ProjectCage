
using UnityEngine;

namespace Scripts.Items
{
    [RequireComponent(typeof(ItemState))]
    public class Pickupable : MonoBehaviour
    {
        public bool isBeingPickedUp;

        public void LateUpdate()
        {
            isBeingPickedUp = false;
        }
    }
}