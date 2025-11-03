using EFT;
using EFT.Animations;
using HarmonyLib;
using SPT.Reflection.Patching;
using SPTRecoilRework.Components;
using SPTRecoilRework.Config.Settings;
using SPTRecoilRework.Helpers;
using System;
using System.Reflection;

namespace SPTRecoilRework.Patches
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

    public class UpdateHipAccuracyPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(Player.FirearmController), nameof(Player.FirearmController.UpdateHipInaccuracy));
        }

        private static bool IsTacEnabled(TacticalComboItemClass tacItem)
        {
            return tacItem.Light != null && tacItem.Light.IsActive;
        }

        [PatchPostfix]
        private static void PatchPostfix(Player.FirearmController __instance)
        {
            Player player = __instance.GetComponent<Player>();
            BreathEffector breath = player.ProceduralWeaponAnimation.Breath;

            if (!player.IsYourPlayer) return;

            bool isPistol = WeaponHelper.IsPistolCurrentlyEquipped;
            float onMult = isPistol ? PistolRecoilSettings.HipPenaltyTacOnMult.Value : RecoilSettings.HipPenaltyTacOnMult.Value;
            float offMult = isPistol ? PistolRecoilSettings.HipPenaltyTacOffMult.Value : RecoilSettings.HipPenaltyTacOffMult.Value;

            if (__instance.AimingDevices.Length == 0)
            {
                __instance.HipInaccuracy *= offMult;
                breath.HipPenalty = __instance.HipInaccuracy;
                return;
            }

            foreach (TacticalComboItemClass tacItem in __instance.AimingDevices)
            {
                if (tacItem.Light != null && tacItem.Light.IsActive)
                {
                    __instance.HipInaccuracy *= onMult;
                    breath.HipPenalty = __instance.HipInaccuracy;
                    return;
                }
            }

            __instance.HipInaccuracy *= offMult;
            breath.HipPenalty = __instance.HipInaccuracy;
        }
    }
}
