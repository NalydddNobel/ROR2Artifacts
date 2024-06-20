using RiskOfTerrain.Content.Artifacts;

namespace RiskOfTerrain.Content.Artifacts.Items
{
    public class Sacrifice : BaseArtifact
    {
        public override bool ActiveFlag { get => ArtifactSystem.sacrifice; set => ArtifactSystem.sacrifice = value; }

        public override bool unimplemented => true;
    }
}