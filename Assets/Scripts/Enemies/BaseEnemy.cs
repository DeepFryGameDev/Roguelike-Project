using UnityEngine;

// Purpose: Manages all enemy basic methods and vars
// Directions: Should be attached to every enemy gameobject
// Other notes: 

public class BaseEnemy : MonoBehaviour
{
    [Tooltip("Attach Enemy Scriptable Object for the desired enemy")]
    [SerializeField] EnemyScriptableObject enemySO;
    public EnemyScriptableObject GetEnemy() { return enemySO; }

    [Tooltip("If true, will show the enemy's range to aggro and range to attack the player in the editor")]
    [SerializeField] bool viewRangeGizmos;

    [Tooltip("Distance from the enemy to the player before the enemy begins chasing the player")]
    [SerializeField] float aggroRange = 50f;
    public float GetAggroRange() { return aggroRange; }

    [Tooltip("Distance from the enemy to the player before the enemy starts attacking the player")]
    [SerializeField] float attackRange = 30f;
    public float GetAttackRange() { return attackRange; }

    EnemySpawner spawnedFrom; // The enemySpawner that spawned this enemy
    public EnemySpawner GetSpawnedFrom() { return spawnedFrom; }
    public void SetSpawnedFrom(EnemySpawner spawnedFrom) { this.spawnedFrom = spawnedFrom; }

    // HP of the enemy
    int currentHP;

    // Set to the enemy's max HP when the object is created
    int maxHP;

    // Collider attached to the object
    Collider col;

    // Used to grant the player experience upon defeat
    PlayerManager playerManager;

    // Used to stop movement upon defeat
    NavManager navManager;

    // Set to the enemy's attached Attack Manager
    EnemyAttackManager enemyAttackManager;

    void Awake()
    {
        SetVars();
        PrepareCollisionTriggers();
    }

    void SetVars()
    {
        maxHP = enemySO.HP;
        currentHP = maxHP;

        enemyAttackManager = GetComponent<EnemyAttackManager>();

        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();

        col = GetComponent<Collider>();

        navManager = GetComponent<NavManager>();        
    }

    /// <summary>
    /// Ensures EAM is not null before setting collision triggers
    /// </summary>
    void PrepareCollisionTriggers()
    {
        if (enemyAttackManager == null)
        {
            Debug.LogError("EAM is null on " + gameObject.name);
        }

        SetCollisionTriggers(enemySO.basicAttack.collisionTrigger);
    }

    /// <summary>
    /// Creates the collision trigger based on the enemy's basic attack, and instantiates it attached to the enemy object
    /// </summary>
    /// <param name="obj">Collision Trigger prefab to be instantiated to the enemy transform</param>
    void SetCollisionTriggers(GameObject obj)
    {
        // Debug.Log("Setting collision trigger for " + gameObject.name);
        Instantiate(obj, enemyAttackManager.GetAttackCollisionTriggerTransform());
    }

    /// <summary>
    /// Lowers the enemy's HP by the amount of damage provided, and processes death if HP is 0 or below
    /// </summary>
    /// <param name="damage">Amount of damage to be applied</param>
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            GrantPlayerEXP();
            Die();
        }
    }

    /// <summary>
    /// Increases the player's total experience points by the enemy's EXP value
    /// </summary>
    void GrantPlayerEXP()
    {
        playerManager.GrantExp(enemySO.expGiven);
    }

    /// <summary>
    /// Triggers when the enemy's HP reaches 0 - stops movement, plays death animation, disables collider, and destroys the object from the world
    /// </summary>
    void Die()
    {
        Debug.Log(enemySO.name + " is dead!");

        // stop movement on navmesh
        navManager.StopMovement();

        // play death animation
        GetComponent<Animator>().SetBool("isDead", true);

        // disable collider
        col.enabled = false;

        // destroy after x seconds
        Destroy(gameObject, CombatManager.enemyDespawnTime);
    }

    /// <summary>
    /// If viewRangeGizmos is true, the editor will display the aggro and attack ranges of the enemy.
    /// </summary>
    void OnDrawGizmos()
    {
        if (viewRangeGizmos)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, aggroRange);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
