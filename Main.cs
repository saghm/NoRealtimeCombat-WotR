using HarmonyLib;
using Kingmaker.Settings;
using Kingmaker.UI.MVVM._ConsoleView.InGame;
using Owlcat.Runtime.UI.ConsoleTools.HintTool;
using System.Linq;
using UnityModManagerNet;

namespace NoRealtimeCombat
{
    public class Main
    {
        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll();

            var confirmTempValue = typeof(SettingsEntity<bool>)
                .GetMethod("ConfirmTempValue", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            var prefix = typeof(PatchConfirmTempValue)
                .GetMethod("Prefix", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            harmony.Patch(confirmTempValue, new HarmonyMethod(prefix));

            return true;
        }
    }

    [HarmonyPatch(typeof(InGameConsoleView), "UpdateTBMModeHint")]
    static class PatchTBMModeHint
    {
        static bool Prefix(InGameConsoleView __instance)
        {
            Traverse.Create(__instance).Field("m_TBMModeHint").GetValue<ConsoleHint>()?.SetActive(false);
            return false;
        }
    }

    static class PatchConfirmTempValue
    {
        static bool Prefix(SettingsEntity<bool> __instance)
        {
            if (__instance.Key == "settings.game.turn-based.enable-turn-based")
            {
                return false;
            }
            return true;
        }
    }
}