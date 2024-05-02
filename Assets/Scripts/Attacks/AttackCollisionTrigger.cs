using UnityEngine;

// Purpose: To handle the processing of attack triggers upon collision between player/enemies
// Directions: Attach to any AttackCollision prefab
// Other notes:

public class AttackCollisionTrigger : MonoBehaviour
{
    [Tooltip("Whether the attacking unit is the player or an enemy")]
    public EnumHandler.UnitTypes attackingUnit;

    [Tooltip("Attack that this trigger will be firing off - set at runtime")]
    [ReadOnly] public AttackScriptableObject attack;

    [Tooltip("If the attack has just recently fired and is waiting until it can be used again")]
    [ReadOnly] public bool onCooldown;

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
                    attack = am.playerObj.GetComponent<BasePlayer>().basicAttack;
                    break;
                case EnumHandler.UnitTypes.ENEMY:
                    attack = eam.enemyObj.GetComponent<BaseEnemy>().enemySO.basicAttack;
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
                    if (collidedObj.CompareTag("Enemy"))
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
