using RiskOfTerrain.Content.Artifacts;

namespace RiskOfTerrain.Content.Artifacts.Items
{
    public class Swarms : BaseArtifact
    {
        public override bool ActiveFlag { get => ArtifactSystem.swarms; set => ArtifactSystem.swarms = value; }

        public override bool unimplemented => true;
    }
}