using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RiskOfTerrain.Buffs;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.NPCs;
using RiskOfTerrain.Projectiles.Elite;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Elites.Aspects
{
    public class CelestineAspect : GenericAspect
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Cyan;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CelestineUpdate(player);
        }

        public static void CelestineUpdate(Player player)
        {
            player.ROR().aspCelestine = true;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].friendly)
                {
                    bool canBeInvised = true;
                    Main.npc[i].GetElitePrefixes(out var prefixes);
                    foreach (var p in prefixes)
                    {
                        if (p.Prefix == Language.GetTextValue("Mods.RiskOfTerrain.CelestineElite"))
                        {
                            canBeInvised = false;
                        }
                    }
                    if (player.Distance(Main.npc[i].Hitbox.ClosestDistance(player.Center)) < 240f && canBeInvised)
                    {
                        Main.npc[i].AddBuff(ModContent.BuffType<CelestineInvis>(), 2);
                    }
                }
            }

            for (int j = 0; j < Main.maxPlayers; j++)
            {
                if (Main.player[j].active && Main.player[j].team == player.team)
                {
                    if (player.Distance(Main.player[j].Hitbox.ClosestDistance(player.Center)) < 240f && player.whoAmI != Main.player[j].whoAmI)
                    {
                        Main.player[j].AddBuff(BuffID.Invisibility, 2);
                    }
                }
            }
        }

        public override void OnHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, NPC.HitInfo hit)
        {
            CelestineOnHit(victim);
        }

        public static void CelestineOnHit(EntityInfo victim)
        {
            if (victim.entity is Player player)
            {
                player.AddBuff(ModContent.BuffType<CelestineSlow>(), 180);
            }

            if (victim.entity is NPC npc)
            {
                npc.AddBuff(ModContent.BuffType<CelestineSlow>(), 180);
            }
        }

        public override void PostUpdate(EntityInfo entity)
        {
            CelestinePostUpdate(entity, Item);
        }

        public static void CelestinePostUpdate(EntityInfo entity, Item item)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].type == ModContent.ProjectileType<CelestineProj>() && entity.OwnsThisProjectile(Main.projectile[i]))
                {
                    UpdateProjectile(entity, Main.projectile[i]);
                    return;
                }
            }
            if (entity.entity is Player player)
            {
                UpdateProjectile(entity, Projectile.NewProjectileDirect(player.GetSource_Accessory(item), entity.entity.Center, Vector2.Zero,
                ModContent.ProjectileType<CelestineProj>(), 0, 0f, entity.GetProjectileOwnerID(), entity.GetProjectileOwnerID(), 1));
            }
        }

        public static void UpdateProjectile(EntityInfo entity, Projectile projectile)
        {
            projectile.scale = MathHelper.Lerp(projectile.scale, 480f, 0.2f);
            projectile.Center = entity.entity.Center;
            projectile.timeLeft = 2;
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            Lighting.AddLight(Item.Center, new Vector3(1f, 0.95f, 0.85f));
        }
    }
}