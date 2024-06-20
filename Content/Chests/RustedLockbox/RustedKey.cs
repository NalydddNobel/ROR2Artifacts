using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Chests.RustedLockbox;

public class RustedKey : ModItem {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 3;
    }

    public override void SetDefaults() {
        Item.CloneDefaults(ItemID.GoldenKey);
        Item.rare = ItemRarityID.Blue;
    }
}