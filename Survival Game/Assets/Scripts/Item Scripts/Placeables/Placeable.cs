using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Placeable", menuName = "Placeable")]
public class Placeable : Item
{
    public override void Use() {
        quantity--;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mousePos.x = Mathf.Round(mousePos.x);
        mousePos.y = Mathf.Round(mousePos.y);

        TerrainGenerator.PlaceBlock(GetSprite(), (int) mousePos.x, (int) mousePos.y);
    }
}