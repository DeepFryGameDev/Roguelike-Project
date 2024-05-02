using UnityEngine;

// Purpose: Contains primary functionality behind connecting GameObjects in the world to interactions with ScriptedEvents
// Directions: Attach to Player -> InteractPoint Transform
// Other notes: 

public class PlayerInteraction : MonoBehaviour
{
    InteractionHandler ih; // Used to display/hide interaction UI graphic, as well as set the interactedObject

    int layerMask = 1 << 9; // Inverted to only include layerMask 9 (interactable) - this ensures only interactable objects will receive instruction from the raycast

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
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, ih.interactDistance, layerMask))
        {
            if (ih.showInteractionRay) Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.green);

            ih.ToggleInteraction(true);
            ih.interactedObject = hit.transform.gameObject;
        }
        else
        {
            if (ih.showInteractionRay) Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * ih.interactDistance, Color.white);

            ih.ToggleInteraction(false);

            if (ih.interactedObject != null)
            {
                ih.interactedObject = null;
            }
        }
    }
}
