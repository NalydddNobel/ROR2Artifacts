using Microsoft.Xna.Framework;
using RiskOfTerrain.Content.Elites;
using RiskOfTerrain.Items;
using RiskOfTerrain.NPCs;
using RiskOfTerrain.Projectiles.Accessory.Damaging;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Accessories.Tier6;

public class SoulboundCatalyst : ModAccessory {
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

    public int soulIndex = 0;
    public int savedLife;

    public override void UpdateAccessory(Player player, bool hideVisual) {
        player.ROR().boundSoulRotTick++;
        player.ROR().releaseTheGhosts = false;

        if (savedLife > player.statLife) {
            player.ROR().releaseTheGhosts = true;
            player.ROR().boundSoulCount = 0;
            soulIndex = 0;
        }

        savedLife = player.statLife;
    }

    public override void OnUnequip(EntityInfo entity) {
        if (entity.entity is Player player) {
            player.ROR().releaseTheGhosts = true;
            player.ROR().boundSoulCount = 0;
            soulIndex = 0;
        }
    }

    public override void OnKillEnemy(EntityInfo entity, OnKillInfo info) {
        if (!info.friendly && info.lifeMax > 5 && !info.spawnedFromStatue && entity.entity is Player player && info.lastHitProjectileType != ModContent.ProjectileType<BoundSoul>()) {
            int elite = 0;
            int boss = 0;
            NPC killed = Main.npc[info.whoAmI];

            if (killed.boss) {
                boss = 1;
            }

            NPC npc = Main.npc[info.whoAmI];

            for (int i = 0; i < RORNPC.RegisteredElites.Count; i++) {
                var l = new List<EliteNPCBase>(RORNPC.RegisteredElites);
                if (npc.GetGlobalNPC(l[i]).Active == true) {
                    elite = i + 1;
                }
            }

            Projectile.NewProjectile(entity.GetSource_Accessory(Item), info.Center, Vector2.Zero, ModContent.ProjectileType<BoundSoul>(), 0, 0, entity.GetProjectileOwnerID(), soulIndex, boss, elite);
            player.ROR().boundSoulCount++;
            soulIndex++;
        }
    }
}
