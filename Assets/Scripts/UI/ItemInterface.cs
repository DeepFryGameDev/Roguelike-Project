using UnityEngine;

// Purpose: Used to manage interactions between items in the player's inventory and menu interface
// Directions: Put this script on the ItemInventory container prefab to be instantiated for every item in the player's inventory. When the prefab is instantiated, the script is called.
// Other notes: 

public class ItemInterface : MonoBehaviour
{
    // The item that this UI object is assigned to. Each item in the player's inventory should have this assigned when instantiated
    BaseItemScriptableObject item;
    public void SetItem(BaseItemScriptableObject item) { this.item = item; }

    // Used if the item assigned is an equipment - simply used as the equipment version of this item
    BaseEquipmentScriptableObject itemAsEquip;

    // Used to ensure player cannot swap equipment during a battle scene
    SceneInfo sceneInfo;

    private void Start()
    {
        itemAsEquip = item as BaseEquipmentScriptableObject;
    }

    /// <summary>
    /// Set on the attached UI prefab to be called when the player clicks the object in the UI
    /// Simply equips item if it is an equipment (and the player is not on a battle scene)
    /// </summary>
    public void OnButtonClick()
    {
        sceneInfo = FindObjectOfType<SceneInfo>();
        if (!sceneInfo.battleScene) // Ensures player cannot swap equipment during battle scene
        {
            // if Item is equipment, equip it.
            if (itemAsEquip != null)
            {
                EquipmentManager.EquipItem(itemAsEquip);
            }
            else
            { // otherwise, use it
                Debug.Log("Item in inventory clicked!  Nothing to do yet for normal items!");
            }
        }
    }

    /// <summary>
    /// Set on the attached UI prefab to be called when the player hovers the mouse cursor over the object in the UI
    /// Simply shows the tooltip for the item/equipment based on the type of item/equipment
    /// </summary>
    public void OnMouseEnter()
    {
        // Debug.Log("Testing - on mouse enter on inventory item");

        if (itemAsEquip != null) // Equipment item - show equipment tooltip
        {
            if (itemAsEquip.equipmentSlot == EnumHandler.EquipmentSlots.MAINHAND || itemAsEquip.equipmentSlot == EnumHandler.EquipmentSlots.OFFHAND) // show weapon tooltip
            {
                WeaponTooltipScreenSpaceUI.ShowTooltip_Static(itemAsEquip);
            } else // show armor tooltip
            {
                ArmorTooltipScreenSpaceUI.ShowTooltip_Static(itemAsEquip);
            }

        } else // Regular item - show item tooltip
        {

        }
    }

    /// <summary>
    /// Set on the attached UI prefab to be called when the player takes the mouse cursor off of the object in the UI
    /// Simply hides the tooltip
    /// </summary>
    public void OnMouseExit()
    {
        // Hide tooltips
        WeaponTooltipScreenSpaceUI.HideTooltip_Static();
        ArmorTooltipScreenSpaceUI.HideTooltip_Static();
    }
}
