using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Purpose: Manages the UI elements of the player menu used when the player presses the 'tab' key
// Directions: Add to the '[UI]/Canvas/CharacterMenuPanel' object
// Other notes: 

public class PlayerMenuHandler : MonoBehaviour
{
    // Stats Panel
    [Tooltip("Set to the object holding TextMeshPro text component for the player's name")]
    [SerializeField] TextMeshProUGUI nameText;

    [Tooltip("Set to the object holding TextMeshPro text component for the player's strength value (PlayerStatsPanel/StatsContainer/StrengthContainer/StrengthValue)")]
    [SerializeField] TextMeshProUGUI strValueText;
    [Tooltip("Set to the object holding fill Image component for the player's strength bar (PlayerStatsPanel/StatsContainer/StrengthContainer/StrengthBarBackground/StrengthBarFill)")]
    [SerializeField] Image strFillImage;
    [Tooltip("Set to the object holding TextMeshPro text component for the player's endurance value (PlayerStatsPanel/StatsContainer/EnduranceContainer/EnduranceValue)")]
    [SerializeField] TextMeshProUGUI endValueText;
    [Tooltip("Set to the object holding fill Image component for the player's endurance bar (PlayerStatsPanel/StatsContainer/EnduranceContainer/EnduranceBarBackground/EnduranceBarFill)")]
    [SerializeField] Image endFillImage;
    [Tooltip("Set to the object holding TextMeshPro text component for the player's agility value (PlayerStatsPanel/StatsContainer/AgilityContainer/AgilityValue)")]
    [SerializeField] TextMeshProUGUI agiValueText;
    [Tooltip("Set to the object holding fill Image component for the player's agility bar (PlayerStatsPanel/StatsContainer/AgilityContainer/AgilityBarBackground/AgilityBarFill)")]
    [SerializeField] Image agiFillImage;
    [Tooltip("Set to the object holding TextMeshPro text component for the player's dexterity value (PlayerStatsPanel/StatsContainer/DexterityContainer/DexterityValue)")]
    [SerializeField] TextMeshProUGUI dexValueText;
    [Tooltip("Set to the object holding fill Image component for the player's dexterity bar (PlayerStatsPanel/StatsContainer/DexterityContainer/DexterityBarBackground/DexterityBarFill)")]
    [SerializeField] Image dexFillImage;
    [Tooltip("Set to the object holding TextMeshPro text component for the player's intelligence value (PlayerStatsPanel/StatsContainer/IntelligenceContainer/IntelligenceValue)")]
    [SerializeField] TextMeshProUGUI intValueText;
    [Tooltip("Set to the object holding fill Image component for the player's intelligence bar (PlayerStatsPanel/StatsContainer/IntelligenceContainer/IntelligenceBarBackground/IntelligenceBarFill)")]
    [SerializeField] Image intFillImage;
    [Tooltip("Set to the object holding TextMeshPro text component for the player's resistance value (PlayerStatsPanel/StatsContainer/ResistanceContainer/ResistanceValue)")]
    [SerializeField] TextMeshProUGUI resValueText;
    [Tooltip("Set to the object holding fill Image component for the player's resistance bar (PlayerStatsPanel/StatsContainer/ResistanceContainer/ResistanceBarBackground/ResistanceBarFill)")]
    [SerializeField] Image resFillImage;

    [Tooltip("Set to the object holding TextMeshPro text component for the player's armor value (CharacterMenuPanel/PlayerEquipmentPanel/ArmorIcon/ArmorText)")]
    [SerializeField] TextMeshProUGUI armorValueText;
    [Tooltip("Set to the object holding TextMeshPro text component for the player's magic resist value (CharacterMenuPanel/PlayerEquipmentPanel/MagicResistIcon/MagicResistText)")]
    [SerializeField] TextMeshProUGUI magicResistValueText;

    [Tooltip("Set to the object holding CanvasGroup component for CharacterMenuPanel")]
    [SerializeField] CanvasGroup characterMenuCanvasGroup;

