using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace HexNowYouRest
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "com.hex.nowyourest";
        private const string PluginName = "HexNowYouRest";
        private const string PluginVersion = "1.0.0";

        private Harmony _harmonyInstance;

        internal static ManualLogSource Log;
        internal static Plugin Instance;

        private void Awake()
        {
            Instance = this;
            Log = Logger;

            _harmonyInstance = new Harmony(PluginGuid);
            _harmonyInstance.PatchAll();

            Log.LogInfo($"{PluginName} v{PluginVersion} loaded.");
        }

        private void OnDestroy()
        {
            Log.LogInfo($"{PluginName} v{PluginVersion} unloaded.");

            _harmonyInstance?.UnpatchSelf();
            _harmonyInstance = null;
            Log = null;
            Instance = null;
        }

        [HarmonyPatch(typeof(SE_Cozy), nameof(SE_Cozy.Setup))]
        internal static class PatchSECozySetup
        {
            private static void Postfix(SE_Cozy __instance)
            {
                if (Instance == null || __instance == null)
                {
                    return;
                }

                if (__instance.m_statusEffect != "Rested")
                {
                    return;
                }
                
                __instance.m_delay = 2f;
            }
        }
    }
}
