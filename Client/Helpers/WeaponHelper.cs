using EFT.InventoryLogic;
using PeinRecoilRework.Data;

namespace PeinRecoilRework.Helpers
{
    public class WeaponHelper
    {
        public static bool IsPistolCurrentlyEquipped = false;
        public static bool IsLeftStance = false;
        public static WeaponTemplate CurrentTemplate { get; set; } = null;

        public static bool IsPistol(WeaponTemplate template)
        {
            return template != null && template.weapClass == "pistol";
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
    }
}
