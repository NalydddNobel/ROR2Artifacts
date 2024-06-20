using Microsoft.Xna.Framework;
using RiskOfTerrain.Items;
using Terraria;
using Terraria.ID;

namespace RiskOfTerrain.Content.Accessories.Tier6;

public class RejuvenationRack : ModAccessory {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        RORItem.RedTier.Add((Type, () => {
            return Main.hardMode;
        }

        ));
    }

    public override void SetDefaults() {
        Item.width = 32;
        Item.height = 38;
        Item.accessory = true;
        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.sellPrice(gold: 5);
    }

    public int savedHealth;

    public override void OnEquip(EntityInfo entity) {
        if (entity.entity is Player player) {
            savedHealth = player.statLife;
        }
    }

    public bool beginningState = true;

    public override void UpdateAccessory(Player player, bool hideVisual) {
        if (!beginningState) {
            if (player.statLife > savedHealth) {
                player.statLife += (int)MathHelper.Min(player.statLife - savedHealth, player.statLifeMax2 - player.statLife);
            }
        }
        else {
            beginningState = false;
        }

        savedHealth = player.statLife;
    }
}
