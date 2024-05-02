using UnityEngine;

// Purpose: This does not do anything yet, but some camera based actions in PlayerMovement will be moved here
// Directions: Attach to [Cameras] Transform
// Other notes: 

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance; // This keeps the script persisting across scenes

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
