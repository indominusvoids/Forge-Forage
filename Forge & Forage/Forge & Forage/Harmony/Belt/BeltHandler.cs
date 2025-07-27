using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

[HarmonyPatch(typeof(Inventory), "get_PUBLIC_SLOTS_PLAYMODE")]
public static class DynamicPublicSlotsPlaymodePatch
{
    // MethodInfo for your helper
    private static readonly MethodInfo getExtraSlots = AccessTools.Method(typeof(ScavengerBeltManager), nameof(ScavengerBeltManager.GetExtraSlots));

    // Transpiler completely replaces original instructions
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        Debug.Log("[DynamicPublicSlotsPlaymodePatch] Replacing method body with custom instructions.");

        // build a fresh list of IL:
        var codes = new List<CodeInstruction>
        {
            // load constant 5 (baseSlots)
            new CodeInstruction(OpCodes.Ldc_I4, 5),
            // call ScavengerBeltManager.GetExtraSlots()
            new CodeInstruction(OpCodes.Call, getExtraSlots),
            // add them
            new CodeInstruction(OpCodes.Add),
            // return
            new CodeInstruction(OpCodes.Ret)
        };

        return codes.AsEnumerable();
    }
}

[HarmonyPatch(typeof(Inventory), "get_SHIFT_KEY_SLOT_OFFSET")]
public static class DynamicShiftKeySlotOffsetPatch
{
    private static readonly MethodInfo getExtraSlots = AccessTools.Method(typeof(ScavengerBeltManager), nameof(ScavengerBeltManager.GetExtraSlots));

    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        Debug.Log("[DynamicShiftKeySlotOffsetPatch] Replacing method body with custom instructions.");

        var codes = new List<CodeInstruction>
        {
            // load constant 5 (baseSlots)
            new CodeInstruction(OpCodes.Ldc_I4, 5),
            // call ScavengerBeltManager.GetExtraSlots()
            new CodeInstruction(OpCodes.Call, getExtraSlots),
            // add them
            new CodeInstruction(OpCodes.Add),
            // return
            new CodeInstruction(OpCodes.Ret)
        };

        return codes.AsEnumerable();
    }
}