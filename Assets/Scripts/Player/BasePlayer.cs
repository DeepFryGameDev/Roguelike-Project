using System;
using UnityEngine;

// Purpose: Controls all of the player's base stats and basic functions
// Directions: Attach to Player GameObject
// Other notes:

public class BasePlayer : MonoBehaviour
{
    [Tooltip("Base Max Health Points for the player - taken into account before class stats")]
    [SerializeField] int maxHP;
    public int GetMaxHP() { return maxHP; }

    [Tooltip("Base Max Stamina for the player - taken into account before class stats - used to determine sprinting duration")]
    [SerializeField] float maxStamina;
    public float GetMaxStamina() {  return maxStamina; }

    //---

    // Current Health Points
    int currentHP;
    public int GetCurrentHP() {  return currentHP; }

    // Current Stamina
    float currentStamina;
    public float GetCurrentStamina() { return currentStamina; }
    public void SetCurrentStamina(float stamina) { currentStamina = stamina;}

    // Current Experience Points
    int exp;

    // Current level
    int level;

    // The player's basic attack to be automatically equipped
    AttackScriptableObject basicAttack;
    public AttackScriptableObject GetBasicAttack() { return basicAttack; }
    public void SetBasicAttack(AttackScriptableObject basicAttack) { this.basicAttack = basicAttack; }

    // Attacks that have been gained throughout the game
    AttackScriptableObject[] secondaryAttacks;

    // Experience points needed to progress to the next level
    int currentExpToLevel;

    // Used to handle experience points, stamina, and health
    PlayerManager pm;

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

        pm.UpdateHealthBar();

        if (currentHP <= 0)
        {
            // die
            Die();
        }
    }

    /// <summary>
    /// Raises players health points by the provided value
    /// </summary>
    /// <param name="healthToHeal">Amount of health points to be replenished</param>
    public void Heal(int healthToHeal)
    {
        currentHP += healthToHeal;

        if (currentHP >= maxHP)
        {
            currentHP = maxHP;
        }

        pm.UpdateHealthBar();
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
        // Turn off player controls

        // Display GameOver
        UIHandler uih = FindObjectOfType<UIHandler>();

        uih.DisplayGameOver();

        // Show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
