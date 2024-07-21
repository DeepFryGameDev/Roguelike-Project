// Purpose: Parent class for all equipment that can be 'equipped' by the player to increase their stats and provide attacks/abilities
// Directions: Any functionality/vars to be used for all equipment should be set in here. These are set per piece of equipment.
// Other notes:

using UnityEngine;

public class BaseEquipmentScriptableObject : BaseItemScriptableObject
{
    [Tooltip("Slot that the equipment should be set to")]
    public EnumHandler.EquipmentSlots equipmentSlot;

    [Tooltip("How rare the object is - impacts drop rates (and in the future, increases stat values)")]
    public EnumHandler.ItemRarities rarity;

    //---

    [Tooltip("Strength that should be added to the player's stats when equipped")]
    public int strength;

    [Tooltip("Endurance that should be added to the player's stats when equipped")]
    public int endurance;

    [Tooltip("Agility that should be added to the player's stats when equipped")]
    public int agility;

    [Tooltip("Dexterity that should be added to the player's stats when equipped")]
    public int dexterity;

    [Tooltip("Intelligence that should be added to the player's stats when equipped")]
    public int intelligence;

    [Tooltip("Resistance that should be added to the player's stats when equipped")]
    public int resistance;

    //---

    [Tooltip("Armor that should be added to the player's stats when equipped")]
    public int armor;

    [Tooltip("Magic Resist that should be added to the player's stats when equipped")]
    public int magicResist;
}
