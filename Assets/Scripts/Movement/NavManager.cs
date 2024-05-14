using UnityEngine;
using UnityEngine.AI;

// Purpose: Handles all enemy navigation pathing in the world
// Directions: Should be attached to every enemy gameObject
// Other notes: 

public class NavManager : MonoBehaviour
{
    Animator anim; // Used to change walking/running animation for enemy units

    GameObject playerObject; // Used to determine aggro pathing and detect distance from this unit to the player

    BaseEnemy enemy; // Used to get enemy's attributes (moveSpeed, aggroRange, etc)

    bool inAggroRange; // Turns true when player is in aggro range (distance between player/enemy is less than enemy's aggroRange)

    // for random movement
    bool foundDestination; // Set to true when a random destination is checked and valid for the enemy to move to
    Vector3 randomDestination; // Set to the random destination that is selected during FindRandomPosition()

    NavMeshAgent agent; // The gameObject's NavMesh Agent to be used for navigation

    void Start()
    {
        SetVars();
    }

    void SetVars()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        enemy = GetComponent<BaseEnemy>();

        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
         // move enemy to player based on enemy.moveSpeed
        DetermineMovement();

        HandleAnim();
    }

    /// <summary>
    /// Checks if enemy unit should move about randomly or if the player is in range to be attacked, then processes the decision accordingly
    /// </summary>
    void DetermineMovement()
    {
        // if player is within aggro range threshold, move towards them

        if (GetRangeToPlayer() <= enemy.GetAggroRange())
        {
            // move towards player
            // Debug.Log("Player is within range, aggro to them");

            if (agent.speed != enemy.GetEnemy().chaseSpeed)
            {
                agent.speed = enemy.GetEnemy().chaseSpeed;
            }

            if (agent.stoppingDistance != CombatManager.enemyPathingToPlayerStoppingDistance)
            {
                agent.stoppingDistance = CombatManager.enemyPathingToPlayerStoppingDistance;
            }

            MoveToPlayer();

            inAggroRange = true;
        }
        else if (GetRangeToPlayer() > enemy.GetAggroRange())
        {
            // if not, move randomly
            inAggroRange = false;

            if (agent.speed != enemy.GetEnemy().moveSpeed)
            {
                agent.speed = enemy.GetEnemy().moveSpeed;
            }

            if (agent.stoppingDistance != CombatManager.enemyPathingToRandomStoppingDistance)
            {
                agent.stoppingDistance = CombatManager.enemyPathingToRandomStoppingDistance;
            }

            MoveRandomly();
            // Debug.Log("Enemy should be moving randomly");
        }
    }

    /// <summary>
    /// Checks a random position within the bounds of randomPositionRange, and if it is valid, sets the randomDestination to it
    /// </summary>
    void MoveRandomly()
    {
        if (!foundDestination && !agent.hasPath)
        {
            // Debug.Log("Move to random destination");
            
            FindRandomPosition();
        } else if (foundDestination && !agent.hasPath)
        {
            // Debug.Log("Move to " + randomDestination);
            ChangeDestination(randomDestination);
            foundDestination = false;
        }
    }

    /// <summary>
    /// Finds a random position within the bounds of randomPositionRange
    /// </summary>
    void FindRandomPosition()
    {
        Vector3 randomPosition = Vector3.zero;

        if (!foundDestination)
        {           
            float curX = transform.position.x;
            float curZ = transform.position.z;

            float randX = UnityEngine.Random.Range(curX - CombatManager.enemyRandomPathingRange, curX + CombatManager.enemyRandomPathingRange);
            float randZ = UnityEngine.Random.Range(curZ - CombatManager.enemyRandomPathingRange, curZ + CombatManager.enemyRandomPathingRange);

            Vector3 tempPosition = new Vector3(randX, 0, randZ);

            // Debug.Log("Trying random position at: " + tempPosition);

            // use navmesh.SamplePosition
            NavMeshHit hit;

            if (NavMesh.SamplePosition(tempPosition, out hit, 50f, NavMesh.AllAreas))
            {
                // Debug.Log("Hit: " + hit.position);

                randomPosition = hit.position;
                foundDestination = true;
            }

        }
        
        if (foundDestination)
        {
            // Debug.Log("Found destination: " + randomPosition);
            randomDestination = randomPosition;
        } else
        {
            // just stay idle until found
        }
    }

    /// <summary>
    /// Simply calls ChangeDestination using the player's gameObject as the parameter
    /// </summary>
    void MoveToPlayer()
    {
        ChangeDestination(playerObject);        
    }

    /// <summary>
    /// Keeps the enemy's animator state updated
    /// </summary>
    void HandleAnim()
    {
        if (!agent.isStopped && !inAggroRange)
        {
            // Debug.Log("Enemy should be walking");
            anim.SetBool("isMoving", true);
            anim.SetBool("isChasing", false);
        }
        else if (!agent.isStopped && inAggroRange)
        {
            // Debug.Log("Enemy should be running");
            anim.SetBool("isMoving", false);
            anim.SetBool("isChasing", true);
        }
    }

    /// <summary>
    /// Sets the enemy agent destination to the gameObject's position
    /// </summary>
    /// <param name="gameObject">gameObject to move to</param>
    void ChangeDestination(GameObject gameObject)
    {
        agent.destination = gameObject.transform.position;
    }

    /// <summary>
    /// Sets the enemy agent destination to the given Vector3
    /// </summary>
    /// <param name="destination">Destination the enemy unit should move to</param>
    void ChangeDestination(Vector3 destination)
    {
        agent.destination = destination;
    }

    /// <summary>
    /// Stops the enemy's movement by setting agent.isStopped to true
    /// </summary>
    public void StopMovement()
    {
        agent.isStopped = true;
    }

    /// <summary>
    /// Returns the distance between this enemy's gameObject and the player's gameObject
    /// </summary>
    public float GetRangeToPlayer()
    {
        // Debug.Log("Distance from enemy to player: " + Vector3.Distance(transform.position, playerManager.gameObject.transform.position));
        return Vector3.Distance(transform.position, playerObject.transform.position);
    }

    /// <summary>
    /// Returns true if range to player's distance to the enemy is lower than or equal to the enemy's attack range
    /// </summary>
    public bool InAttackRange()
    {
        if (GetRangeToPlayer() <= enemy.GetAttackRange())
        {
            return true;
        } else
        {
            return false;
        }
    }

}
