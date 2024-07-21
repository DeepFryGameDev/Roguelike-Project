using UnityEngine;
using UnityEngine.UI;

// Purpose: To maintain a crosshair on the center of the screen during battle stages. This will help the player to aim their attacks
// Directions: Attach to object "[UI]/[Crosshair]".  Crosshair image should be added to the image component of this object, and alpha set to 0
// Other notes: Will be updating this script to something much more conducive to show the hitbox of each attack

public class CrosshairHandler : MonoBehaviour
{
    Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    /// <summary>
    /// Displays or hides the crosshair by simply changing the alpha
    /// </summary>
    /// <param name="toggle">Set to true if crosshair should be shown, or false to hide it</param>
    public void ToggleCrosshair(bool toggle)
    {
        if (toggle) image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        if (!toggle) image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
    }
}
