using UnityEngine;

// Purpose: To set up attacks to the player to be used for combat
// Directions: Attach to [System] GameObject and use SetAttacks() to load the basic attacks for each class
// Other notes:

public class AttackLoader : MonoBehaviour
{
    public static AttackLoader instance; // Used for singleton

    Transform attackCollisionTriggerTransform; // Set to attack collisions parent transform on the player - Used to instantiate attack collisions for the player
    public void SetAttackCollisionTriggerTransform(Transform attackCollisionTriggerTransform) { this.attackCollisionTriggerTransform = attackCollisionTriggerTransform; }

    BasePlayer player; // Set to BasePlayer class on the player - Used to set the player's basic attack
    public void SetPlayer(BasePlayer player) { this.player = player; }
    
    PlayerManager pm; // Used to gather the player's class

    AttackManager am; // For orbit player attacks, instantiates and runs setup on the attack particle

    Transform orbitTransform; // Sets the parent transform for orbit attacks to this transform, to keep position of the orbit from updating with player's rotation

    void Start()
    {
        pm = FindObjectOfType<PlayerManager>();

        am = FindObjectOfType<AttackManager>();

        // Might need to set OrbitTransform height
    }

    void Awake()
    {
        Singleton();
    }

    void Singleton()
    {
        if (instance == null) //check if instance exists
        {
            instance = this; //if not set the instance to this
        }
        else if (instance != this) //if it exists but is not this instance
        {
            Destroy(gameObject); //destroy it
        }
        DontDestroyOnLoad(gameObject); //set this to be persistable across scenes
    }

    /// <summary>
    /// Sets the player's basic attack to the given Scriptable Object in Resources folder
    /// Also sets CollisionTriggers for the given attack
    /// </summary>
    public void SetAttackCollisionTriggers()
    {
        GenerateCollisionTriggers(player.GetPrimaryAttack().collisionTrigger);
    }

    /// <summary>
    /// Generates the attacks particles whenever the attack (or attack's trigger more specifically) is not on cooldown
    /// </summary>
    /// <param name="triggerObj">The Collision Trigger Prefab to be instantiated</param>
    void GenerateCollisionTriggers(GameObject triggerObj)
    {
        //Debug.Log("Setting collision trigger for " + triggerObj.name);
        Instantiate(triggerObj, attackCollisionTriggerTransform);
    }

    /// <summary>
    /// Instantiates orbit particle as it is rendered automatically
    /// </summary>
    public void InstantiateOrbitParticles()
    {
        if (player.GetPrimaryAttack().attackProjectionType == EnumHandler.AttackProjectionTypes.ORBIT)
        {
            Debug.Log("Instantiating orbit particles");

            orbitTransform = GameObject.Find("[OrbitTransform]").transform;

            GameObject newParticle = Instantiate(player.GetPrimaryAttack().attackParticles[0], transform.position, transform.rotation, orbitTransform);

            am.SetupAttackParticle(player.GetPrimaryAttack(), newParticle);
        }        
    }

    /// <summary>
    /// Clears any orbit particles that are currently instantiated.  Not yet being used
    /// </summary>
    public void ClearOrbitParticles()
    {
        //Debug.Log("Clear orbit particles");
    }
}
