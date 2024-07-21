using UnityEngine;

// Purpose: Equipment to be set in the Hands slot
// Directions: This is a scriptable object. Simply create a new object via right click > Equipment > Armor > New Hands
// Other notes:

[CreateAssetMenu(fileName = "NewHand", menuName = "Equipment/Armor/New Hands", order = 1)]
public class BaseHandEquipment : BaseEquipmentArmor
{
    // Start is called before the first frame update
    void Start()
    {
        equipmentSlot = EnumHandler.EquipmentSlots.HANDS;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
