using Mirror;
using UnityEngine;

namespace Scripts.Character
{
    public class CharacterAnimator : NetworkBehaviour
    {
        public Rigidbody2D body;
        public Animator animationController;
        public SpriteRenderer spriteRenderer;

        public bool isRightFlipped;

        private bool previousFlippedState;

        public Vector2 previousMove = Vector2.zero;

        [SyncVar]
        private bool flippedState;

        [Command]
        public void CmdSetFlippedState(bool flippedState)
        {
            this.flippedState = flippedState;
        }

        void Update()
        {
            if (isLocalPlayer)
            {
                if (previousMove.magnitude > 0)
                {
                    animationController.SetFloat("PreviousX", previousMove.x);
                    animationController.SetFloat("PreviousY", previousMove.y);
                }
                animationController.SetFloat("MoveX", body.velocity.x);
                animationController.SetFloat("MoveY", body.velocity.y);
                previousMove = body.velocity;

                animationController.SetBool("Walking", body.velocity.magnitude > 0);

                bool newFlippedState = isRightFlipped == (body.velocity.x > 0);
                if (newFlippedState != previousFlippedState)
                {
                    CmdSetFlippedState(newFlippedState);
                }
                previousFlippedState = newFlippedState;
            }

            spriteRenderer.flipX = this.flippedState;
        }
    }
}