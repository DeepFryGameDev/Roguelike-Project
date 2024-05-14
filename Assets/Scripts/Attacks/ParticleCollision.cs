using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Purpose: Handles projectile particle collision and movement
// Directions: Attach to any attack projectile prefab in /Particles folder
// Other notes:Attached to every particle to be checked for collision for an attack made by an enemy or player

public class ParticleCollision : MonoBehaviour
{
    [Tooltip("Attacker's type - player or enemy")]
    public EnumHandler.UnitTypes sourceUnitType;

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
        if (attack.attackProjectionType == EnumHandler.AttackProjectionTypes.PROJECTILE)
        {
            transform.position += transform.forward * Time.deltaTime * attack.projectionSpeed;
        }

        if (attack != null && !selfDestructing)
        {
            //Debug.Log("Self destructing " + gameObject.name + " after " + attack.projectionTime + " seconds");
            StartCoroutine(StartSelfDestruct());
        }
    }

    /// <summary>
    /// Triggers on callback from any ParticleSystem > Trigger
    /// This method specifically checks for sourceUnitType, and then checks for potential targets, and applies damage to any targets hit by the particle
    /// </summary>
    void OnParticleTrigger()
    {
        int maxColliders = 20;
        Collider[] hitColliders = new Collider[maxColliders];

        if (sourceUnitType == EnumHandler.UnitTypes.PLAYER)
        {
            int numOfCollisions = Physics.OverlapSphereNonAlloc(transform.position, collisionRadius, hitColliders, enemyLayerMask);

            for (int i = 0; i < numOfCollisions; i++)
            {
                if (!collidedTargets.Contains(hitColliders[i].gameObject))
                {
                    collidedTargets.Add(hitColliders[i].gameObject);

                    switch (hitColliders[i].tag)
                    {
                        case "Enemy":

                            Debug.Log("Player attacks " + hitColliders[i].GetComponent<BaseEnemy>().GetEnemy().name + "!");
                            Debug.Log("Dealing " + attack.damage + " damage to " + hitColliders[i].gameObject.name);

                            hitColliders[i].GetComponent<BaseEnemy>().TakeDamage(attack.damage);

                            break;

                        case "EnemySpawner":
                            Debug.Log("Player attacks " + hitColliders[i].GetComponent<EnemySpawner>().name + "!");
                            Debug.Log("Dealing " + attack.damage + " damage to " + hitColliders[i].gameObject.name);

                            hitColliders[i].GetComponent<EnemySpawner>().TakeDamage(attack.damage);

                            break;
                    }
                    

                    if (attack.attackProjectionType == EnumHandler.AttackProjectionTypes.PROJECTILE)
                    {
                        Destroy(gameObject);
                    }
                }                
            }
        }

        if (sourceUnitType == EnumHandler.UnitTypes.ENEMY)
        {
            int numOfCollisions = Physics.OverlapSphereNonAlloc(transform.position, 5f, hitColliders, playerLayerMask);

            for (int i = 0; i < numOfCollisions; i++)
            {
                if (!collidedTargets.Contains(hitColliders[i].transform.parent.gameObject))
                {
                    collidedTargets.Add(hitColliders[i].transform.parent.gameObject);

                    Debug.Log("Enemy attacks the player!");
                    Debug.Log("Dealing " + attack.damage + " damage to " + hitColliders[i].transform.parent.gameObject.name);

                    hitColliders[i].transform.parent.GetComponent<BasePlayer>().TakeDamage(attack.damage);

                    if (attack.attackProjectionType == EnumHandler.AttackProjectionTypes.PROJECTILE)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
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
