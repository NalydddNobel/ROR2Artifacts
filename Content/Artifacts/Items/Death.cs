using RiskOfTerrain.Content.Artifacts;

namespace RiskOfTerrain.Content.Artifacts.Items
{
    public class Death : BaseArtifact
    {
        public override bool ActiveFlag { get => ArtifactSystem.death; set => ArtifactSystem.death = value; }
    }
}