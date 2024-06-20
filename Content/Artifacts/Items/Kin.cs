using RiskOfTerrain.Content.Artifacts;

namespace RiskOfTerrain.Content.Artifacts.Items
{
    public class Kin : BaseArtifact
    {
        public override bool ActiveFlag { get => ArtifactSystem.kin; set => ArtifactSystem.kin = value; }

        public override bool unimplemented => true;
    }
}