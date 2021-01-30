using Mirror;
using UnityEngine;
using Scripts.Items;
using System.Collections;

namespace Scripts.Character
{
    public class HoldObject : NetworkBehaviour
    {
        public ItemLibrary worldItemLibrary;

        public ItemLibrary heldItemLibrary;

        public Transform holdingTransform;

        public Rigidbody2D playerRigidbody;

        public float throwSpeed = 5.0f;

        public float thrownCooldown = 0.1f;

        [SyncVar(hook = nameof(OnChangeEquipment))]
        public Item heldItem;

        public void OnChangeEquipment(Item oldItem, Item newItem)
        {
            StartCoroutine(ChangeItem(newItem));
        }

        [Command]
        public void CmdYeetItem(Item yeet)
        {
            GameObject yeetedPrefab = worldItemLibrary.GetItem(yeet);
            GameObject spawned = Instantiate(yeetedPrefab);
            Pickupable pickup = spawned.GetComponent<Pickupable>();
            pickup.pickupCooldown = thrownCooldown;
            NetworkServer.Spawn(spawned);
            Rigidbody2D thrown = spawned.GetComponent<Rigidbody2D>();
            Vector2 throwDir = GetComponent<CharacterMovement>().lastMovement;
            if (thrown != null)
            {
                if (playerRigidbody.velocity.magnitude > 0)
                {
                    throwDir = playerRigidbody.velocity.normalized;
                }

                thrown.velocity = throwDir * throwSpeed + playerRigidbody.velocity;
            }
            spawned.transform.position = transform.position + new Vector3(throwDir.x, throwDir.y);
        }

        public IEnumerator ChangeItem(Item item)
        {
            while (holdingTransform.childCount > 0)
            {
                Destroy(holdingTransform.GetChild(0).gameObject);
                yield return null;
            }

            if (item != Item.None && item != Item.Player)
            {
                Instantiate(heldItemLibrary.GetItem(item), holdingTransform);
            }
        }

        [Command]
        public void CmdSetHeldItem(Item selectedItem)
        {
            this.heldItem = selectedItem;
        }

        public void Update()
        {
            if (!isLocalPlayer)
            {
                return;
            }

            if (Input.GetButtonDown("Drop") && heldItem != Item.None)
            {
                if (ItemState.IsThrowableItem(this.heldItem))
                {
                    CmdYeetItem(this.heldItem);
                }
                CmdSetHeldItem(Item.None);
            }
        }
    }
}