using RiskOfTerrain.Items;
using Terraria;
using Terraria.ID;

namespace RiskOfTerrain.Content.Accessories.Tier2;

public class RoseBuckler : ModAccessory {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        RORItem.GreenTier.Add((Type, () => {
            return NPC.downedBoss1;
        }

        ));
    }

    public override void SetDefaults() {
        Item.width = 24;
        Item.height = 24;
        Item.accessory = true;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.sellPrice(gold: 1, silver: 50);
    }

    public override void UpdateAccessory(Player player, bool hideVisual) {
        if (player.ROR().Sprinting) {
            player.statDefense += 30;
        }
    }
}