using UnityEngine;
using UnityEngine.AI;
namespace Scripts.Navigation
{
    public class Chaser : MonoBehaviour
    {
        public GameObject target;
        private NavMeshAgent agent;
        private bool stop;
        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }
        // Update is called once per frame
        void Update()
        {
            if(target)
            {
                agent.SetDestination(target.transform.position);
                Debug.Log("No navigation target.");
            }
            DebugDrawPath(agent.path.corners);
        }
        public static void DebugDrawPath(Vector3[] corners)
        {
            if (corners.Length < 2) { return; }
            int i = 0;
            for (; i < corners.Length - 1; i++)
            {
                Debug.DrawLine(corners[i], corners[i + 1], Color.blue);
            }
            Debug.DrawLine(corners[0], corners[1], Color.red);
        }
    }
}
