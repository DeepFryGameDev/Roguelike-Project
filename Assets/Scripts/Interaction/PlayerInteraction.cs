using UnityEngine;

// Purpose: Contains primary functionality behind connecting GameObjects in the world to interactions with ScriptedEvents
// Directions: Attach to Player -> InteractPoint Transform
// Other notes: 

public class PlayerInteraction : MonoBehaviour
{
    InteractionHandler ih; // Used to display/hide interaction UI graphic, as well as set the interactedObject

    int layerMask = 1 << 9; // Set to layer layerInteractable - this ensures only interactable objects will receive instruction from the raycast

    void Start()
    {
        SetVars();
    }

    void SetVars()
    {
        ih = FindObjectOfType<InteractionHandler>();
    }

    void FixedUpdate()
    {
        CheckForAvailableInteraction();
    }

    /// <summary>
    /// Draws a ray from the player's gameObject in the direction they are facing, with distance being the interactDistance in InteractionHandler
    /// If an interactable gameObject is detected, the interaction UI graphic is displayed and the object hit is set as the interactedObject
    /// </summary>
    void CheckForAvailableInteraction()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, ih.GetInteractDistance(), layerMask))
        {
            if (ih.GetShowInteractionRay()) Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.green);

            ih.ToggleInteraction(true);
            ih.SetInteractedObject(hit.transform.gameObject);
        }
        else
        {
            if (ih.GetInteractedObject()) Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * ih.GetInteractDistance(), Color.white);

            ih.ToggleInteraction(false);

            if (ih.GetInteractedObject() != null)
            {
                ih.SetInteractedObject(null);
            }
        }
    }
}
