using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Purpose: Provides the basis for all common events that can be called via script in the world
// Directions: Set this on the [System] GameObject along with InteractionHandler
// Other notes: This script was taken from an old project - most of it will be rewritten

public class BaseScriptedEvent : MonoBehaviour
{
    //public string method; //name of the method to be run

    [System.NonSerialized] public float baseTextSpeed = 0.030225f; //textSpeed in DialogueEvent (update this if text speed is updated there)

    [System.NonSerialized] public float baseMoveSpeed = 0.5f; //base move speed for movement methods

    [System.NonSerialized] public float collisionDistance = 1;

    [System.NonSerialized] public Transform thisTransform; //transform of the game object this script is attached to
    [System.NonSerialized] public GameObject thisGameObject; //game object that this script is attached to
    [System.NonSerialized] public Transform playerTransform; //transform of the player game object
    [System.NonSerialized] public GameObject playerGameObject; //game object of the player
    [System.NonSerialized] public GameManager gameManager; //the game manager

    [System.NonSerialized] public bool otherEventRunning = false;
    [System.NonSerialized] public bool inMenu = false;

    public AttackManager am;

    public int tempSceneIndex, currentSceneIndex;

    //GameMenu menu;

    Transform cursor;
    bool dpadPressed;

    enum cursorModes
    {
        DIALOGUECHOICE,
        IDLE
    }

    [HideInInspector] public bool confirmPressed;
    bool checkForConfirmPressed;

    [HideInInspector] public bool messageFinished;

    AudioSource audioSource;

    //Enums
    [HideInInspector]
    public enum MenuButtons
    {
        Item,
        Magic,
        Equip,
        Status,
        Talents,
        Party,
        Grid,
        Quests,
        Bestiary
    }
    [HideInInspector] public MenuButtons menuButton;

    private void Start()
    {
        thisGameObject = this.gameObject; //sets thisGameObject to game object this script is attached to
        thisTransform = this.gameObject.transform; //sets thisTransform to transform of game object this script is attached to
        playerGameObject = GameObject.Find("Player"); //sets playerGameObject to gameobject of player
        playerTransform = playerGameObject.transform; //sets playerTransform to transform of gameobject of player
        gameManager = GameObject.Find("[GameManager]").GetComponent<GameManager>(); //sets gameManager to the game manager object in scene

        am = FindObjectOfType<AttackManager>();

        messageFinished = true;
    }

    private void Update()
    {

    }

    //DIFFERENT FUNCTIONS THAT CAN BE RUN BY ANY EVENT SCRIPT

    #region ---MOVEMENT---

    /// <summary>
    /// Changes default move speed for player
    /// </summary>
    /// <param name="newMoveSpeed">Move speed to be set (higher = faster)</param>
    public void ChangeDefaultMoveSpeed(float newMoveSpeed)
    {
        
    }

    /// <summary>
    /// Disables player's movement
    /// </summary>
    public void DisablePlayerMovement()
    {

    }

    /// <summary>
    /// Enables player's movement
    /// </summary>
    public void EnablePlayerMovement()
    {

    }

    #endregion

    #region ---BATTLE MANAGEMENT---

    #endregion

    #region ---SCENE MANAGEMENT---

    public void TransitionToScene(int sceneIndex)
    {
        Debug.Log("Loading scene: " + sceneIndex + ".");
        SceneManager.LoadScene(sceneIndex);

        currentSceneIndex = sceneIndex;
    }

    /// <summary>
    /// Forces menu to be opened
    /// </summary>
    public void OpenMenu()
    {
        
    }

    //void OpenSave

    //void GameOver

    //void ReturnToTitle

    #endregion

    #region ---GAME MANAGEMENT---

    /// <summary>
    /// Changes switch value (not yet implemented)
    /// </summary>
    /// <param name="whichObject">GameObject with the switch</param>
    /// <param name="whichEvent">Switch's event index</param>
    /// <param name="whichSwitch">The switch to be changed</param>
    /// <param name="whichBool">To change the switch to true or false</param>
    public void ChangeSwitch(GameObject whichObject, int whichEvent, int whichSwitch, bool whichBool)
    {
        /*
        BaseDialogueEvent e = whichObject.GetComponent<DialogueEvents>().eventOrDialogue[whichEvent];

        if (whichSwitch == 1)
        {
            e.switch1 = whichBool;
        }
        else if (whichSwitch == 2)
        {
            e.switch2 = whichBool;
        }*/
    }

    /// <summary>
    /// Returns if switch is true or false on given object (not yet fully implemented)
    /// </summary>
    /// <param name="whichObject">GameObject with the switch</param>
    /// <param name="whichEvent">Switch's event index</param>
    /// <param name="whichSwitch">The switch to be returned</param>
    public bool GetSwitchBool(GameObject whichObject, int whichEvent, int whichSwitch)
    {/*
        BaseDialogueEvent e = whichObject.GetComponent<DialogueEvents>().eventOrDialogue[whichEvent];

        if (whichSwitch == 1)
        {
            return e.switch1;
        }
        else if (whichSwitch == 2)
        {
            return e.switch2;
        }
        else
        {
            Debug.Log("GetSwitchBool - invalid switch: " + whichSwitch);
            return false;
        }*/
        return false;
    }

