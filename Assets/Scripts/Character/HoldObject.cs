using Mirror;
using UnityEngine;
using Scripts.Items;
using System.Collections;
using Scripts.Environment;

namespace Scripts.Character
{
    public class HoldObject : NetworkBehaviour
    {
        public SFXPlayer soundEffectsPlayer;

        public ItemLibrary worldItemLibrary;

        public ItemLibrary heldItemLibrary;

        public Transform holdingTransform;

        public Rigidbody2D playerRigidbody;

        public float itemThrowSpeed = 8.0f;

        public float playerThrowSpeed = 12.0f;

        public float itemPickupCooldown = 3.0f;

        [SyncVar(hook = nameof(OnChangeEquipment))]
        public Item heldItem;

        [SyncVar]
        public uint heldPlayerId;

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

        public void DropThings()
        {
            if (ItemState.IsThrowableItem(this.heldItem))
            {
                YeetItem(this.heldItem, this.gameObject);
            }
            if (this.heldItem == Item.Player)
            {
                YeetPlayer(this.heldPlayer);
            }
        }

        [Command]
        public void CmdDropThings()
        {
            DropThings();
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

        public Vector2 GetThrowPosition(Vector2 throwDir, float distMod = 1.0f)
        {
            Vector2 targetPosition = transform.position + new Vector3(throwDir.x, throwDir.y);
            Physics2D.queriesStartInColliders = false;
            Physics2D.queriesHitTriggers = false;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, throwDir, throwDir.magnitude, ~(1 << LayerMask.NameToLayer("Player")));
            return hit ? hit.point : targetPosition;
        }

        public Vector2 GetThrownVelocity(Vector2 throwDir, float speedMultiplier)
        {
            return throwDir * speedMultiplier + playerRigidbody.velocity;
        }

        [ClientRpc]
        public void RpcYeetSound()
        {
            soundEffectsPlayer.PlayYeetSound();
        }
        public void YeetItem(Item yeet, GameObject scoreCredit)
        {
            RpcYeetSound();
            GameObject yeetedPrefab = worldItemLibrary.GetItem(yeet);
            this.heldItem = Item.None;
            Vector2 throwDir = GetThrowDirection();
            Vector2 targetPosition = GetThrowPosition(throwDir);
            GameObject spawned = Instantiate(yeetedPrefab, targetPosition, Quaternion.identity);
            Pickupable pickup = spawned.GetComponent<Pickupable>();

            // include credit to player who threw object
            spawned.GetComponent<ItemState>().scoreCredit = scoreCredit;

            pickup.SetCooldown(itemPickupCooldown, this.netId);
            NetworkServer.Spawn(spawned);
            Rigidbody2D thrown = spawned.GetComponent<Rigidbody2D>();
            if (thrown != null)
            {
                thrown.velocity = GetThrownVelocity(throwDir, itemThrowSpeed);
            }
        }

        [Command]
        public void CmdYeetItem(Item yeet)
        {
            YeetItem(yeet, this.gameObject);
        }

        public IEnumerator ChangeItem(Item item)
        {
            while (holdingTransform.childCount > 0)
            {
                Destroy(holdingTransform.GetChild(0).gameObject);
                yield return null;
            }

            bool beingPickedUp = item == Item.Player && heldPlayerId == NetworkClient.connection.identity.netId;
            bool shouldPlaySFX = beingPickedUp || isLocalPlayer;
            // Item sound effect
            if (shouldPlaySFX && item != Item.None)
            {
                soundEffectsPlayer.PickupSound();
            }

            if (ItemState.IsThrowableItem(item))
            {
                Instantiate(heldItemLibrary.GetItem(item), holdingTransform);
            }
            if (item == Item.Player && heldPlayerId != NetworkClient.connection.identity.netId)
            {
                GameObject heldItem = Instantiate(heldItemLibrary.GetItem(item), holdingTransform);
                // Link the animating renderer and the normal sprite controller
                if (heldPlayer != null)
                {
                    heldItem.GetComponent<HeldCharacterSkin>().linkedCharacter = heldPlayer.GetComponent<CharacterSkin>();
                }
            }
        }

        [Command]
        public void CmdSetHeldItem(Item selectedItem)
        {
            this.heldItem = selectedItem;
        }

        public void YeetPlayer(GameObject otherPlayer)
        {
            RpcYeetSound();
            CharacterMovement otherMovement = otherPlayer.GetComponent<CharacterMovement>();
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

            otherPlayer.GetComponent<CharacterMovement>().YeetPlayer(GetThrownVelocity(throwDir, playerThrowSpeed), targetPosition);

            if (!otherPlayer.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                otherPlayer.GetComponent<CharacterMovement>().RpcYeetPlayer(GetThrownVelocity(throwDir, playerThrowSpeed), targetPosition);
            }
        }

        [Command]
        public void CmdYeetPlayer(GameObject otherPlayer)
        {
            YeetPlayer(otherPlayer);
        }

        public IEnumerator PickupAnotherPlayerCoroutine()
        {
            CharacterMovement currentMovement = GetComponent<CharacterMovement>();
            CharacterMovement otherMovement = heldPlayer.GetComponent<CharacterMovement>();
            // Set the held state of the other characer to held
            otherMovement.heldState = CharacterHeld.Held;
            // Set their held position as our holding position
            otherMovement.holder = gameObject;
            // Tell the other player we are carrying them
            otherMovement.holdingCharacterController = currentMovement;
            yield return null;
            // Update held item state
            heldItem = Item.Player;
        }

        [Command]
        public void CmdPickupAnotherPlayer(GameObject otherPlayer)
        {
            CharacterMovement otherMovement = otherPlayer.GetComponent<CharacterMovement>();
            CharacterMovement currentMovement = GetComponent<CharacterMovement>();
            // Make sure player isn't holding anything
            if (heldItem != Item.None)
            {
                return;
            }
            // Only pickup other player if they are not already held
            if (otherMovement.heldState != CharacterHeld.Normal)
            {
                return;
            }
            // make sure cooldown is done
            if (!GetComponent<ItemPickup>().DoneWithCooldown())
            {
                return;
            }
            // Can only pickup player if we are also not held
            if (currentMovement.heldState != CharacterHeld.Normal)
            {
                return;
            }
            // Save that we are holding that player
            heldPlayer = otherPlayer;
            // Save the held playerId
            heldPlayerId = otherPlayer.GetComponent<NetworkIdentity>().netId;
            StartCoroutine(PickupAnotherPlayerCoroutine());
        }

        public void Update()
        {
            if (!isLocalPlayer)
            {
                return;
            }
            if (!GetComponent<DeathActions>().IsAlive)
            {
                return;
            }

            ItemPickup itemPickup = GetComponent<ItemPickup>();

            if (Input.GetButtonDown("Drop") && heldItem != Item.None)
            {
                GetComponent<ItemPickup>().StartCooldown();
                if (ItemState.IsThrowableItem(this.heldItem))
                {
                    CmdYeetItem(this.heldItem);
                }
                if (this.heldItem == Item.Player)
                {
                    CmdYeetPlayer(this.heldPlayer);
                }
            }
            if (Input.GetButtonDown("Drop") && itemPickup.focusedPlayer != null)
            {
                CmdPickupAnotherPlayer(itemPickup.focusedPlayer);
            }
        }
    }
}