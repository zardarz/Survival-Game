using UnityEngine;

[CreateAssetMenu(fileName = "New Pickaxe", menuName = "Tool/Pickaxe")]
public class Pickaxe : Tool
{

    [Header("Pickaxe")]
    [SerializeField] public LayerMask collidableTileMap;

    public override void Use() {
        Transform playerTransform = PlayerMovement.GetPlayerTransform();

        Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - playerTransform.position;

        RaycastHit2D hit = Physics2D.Raycast(playerTransform.position, dir, toolRange, collidableTileMap);

        TerrainGenerator.BreakBlock(hit, GetToolStrength());
    }
}