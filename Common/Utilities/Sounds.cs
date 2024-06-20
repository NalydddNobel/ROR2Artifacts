using Terraria.Audio;

namespace RiskOfTerrain.Common.Utilities;

public partial class Sounds {
    private const string Path = "RiskOfTerrain/Assets/Sounds/";

    internal static SoundStyle GetMulti(string name, int num, float volume = 1f, float pitch = 0f, float variance = 0f) {
        return new SoundStyle($"{Path}{name}", 0, num) { Volume = volume, Pitch = pitch, PitchVariance = variance, };
    }
    internal static SoundStyle Get(string name, float volume = 1f, float pitch = 0f, float variance = 0f) {
        return new SoundStyle($"{Path}{name}") { Volume = volume, Pitch = pitch, PitchVariance = variance, };
    }

}
