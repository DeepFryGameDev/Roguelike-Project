using UnityEngine;

// Purpose: Grants the ability to access UI elements - Will likely be updated
// Directions: Call 'GameAssets.i.[UI Object listed below] to obtain the object set in inspector in the 'Resources/GameAssets' object
// Other notes: 

public class GameAssets : MonoBehaviour
{
    public Transform damagePopup;
    public GameObject cooldownRadial;
    public GameObject inventoryItemPrefab;

    public Sprite blankMainHandIcon;
    public Sprite blankOffHandIcon;
    public Sprite blankHelmIcon;
    public Sprite blankChestIcon;
    public Sprite blankHandsIcon;
    public Sprite blankLegsIcon;
    public Sprite blankFeetIcon;
    public Sprite blankAmuletIcon;
    public Sprite blankRingIcon;

    private static GameAssets _i;

    public static GameAssets i
    {
        get
        {
            if (_i == null) _i = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            return _i;
        }
    }
}
