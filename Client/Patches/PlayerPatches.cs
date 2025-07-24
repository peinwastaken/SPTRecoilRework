using EFT;
using EFT.Animations;
using HarmonyLib;
using PeinRecoilRework.Components;
using PeinRecoilRework.Config.Settings;
using PeinRecoilRework.Helpers;
using SPT.Reflection.Patching;
using System.Reflection;

namespace PeinRecoilRework.Patches
{
    public class PlayerInitPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(Player), nameof(Player.Init));
        }

        [PatchPostfix]
        public static void PatchPostfix(Player __instance)
        {
            if (__instance.IsYourPlayer == true)
            {
                __instance.gameObject.AddComponent<RealRecoilComponent>();
                __instance.gameObject.AddComponent<CameraOffsetComponent>();
                // __instance.gameObject.AddComponent<LeftStanceComponent>();
            }
        }
    }

    public class SetPlayerAimingPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(Player.FirearmController), "SetAim", new[] { typeof(bool) });
        }

        [PatchPostfix]
        public static void PatchPostfix(Player.FirearmController __instance, bool value)
        {
            if (__instance == null) return; // nre fix

            Player player = __instance.GetComponent<Player>();
            bool isYourPlayer = player.IsYourPlayer;
            ProceduralWeaponAnimation pwa = player.ProceduralWeaponAnimation;
            EPointOfView pov = pwa.PointOfView;

            if (isYourPlayer && pov == EPointOfView.FirstPerson)
            {
                player.MovementContext.PlayerAnimator.SetAiming(false);
            }
        }
    }
}
