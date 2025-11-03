using BepInEx.Configuration;
using UnityEngine;

namespace SPTRecoilRework.Config.Settings
{
    public class WeaponSwaySettings
    {
        public static ConfigEntry<bool> EnableWeaponSway { get; set; }
        public static ConfigEntry<Vector2> WeaponSwayMult { get; set; }

        public static void Bind(ConfigFile Config, int order, string category)
        {
            string formattedCategory = Category.Format(order, category);

            EnableWeaponSway = Config.Bind(formattedCategory, "Enable Weapon Sway", true, new ConfigDescription("Enables weapon sway when moving.", null, new ConfigurationManagerAttributes { Order = 1000 }));
            WeaponSwayMult = Config.Bind(formattedCategory, "Weapon Sway Multiplier", new Vector2(1f, 1f), new ConfigDescription("Multiplier for weapon sway. X = horizontal, Y = vertical.", null, new ConfigurationManagerAttributes { Order = 990 }));
        }
    }
}
