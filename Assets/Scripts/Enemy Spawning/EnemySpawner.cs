using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Tooltip("Health Points for the spawner")]
    [SerializeField] int HP;

    int currentHP; // amount of HP that the spawner still has left

    [Tooltip("Enemies that will spawn randomly")]
    [SerializeField] GameObject[] enemies;

    [Tooltip("Maximum number of spawns allowed on the field")]
    [SerializeField] int maxSpawns = 5;

    IEnumerator spawnRoutine; // Used to stop the coroutine in the event of death

    Transform spawnPoint; // Used for starting point for spawning enemy

    Transform enemiesParent; // Used to set GameObject in hierarchy

    void Start()
    {
        currentHP = HP;

        spawnPoint = transform.GetChild(0);

        enemiesParent = GameObject.Find("[NPCs]").transform;

        // Start coroutine to randomly spawn enemies
        spawnRoutine = SpawnEnemies();
        StartCoroutine(spawnRoutine);
    }

    void Update()
    {

    }

    IEnumerator SpawnEnemies()
    {
        // Get random number between baseSpawnDelay and spawnDelayMax
        float rand = Random.Range(CombatManager.baseEnemySpawnDelay, CombatManager.baseEnemySpawnDelay + CombatManager.enemySpawnDelayMax);

        Debug.Log("Waiting for " + rand + " seconds");

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

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void SpawnEnemyUnit()
    {
        GameObject newEnemy = Instantiate(enemies[0], spawnPoint.position, spawnPoint.rotation, enemiesParent);
        newEnemy.GetComponent<BaseEnemy>().SetSpawnedFrom(this);
    }

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

        Debug.Log("Spawn count: " + tempCount);
        return tempCount;
    }

    void Die()
    {
        StopCoroutine(spawnRoutine);
        Destroy(gameObject);
    }
}
