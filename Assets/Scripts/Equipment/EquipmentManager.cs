using UnityEngine;

// Purpose: Static class to manage equipment actions (between the equipment itself and the player)
// Directions: Call the class when needing to call equipment related methods
// Other notes: 

public static class EquipmentManager
{
    static PlayerManager playerManager; // Used to gather the BasePlayer for the player
    static BasePlayer player; // Used to set equipment on the player

    static AttackLoader attackLoader; // Used to set attacks when main hand is equipped
    static UIHandler uiHandler; // Used to set attack radials when changing main hand equipped

    public static void Startup()
    {
        playerManager = GameObject.FindObjectOfType<PlayerManager>();
        player = playerManager.GetPlayer();

        attackLoader = GameObject.FindObjectOfType<AttackLoader>();
        uiHandler = GameObject.FindObjectOfType<UIHandler>();
    }

    /// <summary>
    /// Sets the provided equipment to the equipment slot on the player
    /// </summary>
    /// <param name="equip">The equipment to set on the player</param>
    public static void EquipItem(BaseEquipmentScriptableObject equip)
    {        
        switch (equip.equipmentSlot)
        {
            case EnumHandler.EquipmentSlots.MAINHAND:
                // Add item to player's equipped slot
                BaseMainHandEquipment mainHand = equip as BaseMainHandEquipment;

                player.SetEquippedMainHand(mainHand);

                attackLoader.SetAttackCollisionTriggers();
                uiHandler.GenerateCooldownRadials();

                break;
            case EnumHandler.EquipmentSlots.OFFHAND:
                // Add item to player's equipped slot
                BaseOffHandEquipment offHand = equip as BaseOffHandEquipment;

                player.SetEquippedOffHand(offHand);
                break;
            case EnumHandler.EquipmentSlots.HELM:
                // Add item to player's equipped slot
                BaseHelmEquipment helm = equip as BaseHelmEquipment;

                player.SetEquippedHelm(helm);
                break;
            case EnumHandler.EquipmentSlots.CHEST:
                // Add item to player's equipped slot
                BaseChestEquipment chest = equip as BaseChestEquipment;

                player.SetEquippedChest(chest);
                break;
            case EnumHandler.EquipmentSlots.HANDS:
                // Add item to player's equipped slot
                BaseHandEquipment hands = equip as BaseHandEquipment;

                player.SetEquippedHands(hands);
                break;
            case EnumHandler.EquipmentSlots.LEGS:
                // Add item to player's equipped slot
                BaseLegsEquipment legs = equip as BaseLegsEquipment;

                player.SetEquippedLegs(legs);
                break;
            case EnumHandler.EquipmentSlots.FEET:
                // Add item to player's equipped slot
                BaseFeetEquipment feet = equip as BaseFeetEquipment;

                player.SetEquippedFeet(feet);
                break;
            case EnumHandler.EquipmentSlots.AMULET:
                // Add item to player's equipped slot
                BaseAmuletEquipment amulet = equip as BaseAmuletEquipment;

                player.SetEquippedAmulet(amulet);
                break;
            case EnumHandler.EquipmentSlots.RING:
                BaseRingEquipment ring = equip as BaseRingEquipment;

                // if ringOne slot is null, add the item to that slot
                if (player.GetEquippedRingOne() == null)
                {
                    player.SetEquippedRingOne(ring);
                } else // then add to ringTwo slot if ringOne is not null
                {
                    player.SetEquippedRingTwo(ring);
                }                
                break;
        }

        Debug.Log("Equipped " + equip.name);

        InventoryManager.RemoveItem(equip);

        // Need to update player's movement stats
        player.UpdateHPForMax();
        player.UpdateStaminaForMax();

        InventoryManager.RefreshUI();
    }

    /// <summary>
    /// Using the given equipment, sets the player's equipped slot to null and adds the equipment back to player's inventory
    /// </summary>
    /// <param name="equip">Equipment to remove from player and add to inventory</param>
    /// <param name="equipInterface">The equipInterface for the ui object that is being clicked to unequip the item. Used only to check if the slot is ringTwo.</param>
    public static void UnequipItem(BaseEquipmentScriptableObject equip, EquipmentInterface equipInterface)
    {
        // Set equipped slot to null

        switch (equip.equipmentSlot)
        {
            case EnumHandler.EquipmentSlots.MAINHAND:
                player.SetEquippedMainHand(null);

                uiHandler.ClearRadials();

                break;
            case EnumHandler.EquipmentSlots.OFFHAND:
                player.SetEquippedOffHand(null);
                break;
            case EnumHandler.EquipmentSlots.HELM:
                player.SetEquippedHelm(null);
                break;
            case EnumHandler.EquipmentSlots.CHEST:
                player.SetEquippedChest(null);
                break;
            case EnumHandler.EquipmentSlots.HANDS:
                player.SetEquippedHands(null);
                break;
            case EnumHandler.EquipmentSlots.LEGS:
                player.SetEquippedLegs(null);
                break;
            case EnumHandler.EquipmentSlots.FEET:
                player.SetEquippedFeet(null);
                break;
            case EnumHandler.EquipmentSlots.AMULET:
                player.SetEquippedAmulet(null);
                break;
            case EnumHandler.EquipmentSlots.RING:
                if (!equipInterface.ifRingTwo)
                {
                    player.SetEquippedRingOne(null);
                } else
                {
                    player.SetEquippedRingTwo(null);
                }

                break;
        }

        // Add item to inventory
        InventoryManager.AddItem(equip);

        InventoryManager.RefreshUI();
    }
}
