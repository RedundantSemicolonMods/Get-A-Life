using Kitchen;
using KitchenData;
using KitchenLib;
using KitchenLib.Event;
using KitchenLib.Logging;
using KitchenLib.Preferences;
using KitchenLib.References;
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
            // --- PAUSE MENU LOGGING ---
            Events.PreferenceMenu_PauseMenu_CreateSubmenusEvent += (s, args) =>
            {
                try
                {
                    Logger.LogInfo("[DEBUG] Attempting to build Pause Menu instance...");
                    var menuInstance = new GetALifeMenu<PauseMenuAction>(args.Container, args.Module_list);
                    args.Menus.Add(typeof(GetALifeMenu<PauseMenuAction>), menuInstance);
                    Logger.LogInfo("[DEBUG] Pause Menu added to args.Menus successfully!");
                }
                catch (Exception e)
                {
                    Logger.LogError($"[FATAL] Pause Menu failed to instantiate: {e.Message}");
                    Logger.LogError(e.StackTrace);
                }
            };

            ModsPreferencesMenu<PauseMenuAction>.RegisterMenu(MOD_NAME, typeof(GetALifeMenu<PauseMenuAction>), typeof(PauseMenuAction));

            // --- MAIN MENU LOGGING ---
            Events.PreferenceMenu_MainMenu_CreateSubmenusEvent += (s, args) =>
            {
                try
                {
                    Logger.LogInfo("[DEBUG] Attempting to build Main Menu instance...");
                    var menuInstance = new GetALifeMenu<MainMenuAction>(args.Container, args.Module_list);
                    args.Menus.Add(typeof(GetALifeMenu<MainMenuAction>), menuInstance);
                    Logger.LogInfo("[DEBUG] Main Menu added to args.Menus successfully!");
                }
                catch (Exception e)
                {
                    Logger.LogError($"[FATAL] Main Menu failed to instantiate: {e.Message}");
                    Logger.LogError(e.StackTrace);
                }
            };

            ModsPreferencesMenu<MainMenuAction>.RegisterMenu(MOD_NAME, typeof(GetALifeMenu<MainMenuAction>), typeof(MainMenuAction));

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
