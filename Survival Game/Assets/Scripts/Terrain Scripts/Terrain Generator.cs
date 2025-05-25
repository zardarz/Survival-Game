using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainGenerator : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] private Tilemap tilemap;

    [Header("Settings")]
    [SerializeField] private Vector2Int textureSize;

    void Start() {
        for(int x = 0; x < 5; x++) {
            for(int y = 0; y < 5; y++) {
                PlaceGrass(x, y);
            }
        }
    }

    private void PlaceGrass(int x, int y) {
        Tile grassTile = ScriptableObject.CreateInstance<Tile>();

        Sprite grassSprite = Sprite.Create(
            GenerateRandomGrassTexture(),
            new Rect(0, 0, textureSize.x, textureSize.y),
            new Vector2(0.5f, 0.5f),
            16f // pixels per unit
        );

        grassTile.sprite = grassSprite;

        tilemap.SetTile(new Vector3Int(x,y,0), grassTile);
    }

    private Texture2D GenerateRandomGrassTexture() {
        Texture2D grassTexture = new Texture2D(textureSize.x, textureSize.y);

        for(int x = 0; x < textureSize.x; x++) {
            for(int y = 0; y < textureSize.y; y++) {
                Color pixelColor = new Color(0,1,0);

                grassTexture.SetPixel(x,y, pixelColor);
            }
        }

        grassTexture.Apply();

        return grassTexture;
    }
}