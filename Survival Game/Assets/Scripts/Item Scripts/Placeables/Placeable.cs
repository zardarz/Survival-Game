using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Placeable", menuName = "Placeable")]
public class Placeable : Item
{
    [SerializeField] private Tile tile;

    [SerializeField] private bool collidable;

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
}