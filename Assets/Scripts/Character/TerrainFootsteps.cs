using UnityEngine;

namespace Scripts.Character
{
    [System.Serializable]
    public class TerrainSoundModifier
    {
        public AudioClip terrainSound;
        public string terrainTag;
    }

    public class TerrainFootsteps : MonoBehaviour
    {
        public string currentTerrain;
        public AudioClip originalSound;
        public AudioClip currentSound;

        public AudioSource audioSource;

        private float timeElapsed = 0f;
        public float audioInterval = .3f;

        [SerializeField]
        public TerrainSoundModifier[] currentSounds;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called every frame
        void Update()
        {
            timeElapsed += Time.deltaTime;
            if (GetComponent<CharacterMovement>().IsMoving() && timeElapsed >= audioInterval)
            {
                audioSource.clip = currentSound;
                audioSource.Play();
                timeElapsed = 0f;
            }
        }

        private bool HasSoundForTerrain(string terrain)
        {
            foreach (TerrainSoundModifier sound in currentSounds)
            {
                if (sound.terrainTag == terrain)
                {
                    return true;
                }
            }
            return false;
        }

        private AudioClip GetSoundForTerrain(string terrain)
        {
            foreach (TerrainSoundModifier sound in currentSounds)
            {
                if (sound.terrainTag == terrain)
                {
                    return sound.terrainSound;
                }
            }
            return originalSound;
        }

        void OnTriggerStayOrEnter(Collider2D col)
        {
            if (HasSoundForTerrain(col.gameObject.tag))
            {
                currentSound = GetSoundForTerrain(col.gameObject.tag);
                currentTerrain = col.gameObject.tag;
            }
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            OnTriggerStayOrEnter(col);
        }

        void OnTriggerStay2D(Collider2D col)
        {
            OnTriggerStayOrEnter(col);
        }

        void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.tag == currentTerrain)
            {
                currentSound = originalSound;
            }
        }

    }

}