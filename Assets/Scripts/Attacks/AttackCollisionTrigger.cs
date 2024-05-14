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

    // Used to set corresponding unit's attack manager
    AttackManager am;
    EnemyAttackManager eam;

    void Start()
    {
        SetAttackManager();
    }

    /// <summary>
    /// Sets AttackManager or EnemyAttackManager depending on type of unit
    /// </summary>
    void SetAttackManager()
    {
        switch (attackingUnit)
        {
            case EnumHandler.UnitTypes.PLAYER:
                am = GetComponentInParent<AttackManager>();
                break;
            case EnumHandler.UnitTypes.ENEMY:
                eam = GetComponentInParent<EnemyAttackManager>();
                break;
        }
    }

    void Update()
    {
        SetBasicAttack();
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
                    attack = am.GetBasePlayer().GetBasicAttack();
                    break;
                case EnumHandler.UnitTypes.ENEMY:
                    attack = eam.GetBaseEnemy().GetEnemy().basicAttack;
                    break;
            }
        }
    }

    /// <summary>
    /// This is fired off any time an object is inside the collision of this object
    /// </summary>
    void OnTriggerStay(Collider collidedObj)
    {
        if (attack != null)
        {
            switch (attackingUnit) // Calls OnTrigger function of attack manager when there is an opposing unit inside this trigger
            {
                case EnumHandler.UnitTypes.PLAYER:
                    if (collidedObj.CompareTag("Enemy") || collidedObj.CompareTag("EnemySpawner"))
                    {
                        am.OnTrigger(this, attack);
                    }

                    break;
                case EnumHandler.UnitTypes.ENEMY:
                    if (collidedObj.CompareTag("Player"))
                    {
                        eam.OnTrigger(this, attack);
                    }
                    break;
            }
        }
    }
}
