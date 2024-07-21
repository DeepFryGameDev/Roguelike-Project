using System.Collections.Generic;
using UnityEngine;

// Purpose: Provides functionality to manage the player's inventory
// Directions: As this is a static class, simply call 'InventoryManager' when calling functionality to the player's inventory
// Other notes: 

public static class InventoryManager
{
    public static List<BaseItemScriptableObject> items = new List<BaseItemScriptableObject>();
    static PlayerMenuHandler playerMenuHandler;

    public static void Startup()
    {
        playerMenuHandler = GameObject.FindObjectOfType<PlayerMenuHandler>();
    }

    /// <summary>
    /// Adds the provided item to the player's inventory.
    /// </summary>
    /// <param name="item">Item to be added to the player's inventory</param>
    public static void AddItem(BaseItemScriptableObject item)
    {
        Debug.Log("Item added to player inventory: " + item.name);

        items.Add(item);
        RefreshUI();
    }

    /// <summary>
    /// Removes the provided item from the player's inventory. Note: Equipped equipment does not count towards the player's inventory, so they will be removed when equipped.
    /// </summary>
    /// <param name="item">Item to be removed from the player's inventory</param>
    public static void RemoveItem(BaseItemScriptableObject item)
    {
        items.Remove(item);
    }

    /// <summary>
    /// Hides tooltips and resets all values to be set in the UI
    /// </summary>
    public static void RefreshUI()
    {
        playerMenuHandler.RefreshUI();
        WeaponTooltipScreenSpaceUI.HideTooltip_Static();
        ArmorTooltipScreenSpaceUI.HideTooltip_Static();
    }
}
