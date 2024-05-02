// Purpose: Contains all enums for the project to keep them in one organized location
// Directions: Simply call EnumHandler.enum when looking for them in another script
// Other notes:

public static class EnumHandler
{
    public enum AttackTypes
    {
        OFFENSIVE,
        DEFENSIVE
    }

    public enum PlayerClasses
    {
        WARRIOR,
        ARCHER,
        MAGE
    }

    public enum AttackProjectionTypes
    {
        FULLANIM,
        PROJECTILE
    }

    public enum UnitTypes
    {
        PLAYER,
        ENEMY
    }

    public enum CameraModes
    {
        BASIC,
        COMBAT,
        TOPDOWN
    }
}
