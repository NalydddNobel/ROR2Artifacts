using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using RiskOfTerrain.Projectiles.Accessory.Damaging;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.CharacterSets.Commando
{
    public class CommandoGunWeapon : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 14;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Yellow;

            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;

            Item.noMelee = true;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 1;
            Item.knockBack = 1;

            Item.shootSpeed = 16f;
            Item.useAmmo = AmmoID.Bullet;
            Item.shoot = ProjectileID.PurificationPowder;
        }

        public int pierceCooldown = 180;

        public override void HoldItem(Player player)
        {
            if (pierceCooldown < 180)
            {
                pierceCooldown++;
            }
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (Main.mouseRight && pierceCooldown == 180)
            {
                type = ModContent.ProjectileType<DoubleTapPierce>();
                damage = 15;
                knockback = 10;
                pierceCooldown = 0;
            }
            else
            {
                damage = 2;
                knockback = 10;
            }
        }
    }
}