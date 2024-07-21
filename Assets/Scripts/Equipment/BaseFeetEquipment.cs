using UnityEngine;

// Purpose: Equipment to be set in the Feet slot
// Directions: This is a scriptable object. Simply create a new object via right click > Equipment > Armor > New Feet
// Other notes:

[CreateAssetMenu(fileName = "NewFeet", menuName = "Equipment/Armor/New Feet", order = 1)]
public class BaseFeetEquipment : BaseEquipmentArmor
{
    // Start is called before the first frame update
    void Start()
    {
        equipmentSlot = EnumHandler.EquipmentSlots.FEET;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
