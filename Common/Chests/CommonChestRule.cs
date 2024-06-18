using Terraria.GameContent.ItemDropRules;

namespace RiskOfTerrain.Common.Chests;

public class CommonChestRule(int itemId, int chanceDenominator, int amountDroppedMinimum = 1, int amountDroppedMaximum = 1, int chanceNumerator = 1) : CommonDrop(itemId, chanceDenominator, amountDroppedMinimum, amountDroppedMaximum, chanceNumerator), IChestDropRule {
    void IChestDropRule.DropFromChest(ChestDropInfo info) {

    }
}
