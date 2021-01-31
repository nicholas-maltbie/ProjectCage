using Mirror;
using UnityEngine;

namespace Scripts.Character
{
    [RequireComponent(typeof(CharacterMovement))]
    public class CharacterAnimator : NetworkBehaviour
    {
        public Rigidbody2D body;
        public Animator animationController;
        public SpriteRenderer spriteRenderer;

        public bool isRightFlipped;

        private bool previousFlippedState;

        private CharacterMovement characterMovement;

        [SyncVar]
        private bool flippedState;

        public void SetFlippedState(bool flippedState)
        {
            this.flippedState = flippedState;
        }

        [Command]
        public void CmdSetFlippedState(bool flippedState)
        {
            SetFlippedState(flippedState);
        }

        public void Start()
        {
            characterMovement = GetComponent<CharacterMovement>();
        }

        void Update()
        {
            if (isLocalPlayer || (characterMovement.isDebugPlayer && isServer))
            {
                Vector2 previousMove = characterMovement.lastMovement;
                animationController.SetFloat("PreviousX", previousMove.x);
                animationController.SetFloat("PreviousY", previousMove.y);
                animationController.SetFloat("MoveX", body.velocity.x);
                animationController.SetFloat("MoveY", body.velocity.y);
                animationController.SetBool("Walking", body.velocity.magnitude > 0);
                animationController.SetBool("Held", characterMovement.heldState == CharacterHeld.Held || !GetComponent<DeathActions>().IsAlive);
                animationController.SetBool("Thrown", characterMovement.heldState == CharacterHeld.Thrown);

                // Check for flipped state when moving
                bool newFlippedState = isRightFlipped == (body.velocity.x > 0);
                // Check for flipped state when still
                if (body.velocity.magnitude == 0)
                {
                    newFlippedState = isRightFlipped == (previousMove.x > 0);
                }
                // Check for flipped state when held
                if (characterMovement.heldState == CharacterHeld.Held)
                {
                    newFlippedState = isRightFlipped == (characterMovement.holdingCharacterController.lastMovement.x > 0);
                }

                if (newFlippedState != previousFlippedState)
                {
                    if (!isServer)
                    {
                        CmdSetFlippedState(newFlippedState);
                    }
                }
                previousFlippedState = newFlippedState;
                this.flippedState = newFlippedState;
            }

            if (characterMovement.heldState == CharacterHeld.Held)
            {
                spriteRenderer.sortingLayerName = "HeldPlayer";
            }
            else
            {
                spriteRenderer.sortingLayerName = "Player";
            }

            spriteRenderer.flipX = this.flippedState;
        }
    }
}