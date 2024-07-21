using System.Collections;
using UnityEngine;

// Purpose: To handle the processing of attack triggers upon collision between player/enemies
// Directions: Attach to any AttackCollision prefab
// Other notes:

public class AttackCollisionTrigger : MonoBehaviour
{
    [Tooltip("Whether the attacking unit is the player or an enemy")]
    public EnumHandler.UnitTypes attackingUnit;

    // Attack that this trigger will be firing off - set automatically at runtime
    AttackScriptableObject attack;
    public AttackScriptableObject GetAttack() { return attack; }
    public void SetAttack(AttackScriptableObject attack) { this.attack = attack; }

    // If the attack has just recently fired and is waiting until it can be used again
    bool onCooldown; 
    public bool GetOnCooldown() { return onCooldown; }
    public void SetOnCooldown(bool set) { onCooldown = set; }

    IEnumerator lockOnTrigger; // Used for detecting lock on mechanism for attacks requiring a lockon
    bool lockedOn; // Set to true when player has aligned the lock on collision for the attack's lockOn duration

    // Used to set corresponding unit's attack manager (these should be inhereted by a baseAttackManager or something similar
    AttackManager attackManager;
    EnemyAttackManager enemyAttackManager;

    void Start()
    {
        SetAttackManager();
    }

    /// <summary>
    /// Sets AttackManager or EnemyAttackManager depending on type of unit. Used for setting attack
    /// </summary>
    void SetAttackManager()
    {
        switch (attackingUnit)
        {
            case EnumHandler.UnitTypes.PLAYER:
                attackManager = GetComponentInParent<AttackManager>();
                break;
            case EnumHandler.UnitTypes.ENEMY:
                enemyAttackManager = GetComponentInParent<EnemyAttackManager>();
                break;
        }
    }

    void Update()
    {
        SetBasicAttack(); // Can maybe move outside of Update eventually? Not 100% sure why this is in Update.  Can maybe move to Awake
    }

    /// <summary>
    /// Sets the main attack for the unit based on the attached BasePlayer or BaseEnemy
    /// </summary>
    void SetBasicAttack()
    {
        if (attack == null)
        {
            switch (attackingUnit) // Setting attack var of this trigger to the attack attached to the enemy or player's basicAttack
            {
                case EnumHandler.UnitTypes.PLAYER:
                    attack = attackManager.GetBasePlayer().GetPrimaryAttack();
                    break;
                case EnumHandler.UnitTypes.ENEMY:
                    attack = enemyAttackManager.GetBaseEnemy().GetAttack();
                    break;
            }
        }
    }

    /// <summary>
    /// Coroutine used to trigger the attack to be fired if lockOn is successful (player keeps collisionTrigger on the enemy for the attack's lockOnTime)
    /// Started during OnTriggerStay and uses lockedOn bool to ensure it is only ran once until the attack's cooldown time has reset
    /// </summary>
    IEnumerator LockOnTrigger()
    {
        yield return new WaitForSeconds(attack.lockOnTime);
        // Debug.Log("Lock on successful, firing attack");
        if (lockedOn) attackManager.OnTrigger(this, attack);
        lockedOn = false;
    }

    /// <summary>
    /// This is fired off any time an object is inside the collision of this object
    /// </summary>
    void OnTriggerStay(Collider collidedObj)
    {
        if (attack != null)
        {
            switch (attack.lockOnTime)
            {
                case 0: // For attacks with no lock on time
                    switch (attackingUnit) // Calls OnTrigger function of attack manager when there is an opposing unit inside this trigger
                    {
                        case EnumHandler.UnitTypes.PLAYER:
                            if (collidedObj.CompareTag("Enemy") || collidedObj.CompareTag("EnemySpawner"))
                            {
                                attackManager.OnTrigger(this, attack);
                            }
                            break;
                        case EnumHandler.UnitTypes.ENEMY:
                            if (collidedObj.CompareTag("PlayerAttackable"))
                            {
                                enemyAttackManager.OnTrigger(this, attack);
                            }
                            break;
                    }
                    break;

                case > 0: // For attacks with lock on time
                    switch (attackingUnit)
                    {
                        case EnumHandler.UnitTypes.PLAYER:
                            if ((collidedObj.CompareTag("Enemy") || collidedObj.CompareTag("EnemySpawner")) && !lockedOn && !onCooldown)
                            {
                                // Start timer for firing attack
                                // Can use Coroutine, and stop coroutine if trigger exit
                                // Debug.Log("Starting lockon for " + attack.lockOnTime + " seconds.");
                                lockedOn = true;
                                lockOnTrigger = LockOnTrigger();
                                StartCoroutine(lockOnTrigger);
                            }
                            break;
                        case EnumHandler.UnitTypes.ENEMY:
                            if (collidedObj.CompareTag("PlayerAttackable"))
                            {
                                // Nothing here yet as no enemy attacks require lockon
                            }
                            break;
                    }
                break;
            }
        }      
    }

    /// <summary>
    /// This is fired off any time an object exits the collision of this object
    /// Primarily used to cancel attacks with lock on time
    /// </summary>
    private void OnTriggerExit(Collider collidedObj)
    {
        if (lockedOn)
        {
            // Debug.Log("Cancelled lock on");
            lockedOn = false;
            StopCoroutine(lockOnTrigger);
        }
    }
}
