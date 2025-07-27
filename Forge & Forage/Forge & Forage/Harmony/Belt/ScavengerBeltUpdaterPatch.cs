using HarmonyLib;
using UnityEngine;

[HarmonyPatch(typeof(EntityPlayerLocal), "callInventoryChanged")]
public static class ScavengerBeltUpdaterPatch
{
    [HarmonyPostfix]
    static void Postfix(EntityPlayerLocal __instance)
    {
        try
        {
            if (__instance == null)
                return;

            // Update slot count based on equipment
            ScavengerBeltManager.UpdateExtraSlots(__instance);

            // In most cases you do NOT need to force a private method call
            if (__instance.inventory == null)
            {
                Debug.LogWarning("[ScavengerBeltUpdaterPatch] Inventory is null, skipping refresh.");
                return;
            }

            Debug.Log("[ScavengerBeltUpdaterPatch] Extra slots updated, waiting for normal refresh.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[ScavengerBeltUpdaterPatch] Exception in Postfix: {ex}");
        }
    }
}
