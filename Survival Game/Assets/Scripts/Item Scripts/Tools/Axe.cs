using UnityEngine;

[CreateAssetMenu(fileName = "New Axe", menuName = "Axe")]
public class Axe : Tool
{

    [Header("Axe")]
    [SerializeField] public LayerMask tree;
    public override void Use() {
        Transform playerTransform = PlayerMovement.GetPlayerTransform();

        Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - playerTransform.position;

        RaycastHit2D hit = Physics2D.Raycast(playerTransform.position, dir, toolRange, tree);

        if(hit.transform != null && hit.transform.CompareTag("Tree") && hit.transform.GetComponent<TreeScript>().treeStrength <= GetToolStrength()) {
            Placeable instantiatedWood = Instantiate(PlaceableItems.placeables["Wood"]);

            instantiatedWood.SetQuantity(Random.Range(1,3));

            InventoryManager.AddItemToInventory(instantiatedWood);
            Destroy(hit.collider.gameObject);
        }
    }
}