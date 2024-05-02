using UnityEngine;

// Purpose: Handles the player's stamina as well as granting them experience
// Directions: Attach to Player GameObject
// Other notes: Can probably move the experience methods to the base player and leave there

public class PlayerManager : MonoBehaviour
{
    [Header("Class")]
    [Tooltip("The player's current class")]
    public EnumHandler.PlayerClasses playerClass;

    [Header("EXP")]
    [Tooltip("Index 0 is treated as level 1 - array of experience points needed to progress to the next level")]
    public int[] expToNextLevel;

    // ---
    [Header("Sprinting")]
    [Tooltip("Base value for sprinting speed")]
    [SerializeField] float sprintBase = 1;
    [Tooltip("Multiplied by the sprintBase to determine final sprint speed")]
    [SerializeField] float sprintModifier = 20;
    [Tooltip("Base value for recovering stamina after sprinting")]
    [SerializeField] float sprintRecoverBase = 1;
    [Tooltip("Multiplied by the sprintRecoverBase to determine final stamina recovery rate")]
    [SerializeField] float sprintRecoverModifier = 10;
    [Tooltip("When standing still, the stamina recovery rate is increased by this value")]
    [SerializeField] float standingRecoveryModifier = .5f;

    [Tooltip("Turns true if player has used up all of their stamina and needs to recover fully")]
    [ReadOnly] public bool stamDepleted;
    [Tooltip("Turns true if player is not moving")]
    [ReadOnly] public bool standingStill;

    bool recoveringStamina; // Turns true if player is recovering stamina from sprinting
    Color baseStamBarColor; // Default value of stamina bar color

    BasePlayer player; // Used to manipulate stamina

    PrefabManager prefabManager; // Used to manipulate stamina bar on UI

    void Start()
    {
        SetVars();
    }

    void SetVars()
    {
        player = GetComponent<BasePlayer>();
        prefabManager = FindObjectOfType<PrefabManager>();

        baseStamBarColor = prefabManager.stamBarImage.color;
    }

    /// <summary>
    /// Adds the given amount of EXP to the player's total experience
    /// </summary>
    /// <param name="exp">Amount of experience to be added</param>
    public void GrantExp(int exp)
    {
        player.GainExp(exp);

        // update UI
    }

    /// <summary>
    /// Ensures the player has enough stamina to sprint, and if so, lowers the player's stamina
    /// </summary>
    public void ReduceStaminaFromSprint() // called every frame player is moving with sprint held down
    {
        if (player.currentStamina > 0 && !stamDepleted)
        {
            player.currentStamina -= ((sprintBase * sprintModifier) * Time.deltaTime);

            float temp = (player.currentStamina / player.maxStamina) * 1;

            prefabManager.staminaSlider.value = temp;

            if (!recoveringStamina)
                recoveringStamina = true;
        }
        
        if (player.currentStamina <= 0) // Player is out of stamina from running
        {
            prefabManager.stamBarImage.color = prefabManager.outOfStaminaBarColor;
            stamDepleted = true;
        }
    }

    /// <summary>
    /// Ensures the player's stamina is in a recoverable state, and if so, recovers the stamina
    /// </summary>
    public void RecoverStamina()
    {
        if (player.currentStamina < player.maxStamina && recoveringStamina)
        {
            if (!stamDepleted)
            {
                if (standingStill)
                {
                    player.currentStamina += ((sprintRecoverBase * sprintRecoverModifier) * Time.deltaTime);
                }
                else
                {
                    player.currentStamina += ((sprintRecoverBase * sprintRecoverModifier) * Time.deltaTime) * standingRecoveryModifier;
                }
            } else
            {
                player.currentStamina += ((sprintRecoverBase * sprintRecoverModifier) * Time.deltaTime);
            }


            float temp = (player.currentStamina / player.maxStamina) * 1;

            prefabManager.staminaSlider.value = temp;
        }

        if (player.currentStamina >= player.maxStamina && recoveringStamina)
        {
            recoveringStamina = false;
            player.currentStamina = player.maxStamina;

            if (stamDepleted)
            {
                prefabManager.stamBarImage.color = baseStamBarColor;
                stamDepleted = false;
            }            
        }
    }
}
