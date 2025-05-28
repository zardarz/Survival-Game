using UnityEngine;

public class RandomTextureGenerator : MonoBehaviour
{

    public static int amountOfUniqueRandomTextures = 10;
    public static Vector2Int textureSize = new Vector2Int(16,16);
    public static Texture2D[] randomTextures;

    [Header("Texture Colors")]
    [SerializeField] private Color grassColor;
    [SerializeField] private Color stoneColor;

    public static Texture2D[] grassTextures;
    public static Sprite[] grassSprites;

    public static Texture2D[] stoneTextures;
    public static Sprite[] stoneSprites;

    void Awake() {
        randomTextures = new Texture2D[amountOfUniqueRandomTextures];
        GenerateRandomTextures();

        // generate all of the textures for the world
        grassTextures = GetRandomTextures(grassColor);
        stoneTextures = GetRandomTextures(stoneColor);

        // generate all of the sprites for the world
        grassSprites = GetGrassSprites();
        stoneSprites = GetStoneSprites();
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

        return newTexture;
    }

    private static Texture2D GetRandomTexture(Color color, int index) {
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

    public static Texture2D[] GetRandomTextures(Color color) {
        Texture2D[] newTextures = new Texture2D[amountOfUniqueRandomTextures];

        for(int i = 0; i < amountOfUniqueRandomTextures; i++) {
            newTextures[i] = GetRandomTexture(color, i);
        }

        return newTextures;
    }

    public static Texture2D GetRandomGrassTexture() { return grassTextures[Random.Range(0,amountOfUniqueRandomTextures)];} // returns random grass texture
    public static Texture2D GetRandomStoneTexture() { return stoneTextures[Random.Range(0,amountOfUniqueRandomTextures)];} // returns random stone texture


    public static Sprite GetRandomGrassSprite() { return grassSprites[Random.Range(0,amountOfUniqueRandomTextures)];} // returns random grass texture
    public static Sprite GetRandomStoneSprite() { return stoneSprites[Random.Range(0,amountOfUniqueRandomTextures)];} // returns random stone texture

    private static Sprite[] GetGrassSprites() {
        Sprite[] sprites = new Sprite[amountOfUniqueRandomTextures];

        for(int i = 0; i < amountOfUniqueRandomTextures; i++) {
            sprites[i] = TextureToSprite(grassTextures[i]);
        }

        return sprites;
    }

    private static Sprite[] GetStoneSprites() {
        Sprite[] sprites = new Sprite[amountOfUniqueRandomTextures];

        for(int i = 0; i < amountOfUniqueRandomTextures; i++) {
            sprites[i] = TextureToSprite(stoneTextures[i]);
        }

        return sprites;
    }

    public static Sprite GetRandomSprite(string name) {
        if(name.Equals("Grass")) {return GetRandomGrassSprite();}
        if(name.Equals("Stone")) {return GetRandomStoneSprite();}

        return null;
    }

    public static Sprite TextureToSprite(Texture2D texture) {
        return Sprite.Create(
            texture,
            new Rect(0, 0, textureSize.x, textureSize.y),
            new Vector2(0.5f, 0.5f),
            16f // pixels per unit
        );
    }

}