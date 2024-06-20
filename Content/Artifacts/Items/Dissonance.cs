using RiskOfTerrain.Content.Artifacts;

namespace RiskOfTerrain.Content.Artifacts.Items
{
    public class Dissonance : BaseArtifact
    {
        public override bool ActiveFlag { get => ArtifactSystem.dissonance; set => ArtifactSystem.dissonance = value; }
    }
}