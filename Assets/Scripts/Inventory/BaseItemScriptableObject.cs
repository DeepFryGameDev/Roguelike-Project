using UnityEngine;

// Purpose: Houses all vars and functionality for any item or equipment
// Directions: Any vars or functionality to be shared between all items and equipment should be set here
// Other notes: 

public class BaseItemScriptableObject : ScriptableObject
{
    [Tooltip("The name of the item or equipment")]
    [SerializeField] new string name;

    [Tooltip("Sprite to be used as the item icon")]
    public Sprite icon;

    [Tooltip("Value in gold that the item is worth")]
    public int goldWorth;

    [Tooltip("Description of the item to be displayed in the menu")]
    public string description;
}
