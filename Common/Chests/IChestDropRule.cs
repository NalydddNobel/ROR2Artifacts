namespace RiskOfTerrain.Common.Chests;

public interface IChestDropRule {
    void DropFromChest(ChestDropInfo info);
}

public record struct ChestDropInfo(int X, int Y);