﻿using Microsoft.Xna.Framework;
using RiskOfTerrain.Items;
using RiskOfTerrain.Projectiles.Accessory.Damaging;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Accessories.Tier2;

public class StickyExplosives : ModAccessory {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        RORItem.WhiteTier.Add((Type, () => {
            return NPC.downedBoss1;
        }

        ));
    }

    public override void SetDefaults() {
        Item.width = 26;
        Item.height = 26;
        Item.accessory = true;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.sellPrice(gold: 1);
    }

    public override void OnHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, NPC.HitInfo hit) {
        if (victim.entity is NPC npc && !npc.immortal && entity.entity is Player player) {
            entity.GetProc(out float proc);
            if (Main.rand.NextFloat(1f) <= proc && entity.RollLuck(10) == 0) {
                Projectile.NewProjectile(player.GetSource_Accessory(Item), entity.entity.Center + Main.rand.NextVector2Unit() * 100f, Vector2.Zero, ModContent.ProjectileType<StickyExplosivesProj>(),
                    (int)(hit.Damage * 0.75f * proc), 0f, entity.GetProjectileOwnerID(), npc.whoAmI);
            }
        }
    }
}