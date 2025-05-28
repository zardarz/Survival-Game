using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Placeable", menuName = "Placeable")]
public class Placeable : Item
{
    [SerializeField] private Tile tile;

    public override void Use() {
        quantity--;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mousePos.x = Mathf.Floor(mousePos.x);
        mousePos.y = Mathf.Floor(mousePos.y);

        TerrainGenerator.PlaceBlock(tile, (int) mousePos.x, (int) mousePos.y, GetName());
    }
}