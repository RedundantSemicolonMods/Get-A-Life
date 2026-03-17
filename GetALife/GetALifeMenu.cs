using Kitchen;
using Kitchen.Modules;
using KitchenData;
using KitchenLib;
using KitchenLib.Preferences;
using System.Collections.Generic;

namespace RedundantSemicolonMods.GetALife
{
    public class GetALifeMenu<T> : KLMenu<T>
    {
        public GetALifeMenu(UnityEngine.Transform container, ModuleList module_list) : base(container, module_list) { }

        public override void Setup(int player_id)
        {
            AddLabel("Get A Life Settings");

            // select rarity
            AddInfo("Item Frequency");
            Add(new Option<int>(
                new List<int> { 
                    (int)RarityTier.Common, 
                    (int)RarityTier.Uncommon, 
                    (int)RarityTier.Rare,
                    (int)RarityTier.Special 
                },
                GetALifeMain.Prefs.GetPreference<PreferenceInt>(GetALifeMain.RARITY_TIER_ID).Value,
                new List<string> { 
                    "Common", 
                    "Uncommon", 
                    "Rare", 
                    "Special" 
                }
            )).OnChanged += (obj, value) =>
            {
                GetALifeMain.Prefs.Set<PreferenceInt>(GetALifeMain.RARITY_TIER_ID, value);
                GetALifeMain.Prefs.Save();
            };

            // select price
            AddInfo("Item Price");
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
                GetALifeMain.Prefs.GetPreference<PreferenceInt>(GetALifeMain.PRICE_TIER_ID).Value,
                new List<string> { 
                    $"Free ({Appliance.GetPrice(PriceTier.Free)}g)",
                    $"Very Cheap ({Appliance.GetPrice(PriceTier.VeryCheap)}g)",
                    $"Cheap ({Appliance.GetPrice(PriceTier.Cheap)}g)", 
                    $"Medium Cheap ({Appliance.GetPrice(PriceTier.MediumCheap)}g)", 
                    $"Medium ({Appliance.GetPrice(PriceTier.Medium)}g)", 
                    $"Expensive ({Appliance.GetPrice(PriceTier.Expensive)}g)", 
                    $"Very Expensive ({Appliance.GetPrice(PriceTier.VeryExpensive)}g)", 
                    $"Extremely Expensive ({Appliance.GetPrice(PriceTier.ExtremelyExpensive)}g)" 
                }
            )).OnChanged += (obj, value) =>
            {
                GetALifeMain.Prefs.Set<PreferenceInt>(GetALifeMain.PRICE_TIER_ID, value);
                GetALifeMain.Prefs.Save();
            };

            AddButton("Back", (obj) => RequestPreviousMenu());
        }
    }
}
