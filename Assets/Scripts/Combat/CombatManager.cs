// Purpose: Contains combat management vars and methods
// Directions: Simply call CombatManager to reference
// Other notes: Some variables are still to be moved here

using UnityEngine;

public static class CombatManager
{
    public static float enemyDespawnTime = 3f; // after an enemy dies, how many seconds pass before the corpse despawns

    public static float baseEnemySpawnDelay = 10f; // Minimum number of seconds to wait before spawning enemy

    public static float enemySpawnDelayMax = 10f; // Maximum number of seconds to wait before spawning enemy (taken into account after base spawn delay)

    public static float attackResetPeriod = .2f; // Global wait time to wait after attack cooldowns before attack can be fired again

    public static float enemyRandomPathingRange = 15f; // Used to determine distance from the enemy unit's current to target position

    public static float enemyPathingToPlayerStoppingDistance = 3.5f; // used to determine where to stop when pathing to player's position

    public static float enemyPathingToRandomStoppingDistance = 0f;  // used to determine where to stop when pathing to a random position

    // For damage calculations

    static int finalDamageCalcOffsetRange = 20; // Final damage is randomly chosen by decreasing or increasing the damage value by this number

    static float strengthToPhysicalDamageMod = .5f;  // When dealing physical damage, for each point of strength, effectiveness of attack is increased by this %

    static float dexterityToPhysicalDamageMod = .65f; // When dealing ranged physical damage, for each point of dexterity, effectiveness is increased by this %

    static float armorToPhysicalDamageMod = .25f; // When receiving physical damage, for each point of armor, effectiveness of attack is reduced by this %

    static float intelligenceToMagicalDamageMod = .65f; // When dealing magical damage, for each point of intelligence, effectiveness of attack is increased by this %

    static float magicResistToMagicalDamageMod = .35f; // When receiving magical damage, for each point of magic resistance, effectiveness of attack is reduced by this %

    static float criticalDamageMod = 2; // Damage is multiplied by this value in the event of a crit

    /// <summary>
    /// Calculates the damage taken from the attack to the target unit
    /// Takes into account all unit parameters
    /// </summary>
    /// <param name="attack">Attack being made</param>
    /// <param name="attackingUnit">Unit that is processing the attack</param>
    /// <param name="targetUnit">Unit that is receiving the attack's damage</param>
    /// <returns>Final Damage to be taken after random offset value</returns>
    public static int CalculateDamage(AttackScriptableObject attack, BaseAttackableUnit attackingUnit, BaseAttackableUnit targetUnit, out EnumHandler.DamageTextTypes textType)
    {
        int damageToReturn = 0;

        textType = EnumHandler.DamageTextTypes.NORMAL;

        switch (attack.attackDamageType)
        {
            case EnumHandler.AttackDamageModes.PHYSICAL:
                return GetFinalDamageAfterOffset(CalculatePhysicalDamageDealt(attack, attackingUnit, targetUnit, out textType));
            case EnumHandler.AttackDamageModes.MAGICAL:
                return GetFinalDamageAfterOffset(CalculateMagicalDamageDealt(attack, attackingUnit, targetUnit));
        }

        return GetFinalDamageAfterOffset(damageToReturn);
    }

    /// <summary>
    /// Calculates damage to be dealt based on the attacking unit's str/dex * the physicalDamageMod
    /// </summary>
    /// <param name="attack">Attack to calculate damage</param>
    /// <param name="attackingUnit">Offensive unit that is firing the attack</param>
    /// <param name="targetUnit">Defensive unit that is receiving the attack</param>
    /// <param name="textType">Outputs the type of text based on if the damage is a crit (which will show larger text)</param>
    /// <returns>Damage after calculating attacking unit's strength/dexterity attribute to the opposing unit's armor</returns>
    static int CalculatePhysicalDamageDealt(AttackScriptableObject attack, BaseAttackableUnit attackingUnit, BaseAttackableUnit targetUnit, out EnumHandler.DamageTextTypes textType)
    {
        Debug.Log("-----");
        Debug.Log("Damage test for attack: " + attack.name + " with damage of " + attack.damage);
        // use attackingUnit's strength to targetUnit's armor

        int damageToReturn;
        int attackerDamageMod = 0;

        textType = EnumHandler.DamageTextTypes.NORMAL;

        switch (attack.attackProjectionType)
        {
            case EnumHandler.AttackProjectionTypes.FULLANIM:
                attackerDamageMod = Mathf.RoundToInt(attackingUnit.GetStrength() * strengthToPhysicalDamageMod);
                Debug.Log("Attacking unit's Strength: " + attackingUnit.GetStrength() + " * strengthToPhysicalDamageMod " + strengthToPhysicalDamageMod + " = " + attackerDamageMod);
                break;
            case EnumHandler.AttackProjectionTypes.PROJECTILE:
                attackerDamageMod = Mathf.RoundToInt(attackingUnit.GetDexterity() * dexterityToPhysicalDamageMod);
                Debug.Log("Attacking unit's Dexterity: " + attackingUnit.GetDexterity() + " * dexterityToPhysicalDamageMod " + dexterityToPhysicalDamageMod + " = " + attackerDamageMod);
                break;
        }        

        int targetDamageBlockedMod = Mathf.RoundToInt(targetUnit.GetArmor() * armorToPhysicalDamageMod);
        Debug.Log("Target unit's Armor: " + targetUnit.GetArmor() + " * armorToPhysicalDamageMod " + armorToPhysicalDamageMod + " = " + targetDamageBlockedMod);

        Debug.Log("Attacking damage: " + (attack.damage * attackerDamageMod) + " - target's blocked: " + (attack.damage * targetDamageBlockedMod));

        damageToReturn = (attack.damage * attackerDamageMod) - (attack.damage * targetDamageBlockedMod);

        BasePlayer checkPlayerUnit = attackingUnit as BasePlayer;

        if (checkPlayerUnit != null) // Is a player unit
        {
            // Debug.Log("Damage to return before crit check: " + damageToReturn);

            if (IfAttackIsCrit(attackingUnit))
            {
                damageToReturn = Mathf.RoundToInt(damageToReturn * criticalDamageMod);
                textType = EnumHandler.DamageTextTypes.CRIT;
            }

            // Debug.Log("Damage to return after crit check: " + damageToReturn);
        }
        
        if (attackingUnit.GetType() == typeof(BaseEnemy))
        {
            Debug.Log("Damage to return: " + damageToReturn);
        }

        Debug.Log("-----");
        return damageToReturn;
    }

