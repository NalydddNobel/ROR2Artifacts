using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.NPCs;
using RiskOfTerrain.Projectiles.Misc;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.Accessories.Aspects
{
    public class OverloadingAspect : GenericAspect
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Blue;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 /= 2;
            player.ROR().maxShield += 0.5f;
        }

        public override void OnHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, int damage, float knockBack, bool crit)
        {
            if (victim.entity is NPC npc && entity.entity is Player player && projOrItem is Projectile projectile && projectile.type != ModContent.ProjectileType<OverloadingBomb>() && !projectile.ROR().spawnedFromElite && !npc.friendly)
            {
                int p = Projectile.NewProjectile(player.GetSource_FromThis(), npc.Center + new Vector2(Main.rand.Next(0, 16), Main.rand.Next(0, 16)), Vector2.Zero, ModContent.ProjectileType<OverloadingBomb>(), 0, 0, Owner: player.whoAmI, ai0: npc.whoAmI, ai1: 2);
                Main.projectile[p].ROR().spawnedFromElite = true;
                Main.projectile[p].friendly = true;
                Main.projectile[p].hostile = false;
            }
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            Lighting.AddLight(Item.Center, TorchID.Blue);
        }
    }
}