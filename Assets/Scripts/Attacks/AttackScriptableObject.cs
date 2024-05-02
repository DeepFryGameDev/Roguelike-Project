using UnityEngine;

// Purpose: Base Scriptable Object for each attack used by both player and enemies
// Directions: Set up as needed and save the file to Resources/AttackScriptableObjects
// Other notes:

[CreateAssetMenu(fileName = "NewAttack", menuName = "Attacks/New Attack", order = 1)]
public class AttackScriptableObject : ScriptableObject
{
    [Tooltip("Name of the attack to be displayed in game")]
    public string attackName;

    [Tooltip("Name of the attack scriptableObject created in 'Resources/Attack Scriptable Objects' folder")]
    public string pathName;

    [Tooltip("Level of the attack")]
    public int level;

    [Tooltip("Offensive attacks are fired when their collisionTrigger hits an opposing target, Defensive attacks are fired when an opposing attack is received")]
    public EnumHandler.AttackTypes attackType;

    [Tooltip("How long the attack needs to wait before it can be used again")]
    public int cooldown;

    [Tooltip("Base damage dealt")]
    public int damage;

    //---

    [Tooltip("FULLANIM will cause the particle to stay in place, PROJECTILE will cause the particle to be fired in the direction of the attacker, based on projectionSpeed")]
    public EnumHandler.AttackProjectionTypes attackProjectionType;

    [Tooltip("Particles to be created when the attack is ready to be used")]
    public GameObject[] attackParticles;

    [Tooltip("Secondary particles used for cosmetic effect")]
    public GameObject[] cosmeticParticles;

    [Tooltip("How fast the animation of the particle animates - FULLANIM will typically use '1', but PROJECTILE attacks use this to determine movement speed")]
    public float projectionSpeed = 1;

    [Tooltip("How long the particle is active before being destroyed in the world")]
    public float projectionTime;

    [Tooltip("This object will attach itself to the attacker to check collisions for the attack to be fired.")]
    public GameObject collisionTrigger;
}
