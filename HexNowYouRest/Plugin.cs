using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;

namespace HexNowYouRest
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "com.hex.nowyourest";
        private const string PluginName = "HexNowYouRest";
        private const string PluginVersion = "1.0.1";

        private Harmony _harmonyInstance;

        internal static ManualLogSource Log;
        internal static Plugin Instance;

        private void Awake()
        {
            Instance = this;
            Log = Logger;

            Assembly assembly = Assembly.GetExecutingAssembly();
            _harmonyInstance = new Harmony(PluginGuid);
            _harmonyInstance.PatchAll(assembly);

            Log.LogInfo($"{PluginName} v{PluginVersion} loaded.");
        }

        private void OnDestroy()
        {
            Log.LogInfo($"{PluginName} v{PluginVersion} unloaded.");

            _harmonyInstance?.UnpatchSelf();
            _harmonyInstance = null;
            Instance = null;
            Log = null;
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
