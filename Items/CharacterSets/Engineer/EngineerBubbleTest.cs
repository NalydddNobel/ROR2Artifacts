using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.CharacterSets.Engineer
{
    public class EngineerBubbleTest : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Yellow;

            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = false;
            Item.noUseGraphic = true;
            Item.UseSound = SoundID.Item1;

            Item.noMelee = true;
            Item.DamageType = DamageClass.Summon;
            Item.sentry = true;
            Item.mana = 8;

            Item.shoot = ModContent.ProjectileType<EngineerBubble>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.UpdateMaxTurrets();
            return true;
        }
    }

    public class EngineerBubble : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 168;
            Projectile.height = 168;
        }

        public override void AI()
        {
            for (int i = 0; i < Main.maxProjectiles; i++) 
            { 
                if (Projectile.Hitbox.Intersects(Main.projectile[i].Hitbox))
                {
                    Projectile p = Main.projectile[i];
                    float r = Projectile.Center.Distance(p.Center);
                    float y = p.Center.Y - Projectile.Center.Y;

                    p.rotation = (float)Math.Acos(y / r) + (float)Math.Atan(p.velocity.Y / p.velocity.X);
                }
            }
        }
    }
}