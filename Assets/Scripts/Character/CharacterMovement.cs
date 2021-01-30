using Mirror;
using UnityEngine;

namespace Scripts.Character
{
    public class CharacterMovement : NetworkBehaviour
    {
        private Rigidbody2D body;
        private float horizontal;
        private float vertical;

        public float runSpeed = 20.0f;

        private Vector2 previousLastMovement;

        public Vector2 lastMovement;

        [Command]
        public void CmdSetLastMovement(Vector2 movement)
        {
            lastMovement = movement;
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
        }

        void FixedUpdate()
        {
            if (isLocalPlayer)
            {
                Vector2 movement = new Vector2(horizontal, vertical);
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