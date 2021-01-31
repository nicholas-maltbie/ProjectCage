using Mirror;
using UnityEngine;

namespace Scripts.Environment
{
    public class KillBox : MonoBehaviour
    {
        public void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("DIE DIE DIE" + " " + other.gameObject.name);
            other.gameObject.SendMessage("KillCharacter", SendMessageOptions.DontRequireReceiver);
        }
    }
}