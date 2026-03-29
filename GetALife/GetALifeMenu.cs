using Kitchen;
using Kitchen.Modules;
using KitchenData;
using KitchenLib;
using KitchenLib.Preferences;
using KitchenLib.References;
using System.Collections.Generic;
using UnityEngine;

namespace RedundantSemicolonMods.GetALife
{
    public class GetALifeMenu : KLMenu
    {
        private Option<bool> modEnabled;
        private Option<int> rarityOption;
        private Option<int> priceOption;

        private readonly List<bool> modEnabledValues = new List<bool> { true, false };

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

        private readonly List<string> modEnabledLabels = new List<string> { "Enabled", "Disabled" };
        private readonly List<string> rarityLabels = new List<string> { "Common", "Uncommon", "Rare", "Special" };
        private readonly List<string> priceLabelsPlain = new List<string>
        {
            "Free", "Very Cheap", "Cheap", "Medium Cheap", "Medium", "Expensive", "Very Expensive", "Extremely Expensive"
        };

        public GetALifeMenu(Transform container, ModuleList module_list) : base(container, module_list)
        {
            GetALifeMain.Logger.LogInfo("[GetALifeMenu] Constructor");
        }

        public override void Setup(int player_id)
        {
            GetALifeMain.Logger.LogInfo("[GetALifeMenu] Setup start");
            ModuleList.Clear();

            AddLabel("Get A Life! Settings");

            // Master toggle
            bool isModEnabled = GetALifeMain.SafeGetPrefValue(GetALifeMain.MOD_ENABLED_ID, true);
            modEnabled = new Option<bool>(modEnabledValues, isModEnabled, modEnabledLabels);
            AddSelect(modEnabled);
            modEnabled.OnChanged += (s, value) =>
            {
                var pref = GetALifeMain.Prefs.GetPreference<PreferenceBool>(GetALifeMain.MOD_ENABLED_ID);
                if (pref != null)
                {
                    pref.Set(value);
                    GetALifeMain.Prefs.Save();
                    GetALifeMain.Logger.LogInfo($"[GetALifeMenu] Saved mod enabled preference = {value}");

                    if (GameData.Main != null)
                    {
                        var extraLife = GameData.Main.Get<Appliance>(ApplianceReferences.ExtraLife);
                        if (extraLife != null)
                        {
                            extraLife.IsPurchasable = value;
                        }
                    }
                }
                else
                {
                    GetALifeMain.Logger.LogError("[GetALifeMenu] Failed to save mod enabled preference: preference object null.");
                }
            };

            New<SpacerElement>(true);

            // Rarity option
            AddInfo("Item Frequency");
            int rarityValue = SafeGetPrefValue(GetALifeMain.RARITY_TIER_ID, (int)RarityTier.Special, rarityValues);
            rarityOption = new Option<int>(rarityValues, rarityValue, rarityLabels);
            AddSelect(rarityOption);
            rarityOption.OnChanged += (s, value) =>
            {
                var pref = GetALifeMain.Prefs.GetPreference<PreferenceInt>(GetALifeMain.RARITY_TIER_ID);
                if (pref != null)
                {
                    pref.Set(value);
                    GetALifeMain.Prefs.Save();
                    GetALifeMain.Logger.LogInfo($"[GetALifeMenu] Saved rarity preference = {value}");

                    if (GameData.Main != null)
                    {
                        var extraLife = GameData.Main.Get<Appliance>(ApplianceReferences.ExtraLife);
                        if (extraLife != null)
                        {
                            extraLife.RarityTier = (RarityTier)value;
                        }
                    }
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
                var pref = GetALifeMain.Prefs.GetPreference<PreferenceInt>(GetALifeMain.PRICE_TIER_ID);
                if (pref != null)
                {
                    pref.Set(value);
                    GetALifeMain.Prefs.Save();
                    GetALifeMain.Logger.LogInfo($"[GetALifeMenu] Saved price preference = {(PriceTier)value}");

                    if (GameData.Main != null)
                    {
                        var extraLife = GameData.Main.Get<Appliance>(ApplianceReferences.ExtraLife);
                        if (extraLife != null)
                        {
                            extraLife.PriceTier = (PriceTier)value;
                            extraLife.PurchaseCost = 0;
                        }
                    }
                }
            };

            New<SpacerElement>(true);
            AddButton("Back", (_) => RequestPreviousMenu());

            // finalize layout
            ResetPanel();

            GetALifeMain.Logger.LogInfo("[GetALifeMenu] Setup complete");
        }

        private int SafeGetPrefValue(string prefKey, int defaultValue, List<int> validValues)
        {
            var pref = GetALifeMain.Prefs?.GetPreference<PreferenceInt>(prefKey);
            int val = pref != null ? pref.Value : defaultValue;
            if (!validValues.Contains(val))
            {
                GetALifeMain.Logger.LogWarning($"[GetALifeMenu] Preference {prefKey} had invalid value {val}, defaulting to {defaultValue}");
                return defaultValue;
            }
            return val;
        }
    }
}