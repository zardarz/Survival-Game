using System.Collections.Generic;
using UnityEngine;

public class PlaceableItems : MonoBehaviour
{
    [SerializeField] private PlaceableTileEntry[] placeableTileEntries;
    [SerializeField] private InventoryManager inventoryManager;

    public static Dictionary<string, Placeable> placeables = new Dictionary<string, Placeable>();

    void Start()
    {
        for(int i = 0; i < placeableTileEntries.Length; i++) {
            Placeable placeable = Instantiate(placeableTileEntries[i].placeable);
            placeables.Add(placeableTileEntries[i].placeableName, placeable);

            if(placeable.GetGeneratedSprite()) {
                placeable.GenerateSprite();
                placeable.SpriteWasGenerated();
            }

            if(placeable is Interactable) {
                Interactable interactable = placeable as Interactable;

                interactable.onRightClick.RemoveAllListeners();
                interactable.onRightClick.AddListener(inventoryManager.ToggleCraftingInterface);
            }
        }
        print("Start");
    }
}