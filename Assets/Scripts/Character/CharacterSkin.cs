using Mirror;
using UnityEngine;

namespace Scripts.Character
{
    public class CharacterSkin : NetworkBehaviour
    {
        public Animator characterAnimator;

        public RuntimeAnimatorController[] animatorSkins;

        [SyncVar]
        private int selectedSkin = 0;

        public override void OnStartServer()
        {
            selectedSkin = ((int)this.netId) % animatorSkins.Length;
        }

        void Update()
        {
            characterAnimator.runtimeAnimatorController = animatorSkins[selectedSkin];
        }
    }
}