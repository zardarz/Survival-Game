using UnityEngine;

[System.Serializable]
public class TileType
{
    public string elevationTileTypeName;

    [Range(0,1)]
    public float height;
}