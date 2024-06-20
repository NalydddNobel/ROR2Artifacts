using RiskOfTerrain.Content.Artifacts;

namespace RiskOfTerrain.Content.Artifacts.Items
{
    public class Glass : BaseArtifact
    {
        public override bool ActiveFlag { get => ArtifactSystem.glass; set => ArtifactSystem.glass = value; }
    }
}