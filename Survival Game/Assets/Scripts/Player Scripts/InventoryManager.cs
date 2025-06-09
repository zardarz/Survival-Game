using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory/Hotbar Variables")]
    [SerializeField] private Item[] inventoryItems;
    [SerializeField] private GameObject inventoryGO;
    
    [SerializeField] private Item[] hotBarItems;
    [SerializeField] private GameObject hotBarGO;


    [Header("Showing What Item is Selected")]
    [SerializeField] private int slotSelected = 1;
    private Item itemSelectedInHand;
    [SerializeField] private SpriteRenderer hand;

    // Inventory settings
    private Vector2Int inventorySize = new Vector2Int(3,6);
    private int totalSlots;
    private bool inventoryIsOpened;

    // Fire rate stuff
    private float nextTimeToFire;


    [Header("Inventory Swapping")]
    [SerializeField] private GameObject selectedItemWithCursorGO;
    private Image selectedItemWithCursorImage;
    private TMP_Text selectedItemWithCursorCount;
    private Item itemSelectedWithCursor;

    [Header("Dropping Items")]
    [SerializeField] private GameObject droppedItemPrefab;
    [SerializeField] private float droppingItemStrength;

    [Header("Test Items")]
    public Item testItem;

    public Placeable testPlaceable;

    public Placeable testTestPlaceable;

    void Start() {
        totalSlots = inventorySize.x * inventorySize.y;

        inventoryItems = new Item[totalSlots];
        hotBarItems = new Item[inventorySize.y];

        selectedItemWithCursorImage = selectedItemWithCursorGO.GetComponent<Image>();
        selectedItemWithCursorCount = selectedItemWithCursorGO.transform.GetChild(0).GetComponent<TMP_Text>();

        AddItemToInventory(Instantiate(testItem));
        AddItemToInventory(Instantiate(testPlaceable));
        AddItemToInventory(Instantiate(testTestPlaceable));
        AddItemToInventory(Instantiate(testItem));
        AddItemToInventory(Instantiate(testPlaceable));
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

        SetSelectedItemWithCursorImageAndCount();
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

        if(Input.GetMouseButtonDown(0) && itemSelectedInHand != null && !inventoryIsOpened) {itemSelectedInHand.Use();}

        if(Input.GetMouseButton(0) && itemSelectedInHand is Tool && Time.time >= nextTimeToFire) {
            nextTimeToFire = Time.time + 1f / itemSelectedInHand.GetToolSpeed();
            itemSelectedInHand.Use();
        }
    }

    private void LoopSelectedSlot() {
        if(slotSelected < 1) {slotSelected = inventorySize.y;} // make the slot selected loop
        if(slotSelected > inventorySize.y) {slotSelected = 1;}
    }

    private void SetItemSelected() {
        if(hotBarItems[slotSelected - 1] == null) {itemSelectedInHand = null; hideSelectedItem();} else {
            itemSelectedInHand = hotBarItems[slotSelected - 1]; // get the selected item
        }
    }

    private void TryToShowSelectedItem() {
        if(itemSelectedInHand == null) {return;}

        if(Input.GetMouseButton(0) || itemSelectedInHand.GetShowWhenHolding()) { // if the mouse button is down or the item should be shown we show it
            showSelectedItem();
        } else { // else we hide it
            hideSelectedItem();
        }
    }

    private void showSelectedItem() {
        hand.sprite = itemSelectedInHand.GetSprite();
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
        if(Input.GetKeyDown(KeyCode.E)) {ToggleInventory();} // toggle inventory
        if(Input.GetKeyDown(KeyCode.Q)) {DropItem();} // drop item
    }

    private void DropItem() {
        if(itemSelectedInHand == null) {return;} // if we don't have an item in our hand we don't do anything

        GameObject newDroppedItem = Instantiate(droppedItemPrefab); // make a dropped item prefab

        newDroppedItem.GetComponent<SpriteRenderer>().sprite = itemSelectedInHand.GetSprite(); // set the dropped item sprite to the item sprite
        newDroppedItem.transform.position = hand.transform.position; // set its position to the player's hand position

        Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized; // find the direction of the mouse relative to the player

        newDroppedItem.GetComponent<Rigidbody2D>().AddForce(dir * droppingItemStrength, ForceMode2D.Impulse); // add some force to the dropped item
        newDroppedItem.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-30,30)); // add to spinyness to it 
        newDroppedItem.GetComponent<DroppedItem>().item = Instantiate(itemSelectedInHand); // set the item on the dropped item to the corresponding item
        newDroppedItem.GetComponent<DroppedItem>().item.SetQuantity(1);

        StartCoroutine(EnablePickingUpAfterTime(0.5f, newDroppedItem.GetComponent<DroppedItem>())); // enable picking up after .5 seconds

        itemSelectedInHand.AddToQuantity(-1); // drop the item quantity by one
    }

    private void ToggleInventory() {
        // we are using the parent of the inventory gameobject because i set the parent to the background which also needs to be disactivated 

        // if the parent is active we set it to unactive and visersa
        if(inventoryGO.transform.parent.gameObject.activeSelf) {
            inventoryGO.transform.parent.gameObject.SetActive(false);
            inventoryIsOpened = false;
        } else {
            inventoryGO.transform.parent.gameObject.SetActive(true);
            inventoryIsOpened = true;
        }
    }

    public void OnButtonClick(GameObject buttonObj, bool wasRightClick) {
        // get the object that was clicked
        GameObject clickedObj = buttonObj.transform.parent.gameObject;

        // get the slot of it 
        // hotbar slot number is the 11th character
        string numberString = clickedObj.name.Substring(11);
        bool isInInventory = false;

        // if the 11th charcter is an l, that means it is in the inventory
        if(numberString[0].Equals('l')) {numberString = numberString.Substring(4); isInInventory = true;}

        // turn the string to an int
        int slot = int.Parse(numberString) - 1;

        // if the slot is in the hotbar and the inventory is closed we turn the selected slot to the slot that was clicked 
        if(!isInInventory && !inventoryIsOpened) {
            slotSelected = slot + 1;
            return;
        }

        // if it was a right click we add the selected item to the slot it was in
        if(wasRightClick) {
            AddItemToItem(slot, isInInventory);
            return;
        }

        if(Input.GetKey(KeyCode.LeftShift)) {
            CollectAllItems(slot, isInInventory);
            return;
        }

        // add item in swpping checks if the item we are swapping is the same as the item we have selected
        // if it is false that means we didn't add so we can swap
        if(AddItemsInSwapping(slot, isInInventory) == false) {
            SwapItems(slot, isInInventory);
        }
    }

    private void SetSelectedItemWithCursorImageAndCount() {
        // if the selected item with currsor is nothing 
        // we turn of the iamge and count

        if(itemSelectedWithCursor == null) {
            selectedItemWithCursorImage.enabled = false;
            selectedItemWithCursorCount.enabled = false;
            return;
        }

        // else we turn them on
        selectedItemWithCursorImage.enabled = true;
        selectedItemWithCursorCount.enabled = true;

        // and set its sprite to the item selected sprite and set the count to the items quantity
        selectedItemWithCursorImage.sprite = itemSelectedWithCursor.GetSprite();
        selectedItemWithCursorCount.text = itemSelectedWithCursor.GetQuantity().ToString();
    }

    private void SwapItems(int slot, bool isInInventory) {
        Item temp;
        
        // if the swap is in the inventory we swap with the item in the inventory
        if(isInInventory) {
            temp = inventoryItems[slot];

            inventoryItems[slot] = itemSelectedWithCursor;
            itemSelectedWithCursor = temp;
        } else {
            // else we swap with the hotbar 
            temp = hotBarItems[slot];

            hotBarItems[slot] = itemSelectedWithCursor;
            itemSelectedWithCursor = temp;
        }
    }

    private bool AddItemsInSwapping(int slot, bool isInInventory) {
        Item item;

        // get the item either in the hotbar or the inventory
        if(isInInventory) {
            item = inventoryItems[slot];
        } else {
            item = hotBarItems[slot];
        }

        // if the item in the cursor or the item in the slot is null then we don't add
        if(itemSelectedWithCursor == null || item == null) {
            return false;
        }

        // else we check if the item is the same as the item in the cursor
        if(item.Equals(itemSelectedWithCursor)) {
            // we calculate how much space there is left in the slot 
            int amountLeft = item.GetMaxStack() - item.GetQuantity();

            // and if it is more than or equal to 0 we add it to the item and remove it from the item in the cursor
            if(!(amountLeft >= itemSelectedWithCursor.GetQuantity())) {
                item.AddToQuantity(amountLeft);
                itemSelectedWithCursor.AddToQuantity(-amountLeft);
                return true;
            }

            // else we add all of it the item
            item.AddToQuantity(itemSelectedWithCursor.GetQuantity());
            itemSelectedWithCursor.AddToQuantity(-itemSelectedWithCursor.GetQuantity());

            return true;
        }

        return false;
    }

    private void AddItemToItem(int slot, bool isInInventory) {
        // if we don't have a selected item we don't do anything
        if(itemSelectedWithCursor == null) {return;}

        Item item;

        // get the item if it is in the hotbar or inventory
        if(isInInventory) {
            item = inventoryItems[slot];

            // if there is no item in the slot
            if(item == null) {
                // set that slot to the item in the cursor and set its quantity to 1
                inventoryItems[slot] = Instantiate(itemSelectedWithCursor);
                inventoryItems[slot].SetQuantity(1);
                itemSelectedWithCursor.AddToQuantity(-1);
            } else if(item.Equals(itemSelectedWithCursor) && !(item.GetQuantity() + 1 > item.GetMaxStack())) {
                // else we add 1 to its quantity
                inventoryItems[slot].AddToQuantity(1);
                itemSelectedWithCursor.AddToQuantity(-1);
            }

        } else {
            item = hotBarItems[slot];

            // same thing but with the hotbar
            if(item == null) {
                hotBarItems[slot] = Instantiate(itemSelectedWithCursor);
                hotBarItems[slot].SetQuantity(1);
                itemSelectedWithCursor.AddToQuantity(-1);
            } else if(item.Equals(itemSelectedWithCursor) && !(item.GetQuantity() + 1 > item.GetMaxStack())) {
                hotBarItems[slot].AddToQuantity(1);
                itemSelectedWithCursor.AddToQuantity(-1);
            }
        }
    }

    private void CollectAllItems(int slot, bool isInInventory) {
        // get the item
        Item item;

        if (isInInventory) {
            item = inventoryItems[slot];
        } else {
            item = hotBarItems[slot];
        }

        // go foreach inventory slot
        for(int i = 0; i < totalSlots; i++) {
            // get the amount of quantity left in the pressed slot 
            int amountAbleToBeTaken = item.GetMaxStack() - item.GetQuantity();

            // if it is left than or equal to 0 we return
            if(amountAbleToBeTaken <= 0) {
                return;
            }

            // get the item we need to collect
            Item itemToCollect = inventoryItems[i];

            // if the item is null or the item is not the same as the pressed item or we are on the same slot we continue to the next slot
            if(itemToCollect == null || !itemToCollect.Equals(item) || slot == i) {
                continue;
            }

            // if the quantity in the item we want to collect is less than the amount of space we have left
            if(itemToCollect.GetQuantity() < amountAbleToBeTaken) {
                // we add all of it to te pressed slot and remove it from the collected slot
                item.AddToQuantity(itemToCollect.GetQuantity());
                itemToCollect.AddToQuantity(-itemToCollect.GetQuantity());
            } else {
                // else we take and remove as much as we can 
                item.AddToQuantity(amountAbleToBeTaken);
                itemToCollect.AddToQuantity(-amountAbleToBeTaken);
            }
        }

        // same thing but for the hotbar

        for(int i = 0; i < hotBarItems.Length; i++) {
            int amountAbleToBeTaken = item.GetMaxStack() - item.GetQuantity();

            if(amountAbleToBeTaken <= 0) {
                return;
            }

            Item itemToCollect = hotBarItems[i];

            if(itemToCollect == null || !itemToCollect.Equals(item) || slot == i) {
                continue;
            }

            if(itemToCollect.GetQuantity() < amountAbleToBeTaken) {
                item.AddToQuantity(itemToCollect.GetQuantity());
                itemToCollect.AddToQuantity(-itemToCollect.GetQuantity());
            } else {
                item.AddToQuantity(amountAbleToBeTaken);
                itemToCollect.AddToQuantity(-amountAbleToBeTaken);
            }
        }
    }

    public void AddItemToInventory(Item itemToAdd) {
        int i = 0;

        // Try to add item to hotbar
        foreach(Item item in hotBarItems) {

            // if the item is blank we add it
            if(item == null) {
                hotBarItems[i] = itemToAdd;
                return;
            }

            // if the item isn't stackable, or the item will over flow, we dont add to it and continue
            if(item.GetStackable() == false || item.GetQuantity() + itemToAdd.GetQuantity() > item.GetMaxStack()) {
                i++;
                continue;
            }

            // if the item is the same as the item we want to add we add their quantities together
            if(item.Equals(itemToAdd)) {
                hotBarItems[i].AddToQuantity(itemToAdd.GetQuantity());
                return;
            }

            i++;
        }

        i = 0;

        // Try to add item to inventory
        foreach(Item item in inventoryItems) {

            // if the item is blank we add it
            if(item == null) {
                inventoryItems[i] = itemToAdd;
                return;
            }

            // if the item isn't stackable, or the item will over flow, we dont add to it and continue
            if(item.GetStackable() == false || item.GetQuantity() + itemToAdd.GetQuantity() > item.GetMaxStack()) {
                i++;
                continue;
            }

            // if the item is the same as the item we want to add we add their quantities together
            if(item.Equals(itemToAdd)) {
                inventoryItems[i].AddToQuantity(itemToAdd.GetQuantity());
                return;
            }

            i++;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<DroppedItem>() != null && other.gameObject.GetComponent<DroppedItem>().canBePickedup) { // if the collider has a dropped item componnent
            AddItemToInventory(other.gameObject.GetComponent<DroppedItem>().item); // we add it to the inventory
            Destroy(other.gameObject);
        }
    }

    private IEnumerator EnablePickingUpAfterTime(float delay, DroppedItem item)
    {
        yield return new WaitForSeconds(delay);
        item.canBePickedup = true;
    }
}