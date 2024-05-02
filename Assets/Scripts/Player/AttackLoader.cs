using UnityEngine;

// Purpose: To "equip" attacks to the player to be used for combat
// Directions: Attach to [System] GameObject and use SetAttacks() to load the basic attacks for each class
// Other notes:

public class AttackLoader : MonoBehaviour
{
    [Tooltip("Set to attack collisions parent transform on the player - Used to instantiate attack collisions for the player")]
    public Transform attackCollisionTriggerTransform;

    [Tooltip("Set to BasePlayer class on the player - Used to set the player's basic attack")]
    public BasePlayer player;
    
    string attackResourcesPath = "AttackScriptableObjects/"; // Points to the path in the resources folder which Attack Scriptable Objects are held
    
    PlayerManager pm; // Used to gather the player's class

    void Start()
    {
        pm = FindObjectOfType<PlayerManager>();

        SetAttacks();
    }

    /// <summary>
    /// Sets the player's basic attack to the given Scriptable Object in Resources folder
    /// Also sets CollisionTriggers for the given attack
    /// </summary>
    void SetAttacks()
    {
        switch (pm.playerClass)
        {
            case EnumHandler.PlayerClasses.WARRIOR:
                player.basicAttack = Resources.Load<AttackScriptableObject>(attackResourcesPath + "TestAttack");
                SetCollisionTriggers(player.basicAttack.collisionTrigger);
                break;
        }
    }

    /// <summary>
    /// Generates the attacks particles whenever the attack (or attack's trigger more specifically) is not on cooldown
    /// </summary>
    /// <param name="triggerObj">The Collision Trigger Prefab to be instantiated</param>
    void SetCollisionTriggers(GameObject triggerObj)
    {
        Instantiate(triggerObj, attackCollisionTriggerTransform);
    }
}
