using BepInEx.Configuration;
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
            { EWeaponClass.Pistol, Config.Settings.RealRecoilSettings.PistolRealRecoilMult },
            { EWeaponClass.SubMachineGun, Config.Settings.RealRecoilSettings.SmgRealRecoilMult },
            { EWeaponClass.Shotgun, Config.Settings.RealRecoilSettings.ShotgunRealRecoilMult },
            { EWeaponClass.SniperRifle, Config.Settings.RealRecoilSettings.SniperRealRecoilMult },
            { EWeaponClass.MarksmanRifle, Config.Settings.RealRecoilSettings.MarksmanRealRecoilMult },
            { EWeaponClass.MachineGun, Config.Settings.RealRecoilSettings.MachineGunRealRecoilMult },
            { EWeaponClass.GrenadeLauncher, Config.Settings.RealRecoilSettings.GrenadeLauncherRealRecoilMult }
        };

        public static WeaponTemplate CurrentTemplate { get; set; } = null;
        public static Vector2 CurrentRecoilMult = new Vector2(1, 1);

        public static bool IsPistolCurrentlyEquipped = false;
        public static bool IsLeftStance = false;
        public static float LeftStanceMult = 0f;

        public static bool IsPistol(WeaponTemplate template)
        {
            return GetWeaponClass(template) == EWeaponClass.Pistol;
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
    }
}
