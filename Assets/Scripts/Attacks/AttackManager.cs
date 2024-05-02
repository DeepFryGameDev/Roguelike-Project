using System;
using System.Collections;
using UnityEngine;

// Purpose: Facilitates management of player's attacks
// Directions: Attach to Player GameObject
// Other notes:

public class AttackManager : MonoBehaviour
{
    [Tooltip("Global wait time to wait after attack cooldowns before attack can be fired again")] // Also in EnemyAttackManager - can move to a static class eventually
    public float attackResetPeriod = .2f;
    
    [NonSerialized] public GameObject playerObj; // Set to the player's gameObject in Start()

    void Start()
    {
        playerObj = gameObject;
    }

    /// <summary>
    /// Is called when the player's given AttackCollisionTrigger is entered by an enemy
    /// </summary>
    /// <param name="trigger">Collision Trigger for the attack</param>
    /// <param name="attack">Attack that is being processed</param>
    public void OnTrigger(AttackCollisionTrigger trigger, AttackScriptableObject attack)
    {
        if (!trigger.onCooldown)
            StartCoroutine(ProcessAttack(trigger, attack)); 
    }

    /// <summary>
    /// Generates the attacks particles whenever the attack (or attack's trigger more specifically) is not on cooldown
    /// </summary>
    /// <param name="trigger">Collision Trigger for the attack</param>
    /// <param name="attack">Attack that is being processed</param>
    IEnumerator ProcessAttack(AttackCollisionTrigger trigger, AttackScriptableObject attack)
    {
        if (!trigger.onCooldown)
        {
            trigger.onCooldown = true;

            GameObject newParticle = Instantiate(attack.attackParticles[0], playerObj.transform.position, playerObj.transform.rotation);

            ParticleCollision pc = newParticle.GetComponent<ParticleCollision>();
            pc.attack = attack;
            pc.sourceUnitType = EnumHandler.UnitTypes.PLAYER;

            // Add triggers for all enemies in scene
            ParticleSystem ps = newParticle.GetComponent<ParticleSystem>();

            for (int i = 0; i < ps.trigger.colliderCount; i++)
            {
                ps.trigger.SetCollider(i, null);
            }

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                ps.trigger.AddCollider(enemy.GetComponent<Collider>());
            }

            //wait for cooldown of attack
            yield return new WaitForSeconds(attack.cooldown);

            // wait for reset period (trying .2 seconds)
            yield return new WaitForSeconds(attackResetPeriod);

            trigger.onCooldown = false;
        }   
    }
}
