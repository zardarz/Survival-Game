using UnityEngine;
using System.Collections.Generic;

public class RandomTextureGenerator : MonoBehaviour
{

    public static int amountOfUniqueRandomTextures = 10;
    public static Vector2Int textureSize = new Vector2Int(16,16);
    public static Texture2D[] randomTextures;

    [SerializeField] public GeneratedTileEntry[] generatedTileEntries;

    private static Dictionary<string, Texture2D[]> textures = new Dictionary<string, Texture2D[]>();

    private static Dictionary<string, Sprite[]> sprites = new Dictionary<string, Sprite[]>();

    void Awake() {
        randomTextures = new Texture2D[amountOfUniqueRandomTextures];
        GenerateRandomTextures();

        // generate all of the textures for the world
        SetTextures();

        // generate all of the sprites for the world
        SetSprites();
    }

    private void GenerateRandomTextures() {
        for(int i = 0; i < amountOfUniqueRandomTextures; i++) {
            randomTextures[i] = GenerateRandomTexture(textureSize.x, textureSize.y);
        }
    }

    private Texture2D GenerateRandomTexture(int textureSizeX, int textureSizeY) {
        Texture2D randomTexture = new Texture2D(textureSizeX, textureSizeY);

        for(int x = 0; x < textureSizeX; x++) {
            for(int y = 0; y < textureSizeY; y++) {
                float brightness = Random.Range(0.85f, 1f); // get how bright the pixel should be 

                Color pixelColor = new Color(brightness, brightness, brightness); // make a color based on the brightness

                randomTexture.SetPixel(x,y, pixelColor); // set the pixel
            }
        }

        randomTexture.filterMode = FilterMode.Point; // make the texture not look like poop
        randomTexture.wrapMode = TextureWrapMode.Clamp;

        randomTexture.Apply(); // apply the texture

        return randomTexture;
    }

    private Texture2D GetRandomTexture(Color color, int index) {
        Texture2D randomTexture = randomTextures[index]; // get a random gray scale texture

        Texture2D newTexture = new Texture2D(textureSize.x, textureSize.y);

        for(int x = 0; x < textureSize.x; x++) {
            for(int y = 0; y < textureSize.y; y++) {
                Color randomTextureColor = randomTexture.GetPixel(x,y); // get the color from the gray scale texture

                Color newColor = new Color(color.r * randomTextureColor.r, color.g * randomTextureColor.g, color.b * randomTextureColor.b); // gray scale color * desired color

                newTexture.SetPixel(x, y, newColor); // set that pixel
            }
        }

        newTexture.filterMode = FilterMode.Point; // make the texture not look like poop
        newTexture.wrapMode = TextureWrapMode.Clamp;

        newTexture.Apply(); // apply the texture

        return newTexture;
    }

    private Texture2D[] GetRandomTextures(Color color) {
        Texture2D[] newTextures = new Texture2D[amountOfUniqueRandomTextures];

        for(int i = 0; i < amountOfUniqueRandomTextures; i++) {
            newTextures[i] = GetRandomTexture(color, i);
        }

        return newTextures;
    }


    private void SetTextures() {
        for(int i = 0; i < generatedTileEntries.Length; i++) {
            textures.Add(generatedTileEntries[i].tileName, GetRandomTextures(generatedTileEntries[i].tileColor));
        }
    }

    private void SetSprites() {
        for(int i = 0; i < generatedTileEntries.Length; i++) {
            sprites.Add(generatedTileEntries[i].tileName, GenerateSprites(generatedTileEntries[i].tileName));
        }
    }

    private static Sprite[] GenerateSprites(string name) {
        Sprite[] generatedSprites = new Sprite[amountOfUniqueRandomTextures];
        Texture2D[] textureToBeConverted = textures[name];

        for(int i = 0; i < amountOfUniqueRandomTextures; i++) {
            generatedSprites[i] = TextureToSprite(textureToBeConverted[i]);
        }

        return generatedSprites;
    }

    public static Sprite[] GetSprites(string name) {
        if(sprites.TryGetValue(name, out Sprite[] result)) {return result;}

        return null;
    }

    public static Sprite GetRandomSprite(string name) {
        if(sprites.TryGetValue(name, out Sprite[] result)) {return result[Random.Range(0,amountOfUniqueRandomTextures)];}

        return null;
    }

    private static Sprite TextureToSprite(Texture2D texture) {
        return Sprite.Create(
            texture,
            new Rect(0, 0, textureSize.x, textureSize.y),
            new Vector2(0.5f, 0.5f),
            16f // pixels per unit
        );
    }

}