using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New TileData", menuName = "TileData")]
public class TileData : Tile
{
    [SerializeField] private Placeable placeable;

    public Placeable GetPlaceable() {
        return placeable;
    }
}