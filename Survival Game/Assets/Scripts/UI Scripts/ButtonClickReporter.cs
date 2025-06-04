using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonClickReporter : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private InventoryManager inventoryManager;

    public void OnPointerDown(PointerEventData eventData)
    {
        bool isRightClick = eventData.button == PointerEventData.InputButton.Right;
        inventoryManager.OnButtonClick(gameObject, isRightClick);
    }
}
