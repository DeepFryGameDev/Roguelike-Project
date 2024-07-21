using UnityEngine;

// Purpose: Handles the processing for the buttons on the Main Menu
// Directions: Attach to MainMenu object on Main Menu scene
// Other notes:

public class MainMenuManager : MonoBehaviour
{
    [Tooltip("Set manually to the main menu panel")]
    [SerializeField] GameObject MainMenuPanel;

    [Tooltip("Set manually to the new game setup panel")]
    [SerializeField] GameObject NewGameSetupPanel;

    [Tooltip("Set manually to the Text field to be set when player changes value for strength")]
    [SerializeField] TMPro.TextMeshProUGUI StrengthValueText;
    public int GetStrengthValueText() { return int.Parse(StrengthValueText.text); }

    [Tooltip("Set manually to the Text field to be set when player changes value for endurance")]
    [SerializeField] TMPro.TextMeshProUGUI EnduranceValueText;
    public int GetEnduranceValueText() { return int.Parse(EnduranceValueText.text); }

    [Tooltip("Set manually to the Text field to be set when player changes value for agility")]
    [SerializeField] TMPro.TextMeshProUGUI AgilityValueText;
    public int GetAgilityValueText() { return int.Parse(AgilityValueText.text); }

    [Tooltip("Set manually to the Text field to be set when player changes value for dexterity")]
    [SerializeField] TMPro.TextMeshProUGUI DexterityValueText;
    public int GetDexterityValueText() { return int.Parse(DexterityValueText.text); }

    [Tooltip("Set manually to the Text field to be set when player changes value for intelligence")]
    [SerializeField] TMPro.TextMeshProUGUI IntelligenceValueText;
    public int GetIntelligenceValueText() { return int.Parse(IntelligenceValueText.text); }

    [Tooltip("Set manually to the Text field to be set when player changes value for resistance")]
    [SerializeField] TMPro.TextMeshProUGUI ResistanceValueText;
    public int GetResistanceValueText() { return int.Parse(ResistanceValueText.text); }

    [Tooltip("Set manually to the Text Input field for changing the player's name")]
    public TMPro.TMP_InputField nameInputField;

    [Tooltip("Set manually to the Text field to be set for remaining points")]
    [SerializeField] TMPro.TextMeshProUGUI RemainingPointsText;

    [Tooltip("Set manually to the dropdown field for the class dropdown")]
    [SerializeField] TMPro.TMP_Dropdown classDropdown;

    [Tooltip("Set manually to the BasePlayer field to be set for Warrior prefab")]
    [SerializeField] BasePlayer WarriorClass;

    [Tooltip("Set manually to the BasePlayer field to be set for Mage prefab")]
    [SerializeField] BasePlayer MageClass;

    [Tooltip("Set manually to the BasePlayer field to be set for Archer prefab")]
    [SerializeField] BasePlayer ArcherClass;

    int maxStatValue = 5; // maximum value that the player is allowed to allocate to one stat while creating character

    int remainingPoints = 0; // remaining stat allocation points to the player - this is adjusted when the player clicks 'up/down' on the stat arrows

    NewGameSetup ngs; // Used to set all the required variables for starting a game when the player hits "Start Game" button

    UIHandler uiHandler; // Used to change visibility of various UI elements

    private void Start()
    {
        ngs = GetComponent<NewGameSetup>();

        uiHandler = FindObjectOfType<UIHandler>();

        SetDefaultStatsText();
    }

    /// <summary>
    /// Is called when the player presses the 'New Game' button on the main menu
    /// </summary>
    public void NewGameButtonPressed()
    {
        // Hide MainMenu panel
        uiHandler.ToggleObject(MainMenuPanel, false);

        // Display NewGameSetup panel
        uiHandler.ToggleObject(NewGameSetupPanel, true);
    }

    /// <summary>
    /// Not yet implemented - is called when the player presses the 'Load Game' button on the main menu
    /// </summary>
    public void LoadGameButtonPressed()
    {

    }

    /// <summary>
    /// Not yet implemented - is called when player presses the 'Options' button on the main menu
    /// </summary>
    public void OptionsButtonPressed()
    {

    }

    /// <summary>
    /// Closes the program when the player hits 'Quit' button on the main menu
    /// </summary>
    public void QuitButtonPressed()
    {
        Application.Quit();
    }

    /// <summary>
    /// Called when the player has entered their name and selected a class, and presses the "Start Game" button on the main menu
    /// </summary>
    public void NewGameStartButtonPressed()
    {
        ngs.StartGameFromMenu();
    }

    /// <summary>
    /// Called when the player has opened the "New Game" panel, but then clicks "Back" to the main menu
    /// </summary>
    public void NewGameBackButtonPressed()
    {
        // Hide NewGameSetup panel
        uiHandler.ToggleObject(NewGameSetupPanel, false);

        // Display MainMenu panel
        uiHandler.ToggleObject(MainMenuPanel, true);
    }

    #region Stat Button Functions

