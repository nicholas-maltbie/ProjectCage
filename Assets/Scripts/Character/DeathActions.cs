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
        [SyncVar]
        public PlayerDeath deathState;

        public SFXPlayer player;

        public float deathTimer = 5.0f;

        public bool IsAlive => deathState == PlayerDeath.Alive;

        public void KillCharacter()
        {
            // Drop what we're carrying
            GetComponent<HoldObject>().CmdDropThings();
            // Play a death sound effect
            this.CmdPlayerDeathSound();
            // Increment death Counter
            TeamDeathCounter.Instance.IncrementDeaths();
            // Kill the player... for now
            CmdStartDeathTimer();
        }

        public IEnumerator PlayerDeathTimer()
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            deathState = PlayerDeath.Dead;
            yield return new WaitForSeconds(deathTimer);
            deathState = PlayerDeath.Alive;
            // Teleport back to a spawn location
            Transform teleport = GameObject.FindObjectOfType<NetworkManager>().GetStartPosition();
            gameObject.transform.position = teleport.position;
        }

        [Command]
        public void CmdStartDeathTimer()
        {
            StartCoroutine(PlayerDeathTimer());
        }

        [ClientRpc]
        public void RpcPlayDeathSound()
        {
            player.PlayDeathSound();
        }

        [Command]
        public void CmdPlayerDeathSound()
        {
            RpcPlayDeathSound();
        }
    }
}