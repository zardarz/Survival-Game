using UnityEngine;

[CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    [SerializeField] private CraftingIngredient[] CraftingIngredients;
    [SerializeField] private Item ResultingItem;
    [SerializeField] private int ResultingItemQuantity = 1;

    public CraftingIngredient[] craftingIngredients => CraftingIngredients;
    public Item resultingItem => ResultingItem;
    public int resultingItemQuantity => ResultingItemQuantity;
}
