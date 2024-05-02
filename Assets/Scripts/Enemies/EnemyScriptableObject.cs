using UnityEngine;

// Purpose: Provides the base for all enemy vars and functions
// Directions: Create an enemy and save it to Resources/Enemies, and attach to an enemy GameObject using BaseEnemy
// Other notes: 

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Enemies/New Enemy", order = 1)]
public class EnemyScriptableObject : ScriptableObject
{
    [Tooltip("Name of the enemy to be used in the game")]
    public string enemyName;

    [Tooltip("Base Maximum Hit Points")]
    public int HP;

    [Tooltip("How quickly the enemy's gameObject will travel around the world")]
    public int moveSpeed;

    [Tooltip("How quickly the enemy's gameObject will travel when the player has aggro from the enemy")]
    public int chaseSpeed;

    [Tooltip("Amount of EXP provided when the enemy is killed")]
    public int expGiven;

    [Tooltip("Set to the scriptable object for the attack the enemy will fire")]
    public AttackScriptableObject basicAttack;

}