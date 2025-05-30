using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[RequireComponent(typeof(RandomTextureGenerator))]
public class TerrainGenerator : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] private static Tilemap tilemap;

    private static Dictionary<string, Tile[]> tiles = new Dictionary<string, Tile[]>();

    private RandomTextureGenerator randomTextureGenerator;
    
    void Start() {

        randomTextureGenerator = gameObject.GetComponent<RandomTextureGenerator>();

        tilemap = transform.GetChild(0).GetChild(0).GetComponent<Tilemap>();

        SetUpTiles();

        for(int x = -50; x < 50; x++) {
            for(int y = -50; y < 50; y++) {
                PlaceBlock(x, y, "Grass");
            }
        }
    }

    private Tile[] GetTiles(string tileName) {
        Tile[] tiles = new Tile[RandomTextureGenerator.amountOfUniqueRandomTextures];
        Sprite[] sprites = RandomTextureGenerator.GetSprites(tileName);

        for(int i = 0; i < RandomTextureGenerator.amountOfUniqueRandomTextures; i++) {
            Tile newTile = ScriptableObject.CreateInstance<Tile>();

            newTile.sprite = sprites[i];

            tiles[i] = newTile;
        }

        return tiles;
    }

    public static void PlaceBlock(Tile tile, int x, int y, string blockName) {
        Vector3Int pos = new Vector3Int(x, y, 0);

        if(GetRandomTile(blockName) != null) {
            tilemap.SetTile(pos, GetRandomTile(blockName));
        } else {

            tilemap.SetTile(pos, tile);
        }
    }

    public static void PlaceBlock(int x, int y, string blockName) {
        Vector3Int pos = new Vector3Int(x, y, 0);

        tilemap.SetTile(pos, GetRandomTile(blockName));
    }

    private static Tile GetRandomTile(string name) {
        if(tiles.TryGetValue(name, out Tile[] result)) {return result[Random.Range(0,RandomTextureGenerator.amountOfUniqueRandomTextures)];}

        return null;
    }

    private void SetUpTiles() {
        for(int i = 0; i < randomTextureGenerator.generatedTileEntries.Length; i++) {
            string tileName = randomTextureGenerator.generatedTileEntries[i].tileName;
            tiles.Add(tileName, GetTiles(tileName));
        }
    }
}