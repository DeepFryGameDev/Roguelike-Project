using UnityEngine;

// Purpose: Handles the player's resources (exp, health, stamina, etc) and UI elements to be updated with them, along with setting player stats
// Directions: Attach to Player GameObject
// Other notes: Can probably move the experience methods to the base player and leave there

public class PlayerManager : MonoBehaviour
{
    [Header("Class")]
    [Tooltip("The player's current class - used to instantiate the player object and set vars")]
    public EnumHandler.PlayerClasses playerClass;

    [Header("EXP")]
    [Tooltip("Index 0 is treated as level 1 - array of experience points needed to progress to the next level")]
    public int[] expToNextLevel;

    // ---
    [Header("Moving")]
    float baseMoveSpeed = 3; // The move speed value to start calculation
    float agilityToMoveSpeedMod = .5f; // For each point of agility, move speed is increased by this value

    [Header("Sprinting")]
    [Tooltip("Base value for sprinting speed")]
    [SerializeField] float sprintBase = 1;
    [Tooltip("Multiplied by the sprintBase to determine final sprint speed")]
    [SerializeField] float sprintModifier = 20;
    [Tooltip("Base value for recovering stamina after sprinting")]
    [SerializeField] float sprintRecoverBase = 2;
    [Tooltip("Multiplied by the sprintRecoverBase to determine final stamina recovery rate")]
    [SerializeField] float sprintRecoverModifier = 2.5f;
    [Tooltip("When standing still, the stamina recovery rate is increased by this value")]
    [SerializeField] float standingRecoveryModifier = .5f;

    float agilityToStaminaRecoveryMod = 1.5f; // Stamina recovery rate is increased by this value for each point in agility

    // Current Experience Points
    int exp;

    // Current level
    int level;

    // Experience points needed to progress to the next level
    int currentExpToLevel;

    // Turns true if player has used up all of their stamina and needs to recover fully
    bool stamDepleted;
    public bool GetStamDepleted() { return stamDepleted; }

    // Turns true if player is not moving
    bool standingStill;
    public void SetStandingStill(bool standingStill) { this.standingStill = standingStill; }

    bool recoveringStamina; // Turns true if player is recovering stamina from sprinting
    Color baseStamBarColor; // Default value of stamina bar color

    BasePlayer player; // Used to manipulate stamina and get global variable for player
    public BasePlayer GetPlayer() { return player; }

    PrefabManager prefabManager; // Used to manipulate stamina bar on UI

    static PlayerManager instance; // For singleton to ensure script persists across scenes

    bool playerSet; // Set to true when the playerManager vars have all been set up for game startup
    public void SetPlayerSet(bool set) { this.playerSet = set; }
    public bool GetPlayerSet() { return playerSet; }

    private void Awake()
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

    void SetVars(GameObject playerParent)
    {
        player = playerParent.GetComponent<BasePlayer>();

        prefabManager = FindObjectOfType<PrefabManager>();

        baseStamBarColor = prefabManager.GetStaminaBarImage().color;

        level = 1;

        currentExpToLevel = expToNextLevel[level - 1];
    }

    /// <summary>
    /// Used to calculate the player's base movement speed to be used before sprint speed calculations
    /// </summary>
    /// <returns>Value calculated from adding baseMoveSpeed to the player's agility * the agilityToMoveSpeed modifier</returns>
    public float GetMoveSpeed()
    {
        return (baseMoveSpeed + (agilityToMoveSpeedMod * player.GetAgility()));
    }

    /// <summary>
    /// Ran during game setup to set variables needed for playing the game
    /// </summary>
    public void Setup(GameObject playerParent)
    {
        SetVars(playerParent);
        SetClass();
    }

    /// <summary>
    /// Used to provide the player with equipment/items to start the game.
    /// </summary>
    public void GiveStartupItems()
    {
        switch (playerClass)
        {
            case EnumHandler.PlayerClasses.WARRIOR:
                // InventoryManager.AddItem(GameItems.i.testSword);
                // InventoryManager.AddItem(GameItems.i.testShield);

                EquipmentManager.EquipItem((BaseEquipmentScriptableObject)GameItems.i.testSword);
                EquipmentManager.EquipItem((BaseEquipmentScriptableObject)GameItems.i.testShield);

                break;
            case EnumHandler.PlayerClasses.MAGE:
                // InventoryManager.AddItem(GameItems.i.testStaff);

                EquipmentManager.EquipItem((BaseEquipmentScriptableObject)GameItems.i.testStaff);
                break;
            case EnumHandler.PlayerClasses.ARCHER:
                // InventoryManager.AddItem(GameItems.i.testBow);

                EquipmentManager.EquipItem((BaseEquipmentScriptableObject)GameItems.i.testBow);
                break;
        }

        InventoryManager.AddItem(GameItems.i.testHelm);
        InventoryManager.AddItem(GameItems.i.testChest);
        InventoryManager.AddItem(GameItems.i.testHands);
        InventoryManager.AddItem(GameItems.i.testLegs);
        InventoryManager.AddItem(GameItems.i.testFeet);

        InventoryManager.AddItem(GameItems.i.testAmulet);
        InventoryManager.AddItem(GameItems.i.testRingOne);
        InventoryManager.AddItem(GameItems.i.testRingTwo);
    }

