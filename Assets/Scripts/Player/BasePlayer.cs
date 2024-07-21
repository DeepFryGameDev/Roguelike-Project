using UnityEngine;

// Purpose: Controls all of the player's base stats and basic functions
// Directions: Attach to Player GameObject
// Other notes: Several overrides are used here to separate the calculations for player and enemy stats. These methods are defined in the 'Get Attribute/stat values' region

public class BasePlayer : BaseAttackableUnit
{
    // -- HP --
    int baseHP = 100; // Base health before modifying with Endurance
    float enduranceToHPMod = .375f; // Used to calculate maximum HP based on the player's Endurance

    // -- Stamina --
    float baseStamina = 100; // Base Max Stamina for the player - taken into account before class stats - used to determine sprinting duration
    float enduranceToStaminaMod = .25f; // Used to calculate maximum stamina based on the player's Endurance

    // -- Crit --
    float baseCritChance = 1; // Before taking player's agility into account, the flat % to crit is this value
    float baseAgilityToCritChanceMod = 2.5f; // For each point of agility, crit chance goes up by this amount

    float currentStamina; // Used to track what the player's current stamina at any given point is
    public float GetCurrentStamina() { return currentStamina; }
    public void SetCurrentStamina(float stamina) { currentStamina = stamina;}

    float resistToArmorMod = .5f; // Makes armor more effective based on player's resistance

    float resistToMagicResistMod = .5f; // Makes magic resist more effective based on player's resistance

    // -- Secondary stats

    #region Equipment

    // Houses all of the currently equipped items on the player

    BaseMainHandEquipment equippedMainHand;
    public void SetEquippedMainHand(BaseMainHandEquipment mainHand) { this.equippedMainHand = mainHand; }
    public BaseMainHandEquipment GetEquippedMainHand() { return equippedMainHand; }

    BaseOffHandEquipment equippedOffHand;
    public void SetEquippedOffHand(BaseOffHandEquipment offHand) { this.equippedOffHand = offHand; }
    public BaseOffHandEquipment GetEquippedOffHand() { return equippedOffHand; }

    BaseAmuletEquipment equippedAmulet;
    public void SetEquippedAmulet(BaseAmuletEquipment amulet) { this.equippedAmulet = amulet; }
    public BaseAmuletEquipment GetEquippedAmulet() { return equippedAmulet; }

    BaseRingEquipment equippedRingOne;
    public void SetEquippedRingOne(BaseRingEquipment ringOne) { this.equippedRingOne = ringOne; }
    public BaseRingEquipment GetEquippedRingOne() { return equippedRingOne; }

    BaseRingEquipment equippedRingTwo;
    public void SetEquippedRingTwo(BaseRingEquipment ringTwo) { this.equippedRingTwo = ringTwo; }
    public BaseRingEquipment GetEquippedRingTwo() { return equippedRingTwo; }

    BaseHelmEquipment equippedHelm;
    public void SetEquippedHelm(BaseHelmEquipment helm) { this.equippedHelm = helm; }
    public BaseHelmEquipment GetEquippedHelm() { return equippedHelm; }

    BaseChestEquipment equippedChest;
    public void SetEquippedChest(BaseChestEquipment chest) { this.equippedChest = chest; }
    public BaseChestEquipment GetEquippedChest() { return equippedChest; }

    BaseHandEquipment equippedHands;
    public void SetEquippedHands(BaseHandEquipment hands) { this.equippedHands = hands; }
    public BaseHandEquipment GetEquippedHands() { return equippedHands; }

    BaseLegsEquipment equippedLegs;
    public void SetEquippedLegs(BaseLegsEquipment legs) { this.equippedLegs = legs; }
    public BaseLegsEquipment GetEquippedLegs() { return equippedLegs; }

    BaseFeetEquipment equippedFeet;
    public void SetEquippedFeet(BaseFeetEquipment feet) { this.equippedFeet = feet; }
    public BaseFeetEquipment GetEquippedFeet() { return equippedFeet; }

