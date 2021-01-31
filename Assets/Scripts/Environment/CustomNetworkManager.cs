

using Mirror;
using Scripts.Character;
using UnityEngine;

namespace Scripts.Environment
{
    public class CustomNetworkManager : NetworkManager
    {
        public override void OnServerDisconnect(NetworkConnection conn)
        {
            foreach (HoldObject hold in GameObject.FindObjectsOfType<HoldObject>())
            {
                NetworkIdentity identity = hold.gameObject.GetComponent<NetworkIdentity>();
                if (identity.netId == conn.identity.netId)
                {
                    hold.DropThings();
                    // If held by someone else, make them drop you
                    CharacterMovement movement = identity.GetComponent<CharacterMovement>();
                    if (movement.heldState == CharacterHeld.Held)
                    {
                        // Make set held item to nothing
                        movement.holdingCharacterController.GetComponent<HoldObject>().heldItem = Items.Item.None;
                    }
                }
            }
            base.OnServerDisconnect(conn);
        }
    }
}