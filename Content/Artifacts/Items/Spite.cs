using RiskOfTerrain.Content.Artifacts;

namespace RiskOfTerrain.Content.Artifacts.Items
{
    public class Spite : BaseArtifact
    {
        public override bool ActiveFlag { get => ArtifactSystem.spite; set => ArtifactSystem.spite = value; }
    }
}