using Mirror;
using UnityEngine;

namespace Scripts.Character
{
    public class DropAndTeleportOnDeath : MonoBehaviour
    {
        public void KillCharacter()
        {
            // Drop what we're carrying
            GetComponent<HoldObject>().CmdDropThings();
            // Teleport back to a spawn location
            Transform teleport = GameObject.FindObjectOfType<NetworkManager>().GetStartPosition();
            gameObject.transform.position = teleport.position;
            // Increment death Counter
            TeamDeathCounter.Instance.IncrementDeaths();
        }
    }
}