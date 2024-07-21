using UnityEngine;

// Purpose: Equipment to be set in the Amulet slot
// Directions: This is a scriptable object. Simply create a new object via right click > Equipment > Jewelry > New Amulet
// Other notes:

[CreateAssetMenu(fileName = "NewAmulet", menuName = "Equipment/Jewelry/New Amulet", order = 1)]
public class BaseAmuletEquipment : BaseJewelryEquipment
{
    // Start is called before the first frame update
    void Start()
    {
        equipmentSlot = EnumHandler.EquipmentSlots.AMULET;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
