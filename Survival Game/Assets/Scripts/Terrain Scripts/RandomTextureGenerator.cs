using UnityEngine;

public class RandomTextureGenerator : MonoBehaviour
{

    private static int amountOfUniqueRandomTextures = 10;

    public static Vector2Int textureSize = new Vector2Int(16,16);
    public static Texture2D[] randomTextures;

    void Start() {
        randomTextures = new Texture2D[amountOfUniqueRandomTextures];
        GenerateRandomTextures();
    }

    private void GenerateRandomTextures() {
        for(int i = 0; i < amountOfUniqueRandomTextures; i++) {
            Debug.Log("Random texture generated");
            randomTextures[i] = GenerateRandomTexture(textureSize.x, textureSize.y);
        }
    }

    private Texture2D GenerateRandomTexture(int textureSizeX, int textureSizeY) {
        Texture2D randomTexture = new Texture2D(textureSizeX, textureSizeY);

        for(int x = 0; x < textureSizeX; x++) {
            for(int y = 0; y < textureSizeY; y++) {
                float brightness = Random.Range(0.9f, 1f); // get how bright the pixel should be 

                Color pixelColor = new Color(brightness, brightness, brightness); // make a color based on the brightness

                randomTexture.SetPixel(x,y, pixelColor); // set the pixel
            }
        }

        randomTexture.filterMode = FilterMode.Point; // make the texture not look like poop
        randomTexture.wrapMode = TextureWrapMode.Clamp;

        randomTexture.Apply(); // apply the texture

        return randomTexture;
    }

    public static Texture2D GetRandomTexture(Color color) {
        Texture2D randomTexture = randomTextures[Random.Range(0, amountOfUniqueRandomTextures)]; // get a random gray scale texture

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

        return randomTexture;
    }
}