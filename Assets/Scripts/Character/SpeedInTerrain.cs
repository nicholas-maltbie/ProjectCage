using UnityEngine;

namespace Scripts.Character
{
    [System.Serializable]
    public class TerrainSpeedModifier
    {
        public float speedModifier;
        public string terrainTag;
    }

    public class SpeedInTerrain : MonoBehaviour
    {
        public string currentTerrain;

        public float originalSpeed;

        [SerializeField]
        public TerrainSpeedModifier[] speedModifiers;

        // Start is called before the first frame update
        void Start()
        {
            originalSpeed = this.GetComponent<CharacterMovement>().runSpeed;
        }

        private bool HasSpeedForTerrain(string terrain)
        {
            foreach (TerrainSpeedModifier speedMod in speedModifiers)
            {
                if (speedMod.terrainTag == terrain)
                {
                    return true;
                }
            }
            return false;
        }

        private float GetSpeedForTerrain(string terrain)
        {
            foreach (TerrainSpeedModifier speedMod in speedModifiers)
            {
                if (speedMod.terrainTag == terrain)
                {
                    return speedMod.speedModifier * originalSpeed;
                }
            }
            return originalSpeed;
        }

        void OnTriggerStayOrEnter(Collider2D col)
        {
            if (HasSpeedForTerrain(col.gameObject.tag))
            {
                this.GetComponent<CharacterMovement>().runSpeed = GetSpeedForTerrain(col.gameObject.tag);
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
                this.GetComponent<CharacterMovement>().runSpeed = originalSpeed;
            }
        }

    }

}