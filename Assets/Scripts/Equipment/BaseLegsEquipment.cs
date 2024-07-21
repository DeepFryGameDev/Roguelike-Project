using UnityEngine;

// Purpose: Equipment to be set in the Legs slot
// Directions: This is a scriptable object. Simply create a new object via right click > Equipment > Armor > New Legs
// Other notes:

[CreateAssetMenu(fileName = "NewLegs", menuName = "Equipment/Armor/New Legs", order = 1)]
public class BaseLegsEquipment : BaseEquipmentArmor
{
    // Start is called before the first frame update
    void Start()
    {
        equipmentSlot = EnumHandler.EquipmentSlots.LEGS;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
