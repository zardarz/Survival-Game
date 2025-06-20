using System.Collections.Generic;
using UnityEngine;

public class PlaceableItems : MonoBehaviour
{
    [SerializeField] private PlaceableTileEntry[] placeableTileEntries;

    public static Dictionary<string, Placeable> placeables = new Dictionary<string, Placeable>();

    void Awake()
    {
        for(int i = 0; i < placeableTileEntries.Length; i++) {
            placeables.Add(placeableTileEntries[i].placeableName, Instantiate(placeableTileEntries[i].placeable));

            if(placeables[placeableTileEntries[i].placeableName].GetGeneratedSprite()) {
                placeables[placeableTileEntries[i].placeableName].GenerateSprite();
                placeables[placeableTileEntries[i].placeableName].SpriteWasGenerated();
            }
        }
    }
}