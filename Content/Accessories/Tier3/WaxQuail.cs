using RiskOfTerrain.Items;
using Terraria;
using Terraria.ID;

namespace RiskOfTerrain.Content.Accessories.Tier3;

public class WaxQuail : ModAccessory {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        RORItem.GreenTier.Add((Type, () => {
            return NPC.downedBoss2;
        }

        ));
    }

    public override void SetDefaults() {
        Item.width = 24;
        Item.height = 24;
        Item.accessory = true;
        Item.rare = ItemRarityID.Orange;
        Item.value = Item.sellPrice(gold: 2);
    }

    public override void UpdateAccessory(Player player, bool hideVisual) {
        player.ROR().accWaxQuail = true;
    }

    public override void AddRecipes() {
        CreateRecipe()
            .AddIngredient(ItemID.BeeWax, 15)
            .AddTile(TileID.GlassKiln)
            .Register();
    }
}