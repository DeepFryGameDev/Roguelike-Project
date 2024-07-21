using System.Collections;
using TMPro;
using UnityEngine;

// Purpose: Handles the processing of setting the player and camera variables for a new game
// Directions: Attach to MainMenu object on Main Menu scene.  Alternatively, attach to [Debugging] object on any scene that is intended to be tested from the editor.
// Other notes:

public class NewGameSetup : MonoBehaviour
{
    [Tooltip("Check this if starting from a scene that is not the main menu.  Player will be set up using class selected in PlayerManager on [Player] object")]
    [SerializeField] bool debugging;

    EnumHandler.PlayerClasses classToSet; // Set to the class that is either 1. Selected by the player on Main Menu, or 2. Chosen on [Player] PlayerManager dropdown when testing

    BaseScriptedEvent bse; // Used to transition to another scene (will be moved)

    PlayerManager playerManager; // Used to set the BasePlayer and AttackAnchor object to the Player Manager
    PlayerMovement playerMovement; // Used to set vars needed for player movement
    CameraManager cameraManager; // Used to set camera/cinemachine variables, such as LookAt and Follow
    AttackManager attackManager; // Used to set the BasePlayer and AttackAnchor object to the Attack Manager

    UIHandler uiHandler; // Used to display the resource and EXP panels

    AttackLoader attackLoader; // Used to set the player's default attacks

    ThirdPersonCam thirdPersonCam; // Used to set needed variables for thirdPersonCam script as well as hide the cursor when the game starts

    GameObject playerParent; // Set to the parent anchor for the player object "[Player]"

    SpawnPoint spawnPoint; // Used when testing the game - loads the player at the scene spawn point

    IEnumerator spawnPlayer; // Used as a coroutine to ensure the game is completely set before spawning the player

    MainMenuManager mainMenuManager; // Used to gather the stats set from the main menu when creating the player from 'New Game'

    // Player class prefab paths in Resources folder
    string playerWarriorPath = "CharacterPlayers/WarriorPlayer";
    string playerMagePath = "CharacterPlayers/MagePlayer";
    string playerArcherPath = "CharacterPlayers/ArcherPlayer";

    private void Start()
    {
        if (!GameManager.GetGameSet()) // Ensures this is only ran once, even if this script is on multiple scenes
        {
            InitialSetup();

            SceneInfo sceneInfo = FindAnyObjectByType<SceneInfo>();
        }        
    }

    /// <summary>
    /// Sets appropriate variables for this script to be able to set the rest of vars needed to start the game
    /// </summary>
    void InitialSetup()
    {
        bse = FindObjectOfType<BaseScriptedEvent>();
        playerManager = FindObjectOfType<PlayerManager>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        cameraManager = FindObjectOfType<CameraManager>();
        attackLoader = FindObjectOfType<AttackLoader>();
        attackManager = FindObjectOfType<AttackManager>();

        uiHandler = FindObjectOfType<UIHandler>();

        thirdPersonCam = cameraManager.GetComponentInChildren<ThirdPersonCam>();

        playerParent = GameObject.FindGameObjectWithTag("Player");

        mainMenuManager = FindObjectOfType<MainMenuManager>();

        spawnPlayer = SpawnPlayer();

        if (!debugging) // Starting from main menu
        {

        }
        else // Starting from another scene (likely DebugScene)
        {
            classToSet = playerManager.playerClass;
            spawnPoint = FindObjectOfType<SpawnPoint>();

            PlayerSetup();
        }
    }

    /// <summary>
    /// Called when clicking "Start Game" from the main menu
    /// </summary>
    public void StartGameFromMenu()
    {
        classToSet = GetClassFromDropdown();
        playerManager.playerClass = classToSet;

        // wait until scene is loaded before playerSetup
        PlayerSetup();

        StartCoroutine(spawnPlayer);
    }

    /// <summary>
    /// Coroutine - Continously runs until the GameManager is set, at which point player is spawned into the world via the SpawnPoint
    /// </summary>
    IEnumerator SpawnPlayer() // keep re-running this coroutine until GameManager.gameSet = true. then transition to scene
    {
        // Debug.Log("Checking if game is ready: " + GameManager.gameSet);
        yield return new WaitForEndOfFrame();
        
        if (!GameManager.GetGameSet())
        {
            // Debug.Log("Game not ready, re-checking");
            spawnPlayer = SpawnPlayer();
            StartCoroutine(spawnPlayer);
        } else
        {
            // once player is set, then transition:
            // Debug.Log("Game ready, spawning into game");
            bse.TransitionToScene(1);
            StopCoroutine(spawnPlayer);
        }        
    }

    /// <summary>
    /// Returns the PlayerClass from the given class choice in the main menu 'New Game' dropdown
    /// </summary>
    EnumHandler.PlayerClasses GetClassFromDropdown()
    {
        TMPro.TMP_Dropdown dropDown = transform.Find("NewGameSetupPanel/ClassDropdown").GetComponent<TMP_Dropdown>();

        switch (dropDown.value)
        {
            case 0: // Warrior
                return EnumHandler.PlayerClasses.WARRIOR;
            case 1: // Archer
                return EnumHandler.PlayerClasses.ARCHER;
            case 2: // Mage
                return EnumHandler.PlayerClasses.MAGE;
        }

        return EnumHandler.PlayerClasses.WARRIOR; // default
    }

