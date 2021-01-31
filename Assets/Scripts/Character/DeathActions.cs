using Mirror;
using UnityEngine;
using Scripts.Environment;
using System.Collections;

namespace Scripts.Character
{
    public enum PlayerDeath
    {
        Dead,
        Alive
    }

    public class DeathActions : NetworkBehaviour
    {
        public GameObject deathSplatter;

        [SyncVar]
        public PlayerDeath deathState;

        public SFXPlayer player;

        public float deathTimer = 5.0f;

        public bool IsAlive => deathState == PlayerDeath.Alive;

        [Command]
        public void CmdKillCharacter()
        {
            // Increment death Counter
            TeamDeathCounter.Instance.IncrementDeaths();
            // Kill the player... for now
            // Play a death sound effect
            RpcPlayDeathSound();
            StartCoroutine(PlayerDeathServerActions());
        }

        public void KillCharacter()
        {
            if (isLocalPlayer)
            {
                // Drop what we're carrying
                GetComponent<HoldObject>().CmdDropThings();
                StartCoroutine(PlayerDeathTimer());
                CmdKillCharacter();
            }
        }

        public IEnumerator PlayerDeathServerActions()
        {
            GameObject deathSplatterInstance = Instantiate(deathSplatter);
            deathSplatterInstance.transform.position = transform.position;
            NetworkServer.Spawn(deathSplatterInstance);
            deathState = PlayerDeath.Dead;
            yield return new WaitForSeconds(deathTimer);
            deathState = PlayerDeath.Alive;
            NetworkServer.Destroy(deathSplatterInstance);
        }

        public IEnumerator PlayerDeathTimer()
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().isKinematic = true;
            deathState = PlayerDeath.Dead;
            yield return new WaitForSeconds(deathTimer);
            deathState = PlayerDeath.Alive;
            // Teleport back to a spawn location
            Transform teleport = GameObject.FindObjectOfType<NetworkManager>().GetStartPosition();
            gameObject.transform.position = teleport.position;
            GetComponent<Rigidbody2D>().isKinematic = false;
        }

        [ClientRpc]
        public void RpcPlayDeathSound()
        {
            player.PlayDeathSound();
        }
    }
}