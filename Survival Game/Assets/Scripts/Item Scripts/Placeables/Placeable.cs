using UnityEngine;

[CreateAssetMenu(fileName = "New Placeable", menuName = "Placeable")]
public class Placeable : Item
{
    [Header("Placeable")]
    [SerializeField] private TileData tile;

    [SerializeField] private bool collidable;

    [SerializeField] private float placeableStrength;

    public override void Use() {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mousePos.x = Mathf.Floor(mousePos.x);
        mousePos.y = Mathf.Floor(mousePos.y);

        if(TerrainGenerator.PlaceBlock(tile, (int) mousePos.x, (int) mousePos.y, GetName(), collidable)) {
            quantity--;
        }
    }

    public bool GetCollidable() {
        return collidable;
    }

    public float GetPlaceabledStrength() {
        return placeableStrength;
    }
}