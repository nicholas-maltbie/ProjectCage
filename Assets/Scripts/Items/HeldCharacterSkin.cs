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
                characterAnimator.SetBool("Held", true);
            }
        }
    }
}