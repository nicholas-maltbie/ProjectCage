using UnityEngine;
using Mirror;
using Scripts.Items;

namespace Scripts.Character
{
    [RequireComponent(typeof(HoldObject))]
    public class ItemPickup : NetworkBehaviour
    {
        public float pickupResetTime = 0.1f;

        private float pickupCooldown = 0.0f;

        public ObjectPickupRadius focusedPickup;

        public bool DoneWithCooldown()
        {
            return pickupCooldown <= 0;
        }

        public void StartCooldown()
        {
            pickupCooldown = pickupResetTime;
        }

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

            // Make sure we're on cooldown
            if (!this.DoneWithCooldown())
            {
                return;
            }

            pickup.isBeingPickedUp = true;

            // Destory the current item
            NetworkServer.Destroy(worldItem);

            StartCooldown();

            // Get the item type of the thing being picked up
            Item item = worldItem.GetComponent<ItemState>().item;
            // Set the player's held item
            holder.heldItem = item;
        }

        public void Update()
        {
            if (pickupCooldown > 0)
            {
                pickupCooldown -= Time.deltaTime;
            }
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            if (isLocalPlayer)
            {
                var playerPickup = other.gameObject.GetComponent<ObjectPickupRadius>();
                if (playerPickup != null && playerPickup == focusedPickup)
                {
                    focusedPickup = null;
                }
            }
        }

        private void TriggerEnterOrStay(Collider2D other)
        {
            var identity = other.GetComponent<NetworkIdentity>();
            if (isLocalPlayer && GetComponent<DeathActions>().IsAlive)
            {
                if (identity != null && identity.isActiveAndEnabled && GetComponent<HoldObject>().heldItem == Item.None)
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
                var focusPickup = other.gameObject.GetComponent<ObjectPickupRadius>();
                if (other.gameObject.GetComponent<ObjectPickupRadius>() != null)
                {
                    focusedPickup = focusPickup;
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