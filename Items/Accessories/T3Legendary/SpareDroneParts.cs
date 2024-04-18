using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using RiskOfTerrain.Content.Accessories;
using RiskOfTerrain.Buffs.Debuff;
using Terraria.DataStructures;
using RiskOfTerrain.NPCs;
using RiskOfTerrain.Projectiles.Accessory.Damaging.Droneman;

namespace RiskOfTerrain.Items.Accessories.T3Legendary
{
    public class SpareDroneParts : ModAccessory
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            RORItem.RedTier.Add((Type, () => NPC.downedMartians));
        }

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 32;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 5);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!hideVisual)
            {
                player.ROR().dronemanOut = true;

                bool spawnHook = true;
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    if (Main.projectile[i].type == ModContent.ProjectileType<DronemanHead>() && Main.projectile[i].owner == player.whoAmI && Main.projectile[i].active)
                    {
                        spawnHook = false; break;
                    }
                }

                if (spawnHook) { Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<DronemanHead>(), 0, 0, Owner: player.whoAmI); }
            }

            player.GetDamage<SummonDamageClass>() += 0.25f;
        }
    }
}