    // Equipment Panel
    [Tooltip("Set to the object holding image component to display the player's silhouette (CharacterMenuPanel/PlayerEquipmentPanel/SilhouetteContainer/SilhouetteImage")]
    [SerializeField] Image silhouetteImage;
    [Tooltip("Set to whichever sprite should be used to display as the silhouette for warrior class")]
    [SerializeField] Sprite warriorSilouette;
    [Tooltip("Set to whichever sprite should be used to display as the silhouette for mage class")]
    [SerializeField] Sprite mageSilhouette;
    [Tooltip("Set to whichever sprite should be used to display as the silhouette for archer class")]
    [SerializeField] Sprite archerSilhouette;

    [Tooltip("Set to the object holding GridLayoutGroup component to display player's inventory (CharacterMenuPanel/PlayerInventoryPanel/InventoryGrid")]
    [SerializeField] GridLayoutGroup inventoryGrid;

    [Tooltip("Set to the object for the player's equipped main hand container (CharacterMenuPanel/PlayerEquipmentPanel/LeftFrameLayoutGroup/MainHandContainer)")]
    [SerializeField] GameObject mainHandContainer;
    [Tooltip("Set to the object for the player's equipped off hand container (CharacterMenuPanel/PlayerEquipmentPanel/LeftFrameLayoutGroup/OffHandContainer)")]
    [SerializeField] GameObject offHandContainer;
    [Tooltip("Set to the object for the player's equipped helm container (CharacterMenuPanel/PlayerEquipmentPanel/RightFrameLayoutGroup/HelmContainer)")]
    [SerializeField] GameObject helmContainer;
    [Tooltip("Set to the object for the player's equipped chest container (CharacterMenuPanel/PlayerEquipmentPanel/RightFrameLayoutGroup/ChestContainer)")]
    [SerializeField] GameObject chestContainer;
    [Tooltip("Set to the object for the player's equipped hands container (CharacterMenuPanel/PlayerEquipmentPanel/RightFrameLayoutGroup/HandsContainer)")]
    [SerializeField] GameObject handsContainer;
    [Tooltip("Set to the object for the player's equipped legs container (CharacterMenuPanel/PlayerEquipmentPanel/RightFrameLayoutGroup/LegsContainer)")]
    [SerializeField] GameObject legsContainer;
    [Tooltip("Set to the object for the player's equipped feet container (CharacterMenuPanel/PlayerEquipmentPanel/RightFrameLayoutGroup/FeetContainer)")]
    [SerializeField] GameObject feetContainer;
    [Tooltip("Set to the object for the player's equipped amulet container (CharacterMenuPanel/PlayerEquipmentPanel/LeftFrameLayoutGroup/AmuletContainer)")]
    [SerializeField] GameObject amuletContainer;
    [Tooltip("Set to the object for the player's equipped ring one container (CharacterMenuPanel/PlayerEquipmentPanel/LeftFrameLayoutGroup/RingOneContainer)")]
    [SerializeField] GameObject ringOneContainer;
    [Tooltip("Set to the object for the player's equipped ring two container (CharacterMenuPanel/PlayerEquipmentPanel/LeftFrameLayoutGroup/RingTwoContainer)")]
    [SerializeField] GameObject ringTwoContainer;

    float statMax = 9; // Should be moved - maximum any stat can be set to

    BasePlayer player; // Used to gather the player's stats and equipment
    PlayerManager pm; // Used to gather player class
    MainMenuManager mainMenuManager; // Used to determine if game is being generated from main menu or Unity

    bool menuOpen; // Set to true when the menu is open

