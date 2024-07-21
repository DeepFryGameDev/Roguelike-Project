using UnityEngine;

// Purpose: Handles the pathing for orbit attacks
// Directions: Attach to any particle that is using orbit projection
// Other notes:

public class AttackParticleOrbit : MonoBehaviour
{
    float forwardSpeed; // Speed at which the particle moves - is set according to attack projection speed

    float distance = 15; // Distance from the player (should be eventually set in the attack?)

    float yOffset = 1; // Adds to the y (height) value to keep it at player object's level

    AttackManager am; // Used to add to the particle system to the attack manager

    ParticleSystem ps; // Set as the particle system for this particle

    private void Awake()
    {
        // Debug.Log("Set position and orbit distance");

        am = FindAnyObjectByType<AttackManager>();

        ps = GetComponent<ParticleSystem>();

        forwardSpeed = am.GetBasePlayer().GetPrimaryAttack().projectionSpeed;

        AddToOrbitParticles();
    }

    private void Update()
    {
        RotateAround();
    }

    /// <summary>
    /// Simply adds the particle system to the attack manager
    /// Used through the Attack Manager so enemy spawners can set spawned enemies to this particle system's triggers
    /// </summary>
    void AddToOrbitParticles()
    {
        am.AddToOrbitParticles(ps);
    }

    /// <summary>
    /// Is called when the player's given AttackCollisionTrigger is entered by an enemy
    /// </summary>
    void RotateAround()
    {
        Quaternion newRotation = Quaternion.LookRotation(am.GetAttackAnchor().transform.position - transform.position);
        newRotation.z = 0.0f;
        newRotation.x = 0.0f;
        transform.rotation = newRotation;

        transform.position = am.GetAttackAnchor().transform.position;
        transform.position -= transform.rotation * Vector3.forward * distance;
        transform.position += transform.right * forwardSpeed * Time.deltaTime;

        transform.position = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);
    }
}
