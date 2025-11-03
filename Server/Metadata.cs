using SPTarkov.Server.Core.Models.Spt.Mod;
using Range = SemanticVersioning.Range;
using Version = SemanticVersioning.Version;

namespace SPTRecoilReworkServerMod
{
    public record Metadata : AbstractModMetadata
    {
        public override string ModGuid { get; init; } = "com.pein.camerarecoilmod";
        public override string Name { get; init; } = "Recoil Rework";
        public override string Author { get; init; } = "pein";
        public override Version Version { get; init; } = new Version("3.0.0");
        public override Range SptVersion { get; init; } = new Range("~4.0.0");
        public override string? Url { get; init; } = "https://github.com/peinwastaken";
        public override string License { get; init; } = "MIT";
        
        // unused
        public override bool? IsBundleMod { get; init; }
        public override List<string>? Contributors { get; init; }
        public override List<string>? Incompatibilities { get; init; }
        public override Dictionary<string, Range>? ModDependencies { get; init; }
    }
}
