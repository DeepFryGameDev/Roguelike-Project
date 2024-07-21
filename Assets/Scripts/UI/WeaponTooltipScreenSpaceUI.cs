using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Purpose: Child class from the Equipment tooltip class - used to display weapon details when the player hovers their cursor over any weapon element in the UI
// Directions: Attach to the tooltip object that should be used to display armor details
// Other notes: 

public class WeaponTooltipScreenSpaceUI : BaseEquipmentTooltipScreenSpaceUI
{
    public static WeaponTooltipScreenSpaceUI Instance;

    [Tooltip("Set to the object holding the image component to display the weapon's attack icon")]
    [SerializeField] Image attackIconImage;

    [Tooltip("Set to the object holding the TextMeshPro text component to display the weapon's attack name")]
    [SerializeField] TextMeshProUGUI attackNameText;

    [Tooltip("Set to the object holding the TextMeshPro text component to display the weapon's attack description")]
    [SerializeField] TextMeshProUGUI attackDescriptionText;

    void Update()
    {
        /*if (Instance == null)
        {
            Setup();

            Instance = this;
            Instance.HideTooltip();
        }*/

        AnchorTooltip();
    }

    private void Awake()
    {
        Setup();

        Instance = this;
        Instance.HideTooltip();
    }

    /// <summary>
    /// Sets the text and icon elements of the tooltip to the details from the equipment provided
    /// Calls the base function for SetTooltip, and then includes the details for weapons that are not used by armor
    /// </summary>
    /// <param name="equipmentItem">Weapon item to set tooltip details</param>
    public override void SetTooltip(BaseEquipmentScriptableObject equipmentItem)
    {
        base.SetTooltip(equipmentItem);

        if (equipmentItem.equipmentSlot == EnumHandler.EquipmentSlots.MAINHAND)
        {
            BaseMainHandEquipment mainHandEquip = equipmentItem as BaseMainHandEquipment;

            attackIconImage.sprite = mainHandEquip.attack.icon;
            attackNameText.SetText(mainHandEquip.attack.name);
            attackDescriptionText.SetText(mainHandEquip.attack.description);

            attackIconImage.color = new Color(attackIconImage.color.r, attackIconImage.color.g, attackIconImage.color.b, 1);

        }
        else if (equipmentItem.equipmentSlot == EnumHandler.EquipmentSlots.OFFHAND) // this will be updated
        {
            BaseOffHandEquipment offHandEquip = equipmentItem as BaseOffHandEquipment;

            attackIconImage.sprite = offHandEquip.attack.icon;
            attackNameText.SetText(offHandEquip.attack.name);
            attackDescriptionText.SetText(offHandEquip.attack.description);

            attackIconImage.color = new Color(attackIconImage.color.r, attackIconImage.color.g, attackIconImage.color.b, 0);
        }
    }

    /// <summary>
    /// Calls functionality to set the tooltip elements for the provided equipment and displays the tooltip object to the player
    /// </summary>
    /// <param name="equipmentItem">Weapon item to display tooltip details</param>
    public static void ShowTooltip_Static(BaseEquipmentScriptableObject equipmentItem)
    {
        Instance.SetTooltip(equipmentItem);

        Instance.ShowTooltip();
    }

    /// <summary>
    /// Hides the tooltip from the player - called when the player moves the mouse cursor off of the UI element
    /// </summary>
    public static void HideTooltip_Static()
    {
        if (Instance != null) { Instance.HideTooltip(); }        
    }
}
