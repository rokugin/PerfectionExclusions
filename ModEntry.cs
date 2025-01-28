using HarmonyLib;
using PerfectionExclusions.Framework;
using PerfectionExclusions.Patches;
using StardewModdingAPI;
using StardewValley;

namespace PerfectionExclusions;

internal class ModEntry : Mod {

    public static string? MODID;

    public override void Entry(IModHelper helper) {
        MODID = ModManifest.UniqueID;

        helper.Events.Content.AssetRequested += static (_, e) => AssetManager.OnAssetRequested(e);
        helper.Events.Content.AssetsInvalidated += static (_, e) => AssetManager.OnAssetInvalidated(e);

        helper.ConsoleCommands.Add("rokugin.exclusions", "Print list of recipes needed for perfection.", PrintExclusions);

        var harmony = new Harmony(MODID);

        harmony.Patch(
            original: AccessTools.Method(typeof(Utility), nameof(Utility.getCookedRecipesPercent)),
            postfix: new HarmonyMethod(typeof(UtilityPatch), nameof(UtilityPatch.GetCookedRecipesPercent)));
        harmony.Patch(
            original: AccessTools.Method(typeof(Utility), nameof(Utility.getCraftedRecipesPercent)),
            postfix: new HarmonyMethod(typeof(UtilityPatch), nameof(UtilityPatch.GetCraftedRecipesPercent)));
    }

    private void PrintExclusions(string command, string[] args) {
        if (!Context.IsWorldReady) return;

        if (args.Length < 1) {
            Monitor.Log("\nrokugin.exclusions command missing arguments.\nExpected values:\ncooking\ncrafting\n", LogLevel.Error);
            return;
        }

        if (args[0] == "cooking") {
            Monitor.Log(BuildRecipeList(args[0]), LogLevel.Info);
            return;
        }

        if (args[0] == "crafting") {
            Monitor.Log(BuildRecipeList(args[0]), LogLevel.Info);
            return;
        }

        Monitor.Log($"{args[0]} not recognized as a valid argument.\nExpected values:\ncooking\ncrafting\n", LogLevel.Error);
    }

    string BuildRecipeList(string recipeTypes) {
        string s = "\n";
        List<string> recipesList = new();

        if (recipeTypes == "cooking") {
            foreach (var group in AssetManager.Recipes) {
                if (group.Value.CookingRecipes != null) recipesList.AddRange(group.Value.CookingRecipes);
            }

            Dictionary<string, string> recipes = new Dictionary<string, string>(CraftingRecipe.cookingRecipes);

            foreach (var v in recipes) {
                string recipeKey = v.Key;

                if (recipesList.Contains(recipeKey)) {
                    continue;
                }

                string recipe = ArgUtility.SplitBySpaceAndGet(ArgUtility.Get(v.Value.Split('/'), 2), 0);

                if (!Game1.player.recipesCooked.ContainsKey(recipe)) {
                    s += $"{recipeKey}\n";
                }
            }
        } else {
            foreach (var group in AssetManager.Recipes) {
                if (group.Value.CraftingRecipes != null) recipesList.AddRange(group.Value.CraftingRecipes);
            }

            Dictionary<string, string> recipes = new Dictionary<string, string>(CraftingRecipe.craftingRecipes);

            foreach (var v in recipes) {
                string recipeKey = v.Key;

                if (recipeKey == "Wedding Ring" || recipesList.Contains(recipeKey)) {
                    continue;
                }

                if (Game1.player.craftingRecipes.TryGetValue(recipeKey, out var timesCrafted) && timesCrafted > 0) {
                    continue;
                }

                s += $"{recipeKey}\n";
            }
        }

        return s;
    }
}