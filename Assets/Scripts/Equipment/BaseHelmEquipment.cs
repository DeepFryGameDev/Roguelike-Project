using UnityEngine;

// Purpose: Equipment to be set in the Helmet slot
// Directions: This is a scriptable object. Simply create a new object via right click > Equipment > Armor > New Helmet
// Other notes:

[CreateAssetMenu(fileName = "NewHelmet", menuName = "Equipment/Armor/New Helmet", order = 1)]
public class BaseHelmEquipment : BaseEquipmentArmor
{
    void Start()
    {
        equipmentSlot = EnumHandler.EquipmentSlots.HELM;
    }
}
