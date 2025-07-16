using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Vector2 = UnityEngine.Vector2;
using System;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory/Hotbar Variables")]
    [SerializeField] private static Item[] inventoryItems;
    [SerializeField] private GameObject inventoryGO;
    
    [SerializeField] private  static Item[] hotBarItems;
    [SerializeField] private GameObject hotBarGO;


    [Header("Showing What Item is Selected")]
    [SerializeField] private int slotSelected = 1;
    private Item itemSelectedInHand;
    [SerializeField] private SpriteRenderer selectedItemInHandSpriteRenderer;
    [SerializeField] private Transform hand;

    // Inventory settings
    private Vector2Int inventorySize = new Vector2Int(3,6); // i know the x and y should be flipped but i realized too late and it breaks when you fix it
    private int totalSlots;
    private bool inventoryIsOpened;

    // Fire rate stuff
    private float nextTimeToFire;


    [Header("Inventory Swapping")]
    [SerializeField] private GameObject selectedItemWithCursorGO;
    private Image selectedItemWithCursorImage;
    private TMP_Text selectedItemWithCursorCount;
    private Item itemSelectedWithCursor;

    private bool canUseItem = true;

    [Header("Dropping Items")]
    [SerializeField] private GameObject droppedItemPrefab;
    [SerializeField] private float droppingItemStrength;

    [Header("Crafting UI Refrences")]

    [SerializeField] private GameObject craftingInterface;
    [SerializeField] private GameObject craftingInterfaceContent;
    [SerializeField] private GameObject craftingRecipeContentPrefab;
    [SerializeField] private Image selectedCraftingRecipeResultingItemImage;
    [SerializeField] private TMP_Text selectedCraftingRecipeResultingItemName;
    [SerializeField] private TMP_Text selectedCraftingRecipeResultingItemDescription;
    [SerializeField] private GameObject craftingSelectionGO;

    [SerializeField] private CraftingRecipe craftingRecipeForCraftingStation;

    // info for crafting
    private List<CraftingRecipe> craftableCraftingRecipes = new List<CraftingRecipe>();

    private CraftingRecipe selectedCraftingRecipe;
    private Vector2Int posOfOpenedWorkingStation = new Vector2Int(1234567890, 0);

    void Start() {
        totalSlots = inventorySize.x * inventorySize.y;

        inventoryItems = new Item[totalSlots];
        hotBarItems = new Item[inventorySize.y];

        selectedItemWithCursorImage = selectedItemWithCursorGO.GetComponent<Image>();
        selectedItemWithCursorCount = selectedItemWithCursorGO.transform.GetChild(0).GetComponent<TMP_Text>();
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

        CloseOpenedWorkStationIfNeeded();
    }

    private void HandleHotBarInputs() {
        if(Input.GetKeyDown(KeyCode.Alpha1)) {slotSelected = 1; nextTimeToFire = 0f;} // pressed one
        if(Input.GetKeyDown(KeyCode.Alpha2)) {slotSelected = 2; nextTimeToFire = 0f;} // pressed two
        if(Input.GetKeyDown(KeyCode.Alpha3)) {slotSelected = 3; nextTimeToFire = 0f;} // pressed three
        if(Input.GetKeyDown(KeyCode.Alpha4)) {slotSelected = 4; nextTimeToFire = 0f;} // pressed four
        if(Input.GetKeyDown(KeyCode.Alpha5)) {slotSelected = 5; nextTimeToFire = 0f;} // pressed five
        if(Input.GetKeyDown(KeyCode.Alpha6)) {slotSelected = 6; nextTimeToFire = 0f;} // pressed six

        if(itemSelectedInHand == null) {
            return;
        }

        if(canUseItem == false) {return;}

        Animator handSpriteAnimator = selectedItemInHandSpriteRenderer.gameObject.GetComponent<Animator>();

        if(Input.GetMouseButtonDown(0) && itemSelectedInHand is not Tool && itemSelectedInHand is not Weapon) {itemSelectedInHand.Use();}

        if(itemSelectedInHand is Sword) {
            handSpriteAnimator.SetBool("isUsingSword", true);
        } else {
            handSpriteAnimator.SetBool("isUsingSword", false);
        }

        if(itemSelectedInHand is Tool) {
            handSpriteAnimator.SetBool("isUsingTool", true);
        } else {
            handSpriteAnimator.SetBool("isUsingTool", false);
        }

        if(itemSelectedInHand is not Tool and not Sword) {
            handSpriteAnimator.SetBool("isIdle", true);
        } else {
            handSpriteAnimator.SetBool("isIdle", false);
        }

        if(Input.GetMouseButton(0) == false) return;

        if(itemSelectedInHand is Tool && Time.time >= nextTimeToFire) {
            Tool toolSelectedInHand = itemSelectedInHand as Tool;
            nextTimeToFire = Time.time + 1f / toolSelectedInHand.GetToolSpeed();

            handSpriteAnimator.speed = toolSelectedInHand.GetToolSpeed();
            toolSelectedInHand.Use();
        }

        if(itemSelectedInHand is Weapon && Time.time >= nextTimeToFire) {
            Weapon weaponSelectedInHand = itemSelectedInHand as Weapon;
            nextTimeToFire = Time.time + 1f / weaponSelectedInHand.GetWeaponSpeed();

            handSpriteAnimator.speed = weaponSelectedInHand.GetWeaponSpeed();
            weaponSelectedInHand.Use();
        }
    }

    private void LoopSelectedSlot() {
        if(slotSelected < 1) {slotSelected = inventorySize.y;} // make the slot selected loop
        if(slotSelected > inventorySize.y) {slotSelected = 1;}
    }

    private void SetItemSelected() {
        if(hotBarItems[slotSelected - 1] == null) { 
            itemSelectedInHand = null;
            HideSelectedItem();
            } else {
            itemSelectedInHand = hotBarItems[slotSelected - 1]; // get the selected item
        }
    }

    private void TryToShowSelectedItem() {
        if(itemSelectedInHand == null || inventoryIsOpened) {return;}

        if(Input.GetMouseButton(0) || itemSelectedInHand.GetShowWhenHolding()) { // if the mouse button is down or the item should be shown we show it
            ShowSelectedItem();
        } else { // else we hide it
            HideSelectedItem();
        }
    }

    private void ShowSelectedItem() {
        selectedItemInHandSpriteRenderer.sprite = itemSelectedInHand.GetSprite();
    }

    private void HideSelectedItem() {
        selectedItemInHandSpriteRenderer.sprite = null;
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
        DroppedItem newDroppedItemDroppedItemComponent = newDroppedItem.GetComponent<DroppedItem>();

        newDroppedItem.transform.position = selectedItemInHandSpriteRenderer.transform.position; // set its position to the player's hand position

        Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized; // find the direction of the mouse relative to the player

        newDroppedItemDroppedItemComponent.AddForceOnDrop(droppingItemStrength, dir); // add some force to the dropped item
        newDroppedItemDroppedItemComponent.AddRandomTorque(30f); // add to spinyness to it 

        newDroppedItemDroppedItemComponent.item = Instantiate(itemSelectedInHand); // set the item on the dropped item to the corresponding item
        newDroppedItemDroppedItemComponent.item.SetQuantity(1);

        newDroppedItemDroppedItemComponent.ChangeSprite(); // set the dropped item sprite to the item sprite
        newDroppedItemDroppedItemComponent.StartCotrotean(0.5f); // enable picking up after .5 seconds

        itemSelectedInHand.AddToQuantity(-1); // drop the item quantity by one
    }

    private void ToggleInventory() {
        // we are using the parent of the inventory gameobject because i set the parent to the background which also needs to be disactivated 

        // if the parent is active we set it to unactive and visersa
        if(inventoryGO.transform.parent.gameObject.activeSelf) {
            inventoryGO.transform.parent.gameObject.SetActive(false);
            canUseItem = true;
            inventoryIsOpened = false;
        } else {
            inventoryGO.transform.parent.gameObject.SetActive(true);
            canUseItem = false;
            inventoryIsOpened = true;
        }
    }

    public void ToggleCraftingInterface() {

        if(craftingInterface.activeSelf) {
            craftingInterface.SetActive(false);
            craftingSelectionGO.SetActive(false);
            canUseItem = true;
        } else {
            CalculateCraftableCraftingRecipes();
            AddToCraftingInterfaceContent();
            craftingInterface.SetActive(true);
            canUseItem = false;
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        posOfOpenedWorkingStation = new Vector2Int((int) Mathf.Round(mousePos.x), (int) Mathf.Round(mousePos.y));
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

    public static void AddItemToInventory(Item itemToAdd) {
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
            AddItemToInventory(Instantiate(other.gameObject.GetComponent<DroppedItem>().item)); // we add it to the inventory
            Destroy(other.gameObject);
        }
    }

    private void CalculateCraftableCraftingRecipes() {
        craftableCraftingRecipes.Clear();

        CraftingRecipe[] allCraftingRecipes = CraftingRecipes.craftingRecipes;

        for(int i = 0; i < allCraftingRecipes.Length; i++) {
            CraftingRecipe craftingRecipe = Instantiate(allCraftingRecipes[i]);
            CraftingIngredient[] craftingIngredients = craftingRecipe.craftingIngredients;

            bool canCraftCraftable = true;

            for(int craftingIngredientIndex = 0; craftingIngredientIndex < craftingIngredients.Length; craftingIngredientIndex++) {
                CraftingIngredient craftingIngredient = craftingIngredients[craftingIngredientIndex];

                if(!DoesPlayerHaveItem(craftingIngredient.craftingIngredientItem, (int) craftingIngredient.craftingIngredientQuantity)) {
                    canCraftCraftable = false;
                    break;
                }
            }

            if(canCraftCraftable) {
                craftableCraftingRecipes.Add(craftingRecipe);
            }
        }
    }

    private bool DoesPlayerHaveItem(Item itemToFind, int itemQuanity) {

        if(itemToFind == null) {
            Debug.Log("item to find is null");
            return false;
        }

        int totalAmountOfItemInventory = 0;
        
        for(int i = 0; i < hotBarItems.Length; i++) {
            Item currentItem = hotBarItems[i];

            if(currentItem == null) continue;

            if(currentItem.Equals(itemToFind)) {
                totalAmountOfItemInventory += currentItem.GetQuantity();
            }
        }

        for(int i = 0; i < inventoryItems.Length; i++) {
            Item currentItem = inventoryItems[i];

            if(currentItem == null) continue;

            if(currentItem.Equals(itemToFind)) {
                totalAmountOfItemInventory += currentItem.GetQuantity();
            }
        }

        if(totalAmountOfItemInventory >= itemQuanity) {
            return true;
        }

        return false;
    }

    private void AddToCraftingInterfaceContent() {

        for(int i = 0; i < craftingInterfaceContent.transform.childCount; i++) {
            Destroy(craftingInterfaceContent.transform.GetChild(i).gameObject);
        }

        for(int i = 0; i < craftableCraftingRecipes.Count; i++) {
            GameObject newContent = Instantiate(craftingRecipeContentPrefab, craftingInterfaceContent.GetComponent<RectTransform>());

            newContent.GetComponent<Image>().sprite = craftableCraftingRecipes[i].resultingItem.GetSprite();

            newContent.GetComponent<SelectionForCraftingRecipe>().craftingRecipeOnCraftingOption = craftableCraftingRecipes[i];
            
            newContent.GetComponent<Button>().onClick.AddListener(ChangeSelectedCraftingRecipe);
        }
    }

    public void Craft() {
        if(selectedCraftingRecipe == null || IsThereSpaceInInventory() == false) {
            return;
        }

        for(int i = 0; i < selectedCraftingRecipe.craftingIngredients.Length; i++) {
            CraftingIngredient craftingIngredient = selectedCraftingRecipe.craftingIngredients[i];

            if(DoesPlayerHaveItem(craftingIngredient.craftingIngredientItem, (int) craftingIngredient.craftingIngredientQuantity)) {
                RemoveItemFromInventory(craftingIngredient.craftingIngredientItem, (int) craftingIngredient.craftingIngredientQuantity);
                CalculateCraftableCraftingRecipes();
                AddToCraftingInterfaceContent();
            } else {
                selectedCraftingRecipe = null;
                craftingSelectionGO.SetActive(false);
                return;
            }

        }

        for(int i = 0; i < selectedCraftingRecipe.resultingItemQuantity; i++) {
            AddItemToInventory(selectedCraftingRecipe.resultingItem);
        }

        CalculateCraftableCraftingRecipes();
    }

    public void CraftCraftingStation() {
        if(IsThereSpaceInInventory() == false) {
            return;
        }

        for(int i = 0; i < craftingRecipeForCraftingStation.craftingIngredients.Length; i++) {
            CraftingIngredient craftingIngredient = craftingRecipeForCraftingStation.craftingIngredients[i];

            if(DoesPlayerHaveItem(craftingIngredient.craftingIngredientItem, (int) craftingIngredient.craftingIngredientQuantity)) {
                RemoveItemFromInventory(craftingIngredient.craftingIngredientItem, (int) craftingIngredient.craftingIngredientQuantity);
                CalculateCraftableCraftingRecipes();
                AddToCraftingInterfaceContent();
            } else {
                return;
            }

        }

        for(int i = 0; i < craftingRecipeForCraftingStation.resultingItemQuantity; i++) {
            AddItemToInventory(Instantiate(craftingRecipeForCraftingStation.resultingItem));
        }

        CalculateCraftableCraftingRecipes();
    }

    private void RemoveItemFromInventory(Item itemToRemove, int quantityToRemove) {
        int quantityLeftToRemove = quantityToRemove;

        for(int i = 0; i < inventoryItems.Length; i++) {
            Item currentItem = inventoryItems[i];

            if(currentItem == null || !currentItem.Equals(itemToRemove)) continue;

            if(currentItem.GetQuantity() >= quantityLeftToRemove) {
                currentItem.AddToQuantity(-quantityLeftToRemove);
                quantityLeftToRemove = 0;
            } else {
                quantityLeftToRemove -= currentItem.GetQuantity();
                currentItem.SetQuantity(0);
            }
        }


        for(int i = 0; i < hotBarItems.Length; i++) {
            Item currentItem = hotBarItems[i];
            
            if(currentItem == null || !currentItem.Equals(itemToRemove)) continue;

            if(currentItem.GetQuantity() >= quantityLeftToRemove) {
                currentItem.AddToQuantity(-quantityLeftToRemove);
                quantityLeftToRemove = 0;
            } else {
                quantityLeftToRemove -= currentItem.GetQuantity();
                currentItem.SetQuantity(0);
            }
        }
    }

    public void ChangeSelectedCraftingRecipe() {
        GameObject caller = EventSystem.current.currentSelectedGameObject;

        selectedCraftingRecipe = caller.GetComponent<SelectionForCraftingRecipe>().craftingRecipeOnCraftingOption;

        selectedCraftingRecipeResultingItemImage.sprite = selectedCraftingRecipe.resultingItem.GetSprite();

        selectedCraftingRecipeResultingItemName.text = selectedCraftingRecipe.resultingItem.GetName();
        
        selectedCraftingRecipeResultingItemDescription.text = selectedCraftingRecipe.resultingItem.GetDescription();

        craftingSelectionGO.SetActive(true);
    }

    private bool IsThereSpaceInInventory() {
        foreach(Item item in hotBarItems) {
            if(item == null) {
                return true;
            }
        }


        foreach(Item item in inventoryItems) {
            if(item == null) {
                return true;
            }
        }


        return false;
    }

    private void CloseOpenedWorkStationIfNeeded() {
        if(posOfOpenedWorkingStation.x == 1234567890) return;

        if(Vector2.Distance(transform.position, (Vector2) posOfOpenedWorkingStation) > 2f) {
            ToggleCraftingInterface();
            posOfOpenedWorkingStation.x = 1234567890;
        }
    }
}