    /// <summary>
    /// Changes value of global event bools
    /// </summary>
    /// <param name="index">Index of the global bool</param>
    /// <param name="boolean">Change bool to true or false</param>
    public void ChangeGlobalBool(int index, bool boolean)
    {
        //GlobalBoolsDB.instance.globalBools[index] = boolean;
    }

    #endregion

    #region ---MUSIC/SOUNDS---

    /// <summary>
    /// Plays given sound effect once
    /// </summary>
    /// <param name="SE">Sound effect to play</param>
    void PlaySE(AudioClip SE)
    {
        audioSource.PlayOneShot(SE);
    }

    //void PlayBGM

    //ienumerator FadeOutBGM

    //ienumerator FadeInBGM

    //void PlayBGS

    //ienumerator FadeOutBGS

    //void StopBGM

    //void StopSE

    #endregion

    #region ---TIMING---

    /// <summary>
    /// Halt processing for given seconds
    /// </summary>
    /// <param name="waitTime">Time to move in seconds</param>
    public IEnumerator WaitForSeconds(float waitTime) //pause for period of time
    {
        yield return new WaitForSeconds(waitTime);
    }

    #endregion

    #region ---DIALOGUE---

    #endregion

    #region ---QUESTS---

    /// <summary>
    /// Adds given quest to active quests list
    /// </summary>
    /// <param name="ID">ID of quest in QuestDB</param>
    public void StartQuest(int ID)
    {

    }

    /// <summary>
    /// Marks given quest as complete
    /// </summary>
    /// <param name="ID">Quest to check</param>
    public void CompleteQuest(int ID)
    {
        //QuestDB.instance.CompleteQuest(quest);
    }

    /// <summary>
    /// Marks bool of given quest as given value if quest type is 'bool'
    /// </summary>
    /// <param name="ID">Quest to check</param>
    /// <param name="index">Index of bool in quest</param>
    /// <param name="value">To mark the bool as true or false</param>
    public void MarkQuestBool(int ID, int index, bool value)
    {
        
    }

    /// <summary>
    /// Returns given quest and index bool value
    /// </summary>
    /// <param name="ID">Quest to check</param>
    /// <param name="index">Index of bool in quest</param>
    public bool QuestBool(int ID, int index)
    {
        return false;
    }

    /// <summary>
    /// Returns Quest by ID   	
    /// </summary>
    /// <param name="type">Use DB, Active, or Complete</param>
    /// <param name="ID">ID of quest</param>
    /*public BaseQuest GetQuestByID(string type, int ID)
    {
        if (type == "DB")
        {
            foreach (BaseQuest quest in QuestDB.instance.quests)
            {
                if (quest.ID == ID)
                {
                    return quest;
                }
            }
        }

        if (type == "Active")
        {
            foreach (BaseQuest quest in GameManager.instance.activeQuests)
            {
                if (quest.ID == ID)
                {
                    return quest;
                }
            }
        }

        if (type == "Complete")
        {
            foreach (BaseQuest quest in GameManager.instance.completedQuests)
            {
                if (quest.ID == ID)
                {
                    return quest;
                }
            }
        }

        return null;
    }*/

    /// <summary>
    /// Returns given quest by ID from QuestDB
    /// </summary>
    /// <param name="ID">ID of quest in QuestDB</param>
    /*public BaseQuest GetQuest(int ID)
    {
        foreach (BaseQuest quest in QuestDB.instance.quests)
        {
            if (quest.ID == ID)
            {
                return quest;
            }
        }

        return null;
    }*/

    #endregion

    #region ---SYSTEM SETTINGS---

    //void ChangeBattleBGM

    /// <summary>
    /// Enables or disables ability to open menu
    /// </summary>
    /// <param name="canOpen">If true, menu can be opened</param>
    public void ChangeMenuAccess(bool canOpen)
    {

    }

    /// <summary>
    /// Enables or disables ability to access menu buttons (Items, Magic, etc)
    /// </summary>
    /// <param name="button">Button to change access</param>
    /// <param name="canAccess">If true, button can be accessed</param>
    public void ChangeMenuButtonAccess(MenuButtons button, bool canAccess)
    {

    }

    #endregion

    #region ---SPRITES---

    //void ChangeGraphic

    //void ChangeOpacity

    //void AddSprite

    //void RemoveSprite

    #endregion

    #region ---ACTORS---

    /// <summary>
    /// Fully restores HP and MP of all active heroes
    /// </summary>
    public void FullHeal()
    {
        
    }

    /// <summary>
    /// Adds or subtracts current HP of given hero
    /// </summary>
    /// <param name="ID">Hero to modify HP</param>
    /// <param name="hp">CurrentHP to add/subtract (subtract with negative value)</param>
    public void ChangeHP(int ID, int hp)
    {
        
    }

