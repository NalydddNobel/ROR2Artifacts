using RiskOfTerrain.Content.Artifacts;

namespace RiskOfTerrain.Content.Artifacts.Items
{
    public class WeakAssKnees : BaseArtifact
    {
        public override bool ActiveFlag { get => ArtifactSystem.frailty; set => ArtifactSystem.frailty = value; }

        public override bool unimplemented => true;
    }
}