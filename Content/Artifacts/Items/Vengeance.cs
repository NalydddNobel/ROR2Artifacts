using RiskOfTerrain.Content.Artifacts;

namespace RiskOfTerrain.Content.Artifacts.Items
{
    public class Vengeance : BaseArtifact
    {
        public override bool ActiveFlag { get => ArtifactSystem.vengeance; set => ArtifactSystem.vengeance = value; }

        public override bool unimplemented => true;
    }
}