using UnityEngine;
using UnityEditor;

public class FindBrokenRecipes : EditorWindow
{
    [MenuItem("Tools/Find Broken Crafting Recipes")]
    public static void FindBroken()
    {
        var recipeGuids = AssetDatabase.FindAssets("t:CraftingRecipe");
        int brokenCount = 0;

        foreach (var guid in recipeGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            CraftingRecipe recipe = AssetDatabase.LoadAssetAtPath<CraftingRecipe>(path);

            if (recipe == null)
            {
                Debug.LogError($"Could not load recipe at {path}");
                continue;
            }

            if (recipe.resultingItem == null)
            {
                Debug.LogError($"🚨 Missing resulting item in recipe: {path}");
                brokenCount++;
            }

            if (recipe.craftingIngredients == null)
            {
                Debug.LogError($"🚨 Missing ingredient array in recipe: {path}");
                brokenCount++;
                continue;
            }

            foreach (var ingredient in recipe.craftingIngredients)
            {
                if (ingredient == null)
                {
                    Debug.LogError($"🚨 Null ingredient slot in recipe: {path}");
                    brokenCount++;
                }
                else if (ingredient.craftingIngredientItem == null)
                {
                    Debug.LogError($"🚨 Missing item in ingredient in recipe: {path}");
                    brokenCount++;
                }
            }
        }

        Debug.Log($"✅ Scan complete. Found {brokenCount} broken references.");
    }
}
