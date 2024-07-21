// Purpose: Child class from the Equipment tooltip class - used to display armor details when the player hovers their cursor over any armor element in the UI
// Directions: Attach to the tooltip object that should be used to display armor details
// Other notes: 

public class ArmorTooltipScreenSpaceUI : BaseEquipmentTooltipScreenSpaceUI
{
    public static ArmorTooltipScreenSpaceUI Instance;

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
    /// </summary>
    /// <param name="equipmentItem">Armor item to set tooltip details</param>
    public override void SetTooltip(BaseEquipmentScriptableObject equipmentItem)
    {
        base.SetTooltip(equipmentItem);

    }

    /// <summary>
    /// Calls functionality to set the tooltip elements for the provided equipment and displays the tooltip object to the player
    /// </summary>
    /// <param name="equipmentItem">Armor item to display tooltip details</param>
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
