using Kitchen;
using Kitchen.Modules;
using KitchenData;
using KitchenLib;
using KitchenLib.Preferences;
using System.Collections.Generic;

namespace RedundantSemicolonMods.GetALife
{
    public class GetALifeMenu : KLMenu
    {
        public GetALifeMenu(UnityEngine.Transform container, ModuleList module_list) : base(container, module_list) {
            GetALifeMain.Logger.LogInfo($"[DEBUG] GetALifeMenu Constructor Reached!");
        }

        public override void Setup(int player_id)
        {
            GetALifeMain.Logger.LogInfo($"[DEBUG] GetALifeMenu Setup Reached!");
            AddLabel("Get A Life Settings");

            // select rarity
            AddInfo("Item Frequency");
            int rarityValue = GetALifeMain.Prefs.GetPreference<PreferenceInt>(GetALifeMain.RARITY_TIER_ID).Value;
            rarityValue = (int)GetALifeMain.CoerceEnum<RarityTier>(rarityValue, RarityTier.Special);

            GetALifeMain.Logger.LogInfo($"[DEBUG] RARITY TARGET VALUE: {rarityValue}");
            GetALifeMain.Logger.LogInfo($"[DEBUG] RARITY VALID LIST: {(int)RarityTier.Common}, {(int)RarityTier.Uncommon}, {(int)RarityTier.Rare}, {(int)RarityTier.Special}");

            Add(new Option<int>(
                new List<int> { 
                    (int)RarityTier.Common, 
                    (int)RarityTier.Uncommon, 
                    (int)RarityTier.Rare,
                    (int)RarityTier.Special 
                },
                rarityValue,
                new List<string> { 
                    "Common", 
                    "Uncommon", 
                    "Rare", 
                    "Special" 
                }
            )).OnChanged += (obj, value) =>
            {
                GetALifeMain.Prefs.GetPreference<PreferenceInt>(GetALifeMain.RARITY_TIER_ID).Set(value);
                GetALifeMain.Prefs.Save();
            };

            // select price
            AddInfo("Item Price");
            int priceValue = GetALifeMain.Prefs.GetPreference<PreferenceInt>(GetALifeMain.PRICE_TIER_ID).Value;
            priceValue = (int)GetALifeMain.CoerceEnum<PriceTier>(priceValue, PriceTier.Expensive);

            GetALifeMain.Logger.LogInfo($"[DEBUG] PRICE TARGET VALUE: {priceValue}");
            GetALifeMain.Logger.LogInfo($"[DEBUG] PRICE VALID LIST: {(int)PriceTier.Free}, {(int)PriceTier.VeryCheap}, {(int)PriceTier.Cheap}, {(int)PriceTier.MediumCheap}, {(int)PriceTier.Medium}, {(int)PriceTier.Expensive}, {(int)PriceTier.VeryExpensive}, {(int)PriceTier.ExtremelyExpensive}");

            Add(new Option<int>(
                new List<int> { 
                    (int)PriceTier.Free, 
                    (int)PriceTier.VeryCheap,
                    (int)PriceTier.Cheap, 
                    (int)PriceTier.MediumCheap, 
                    (int)PriceTier.Medium, 
                    (int)PriceTier.Expensive, 
                    (int)PriceTier.VeryExpensive ,
                    (int)PriceTier.ExtremelyExpensive
                },
                priceValue,
                new List<string> { 
                    "Free","Very Cheap","Cheap", "Medium Cheap", "Medium", "Expensive", "Very Expensive", "Extremely Expensive"
                }
            )).OnChanged += (obj, value) =>
            {
                GetALifeMain.Prefs.GetPreference<PreferenceInt>(GetALifeMain.PRICE_TIER_ID).Set(value);
                GetALifeMain.Prefs.Save();
            };
            New<SpacerElement>(true);
            AddButton("Back", (obj) => RequestPreviousMenu());
            ResetPanel();
        }
    }
}