    #endregion

    // Attacks
    AttackScriptableObject[] secondaryAttacks; // Not yet used, will be for attacks not tied to the player's weapon that they can equip
        
    PlayerManager pm; // Used to handle stamina and health

    /// <summary>
    /// All inherited classes should call base.Awake() in 'protected override void Awake()'
    /// </summary>
    protected virtual void Awake()
    {
        SetUp();
    }

    /// <summary>
    /// Sets base stats and attributes for the player
    /// </summary>
    protected void SetUp()
    {
        pm = FindObjectOfType<PlayerManager>();

        //if (pm != null) { Debug.Log("PM is set!"); } else { Debug.Log("PM is null!"); }
    }

    /// <summary>
    /// Used to calculate the player's maximum health points
    /// </summary>
    /// <returns>Value by rounding the result of baseHP * the product of player's endurance and enduranceToHP modifier</returns>
    public int GetMaxHP()
    {
        return Mathf.RoundToInt(baseHP * (GetEndurance() * enduranceToHPMod));
    }

    /// <summary>
    /// Sets current HP to player's max HP
    /// </summary>
    public void UpdateHPForMax()
    {
        currentHP = GetMaxHP();
    }

    /// <summary>
    /// Used to calculate the player's maximum stamina
    /// </summary>
    /// <returns>Value calculated by baseStamina * (player's endurance * enduranceToStamina modifier)</returns>
    public float GetMaxStamina()
    {
        return (baseStamina * (GetEndurance() * enduranceToStaminaMod));
    }

