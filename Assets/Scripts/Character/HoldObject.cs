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

        public float itemThrowSpeed = 8.0f;

        public float playerThrowSpeed = 12.0f;

        public float thrownCooldown = 0.1f;

        [SyncVar(hook = nameof(OnChangeEquipment))]
        public Item heldItem;

        [SyncVar]
        public GameObject heldPlayer;

        public void Start()
        {
            playerRigidbody = GetComponent<Rigidbody2D>();
        }

        public void OnChangeEquipment(Item oldItem, Item newItem)
        {
            StartCoroutine(ChangeItem(newItem));
        }

        public Vector2 GetThrowDirection()
        {
            Vector2 throwDir = GetComponent<CharacterMovement>().lastMovement;
            if (playerRigidbody.velocity.magnitude > 0)
            {
                throwDir = playerRigidbody.velocity.normalized;
            }

            return throwDir;
        }

        public Vector2 GetThrowPosition(Vector2 throwDir)
        {
            Vector2 targetPosition = transform.position + new Vector3(throwDir.x, throwDir.y);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, throwDir, throwDir.magnitude, ~(1 << LayerMask.NameToLayer("Player")));

            if (hit)
            {
                targetPosition = transform.position;
            }

            return targetPosition;
        }

        public Vector2 GetThrownVelocity(Vector2 throwDir, float speedMultiplier)
        {
            return throwDir * speedMultiplier + playerRigidbody.velocity;
        }

        [Command]
        public void CmdYeetItem(Item yeet)
        {
            GameObject yeetedPrefab = worldItemLibrary.GetItem(yeet);
            Vector2 throwDir = GetThrowDirection();
            Vector2 targetPosition = GetThrowPosition(throwDir);
            GameObject spawned = Instantiate(yeetedPrefab, targetPosition, Quaternion.identity);
            Pickupable pickup = spawned.GetComponent<Pickupable>();

            pickup.pickupCooldown = thrownCooldown;
            NetworkServer.Spawn(spawned);
            Rigidbody2D thrown = spawned.GetComponent<Rigidbody2D>();
            if (thrown != null)
            {
                thrown.velocity = GetThrownVelocity(throwDir, itemThrowSpeed);
            }
        }

        public IEnumerator ChangeItem(Item item)
        {
            while (holdingTransform.childCount > 0)
            {
                Destroy(holdingTransform.GetChild(0).gameObject);
                yield return null;
            }

            if (ItemState.IsThrowableItem(item))
            {
                Instantiate(heldItemLibrary.GetItem(item), holdingTransform);
            }
            if (item == Item.Player)
            {
                GameObject heldItem = Instantiate(heldItemLibrary.GetItem(item), holdingTransform);
                // Link the animating renderer and the normal sprite controller
                heldItem.GetComponent<HeldCharacterSkin>().linkedCharacter = heldPlayer.GetComponent<CharacterSkin>();
            }
        }

        [Command]
        public void CmdSetHeldItem(Item selectedItem)
        {
            this.heldItem = selectedItem;
        }

        [Command]
        public void CmdYeetPlayer(GameObject otherPlayer)
        {
            CharacterMovement otherMovement = otherPlayer.GetComponent<CharacterMovement>();
            // Yeet the other player, set their state to thrown
            otherMovement.thrownCooldown = 3.0f;
            otherMovement.heldState = CharacterHeld.Thrown;
            // Reset their held information
            otherMovement.holder = null;
            // Set our held item as none
            heldItem = Item.None;
            // Reset held player
            heldPlayer = null;
            // Get the new position and velocity of thrown player
            Vector2 throwDir = GetThrowDirection();
            Vector2 targetPosition = GetThrowPosition(throwDir);

            otherPlayer.transform.position = targetPosition;

            Rigidbody2D thrown = otherPlayer.GetComponent<Rigidbody2D>();
            if (thrown != null)
            {
                thrown.velocity = GetThrownVelocity(throwDir, playerThrowSpeed);
            }
        }

        [Command]
        public void CmdPickupAnotherPlayer(GameObject otherPlayer)
        {
            // Make sure player isn't holding anything
            if (heldItem != Item.None)
            {
                return;
            }
            CharacterMovement otherMovement = otherPlayer.GetComponent<CharacterMovement>();
            // Only pickup other player if they are not already held
            if (otherMovement.heldState != CharacterHeld.Normal)
            {
                return;
            }
            CharacterMovement currentMovement = GetComponent<CharacterMovement>();;
            // Can only pickup player if we are also not held
            if (currentMovement.heldState != CharacterHeld.Normal)
            {
                return;
            }
            // Set the held state of the other characer to held
            otherMovement.heldState = CharacterHeld.Held;
            // Set their held position as our holding position
            otherMovement.holder = holdingTransform;
            // Save that we are holding that player
            heldPlayer = otherPlayer;
            heldItem = Item.Player;
            // Tell the other player we are carrying them
            otherMovement.holdingCharacterController = currentMovement;
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
                    CmdSetHeldItem(Item.None);
                }
                if (this.heldItem == Item.Player)
                {
                    CmdYeetPlayer(this.heldPlayer);
                }
            }
        }
    }
}