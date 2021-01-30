using UnityEngine;
using Mirror;
using Scripts.Items;

namespace Scripts.Character
{
    [RequireComponent(typeof(HoldObject))]
    public class ItemPickup : NetworkBehaviour
    {
        [Command]
        public void CmdPickupItem(GameObject worldItem)
        {
            HoldObject holder = GetComponent<HoldObject>();

            // only let the player hold item
            if (holder.heldItem != Item.None)
            {
                return;
            }

            // set the pickup state of the item, no duplicating items
            Pickupable pickup = worldItem.GetComponent<Pickupable>();
            if (pickup.isBeingPickedUp)
            {
                return;
            }
            pickup.isBeingPickedUp = true;
            
            // Destory the current item
            NetworkServer.Destroy(worldItem);

            // Get the item type of the thing being picked up
            Item item = worldItem.GetComponent<ItemState>().item;
            // Set the player's held item
            holder.heldItem = item;
        }

        public void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.GetComponent<Pickupable>() != null)
            {
                CmdPickupItem(other.gameObject);
            }
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<Pickupable>() != null)
            {
                CmdPickupItem(other.gameObject);
            }
            else if (other.gameObject.GetComponent<PickupableRadius>() != null)
            {
                CmdPickupItem(other.GetComponent<PickupableRadius>().pickupReference.gameObject);
            }
        }
    }
}