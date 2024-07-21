using System.Collections;
using UnityEngine;


// Purpose: Handles the pathing for orbit attacks
// Directions: Attach to any particle that is using orbit projection
// Other notes:


public class EnemySpawner : BaseAttackableUnit
{
    [Tooltip("Base Max Health Points for the player - taken into account before class stats")]
    [SerializeField] protected int maxHP;
    public int GetMaxHP() { return maxHP; }

    [Tooltip("Enemies that will spawn randomly")]
    [SerializeField] GameObject[] enemies;

    [Tooltip("Maximum number of spawns allowed on the field")]
    [SerializeField] int maxSpawns = 5;

    IEnumerator spawnRoutine; // Used to stop the coroutine for spawning enemies in the event of death

    Transform spawnPoint; // Used for starting point for spawning enemy

    Transform enemiesParent; // Used to set GameObject in hierarchy

    AttackManager am; // Used to add the collider for spawned enemy's to any orbit attack (or any that do not expire)

    void Start()
    {
        am = FindAnyObjectByType<AttackManager>();

        currentHP = maxHP;

        spawnPoint = transform.GetChild(0);

        enemiesParent = GameObject.Find("[NPCs]").transform;        

        // Start coroutine to randomly spawn enemies
        spawnRoutine = SpawnEnemies();
        StartCoroutine(spawnRoutine);        
    }

    /// <summary>
    /// Coroutine - used to continuously spawn enemies until the enemy spawner has been killed (at which point this is interrupted)
    /// Enemies will also stop being spawned when the limit has been reached
    /// </summary>
    IEnumerator SpawnEnemies()
    {
        // Get random number between baseSpawnDelay and spawnDelayMax
        float rand = Random.Range(CombatManager.baseEnemySpawnDelay, CombatManager.baseEnemySpawnDelay + CombatManager.enemySpawnDelayMax);

        //Debug.Log("Waiting for " + rand + " seconds");

        yield return new WaitForSeconds(rand);

        // if HP > 0, and maxSpawns have not been reached, continue spawning enemies

        if (GetSpawnCount() < maxSpawns)
        {
            // Spawn an enemy
            SpawnEnemyUnit();
        }

        spawnRoutine = SpawnEnemies();
        StartCoroutine(spawnRoutine);
    }

    /// <summary>
    /// Called when an attack's collider from the player hits the enemy spawner
    /// </summary>
    /// <param name="damage">Amount of HP to be lowered from currentHP</param>
    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Instantiates the enemy assigned to the spawner and sets the enemy's collider to any active orbit attack particles
    /// </summary>
    void SpawnEnemyUnit()
    {
        GameObject newEnemy = Instantiate(enemies[0], spawnPoint.position, spawnPoint.rotation, enemiesParent);
        newEnemy.GetComponent<BaseEnemy>().SetSpawnedFrom(this);

        // Add to orbit attack triggers
        foreach (ParticleSystem ps in am.GetOrbitParticles())
        {
            ps.trigger.AddCollider(newEnemy.GetComponent<Collider>());
        }
    }

    /// <summary>
    /// Returns the amount of enemies that have been spawned from this enemy spawner
    /// </summary>
    int GetSpawnCount()
    {
        int tempCount = 0;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<BaseEnemy>().GetSpawnedFrom() == this)
            {
                tempCount++;
            }
        }

        //Debug.Log("Spawn count: " + tempCount);
        return tempCount;
    }

    /// <summary>
    /// Called when the enemy spawner's health reaches 0. Stops any currently spawning enemy and destroys the game object
    /// </summary>
    void Die()
    {
        StopCoroutine(spawnRoutine);
        Destroy(gameObject);
    }
}
