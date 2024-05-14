using UnityEngine;

// Purpose: Handles the processing of generating a new game
// Directions: Attach to MainMenu object on Main Menu scene
// Other notes:

public class NewGameSetup : MonoBehaviour
{
    BaseScriptedEvent bse;

    private void Start()
    {
        bse = FindObjectOfType<BaseScriptedEvent>();
    }

    public void StartGame()
    {
        bse.TransitionToScene(1);
    }
}
