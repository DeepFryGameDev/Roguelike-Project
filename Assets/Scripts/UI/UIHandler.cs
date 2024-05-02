using UnityEngine;

// Purpose: 
// Directions: 
// Other notes:

public class UIHandler : MonoBehaviour
{
    public static UIHandler instance;

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
}
