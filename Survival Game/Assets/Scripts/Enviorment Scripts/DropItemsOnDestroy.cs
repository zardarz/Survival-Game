using UnityEngine;
using Random = UnityEngine.Random;

public class DropItemsOnDestroy : MonoBehaviour
{
    
    [SerializeField] private DropItemOnDestroyEntry[] dropItemOnDestroyEntries;

    [SerializeField] private GameObject droppedItemPrefab;

    private bool isQuitting = false;

    void OnDestroy()
    {
        // this makes sure it doesn't drop items when i stop the game
        if(isQuitting) return;

        for(int i = 0; i < dropItemOnDestroyEntries.Length; i++) {
            DropItemOnDestroyEntry dropItemOnDestroyEntry = dropItemOnDestroyEntries[i];
            int amountOfItem = Random.Range(dropItemOnDestroyEntry.minAmountOfItem, dropItemOnDestroyEntry.maxAmountOfItem);
            DropItem(dropItemOnDestroyEntry.itemToDrop, amountOfItem);
        }
    }

    private void DropItem(Item itemToDrop, int amountOfItem) {
        for(int i = 0; i < amountOfItem; i++) {
            GameObject droppedItem = Instantiate(droppedItemPrefab);
            DroppedItem droppedItemComponent = droppedItem.GetComponent<DroppedItem>();

            droppedItem.transform.position = transform.position;

            droppedItemComponent.item = Instantiate(itemToDrop);
            droppedItemComponent.AddForceOnDrop(2);
            droppedItemComponent.AddRandomTorque(30);

            droppedItemComponent.ChangeSprite();

            droppedItemComponent.StartCotrotean(0f);
        }
    }

    void OnApplicationQuit() {
        isQuitting = true;
    }
}