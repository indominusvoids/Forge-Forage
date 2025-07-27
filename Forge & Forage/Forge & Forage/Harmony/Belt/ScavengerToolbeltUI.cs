using UnityEngine;

public static class ScavengerToolbeltUI
{
    public static void RefreshToolbeltUI(EntityPlayerLocal player)
    {
        if (player == null)
        {
            Debug.Log("[ScavengerToolbeltUI] Player is null.");
            return;
        }

        LocalPlayerUI ui = LocalPlayerUI.GetUIForPlayer(player);
        if (ui == null)
        {
            Debug.Log("[ScavengerToolbeltUI] LocalPlayerUI is null.");
            return;
        }

        // Find the toolbelt window group
        XUiController group = ui.xui.FindWindowGroupByName("toolbelt");
        if (group == null)
        {
            Debug.Log("[ScavengerToolbeltUI] toolbelt window group not found.");
            return;
        }

        // Get the toolbelt window
        XUiC_Toolbelt toolbelt =
            (group.GetChildById("windowToolbelt") as XUiC_ToolbeltWindow)?.GetChildByType<XUiC_Toolbelt>();
        if (toolbelt == null)
        {
            Debug.Log("[ScavengerToolbeltUI] windowToolbelt or XUiC_Toolbelt not found.");
            return;
        }

        // Get the slots controller
        XUiController slotsController = toolbelt.GetChildById("toolbeltSlots");
        if (!(slotsController?.ViewComponent is XUiV_Grid grid))
        {
            Debug.Log("[ScavengerToolbeltUI] toolbeltSlots grid not found.");
            return;
        }

        // Get the slot components
        XUiC_ItemStack[] children = slotsController.GetChildrenByType<XUiC_ItemStack>();
        if (children == null || children.Length == 0)
        {
            Debug.LogWarning("[ScavengerToolbeltUI] No slot components found.");
            return;
        }

        // The maximum number of slots defined in XML (e.g., 10)
        int maxSlots = children.Length;

        // LEFT SIDE = first 5 slots always visible
        // RIGHT SIDE = extra slots based on upgrades
        int leftCount = 5;
        int unlockedRightSlots = ScavengerBeltManager.GetExtraSlots();
        if (unlockedRightSlots < 0) unlockedRightSlots = 0;
        if (unlockedRightSlots > 5) unlockedRightSlots = 5; // clamp

        Vector3 shown = Vector3.one;
        Vector3 hidden = Vector3.zero;

        int idx = -1;

        // ----- LEFT SIDE -----
        for (int i = 0; i < leftCount; i++)
        {
            idx++;
            if (idx >= children.Length) break;

            children[idx].ViewComponent.UiTransform.name = "A" + i;
            children[idx].ViewComponent.UiTransform.localScale = shown; // left slots always visible
        }

        // ----- RIGHT SIDE -----
        for (int j = 0; j < 5; j++)
        {
            idx++;
            if (idx >= children.Length) break;

            bool show = j < unlockedRightSlots;
            children[idx].ViewComponent.UiTransform.name = "C" + j;
            children[idx].ViewComponent.UiTransform.localScale = show ? shown : hidden;
        }

        // Update UI
        grid.Update(0f);
        toolbelt.Update(0f);

        Debug.Log($"[ScavengerToolbeltUI] Toolbelt UI refreshed. Right-side slots unlocked: {unlockedRightSlots}");
    }
}
