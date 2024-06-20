using Microsoft.Xna.Framework;
using RiskOfTerrain.Items;
using RiskOfTerrain.Projectiles.Accessory.Damaging;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Accessories.Tier7;

public class FrostRelic : ModAccessory {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        RORItem.RedTier.Add(Type);
    }

    public override void SetDefaults() {
        Item.width = 32;
        Item.height = 38;
        Item.accessory = true;
        Item.rare = ItemRarityID.Lime;
        Item.value = Item.sellPrice(gold: 5);
    }

    public override void OnKillEnemy(EntityInfo entity, OnKillInfo info) {
        for (int i = 0; i < 3; i++) {
            Projectile.NewProjectile(entity.GetSource_Accessory(Item), entity.Center, Vector2.Zero, ModContent.ProjectileType<FrostRelicIcicle>(), 3, 0, entity.GetProjectileOwnerID(), i);
        }
    }
}