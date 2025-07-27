using HarmonyLib;
using UnityEngine;

// Patch to provide values for labels and fill amounts
[HarmonyPatch(typeof(XUiC_HUDStatBar), "GetBindingValue")]
public static class NutritionHUDStatBarPatch
{
    [HarmonyPrefix]
    public static bool Prefix(
        string bindingName,
        ref string value,
        ref bool __result,
        XUiC_HUDStatBar __instance,
        HUDStatTypes ___statType
    )
    {
        var player = __instance?.xui?.playerUI?.entityPlayer;
        if (player == null)
        {
            // If no player, return "-" for known bindings to avoid errors
            if (IsHandledBinding(bindingName, ___statType))
            {
                value = "-";
                __result = true;
                return false; // skip original method
            }
            return true; // default behavior
        }

        // Cast to your custom enum for clarity
        var statType = (CustomHUDStatTypes)___statType;

        switch (bindingName)
        {
            case "statcurrent":
                switch (statType)
                {
                    case CustomHUDStatTypes.Protein:
                        value = ((int)player.Buffs.GetCustomVar("ProteinRaw")).ToString();
                        __result = true; return false;
                    case CustomHUDStatTypes.Carbs:
                        value = ((int)player.Buffs.GetCustomVar("CarbsRaw")).ToString();
                        __result = true; return false;
                    case CustomHUDStatTypes.Fat:
                        value = ((int)player.Buffs.GetCustomVar("FatRaw")).ToString();
                        __result = true; return false;
                    case CustomHUDStatTypes.Diet:
                        value = ((int)player.Buffs.GetCustomVar("DisplayNutrition")).ToString();
                        __result = true; return false;
                }
                break;

            case "statfill":
                switch (statType)
                {
                    case CustomHUDStatTypes.Protein:
                        value = Mathf.Clamp01(player.Buffs.GetCustomVar("DisplayProteinNutrition")).ToString("F2");
                        __result = true; return false;
                    case CustomHUDStatTypes.Carbs:
                        value = Mathf.Clamp01(player.Buffs.GetCustomVar("DisplayCarbsNutrition")).ToString("F2");
                        __result = true; return false;
                    case CustomHUDStatTypes.Fat:
                        value = Mathf.Clamp01(player.Buffs.GetCustomVar("DisplayFatNutrition")).ToString("F2");
                        __result = true; return false;
                    case CustomHUDStatTypes.Diet:
                        value = Mathf.Clamp01(player.Buffs.GetCustomVar("DisplayNutrition")).ToString("F2");
                        __result = true; return false;
                }
                break;

            // Custom delta labels for protein, carbs, fat, diet changes
            case "playerproteinchange":
                {
                    float cur = player.Buffs.GetCustomVar("ProteinRaw");
                    float max = player.Buffs.GetCustomVar("DisplayProteinNutritionMax");
                    int delta = Mathf.RoundToInt(cur - max);
                    value = FormatDelta(delta);
                    __result = true;
                    return false;
                }
            case "playercarbschange":
                {
                    float cur = player.Buffs.GetCustomVar("CarbsRaw");
                    float max = player.Buffs.GetCustomVar("DisplayCarbsNutritionMax");
                    int delta = Mathf.RoundToInt(cur - max);
                    value = FormatDelta(delta);
                    __result = true;
                    return false;
                }
            case "playerfatchange":
                {
                    float cur = player.Buffs.GetCustomVar("FatRaw");
                    float max = player.Buffs.GetCustomVar("DisplayFatNutritionMax");
                    int delta = Mathf.RoundToInt(cur - max);
                    value = FormatDelta(delta);
                    __result = true;
                    return false;
                }
            case "playerdietchange":
                {
                    float cur = player.Buffs.GetCustomVar("DisplayNutrition");
                    float max = player.Buffs.GetCustomVar("DisplayNutritionMax");
                    int delta = Mathf.RoundToInt(cur - max);
                    value = FormatDelta(delta);
                    __result = true;
                    return false;
                }

            // Raw values for other player bindings
            case "playerprotein":
                value = ((int)player.Buffs.GetCustomVar("ProteinRaw")).ToString();
                __result = true; return false;
            case "playercarbs":
                value = ((int)player.Buffs.GetCustomVar("CarbsRaw")).ToString();
                __result = true; return false;
            case "playerfat":
                value = ((int)player.Buffs.GetCustomVar("FatRaw")).ToString();
                __result = true; return false;
            case "playerdiet":
                value = ((int)player.Buffs.GetCustomVar("DisplayNutrition")).ToString();
                __result = true; return false;
        }

        // Not handled, let original method run
        return true;
    }

    private static bool IsHandledBinding(string bindingName, HUDStatTypes statType)
    {
        return bindingName == "playerprotein" ||
               bindingName == "playerproteinchange" ||
               bindingName == "playercarbs" ||
               bindingName == "playercarbschange" ||
               bindingName == "playerfat" ||
               bindingName == "playerfatchange" ||
               bindingName == "playerdiet" ||
               bindingName == "playerdietchange" ||
               (bindingName == "statcurrent" &&
                (statType == (HUDStatTypes)CustomHUDStatTypes.Protein ||
                 statType == (HUDStatTypes)CustomHUDStatTypes.Carbs ||
                 statType == (HUDStatTypes)CustomHUDStatTypes.Fat ||
                 statType == (HUDStatTypes)CustomHUDStatTypes.Diet)) ||
               (bindingName == "statfill" &&
                (statType == (HUDStatTypes)CustomHUDStatTypes.Protein ||
                 statType == (HUDStatTypes)CustomHUDStatTypes.Carbs ||
                 statType == (HUDStatTypes)CustomHUDStatTypes.Fat ||
                 statType == (HUDStatTypes)CustomHUDStatTypes.Diet));
    }

    private static string FormatDelta(int delta)
    {
        if (delta == 0) return " ";
        return delta > 0 ? "+" + delta : delta.ToString();
    }
}