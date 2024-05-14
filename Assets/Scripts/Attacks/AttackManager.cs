using System.Collections;
using UnityEngine;

// Purpose: Facilitates management of player's attacks
// Directions: Attach to Player GameObject
// Other notes:

public class AttackManager : MonoBehaviour
{
    // Set automatically to the player's BasePlayer in Start()
    BasePlayer basePlayer; 
    public BasePlayer GetBasePlayer() { return basePlayer; }

    void Start()
    {
        basePlayer = GetComponent<BasePlayer>();
    }

    /// <summary>
    /// Is called when the player's given AttackCollisionTrigger is entered by an enemy
    /// </summary>
    /// <param name="trigger">Collision Trigger for the attack</param>
    /// <param name="attack">Attack that is being processed</param>
    public void OnTrigger(AttackCollisionTrigger trigger, AttackScriptableObject attack)
    {
        if (!trigger.GetOnCooldown())
            StartCoroutine(ProcessAttack(trigger, attack)); 
    }

    /// <summary>
    /// Generates the attacks particles whenever the attack (or attack's trigger more specifically) is not on cooldown
    /// </summary>
    /// <param name="trigger">Collision Trigger for the attack</param>
    /// <param name="attack">Attack that is being processed</param>
    IEnumerator ProcessAttack(AttackCollisionTrigger trigger, AttackScriptableObject attack)
    {
        if (!trigger.GetOnCooldown())
        {
            trigger.SetOnCooldown(true);

            GameObject newParticle = Instantiate(attack.attackParticles[0], transform.position, transform.rotation);

            ParticleCollision pc = newParticle.GetComponent<ParticleCollision>();
            pc.SetAttack(attack);
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

            // Add triggers for all EnemySpawners

            GameObject[] enemySpawners = GameObject.FindGameObjectsWithTag("EnemySpawner");
            foreach (GameObject spawner in enemySpawners)
            {
                ps.trigger.AddCollider(spawner.GetComponent<Collider>());
            }

            //wait for cooldown of attack
            yield return new WaitForSeconds(attack.cooldown);

            // wait for reset period (trying .2 seconds)
            yield return new WaitForSeconds(CombatManager.attackResetPeriod);

            trigger.SetOnCooldown(false);
        }   
    }
}
