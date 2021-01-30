using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using Mirror;

public abstract class Animal : NetworkBehaviour
{
    // Start is called before the first frame update

    public GameObject target;
    private NavMeshAgent agent;

    private Animator animator;

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(target)
        {
            agent.SetDestination(target.transform.position);
            Debug.Log("No navigation target.");
        }
        DebugDrawPath(agent.path.corners);
        animator.SetFloat("MoveX", agent.velocity.x);
        animator.SetFloat("MoveY", agent.velocity.y);
        var isWalking = target ? true : false;
        animator.SetBool("Walking", isWalking);
    }
    protected static void DebugDrawPath(Vector3[] corners)
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
