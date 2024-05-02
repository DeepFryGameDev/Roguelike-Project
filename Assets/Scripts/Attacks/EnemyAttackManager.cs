using System;
using System.Collections;
using UnityEngine;

// Purpose: Controls the attached enemy's attacking mechanisms
// Directions: Attach to all enemy GameObjects
// Other notes:

public class EnemyAttackManager : MonoBehaviour
{
    [Tooltip("Global wait time to wait after attack cooldowns before attack can be fired again")] // Also in AttackManager - can move to a static class eventually
    public float attackResetPeriod = .2f;

    [Tooltip("Set to the parent transform for the enemy's Attack Collision Trigger")]
    public Transform attackCollisionTriggerTransform;

    [Tooltip("Set to the transform for where the enemy should be firing the attack from")]
    public Transform attackParticleSource;

    [NonSerialized] public GameObject enemyObj; // This is the gameObject for the enemy.  Will need to probably replace this eventually.

    void Start()
    {
        enemyObj = gameObject;
    }
    
    /// <summary>
    /// Is called when the enemy's given AttackCollisionTrigger is entered by the player
    /// </summary>
    /// <param name="trigger">Collision Trigger for the attack</param>
    /// <param name="attack">Attack that is being processed</param>
    public void OnTrigger(AttackCollisionTrigger trigger, AttackScriptableObject attack)
    {
        if (!trigger.onCooldown)
        {
            StartCoroutine(ProcessAttack(trigger, attack));
        }            
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

            GameObject newParticle = Instantiate(attack.attackParticles[0], attackParticleSource.position, enemyObj.transform.rotation);

            ParticleCollision pc = newParticle.GetComponent<ParticleCollision>();
            pc.attack = attack;
            pc.sourceUnitType = EnumHandler.UnitTypes.ENEMY;

            // Add trigger for player
            ParticleSystem ps = newParticle.GetComponent<ParticleSystem>();
            
            for (int i=0; i < ps.trigger.colliderCount; i++)
            {
                ps.trigger.SetCollider(i, null);
            }

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            ps.trigger.SetCollider(0, player.transform.GetChild(0).GetComponent<Collider>());

            //wait for cooldown of attack
            yield return new WaitForSeconds(attack.cooldown);

            // wait for reset period (trying .2 seconds)
            yield return new WaitForSeconds(attackResetPeriod);

            trigger.onCooldown = false;
        }
    }
}
