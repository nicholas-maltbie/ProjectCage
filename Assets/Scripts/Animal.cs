using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using Mirror;
using Scripts.Character;
using Scripts.Animals;
using Scripts.Items;

public enum Direction
{
    Right = 1,

    Left = -1
}

namespace Scripts.Animals
{
    public enum AnimalSpecies
    {
        Penguin = 1,
        Panda = 2,
        Lion = 3
    }
}
public abstract class Animal : NetworkBehaviour
{
    // Start is called before the first frame update

    public GameObject target;

    public Scripts.Items.Item favoriteFood;

    // This variable denotes whether a sprite is drawn facing Right or Left by default.
    public Direction initialFacingDirection = Direction.Left;
    private NavMeshAgent agent;
    private Animator animator;
    private bool isFacingRight;

    public AnimalSpecies species;

    protected virtual void Start()
    {
        // Set some variables
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        // Tell enclosure that this animal exists.
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (isServer)
        {
            if (target)
            {
                agent.SetDestination(target.transform.position);
            }

            DebugDrawPath(agent.path.corners);

            animator.SetFloat("MoveX", agent.velocity.x);
            SetSpriteFlip(agent.velocity.x);
            animator.SetFloat("MoveY", agent.velocity.y);
            var isWalking = agent.remainingDistance > 0.05f ? true : false;
            animator.SetBool("Walking", isWalking);
        }
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

    protected void SetSpriteFlip(float xdirection)
    {
        var shouldFlip = false;
        var sr = GetComponent<SpriteRenderer>();
        if (initialFacingDirection == Direction.Right)
        {
            // started facing right, is moving left
            if (xdirection < 0)
            {
                shouldFlip = true;
            }
        }
        // started facing left, is moving right
        else if (xdirection > 0)
        {
            shouldFlip = true;
        }
        sr.flipX = shouldFlip;
    }
    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        var isPlayer = other.CompareTag("Player");
        var isPlayerHoldingFood = isPlayer && other.GetComponent<HoldObject>().heldItem == favoriteFood;
        var isFoodItem = other.CompareTag("Food Item") && other.GetComponent<ItemState>().item == favoriteFood;
        if (isPlayerHoldingFood || isFoodItem)
        {
            target = other.gameObject;
        }
        else if (isPlayer && other.gameObject == target)
        {
            // Player was holding food, but threw it and shouldn't be followed anymore.
            target = null;
        }

    }
    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (target == other.gameObject)
        {
            target = null;
        }
    }
}
