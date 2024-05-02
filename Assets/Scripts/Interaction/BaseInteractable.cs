using UnityEngine;

// Purpose: Used for anything in the world that can be interacted with by the player - simply connects the world to the BaseScriptedEvent script
// Directions: Create a new script and inherit this class, and refer to bse to use any script
// Other notes: 

public class BaseInteractable : MonoBehaviour
{
    protected BaseScriptedEvent bse;

    void Awake()
    {
        bse = FindFirstObjectByType<BaseScriptedEvent>();
    }

    /// <summary>
    /// Should be overridden by any script that inherits this
    /// This is called when the player interacts with the object in the world
    /// Interaction key is set in InteractionHandler - default is 'e'
    /// </summary>
    public virtual void OnInteract()
    {

    }
}
