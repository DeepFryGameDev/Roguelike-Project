using UnityEngine;

// Purpose: Manages all enemy basic methods and vars
// Directions: Should be attached to every enemy gameobject
// Other notes: 

public class BaseEnemy : BaseAttackableUnit
{
    [Tooltip("Base Max Health Points for the player - taken into account before class stats")]
    [SerializeField] protected int maxHP;
    public int GetMaxHP() { return maxHP; }

    [Tooltip("How quickly the enemy's gameObject will travel around the world")]
    public int moveSpeed;

    [Tooltip("How quickly the enemy's gameObject will travel when the player has aggro from the enemy")]
    public int chaseSpeed;

    [Tooltip("Amount of EXP provided when the enemy is killed")]
    public int expGiven;

    [Tooltip("Enemy's attack to be fired")]
    [SerializeField] AttackScriptableObject attack; 
    public AttackScriptableObject GetAttack() { return attack; }

    [Tooltip("Distance from the enemy to the player before the enemy begins chasing the player")]
    [SerializeField] float aggroRange = 50f;
    public float GetAggroRange() { return aggroRange; }

    [Tooltip("Distance from the enemy to the player before the enemy starts attacking the player")]
    [SerializeField] float attackRange = 30f;
    public float GetAttackRange() { return attackRange; }

    [Tooltip("If true, will show the enemy's range to aggro and range to attack the player in the editor")]
    [SerializeField] bool viewRangeGizmos;

    EnemySpawner spawnedFrom; // The enemySpawner that spawned this enemy
    public EnemySpawner GetSpawnedFrom() { return spawnedFrom; }
    public void SetSpawnedFrom(EnemySpawner spawnedFrom) { this.spawnedFrom = spawnedFrom; }

    // Collider attached to the object
    Collider col;

    // Used to grant the player experience upon defeat
    PlayerManager playerManager;

    // Used to stop movement upon defeat
    NavManager navManager;

    // Set to the enemy's attached Attack Manager
    EnemyAttackManager enemyAttackManager;

    PrefabManager prefabManager;

    void Awake()
    {
        SetVars();
        PrepareCollisionTriggers();
    }

    void SetVars()
    {
        enemyAttackManager = GetComponent<EnemyAttackManager>();

        prefabManager = FindAnyObjectByType<PrefabManager>();

        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();

        col = GetComponent<Collider>();

        navManager = GetComponent<NavManager>();

        SetStrength(GetBaseStrength());
        SetEndurance(GetBaseEndurance());
        SetAgility(GetBaseAgility());
        SetDexterity(GetBaseDexterity());
        SetIntelligence(GetBaseIntelligence());
        SetResist(GetBaseResist());

        currentHP = GetMaxHP();
    }

    /// <summary>
    /// As enemy does not have equipment, BaseArmor is the true souce of data for unit's armor
    /// </summary>
    /// <returns>Unit's BaseArmor</returns>
    public override int GetArmor()
    {
        return GetBaseArmor();
    }

    /// <summary>
    /// As enemy does not have equipment, BaseMagicResist is the true souce of data for unit's magic resist
    /// </summary>
    /// <returns>Unit's BaseMagicResist</returns>
    public override int GetMagicResist()
    {
        return GetBaseMagicResist();
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

        SetCollisionTriggers(attack.collisionTrigger);
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
    public void TakeDamage(int damage, EnumHandler.DamageTextTypes textType)
    {
        // Show damage popup
        ShowDamagePopup(damage, textType);

        currentHP -= damage;
        if (currentHP <= 0)
        {
            GrantPlayerEXP();
            Die();
        }
    }

    /// <summary>
    /// Shows damage floating text in UI for user feedback to know what damage was taken when an attack connects.
    /// (This will need to be moved, but not sure where to put it yet. Does not belong on BaseEnemy)
    /// </summary>
    /// <param name="damage">Damage value to be displayed</param>
    /// <param name="textType">If text should be standard text, or if attack is a critical hit, text is larger</param>
    void ShowDamagePopup(int damage, EnumHandler.DamageTextTypes textType)
    {
        // Debug.Log("Creating popup at " + gameObject.transform.position);
        DamageFloatingText.Create(gameObject.transform.position, damage, textType);
    }

    /// <summary>
    /// Increases the player's total experience points by the enemy's EXP value
    /// </summary>
    void GrantPlayerEXP()
    {
        playerManager.GainExp(expGiven);
    }

    /// <summary>
    /// Triggers when the enemy's HP reaches 0 - stops movement, plays death animation, disables collider, and destroys the object from the world
    /// </summary>
    void Die()
    {
        Debug.Log(name + " is dead!");

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
