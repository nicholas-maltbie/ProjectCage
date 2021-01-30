using Mirror;
using UnityEngine;

namespace Scripts.Items
{
    [RequireComponent(typeof(ItemState))]
    public class Pickupable : MonoBehaviour
    {
        public uint lastToPickup;

        private float pickupCooldown;

        public bool isBeingPickedUp;

        public void SetCooldown(float cooldown, uint clientId)
        {
            this.lastToPickup = clientId;
            this.pickupCooldown = cooldown;
            Collider2D collider = GetComponent<Collider2D>();
            // if (collider != null)
            // {
            //     collider.enabled = false;
            // }
        }

        public bool CanPickup(uint clientId)
        {
            return !isBeingPickedUp && (clientId != lastToPickup || pickupCooldown <= 0);
        }

        public void LateUpdate()
        {
            if (pickupCooldown > 0)
            {
                pickupCooldown -= Time.deltaTime;
                Collider2D collider = GetComponent<Collider2D>();
                // if (collider != null)
                // {
                //     collider.enabled = false;
                // }
            }
            if (pickupCooldown <= 0)
            {
                pickupCooldown = 0;
                Collider2D collider = GetComponent<Collider2D>();
                // if (collider != null)
                // {
                //     collider.enabled = true;
                // }
            }
            isBeingPickedUp = false;
        }
    }
}