    /// <summary>
    /// Called when the player presses the up or down buttons for Strength stat on New Game Setup
    /// </summary>
    /// <param name="num">+1 or -1</param>
    public void StrengthButtonPressed(int num)
    {
        int strengthValue = GetIntFromText(StrengthValueText);

        if (num > 0 && strengthValue < maxStatValue && remainingPoints > 0)
        {
            remainingPoints--;
            strengthValue++;
        } else if (num < 0 && strengthValue > 1)
        {
            remainingPoints++;
            strengthValue--;
        }

        RemainingPointsText.text = remainingPoints.ToString();
        StrengthValueText.text = strengthValue.ToString();
    }

    /// <summary>
    /// Called when the player presses the up or down buttons for Endurance stat on New Game Setup
    /// </summary>
    /// <param name="num">+1 or -1</param>
    public void EnduranceButtonPressed(int num)
    {
        int enduranceValue = GetIntFromText(EnduranceValueText);

        if (num > 0 && enduranceValue < maxStatValue && remainingPoints > 0)
        {
            remainingPoints--;
            enduranceValue++;
        }
        else if (num < 0 && enduranceValue > 1)
        {
            remainingPoints++;
            enduranceValue--;
        }

        RemainingPointsText.text = remainingPoints.ToString();
        EnduranceValueText.text = enduranceValue.ToString();
    }

    /// <summary>
    /// Called when the player presses the up or down buttons for Agility stat on New Game Setup
    /// </summary>
    /// <param name="num">+1 or -1</param>
    public void AgilityButtonPressed(int num)
    {
        int agilityValue = GetIntFromText(AgilityValueText);

        if (num > 0 && agilityValue < maxStatValue && remainingPoints > 0)
        {
            remainingPoints--;
            agilityValue++;
        }
        else if (num < 0 && agilityValue > 1)
        {
            remainingPoints++;
            agilityValue--;
        }

        RemainingPointsText.text = remainingPoints.ToString();
        AgilityValueText.text = agilityValue.ToString();
    }

    /// <summary>
    /// Called when the player presses the up or down buttons for Dexterity stat on New Game Setup
    /// </summary>
    /// <param name="num">+1 or -1</param>
    public void DexterityButtonPressed(int num)
    {
        int dexterityValue = GetIntFromText(DexterityValueText);

        if (num > 0 && dexterityValue < maxStatValue && remainingPoints > 0)
        {
            remainingPoints--;
            dexterityValue++;
        }
        else if (num < 0 && dexterityValue > 1)
        {
            remainingPoints++;
            dexterityValue--;
        }

        RemainingPointsText.text = remainingPoints.ToString();
        DexterityValueText.text = dexterityValue.ToString();
    }

    /// <summary>
    /// Called when the player presses the up or down buttons for Intelligence stat on New Game Setup
    /// </summary>
    /// <param name="num">+1 or -1</param>
    public void IntelligenceButtonPressed(int num)
    {
        int intelligenceValue = GetIntFromText(IntelligenceValueText);

        if (num > 0 && intelligenceValue < maxStatValue && remainingPoints > 0)
        {
            remainingPoints--;
            intelligenceValue++;
        }
        else if (num < 0 && intelligenceValue > 1)
        {
            remainingPoints++;
            intelligenceValue--;
        }

        RemainingPointsText.text = remainingPoints.ToString();
        IntelligenceValueText.text = intelligenceValue.ToString();
    }

    /// <summary>
    /// Called when the player presses the up or down buttons for Resistance stat on New Game Setup
    /// </summary>
    /// <param name="num">+1 or -1</param>
    public void ResistanceButtonPressed(int num)
    {
        int resistanceValue = GetIntFromText(ResistanceValueText);

        if (num > 0 && resistanceValue < maxStatValue && remainingPoints > 0)
        {
            remainingPoints--;
            resistanceValue++;
        }
        else if (num < 0 && resistanceValue > 1)
        {
            remainingPoints++;
            resistanceValue--;
        }

        RemainingPointsText.text = remainingPoints.ToString();
        ResistanceValueText.text = resistanceValue.ToString();
    }

    /// <summary>
    /// Sets the player's default stats using the chosen class base values
    /// </summary>
    public void SetDefaultStatsText()
    {
        remainingPoints = 0;
        RemainingPointsText.text = remainingPoints.ToString();

        BasePlayer classToSet = null;

        switch (classDropdown.value)
        {
            case 0: // WARRIOR
                classToSet = WarriorClass;
                break;
            case 1: // ARCHER
                classToSet = ArcherClass;
                break;
            case 2: // MAGE
                classToSet = MageClass;
                break;
        }

        StrengthValueText.text = classToSet.GetBaseStrength().ToString();
        EnduranceValueText.text = classToSet.GetBaseEndurance().ToString();
        AgilityValueText.text = classToSet.GetBaseAgility().ToString();
        DexterityValueText.text = classToSet.GetBaseDexterity().ToString();
        IntelligenceValueText.text = classToSet.GetBaseIntelligence().ToString();
        ResistanceValueText.text = classToSet.GetBaseResist().ToString();
    }

    /// <summary>
    /// Simply returns an integer from a TextMeshPro text field
    /// </summary>
    /// <param name="text">The TextMeshProUGUI text field to pull integer from</param>
    /// <returns>Integer format of the contents of the text field</returns>
    int GetIntFromText(TMPro.TextMeshProUGUI text)
    {
        return int.Parse(text.text);
    }

    #endregion
}
