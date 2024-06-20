using RiskOfTerrain.Content.Artifacts;

namespace RiskOfTerrain.Content.Artifacts.Items
{
    public class Evolution : BaseArtifact
    {
        public override bool ActiveFlag { get => ArtifactSystem.evolution; set => ArtifactSystem.evolution = value; }

        public override bool unimplemented => true;
    }
}