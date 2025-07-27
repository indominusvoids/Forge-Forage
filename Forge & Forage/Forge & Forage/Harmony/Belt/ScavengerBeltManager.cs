using UnityEngine;
using System;
using System.Reflection;

public static class ScavengerBeltManager
{
    private static int extraSlots = 0;

    public static void UpdateExtraSlots(EntityPlayerLocal player)
    {
        try
        {
            if (player == null || player.equipment == null)
            {
                extraSlots = 0;
                Debug.Log("[ScavengerBeltManager] Player or equipment is null. Reset extraSlots to 0.");
                return;
            }

            int newExtra = 0;

            var slotsArray = player.equipment.GetItems();
            if (slotsArray == null)
            {
                Debug.LogError("[ScavengerBeltManager] ERROR: GetItems() returned null.");
                extraSlots = 0;
                return;
            }

            // Reflect the Mods field once
            var modsField = typeof(ItemValue).GetField("Modifications",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (modsField == null)
            {
                Debug.LogError("[ScavengerBeltManager] ERROR: 'Modifications' field not found on ItemValue!");
                extraSlots = 0;
                return;
            }

            // Iterate through equipment slots
            foreach (var item in slotsArray)
            {
                if (item == null || item.type == 0)
                    continue;

                int[] modsArray = modsField.GetValue(item) as int[];
                if (modsArray == null)
                    continue;

                foreach (var modId in modsArray)
                {
                    if (modId == 0) continue;

                    ItemClass modClass = null;
                    try
                    {
                        modClass = ItemClass.GetForId(modId);
                    }
                    catch (Exception exClass)
                    {
                        Debug.LogError($"[ScavengerBeltManager] ERROR getting ItemClass for modId {modId}: {exClass}");
                        continue;
                    }

                    if (modClass == null)
                        continue;

                    switch (modClass.Name)
                    {
                        case "modScavengersBeltUpgrade1": newExtra += 2; break;
                        case "modScavengersBeltUpgrade2": newExtra += 3; break;
                        case "modScavengersBeltUpgrade3": newExtra += 5; break;
                    }
                }
            }

            extraSlots = newExtra;
            Debug.Log($"[ScavengerBeltManager] extraSlots updated to {extraSlots}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"[ScavengerBeltManager] Unhandled exception in UpdateExtraSlots: {ex}");
        }
    }

    public static int GetExtraSlots()
    {
        return extraSlots;
    }
}