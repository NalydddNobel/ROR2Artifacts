using RiskOfTerrain.Content.Artifacts;

namespace RiskOfTerrain.Content.Artifacts.Items
{
    public class Metamorphosis : BaseArtifact
    {
        public override bool ActiveFlag { get => ArtifactSystem.metamorphosis; set => ArtifactSystem.metamorphosis = value; }

        public override bool unimplemented => true;
    }
}