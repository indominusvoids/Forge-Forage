using HarmonyLib;
using UnityEngine;
using System.Reflection;

// Enum for your custom stats
public enum CustomHUDStatTypes
{
    Protein = 9,
    Carbs = 10,
    Fat = 11,
    Diet = 12,
}

// Patch to set icons and images for custom stats
[HarmonyPatch(typeof(XUiC_HUDStatBar), "SetStatValues")]
[HarmonyPriority(Priority.Last)]
public static class SetStatValuesPatch
{
    static void Postfix(XUiC_HUDStatBar __instance)
    {
        // Get private field 'statType' with reflection
        FieldInfo statTypeField = typeof(XUiC_HUDStatBar).GetField("statType", BindingFlags.NonPublic | BindingFlags.Instance);
        if (statTypeField == null) return;

        int statTypeInt = (int)statTypeField.GetValue(__instance);

        switch (statTypeInt)
        {
            case (int)CustomHUDStatTypes.Protein:
                __instance.statImage = "ui_game_stat_bar_health";  // Replace with your desired bar sprite
                __instance.statIcon = "ui_game_stat_protein_icon"; // Replace with your protein icon sprite
                __instance.statGroup = HUDStatGroups.Player;
                break;

            case (int)CustomHUDStatTypes.Carbs:
                __instance.statImage = "ui_game_stat_bar_health";
                __instance.statIcon = "ui_game_stat_carbs_icon";
                __instance.statGroup = HUDStatGroups.Player;
                break;

            case (int)CustomHUDStatTypes.Fat:
                __instance.statImage = "ui_game_stat_bar_health";
                __instance.statIcon = "ui_game_stat_fat_icon";
                __instance.statGroup = HUDStatGroups.Player;
                break;

            case (int)CustomHUDStatTypes.Diet:
                __instance.statImage = "ui_game_stat_bar_health";
                __instance.statIcon = "ui_game_stat_diet_icon";
                __instance.statGroup = HUDStatGroups.Player;
                break;
        }
    }
}