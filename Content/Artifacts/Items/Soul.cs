using RiskOfTerrain.Content.Artifacts;

namespace RiskOfTerrain.Content.Artifacts.Items
{
    public class Soul : BaseArtifact
    {
        public override bool ActiveFlag { get => ArtifactSystem.soul; set => ArtifactSystem.soul = value; }
    }
}