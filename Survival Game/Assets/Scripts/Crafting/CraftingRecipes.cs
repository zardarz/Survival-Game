using UnityEngine;

public class CraftingRecipes : MonoBehaviour
{
    [SerializeField] private CraftingRecipe[] craftingRecipesEntries;

    public static CraftingRecipe[] craftingRecipes;

    void Awake()
    {
        if (craftingRecipesEntries == null || craftingRecipesEntries.Length == 0)
        {
            Debug.LogWarning("No crafting recipes assigned!");
        }

        craftingRecipes = craftingRecipesEntries;
    }
}