using Microsoft.Xna.Framework;
using RiskOfTerrain.Items;
using RiskOfTerrain.Projectiles.Accessory.Damaging;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Accessories.Tier1;

public class BundleOfFireworks : ModAccessory {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        RORItem.WhiteTier.Add(Type);
    }

    public override void SetDefaults() {
        Item.width = 24;
        Item.height = 38;
        Item.accessory = true;
        Item.rare = ItemRarityID.Blue;
        Item.value = Item.sellPrice(gold: 1);
    }

    public override void OnKillEnemy(EntityInfo entity, OnKillInfo info) {
        if (entity.IsMe() && entity.OwnedProjectilesCountLong(ModContent.ProjectileType<FireworksSpawner>()) <= 0 && !info.friendly && !info.spawnedFromStatue && info.lifeMax > 5 && entity.entity is Player player) {
            Projectile.NewProjectile(player.GetSource_Accessory(Item), entity.entity.Center, Vector2.Zero, ModContent.ProjectileType<FireworksSpawner>(),
                Math.Clamp((int)(info.lastHitDamage * (Stacks * 0.5f)), 10, 100) / 8, 0, entity.GetProjectileOwnerID(), 8f + 4f * (Stacks - 1));
        }
    }
}