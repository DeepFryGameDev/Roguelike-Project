using UnityEngine;

// Purpose: Equipment to be set in the Mainhand slot
// Directions: This is a scriptable object. Simply create a new object via right click > Equipment > MainHand > New MainHand
// Other notes:

[CreateAssetMenu(fileName = "NewMainHand", menuName = "Equipment/MainHand/New MainHand", order = 1)]
public class BaseMainHandEquipment : BaseEquipmentScriptableObject
{
    public AttackScriptableObject attack;


    // Start is called before the first frame update
    void Start()
    {
        equipmentSlot = EnumHandler.EquipmentSlots.MAINHAND;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
