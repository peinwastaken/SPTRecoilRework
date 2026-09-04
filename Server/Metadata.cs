using SPTarkov.Server.Core.Models.Spt.Mod;
using Range = SemanticVersioning.Range;
using Version = SemanticVersioning.Version;

namespace SPTRecoilReworkServerMod
{
    public record Metadata : IModMetadata
    {
        public string ModGuid { get; init; } = "com.pein.camerarecoilmod";
        public string Name { get; init; } = "Recoil Rework";
        public string Author { get; init; } = "pein";
        public Version Version { get; init; } = new Version("2.0.0");
        public Range SptVersion { get; init; } = new Range("~4.1.0");
        public bool HasPrepatcher { get; init; } = false;
        public string? Url { get; init; } = "https://github.com/peinwastaken";
        public string License { get; init; } = "MIT";

        // unused
        public List<string>? Contributors { get; init; }
        public List<string>? Incompatibilities { get; init; }
        public Dictionary<string, Range>? ModDependencies { get; init; }
    }
}
