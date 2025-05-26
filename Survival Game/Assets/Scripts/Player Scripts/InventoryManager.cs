using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private Item[] items;

    [SerializeField] private Item[] hotBarItems;
    [SerializeField] private GameObject hotBarGO;

    [SerializeField] private int slotSelected = 1;

    private Vector2Int inventorySize = new Vector2Int(4,6);

    void Start() {
        items = new Item[inventorySize.x * inventorySize.y];
        SetHotBarSprites();
    }

    void Update() {
        if(slotSelected < 1) {slotSelected = inventorySize.y;} // make the slot selected loop
        if(slotSelected > inventorySize.y) {slotSelected = 1;}

        if(Input.GetAxis("Mouse ScrollWheel") > 0f) {slotSelected++;} // scroll up
        if(Input.GetAxis("Mouse ScrollWheel") < 0f) {slotSelected--;} // scroll down
    }

    private void SetHotBarSprites() {
        int i = 0;

        foreach (Transform child in hotBarGO.transform) { // got for each child in the hot bar gameobject

            Image childImage = child.GetComponent<Image>(); // get the image of the child

            if(hotBarItems[i] != null) { // if there is an item in the hotbar coresponding with the image
                childImage.enabled = true; // we set the image to be active 
                childImage.sprite = hotBarItems[i].GetSprite(); // set the image to the item's sprite
            } else {
                childImage.enabled = false; // if it is not we set the image to be inactive
            }

            i++;
        }
    }
}