using UnityEngine;

[CreateAssetMenu(fileName = "New Axe", menuName = "Tool/Axe")]
public class Axe : Tool
{

    [Header("Axe")]
    [SerializeField] public LayerMask tree;
    public override void Use() {
        Transform playerTransform = PlayerMovement.GetPlayerTransform();

        Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - playerTransform.position;

        RaycastHit2D hit = Physics2D.Raycast(playerTransform.position, dir, toolRange, tree);

        if(hit.transform != null && hit.transform.CompareTag("Tree")) {
            if(hit.transform.GetComponent<TreeScript>().treeStrength <= GetToolStrength()) {
                hit.transform.GetComponent<DropItemsOnDestroy>().DropItems();
                Destroy(hit.transform.gameObject);
            }
        }
    }
}