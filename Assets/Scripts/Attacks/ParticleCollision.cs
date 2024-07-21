using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Purpose: Handles projectile particle collision and movement
// Directions: Attach to any attack projectile prefab in /Particles folder
// Other notes: Attached to every particle to be checked for collision for an attack made by an enemy or player

public class ParticleCollision : MonoBehaviour
{
    [Tooltip("Attacker's type - player or enemy")]
    public EnumHandler.UnitTypes sourceUnitType;

    BaseAttackableUnit unit; // The player or enemy unit that the attack particle is spawned from
    public void SetUnit(BaseAttackableUnit unit) { this.unit = unit; }

    AttackScriptableObject attack; // Is set to the attack when the particle is generated by the unit's AttackManager
    public void SetAttack(AttackScriptableObject attack) { this.attack = attack; }

    bool selfDestructing; // set to true automatically when the particle is created, used for StartSelfDestruct()

    int playerLayerMask = 1 << 11; // set to the player's layer (layerPlayer)
    int enemyLayerMask = 1 << 10; // set to the enemy's layer (layerAttackable)

    float collisionRadius = 5f; // As particle collisions use a lot of memory, and trigger collisions cannot detect the colliding object,
                                // this is used with Physics.OverlapSphereNonAlloc for trigger when colliding objects.

    List<GameObject> collidedTargets = new List<GameObject>(); // Is used to ensure the same target is not applied twice

    void Update()
    {
        if (attack.attackProjectionType == EnumHandler.AttackProjectionTypes.PROJECTILE) // If attack is a projectile, this shoots the particle in a forward direction from the source unit
        {
            transform.position += transform.forward * Time.deltaTime * attack.projectionSpeed;
        }

        SelfDestructParticle(); // Starts timer for destroying the particle
    }

    /// <summary>
    /// Destroys the gameobject for the particle after attack's projectionTime has lapsed
    /// </summary>
    void SelfDestructParticle()
    {
        if (attack != null && attack.attackProjectionType != EnumHandler.AttackProjectionTypes.ORBIT && !selfDestructing)
        {
            //Debug.Log("Self destructing " + gameObject.name + " after " + attack.projectionTime + " seconds");
            StartCoroutine(StartSelfDestruct());
        }
    }

    /// <summary>
    /// Triggers on callback from any ParticleSystem > Trigger
    /// This method checks for potential targets, and applies damage to any targets hit by the particle based on the source unit type
    /// </summary>
    void OnParticleTrigger()
    {
        int maxColliders = 20;
        Collider[] hitColliders = new Collider[maxColliders];

        if (sourceUnitType == EnumHandler.UnitTypes.PLAYER) // For attacks being processed from the player to an enemy
        {
            int numOfCollisions = Physics.OverlapSphereNonAlloc(transform.position, collisionRadius, hitColliders, enemyLayerMask);

            EnumHandler.DamageTextTypes textType = EnumHandler.DamageTextTypes.NORMAL;

            for (int i = 0; i < numOfCollisions; i++)
            {
                if (!collidedTargets.Contains(hitColliders[i].gameObject))
                {
                    collidedTargets.Add(hitColliders[i].gameObject);

                    switch (hitColliders[i].tag)
                    {
                        case "Enemy":                            
                            int enemyDamageToTake = CombatManager.CalculateDamage(attack, unit, hitColliders[i].GetComponent<BaseEnemy>(), out textType);

                            Debug.Log("Player attacks " + hitColliders[i].GetComponent<BaseEnemy>().name + "!");
                            Debug.Log("Dealing " + enemyDamageToTake + " damage to " + hitColliders[i].gameObject.name);

                            hitColliders[i].GetComponent<BaseEnemy>().TakeDamage(enemyDamageToTake, textType);

                            break;

                        case "EnemySpawner":
                            int damageToTake = CombatManager.CalculateDamage(attack, unit, hitColliders[i].GetComponent<EnemySpawner>(), out textType);

                            Debug.Log("Player attacks " + hitColliders[i].GetComponent<EnemySpawner>().name + "!");

                            Debug.Log("Dealing " + damageToTake + " damage to " + hitColliders[i].gameObject.name);

                            hitColliders[i].GetComponent<EnemySpawner>().TakeDamage(damageToTake);

                            break;
                    }

                    
                    if (attack.attackProjectionType == EnumHandler.AttackProjectionTypes.ORBIT)
                    {
                        // Start coroutine to wait for attack cooldown duration, and then remove hitColliders[i] from collidedTargets
                        StartCoroutine(ClearTargetFromOrbit(hitColliders[i].gameObject));
                    }

                    if (attack.attackProjectionType == EnumHandler.AttackProjectionTypes.PROJECTILE)
                    {
                        Destroy(gameObject);
                    }
                }                
            }
        }

        if (sourceUnitType == EnumHandler.UnitTypes.ENEMY) // For attacks being processed from an enemy to the player
        {
            int numOfCollisions = Physics.OverlapSphereNonAlloc(transform.position, 5f, hitColliders, playerLayerMask);

            for (int i = 0; i < numOfCollisions; i++)
            {
                if (!collidedTargets.Contains(hitColliders[i].transform.parent.gameObject))
                {
                    collidedTargets.Add(hitColliders[i].transform.parent.gameObject);
                    BasePlayer playerUnit = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetComponent<BasePlayer>();

                    EnumHandler.DamageTextTypes textType = EnumHandler.DamageTextTypes.NORMAL;

                    int damageToTake = CombatManager.CalculateDamage(attack, unit, playerUnit, out textType);

                    Debug.Log("Enemy attacks the player!");
                    Debug.Log("Dealing " + damageToTake + " damage to " + hitColliders[i].transform.parent.gameObject.name);

                    playerUnit.TakeDamage(damageToTake);

                    if (attack.attackProjectionType == EnumHandler.AttackProjectionTypes.PROJECTILE)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

    /// <summary>
    /// For orbit attacks, after attack's cooldown duration, the target is removed from the collidedTargets
    /// This is calculated separately from other attack types
    /// </summary>
    /// <param name="targetToRemove"></param>
    IEnumerator ClearTargetFromOrbit(GameObject targetToRemove)
    {
        yield return new WaitForSeconds(attack.cooldown);

        collidedTargets.Remove(targetToRemove);
    }

    /// <summary>
    /// Called immediately when the particle gameObject is created in the world.  This will ensure the particle is only in the world for it's projectionTime
    /// </summary>
    IEnumerator StartSelfDestruct()
    {
        selfDestructing = true;
        yield return new WaitForSeconds(attack.projectionTime);
        Destroy(gameObject);
    }
}
