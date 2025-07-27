using System;
using UnityEngine;

namespace NutritionUI
{
    public class XUiC_NutritionStatBar : XUiController
    {
        private EntityPlayerLocal player;
        private float nextUpdateTime;

        // Formatters
        private readonly CachedStringFormatter<int> intFormatter = new CachedStringFormatter<int>(i => i.ToString());
        private readonly CachedStringFormatter<float> fillFormatter = new CachedStringFormatter<float>(f => f.ToString("F2"));

        public override void Init()
        {
            base.Init();
            player = xui?.playerUI?.entityPlayer;
        }

        public override void Update(float _dt)
        {
            if (viewComponent.IsVisible && Time.time > nextUpdateTime)
            {
                nextUpdateTime = Time.time + 0.25f;
                RefreshBindings(IsDirty);
                if (IsDirty) IsDirty = false;
            }

            base.Update(_dt);
        }

        public override bool GetBindingValue(ref string value, string bindingName)
        {
            if (player == null || player.Buffs == null)
            {
                if (IsNutritionBinding(bindingName))
                {
                    value = "";
                    return true;
                }
                return base.GetBindingValue(ref value, bindingName);
            }

            // Raw stat bindings
            if (bindingName == "playerprotein") return BindInt(ref value, player, "ProteinRaw");
            if (bindingName == "playercarbs") return BindInt(ref value, player, "CarbsRaw");
            if (bindingName == "playerfat") return BindInt(ref value, player, "FatRaw");
            if (bindingName == "playerdiet") return BindInt(ref value, player, "DietRaw");

            // Fill bar bindings
            if (bindingName == "playerproteinfill") return BindFill(ref value, player, "DisplayProteinNutrition");
            if (bindingName == "playercarbsfill") return BindFill(ref value, player, "DisplayCarbsNutrition");
            if (bindingName == "playerfatfill") return BindFill(ref value, player, "DisplayFatNutrition");
            if (bindingName == "playerdietfill") return BindFill(ref value, player, "DisplayDietNutrition");

            // Delta bindings
            if (bindingName == "playerproteinchange") return BindDelta(ref value, player, "ProteinRaw", "DisplayProteinNutritionMax");
            if (bindingName == "playercarbschange") return BindDelta(ref value, player, "CarbsRaw", "DisplayCarbsNutritionMax");
            if (bindingName == "playerfatchange") return BindDelta(ref value, player, "FatRaw", "DisplayFatNutritionMax");
            if (bindingName == "playerdietchange") return BindDelta(ref value, player, "DietRaw", "DisplayDietNutritionMax");

            return base.GetBindingValue(ref value, bindingName);
        }

        // --- Helper Methods ---

        private bool BindInt(ref string value, EntityPlayerLocal player, string varName)
        {
            value = intFormatter.Format((int)player.Buffs.GetCustomVar(varName));
            return true;
        }

        private bool BindFill(ref string value, EntityPlayerLocal player, string varName)
        {
            value = fillFormatter.Format(Mathf.Clamp01(player.Buffs.GetCustomVar(varName)));
            return true;
        }

        private bool BindDelta(ref string value, EntityPlayerLocal localPlayer, string rawVar, string maxVar, string healingVar = "", PassiveEffects blockageEffect = PassiveEffects.None)
        {
            try
            {
                if (localPlayer == null)
                {
                    value = "-";
                    return true;
                }

                var buffs = localPlayer.Buffs;
                float raw = buffs.GetCustomVar(rawVar);
                float max = buffs.GetCustomVar(maxVar);
                float healingAmount = !string.IsNullOrEmpty(healingVar) ? buffs.GetCustomVar(healingVar) : 0f;
                float blockage = 0f;

                if (blockageEffect != PassiveEffects.None)
                {
                    blockage = EffectManager.GetValue(
                        blockageEffect, null, 0f, localPlayer,
                        null, default(FastTags<TagGroup.Global>),
                        calcEquipment: true, calcHoldingItem: true,
                        calcProgression: true, calcBuffs: true,
                        calcChallenges: true, 1, useMods: true, _useDurability: true
                    );
                }

                int delta = (int)((double)(raw - max + healingAmount + blockage) - 0.9);

                if (delta == 0)
                {
                    value = " ";
                }
                else
                {
                    string sign = delta > 0 ? "+" : "";
                    value = $"({sign}{delta})";
                }

                return true;
            }
            catch
            {
                value = "-";
                return true;
            }
        }

        private static bool IsNutritionBinding(string name)
        {
            return name.StartsWith("playerprotein", StringComparison.OrdinalIgnoreCase)
                || name.StartsWith("playercarbs", StringComparison.OrdinalIgnoreCase)
                || name.StartsWith("playerfat", StringComparison.OrdinalIgnoreCase)
                || name.StartsWith("playerdiet", StringComparison.OrdinalIgnoreCase);
        }
    }
}
