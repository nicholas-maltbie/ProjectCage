using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Character
{
    public class CharacterName : NetworkBehaviour
    {
        public SpriteRenderer characterSprite;
        public Transform nameTransform;

        public Text nameReference;
        
        [SyncVar(hook = nameof(OnChangeName))]
        public string characterName = "Player";

        public void OnChangeName(string oldName, string newName)
        {
            nameReference.text = newName;
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            nameReference = CharacterNameHUD.Instance.AddName(this);
            nameTransform = nameReference.transform;
        }

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            CmdSetCharacterName(CharacterNameLoading.selectedName);
        }

        [Command]
        public void CmdSetCharacterName(string newName)
        {
            characterName = newName;
        }

        void LateUpdate()
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(characterSprite.bounds.center.x, characterSprite.bounds.max.y));
            screenPos.y += 4;
            if (nameTransform != null)
            {
                nameTransform.position = screenPos;
            }
        }
    }
}