    /// <summary>
    /// Sets current HP or given hero to given value
    /// </summary>
    /// <param name="ID">Hero to modify HP</param>
    /// <param name="hp">HP value to set current HP</param>
    public void SetHP(int ID, int hp)
    {

    }

    /// <summary>
    /// Adds or subtracts current MP of given hero
    /// </summary>
    /// <param name="hero">Hero to modify MP</param>
    /// <param name="mp">MP value to set current MP</param>
    public void ChangeMP(int ID, int mp)
    {
       
    }

    /// <summary>
    /// Sets current MP of given hero to given value
    /// </summary>
    /// <param name="ID">Hero to modify MP</param>
    /// <param name="mp">MP value to set current MP</param>
    public void SetMP(int ID, int mp)
    {

    }

    /// <summary>
    /// Adds or subtracts current EXP of given hero (de-leveling not yet supported)
    /// </summary>
    public void ChangeEXP(int ID, int exp)
    {
    }

    /// <summary>
    /// Adds or subtracts given base stat by given value to given hero
    /// </summary>
    /// <param name="ID">Hero to modify parameter</param>
    /// <param name="parameter">Use: "Strength", "Stamina", "Agility", "Dexterity", "Intelligence" or "Spirit"</param>
    /// <param name="paramChange">Value to be added/subtracted</param>
    public void ChangeParameter(int ID, string parameter, int paramChange)
    {
        
    }

    //void AddSkill

    //void RemoveSkill

    /// <summary>
    /// Equip given equipment for given hero
    /// </summary>
    /// <param name="ID">Hero to change equipment</param>
    /// <param name="equipName">Name of the equipment to be equipped</param>
    public void ChangeEquipment(int ID, string equipName)
    {
        
    }

    /// <summary>
    /// Change name of given hero with given string
    /// </summary>
    /// <param name="ID">Hero to modify name</param>
    /// <param name="name">New name of the given hero</param>
    public void ChangeName(int ID, string name)
    {
        
    }

    #endregion

    #region ---PARTY---

    /// <summary>
    /// Adds or subtracts given gold
    /// </summary>
    /// <param name="gold">Number of gold to be added/subtracted</param>
    public void ChangeGold(int gold)
    {
        GameManager.instance.gold += gold;
    }

    /// <summary>
    /// Adds given item to inventory
    /// </summary>
    /// <param name="ID">ID of item from ItemDB to add</param>
    /// <param name="numberToAdd">Number of the given item to add to inventory</param>
    public void AddItem(int ID, int numberToAdd)
    {
    }

    /// <summary>
    /// Adds 1 of given item to inventory
    /// </summary>
    /// <param name="ID">ID of item from ItemDB to add</param>
    public void AddItem(int ID)
    {

    }

    /// <summary>
    /// Removes given item from inventory
    /// </summary>
    /// <param name="ID">ID of item from ItemDB to remove</param>
    /// <param name="numberToRemove">Number of the given item to remove from inventory</param>
    public void RemoveItem(int ID, int numberToRemove)
    {

    }

    /// <summary>
    /// Removes 1 of given item from inventory
    /// </summary>
    /// <param name="ID">ID of item from ItemDB to remove</param>
    public void RemoveItem(int ID)
    {

    }

    /// <summary>
    /// Adds given equipment to inventory
    /// </summary>
    /// <param name="ID">ID of equipment from EquipmentDB to add</param>
    /// <param name="numberToAdd">Number of the given equipment to add to inventory</param>
    public void AddEquipment(int ID, int numberToAdd)
    {

    }

    /// <summary>
    /// Adds 1 of given equipment to inventory
    /// </summary>
    /// <param name="ID">ID of equipment from EquipmentDB to add</param>
    public void AddEquipment(int ID)
    {

    }

    /// <summary>
    /// Removes given equipment from inventory
    /// </summary>
    /// <param name="ID">ID of equipment from EquipmentDB to add</param>
    /// <param name="numberToRemove">Number of the given equipment to add to inventory</param>
    public void RemoveEquipment(int ID, int numberToRemove) //removes equipment from inventory
    {
        
    }

    /// <summary>
    /// Removes 1 of given equipment from inventory
    /// </summary>
    /// <param name="ID">ID of equipment from EquipmentDB to add</param>
    public void RemoveEquipment(int ID) //removes equipment from inventory
    {

    }

    //void ChangePartyMember

    #endregion

    #region ---IMAGES---

    //void ShowPicture

    //void MovePicture

    //void RotatePicture

    //void TintPicture

    //void RemovePicture

    #endregion

    #region ---WEATHER/EFFECTS---

    //void FadeInScreen

    //void FadeOutScreen

    //void TintScreen

    //void FlashScreen

    //void ShakeScreen

    #endregion

    #region ---TOOLS FOR EVENTS---

    #endregion
}

