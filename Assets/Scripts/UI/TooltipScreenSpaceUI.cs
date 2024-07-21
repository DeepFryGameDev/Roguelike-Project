using UnityEngine;
using TMPro;

// Purpose: Maintains all vars and functions for all tooltips (is redundant and will be moved to BaseTooltipScreenSpaceUI)
// Directions: 
// Other notes: Tutorial followed: https://www.youtube.com/watch?v=YUIohCXt_pc

public class TooltipScreenSpaceUI : BaseTooltipScreenSpaceUI
{
    public static TooltipScreenSpaceUI Instance;

    protected TextMeshProUGUI textMeshPro;
    

    protected void Awake()
    {
        Setup();

        textMeshPro = transform.Find("Text").GetComponent<TextMeshProUGUI>();

        HideTooltip();
    }

    // not being used at the moment
    public static void ShowTooltip_Static(string tooltipText)
    {
        Instance.DisplayText(tooltipText);
    }

    // not being used at the moment
    public static void HideTooltip_Static()
    {
        Instance.HideTooltip();
    }

    // not being used at the moment
    void SetText(string tooltipText)
    {
        textMeshPro.SetText(tooltipText);
        textMeshPro.ForceMeshUpdate();

        // keeps size updated - will likely not need this
        Vector2 textSize = textMeshPro.GetRenderedValues(false);

        Vector2 paddingSize = new Vector2(8, 8);

        backgroundRectTransform.sizeDelta = textSize + paddingSize;
    }

    // not being used at the moment
    void DisplayText(string tooltipText)
    {
        SetText(tooltipText);
        ShowTooltip();        
    }
}
