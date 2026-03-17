using Kitchen;
using KitchenData;
using KitchenLib;
using KitchenLib.Event;
using KitchenLib.Logging;
using KitchenLib.Preferences;
using KitchenLib.References;
using KitchenLib.UI.PlateUp.PreferenceMenus;
using KitchenMods;
using System;
using System.Reflection;

namespace RedundantSemicolonMods.GetALife
{
    public class GetALifeMain : BaseMod
    {
        // mod info
        public const string MOD_ID = "com.redundantsemicolonmods.getalife";
        public const string MOD_NAME = "Get A Life!";
        public const string MOD_AUTHOR = "RedundantSemicolonMods";
        public const string MOD_VERSION = "0.1.0";
        public const string COMPATIBILE_VERSIONS = ">=1.1.6";

        // prefs info
        public static PreferenceManager Prefs = new PreferenceManager(MOD_ID);
        public const string PRICE_TIER_ID = "extraLifePriceTier";
        public const string RARITY_TIER_ID = "extraLifeRarityTier";

        // logging
        public static KitchenLogger Logger = new KitchenLogger(MOD_NAME);

        public GetALifeMain() : base(MOD_ID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, COMPATIBILE_VERSIONS, Assembly.GetExecutingAssembly())
        {
            // load prefs
            Prefs.RegisterPreference(new PreferenceInt(PRICE_TIER_ID, (int)PriceTier.Expensive));
            Prefs.RegisterPreference(new PreferenceInt(RARITY_TIER_ID, (int)RarityTier.Special));
            Prefs.Load();

            // subscribe to game data creation
            Events.BuildGameDataEvent += OnBuildGameData;
        }

        protected override void OnPostActivate(Mod mod)
        {
            // Try to get the preferences first. If they return null, register them.
            if (Prefs.GetPreference<PreferenceInt>(PRICE_TIER_ID) == null)
                Prefs.RegisterPreference(new PreferenceInt(PRICE_TIER_ID, (int)PriceTier.Expensive));

            if (Prefs.GetPreference<PreferenceInt>(RARITY_TIER_ID) == null)
                Prefs.RegisterPreference(new PreferenceInt(RARITY_TIER_ID, (int)RarityTier.Special));

            Prefs.Load();

            MainMenuPreferencesesMenu.RegisterMenu(MOD_NAME, typeof(GetALifeMenu));
            PauseMenuPreferencesesMenu.RegisterMenu(MOD_NAME, typeof(GetALifeMenu));

            Logger.LogInfo($"{MOD_NAME} initialized successfully!");
        }

        private void OnBuildGameData(object sender, BuildGameDataEventArgs eventArgs)
        {
            Appliance extraLife = eventArgs.gamedata.Get<Appliance>(ApplianceReferences.ExtraLife);

            if (extraLife != null)
            {
                LogApplianceAttributes(extraLife);

                // enable in shop
                extraLife.IsPurchasable = true;                 // allows it to drop as a blueprint
                extraLife.ShoppingTags = ShoppingTags.Basic;    // group with basic shop items
                extraLife.SellOnlyAsDuplicate = false;          // false = shows up randomly; true = only shows up if you already have one

                // apply defined price tier (coerce into valid enum index)
                int priceIndex = Prefs.GetPreference<PreferenceInt>(PRICE_TIER_ID).Value;
                extraLife.PriceTier = CoerceEnum<PriceTier>(priceIndex, PriceTier.Expensive);

                // apply defined rarity tier (coerce into valid enum index)
                int rarityIndex = Prefs.GetPreference<PreferenceInt>(RARITY_TIER_ID).Value;
                extraLife.RarityTier = CoerceEnum<RarityTier>(rarityIndex, RarityTier.Special);

                Logger.LogInfo("Extra Life has been added to the shop pool!");
            }
            else
            {
                Logger.LogError("Could not find the Extra Life appliance in GameData.");
            }
        }

        private void LogApplianceAttributes(Appliance appliance)
        {
            if (appliance == null) return;

            Logger.LogInfo($"=== Detailed Attributes for {appliance.Name} (ID: {appliance.ID}) ===");

            // 1. Log Fields (This is where Price, Rarity, and IsPurchasable live!)
            FieldInfo[] fields = typeof(Appliance).GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(appliance);
                Logger.LogInfo($"[Field] {field.Name}: {value ?? "null"}");
            }

            // 2. Log Properties (Description, etc.)
            PropertyInfo[] props = typeof(Appliance).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in props)
            {
                try
                {
                    object value = prop.GetValue(appliance, null);
                    Logger.LogInfo($"[Property] {prop.Name}: {value ?? "null"}");
                }
                catch { }
            }
            Logger.LogInfo("==================================================");
        }

        public static T CoerceEnum<T>(int index, T defaultValue) where T : struct, System.Enum
        {
            if (Enum.IsDefined(typeof(T), index))
            {
                return (T)Enum.ToObject(typeof(T), index);
            }

            Logger.LogInfo($"[WARNING] Preference has invalid value {index}. Defaulting to {defaultValue}.");
            return defaultValue;
        }
    }
}