    /// <summary>
    /// Calculates damage to be dealt based on the attacking unit's int * the magicalDamageMod
    /// </summary>
    /// <param name="attack">Attack to calculate damage</param>
    /// <param name="attackingUnit">Offensive unit that is firing the attack</param>
    /// <param name="targetUnit">Defensive unit that is receiving the attack</param>
    /// <returns>Damage after calculating attacking unit's intelligence attribute to the opposing unit's magic resist</returns>
    static int CalculateMagicalDamageDealt(AttackScriptableObject attack, BaseAttackableUnit attackingUnit, BaseAttackableUnit targetUnit)
    {
        Debug.Log("-----");
        Debug.Log("Damage test for attack: " + attack.name + " with damage of " + attack.damage);
        // use intellect

        int damageToReturn;
        int attackerDamageMod = Mathf.RoundToInt(attackingUnit.GetIntelligence() * intelligenceToMagicalDamageMod);
        Debug.Log("Attacking unit's BaseIntelligence: " + attackingUnit.GetIntelligence() + " * intelligenceToMagicalDamageMod " + intelligenceToMagicalDamageMod + " = " + attackerDamageMod);

        int targetDamageBlockedMod = Mathf.RoundToInt(targetUnit.GetMagicResist() * magicResistToMagicalDamageMod);
        Debug.Log("Target unit's MagicResist: " + targetUnit.GetMagicResist() + " * magicResistToMagicalDamageMod " + magicResistToMagicalDamageMod + " = " + targetDamageBlockedMod);

        Debug.Log("Attacking damage: " + (attack.damage * attackerDamageMod) + " - target's blocked: " + (attack.damage * targetDamageBlockedMod));

        damageToReturn = (attack.damage * attackerDamageMod) - (attack.damage * targetDamageBlockedMod);

        Debug.Log("Damage to return: " + damageToReturn);

        Debug.Log("-----");
        return damageToReturn;
    }

    /// <summary>
    /// Simply gathers a random value between -offset and +offset value, and applies it to damageTaken.
    /// If final value is less than 0, returns 0
    /// </summary>
    /// <param name="damageTaken">Damage to calculate offset from</param>
    /// <returns>A random value between -offset and +offset starting from damageTaken</returns>
    static int GetFinalDamageAfterOffset(int damageTaken)
    {
        int calcDamage = Random.Range(-finalDamageCalcOffsetRange, finalDamageCalcOffsetRange);

        int calc = damageTaken + calcDamage;

        if (calc < 0)
        {
            return 0;
        }
        else
        {
            return calc;
        }
    }

    /// <summary>
    /// Gathers a random value between 1 and 100 and tests if the player's crit chance is within that range. The higher the crit chance, the more likely this returns true.
    /// </summary>
    /// <param name="attackingUnit">Offensive unit that should be checked for crit chance</param>
    /// <returns>True if the random value is either 100, or within the range of 100 - player's crit chance</returns>
    static bool IfAttackIsCrit(BaseAttackableUnit attackingUnit)
    {
        BasePlayer playerUnit = attackingUnit as BasePlayer;

        Debug.Log("Checking crit with chance of: " + playerUnit.GetCritChance() + " - Needs a roll of " + (100 - playerUnit.GetCritChance()) + " or higher");

        float rand = Random.Range(1, 100);

        if (rand >= 100 - playerUnit.GetCritChance() || rand == 100)
        {
            Debug.Log("Roll: " + rand + " - Crit!");
            return true;
        }

        Debug.Log("Roll: " + rand + " - Did not crit.");
        return false;
    }
}
