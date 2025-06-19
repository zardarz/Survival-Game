using UnityEngine;

[CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    [SerializeField] private CraftingIngredient[] craftingIngredients;
    [SerializeField] private Item resultingItem;
    [SerializeField] private int resultingItemQuantity = 1;

    public CraftingIngredient[] CraftingIngredients => craftingIngredients;
    public Item ResultingItem => resultingItem;
    public int ResultingItemQuantity => resultingItemQuantity;
}
