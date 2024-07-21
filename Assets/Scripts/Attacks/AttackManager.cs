using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Purpose: Facilitates management of player's attacks
// Directions: Attach to Player GameObject
// Other notes:

public class AttackManager : MonoBehaviour
{
    BasePlayer basePlayer;  // Set automatically to the player's in NewGameSetup
    public BasePlayer GetBasePlayer() { return basePlayer; }
    public void SetBasePlayer(BasePlayer player) { basePlayer = player; }


    List<ParticleSystem> orbitParticles = new List<ParticleSystem>(); // Active Orbit Particles - set from the AttackParticleOrbit script
    public void AddToOrbitParticles(ParticleSystem ps) { orbitParticles.Add(ps); }
    public List<ParticleSystem> GetOrbitParticles() {  return orbitParticles; }


    Transform attackAnchor; // When attack particles are generated, they are set to this transform
    public void SetAttackAnchor(Transform attackAnchor) { this.attackAnchor = attackAnchor; }
    public Transform GetAttackAnchor() { return attackAnchor; }


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
    /// Attack particles instantiated are set as children underneath the attackAnchor transform
    /// </summary>
    /// <param name="trigger">Collision Trigger for the attack</param>
    /// <param name="attack">Attack that is being processed</param>
    IEnumerator ProcessAttack(AttackCollisionTrigger trigger, AttackScriptableObject attack)
    {
        if (!trigger.GetOnCooldown())
        {
            trigger.SetOnCooldown(true);

            GameObject newParticle = Instantiate(attack.attackParticles[0], attackAnchor.position, attackAnchor.rotation, attackAnchor);
            SetupAttackParticle(attack, newParticle);

            attack.GetCooldownRadial().StartTimer(); // Shows the cooldown in UI for user feedback to know when the attack is ready to be fired again

            //wait for cooldown of attack
            yield return new WaitForSeconds(attack.cooldown);

            // wait for reset period (trying .2 seconds)
            yield return new WaitForSeconds(CombatManager.attackResetPeriod);

            trigger.SetOnCooldown(false);
        }
    }

    /// <summary>
    /// Sets attack and colliders for enemies to the attack's particle system triggers
    /// This is how the particles are detected during collision with an enemy
    /// NOTE: If they are not added to the trigger for the particle system, attack collisions won't be registered
    /// </summary>
    /// <param name="attack">Attack to be set to the ParticleCollision script for the particle</param>
    /// <param name="newParticle">The particle GameObject that is instantiated into the world</param>
    public void SetupAttackParticle(AttackScriptableObject attack, GameObject newParticle)
    {
        ParticleCollision pc = newParticle.GetComponent<ParticleCollision>();
        pc.SetAttack(attack);
        pc.sourceUnitType = EnumHandler.UnitTypes.PLAYER;
        pc.SetUnit(basePlayer);

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
    }
}
