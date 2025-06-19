using UnityEngine;

[System.Serializable]
public class CraftingIngredient
{
    [SerializeField] private Item CraftingIngredientItem;
    [SerializeField] private uint CraftingIngredientQuantity;

    public Item craftingIngredientItem => craftingIngredientItem;
    public uint craftingIngredientQuantity => craftingIngredientQuantity;
}