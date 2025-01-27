using PerfectionExclusions.Framework;
using StardewValley;

namespace PerfectionExclusions.Patches;

public static class UtilityPatch {

    public static void GetCookedRecipesPercent(ref float __result, Farmer? who = null) {
        if (who == null) {
            who = Game1.player;
        }

        List<string> recipesList = new();
        foreach (var group in AssetManager.Recipes) {
            if (group.Value.CookingRecipes != null) recipesList.AddRange(group.Value.CookingRecipes);
        }

        Dictionary<string, string> recipes = new Dictionary<string, string>(CraftingRecipe.cookingRecipes);
        float numberOfRecipesCooked = 0f;

        foreach (var v in recipes) {
            string recipeKey = v.Key;

            if (recipesList.Contains(recipeKey)) {
                recipes.Remove(recipeKey);
                continue;
            }

            if (who.cookingRecipes.ContainsKey(recipeKey)) {
                string recipe = ArgUtility.SplitBySpaceAndGet(ArgUtility.Get(v.Value.Split('/'), 2), 0);

                if (who.recipesCooked.ContainsKey(recipe)) {
                    numberOfRecipesCooked += 1f;
                }
            }
        }

        __result = numberOfRecipesCooked / (float)recipes.Count;
    }

    public static void GetCraftedRecipesPercent(ref float __result, Farmer? who = null) {
        if (who == null) {
            who = Game1.player;
        }

        List<string> recipesList = new();
        foreach (var group in AssetManager.Recipes) {
            if (group.Value.CraftingRecipes != null) recipesList.AddRange(group.Value.CraftingRecipes);
        }

        Dictionary<string, string> recipes = new Dictionary<string, string>(CraftingRecipe.craftingRecipes);
        float numberOfRecipesCrafted = 0f;

        foreach (var v in recipes) {
            string recipeKey = v.Key;

            if (recipeKey == "Wedding Ring" || recipesList.Contains(recipeKey)) {
                recipes.Remove(recipeKey);
                continue;
            }

            if (who.craftingRecipes.TryGetValue(recipeKey, out var timesCrafted) && timesCrafted > 0) {
                numberOfRecipesCrafted += 1f;
            }
        }

        __result = numberOfRecipesCrafted / ((float)recipes.Count);
    }

}