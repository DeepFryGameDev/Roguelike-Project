using UnityEngine;

// Purpose: Contains functionality to allow an object in the world to interact with the player
// Directions: Attach to any object used for interactions with the player
// Other notes: 

public class SlimeTestEvent : BaseInteractable
{
    PlayerManager playerManager;

    void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
    }

    /// <summary>
    /// This method should be used on every interactable in the world
    /// Can be customized to perform any event in BaseScriptedEvent by referencing bse
    /// </summary>
    public override void OnInteract()
    {
        base.OnInteract();

        // Check if player has weapon equipped. If no weapon equipped, cannot transition
        // Better user feedback will need to be implemented
        if (playerManager.GetPlayer().GetEquippedMainHand() != null)
        {
            bse.TransitionToScene(2);
        } else
        {
            Debug.Log("Unable to transition to battle without weapon equipped");
        }        
    }
}
