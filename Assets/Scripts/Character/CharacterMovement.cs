using Mirror;
using UnityEngine;

namespace Scripts.Character
{
    public enum CharacterHeld
    {
        Held,
        Thrown,
        Normal
    }

    public class CharacterMovement : NetworkBehaviour
    {
        public Vector2 yeetVelocity;

        public Vector2 yeetPos;

        public bool yeeted;

        public void YeetPlayer(Vector2 newVelocity, Vector2 newPos)
        {
            UnityEngine.Debug.Log($"Command to yeet player {newVelocity}, {newPos}");
            yeetVelocity = newVelocity;
            yeetPos = newPos;
            yeeted = true;
        }

        [ClientRpc]
        public void RpcYeetPlayer(Vector3 newVelocity, Vector3 newPos)
        {
            YeetPlayer(newVelocity, newPos);
        }

        public float thrownCooldown = 3.0f;

        public float minThrow = 1.0f;

        public float throwElapsed = 0.0f;

        [SyncVar]
        public GameObject holder;

        [SyncVar(hook = nameof(OnChangeHeldState))]
        public CharacterHeld heldState = CharacterHeld.Normal;

        [SyncVar]
        public CharacterMovement holdingCharacterController;

        public float stopSlidingSpeed = 0.01f;

        private Rigidbody2D body;
        private float horizontal;
        private float vertical;

        public bool isDebugPlayer = false;

        public float runSpeed = 20.0f;

        private Vector2 previousLastMovement;

        [SyncVar]
        public Vector2 lastMovement;

        public void ApplyYeet()
        {
            if (yeeted)
            {
                transform.position = yeetPos;
                GetComponent<Rigidbody2D>().velocity = yeetVelocity;
                yeeted = false;
            }
        }

        public void OnChangeHeldState(CharacterHeld oldHeld, CharacterHeld newHeld)
        {
            bool colliderEnabled = true;
            bool rigidbodyEnabled = true;
            switch (newHeld)
            {
                case CharacterHeld.Held:
                    colliderEnabled = false;
                    rigidbodyEnabled = true;
                    break;
                case CharacterHeld.Thrown:
                    colliderEnabled = true;
                    rigidbodyEnabled = true;
                    break;
                case CharacterHeld.Normal:
                    colliderEnabled = true;
                    rigidbodyEnabled = true;
                    break;
            }

            GetComponent<Collider2D>().enabled = colliderEnabled;
            GetComponent<Rigidbody2D>().isKinematic = !rigidbodyEnabled;
        }

        [Command]
        public void CmdSetLastMovement(Vector2 movement)
        {
            lastMovement = movement;
        }

        [Command]
        public void CmdSetHeldState(CharacterHeld heldState)
        {
            this.heldState = heldState;
        }

        void Start()
        {
            body = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            if (isLocalPlayer)
            {
                horizontal = Input.GetAxisRaw("Horizontal");
                vertical = Input.GetAxisRaw("Vertical");
            }
            if (!isLocalPlayer)
            {
                if (this.heldState == CharacterHeld.Held)
                {
                    this.GetComponent<CharacterAnimator>().spriteRenderer.enabled = false;
                }
                else
                {
                    this.GetComponent<CharacterAnimator>().spriteRenderer.enabled = true;
                }
            }
        }

        void LateUpdate()
        {
            if ((isLocalPlayer || (isDebugPlayer && isServer)) && heldState == CharacterHeld.Held && holder != null)
            {
                transform.position = holder.GetComponent<HoldObject>().holdingTransform.position;
            }
            if (heldState == CharacterHeld.Thrown)
            {
                ApplyYeet();
            }
        }

        void FixedUpdate()
        {
            if (isLocalPlayer)
            {
                Vector2 movement = new Vector2(horizontal, vertical);
                if (heldState == CharacterHeld.Held)
                {
                    movement = Vector2.zero;
                }
                else if (heldState == CharacterHeld.Thrown)
                {
                    throwElapsed += Time.fixedDeltaTime;
                    if (throwElapsed >= thrownCooldown || (throwElapsed >= minThrow && body.velocity.magnitude <= stopSlidingSpeed))
                    {
                        this.throwElapsed = 0;
                        CmdSetHeldState(CharacterHeld.Normal);
                    }
                    else
                    {
                        return;
                    }
                }
                // Make sure the vector is not larger than 1
                movement = movement.magnitude > 1 ? movement.normalized : movement;

                body.velocity = movement * runSpeed;

                if (movement != previousLastMovement && movement.magnitude > 0)
                {
                    CmdSetLastMovement(movement);
                }
                previousLastMovement = movement;
            }
        }
    }
}