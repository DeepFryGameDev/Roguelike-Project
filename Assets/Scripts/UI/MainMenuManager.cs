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

    NewGameSetup ngs;

    UIHandler uiHandler;

    private void Start()
    {
        ngs = GetComponent<NewGameSetup>();

        uiHandler = FindObjectOfType<UIHandler>();
    }

    public void NewGameButtonPressed()
    {
        // Hide MainMenu panel
        uiHandler.ToggleObject(MainMenuPanel, false);

        // Display NewGameSetup panel
        uiHandler.ToggleObject(NewGameSetupPanel, true);
    }

    public void LoadGameButtonPressed()
    {

    }

    public void OptionsButtonPressed()
    {

    }

    public void QuitButtonPressed()
    {
        Application.Quit();
    }

    public void NewGameStartButtonPressed()
    {
        ngs.StartGame();
    }

    public void NewGameBackButtonPressed()
    {
        // Hide NewGameSetup panel
        uiHandler.ToggleObject(NewGameSetupPanel, false);

        // Display MainMenu panel
        uiHandler.ToggleObject(MainMenuPanel, true);
    }
}
