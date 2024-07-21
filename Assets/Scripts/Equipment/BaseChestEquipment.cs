using UnityEngine;

// Purpose: Equipment to be set in the Chest slot
// Directions: This is a scriptable object. Simply create a new object via right click > Equipment > Armor > New Chest
// Other notes:

[CreateAssetMenu(fileName = "NewChest", menuName = "Equipment/Armor/New Chest", order = 1)]
public class BaseChestEquipment : BaseEquipmentArmor
{
    // Start is called before the first frame update
    void Start()
    {
        equipmentSlot = EnumHandler.EquipmentSlots.CHEST;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
