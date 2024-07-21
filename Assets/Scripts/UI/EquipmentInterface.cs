using UnityEngine;

// Purpose: Used to manage interactions between equipment and menu interface
// Directions: Put this script on every equipment slot in menu interface (labeled CharacterMenuPanel/PlayerEquipmentPanel/*LayoutGroup/*Container)
// Other notes: 

public class EquipmentInterface : MonoBehaviour
{
    BaseEquipmentScriptableObject equipment; // The equipment that is assigned to this UI object (equipment slot)
    public void SetEquipment(BaseEquipmentScriptableObject equipment) { this.equipment = equipment; }
    public BaseEquipmentScriptableObject GetEquipment() { return equipment; }

    public bool ifRingTwo; // Set to true if this slot is set to ring two

    SceneInfo sceneInfo; // Used to check if the current scene is a battle scene - if so, equipment is unable to be changed

    // Used to unequip the equipment in this slot
    public void OnButtonClick()
    {
        sceneInfo = FindObjectOfType<SceneInfo>();
        if (!sceneInfo.battleScene)
        {
            if (equipment != null) // if slot has equipment in it
            {
                EquipmentManager.UnequipItem(equipment, this);
                SetEquipment(null);
            }
            else // nothing in slot already
            {
                // do nothing for now (eventually will pop up with items you can equip)
            }
        }
    }

    // Used to display the tooltip for equipped item that cursor is hovered over in UI
    public void OnMouseEnter()
    {
        // Debug.Log("Testing - on mouse enter on equipment item");

        if (equipment == null) // Nothing in equipment slot
        {
            // show nothing
        } else // Something in equipment slot
        {
            // show equipment tooltip for equip
            if (equipment.equipmentSlot == EnumHandler.EquipmentSlots.MAINHAND || equipment.equipmentSlot == EnumHandler.EquipmentSlots.OFFHAND) // show weapon tooltip
            {
                WeaponTooltipScreenSpaceUI.ShowTooltip_Static(equipment);
            }
            else // show armor tooltip
            {
                ArmorTooltipScreenSpaceUI.ShowTooltip_Static(equipment);
            }
        }
    }
    
    // Used to hide the tooltip when cursor leaves the equipment slot in UI
    public void OnMouseExit()
    {
        // Hide tooltips
        WeaponTooltipScreenSpaceUI.HideTooltip_Static();
        ArmorTooltipScreenSpaceUI.HideTooltip_Static();
    }
}
