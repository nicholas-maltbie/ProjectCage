using UnityEngine;
using Scripts.Character;

namespace Scripts.Items
{
    public class HeldCharacterSkin : MonoBehaviour
    {
        public CharacterSkin linkedCharacter;

        public Animator characterAnimator;

        void Update()
        {
            if (linkedCharacter != null)
            {
                characterAnimator.runtimeAnimatorController = linkedCharacter.animatorSkins[linkedCharacter.selectedSkin];
                characterAnimator.SetFloat("PreviousX", linkedCharacter.characterAnimator.GetFloat("PreviousX"));
                characterAnimator.SetFloat("PreviousY", linkedCharacter.characterAnimator.GetFloat("PreviousY"));
                characterAnimator.SetFloat("MoveX", linkedCharacter.characterAnimator.GetFloat("MoveX"));
                characterAnimator.SetFloat("MoveY", linkedCharacter.characterAnimator.GetFloat("MoveY"));
                characterAnimator.SetBool("Walking", linkedCharacter.characterAnimator.GetBool("Walking"));
                characterAnimator.SetBool("Held", linkedCharacter.characterAnimator.GetBool("Held"));
                characterAnimator.SetBool("Thrown", linkedCharacter.characterAnimator.GetBool("Thrown"));
            }
        }
    }
}