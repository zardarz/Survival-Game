using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New TileData", menuName = "TileData")]
public class TileData : Tile
{
    public string tileName;
    public float tileStrength;
}