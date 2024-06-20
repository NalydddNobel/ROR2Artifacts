using RiskOfTerrain.Content.Artifacts;

namespace RiskOfTerrain.Content.Artifacts.Items
{
    public class Honor : BaseArtifact
    {
        public override bool ActiveFlag { get => ArtifactSystem.honor; set => ArtifactSystem.honor = value; }
    }
}