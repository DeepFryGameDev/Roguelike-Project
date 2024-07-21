using TMPro;
using UnityEngine;

// Purpose: Maintains all functions and vars for equipment tooltip (anything used by armor or weapon tooltips)
// Directions: If armor and weapon tooltips both need functionality, it should be placed in here
// Other notes: 

public class BaseEquipmentTooltipScreenSpaceUI : BaseItemTooltipScreenSpaceUI
{
    [Tooltip("Set to the text field used for displaying rarity")]
    [SerializeField] protected TextMeshProUGUI rarityText;

    [Tooltip("Set to the text field used for displaying the type of equipment")]
    [SerializeField] protected TextMeshProUGUI typeText;

    [Tooltip("Set to the text field used for displaying the change in stats for the equipment being checked")]
    [SerializeField] protected TextMeshProUGUI statChangeText;

    [Tooltip("Set to the text field used for displaying any armor value for the equipment being checked")]
    [SerializeField] protected TextMeshProUGUI armorText;

    [Tooltip("Set to the text field used for displaying any magic resistance value for the equipment being checked")]
    [SerializeField] protected TextMeshProUGUI magicResistText;

    /// <summary>
    /// Sets the text of the TextMeshPro elements of the tooltip
    /// </summary>
    /// <param name="equipmentItem">Equipment to be have details set on the tooltip</param>
    public virtual void SetTooltip(BaseEquipmentScriptableObject equipmentItem)
    {
        icon.sprite = equipmentItem.icon;
        nameText.SetText(equipmentItem.name);

        rarityText.SetText(equipmentItem.rarity.ToString());
        typeText.SetText(equipmentItem.equipmentSlot.ToString());
        statChangeText.SetText(GetStatChangeText(equipmentItem));

        goldWorthText.SetText(equipmentItem.goldWorth.ToString());
        descriptionText.SetText(equipmentItem.description);

        armorText.SetText(equipmentItem.armor.ToString());
        magicResistText.SetText(equipmentItem.magicResist.ToString());
    }

    /// <summary>
    /// Used to check for which stats are included on the equipment item to be displayed on the toolip
    /// </summary>
    /// <param name="equipmentItem">Equipment to check for which stats to be set on the statChangeText on the tooltip</param>
    /// <returns>A string formatted to only include changed stats for the given equipment</returns>
    string GetStatChangeText(BaseEquipmentScriptableObject equipmentItem)
    {
        string statText = string.Empty;

        int totalStats = 0;

        if (equipmentItem.strength > 0)
        {
            statText += "STR +" + equipmentItem.strength;

            totalStats++;
        }

        if (equipmentItem.endurance > 0)
        {
            if (totalStats > 0)
            {
                statText += "\n";
            }

            statText += "END +" + equipmentItem.endurance;

            totalStats++;
        }

        if (equipmentItem.agility > 0)
        {
            if (totalStats > 0)
            {
                statText += "\n";
            }

            statText += "AGI +" + equipmentItem.agility;

            totalStats++;
        }

        if (equipmentItem.dexterity > 0)
        {
            if (totalStats > 0)
            {
                statText += "\n";
            }

            statText += "DEX +" + equipmentItem.dexterity;

            totalStats++;
        }

        if (equipmentItem.intelligence > 0)
        {
            if (totalStats > 0)
            {
                statText += "\n";
            }

            statText += "INT +" + equipmentItem.intelligence;

            totalStats++;
        }

        if (equipmentItem.resistance > 0)
        {
            if (totalStats > 0)
            {
                statText += "\n";
            }

            statText += "RES +" + equipmentItem.resistance;

            totalStats++;
        }

        return statText;
    }

}
