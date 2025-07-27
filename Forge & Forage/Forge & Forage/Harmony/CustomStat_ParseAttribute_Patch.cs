using HarmonyLib;

[HarmonyPatch(typeof(XUiC_HUDStatBar), "ParseAttribute")]
public static class CustomStat_ParseAttribute_Patch
{
    [HarmonyPrefix]
    public static bool Prefix(ref bool __result, XUiC_HUDStatBar __instance, string name, string value, XUiController _parent)
    {
        if (name == "stat_type")
        {
            switch (value.ToLowerInvariant())
            {
                case "protein":
                case "9":
                    __instance.StatType = (HUDStatTypes)9;
                    __result = true;
                    return false;
                case "carbs":
                case "10":
                    __instance.StatType = (HUDStatTypes)10;
                    __result = true;
                    return false;
                case "fat":
                case "11":
                    __instance.StatType = (HUDStatTypes)11;
                    __result = true;
                    return false;
                case "diet":
                case "12":
                    __instance.StatType = (HUDStatTypes)12;
                    __result = true;
                    return false;
            }
        }

        // let the original ParseAttribute handle other cases
        return true;
    }
}
