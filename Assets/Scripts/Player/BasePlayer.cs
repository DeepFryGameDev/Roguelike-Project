using UnityEngine;

// Purpose: Controls all of the player's base stats and basic functions
// Directions: Attach to Player GameObject
// Other notes:

public class BasePlayer : MonoBehaviour
{
    [Tooltip("Base Max Health Points for the player - taken into account before class stats")]
    public int maxHP;

    [Tooltip("Base Max Stamina for the player - taken into account before class stats - used to determine sprinting duration")]
    public float maxStamina;

    //---

    [Tooltip("Current Health Points")]
    [ReadOnly] public int currentHP;

    [Tooltip("Current Stamina")]
    [ReadOnly] public float currentStamina;

    [Tooltip("Current Experience Points")]
    [ReadOnly] public int exp;

    [Tooltip("Current level")]
    [ReadOnly] public int level;

    [Tooltip("The player's basic attack to be automatically equipped")]
    [ReadOnly] public AttackScriptableObject basicAttack;

    [Tooltip("Attacks that have been gained throughout the game")]
    [ReadOnly] public AttackScriptableObject[] secondaryAttacks;

    int currentExpToLevel; // Experience points needed to progress to the next level

    PlayerManager pm; // Used to check experience points needed to the next level

    /// <summary>
    /// All inherited classes should call base.Awake() in 'protected override void Awake()'
    /// </summary>
    protected virtual void Awake()
    {
        SetUp();
    }

    /// <summary>
    /// Sets base stats and attributes for the player
    /// </summary>
    protected void SetUp()
    {
        pm = GetComponent<PlayerManager>();
        currentHP = maxHP;

        currentStamina = maxStamina;
        Debug.Log("Setting currentStamina to " + currentStamina);

        level = 1;

        currentExpToLevel = pm.expToNextLevel[level - 1];
    }

    /// <summary>
    /// Increases player's experience points by provided value, and if the player's total experience points exceed the required amount to level up, initiates the level up method
    /// </summary>
    /// <param name="expToGain">Amount of experience points to add to player's exp</param>
    public void GainExp(int expToGain)
    {
        exp += expToGain;

        if (exp >= currentExpToLevel)
        {
            LevelUp();
            exp = 0;
        }
    }

    /// <summary>
    /// Lower's players health points by the provided value
    /// </summary>
    /// <param name="damage">Amount of health points to be decreased</param>
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            // die
            Die();
        }
    }

    /// <summary>
    /// Increases the player's level, and should prompt them for upgrading their attack or adding a new one
    /// </summary>
    void LevelUp()
    {
        level++;
    }

    /// <summary>
    /// Should result in asking the player if they want to quit or load 
    /// </summary>
    void Die()
    {
        
    }
}
