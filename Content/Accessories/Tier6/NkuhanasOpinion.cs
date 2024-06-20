using Microsoft.Xna.Framework;
using RiskOfTerrain.Items;
using RiskOfTerrain.Projectiles.Accessory.Damaging;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Accessories.Tier6;

public class NkuhanasOpinion : ModAccessory {
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
        Item.rare = ItemRarityID.LightPurple;
        Item.value = Item.sellPrice(gold: 5);
    }

    public int releaseTimer = 60;
    public int playerStoredHP;

    public override void UpdateAccessory(Player player, bool hideVisual) {
        if (releaseTimer > 0) {
            releaseTimer--;
        }
        else {
            if (player.statLife > playerStoredHP) {
                int diff = player.statLife - playerStoredHP;
                Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, new Vector2(0, -5), ModContent.ProjectileType<NkuhanaSkull>(), diff, 2f, Owner: player.whoAmI);
            }
            releaseTimer = 60;
            playerStoredHP = player.statLife;
        }
    }

    public override void OnEquip(EntityInfo entity) {
        if (entity.entity is Player player) {
            playerStoredHP = player.statLife;
        }
    }
}
