using UnityEngine;

// Purpose: The parent class used for displaying tooltips within the UI. Used to display equipment details at the moment.
// Directions: Any shared vars or functions to be used for all tooltips should be added here.
// Other notes: Tutorial followed: https://www.youtube.com/watch?v=YUIohCXt_pc

public class BaseTooltipScreenSpaceUI : MonoBehaviour
{
    [SerializeField] private RectTransform canvasRectTransform;

    protected RectTransform backgroundRectTransform;
    protected RectTransform rectTransform;

    CanvasGroup canvasGroup;

    protected void Setup()
    {
        backgroundRectTransform = transform.Find("Background").GetComponent<RectTransform>();

        rectTransform = transform.GetComponent<RectTransform>();        

        canvasGroup = transform.GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// Called on the child classes in the Update() method
    /// Keeps the tooltip anchored to the bottom left corner of the mouse position on the screen.
    /// If the tooltip were to move off screen, this method keeps the tooltip contained on the screen so it does not run off
    /// </summary>
    protected void AnchorTooltip()
    {
        Vector2 anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;


        if (anchoredPosition.x + backgroundRectTransform.rect.width > canvasRectTransform.rect.width)
        {
            anchoredPosition.x = canvasRectTransform.rect.width - backgroundRectTransform.rect.width;
        }

        if (anchoredPosition.y + backgroundRectTransform.rect.height > canvasRectTransform.rect.height)
        {
            anchoredPosition.y = canvasRectTransform.rect.height - backgroundRectTransform.rect.height;
        }

        rectTransform.anchoredPosition = anchoredPosition;
    }

    /// <summary>
    /// Displays the tooltip object by updating CanvasGroup variables attached to it
    /// </summary>
    protected void ShowTooltip()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    /// <summary>
    /// Hides the tooltip object by updating CanvasGroup variables attached to it
    /// </summary>
    protected void HideTooltip()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

}
