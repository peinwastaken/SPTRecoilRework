using Newtonsoft.Json;
using System.Collections.Generic;
using System.Numerics;

namespace PeinRecoilRework.Data
{
    public class OverrideProperties
    {
        public bool ForceClientOverride { get; set; } = false;
        public float? RecoilForceUp { get; set; } = null;
        public float? RecoilForceBack { get; set; } = null;
        public float? RecoilCategoryMultiplierHandRotation { get; set; } = null;
        public float? RecoilStableAngleIncreaseStep { get; set; } = null;
        public float? HandRecoilAngUpMult { get; set; } = null;
        public float? HandRecoilAngSideMult { get; set; } = null;
        public float? HandRecoilAngIntensity { get; set; } = null;
        public float? HandRecoilAngReturnSpeed { get; set; } = null;
        public float? HandRecoilAngDamping { get; set; } = null;
        public float? HandRecoilPosBackMult { get; set; } = null;
        public float? HandRecoilPosIntensity { get; set; } = null;
        public float? HandRecoilPosReturnSpeed { get; set; } = null;
        public float? HandRecoilPosDamping { get; set; } = null;
        public float? CameraSnap { get; set; } = null;
        public Vector3? RecoilCenter { get; set; } = null;
    }

    public class WeaponRecoilData
    {
        [JsonProperty("weaponId")]
        public string WeaponId { get; set; }

        [JsonProperty("weaponIds")]
        public List<string> WeaponIds { get; set; }

        [JsonProperty("overrideProperties")]
        public OverrideProperties OverrideProperties { get; set; }
    }
}
