using UnityEngine;
using UnityEngine.UI;

// Purpose: Handles various UI related processes
// Directions: Attach to [UI]
// Other notes:

public class UIHandler : MonoBehaviour
{
    [Tooltip("Set to the panel used for game over")]
    [SerializeField] GameObject GameOverPanel; // Used to display game over menu when the player dies, allows player to start again or quit

    [Tooltip("Set to the object that will hold UI radials for attack cooldown")]
    [SerializeField] Transform CooldownRadialParent; // Transform to use to anchor the cooldown radial UI for player feedback

    [Tooltip("Set to the player's menu handler when they open the menu during gameplay")]
    [SerializeField] PlayerMenuHandler playerMenuHandler; // Handles the player's menu interactions during gameplay

    PrefabManager prefabManager; // Used to gather the EXP and Resource panels for game startup

    PlayerManager playerManager; // Used to gather the BasePlayer for the player object

    void Awake()
    {
        prefabManager = GetComponent<PrefabManager>();

        playerManager = FindObjectOfType<PlayerManager>();

        playerMenuHandler = FindObjectOfType<PlayerMenuHandler>();
    }

    /// <summary>
    /// Shows Game Over panel and options
    /// </summary>
    public void DisplayGameOver()
    {
        ToggleObject(GameOverPanel, true);
    }

    /// <summary>
    /// Displays resource and EXP panels when starting a new game
    /// </summary>
    public void UIStartup()
    {
        playerMenuHandler.Startup();

        ToggleObject(prefabManager.GetResourcePanel(), true);
        ToggleObject(prefabManager.GetEXPPanel(), true);

        // GenerateCooldownRadials();
        // newRadialCooldown.transform.SetParent(CooldownRadialParent);
    }

    /// <summary>
    /// Displays attack cooldown radial UI objects for player feedback to know when attacks are available
    /// </summary>
    public void GenerateCooldownRadials()
    {
        // Clear previous radials
        ClearRadials();

        // Generate cooldown radials
        GameObject newRadialCooldown = Instantiate(GameAssets.i.cooldownRadial, CooldownRadialParent);
        newRadialCooldown.GetComponent<CooldownRadial>().Setup(playerManager.GetPlayer().GetPrimaryAttack());
    }

    /// <summary>
    /// Destroys any radials under the cooldown radial parent - used when setting new cooldown radials, or unequipping a weapon
    /// </summary>
    public void ClearRadials()
    {
        foreach (Transform child in CooldownRadialParent)
        {
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Toggles the given panel as active or inactive
    /// </summary>
    /// <param name="Panel">Panel to toggle</param>
    /// <param name="toggle">True: Panel is active/displayed - False: Panel is inactive/hidden</param>
    public void ToggleObject(GameObject Panel, bool toggle)
    {
        Panel.SetActive(toggle);
    }
}
