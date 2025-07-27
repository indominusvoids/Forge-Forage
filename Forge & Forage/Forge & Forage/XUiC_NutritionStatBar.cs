using System;
using UnityEngine;

namespace NutritionUI
{
    public class XUiC_NutritionStatBar : XUiController
    {
        private EntityPlayerLocal player;
        private float nextUpdateTime;

        // Max values for delta calculations (fallback if no cvar found)
        private const int MAX_PROTEIN = 100;
        private const int MAX_CARBS = 100;
        private const int MAX_FAT = 100;
        private const int MAX_DIET = 100;

        private readonly CachedStringFormatter<int> intFormatter =
            new CachedStringFormatter<int>(i => i.ToString());

        private readonly CachedStringFormatter<float> fillFormatter =
            new CachedStringFormatter<float>(f => f.ToString("F2"));

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

            var buffs = player.Buffs;

            switch (bindingName)
            {
                // Raw values
                case "playerprotein":
                    value = intFormatter.Format((int)buffs.GetCustomVar("ProteinRaw"));
                    return true;

                case "playercarbs":
                    value = intFormatter.Format((int)buffs.GetCustomVar("CarbsRaw"));
                    return true;

                case "playerfat":
                    value = intFormatter.Format((int)buffs.GetCustomVar("FatRaw"));
                    return true;

                case "playerdiet":
                    value = intFormatter.Format((int)buffs.GetCustomVar("DietRaw"));
                    return true;

                // Fill values (0–1)
                case "playerproteinfill":
                    value = fillFormatter.Format(Mathf.Clamp01(buffs.GetCustomVar("DisplayProteinNutrition")));
                    return true;

                case "playercarbsfill":
                    value = fillFormatter.Format(Mathf.Clamp01(buffs.GetCustomVar("DisplayCarbsNutrition")));
                    return true;

                case "playerfatfill":
                    value = fillFormatter.Format(Mathf.Clamp01(buffs.GetCustomVar("DisplayFatNutrition")));
                    return true;

                case "playerdietfill":
                    value = fillFormatter.Format(Mathf.Clamp01(buffs.GetCustomVar("DisplayDietNutrition")));
                    return true;

                // Delta calculations
                case "playerproteinchange":
                    if (player != null)
                    {
                        int num = (int)((double)((float)player.Buffs.GetCustomVar("ProteinRaw") - (float)player.Buffs.GetCustomVar("DisplayProteinNutritionMax")) - 0.5);
                        if (num == 0)
                        {
                            value = " ";
                        }
                        else
                        {
                            string text = "";
                            if (num > 0)
                            {
                                text = "+";
                            }
                            value = "(" + text + num + ")";
                        }
                    }
                    else
                    {
                        value = "-";
                    }
                    return true;

                case "playercarbschange":
                    if (player != null)
                    {
                        int num = (int)((double)((float)player.Buffs.GetCustomVar("CarbsRaw") - (float)player.Buffs.GetCustomVar("DisplayCarbsNutritionMax")) - 0.5);
                        if (num == 0)
                        {
                            value = " ";
                        }
                        else
                        {
                            string text = "";
                            if (num > 0)
                            {
                                text = "+";
                            }
                            value = "(" + text + num + ")";
                        }
                    }
                    else
                    {
                        value = "-";
                    }
                    return true;

                case "playerfatchange":
                    if (player != null)
                    {
                        int num = (int)((double)((float)player.Buffs.GetCustomVar("FatRaw") - (float)player.Buffs.GetCustomVar("DisplayFatNutritionMax")) - 0.5);
                        if (num == 0)
                        {
                            value = " ";
                        }
                        else
                        {
                            string text = "";
                            if (num > 0)
                            {
                                text = "+";
                            }
                            value = "(" + text + num + ")";
                        }
                    }
                    else
                    {
                        value = "-";
                    }
                    return true;

                case "playerdietchange":
                    if (player != null)
                    {
                        int num = (int)((double)((float)player.Buffs.GetCustomVar("DietRaw") - (float)player.Buffs.GetCustomVar("DisplayDietNutritionMax")) - 0.5);
                        if (num == 0)
                        {
                            value = " ";
                        }
                        else
                        {
                            string text = "";
                            if (num > 0)
                            {
                                text = "+";
                            }
                            value = "(" + text + num + ")";
                        }
                    }
                    else
                    {
                        value = "-";
                    }
                    return true;


                default:
                    return base.GetBindingValue(ref value, bindingName);
            }
        }

        private static bool IsNutritionBinding(string name)
        {
            return
                name.StartsWith("playerprotein", StringComparison.OrdinalIgnoreCase)
             || name.StartsWith("playercarbs", StringComparison.OrdinalIgnoreCase)
             || name.StartsWith("playerfat", StringComparison.OrdinalIgnoreCase)
             || name.StartsWith("playerdiet", StringComparison.OrdinalIgnoreCase);
        }
    }
}
