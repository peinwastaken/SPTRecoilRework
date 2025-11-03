using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using System.Text.Json.Serialization;

namespace SPTRecoilReworkServerMod.Models
{
    public class WeaponOverrideConfig
    {
        [JsonPropertyName("weaponId")]
        public string? WeaponId { get; set; }
        
        [JsonPropertyName("weaponIds")]
        public List<string>? WeaponIds { get; set; }
        
        [JsonPropertyName("overrideProperties")]
        public required TemplateItemProperties OverrideProperties { get; set; }
    }
}