    public bool canOpenMenu; // Set in SceneInfo to determine if menu can be opened (keeps player from opening player menu on main menu)

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && canOpenMenu)
        {
            if (!menuOpen)
            {
                menuOpen = true;

                Time.timeScale = 0;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                if (FindObjectOfType<SceneInfo>().battleScene)
                {
                    FindObjectOfType<CrosshairHandler>().ToggleCrosshair(false);
                }                

                characterMenuCanvasGroup.alpha = 1;
                characterMenuCanvasGroup.blocksRaycasts = true;
                characterMenuCanvasGroup.interactable = true;

            } else
            {
                menuOpen = false;

                Time.timeScale = 1;

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                if (FindObjectOfType<SceneInfo>().battleScene)
                {
                    FindObjectOfType<CrosshairHandler>().ToggleCrosshair(true);
                }

                characterMenuCanvasGroup.alpha = 0;
                characterMenuCanvasGroup.blocksRaycasts = false;
                characterMenuCanvasGroup.interactable = false;
            }            
        }
    }

    private void Awake()
    {
        if (GameManager.GetGameSet())
        {
            Startup();
        }
    }

    /// <summary>
    /// Call when creating the game session to set the required vars for script to function
    /// </summary>
    public void Startup()
    {
        pm = FindObjectOfType<PlayerManager>();
        mainMenuManager = FindAnyObjectByType<MainMenuManager>();
        player = pm.GetPlayer();

        StartupStatsPanel();
        StartupSilhouette();
        StartupInventory();
    }

    /// <summary>
    /// Clears any previous values from UI elements and sets them with the current values
    /// </summary>
    public void RefreshUI()
    {
        ClearInventoryUI();
        DrawInventory();

        DrawEquipment();

        DrawStats();
    }

    /// <summary>
    /// Used to draw player name and stats when script during Startup()
    /// </summary>
    void StartupStatsPanel()
    {
        // set name
        if (mainMenuManager == null) // starting from debugging state
        {
            nameText.text = pm.playerClass.ToString();
        }
        else
        { // starting from main menu
            nameText.text = mainMenuManager.nameInputField.text;
        }

        DrawStats();
    }

    /// <summary>
    /// Displays the player's stat values and stat bars
    /// </summary>
    void DrawStats()
    {
        // Set values
        int totalStr = player.GetStrength();

        // Set value texts
        strValueText.text = player.GetStrength().ToString();
        endValueText.text = player.GetEndurance().ToString();
        agiValueText.text = player.GetAgility().ToString();
        dexValueText.text = player.GetDexterity().ToString();
        intValueText.text = player.GetIntelligence().ToString();
        resValueText.text = player.GetResist().ToString();

        // set fills
        strFillImage.fillAmount = ((float)player.GetStrength() / statMax);
        endFillImage.fillAmount = ((float)player.GetEndurance() / statMax);
        agiFillImage.fillAmount = ((float)player.GetAgility() / statMax);
        dexFillImage.fillAmount = ((float)player.GetDexterity() / statMax);
        intFillImage.fillAmount = ((float)player.GetIntelligence() / statMax);
        resFillImage.fillAmount = ((float)player.GetResist() / statMax);

        // set armor and magic resist
        armorValueText.text = player.GetArmor().ToString();
        magicResistValueText.text = player.GetMagicResist().ToString();
    }

    /// <summary>
    /// Sets the silhouette image to the sprite for the player class
    /// </summary>
    void StartupSilhouette()
    {
        // set silhouette
        switch (pm.playerClass)
        {
            case EnumHandler.PlayerClasses.WARRIOR:
                silhouetteImage.sprite = warriorSilouette;
                break;
            case EnumHandler.PlayerClasses.MAGE:
                silhouetteImage.sprite = mageSilhouette;
                break;
            case EnumHandler.PlayerClasses.ARCHER:
                silhouetteImage.sprite = archerSilhouette;
                break;
        }
    }

    /// <summary>
    /// Calls the startup functions for Inventory and Equipment managers, as well as draws the player's inventory into the menu
    /// </summary>
    void StartupInventory()
    {
        InventoryManager.Startup();
        EquipmentManager.Startup();

        ClearInventoryUI();
        DrawInventory();
    }

    /// <summary>
    /// Used to clear out the player's inventory from the player menu
    /// </summary>
    void ClearInventoryUI()
    {
        // clear out any previous items in UI to draw new ones
        if (inventoryGrid == null)
        {
            inventoryGrid = transform.Find("PlayerInventoryPanel/InventoryGrid").GetComponent<GridLayoutGroup>();
        }

        foreach (Transform transform in inventoryGrid.transform)
        {
            Destroy(transform.gameObject);
        }
    }

    /// <summary>
    /// Instantiates an inventoryItemPrefab into the inventory grid for every item in player's inventory
    /// </summary>
    void DrawInventory()
    { 
        foreach (BaseItemScriptableObject item in InventoryManager.items)
        {
            GameObject newItemObject = Instantiate(GameAssets.i.inventoryItemPrefab, inventoryGrid.transform);
            newItemObject.transform.Find("ItemIcon").GetComponent<Image>().sprite = item.icon;

            newItemObject.GetComponent<ItemInterface>().SetItem(item);
        }
    }

    /// <summary>
    /// For every piece of equipment that is equipped by the player, the container for that slot has icon and EquipmentInterface/Equipment set.
    /// For every slot without equipment, the icon and EquipmentInterface/Equipment is set to null.
    /// </summary>
    void DrawEquipment()
    {
        // draw equipment part of the menu here
        //Debug.Log("Drawing Equipment");

        #region main/off hands

        if (player.GetEquippedMainHand() == null)
        {
            // Set blank icon
            mainHandContainer.transform.GetChild(2).GetComponent<Image>().sprite = GameAssets.i.blankMainHandIcon;
            // Set attached EquipmentInterface Equipment to null
            mainHandContainer.GetComponent<EquipmentInterface>().SetEquipment(null);
        }
        else
        {
            // Set equipped MainHand icon to mainHandContainer icon
            mainHandContainer.transform.GetChild(2).GetComponent<Image>().sprite = player.GetEquippedMainHand().icon;
            // and attached EquipmentInterface Equipment
            mainHandContainer.GetComponent<EquipmentInterface>().SetEquipment(player.GetEquippedMainHand());
        }

        if (player.GetEquippedOffHand() == null)
        {
            // Set blank icon
            offHandContainer.transform.GetChild(2).GetComponent<Image>().sprite = GameAssets.i.blankOffHandIcon;
            // Set attached EquipmentInterface Equipment to null
            offHandContainer.GetComponent<EquipmentInterface>().SetEquipment(null);
        }
        else
        {
            // Set equipped offHand icon to offHandContainer icon
            offHandContainer.transform.GetChild(2).GetComponent<Image>().sprite = player.GetEquippedOffHand().icon;
            // and attached EquipmentInterface Equipment
            offHandContainer.GetComponent<EquipmentInterface>().SetEquipment(player.GetEquippedOffHand());
        }

        #endregion

        #region armor

        if (player.GetEquippedHelm() == null)
        {
            // Set blank icon
            helmContainer.transform.GetChild(2).GetComponent<Image>().sprite = GameAssets.i.blankHelmIcon;
            // Set attached EquipmentInterface Equipment to null
            helmContainer.GetComponent<EquipmentInterface>().SetEquipment(null);
        }
        else
        {
            // Set equipped helm icon to helmContainer icon
            helmContainer.transform.GetChild(2).GetComponent<Image>().sprite = player.GetEquippedHelm().icon;
            // and attached EquipmentInterface Equipment
            helmContainer.GetComponent<EquipmentInterface>().SetEquipment(player.GetEquippedHelm());
        }

        if (player.GetEquippedChest() == null)
        {
            // Set blank icon
            chestContainer.transform.GetChild(2).GetComponent<Image>().sprite = GameAssets.i.blankChestIcon;
            // Set attached EquipmentInterface Equipment to null
            chestContainer.GetComponent<EquipmentInterface>().SetEquipment(null);
        }
        else
        {
            // Set equipped chest icon to chestContainer icon
            chestContainer.transform.GetChild(2).GetComponent<Image>().sprite = player.GetEquippedChest().icon;
            // and attached EquipmentInterface Equipment
            chestContainer.GetComponent<EquipmentInterface>().SetEquipment(player.GetEquippedChest());
        }

        if (player.GetEquippedHands() == null)
        {
            // Set blank icon
            handsContainer.transform.GetChild(2).GetComponent<Image>().sprite = GameAssets.i.blankHandsIcon;
            // Set attached EquipmentInterface Equipment to null
            handsContainer.GetComponent<EquipmentInterface>().SetEquipment(null);
        }
        else
        {
            // Set equipped hands icon to handsContainer icon
            handsContainer.transform.GetChild(2).GetComponent<Image>().sprite = player.GetEquippedHands().icon;
            // and attached EquipmentInterface Equipment
            handsContainer.GetComponent<EquipmentInterface>().SetEquipment(player.GetEquippedHands());
        }

        if (player.GetEquippedLegs() == null)
        {
            // Set blank icon
            legsContainer.transform.GetChild(2).GetComponent<Image>().sprite = GameAssets.i.blankLegsIcon;
            // Set attached EquipmentInterface Equipment to null
            legsContainer.GetComponent<EquipmentInterface>().SetEquipment(null);
        }
        else
        {
            // Set equipped legs icon to legsContainer icon
            legsContainer.transform.GetChild(2).GetComponent<Image>().sprite = player.GetEquippedLegs().icon;
            // and attached EquipmentInterface Equipment
            legsContainer.GetComponent<EquipmentInterface>().SetEquipment(player.GetEquippedLegs());
        }

        if (player.GetEquippedFeet() == null)
        {
            // Set blank icon
            feetContainer.transform.GetChild(2).GetComponent<Image>().sprite = GameAssets.i.blankFeetIcon;
            // Set attached EquipmentInterface Equipment to null
            feetContainer.GetComponent<EquipmentInterface>().SetEquipment(null);
        }
        else
        {
            // Set equipped feet icon to feetContainer icon
            feetContainer.transform.GetChild(2).GetComponent<Image>().sprite = player.GetEquippedFeet().icon;
            // and attached EquipmentInterface Equipment
            feetContainer.GetComponent<EquipmentInterface>().SetEquipment(player.GetEquippedFeet());
        }

        #endregion

        #region Jewelry

        if (player.GetEquippedAmulet() == null)
        {
            // Set blank icon
            amuletContainer.transform.GetChild(2).GetComponent<Image>().sprite = GameAssets.i.blankAmuletIcon;
            // Set attached EquipmentInterface Equipment to null
            amuletContainer.GetComponent<EquipmentInterface>().SetEquipment(null);
        }
        else
        {
            // Set equipped Amulet icon to amuletContainer icon
            amuletContainer.transform.GetChild(2).GetComponent<Image>().sprite = player.GetEquippedAmulet().icon;
            // and attached EquipmentInterface Equipment
            amuletContainer.GetComponent<EquipmentInterface>().SetEquipment(player.GetEquippedAmulet());
        }

        if (player.GetEquippedRingOne() == null)
        {
            // Set blank icon
            ringOneContainer.transform.GetChild(2).GetComponent<Image>().sprite = GameAssets.i.blankRingIcon;
            // Set attached EquipmentInterface Equipment to null
            ringOneContainer.GetComponent<EquipmentInterface>().SetEquipment(null);
        }
        else
        {
            // Set equipped Amulet icon to amuletContainer icon
            ringOneContainer.transform.GetChild(2).GetComponent<Image>().sprite = player.GetEquippedRingOne().icon;
            // and attached EquipmentInterface Equipment
            ringOneContainer.GetComponent<EquipmentInterface>().SetEquipment(player.GetEquippedRingOne());
        }

        if (player.GetEquippedRingTwo() == null)
        {
            // Set blank icon
            ringTwoContainer.transform.GetChild(2).GetComponent<Image>().sprite = GameAssets.i.blankRingIcon;
            // Set attached EquipmentInterface Equipment to null
            ringTwoContainer.GetComponent<EquipmentInterface>().SetEquipment(null);
            ringTwoContainer.GetComponent<EquipmentInterface>().ifRingTwo = false;
        }
        else
        {
            // Set equipped Amulet icon to amuletContainer icon
            ringTwoContainer.transform.GetChild(2).GetComponent<Image>().sprite = player.GetEquippedRingTwo().icon;
            // and attached EquipmentInterface Equipment
            ringTwoContainer.GetComponent<EquipmentInterface>().SetEquipment(player.GetEquippedRingTwo());
            ringTwoContainer.GetComponent<EquipmentInterface>().ifRingTwo = true;
        }

        #endregion
    }
}
