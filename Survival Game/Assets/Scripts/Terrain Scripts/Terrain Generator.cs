using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainGenerator : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] private Tilemap tilemap;

    private Texture2D[] grassTextures;

    [Header("Settings")]
    [SerializeField] private Vector2Int textureSize;

    [Range(0,1)]
    [SerializeField] private float textureBrightnessMin;

    [Range(0,1)]
    [SerializeField] private float textureBrightnessMax;

    [SerializeField] private int amountOfUniqueGrassTextures;

    void Start() {
        grassTextures = new Texture2D[amountOfUniqueGrassTextures];
        MakeUniqueGrassTextures();

        for(int x = -50; x < 50; x++) {
            for(int y = -50; y < 50; y++) {
                PlaceGrass(x, y);
            }
        }
    }

    private void PlaceGrass(int x, int y) {
        Tile grassTile = ScriptableObject.CreateInstance<Tile>(); // make a new tile

        Sprite grassSprite = Sprite.Create( // make a new sprite
            grassTextures[Random.Range(0, amountOfUniqueGrassTextures)],
            new Rect(0, 0, textureSize.x, textureSize.y),
            new Vector2(0.5f, 0.5f),
            16f // pixels per unit
        );

        grassTile.sprite = grassSprite; // apply the sprite to the tile

        tilemap.SetTile(new Vector3Int(x,y,0), grassTile); // place the tile
    }

    private Texture2D GenerateRandomGrassTexture() {
        Texture2D grassTexture = new Texture2D(textureSize.x, textureSize.y);

        for(int x = 0; x < textureSize.x; x++) {
            for(int y = 0; y < textureSize.y; y++) {
                float brightness = Random.Range(textureBrightnessMin, textureBrightnessMax); // get how bright the pixel should be 

                Color pixelColor = new Color(0.05f * brightness, 0.35f * brightness, 0.13f * brightness); // make a new color preselected green * brightness

                grassTexture.SetPixel(x,y, pixelColor); // set the pixel
            }
        }

        grassTexture.filterMode = FilterMode.Point; // make the texture not look like poop
        grassTexture.wrapMode = TextureWrapMode.Clamp;

        grassTexture.Apply(); // apply the texture

        return grassTexture;
    }

    private void MakeUniqueGrassTextures() {
        for(int i = 0; i < amountOfUniqueGrassTextures; i++) { // makes a specific amount of grass textures that are just repeated
            Texture2D grassTexture = GenerateRandomGrassTexture();
            grassTextures[i] = grassTexture;
        }
    }
}