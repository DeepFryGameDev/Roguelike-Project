using UnityEngine;

// Purpose: Equipment to be set in the Offhand slot
// Directions: This is a scriptable object. Simply create a new object via right click > Equipment > OffHand > New OffHand
// Other notes:

[CreateAssetMenu(fileName = "NewOffHand", menuName = "Equipment/OffHand/New OffHand", order = 1)]
public class BaseOffHandEquipment : BaseEquipmentScriptableObject
{
    public AttackScriptableObject attack;

    // Start is called before the first frame update
    void Start()
    {
        equipmentSlot = EnumHandler.EquipmentSlots.OFFHAND;
    }

    // Update is called once per frame
    void Update()
    {

    }
}