using Mirror;
using Mirror.Experimental;
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
        public float thrownCooldown;

        public Transform holder;

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
                transform.position = holder.transform.position;
            }
        }

        void FixedUpdate()
        {
            if (isDebugPlayer && isServer)
            {
                if (heldState == CharacterHeld.Thrown)
                {
                    thrownCooldown -= Time.fixedDeltaTime;
                    if (thrownCooldown <= 0 || body.velocity.magnitude <= stopSlidingSpeed)
                    {
                        this.thrownCooldown = 0;
                        this.heldState = CharacterHeld.Normal;
                    }
                }
            }
            if (isLocalPlayer)
            {
                Vector2 movement = new Vector2(horizontal, vertical);
                if (heldState == CharacterHeld.Held)
                {
                    movement = Vector2.zero;
                }
                else if (heldState == CharacterHeld.Thrown)
                {
                    thrownCooldown -= Time.fixedDeltaTime;
                    if (thrownCooldown <= 0 || body.velocity.magnitude <= stopSlidingSpeed)
                    {
                        this.thrownCooldown = 0;
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