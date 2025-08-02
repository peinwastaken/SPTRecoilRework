using BepInEx.Configuration;
using EFT;
using EFT.Animations;
using EFT.InventoryLogic;
using PeinRecoilRework.Data;
using System.Collections.Generic;
using UnityEngine;

namespace PeinRecoilRework.Helpers
{
    public static class WeaponHelper
    {
        public static Dictionary<EWeaponClass, ConfigEntry<Vector2>> RealRecoilMultipliers = new Dictionary<EWeaponClass, ConfigEntry<Vector2>>
        {
            { EWeaponClass.AssaultRifle, Config.Settings.RealRecoilSettings.RifleRealRecoilMult },
            { EWeaponClass.AssaultCarbine, Config.Settings.RealRecoilSettings.CarbineRealRecoilMult },
            { EWeaponClass.Pistol, Config.Settings.RealRecoilSettings.PistolRealRecoilMult },
            { EWeaponClass.SubMachineGun, Config.Settings.RealRecoilSettings.SmgRealRecoilMult },
            { EWeaponClass.Shotgun, Config.Settings.RealRecoilSettings.ShotgunRealRecoilMult },
            { EWeaponClass.SniperRifle, Config.Settings.RealRecoilSettings.SniperRealRecoilMult },
            { EWeaponClass.MarksmanRifle, Config.Settings.RealRecoilSettings.MarksmanRealRecoilMult },
            { EWeaponClass.MachineGun, Config.Settings.RealRecoilSettings.MachineGunRealRecoilMult },
            { EWeaponClass.GrenadeLauncher, Config.Settings.RealRecoilSettings.GrenadeLauncherRealRecoilMult }
        };

        public static WeaponTemplate CurrentTemplate = null;
        public static Player.FirearmController CurrentFirearmController = null;
        public static Vector2 CurrentRecoilMult = new Vector2(1, 1);
        public static bool IsPistolCurrentlyEquipped = false;

        public static bool IsPistol(WeaponTemplate template)
        {
            return GetWeaponClass(template) == EWeaponClass.Pistol;
        }

        public static Vector2 GetWeaponRecoilValues(Player.FirearmController fc)
        {
            if (fc == null) return Vector2.zero;

            float recoilUp = fc.Weapon.Template.RecoilForceUp;
            float recoilBack = fc.Weapon.Template.RecoilForceBack;
            float recoilDelta = fc.Weapon.RecoilDelta;

            DebugLogger.LogInfo($"recoilup : {recoilUp}, recoilback: {recoilBack}, recoildelta: {recoilDelta}");
            DebugLogger.LogInfo($"final recoil: {recoilUp + recoilUp * recoilDelta}, {recoilBack + recoilBack * recoilDelta}");

            // x = hor, y = vert
            Vector2 recoilValues = new Vector2(
                recoilBack + recoilBack * recoilDelta,
                recoilUp + recoilUp * recoilDelta
            );

            return recoilValues;
        }

        public static WeaponRecoilData FindRecoilData(string weaponId)
        {
            if (Plugin.WeaponRecoils == null || Plugin.WeaponRecoils.Count == 0)
            {
                return null;
            }

            return Plugin.WeaponRecoils
                .Find(x => x.WeaponId == weaponId || (x.WeaponIds != null && x.WeaponIds.Contains(weaponId)));
        }

        public static EWeaponClass GetWeaponClass(WeaponTemplate template)
        {
            string weaponClassId = template.weapClass.ToLower();

            switch (weaponClassId)
            {
                case "assaultrifle":
                    return EWeaponClass.AssaultRifle;
                case "assaultcarbine":
                    return EWeaponClass.AssaultCarbine;
                case "pistol":
                    return EWeaponClass.Pistol;
                case "shotgun":
                    return EWeaponClass.Shotgun;
                case "sniperrifle":
                    return EWeaponClass.SniperRifle;
                case "machinegun":
                    return EWeaponClass.MachineGun;
                case "smg":
                    return EWeaponClass.SubMachineGun;
                case "marksmanrifle":
                    return EWeaponClass.MarksmanRifle;
                case "grenadelauncher":
                    return EWeaponClass.GrenadeLauncher;
                default:
                    return EWeaponClass.None; // hopefully this never happens
            }
        }

        public static bool IsUsingIrons(ProceduralWeaponAnimation pwa)
        {
            if (pwa.CurrentAimingMod != null)
            {
                return pwa.CurrentAimingMod.Item is IronSightItemClass;
            }
            else if (pwa.CurrentScope != null) // sort of a fallback
            {
                return !pwa.CurrentScope.IsOptic;
            }
            else // actual fallback
            {
                return false;
            }
        }
    }
}
