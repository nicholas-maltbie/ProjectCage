using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Character
{
    public class CharacterNameHUD : MonoBehaviour
    {
        public static CharacterNameHUD Instance { get; private set; }

        private Dictionary<uint, Text> characterNameTagMap = new Dictionary<uint, Text>();

        public Canvas nametagCanvas;

        public GameObject nametagPrefab;

        public void OnEnable()
        {
            Instance = this;
        }

        public void OnDisable()
        {
            Instance = null;
        }

        public void ClearAllNames()
        {
            foreach (uint key in characterNameTagMap.Keys)
            {
                RemoveName(key);
            }
        }

        public void RemoveName(uint playerId)
        {
            if (characterNameTagMap.TryGetValue(playerId, out Text nametag))
            {
                GameObject.Destroy(nametag);
                characterNameTagMap.Remove(playerId);
            }
        }

        public Text GetNametag(uint playerId)
        {
            if (this.characterNameTagMap.TryGetValue(playerId, out Text nametag))
            {
                return nametag;
            }
            return null;
        }

        public Text AddName(CharacterName player)
        {
            GameObject textObj = GameObject.Instantiate(nametagPrefab, nametagCanvas.transform);
            textObj.name = "Name-" + player.netId;
            characterNameTagMap[player.netId] = textObj.GetComponent<Text>();
            characterNameTagMap[player.netId].text = player.characterName;
            player.nameTransform = textObj.transform;
            return characterNameTagMap[player.netId];
        }
    }
}