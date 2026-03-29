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
        public static PreferenceManager Prefs;
        public const string MOD_ENABLED_ID = "getALifeEnabled";
        public const string PRICE_TIER_ID = "extraLifePriceTier";
        public const string RARITY_TIER_ID = "extraLifeRarityTier";

        // logging
        public static KitchenLogger Logger = new KitchenLogger(MOD_NAME);

        public GetALifeMain() : base(MOD_ID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, COMPATIBILE_VERSIONS, Assembly.GetExecutingAssembly())
        {
            // constructor lightweight
        }

        protected override void OnPostActivate(Mod mod)
        {
            Logger.LogInfo($"{MOD_NAME} OnPostActivate starting.");

            // Initialize PreferenceManager for this mod
            if (Prefs == null)
            {
                Prefs = new PreferenceManager(MOD_ID);
                
                // Register preferences immediately upon creation
                Prefs.RegisterPreference(new PreferenceBool(MOD_ENABLED_ID, true));
                Prefs.RegisterPreference(new PreferenceInt(PRICE_TIER_ID, (int)PriceTier.Expensive));
                Prefs.RegisterPreference(new PreferenceInt(RARITY_TIER_ID, (int)RarityTier.Special));
            }

            // KitchenLib's Load() already safely handles IO exceptions and backups internally!
            Prefs.Load();

            // Register menu(s) so the mod appears under Mod Preferences
            ModsPreferencesMenu<MenuAction>.RegisterMenu(MOD_NAME, typeof(GetALifeMenu), typeof(MenuAction));
            ModsPreferencesMenu<PauseMenuAction>.RegisterMenu(MOD_NAME, typeof(GetALifeMenu), typeof(PauseMenuAction));

            // ALSO explicitly register with the MainMenu / PauseMenu preference menus to ensure instance creation
            MainMenuPreferencesesMenu.RegisterMenu(MOD_NAME, typeof(GetALifeMenu));
            PauseMenuPreferencesesMenu.RegisterMenu(MOD_NAME, typeof(GetALifeMenu));

            // Subscribe to BuildGameData to modify the Extra Life appliance once GameData is available
            Events.BuildGameDataEvent += OnBuildGameData;

            Logger.LogInfo($"{MOD_NAME} initialized successfully!");
        }

        private void OnBuildGameData(object sender, BuildGameDataEventArgs eventArgs)
        {
            // Only act when we have game data
            if (eventArgs?.gamedata == null) return;

            Appliance extraLife = eventArgs.gamedata.Get<Appliance>(ApplianceReferences.ExtraLife);

            if (extraLife == null)
            {
                Logger.LogError("Could not find the Extra Life appliance in GameData.");
                return;
            }

            Logger.LogInfo($"Applying {MOD_NAME} preferences to Extra Life (ID: {extraLife.ID}).");

            // enable in shop
            extraLife.IsPurchasable = SafeGetPrefValue(MOD_ENABLED_ID, true);
            extraLife.ShoppingTags = ShoppingTags.Basic;
            extraLife.SellOnlyAsDuplicate = false;

            // apply defined price tier (coerce into valid enum index)
            int priceIndex = SafeGetPrefValue(PRICE_TIER_ID, (int)PriceTier.Expensive);
            extraLife.PriceTier = CoerceEnum<PriceTier>(priceIndex, PriceTier.Expensive);
            extraLife.PurchaseCost = 0; // Force the game to use PriceTier instead of a fixed cost

            // apply defined rarity tier
            int rarityIndex = SafeGetPrefValue(RARITY_TIER_ID, (int)RarityTier.Special);
            extraLife.RarityTier = CoerceEnum<RarityTier>(rarityIndex, RarityTier.Special);

            // We are not setting a numeric PurchaseCost; price is represented by PriceTier only.
            Logger.LogInfo($"Configured Extra Life: PriceTier={extraLife.PriceTier}, RarityTier={extraLife.RarityTier}");

            Logger.LogInfo("Extra Life has been added to the shop pool!");
        }

        /// <summary>
        /// Safely gets a boolean preference value, falling back to defaultValue if missing.
        /// </summary>
        public static bool SafeGetPrefValue(string prefKey, bool defaultValue)
        {
            var pref = Prefs?.GetPreference<PreferenceBool>(prefKey);
            return pref != null ? pref.Value : defaultValue;
        }

        /// <summary>
        /// Safely gets an integer preference value, falling back to defaultValue if missing.
        /// </summary>
        public static int SafeGetPrefValue(string prefKey, int defaultValue)
        {
            var pref = Prefs?.GetPreference<PreferenceInt>(prefKey);
            return pref != null ? pref.Value : defaultValue;
        }

        public static T CoerceEnum<T>(int index, T defaultValue) where T : struct, Enum
        {
            if (Enum.IsDefined(typeof(T), index))
            {
                return (T)Enum.ToObject(typeof(T), index);
            }

            Logger.LogWarning($"Preference has invalid value {index}. Defaulting to {defaultValue}.");
            return defaultValue;
        }
    }
}