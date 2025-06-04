using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class InventoryManager : MonoBehaviour
{
    [SerializeField] private Item[] inventoryItems;
    [SerializeField] private GameObject inventoryGO;
    
    [SerializeField] private Item[] hotBarItems;
    [SerializeField] private GameObject hotBarGO;

    [SerializeField] private int slotSelected = 1;
    private Item itemSelected;
    [SerializeField] private SpriteRenderer hand;

    private Vector2Int inventorySize = new Vector2Int(3,6);

    private int totalSlots;

    [SerializeField] private GameObject selectedItemGO;
    private bool hasSelectedItemOnMouse;
    private bool inventoryIsOpened;

    private int prevSlotSelected;

    public Item testItem;

    public Placeable testPlaceable;

    public Placeable testTestPlaceable;

    void Start() {
        totalSlots = inventorySize.x * inventorySize.y;

        inventoryItems = new Item[totalSlots];

        hotBarItems[0] = Instantiate(testItem);
        hotBarItems[1] = Instantiate(testPlaceable);
        hotBarItems[2] = Instantiate(testTestPlaceable);


        inventoryItems[7] = Instantiate(testItem);
        inventoryItems[10] = Instantiate(testPlaceable);
    }

    void Update() {
        SetHotBarSprites();
        SetHotBarCounts();

        SetInventorySprites();
        SetInventoryCounts();

        HandleHotBarInputs();
        HandleInventoryInputs();
        LoopSelectedSlot();

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

        if(Input.GetMouseButtonDown(0) && itemSelected != null && !inventoryIsOpened) {itemSelected.Use();}
    }

    private void LoopSelectedSlot() {
        if(slotSelected < 1) {slotSelected = inventorySize.y;} // make the slot selected loop
        if(slotSelected > inventorySize.y) {slotSelected = 1;}
    }

    private void SetItemSelected() {
        if(hotBarItems[slotSelected - 1] == null) {itemSelected = null; hideSelectedItem();} else {
            itemSelected = hotBarItems[slotSelected - 1]; // get the selected item
        }
    }

    private void TryToShowSelectedItem() {
        if(itemSelected == null) {return;}

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
            Item currentItem = hotBarItems[i];

            if(currentItem == null) { // if there is no item there
                childImage.enabled = false; // image is disabled
                i++; // and we continue
                continue;
            }



            if(currentItem.GetGeneratedSprite() && !currentItem.GetSpriteWasGenerated()) { // if the item's sprite should be generated we genrate it
                currentItem.GenerateSprite();
                currentItem.SpriteWasGenerated();
            }


            childImage.enabled = true; // we set the image to be active 
            childImage.sprite = currentItem.GetSprite(); // set the image to the item's sprite

            i++;
        }
    }

    private void SetHotBarCounts() {
        int i = 0;

        foreach (Transform child in hotBarGO.transform) { // go for each child in the hot bar gameobject

            Item currentItem = hotBarItems[i];

            if(currentItem == null || currentItem.quantity <= 0) { // if there is no item in the slot or the item quanitiy is at 0
                child.GetChild(0).GetComponent<TMP_Text>().text = ""; // set the text to be nothing
                hotBarItems[i] = null; // remove the item from the hotbar
                i++;  // continue to the next child
                continue;
            }

            if(currentItem.GetQuantity() == 0) { // if there is no count, we set the text to be nothing
                child.GetChild(0).GetComponent<TMP_Text>().text = "";
            } else {
                child.GetChild(0).GetComponent<TMP_Text>().text = currentItem.GetQuantity().ToString(); // get the child (which is a text mesh pro) and set the text to the count of the item
            }


            i++;
        }
    }


    private void SetInventorySprites() {
        int i = 0;

        foreach(Transform child in inventoryGO.transform) {

            Image childImage = child.GetComponent<Image>();
            Item currentItem = inventoryItems[i];

            if(currentItem == null) { // if there is no item there
                childImage.enabled = false; // image is disabled
                i++; // and we continue
                continue;
            }

            if(currentItem.GetGeneratedSprite() && !currentItem.GetSpriteWasGenerated()) { // if the item's sprite should be generated we genrate it
                currentItem.GenerateSprite();
                currentItem.SpriteWasGenerated();
            }


            childImage.enabled = true; // we set the image to be active 
            childImage.sprite = currentItem.GetSprite(); // set the image to the item's sprite

            i++;
        }
    }

    private void SetInventoryCounts() {
        int i = 0;

        foreach (Transform child in inventoryGO.transform) { // go for each child in the inventory gameobject

            Item currentItem = inventoryItems[i];

            if(currentItem == null || currentItem.quantity <= 0) { // if there is no item in the slot or the item quanitiy is at 0
                child.GetChild(0).GetComponent<TMP_Text>().text = ""; // set the text to be nothing
                inventoryItems[i] = null; // remove the item from the inventory
                i++;  // continue to the next child
                continue;
            }

            if(currentItem.GetQuantity() == 0) { // if there is no count, we set the text to be nothing
                child.GetChild(0).GetComponent<TMP_Text>().text = "";
            } else {
                child.GetChild(0).GetComponent<TMP_Text>().text = currentItem.GetQuantity().ToString(); // get the child (which is a text mesh pro) and set the text to the count of the item
            }


            i++;
        }
    }

    private void HandleInventoryInputs() {
        if(Input.GetKeyDown(KeyCode.E)) {ToggleInventory();}
    }

    private void ToggleInventory() { // we have to set the parent to unactive so we cant see the background
        if(inventoryGO.transform.parent.gameObject.activeSelf) {
            inventoryGO.transform.parent.gameObject.SetActive(false);
            inventoryIsOpened = false;
        } else {
            inventoryGO.transform.parent.gameObject.SetActive(true);
            inventoryIsOpened = true;
        }
    }


    public void OnButtonClick() {
        GameObject clickedObj = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
        string numberString = clickedObj.name.Substring(11);
        bool isInInventory = false;
        Image selectedItemImage = selectedItemGO.GetComponent<Image>();

        if(numberString[0].Equals('l')) {numberString = numberString.Substring(4); isInInventory = true;}

        int slot = int.Parse(numberString);

        if(hasSelectedItemOnMouse) {
            if(!isInInventory) {
                SwapItems(prevSlotSelected, slot + totalSlots - 1);
            } else {
                SwapItems(prevSlotSelected, slot - 1);
            }
            selectedItemImage.gameObject.SetActive(false);
            return;
        }

        if(!isInInventory && !inventoryIsOpened) {slotSelected = slot; return;}

        Item selectedItem = null;

        if(isInInventory) {selectedItem = inventoryItems[slot - 1];} else
        {selectedItem = hotBarItems[slot - 1];}

        if(selectedItem != null) { 

            selectedItemImage.sprite = selectedItem.GetSprite();
            if(selectedItem.GetQuantity() == 0) {
                selectedItemImage.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = "";    
            } else {
                selectedItemImage.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = selectedItem.GetQuantity().ToString();
            }

            hasSelectedItemOnMouse = true;
            selectedItemImage.gameObject.SetActive(true);
        }

        if(!inventoryIsOpened || selectedItem == null) {selectedItemImage.gameObject.SetActive(false);}

        if(isInInventory) {
            prevSlotSelected = slot - 1;
        } else {
            prevSlotSelected = slot + totalSlots - 1;
        }

    }

    private void SwapItems(int prevSlot, int slot) {
        Item temp = null;

        bool prevSlotIsInInventory = !(prevSlot >= totalSlots);
        bool slotIsInInventory = !(slot >= totalSlots);

        if(!prevSlotIsInInventory) {prevSlot -= totalSlots;}
        if(!slotIsInInventory) {slot -= totalSlots;}

        //Debug.Log("Prev Slot is in Inventory: " + prevSlotIsInInventory + "\nSlot is in Inventory: " + slotIsInInventory);
        //Debug.Log(prevSlot + " to " + slot);


        if(prevSlotIsInInventory && slotIsInInventory) {
            // both slots are in the inventory

            temp = inventoryItems[prevSlot];

            inventoryItems[prevSlot] = inventoryItems[slot];
            inventoryItems[slot] = temp;
        } else if (!prevSlotIsInInventory && slotIsInInventory) {
            // prev slot is in hotbar

            temp = hotBarItems[prevSlot];

            hotBarItems[prevSlot] = inventoryItems[slot];
            inventoryItems[slot] = temp;
        } else if (prevSlotIsInInventory && !slotIsInInventory) {
            // slot is in hotbar

            temp = inventoryItems[prevSlot];

            inventoryItems[prevSlot] = hotBarItems[slot];
            hotBarItems[slot] = temp;
        } else {
            // both slots are in hotbar

            temp = hotBarItems[prevSlot];

            hotBarItems[prevSlot] = hotBarItems[slot];
            hotBarItems[slot] = temp;
        }

        hasSelectedItemOnMouse = false;
    }
}