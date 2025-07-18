using UnityEngine;

public class InventoryBuffer : MonoBehaviour
{
    public static Item[] inventoryItems;
    public static Item[] hotbarItems;

    private bool hasInitialized = false;

    void Awake()
    {
        if(hasInitialized == false) {
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public Item[] GetInventoryItems() {
        return inventoryItems;
    }

    public Item[] GetHotbarItems() {
        return hotbarItems;
    }

    public void SetInventoryItems(Item[] items) {
        inventoryItems = items;
    }

    public void SetHotbarItems(Item[] items) {
        hotbarItems = items;
    }
}