    /// <summary>
    /// Not yet implemented, but will set sprint rate, etc based on class
    /// </summary>
    void SetClass()
    {
        switch (playerClass)
        {
            case EnumHandler.PlayerClasses.WARRIOR:

                break;

            case EnumHandler.PlayerClasses.MAGE:

                break;

            case EnumHandler.PlayerClasses.ARCHER:

                break;
        }
    }

    /// <summary>
    /// Increases player's experience points by provided value, and if the player's total experience points exceed the required amount to level up, initiates the level up method
    /// </summary>
    /// <param name="expToGain">Amount of experience points to add to player's exp</param>
    public void GainExp(int expToGain)
    {
        exp += expToGain;

        if (exp >= currentExpToLevel)
        {
            LevelUp();
            exp = 0;
        }

        // update UI
    }

    /// <summary>
    /// Increases the player's level, and should prompt them for upgrading their attack or adding a new one (not yet added)
    /// </summary>
    void LevelUp()
    {
        level++;
    }

    /// <summary>
    /// Keeps health bar UI element updated when HP changes
    /// </summary>
    public void UpdateHealthBar()
    {
        float temp = (float)player.GetCurrentHP() / (float)player.GetMaxHP();

        //Debug.Log("Current HP: " + player.GetCurrentHP() + " / MaxHP: " + player.GetMaxHP() + " = " + (temp * 100) + "%");
        prefabManager.GetHealthSlider().value = temp;
    }

    /// <summary>
    /// Ensures the player has enough stamina to sprint, and if so, lowers the player's stamina
    /// </summary>
    public void ReduceStaminaFromSprint() // called every frame player is moving with sprint held down
    {
        // Debug.Log("Player's stamina: " + player.GetCurrentStamina() + " / " + player.GetMaxStamina());

        if (player.GetCurrentStamina() > 0 && !stamDepleted)
        {
            player.SetCurrentStamina(player.GetCurrentStamina() - (sprintBase * sprintModifier) * Time.deltaTime);

            float temp = (player.GetCurrentStamina() / player.GetMaxStamina());

            prefabManager.GetStaminaSlider().value = temp;

            if (!recoveringStamina)
                recoveringStamina = true;
        }
        
        if (player.GetCurrentStamina() <= 0) // Player is out of stamina from running
        {
            prefabManager.GetStaminaBarImage().color = prefabManager.GetOutOfStaminaBarColor();
            stamDepleted = true;
        }
    }

    /// <summary>
    /// Ensures the player's stamina is in a recoverable state, and if so, recovers the stamina
    /// </summary>
    public void RecoverStamina()
    {
        if (player.GetCurrentStamina() < player.GetMaxStamina() && recoveringStamina)
        {
            if (!stamDepleted)
            {
                if (standingStill)
                {
                    //player.SetCurrentStamina(player.GetCurrentStamina() + (sprintRecoverBase * sprintRecoverModifier) * Time.deltaTime);
                    player.SetCurrentStamina(player.GetCurrentStamina() + GetSprintRecoveryRate() * Time.deltaTime);
                }
                else
                {
                    //player.SetCurrentStamina(player.GetCurrentStamina() + ((sprintRecoverBase * sprintRecoverModifier) * Time.deltaTime) * standingRecoveryModifier);
                    player.SetCurrentStamina(player.GetCurrentStamina() + (GetSprintRecoveryRate() * Time.deltaTime) * standingRecoveryModifier);
                }
            } else
            {
                player.SetCurrentStamina(player.GetCurrentStamina() + GetSprintRecoveryRate() * Time.deltaTime);
            }


            float temp = player.GetCurrentStamina() / player.GetMaxStamina();

            prefabManager.GetStaminaSlider().value = temp;
        }

        if (player.GetCurrentStamina() >= player.GetMaxStamina() && recoveringStamina)
        {
            recoveringStamina = false;
            player.SetCurrentStamina(player.GetMaxStamina());

            if (stamDepleted)
            {
                prefabManager.GetStaminaBarImage().color = baseStamBarColor;
                stamDepleted = false;
            }            
        }
    }

    /// <summary>
    /// Calculates the player's sprint recovery rate based on their agility rating
    /// </summary>
    /// <returns>Value calculated by sprintRecoverBase * sprintRecoverModifier, and adding it to the player's agility * the agilityToStaminaRecover modifier</returns>
    public float GetSprintRecoveryRate()
    {
        return ((sprintRecoverBase * sprintRecoverModifier) + (player.GetAgility() * agilityToStaminaRecoveryMod));
    }
}
