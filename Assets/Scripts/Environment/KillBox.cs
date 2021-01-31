using Mirror;
using UnityEngine;

namespace Scripts.Environment
{
    public class KillBox : MonoBehaviour
    {
        public void OnTriggerEnter2D(Collider2D other)
        {
            other.gameObject.SendMessage("KillCharacter", SendMessageOptions.DontRequireReceiver);
        }
    }
}