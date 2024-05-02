using UnityEngine;
using UnityEngine.UI;

// Purpose: Contains prefabs to be manipulated by script
// Directions: Attach to [UI] object
// Other notes:

public class PrefabManager : MonoBehaviour
{
    [Tooltip("Set to slider for stamina gauge")]
    public Slider staminaSlider;
    [Tooltip("Set to stamina bar image")]
    public Image stamBarImage;

    [Tooltip("Set to desired color for the stamina bar to be set to when player is unable to sprint or use stamina")]
    public Color outOfStaminaBarColor = Color.black;

    [Tooltip("Set to InteractionPanel object in [UI]")]
    public GameObject interactionKeyPanel;
}
