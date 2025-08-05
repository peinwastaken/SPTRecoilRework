namespace PeinRecoilRework.Config
{
    public class Category
    {
        public static string General = "General";
        public static string ReallyReal = "Real Recoil";
        public static string CameraRecoil = "Camera Recoil";
        public static string AdditionalCamera = "Additional Camera Recoil";
        public static string Debug = "Debug";
        public static string WeaponSway = "Weapon Sway";
        public static string RecoilSettings = "Recoil Settings";
        public static string PistolRecoilSettings = "Pistol Recoil Settings";

        public static string Format(int order, string category) => $"{order.ToString("D2")}. {category}";
    }
}
