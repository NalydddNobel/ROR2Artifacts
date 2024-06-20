using RiskOfTerrain.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Accessories.Tier1;

public class DelicateWatch : ModAccessory {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        RORItem.WhiteTier.Add((Type, () => {
            return NPC.downedBoss1;
        }

        ));
    }

    public override void SetDefaults() {
        Item.width = 28;
        Item.height = 24;
        Item.accessory = true;
        Item.rare = ItemRarityID.Blue;
        Item.value = Item.sellPrice(gold: 1);
    }

    public override void UpdateAccessory(Player player, bool hideVisual) {
        player.accWatch = 3;
        if (player.statLife > player.statLifeMax / 2) {
            player.GetDamage(DamageClass.Generic) += 0.1f;
        }
    }

    public override void AddRecipes() {
        CreateRecipe()
            .AddRecipeGroup(nameof(ItemID.CopperBar), 5)
            .AddRecipeGroup(RecipeGroupID.IronBar, 3)
            .AddTile(TileID.Chairs)
            .AddTile(TileID.Tables)
            .Register();
    }
}