using RiskOfTerrain.Items;
using Terraria;
using Terraria.ID;

namespace RiskOfTerrain.Content.Accessories.Tier4;

public class IgnitionTank : ModAccessory {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        RORItem.GreenTier.Add((Type, () => {
            return Main.hardMode;
        }

        ));
    }

    public override void SetDefaults() {
        Item.width = 24;
        Item.height = 24;
        Item.accessory = true;
        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.sellPrice(gold: 1, silver: 50);
    }

    public override void UpdateAccessory(Player player, bool hideVisual) {
        player.ROR().accIgnitionTank = true;
        //check RORNPC - OnSpawn, PostAI, ModifyHit for the rest of the implementation
    }
}