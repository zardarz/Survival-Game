using UnityEngine;

[System.Serializable]
public class CraftingIngredient
{
    [SerializeField] private Item CraftingIngredientItem;
    [SerializeField] private uint CraftingIngredientQuantity;

    public Item craftingIngredientItem => CraftingIngredientItem;
    public uint craftingIngredientQuantity => CraftingIngredientQuantity;
}