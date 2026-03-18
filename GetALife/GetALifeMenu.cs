using Kitchen;
using Kitchen.Modules;
using KitchenData;
using KitchenLib;
using KitchenLib.Preferences;
using KitchenLib.UI.PlateUp;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedundantSemicolonMods.GetALife
{
    public class GetALifeMenu : KLMenu
    {
        private Option<int> rarityOption;
        private Option<int> priceOption;

        private readonly List<int> rarityValues = new List<int>
        {
            (int)RarityTier.Common,
            (int)RarityTier.Uncommon,
            (int)RarityTier.Rare,
            (int)RarityTier.Special
        };

        private readonly List<int> priceValues = new List<int>
        {
            (int)PriceTier.Free,
            (int)PriceTier.VeryCheap,
            (int)PriceTier.Cheap,
            (int)PriceTier.MediumCheap,
            (int)PriceTier.Medium,
            (int)PriceTier.Expensive,
            (int)PriceTier.VeryExpensive,
            (int)PriceTier.ExtremelyExpensive
        };

        private readonly List<string> rarityLabels = new List<string> { "Common", "Uncommon", "Rare", "Special" };
        private readonly List<string> priceLabelsPlain = new List<string>
        {
            "Free", "Very Cheap", "Cheap", "Medium Cheap", "Medium", "Expensive", "Very Expensive", "Extremely Expensive"
        };

        public GetALifeMenu(Transform container, ModuleList module_list) : base(container, module_list)
        {
            try
            {
                GetALifeMain.Logger.LogInfo("[GetALifeMenu] Constructor");
            }
            catch (Exception e)
            {
                GetALifeMain.Logger.LogError($"[GetALifeMenu] Constructor exception: {e}");
            }
        }

        public override void Setup(int player_id)
        {
            try
            {
                GetALifeMain.Logger.LogInfo("[GetALifeMenu] Setup start");
                ModuleList.Clear();

                AddLabel("Get A Life Settings");

                // Rarity option
                AddInfo("Item Frequency");
                int rarityValue = SafeGetPrefValue(GetALifeMain.RARITY_TIER_ID, (int)RarityTier.Special, rarityValues);
                rarityOption = new Option<int>(rarityValues, rarityValue, rarityLabels);
                AddSelect(rarityOption);
                rarityOption.OnChanged += (s, value) =>
                {
                    try
                    {
                        var pref = GetALifeMain.Prefs.GetPreference<PreferenceInt>(GetALifeMain.RARITY_TIER_ID);
                        if (pref != null)
                        {
                            pref.Set(value);
                            GetALifeMain.Prefs.Save();
                            GetALifeMain.Logger.LogInfo($"[GetALifeMenu] Saved rarity preference = {value}");
                        }
                        else
                        {
                            GetALifeMain.Logger.LogError("[GetALifeMenu] Failed to save rarity preference: preference object null.");
                        }
                    }
                    catch (Exception e)
                    {
                        GetALifeMain.Logger.LogError($"[GetALifeMenu] Error saving rarity preference: {e}");
                    }
                };

                New<SpacerElement>(true);

                // Price option (plain tier labels, no gold amounts)
                AddInfo("Item Price");
                int priceValue = SafeGetPrefValue(GetALifeMain.PRICE_TIER_ID, (int)PriceTier.Expensive, priceValues);
                priceOption = new Option<int>(priceValues, priceValue, priceLabelsPlain);
                AddSelect(priceOption);
                priceOption.OnChanged += (s, value) =>
                {
                    try
                    {
                        var pref = GetALifeMain.Prefs.GetPreference<PreferenceInt>(GetALifeMain.PRICE_TIER_ID);
                        if (pref != null)
                        {
                            pref.Set(value);
                            GetALifeMain.Prefs.Save();
                            GetALifeMain.Logger.LogInfo($"[GetALifeMenu] Saved price preference = {(PriceTier)value}");
                        }
                        else
                        {
                            GetALifeMain.Logger.LogError("[GetALifeMenu] Failed to save price preference: preference object null.");
                        }
                    }
                    catch (Exception e)
                    {
                        GetALifeMain.Logger.LogError($"[GetALifeMenu] Error saving price preference: {e}");
                    }
                };

                New<SpacerElement>(true);
                AddButton("Back", (_) => RequestPreviousMenu());

                // finalize layout
                ResetPanel();

                GetALifeMain.Logger.LogInfo("[GetALifeMenu] Setup complete");
            }
            catch (Exception e)
            {
                GetALifeMain.Logger.LogError($"[GetALifeMenu] Setup exception: {e}");
            }
        }

        private int SafeGetPrefValue(string prefKey, int defaultValue, List<int> validValues)
        {
            try
            {
                var pref = GetALifeMain.Prefs?.GetPreference<PreferenceInt>(prefKey);
                int val = pref != null ? pref.Value : defaultValue;
                if (!validValues.Contains(val))
                {
                    GetALifeMain.Logger.LogInfo($"[GetALifeMenu] Preference {prefKey} had invalid value {val}, defaulting to {defaultValue}");
                    return defaultValue;
                }
                return val;
            }
            catch (Exception e)
            {
                GetALifeMain.Logger.LogError($"[GetALifeMenu] SafeGetPrefValue error for {prefKey}: {e}");
                return defaultValue;
            }
        }
    }
}