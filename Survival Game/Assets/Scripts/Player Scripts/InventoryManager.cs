using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private Item[] items;
    [SerializeField] private Item[] hotBarItems;
    [SerializeField] private GameObject hotBarGO;

    [SerializeField] private int slotSelected = 1;
    private Item itemSelected;
    [SerializeField] private SpriteRenderer hand;

    private Vector2Int inventorySize = new Vector2Int(4,6);

    void Start() {
        items = new Item[inventorySize.x * inventorySize.y];
    }

    void Update() {
        SetHotBarSprites();
        SetHotBarCounts();

        HandleHotBarInputs();
        LoopSelectedSlot();

        if(hotBarItems[slotSelected - 1] == null) {hideSelectedItem(); return;} // if there is no item in the slected slot we return

        SetItemSelected();
        TryToShowSelectedItem();
    }

    private void HandleHotBarInputs() {
        if(Input.GetAxis("Mouse ScrollWheel") > 0f) {slotSelected++;} // scroll up
        if(Input.GetAxis("Mouse ScrollWheel") < 0f) {slotSelected--;} // scroll down

        if(Input.GetKeyDown(KeyCode.Alpha1)) {slotSelected = 1;} // pressed one
        if(Input.GetKeyDown(KeyCode.Alpha2)) {slotSelected = 2;} // pressed two
        if(Input.GetKeyDown(KeyCode.Alpha3)) {slotSelected = 3;} // pressed three
        if(Input.GetKeyDown(KeyCode.Alpha4)) {slotSelected = 4;} // pressed four
        if(Input.GetKeyDown(KeyCode.Alpha5)) {slotSelected = 5;} // pressed five
        if(Input.GetKeyDown(KeyCode.Alpha6)) {slotSelected = 6;} // pressed six
    }

    private void LoopSelectedSlot() {
        if(slotSelected < 1) {slotSelected = inventorySize.y;} // make the slot selected loop
        if(slotSelected > inventorySize.y) {slotSelected = 1;}
    }

    private void SetItemSelected() {
        itemSelected = hotBarItems[slotSelected - 1]; // get the selected item
    }

    private void TryToShowSelectedItem() {
        if(Input.GetMouseButton(0) || itemSelected.GetShowWhenHolding()) { // if the mouse button is down or the item should be shown we show it
            showSelectedItem();
        } else { // else we hide it
            hideSelectedItem();
        }
    }

    private void showSelectedItem() {
        hand.sprite = itemSelected.GetSprite();
    }

    private void hideSelectedItem() {
        hand.sprite = null;
    }

    private void SetHotBarSprites() {
        int i = 0;

        foreach (Transform child in hotBarGO.transform) { // go for each child in the hot bar gameobject

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

    private void SetHotBarCounts() {
        int i = 0;

        foreach (Transform child in hotBarGO.transform) { // go for each child in the hot bar gameobject

            if(hotBarItems[i] == null) { // if there is no item in the slot
                child.GetChild(0).GetComponent<TMP_Text>().text = ""; // set the text to be nothing
                i++;  // continue to the next child
                continue;
            }

            if(hotBarItems[i].GetQuantity() == 0) { // if there is no count, we set the text to be nothing
                child.GetChild(0).GetComponent<TMP_Text>().text = "";
            } else {
                child.GetChild(0).GetComponent<TMP_Text>().text = hotBarItems[i].GetQuantity().ToString(); // get the child (which is a text mesh pro) and set the text to the count of the item
            }


            i++;
        }
    }
}