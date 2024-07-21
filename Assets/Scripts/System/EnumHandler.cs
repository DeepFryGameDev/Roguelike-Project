// Purpose: Contains all enums for the project to keep them in one organized location
// Directions: Simply call EnumHandler.enum when looking for them in another script
// Other notes:

public static class EnumHandler
{
    /// <summary>
    /// Used to determine if an attack should be fired by the collider touching a target unit, or if the attack should look for the collider touching an attack particle
    /// </summary>
    public enum AttackTypes
    {
        OFFENSIVE,
        DEFENSIVE
    }

    /// <summary>
    /// Used in many different functions to determine which class the player chose
    /// </summary>
    public enum PlayerClasses
    {
        WARRIOR,
        ARCHER,
        MAGE
    }

    /// <summary>
    /// Used by attack particles - FULLANIM is simply one animation and it is destroyed, PROJECTILE will travel until it hits something, ORBIT will rotate around the player continuously.
    /// </summary>
    public enum AttackProjectionTypes
    {
        FULLANIM,
        PROJECTILE,
        ORBIT
    }

    /// <summary>
    /// Used to determine if a unit is the player or if it is an enemy
    /// </summary>
    public enum UnitTypes
    {
        PLAYER,
        ENEMY
    }

    /// <summary>
    /// TOPDOWN is not being used.  BASIC offers full camera control (used while in non-combat scenes).  COMBAT will force the camera to look in the direction the player is facing.
    /// </summary>
    public enum CameraModes
    {
        BASIC,
        COMBAT,
        TOPDOWN
    }

    /// <summary>
    /// Used to determine if the damage for an attack should be calculated as magical or physical
    /// </summary>
    public enum AttackDamageModes
    {
        PHYSICAL,
        MAGICAL
    }

    /// <summary>
    /// Used to determine what type of user feedback UI text should appear in the event of damage being taken (or healed)
    /// </summary>
    public enum DamageTextTypes
    {
        NORMAL,
        CRIT,
        HEAL
    }

    /// <summary>
    /// The slot that equipment should be set into when equipped.
    /// </summary>
    public enum EquipmentSlots
    {
        MAINHAND,
        OFFHAND,
        HELM,
        CHEST,
        HANDS,
        LEGS,
        FEET,
        AMULET,
        RING
    }

    /// <summary>
    /// Will be used to calculate the chance the item drops from an enemy
    /// </summary>
    public enum ItemRarities
    {
        COMMON,
        UNCOMMON,
        RARE,
        EPIC
    }

    /// <summary>
    /// Not yet being used.  Will be used to determine type of damage for various perks/equipment/weaknesses
    /// </summary>
    public enum DamageTypes
    {
        PIERCING,
        SLASHING,
        BLUNT
    }
}
