using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;


[RequireComponent(typeof(RandomTextureGenerator))]
public class TerrainGenerator : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] private static Tilemap backgroundTilemap;
    [SerializeField] private static Tilemap collidableTilemap;

    [SerializeField] private GameObject droppedItemPrefab;
    private static GameObject staticDroppedItemPrefab;

    private static Dictionary<string, TileData[]> tiles = new Dictionary<string, TileData[]>();

    private RandomTextureGenerator randomTextureGenerator;


    [Header("Settings")]
    [SerializeField] private TileType[] tileTypes;

    [SerializeField] private int radiusOfIsland;

    [Range(0,1)]
    [SerializeField] private float treeSpawnRate;

    [SerializeField] private int stoneCircleRadius;

    [Header("Evniorment Piceaces (what ever)")]
    [SerializeField] private GameObject tree;

    [SerializeField] private GameObject graveyard;
    [SerializeField] private int amountOfGraveyards = 2;
    [SerializeField] private int graveyardGrassRadius;

    // debug stuff
    private static Vector3Int brokenBlockPosition;

    public static List<Vector2> posOfCirlces = new List<Vector2>();
    public static List<float> radiusOfCircles = new List<float>();

    void Awake() {
        staticDroppedItemPrefab = droppedItemPrefab;
    }

    void Start() {
        randomTextureGenerator = gameObject.GetComponent<RandomTextureGenerator>();

        backgroundTilemap = gameObject.transform.GetChild(0).GetChild(0).GetComponent<Tilemap>();
        collidableTilemap = gameObject.transform.GetChild(0).GetChild(1).GetComponent<Tilemap>();

        SetUpTiles();

        GenerateIsland();
    }

    private TileData[] GetTiles(string tileName) {
        TileData[] tiles = new TileData[RandomTextureGenerator.amountOfUniqueRandomTextures];
        Sprite[] sprites = RandomTextureGenerator.GetSprites(tileName);

        for(int i = 0; i < RandomTextureGenerator.amountOfUniqueRandomTextures; i++) {
            TileData newTile = ScriptableObject.CreateInstance<TileData>();

            newTile.tileStrength = PlaceableItems.placeables[tileName].GetPlaceabledStrength();

            newTile.sprite = sprites[i];

            newTile.tileName = tileName;

            tiles[i] = newTile;
        }

        return tiles;
    }

    public static bool PlaceBlock(Tile tile, int x, int y, string blockName, bool collidable) {
        Vector3Int pos = new Vector3Int(x, y, 0);

        if(collidable && Physics2D.OverlapBoxAll(new Vector2(x + 0.5f, y + 0.5f), new Vector2(0.9f,0.9f), 0).Length != 0) {return false;}

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
        //  get the position of the tile in a vector 3 int because that is what tilemaps use
        Vector3Int pos = new Vector3Int(x, y, 0);

        if(collidable && Physics2D.OverlapBoxAll(new Vector2(x + 0.5f, y + 0.5f), new Vector2(0.9f,0.9f), 0).Length != 0 && collidableTilemap.GetTile(new(x,y,0)) == null) {return false;}

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
            TileData randomTile = result[Random.Range(0,RandomTextureGenerator.amountOfUniqueRandomTextures)];
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

    public static void BreakBlock(RaycastHit2D hit, float toolStrength) {
        Vector3Int tilePos = collidableTilemap.WorldToCell(hit.point);

        Vector3 hitNormal = hit.normal;

        if(collidableTilemap.GetTile(tilePos) == null) {
            tilePos -= new Vector3Int((int) hitNormal.x, (int) hitNormal.y, 0);
        }

        TileData tileBroken = collidableTilemap.GetTile(tilePos) as TileData;

        if(tileBroken == null) {
            return;
        }

        if(tileBroken.tileStrength >= toolStrength) {
            return;
        }

        collidableTilemap.SetTile(tilePos, null);

        Placeable placeable = Instantiate(PlaceableItems.placeables[tileBroken.tileName]);

        placeable.SetQuantity(1);

        GameObject droppedItem = Instantiate(staticDroppedItemPrefab);
        DroppedItem droppedItemComponent = droppedItem.GetComponent<DroppedItem>();

        droppedItem.transform.position = new Vector3(tilePos.x + 0.5f, tilePos.y + 0.5f, 0f);
        droppedItemComponent.item = placeable;

        droppedItemComponent.AddForceOnDrop(2);
        droppedItemComponent.AddRandomTorque(30);
        droppedItemComponent.StartCotrotean(0f);

        droppedItemComponent.ChangeSprite();

        brokenBlockPosition = tilePos;

    }

    private TileData GetTileDataFromHeight(float height) {
        for(int i = tileTypes.Length - 1; i >= 0; i--) {
            if(height > tileTypes[i].height) {
                TileData tileData = GetRandomTile(tileTypes[i].elevationTileTypeName);
                return tileData;
            }
        }

        throw new Exception("oh no");
    }

    private void GenerateIsland() {
        // place water in a big square

        for(int x = -radiusOfIsland*5; x < radiusOfIsland*5; x++) {
            for(int y = -radiusOfIsland*5; y < radiusOfIsland*5; y++) {
                PlaceBlock(x,y, "Water", false);
            }
        }

        // make the isalnd 
        for(int x = -radiusOfIsland; x < radiusOfIsland; x++) {
            for(int y = -radiusOfIsland; y < radiusOfIsland; y++) {

                float dis = Vector2.Distance(new(0,0), new(x,y));

                if(!(dis > radiusOfIsland)) {
                    float height = Random.Range(0.9f,1f);

                    if(dis / radiusOfIsland > 0.8f) {
                        height *= -25 * Mathf.Pow(dis / radiusOfIsland - 0.8f, 2) + 1;
                    }

                    TileData tilePlaced = GetTileDataFromHeight(height);

                    PlaceBlock(x,y, tilePlaced.tileName, PlaceableItems.placeables[tilePlaced.tileName].GetCollidable());
                }

            }
        }


        // make stone circle

        float randomAngle = Random.Range(0,2 * Mathf.PI);
        Vector2Int stoneCirclePos = new Vector2Int((int) Mathf.Round(Mathf.Cos(randomAngle) * (radiusOfIsland * 0.9f)), (int) Mathf.Round(Mathf.Sin(randomAngle) * (radiusOfIsland * 0.9f)));

        for(int x = Random.Range(-stoneCircleRadius,0); x < Random.Range(0,stoneCircleRadius); x++) {
            for(int y = Random.Range(-stoneCircleRadius,0); y < Random.Range(0,stoneCircleRadius); y++) {
                float dis = Vector2.Distance(stoneCirclePos, new(x + stoneCirclePos.x, y + stoneCirclePos.y));

                if(dis < stoneCircleRadius) {
                    PlaceBlock(x - (stoneCirclePos.x/2), y - (stoneCirclePos.y/2), "Stone", true);
                }
            }
        }


        // make the trees
        for(int x = -radiusOfIsland; x < radiusOfIsland; x++) {
            for(int y = -radiusOfIsland; y < radiusOfIsland; y++) {
                TileData tileData = backgroundTilemap.GetTile(new(x,y,0)) as TileData;

                if(tileData.tileName == "Grass" && collidableTilemap.GetTile(new(x,y,0)) == null && Random.Range(0f,1f) < treeSpawnRate && Vector2.Distance(Vector2.zero, new(x,y)) > 3f) {
                    GameObject treeCopy = Instantiate(tree);

                    treeCopy.transform.position = new(x,y,0);

                    treeCopy.GetComponent<SpriteRenderer>().sortingOrder = radiusOfIsland - y + 2;

                    DontDestroyOnLoad(treeCopy);
                }
            }
        }

        // spawn the graveyards
        for(int i = 0; i < amountOfGraveyards; i++) {
            Invoke(nameof(SpawnGraveyard), 0.1f);
        }
    }

    private void SpawnGraveyard() {
        // spanw the graveyard
        float randomAngle = Random.Range(0,2 * Mathf.PI);
        Vector2Int graveyardPos = new Vector2Int((int) Mathf.Round(Mathf.Cos(randomAngle) * (radiusOfIsland * 0.65f)), (int) Mathf.Round(Mathf.Sin(randomAngle) * (radiusOfIsland * 0.65f)));

        GameObject instantatedGraveyard = Instantiate(graveyard);
        instantatedGraveyard.transform.position = new Vector3(graveyardPos.x, graveyardPos.y, 0);

        DontDestroyOnLoad(instantatedGraveyard);


        // make the grass around the graveyard graveyard grass
        for(int x = -graveyardGrassRadius; x < graveyardGrassRadius; x++) {
            for(int y = -graveyardGrassRadius; y < graveyardGrassRadius; y++) {
                float disFromGraveyard = Vector2.Distance(graveyardPos, new(x + graveyardPos.x, y + graveyardPos.y));

                if(disFromGraveyard > graveyardGrassRadius) continue;

                float probabilityOfGraveyardGrass = Mathf.Pow(2f, -(disFromGraveyard / graveyardGrassRadius));

                if(probabilityOfGraveyardGrass < Random.Range(0f,1f)) {
                    PlaceBlock(x + graveyardPos.x, y + graveyardPos.y, "Graveyard Grass", false);
                }
            }
        }

        
        // destroy all of the trees in the area
        Collider2D[] collidersInGraveyardCircle = Physics2D.OverlapCircleAll(graveyardPos, graveyardGrassRadius + 3, ~0);
        //Debug.Log(collidersInGraveyardCircle.Length);

        posOfCirlces.Add(graveyardPos);
        radiusOfCircles.Add(graveyardGrassRadius);

        for(int i = 0; i < collidersInGraveyardCircle.Length; i++) {
            if(collidersInGraveyardCircle[i].CompareTag("Tree")) {
                collidersInGraveyardCircle[i].GetComponent<DropItemsOnDestroy>().dropItemsOnDestroy = false;
                Destroy(collidersInGraveyardCircle[i].gameObject);
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(brokenBlockPosition + Vector3.one * 0.5f, Vector3.one * 0.3f);

        Gizmos.color = Color.green;

        for (int i = 0; i < posOfCirlces.Count; i++)
        {
            Vector3 center = posOfCirlces[i];
            float radius = radiusOfCircles[i];
            Vector3 prevPoint = center + Vector3.right * radius;

            int segments = 36;

            for (int circleSegmentIndex = 1; circleSegmentIndex <= segments; circleSegmentIndex++)
            {
                float angle = (circleSegmentIndex * 2 * Mathf.PI) / segments;
                Vector3 newPoint = center + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
                Gizmos.DrawLine(prevPoint, newPoint);
                prevPoint = newPoint;
            }
        }
    }


}