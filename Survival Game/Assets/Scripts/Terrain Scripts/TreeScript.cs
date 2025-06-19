using UnityEngine;

public class TreeScript : MonoBehaviour
{
    public float treeStrength;

    [SerializeField] private Placeable typeOfWoodDrop;

    [SerializeField] private GameObject droppedItemPrefab;

    public void TryDestoryTree(float toolStrength) {
        if(treeStrength > toolStrength) {
            return;
        }

        int amountOfWood = Random.Range(1,4);

        for(int i = 0; i < amountOfWood; i++) {
            GameObject droppedItem = Instantiate(droppedItemPrefab);
            DroppedItem droppedItemComponent = droppedItem.GetComponent<DroppedItem>();

            droppedItem.transform.position = transform.position;

            droppedItemComponent.item = Instantiate(typeOfWoodDrop);
            droppedItemComponent.AddForceOnDrop(2);
            droppedItemComponent.AddRandomTorque(30);

            droppedItemComponent.ChangeSprite();

            droppedItemComponent.StartCotrotean(0.5f);
        }

        Destroy(gameObject);
    }
}