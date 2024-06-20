using RiskOfTerrain.Items;
using Terraria;
using Terraria.ID;

namespace RiskOfTerrain.Content.Accessories.Tier1;

public class RollOfPennies : ModAccessory {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        RORItem.WhiteTier.Add(Type);
    }

    public override void SetDefaults() {
        Item.width = 26;
        Item.height = 26;
        Item.accessory = true;
        Item.rare = ItemRarityID.Blue;
        Item.value = Item.sellPrice(silver: 50);
    }

    public override void OnHitBy(EntityInfo entity, EntityInfo attacker, Player.HurtInfo info) {
        if (entity.entity is not Player && (!Main.expertMode || entity.entity is not NPC))
            return;

        if (attacker.entity is Player player) {
            int[] coins = Utils.CoinsSplit(25 * info.Damage);
            var source = entity.entity.GetSource_FromThis();
            if (coins[0] > 0)
                player.QuickSpawnItem(source, ItemID.CopperCoin, coins[0]);

            if (coins[1] > 0)
                player.QuickSpawnItem(source, ItemID.SilverCoin, coins[1]);

            if (coins[2] > 0)
                player.QuickSpawnItem(source, ItemID.GoldCoin, coins[2]);

            if (coins[3] > 0)
                player.QuickSpawnItem(source, ItemID.PlatinumCoin, coins[3]);
        }
    }
}