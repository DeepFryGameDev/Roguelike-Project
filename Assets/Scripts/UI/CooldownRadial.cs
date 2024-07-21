using UnityEngine;
using UnityEngine.UI;

// Purpose: Used to display the a cooldown UI element for attacks so the player has some feedback as to when the attack is ready
// Directions: Add to the 'CooldownRadial' prefab and call Setup() when instantiated
// Other notes: 

public class CooldownRadial : MonoBehaviour
{
    // Set as the time for the attack cooldown until it is ready to be used again
    float cooldownTime;
    void SetCooldownTime(float cooldownTime) { this.cooldownTime = cooldownTime; }

    // The current time to be updated when the timer is on cooldown
    float timer;

    // The image of the icon to be displayed on the cooldown radial object in UI
    Image iconImage;
    void SetIconImage(Image iconImage) { this.iconImage = iconImage; }

    // Automatically set as the image component in the child object - used to show the progress of the timer when attack is on cooldown
    Image fillImage;

    // Set to true when the attack is on cooldown and the timer has started
    bool timerStarted;

    private void Update()
    {
        if (timerStarted)
        {
            timer -= Time.deltaTime;
            fillImage.fillAmount = (timer / cooldownTime);

            if (timer <= 0)
            {
                fillImage.fillAmount = 0;
                timerStarted = false;
            }
        }
    }

    /// <summary>
    /// Call when instantiating the cooldown radial. Sets up the proper vars for the script to function
    /// </summary>
    /// <param name="attack">Attack set to the radial</param>
    public void Setup(AttackScriptableObject attack)
    {
        SetIconImage(GetComponent<Image>());

        SetCooldownTime(attack.cooldown);

        fillImage = transform.GetChild(0).GetComponent<Image>();

        fillImage.fillAmount = 0;

        iconImage.sprite = attack.icon;

        attack.SetCooldownRadial(this); // connects it to the attack
    }

    /// <summary>
    /// Starts the timer for the cooldown radial fillImage to show progress when attack is ready to be used again
    /// </summary>
    public void StartTimer()
    {
        timerStarted = true;
        timer = cooldownTime;
    }
}
