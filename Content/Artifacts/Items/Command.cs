using RiskOfTerrain.Content.Artifacts;

namespace RiskOfTerrain.Content.Artifacts.Items
{
    public class Command : BaseArtifact
    {
        public override bool ActiveFlag { get => ArtifactSystem.command; set => ArtifactSystem.command = value; }

        public override bool unimplemented => true;
    }
}