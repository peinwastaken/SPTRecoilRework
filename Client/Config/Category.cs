namespace PeinRecoilRework.Config
{
    public class Category
    {
        public static string General = "General";
        public static string LeftStance = "Left Stance";
        public static string ReallyReal = "Real Recoil";
        public static string CameraRecoil = "Camera Recoil";
        public static string RecoilPos = "Rifle Recoil Position";
        public static string PistolRecoilPos = "Pistol Recoil Position";
        public static string RecoilAng = "Rifle Recoil Angle";
        public static string PistolRecoilAng = "Pistol Recoil Angle";
        public static string AdditionalCamera = "Additional Camera Recoil";
        public static string Debug = "Debug";
        public static string WeaponSway = "Weapon Sway";

        public static string Format(int order, string category) => $"{order}. {category}";
    }
}
