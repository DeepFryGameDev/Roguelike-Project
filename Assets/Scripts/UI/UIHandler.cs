using UnityEngine;

// Purpose: Handles various UI related processes
// Directions: Attach to [UI]
// Other notes:

public class UIHandler : MonoBehaviour
{
    public static UIHandler instance;

    [SerializeField] GameObject GameOverPanel;

    void Awake()
    {
        if (instance == null) //check if instance exists
        {
            instance = this; //if not set the instance to this
        }
        else if (instance != this) //if it exists but is not this instance
        {
            Destroy(gameObject); //destroy it
        }
        DontDestroyOnLoad(gameObject); //set this to be persistable across scenes
    }

    /// <summary>
    /// Shows Game Over panel and options
    /// </summary>
    public void DisplayGameOver()
    {
        ToggleObject(GameOverPanel, true);
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
