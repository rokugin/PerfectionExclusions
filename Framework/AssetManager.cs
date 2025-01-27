using PerfectionExclusions.Models;
using StardewModdingAPI.Events;
using StardewValley;

namespace PerfectionExclusions.Framework;

internal static class AssetManager {

    static string? MODID => ModEntry.MODID;

    static Dictionary<string, RecipeExclusions>? recipes = null;

    public static Dictionary<string, RecipeExclusions> Recipes {
        get
        {
            if (recipes == null) {
                recipes = Game1.content.Load<Dictionary<string, RecipeExclusions>>($"{MODID}/recipes");
            }
            return recipes;
        }
    }

    internal static void OnAssetRequested(AssetRequestedEventArgs e) {
        if (e.NameWithoutLocale.IsEquivalentTo($"{MODID}/recipes")) {
            e.LoadFrom(() => new Dictionary<string, RecipeExclusions>(), AssetLoadPriority.Exclusive);
            //e.Edit(asset => {
            //    var data = asset.AsDictionary<string, RecipeExclusions>();
            //    data.Data.Add($"{MODID}_Example", new RecipeExclusions());
            //    var cooking = CraftingRecipe.cookingRecipes;
            //    var crafting = CraftingRecipe.craftingRecipes;

            //    foreach (var recipe in cooking) {
            //        if (recipe.Key == "Fried Egg") continue;
            //        data.Data[$"{MODID}_Example"].CookingRecipes.Add(recipe.Key);
            //    }

            //    foreach (var recipe in crafting) {
            //        if (recipe.Key == "Wood Fence") continue;
            //        data.Data[$"{MODID}_Example"].CraftingRecipes.Add(recipe.Key);
            //    }
            //});
        }
    }

    internal static void OnAssetInvalidated(AssetsInvalidatedEventArgs e) {
        foreach (var name in e.NamesWithoutLocale) {
            if (name.IsEquivalentTo($"{MODID}/recipes")) {
                recipes = null;
            }
        }
    }

}