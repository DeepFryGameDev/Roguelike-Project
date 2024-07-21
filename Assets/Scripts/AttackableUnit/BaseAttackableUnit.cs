using UnityEngine;

// Purpose: The parent class used to house all variables and functionality for any unit that can be attacked, both enemies and the player
// Directions: Anything that should be shared between enemies and the player should be set in this class
// Other notes: 

public class BaseAttackableUnit : MonoBehaviour
{
    // -- Name
    [Tooltip("The name of the unit")]
    [SerializeField] new string name;

    // -- Parameters --
    protected int currentHP;
    public int GetCurrentHP() { return currentHP; }
    public void SetCurrentHP(int HP) { currentHP = HP; }

    // -- Stats --
    [Tooltip("Base Strength - Makes melee physical attacks stronger")]
    [SerializeField] protected int baseStrength;
    public int GetBaseStrength() { return baseStrength; }

    int strength; // Current Strength Level
    public virtual int GetStrength() { return strength; }
    public void SetStrength (int strength) { this.strength = strength;}

    [Tooltip("Base Endurance - Gives more health")]
    [SerializeField] protected int baseEndurance;
    public int GetBaseEndurance() { return baseEndurance; }

    int endurance;
    public virtual int GetEndurance() { return endurance; }
    public void SetEndurance(int endurance) { this.endurance = endurance; }

    [Tooltip("Base Agility - Increases critical hit chance and increases move speed, as well as sprint recovery rate. (Not yet implemented for enemy)")]
    [SerializeField] protected int baseAgility;
    public int GetBaseAgility() { return baseAgility; }

    int agility;
    public virtual int GetAgility() { return agility; }
    public void SetAgility(int agility) { this.agility = agility; }

    [Tooltip("Base Dexterity - Makes ranged physical attacks stronger")]
    [SerializeField] protected int baseDexterity;
    public int GetBaseDexterity() { return baseDexterity; }

    int dexterity;
    public virtual int GetDexterity() { return dexterity; }
    public void SetDexterity(int dexterity) { this.dexterity = dexterity; }

    [Tooltip("Base Intelligence - Makes magical attacks stronger")]
    [SerializeField] protected int baseIntelligence;
    public int GetBaseIntelligence() { return baseIntelligence; }

    int intelligence;
    public virtual int GetIntelligence() { return intelligence; }
    public void SetIntelligence(int intelligence) { this.intelligence = intelligence; }

    [Tooltip("Base Resistance - Increases effectiveness of Armor and Magic Resistance")]
    [SerializeField] protected int baseResist;
    public int GetBaseResist() { return baseResist; }

    int resist;
    public virtual int GetResist() { return resist; }
    public void SetResist(int resist) { this.resist = resist; }

    // armor
    [Tooltip("Total armor - Decreases physical damage taken")]
    [SerializeField] protected int baseArmor;
    public int GetBaseArmor() { return baseArmor; }

    int armor;
    public virtual int GetArmor() { return armor; }
    public void SetArmor(int armor) { this.armor = armor; }

    // magic resistance
    [Tooltip("Total Magic Resist - Decreases magical damge taken")]
    [SerializeField] protected int baseMagicResist;
    public int GetBaseMagicResist() { return baseMagicResist; }
    int magicResist;
    public virtual int GetMagicResist() { return magicResist; }
    public void SetMagicResist(int magicResist) { this.magicResist = magicResist; }

    // movement speed

    // -- Other stuff --

}
