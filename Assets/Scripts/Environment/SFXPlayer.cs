using Mirror;
using UnityEngine;

namespace Scripts.Environment
{
    public class SFXPlayer : MonoBehaviour
    {
        public AudioClip pickupSoundClip;
        public AudioClip yeetSoundClip;
        public AudioClip[] deathSound;
        public AudioSource audioSource;

        public void PlayAudioClip(AudioClip clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }

        public void PlayYeetSound()
        {
            PlayAudioClip(yeetSoundClip);
        }

        public void PlayDeathSound()
        {
            PlayAudioClip(deathSound[UnityEngine.Random.Range(0, deathSound.Length)]);
        }

        public void PickupSound()
        {
            PlayAudioClip(pickupSoundClip);
        }
    }
}