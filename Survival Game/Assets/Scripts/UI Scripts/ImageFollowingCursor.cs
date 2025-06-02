using UnityEngine;

public class ImageFollowingCursor : MonoBehaviour
{
    [SerializeField] private Vector2 offset;
    void Update()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 viewportPosition = (Vector2) Camera.main.ScreenToViewportPoint(Input.mousePosition) + offset;
        rectTransform.anchorMin = viewportPosition;
        rectTransform.anchorMax = viewportPosition;
        rectTransform.anchoredPosition = Vector2.zero;
    }
}
