using UnityEngine;

// Purpose: Used to display any floating text to be used when a unit takes damage or is healed
// Directions: Add script to FloatingDamageText prefab, and call DamageFloatingText.Create when displaying any floating text popup
// Other notes: 

public class DamageFloatingText : MonoBehaviour
{
    const float DISAPPEAR_TIMER_MAX = 1f; // How long for the damage text to be displayed

    const float timerDisappearModifier = .5f; // Multiplied by the maximum disappear time to calculate how long text should be displayed

    const int standardDamageTextSize = 8; // Text size used for standard damage text

    static Color standardDamageTextColor = Color.yellow; // Text color used for standard damage text

    const int criticalDamageTextSize = 10; // Text size used for critical damage text

    static Color criticalDamageTextColor = Color.red; // Text color used for critical damage text

    const float moveVectorModifier = 15f; // Multiplied by the random moveVector value calculated from GetRandomMoveVector

    const float moveVectorThreshold = 2f; // The minimum to maximum value the moveVector should be randomly set to

    const float textAnimationSpeed = 8f;

    //---

    [Tooltip("Set to the TextMeshPro text component on the prefab")]
    [SerializeField] TMPro.TextMeshPro damageText;

    EnumHandler.DamageTextTypes textType; // Used to determine if the text should be displayed as healing or damage
    public void SetTextType(EnumHandler.DamageTextTypes textType) { this.textType = textType; }

    float disappearTimer; // Time until the damage text object is destroyed    

    Vector3 moveVector; // Randomly set by GetRandomMoveVector() to display the position for the text to be displayed

    static int sortingOrder; // Used to ensure new instances of floating text is displayed on top of previous instances

    Color textColor; // Automatically set to the default text color used in the inspector

    GameObject playerObj; // Automatically set to the player's GameObject to be used for keeping the text facing towards the player's view when it is instantiated 

    /// <summary>
    /// Sets the text of the damageText var to the given value
    /// </summary>
    /// <param name="text">Value to be displayed on the floating text</param>
    public void SetDamageText(string text) { damageText.text = text; }

    /// <summary>
    /// Sets the font size for the floating text to the given value
    /// </summary>
    /// <param name="fontSize">Size to set for the text</param>
    public void SetFontSize(int fontSize) { damageText.fontSize = fontSize; }

    /// <summary>
    /// Sets the font color for the floating text to the given value
    /// </summary>
    /// <param name="color">Color to be set for the text</param>
    public void SetFontColor(Color color) { damageText.color = color; }

    //---

    void Start()
    {
        //PositionAboveUnit();
        //StartSelfDestruct();

        textColor = damageText.color;

        disappearTimer = DISAPPEAR_TIMER_MAX;

        moveVector = new Vector3(GetRandomMoveVector(), 1) * moveVectorModifier;

        sortingOrder++;
        damageText.sortingOrder = sortingOrder;

        playerObj = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        AnimateText();
    }

    /// <summary>
    /// Instantiates a new instance of floating combat text for when a unit's health is going to be changed
    /// </summary>
    /// <param name="position">The originating position for the damage text to be displayed</param>
    /// <param name="damageAmount">The value of the text that should be displayed</param>
    /// <param name="textType">The type of text type that should be created</param>
    /// <returns></returns>
    public static DamageFloatingText Create(Vector3 position, int damageAmount, EnumHandler.DamageTextTypes textType)
    {
        Transform damagePopupTransform = Instantiate(GameAssets.i.damagePopup, new Vector3(position.x, position.y + 2, position.z), Quaternion.identity);
        DamageFloatingText damagePopup = damagePopupTransform.GetComponent<DamageFloatingText>();
        damagePopup.SetDamageText(damageAmount.ToString());
        damagePopup.SetTextType(textType);

        switch (textType)
        {
            case EnumHandler.DamageTextTypes.NORMAL:
                damagePopup.SetFontSize(standardDamageTextSize);
                damagePopup.SetFontColor(standardDamageTextColor);
                break;
            case EnumHandler.DamageTextTypes.CRIT:
                damagePopup.SetFontSize(criticalDamageTextSize);
                damagePopup.SetFontColor(criticalDamageTextColor);
                break;
            case EnumHandler.DamageTextTypes.HEAL:

                break;
        }
        

        damagePopupTransform.transform.LookAt(position - (GameObject.FindGameObjectWithTag("Player").transform.position - position)); // looks "away" from player (to face the text correctly towards the camera)

        return damagePopup;
    }

    /// <summary>
    /// Used to determine the starting move vector for the text to be displayed/animated
    /// </summary>
    /// <returns>Random value between the negative and positive values for moveVectorThreshold</returns>
    float GetRandomMoveVector()
    {
        return Random.Range(-moveVectorThreshold, moveVectorThreshold);
    }



    /// <summary>
    /// Moves the text position after being instantiated as a sort of 'animation'
    /// Also keeps the text facing "away" from the player, which in reality will make the text readable to the player
    /// </summary>
    void AnimateText()
    {
        transform.LookAt(transform.position - (playerObj.transform.position - transform.position)); // Keeps text facing towards player's viewpoint

        StandardTextAnimation(); // should be changed later when the other damage types are further implemented

        if (textType == EnumHandler.DamageTextTypes.CRIT)
        {
            // make text "shake" or something
        }
    }

    /// <summary>
    /// Moves the position of the text to slowly float upwards while it disappears
    /// </summary>
    void StandardTextAnimation()
    {
        // slowly move up and disappear

        // start with making damage appear and disappear after 2 seconds
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * textAnimationSpeed * Time.deltaTime;

        if (disappearTimer > DISAPPEAR_TIMER_MAX * timerDisappearModifier)
        {
            // first half of popup
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        } else
        {
            // second half of popup
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }

        disappearTimer -= Time.deltaTime;

        if (disappearTimer < 0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            damageText.color = textColor;

            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Currently not being used
    /// </summary>
    private void PositionAboveUnit()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
    }
}
