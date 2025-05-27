using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainGenerator : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] private Tilemap tilemap;

    [Header("Tile Colors / Tiles")]

    [SerializeField] private Color grassColor;

    private Texture2D[] uniqueGrassTextures;

    void Start() {
        uniqueGrassTextures = RandomTextureGenerator.GetRandomTextures(grassColor);

        for(int x = -50; x < 50; x++) {
            for(int y = -50; y < 50; y++) {
                PlaceGrass(x, y);
            }
        }
    }

    private void PlaceGrass(int x, int y) {
        Tile grassTile = ScriptableObject.CreateInstance<Tile>(); // make a new tile

        Sprite grassSprite = Sprite.Create( // make a new sprite
            uniqueGrassTextures[Random.Range(0,RandomTextureGenerator.amountOfUniqueRandomTextures)],
            new Rect(0, 0, RandomTextureGenerator.textureSize.x, RandomTextureGenerator.textureSize.y),
            new Vector2(0.5f, 0.5f),
            16f // pixels per unit
        );

        grassTile.sprite = grassSprite; // apply the sprite to the tile

        tilemap.SetTile(new Vector3Int(x,y,0), grassTile); // place the tile
    }
}