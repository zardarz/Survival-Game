using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    [SerializeField] private string itemName;

    [SerializeField] private Sprite sprite;

    [SerializeField] private bool stackable;

    [SerializeField] private bool showWhenHolding;

    private int quantity = 1;

    public int inventorySlot;

    public string GetName() {
        return itemName;
    }

    public Sprite GetSprite() {
        return sprite;
    }

    public bool GetStackable() {
        return stackable;
    }

    public int GetQuantity() {
        return quantity;
    }

    public bool GetShowWhenHolding() {
        return showWhenHolding;
    }
}