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

        var harmony = new Harmony(MODID);

        harmony.Patch(
            original: AccessTools.Method(typeof(Utility), nameof(Utility.getCookedRecipesPercent)),
            postfix: new HarmonyMethod(typeof(UtilityPatch), nameof(UtilityPatch.GetCookedRecipesPercent)));
        harmony.Patch(
            original: AccessTools.Method(typeof(Utility), nameof(Utility.getCraftedRecipesPercent)),
            postfix: new HarmonyMethod(typeof(UtilityPatch), nameof(UtilityPatch.GetCraftedRecipesPercent)));
    }

}