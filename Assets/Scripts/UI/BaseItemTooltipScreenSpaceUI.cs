using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Purpose: Parent class used to house all vars/functionality for item based tooltips. Currently only used for equipment
// Directions: Simply use this class for any item based tooltips that need to have vars or functionality updated
// Other notes: 

public class BaseItemTooltipScreenSpaceUI : BaseTooltipScreenSpaceUI
{
    [Tooltip("Set to the object that holds the icon image for the attached tooltip object")]
    [SerializeField] protected Image icon;

    [Tooltip("Set to the object that holds the item name TextMeshPro component")]
    [SerializeField] protected TextMeshProUGUI nameText;

    [Tooltip("Set to the object that holds the gold value TextMeshPro component")]
    [SerializeField] protected TextMeshProUGUI goldWorthText;

    [Tooltip("Set to the object that holds the description TextMeshPro component")]
    [SerializeField] protected TextMeshProUGUI descriptionText;
}
