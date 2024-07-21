using UnityEngine;

// Purpose: Equipment to be set in the ring slot
// Directions: This is a scriptable object. Simply create a new object via right click > Equipment > Jewelry > New Ring
// Other notes:

[CreateAssetMenu(fileName = "NewRing", menuName = "Equipment/Jewelry/New Ring", order = 1)]
public class BaseRingEquipment : BaseJewelryEquipment
{
    // Start is called before the first frame update
    void Start()
    {
        equipmentSlot = EnumHandler.EquipmentSlots.RING;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
