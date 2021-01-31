using Mirror;
using UnityEngine;

namespace Scripts.Character
{
    public class TeamDeathCounter : NetworkBehaviour
    {
        public static TeamDeathCounter Instance;

        [SyncVar]
        public int deathCount;

        public void Start()
        {
            TeamDeathCounter.Instance = this;
            deathCount = 0;
        }

        public int GetDeathCount()
        {
            return deathCount;
        }

        public void IncrementDeaths()
        {
            deathCount++;
        }
    }
}