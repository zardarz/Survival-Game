using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainGenerator : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] private static Tilemap tilemap;

    void Start() {

        tilemap = transform.GetChild(0).GetChild(0).GetComponent<Tilemap>();

        for(int x = -50; x < 50; x++) {
            for(int y = -50; y < 50; y++) {
                PlaceGrass(x, y);
            }
        }
    }

    private void PlaceGrass(int x, int y) {
        Tile grassTile = ScriptableObject.CreateInstance<Tile>(); // make a new tile

        Sprite grassSprite = RandomTextureGenerator.GetRandomGrassSprite();
        
        grassTile.sprite = grassSprite; // apply the sprite to the tile

        tilemap.SetTile(new Vector3Int(x,y,0), grassTile); // place the tile
    }

    public static void PlaceBlock(Sprite sprite, int x, int y, string blockName) {
        Vector3Int pos = new Vector3Int(x, y, 0);

        Tile newTile = ScriptableObject.CreateInstance<Tile>();

        if(RandomTextureGenerator.GetRandomSprite(blockName) == null) {newTile.sprite = sprite;} else 
        {newTile.sprite = RandomTextureGenerator.GetRandomSprite(blockName);}


        tilemap.SetTile(pos, newTile);
    }
}