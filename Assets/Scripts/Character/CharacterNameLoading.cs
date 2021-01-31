using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Character
{
    public class CharacterNameLoading : MonoBehaviour
    {
        public static string selectedName;

        public InputField nameReference;

        public void Update()
        {
            CharacterNameLoading.selectedName = nameReference.text;
        }
    }
}