    /// <summary>
    /// The overall method to process setting up needed variables for the player to be spawned into the worldfor the game to start
    /// </summary>
    void PlayerSetup()
    {
        // Debug.Log("Running player setup");

        GameObject newPlayerObject = null;

        // Instantiate player object
        switch (classToSet)
        {
            case EnumHandler.PlayerClasses.WARRIOR:
                newPlayerObject = Instantiate(Resources.Load<GameObject>(playerWarriorPath), playerParent.transform);
                break;

            case EnumHandler.PlayerClasses.MAGE:
                newPlayerObject = Instantiate(Resources.Load<GameObject>(playerMagePath), playerParent.transform);
                break;

            case EnumHandler.PlayerClasses.ARCHER:
                newPlayerObject = Instantiate(Resources.Load<GameObject>(playerArcherPath), playerParent.transform);
                break;
        }

        // Set up different scripts
        CameraSetup(newPlayerObject);

        PlayerMovementSetup(newPlayerObject);

        PlayerManagerSetup(newPlayerObject);

        EquipmentManager.Startup();

        InventoryManager.Startup();

        AttackManagerSetup(newPlayerObject);

        AttackLoaderSetup(newPlayerObject);

        if (!debugging) // Starting from main menu
        {

        }
        else // Starting from another scene
        {
            StartCoroutine(spawnPoint.SetSpawn(playerParent)); // Spawns player at designated spawnPoint location
        }

        thirdPersonCam.HideCursor();

        UISetup();

        // Setup complete vars to true
        playerManager.SetPlayerSet(true);
        thirdPersonCam.SetCameraSetupComplete(true);

        // Give player starter items
        playerManager.GiveStartupItems();

        // Set up stats for player
        PlayerStatsSetup(newPlayerObject.GetComponent<BasePlayer>());

        Debug.Log("Game set!");

        GameManager.SetGameSet(true);
    }

    /// <summary>
    /// Sets the player's initial stats when creating the game session (Will likely be updated/removed)
    /// </summary>
    /// <param name="player">The player to have stats set up on</param>
    void PlayerStatsSetup(BasePlayer player)
    {
        if (!debugging) // Starting from main menu
        {
            player.SetStrength(mainMenuManager.GetStrengthValueText());
            player.SetEndurance(mainMenuManager.GetEnduranceValueText());
            player.SetAgility(mainMenuManager.GetAgilityValueText());
            player.SetDexterity(mainMenuManager.GetDexterityValueText());
            player.SetIntelligence(mainMenuManager.GetIntelligenceValueText());
            player.SetResist(mainMenuManager.GetResistanceValueText());

            player.SetArmor(player.GetBaseArmor());
            player.SetMagicResist(player.GetBaseMagicResist());

            player.UpdateStaminaForMax();

            player.ReportStats();
        }
        else // Starting from another scene (debugging)
        {
            player.SetStrength(player.GetBaseStrength());
            player.SetEndurance(player.GetBaseEndurance());
            player.SetAgility(player.GetBaseAgility());
            player.SetDexterity(player.GetBaseDexterity());
            player.SetIntelligence(player.GetBaseIntelligence());
            player.SetResist(player.GetBaseResist());

            player.SetArmor(player.GetBaseArmor());
            player.SetMagicResist(player.GetBaseMagicResist());

            player.SetCurrentHP(player.GetMaxHP());

            player.UpdateStaminaForMax();

            player.ReportStats();
        }
    }

    /// <summary>
    /// Hides/Shows the various UI elements needed when starting the game
    /// </summary>
    void UISetup()
    {
        uiHandler.UIStartup();
    }

    /// <summary>
    /// Sets the various vars required for game creation for player movement
    /// </summary>
    void PlayerMovementSetup(GameObject newPlayerObject)
    {
        playerMovement.SetVars(newPlayerObject);
    }

    /// <summary>
    /// Sets the vars required for the PlayerManager to function during gameplay
    /// </summary>
    void PlayerManagerSetup(GameObject newPlayerObject)
    {
        playerManager.Setup(newPlayerObject);
    }

    /// <summary>
    /// Sets the vars required for the AttackManager to function during gameplay
    /// </summary>
    void AttackManagerSetup(GameObject newPlayerObject)
    {
        attackManager.SetBasePlayer(newPlayerObject.GetComponent<BasePlayer>());
        attackManager.SetAttackAnchor(newPlayerObject.transform.Find("[AttackAnchor]"));
    }

    /// <summary>
    /// Sets the vars required for the AttackLoader to function during gameplay
    /// </summary>
    void AttackLoaderSetup(GameObject newPlayerObject)
    {
        attackLoader.SetAttackCollisionTriggerTransform(newPlayerObject.transform.Find("[AttackCollisionTriggers]"));
        attackLoader.SetPlayer(newPlayerObject.GetComponent<BasePlayer>());

        // attackLoader.SetAttackCollisionTriggers();
    }

    /// <summary>
    /// Sets the vars required for the camera to perform the needed functions during gameplay
    /// </summary>
    void CameraSetup(GameObject newPlayerObject)
    {
        thirdPersonCam.SetPlayerParent(playerParent.transform);
        thirdPersonCam.SetPlayerObj(newPlayerObject.transform);

        thirdPersonCam.SetOrientation(newPlayerObject.transform.Find("Orientation"));
        thirdPersonCam.SetCombatLookAt(newPlayerObject.transform.Find("Orientation/CombatLookAt"));

        cameraManager.GetCombatCam().GetComponent<Cinemachine.CinemachineFreeLook>().LookAt = newPlayerObject.transform.Find("Orientation/CombatLookAt");

        cameraManager.SetCamerasSet(true);
    }
}
