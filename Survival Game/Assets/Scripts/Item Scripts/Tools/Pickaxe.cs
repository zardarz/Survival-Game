using UnityEngine;

[CreateAssetMenu(fileName = "New Pickaxe", menuName = "Pickaxe")]
public class Pickaxe : Tool
{
    public override void Use() {
        Transform playerTransform = PlayerMovement.GetPlayerTransform();

        Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - playerTransform.position;

        RaycastHit2D hit = Physics2D.Raycast(playerTransform.position, dir, toolRange, tilemap);
        Debug.DrawRay(playerTransform.position, dir, Color.black, 100);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        TerrainGenerator.BreakBlock(hit, angle);
    }
}