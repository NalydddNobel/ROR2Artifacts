﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Buffs.Debuff;
using RiskOfTerrain.Projectiles.Misc;
using Terraria.Audio;

namespace RiskOfTerrain.Items.Accessories.T3Legendary
{
    public class BrilliantBehemoth : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
            RORItem.RedTier.Add((Type, () => Main.hardMode));
        }

        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 54;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 5);
        }

        public override void ModifyHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, ref int damage, ref float knockBack, ref bool crit)
        {
            int d;
            if (projOrItem is Item item)
            {
                d = (int)(item.damage * 0.05);
            }
            else if (projOrItem is Projectile projectile && projectile.type != ModContent.ProjectileType<BehemothExplosion>())
            {
                d = (int)(projectile.damage * 0.05);
            }
            else
            {
                d = (int)(damage * 0.05);
            }

            SoundEngine.PlaySound(RiskOfTerrain.GetSounds("behemoth/item_proc_behemoth_0", 4).WithVolumeScale(0.2f), entity.entity.Center);

            Projectile.NewProjectile(entity.entity.GetSource_FromThis(), victim.Center, Vector2.Zero, ModContent.ProjectileType<BehemothExplosion>(), d, 0, entity.GetProjectileOwnerID());
        }
    }
}