    /// <summary>
    /// Simply sets current stamina to player's max stamina
    /// </summary>
    public void UpdateStaminaForMax()
    {
        currentStamina = GetMaxStamina();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public float GetCritChance()
    {
        float critChance = (baseCritChance + (GetAgility() * baseAgilityToCritChanceMod));
        if (critChance > 100) { return 100; }

        return critChance;
    }

    /// <summary>
    /// Lower's players health points by the provided value
    /// </summary>
    /// <param name="damage">Amount of health points to be decreased</param>
    public void TakeDamage(int damage)
    {
        Debug.Log("Player HP before damage: " + GetCurrentHP());
        currentHP -= damage;

        Debug.Log("Player HP after damage: " + GetCurrentHP());

        pm.UpdateHealthBar();

        if (GetCurrentHP() <= 0)
        {
            // die
            Die();
        }
    }

    /// <summary>
    /// Raises players health points by the provided value
    /// </summary>
    /// <param name="healthToHeal">Amount of health points to be replenished</param>
    public void Heal(int healthToHeal)
    {
        currentHP += healthToHeal;

        if (currentHP >= GetMaxHP())
        {
            currentHP = GetMaxHP();
        }

        pm.UpdateHealthBar();
    }

    /// <summary>
    /// Should result in asking the player if they want to quit or load 
    /// </summary>
    void Die()
    {
        // Turn off player controls

        // Display GameOver
        UIHandler uih = FindObjectOfType<UIHandler>();

        uih.DisplayGameOver();

        // Show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// The player's primary attack that is used to attack enemies
    /// </summary>
    /// <returns>The attack set on the player's equipped weapon</returns>
    public AttackScriptableObject GetPrimaryAttack()
    {
        if (GetEquippedMainHand() != null)
        {
            Debug.Log("Main hand: " + GetEquippedMainHand().name);
            return GetEquippedMainHand().attack;
        } else
        {
            Debug.Log("Nothing in main hand");
            return null;
        }
    }

    /// <summary>
    /// A debug method to output the player's current attributes and stats
    /// </summary>
    public void ReportStats()
    {
        Debug.Log("--Stats Report--");
        Debug.Log("MaxHP: " + GetMaxHP());
        Debug.Log("MaxStamina: " + GetMaxStamina());
        Debug.Log("Move Speed: " + pm.GetMoveSpeed());
        Debug.Log("Stamina recovery rate: " + pm.GetSprintRecoveryRate());
        Debug.Log("Crit chance: " + GetCritChance());
        Debug.Log("-----");
        Debug.Log("Armor: " + GetArmor());
        Debug.Log("Magic Resist: " + GetMagicResist());
        Debug.Log("-----");
        Debug.Log("STR: " + GetStrength());
        Debug.Log("END: " + GetEndurance());
        Debug.Log("AGI: " + GetAgility());
        Debug.Log("DEX: " + GetDexterity());
        Debug.Log("INT: " + GetIntelligence());
        Debug.Log("RES: " + GetResist());
        Debug.Log("--End Stats--");
    }

    #region Get attribute/stat values from Equipment functions
    // These functions return the player's attributes by adding the base value together with all equipped equipment values

    public override int GetStrength()
    {
        int totalStrength = base.GetStrength();

        if (GetEquippedMainHand() != null)
        {
            totalStrength += GetEquippedMainHand().strength;
        }

        if (GetEquippedOffHand() != null)
        {
            totalStrength += GetEquippedOffHand().strength;
        }

        if (GetEquippedHelm() != null)
        {
            totalStrength += GetEquippedHelm().strength;
        }

        if (GetEquippedChest() != null)
        {
            totalStrength += GetEquippedChest().strength;
        }

        if (GetEquippedHands() != null)
        {
            totalStrength += GetEquippedHands().strength;
        }

        if (GetEquippedLegs() != null)
        {
            totalStrength += GetEquippedLegs().strength;
        }

        if (GetEquippedFeet() != null)
        {
            totalStrength += GetEquippedFeet().strength;
        }

        if (GetEquippedAmulet() != null)
        {
            totalStrength += GetEquippedAmulet().strength;
        }

        if (GetEquippedRingOne() != null)
        {
            totalStrength += GetEquippedRingOne().strength;
        }

        if (GetEquippedRingTwo() != null)
        {
            totalStrength += GetEquippedRingTwo().strength;
        }

        return totalStrength; // calculate strength including equipped gear here
    }

    public override int GetEndurance()
    {
        int totalEndurance = base.GetEndurance();

        if (GetEquippedMainHand() != null)
        {
            totalEndurance += GetEquippedMainHand().endurance;
        }

        if (GetEquippedOffHand() != null)
        {
            totalEndurance += GetEquippedOffHand().endurance;
        }

        if (GetEquippedHelm() != null)
        {
            totalEndurance += GetEquippedHelm().endurance;
        }

        if (GetEquippedChest() != null)
        {
            totalEndurance += GetEquippedChest().endurance;
        }

        if (GetEquippedHands() != null)
        {
            totalEndurance += GetEquippedHands().endurance;
        }

        if (GetEquippedLegs() != null)
        {
            totalEndurance += GetEquippedLegs().endurance;
        }

        if (GetEquippedFeet() != null)
        {
            totalEndurance += GetEquippedFeet().endurance;
        }

        if (GetEquippedAmulet() != null)
        {
            totalEndurance += GetEquippedAmulet().endurance;
        }

        if (GetEquippedRingOne() != null)
        {
            totalEndurance += GetEquippedRingOne().endurance;
        }

        if (GetEquippedRingTwo() != null)
        {
            totalEndurance += GetEquippedRingTwo().endurance;
        }

        return totalEndurance; // calculate endurance including equipped gear here
    }

    public override int GetAgility()
    {
        int totalAgility = base.GetAgility();

        if (GetEquippedMainHand() != null)
        {
            totalAgility += GetEquippedMainHand().agility;
        }

        if (GetEquippedOffHand() != null)
        {
            totalAgility += GetEquippedOffHand().agility;
        }

        if (GetEquippedHelm() != null)
        {
            totalAgility += GetEquippedHelm().agility;
        }

        if (GetEquippedChest() != null)
        {
            totalAgility += GetEquippedChest().agility;
        }

        if (GetEquippedHands() != null)
        {
            totalAgility += GetEquippedHands().agility;
        }

        if (GetEquippedLegs() != null)
        {
            totalAgility += GetEquippedLegs().agility;
        }

        if (GetEquippedFeet() != null)
        {
            totalAgility += GetEquippedFeet().agility;
        }

        if (GetEquippedAmulet() != null)
        {
            totalAgility += GetEquippedAmulet().agility;
        }

        if (GetEquippedRingOne() != null)
        {
            totalAgility += GetEquippedRingOne().agility;
        }

        if (GetEquippedRingTwo() != null)
        {
            totalAgility += GetEquippedRingTwo().agility;
        }

        return totalAgility; // calculate strength including equipped gear here
    }

    public override int GetDexterity()
    {
        int totalDexterity = base.GetDexterity();

        if (GetEquippedMainHand() != null)
        {
            totalDexterity += GetEquippedMainHand().dexterity;
        }
        if (GetEquippedOffHand() != null)
        {
            totalDexterity += GetEquippedOffHand().dexterity;
        }

        if (GetEquippedHelm() != null)
        {
            totalDexterity += GetEquippedHelm().dexterity;
        }

        if (GetEquippedChest() != null)
        {
            totalDexterity += GetEquippedChest().dexterity;
        }

        if (GetEquippedHands() != null)
        {
            totalDexterity += GetEquippedHands().dexterity;
        }

        if (GetEquippedLegs() != null)
        {
            totalDexterity += GetEquippedLegs().dexterity;
        }

        if (GetEquippedFeet() != null)
        {
            totalDexterity += GetEquippedFeet().dexterity;
        }

        if (GetEquippedAmulet() != null)
        {
            totalDexterity += GetEquippedAmulet().dexterity;
        }

        if (GetEquippedRingOne() != null)
        {
            totalDexterity += GetEquippedRingOne().dexterity;
        }

        if (GetEquippedRingTwo() != null)
        {
            totalDexterity += GetEquippedRingTwo().dexterity;
        }

        return totalDexterity; // calculate strength including equipped gear here
    }

    public override int GetIntelligence()
    {
        int totalIntelligence = base.GetIntelligence();

        if (GetEquippedMainHand() != null)
        {
            totalIntelligence += GetEquippedMainHand().intelligence;
        }
        if (GetEquippedOffHand() != null)
        {
            totalIntelligence += GetEquippedOffHand().intelligence;
        }

        if (GetEquippedHelm() != null)
        {
            totalIntelligence += GetEquippedHelm().intelligence;
        }

        if (GetEquippedChest() != null)
        {
            totalIntelligence += GetEquippedChest().intelligence;
        }

        if (GetEquippedHands() != null)
        {
            totalIntelligence += GetEquippedHands().intelligence;
        }

        if (GetEquippedLegs() != null)
        {
            totalIntelligence += GetEquippedLegs().intelligence;
        }

        if (GetEquippedFeet() != null)
        {
            totalIntelligence += GetEquippedFeet().intelligence;
        }

        if (GetEquippedAmulet() != null)
        {
            totalIntelligence += GetEquippedAmulet().intelligence;
        }

        if (GetEquippedRingOne() != null)
        {
            totalIntelligence += GetEquippedRingOne().intelligence;
        }

        if (GetEquippedRingTwo() != null)
        {
            totalIntelligence += GetEquippedRingTwo().intelligence;
        }

        return totalIntelligence; // calculate strength including equipped gear here
    }

    public override int GetResist()
    {
        int totalResist = base.GetResist();

        if (GetEquippedMainHand() != null)
        {
            totalResist += GetEquippedMainHand().resistance;
        }
        if (GetEquippedOffHand() != null)
        {
            totalResist += GetEquippedOffHand().resistance;
        }

        if (GetEquippedHelm() != null)
        {
            totalResist += GetEquippedHelm().resistance;
        }

        if (GetEquippedChest() != null)
        {
            totalResist += GetEquippedChest().resistance;
        }

        if (GetEquippedHands() != null)
        {
            totalResist += GetEquippedHands().resistance;
        }

        if (GetEquippedLegs() != null)
        {
            totalResist += GetEquippedLegs().resistance;
        }

        if (GetEquippedFeet() != null)
        {
            totalResist += GetEquippedFeet().resistance;
        }

        if (GetEquippedAmulet() != null)
        {
            totalResist += GetEquippedAmulet().resistance;
        }

        if (GetEquippedRingOne() != null)
        {
            totalResist += GetEquippedRingOne().resistance;
        }

        if (GetEquippedRingTwo() != null)
        {
            totalResist += GetEquippedRingTwo().resistance;
        }

        return totalResist; // calculate strength including equipped gear here
    }

    public override int GetArmor()
    {
        int totalArmor = base.GetArmor();

        if (GetEquippedMainHand() != null)
        {
            totalArmor += GetEquippedMainHand().armor;
        }
        if (GetEquippedOffHand() != null)
        {
            totalArmor += GetEquippedOffHand().armor;
        }

        if (GetEquippedHelm() != null)
        {
            totalArmor += GetEquippedHelm().armor;
        }

        if (GetEquippedChest() != null)
        {
            totalArmor += GetEquippedChest().armor;
        }

        if (GetEquippedHands() != null)
        {
            totalArmor += GetEquippedHands().armor;
        }

        if (GetEquippedLegs() != null)
        {
            totalArmor += GetEquippedLegs().armor;
        }

        if (GetEquippedFeet() != null)
        {
            totalArmor += GetEquippedFeet().armor;
        }

        if (GetEquippedAmulet() != null)
        {
            totalArmor += GetEquippedAmulet().armor;
        }

        if (GetEquippedRingOne() != null)
        {
            totalArmor += GetEquippedRingOne().armor;
        }

        if (GetEquippedRingTwo() != null)
        {
            totalArmor += GetEquippedRingTwo().armor;
        }

        totalArmor = Mathf.RoundToInt(totalArmor * (GetResist() * resistToArmorMod)); // Improves Armor based on player's resist

        return totalArmor; // calculate strength including equipped gear here
    }

    public override int GetMagicResist()
    {
        int totalMagicResist = base.GetMagicResist();

        if (GetEquippedMainHand() != null)
        {
            totalMagicResist += GetEquippedMainHand().magicResist;
        }
        if (GetEquippedOffHand() != null)
        {
            totalMagicResist += GetEquippedOffHand().magicResist;
        }

        if (GetEquippedHelm() != null)
        {
            totalMagicResist += GetEquippedHelm().magicResist;
        }

        if (GetEquippedChest() != null)
        {
            totalMagicResist += GetEquippedChest().magicResist;
        }

        if (GetEquippedHands() != null)
        {
            totalMagicResist += GetEquippedHands().magicResist;
        }

        if (GetEquippedLegs() != null)
        {
            totalMagicResist += GetEquippedLegs().magicResist;
        }

        if (GetEquippedFeet() != null)
        {
            totalMagicResist += GetEquippedFeet().magicResist;
        }

        if (GetEquippedAmulet() != null)
        {
            totalMagicResist += GetEquippedAmulet().magicResist;
        }

        if (GetEquippedRingOne() != null)
        {
            totalMagicResist += GetEquippedRingOne().magicResist;
        }

        if (GetEquippedRingTwo() != null)
        {
            totalMagicResist += GetEquippedRingTwo().magicResist;
        }

        totalMagicResist = Mathf.RoundToInt(totalMagicResist * (GetResist() * resistToMagicResistMod)); // Improves Magic Resist based on player's resist

        return totalMagicResist; // calculate strength including equipped gear here
    }

    #endregion
}
