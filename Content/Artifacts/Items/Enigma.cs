using RiskOfTerrain.Content.Artifacts;

namespace RiskOfTerrain.Content.Artifacts.Items
{
    public class Enigma : BaseArtifact
    {
        public override bool ActiveFlag { get => ArtifactSystem.enigma; set => ArtifactSystem.enigma = value; }
    }
}