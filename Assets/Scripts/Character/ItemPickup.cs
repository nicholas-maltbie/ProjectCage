using UnityEngine;
using Mirror;
using Scripts.Items;

namespace Scripts.Character
{
    [RequireComponent(typeof(HoldObject))]
    public class ItemPickup : NetworkBehaviour
    {
        public GameObject focusedPlayer;

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
            if (!pickup.CanPickup(this.netId))
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
            if (isLocalPlayer)
            {
                if (other.gameObject.GetComponent<Pickupable>() != null)
                {
                    CmdPickupItem(other.gameObject);
                }
                // if (other.gameObject.GetComponent<CharacterMovement>() != null && GetComponent<CharacterMovement>().heldState == CharacterHeld.Normal)
                // {
                //     GetComponent<HoldObject>().CmdPickupAnotherPlayer(other.gameObject);
                // }
            }
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            if (isLocalPlayer)
            {
                var playerPickup = other.gameObject.GetComponent<PlayerPickupRadius>();
                if (playerPickup != null && playerPickup.pickupReference == focusedPlayer)
                {
                    focusedPlayer = null;
                }
            }
        }

        private void TriggerEnterOrStay(Collider2D other)
        {
            var identity = other.GetComponent<NetworkIdentity>();
            if (isLocalPlayer && identity != null && identity.isActiveAndEnabled)
            {
                var playerPickup = other.gameObject.GetComponent<PlayerPickupRadius>();
                if (other.gameObject.GetComponent<Pickupable>() != null)
                {
                    CmdPickupItem(other.gameObject);
                }
                else if (other.gameObject.GetComponent<PickupableRadius>() != null)
                {
                    CmdPickupItem(other.GetComponent<PickupableRadius>().pickupReference.gameObject);
                }
                else if (other.gameObject.GetComponent<PlayerPickupRadius>() != null)
                {
                    focusedPlayer = playerPickup.pickupReference;
                }
            }
        }

        public void OnTriggerStay2D(Collider2D other)
        {
            TriggerEnterOrStay(other);
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            TriggerEnterOrStay(other);
        }
    }
}