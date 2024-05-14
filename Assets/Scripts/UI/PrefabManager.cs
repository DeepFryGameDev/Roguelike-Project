using UnityEngine;
using UnityEngine.UI;

// Purpose: Contains prefabs to be manipulated by script
// Directions: Attach to [UI] object
// Other notes:

public class PrefabManager : MonoBehaviour
{
    [Tooltip("Set to slider for stamina gauge")]
    [SerializeField] Slider staminaSlider;
    public Slider GetStaminaSlider() { return staminaSlider; }

    [Tooltip("Set to stamina bar image")]
    [SerializeField] Image staminaBarImage;
    public Image GetStaminaBarImage() { return staminaBarImage; }

    [Tooltip("Set to desired color for the stamina bar to be set to when player is unable to sprint or use stamina")]
    [SerializeField] Color outOfStaminaBarColor = Color.black;
    public Color GetOutOfStaminaBarColor() { return outOfStaminaBarColor; }

    [Tooltip("Set to InteractionPanel object in [UI]")]
    [SerializeField] GameObject interactionKeyPanel;
    public GameObject GetInteractionKeyPanel() { return interactionKeyPanel; }

    [Tooltip("Set to slider for health gauge")]
    [SerializeField] Slider healthSlider;
    public Slider GetHealthSlider() { return healthSlider; }

    [Tooltip("Set to health bar image")]
    [SerializeField] Image healthBarImage;
    public Image GetHealthBarImage() { return healthBarImage; }
}
