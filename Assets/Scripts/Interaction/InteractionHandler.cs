using System;
using TMPro;
using UnityEngine;

// Purpose: Used to manage interactions between the player and objects in the world that can utilize scripts to perform events
// Directions: Attach to the [System] GameObject
// Other notes: 

public class InteractionHandler : MonoBehaviour
{
    [Tooltip("Distance from the player to the object before interaction is available")]
    [SerializeField] float interactDistance;
    public float GetInteractDistance() { return  interactDistance; }

    [Tooltip("If true, will display the ray from the player to show where interaction is available")]
    [SerializeField] bool showInteractionRay;
    public bool GetShowInteractionRay() { return showInteractionRay; }

    [Tooltip("Key for the player to press to interact with a given object in the world")]
    [SerializeField] KeyCode interactKey;

    // Will turn true when an interaction is in range of the player and the player is able to start the interaction process
    bool interactionReady;

    // Object in the world that is being interacted with
    GameObject interactedObject;   
    public GameObject GetInteractedObject() { return interactedObject; }
    public void SetInteractedObject(GameObject gameObject) { interactedObject = gameObject;}

    // Used to display the interaction graphic when interaction is available
    PrefabManager pm;

    // Used to set the interaction graphic to display the key set by interactionKey
    TextMeshProUGUI interactText;

    // Singleton to keep the GameObject persisting across scenes
    public static InteractionHandler instance; 

    void Awake()
    {
        Singleton();
    }

    void Singleton()
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

    void Start()
    {
        SetVars();

        SetInteractKeyText();
    }

    void SetVars()
    {
        pm = FindAnyObjectByType<PrefabManager>();

        interactText = pm.GetInteractionKeyPanel().GetComponentInChildren<TextMeshProUGUI>();
    }

    /// <summary>
    /// Sets the interact key letter text (to lowercase) on the interaction UI graphic
    /// </summary>
    void SetInteractKeyText()
    {
        string interactKeyLowerCase = interactKey.ToString().ToLower();

        interactText.text = interactKeyLowerCase;
    }

    void Update()
    {
        if (interactionReady && Input.GetKeyDown(interactKey))
        {
            Debug.Log("Interacting with " + interactedObject.name);

            BaseInteractable bi = interactedObject.GetComponent<BaseInteractable>();
            if (bi != null)
            {
                bi.OnInteract();
            }
        }
    }

    /// <summary>
    /// Displays or hides the interaction UI graphic
    /// </summary>
    /// <param name="toggle">True to display interaction UI graphic, False to hide it</param>
    public void ToggleInteraction(bool toggle)
    {
        pm.GetInteractionKeyPanel().SetActive(toggle);

        interactionReady = toggle;
    }
}
