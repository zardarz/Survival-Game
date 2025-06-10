using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[RequireComponent(typeof(RandomTextureGenerator))]
public class TerrainGenerator : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] private static Tilemap backgroundTilemap;
    [SerializeField] private static Tilemap collidableTilemap;

    private static Dictionary<string, TileData[]> tiles = new Dictionary<string, TileData[]>();

    private RandomTextureGenerator randomTextureGenerator;
    
    void Start() {

        randomTextureGenerator = gameObject.GetComponent<RandomTextureGenerator>();

        backgroundTilemap = gameObject.transform.GetChild(0).GetChild(0).GetComponent<Tilemap>();
        collidableTilemap = gameObject.transform.GetChild(0).GetChild(1).GetComponent<Tilemap>();

        SetUpTiles();

        for(int x = -50; x < 50; x++) {
            for(int y = -50; y < 50; y++) {
                PlaceBlock(x, y, "Wood", true);
            }
        }
    }

    private TileData[] GetTiles(string tileName) {
        TileData[] tiles = new TileData[RandomTextureGenerator.amountOfUniqueRandomTextures];
        Sprite[] sprites = RandomTextureGenerator.GetSprites(tileName);

        for(int i = 0; i < RandomTextureGenerator.amountOfUniqueRandomTextures; i++) {
            TileData newTile = ScriptableObject.CreateInstance<TileData>();

            newTile.sprite = sprites[i];

            newTile.tileName = tileName;

            tiles[i] = newTile;
        }

        return tiles;
    }

    public static bool PlaceBlock(Tile tile, int x, int y, string blockName, bool collidable) {
        Vector3Int pos = new Vector3Int(x, y, 0);

        if(collidable && Physics2D.OverlapBoxAll(new Vector2(pos.x + 0.5f, pos.y + 0.5f), new Vector2(0.9f,0.9f), 0).Length != 0) {return false;}

        Tilemap tilemap;

        if(collidable) {
            tilemap = collidableTilemap;
        } else {
            tilemap = backgroundTilemap;
        }

        if(GetRandomTile(blockName) != null) {
            tilemap.SetTile(pos, GetRandomTile(blockName));
        } else {
            tilemap.SetTile(pos, tile);
        }

        return true;
    }

    public static bool PlaceBlock(int x, int y, string blockName, bool collidable) {
        Vector3Int pos = new Vector3Int(x, y, 0);

        if(collidable && Physics2D.OverlapBoxAll(new Vector2(pos.x + 0.5f, pos.y + 0.5f), new Vector2(0.9f,0.9f), 0).Length != 0) {return false;}

        Tilemap tilemap;

        if(collidable) {
            tilemap = collidableTilemap;
        } else {
            tilemap = backgroundTilemap;
        }

        tilemap.SetTile(pos, GetRandomTile(blockName));

        return true;
    }

    private static TileData GetRandomTile(string name) {
        if(tiles.TryGetValue(name, out TileData[] result)) {
            TileData randomTile = result[UnityEngine.Random.Range(0,RandomTextureGenerator.amountOfUniqueRandomTextures)];
            return randomTile;
        }

        return null;
    }

    private void SetUpTiles() {
        for(int i = 0; i < randomTextureGenerator.generatedTileEntries.Length; i++) {
            string tileName = randomTextureGenerator.generatedTileEntries[i].tileName;
            tiles.Add(tileName, GetTiles(tileName));
        }
    }

    public static void BreakBlock(RaycastHit2D hit, float angleOfRay) {
        // this doesn't give you the correct position
        Vector3Int tilePos = collidableTilemap.WorldToCell(hit.point);

        // so we fix it here
        Vector3 hitFrom = hit.normal;

        if(!(angleOfRay > -45f && angleOfRay < 135f)) {
            tilePos -= new Vector3Int((int)hitFrom.x, (int)hitFrom.y, 0);
        }

        TileData tileBroken = collidableTilemap.GetTile(tilePos) as TileData;

        if(tileBroken != null) {
            Placeable placeable = Instantiate(PlaceableItems.placeables[tileBroken.tileName]);

            placeable.SetQuantity(1);

            InventoryManager.AddItemToInventory(placeable);
        }

        collidableTilemap.SetTile(tilePos, null);
    }
}