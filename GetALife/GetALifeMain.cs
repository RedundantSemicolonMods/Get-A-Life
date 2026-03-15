using KitchenLib;
using KitchenLib.Logging;
using KitchenMods;
using System.Reflection;

namespace RedundantSemicolonMods.GetALife
{
    public class GetALifeMain : BaseMod
    {

        public const string MOD_ID = "com.redundantsemicolonmods.getalife";
        public const string MOD_NAME = "Get A Life!";
        public const string MOD_AUTHOR = "RedundantSemicolonMods";
        public const string MOD_VERSION = "0.1.0";
        public const string COMPATIBILE_VERSIONS = ">=1.1.6";

        public GetALifeMain() : base(MOD_ID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, COMPATIBILE_VERSIONS, Assembly.GetExecutingAssembly()) {}

        protected override void OnPostActivate(Mod mod)
        {
            KitchenLogger logger = new KitchenLogger(MOD_NAME);
            logger.LogInfo($"{MOD_NAME} initialized successfully! Time to save some runs.");
        }
    }
}
