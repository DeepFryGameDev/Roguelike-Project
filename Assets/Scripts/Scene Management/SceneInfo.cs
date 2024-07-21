using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Purpose: Unsure if this is needed yet.
// Directions: Create a SceneInfo object in the editor, and attach this to it.
// Other notes:

public class SceneInfo : MonoBehaviour
{
    [Tooltip("If the scene is a battle scene - used to determine if attacks should be fired, and sets camera mode")]
    public bool battleScene;

    public int sceneIndex; // Set to the index of the scene in build order

    AttackLoader attackLoader; // Used to instantiate/clear orbit particles based on battleScene

    CameraManager cameraManager; // Used to switch between camera modes

    PlayerMenuHandler playerMenuHandler; // Used to disable the player's ability to open the player menu while on main menu, as well as refresh the UI when the scene is loaded

    UIHandler uiHandler; // Used to generate the cooldown radials for the scene when it is loaded

    IEnumerator waitForGameSet; // Used to run methods when the game is fully loaded and scene is finished loading (can maybe remove)

    bool sceneSet; // Set to true when the scene setup has been completed

    private void Awake()
    {
        attackLoader = FindObjectOfType<AttackLoader>();

        cameraManager = FindObjectOfType<CameraManager>();

        playerMenuHandler = FindObjectOfType<PlayerMenuHandler>();

        uiHandler = FindObjectOfType<UIHandler>();

        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Run anything here that should happen when scene is first loaded

        if (sceneIndex != 0) // Not running functions when on main menu
        {
            SceneLoaded();
        } else
        {
            Debug.LogWarning("Scene Index 0 detected.  Main Menu scene is expected");
        }        
    }

    private void Update()
    {
        SceneSetup();
    }

    /// <summary>
    /// When the scene is loaded, and game is set, this function is ran.  Used to process anything needed when a scene is freshly loaded.
    /// </summary>
    void ProcessSceneLoadedFunctions()
    {
        if (sceneIndex == 0) // on main menu
        {
            ToggleCanOpenMenu(false);
        }
        else // on any other scene
        {
            ToggleCanOpenMenu(true);

            // Refresh UI
            playerMenuHandler.RefreshUI();

            uiHandler.GenerateCooldownRadials();
        }
    }

    /// <summary>
    /// Starts the coroutine to continuously check if the game is set before methods are run
    /// </summary>
    void SceneLoaded()
    {
        waitForGameSet = WaitForGameSet();
        StartCoroutine(waitForGameSet);
    }

    /// <summary>
    /// Continuously runs until the scene is loaded and game is set, at which point ProcessSceneLoadedFunctions is called
    /// </summary>
    IEnumerator WaitForGameSet()
    {
        if (!GameManager.GetGameSet())
        {
            // Debug.Log("Scene is loaded but game not set. Waiting...");
            yield return new WaitForEndOfFrame();
            waitForGameSet = WaitForGameSet();
            StartCoroutine(waitForGameSet);
        } else
        {
            // Debug.Log("Scene loaded and game set!");
            ProcessSceneLoadedFunctions();
        }
    }

    /// <summary>
    /// Enables or disables the ability for the player to open the player menu.
    /// </summary>
    /// <param name="toggle">If player should be able to open menu, set to true. Conversely, set to false.</param>
    void ToggleCanOpenMenu(bool toggle)
    {
        playerMenuHandler.canOpenMenu = toggle;
    }

    /// <summary>
    /// Instantiates/clears orbit particles and sets camera mode based on if the scene is a battle scene
    /// </summary>
    void SceneSetup()
    {
        if (!sceneSet && cameraManager.GetCamerasSet())
        {
            if (battleScene)
            {
                attackLoader.InstantiateOrbitParticles();

                cameraManager.SwitchCameraMode(EnumHandler.CameraModes.COMBAT);
            }
            else
            {
                attackLoader.ClearOrbitParticles();

                cameraManager.SwitchCameraMode(EnumHandler.CameraModes.BASIC);
            }
            sceneSet = true;
        }
    }
}
