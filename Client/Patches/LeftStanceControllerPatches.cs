using EFT;
using HarmonyLib;
using PeinRecoilRework.Helpers;
using SPT.Reflection.Patching;
using System.Reflection;

namespace PeinRecoilRework.Patches
{
    public class ToggleLeftStancePatch : ModulePatch
    {
        private static FieldInfo playerField;

        protected override MethodBase GetTargetMethod()
        {
            playerField = AccessTools.Field(typeof(Player.FirearmController), "_player");
            return AccessTools.Method(typeof(Player.FirearmController), nameof(Player.FirearmController.ChangeLeftStance));
        }

        [PatchPrefix]
        private static bool PatchPrefix(Player.FirearmController __instance)
        {
            Player player = (Player)playerField.GetValue(__instance);

            if (!Util.GetLocalPlayer() == player)
            {
                return true;
            }
            else
            {
                if (__instance.Blindfire)
                {
                    return false;
                }

                if (player.MovementContext.IsInMountedState)
                {
                    return false;
                }

                WeaponHelper.IsLeftStance = !WeaponHelper.IsLeftStance;
            }

            return false;
        }
    }
}
