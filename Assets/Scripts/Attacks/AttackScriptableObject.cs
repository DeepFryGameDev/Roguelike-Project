using UnityEngine;

// Purpose: Base Scriptable Object for each attack used by both player and enemies
// Directions: Set up as needed and save the file to Resources/AttackScriptableObjects
// Other notes: There should be new AttackScriptableObjects created that inherit this class.  To be done.

[CreateAssetMenu(fileName = "NewAttack", menuName = "Attacks/New Attack", order = 1)]
public class AttackScriptableObject : ScriptableObject
{
    [Tooltip("Name of the attack to be displayed in game")]
    public string attackName;

    [Tooltip("Level of the attack")]
    public int level;

    [Tooltip("Description of the attack to be displayed in UI")]
    public string description;

    [Tooltip("Offensive attacks are fired when their collisionTrigger hits an opposing target, Defensive attacks are fired when an opposing attack is received")]
    public EnumHandler.AttackTypes attackType;

    [Tooltip("Attack deals this type of damage")]
    public EnumHandler.AttackDamageModes attackDamageType;

    [Tooltip("How long the attack needs to wait before it can be used again")]
    public float cooldown;

    [Tooltip("The image of the icon to be displayed on the cooldown panel")]
    public Sprite icon;

    [Tooltip("Base damage dealt")]
    public int damage;

    //---

    [Tooltip("- FULLANIM will cause the particle to stay in place \n" +
        "- PROJECTILE will cause the particle to be fired in the direction of the attacker, based on projectionSpeed \n" +
        "- ORBIT will cause the particle to permanently orbit the player")]
    public EnumHandler.AttackProjectionTypes attackProjectionType;

    [Tooltip("Particles to be created when the attack is ready to be used")]
    public GameObject[] attackParticles;

    [Tooltip("Secondary particles used for cosmetic effect")]
    public GameObject[] cosmeticParticles;

    [Tooltip("How fast the animation of the particle animates - FULLANIM will typically use '1' \n" +
        "PROJECTILE and ORBIT attacks use this to determine movement speed")]
    public float projectionSpeed = 1;

    [Tooltip("How long the particle is active before being destroyed in the world")]
    public float projectionTime;

    [Tooltip("How long the collider needs to touch the enemy before firing.  If set to 0, attack will fire immediately when colliders intersect")]
    public float lockOnTime;

    [Tooltip("This object will attach itself to the attacker to check collisions for the attack to be fired.")]
    public GameObject collisionTrigger;

    CooldownRadial cooldownRadial; // The cooldown radial UI object to be displayed so the player knows when this attack is on cooldown
    public void SetCooldownRadial(CooldownRadial cooldownRadial) { this.cooldownRadial = cooldownRadial; }
    public CooldownRadial GetCooldownRadial() { return cooldownRadial; }
}
