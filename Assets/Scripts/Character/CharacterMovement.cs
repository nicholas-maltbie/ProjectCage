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

      void Start ()
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
            movement = movement.magnitude > 1 ? movement / movement.magnitude : movement;

            body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
         }
      }
   }
}