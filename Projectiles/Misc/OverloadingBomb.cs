﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Projectiles.Misc
{
    public class OverloadingBomb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 8;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 0;
            Projectile.knockBack = 0f;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.hostile = true;
            Projectile.width = 62;
            Projectile.height = 62;
            Projectile.alpha = 125;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(52, 210, 235, 230);
        }

        public int explodeCooldown = 90;
        public Vector2 positionOffset;

        public override void OnSpawn(IEntitySource source)
        {
            if (Projectile.ai[1] == 1)
            {
                positionOffset = Projectile.Center - Main.player[(int)Projectile.ai[0]].Center;
            }
            else if (Projectile.ai[1] == 2)
            {
                positionOffset = Projectile.Center - Main.npc[(int)Projectile.ai[0]].Center;
            }
        }

        public override void AI()
        {
            if (explodeCooldown > 0)
            {
                explodeCooldown--;
            }
            else
            {
                Projectile.frameCounter++;
                if (Projectile.frameCounter > 2)
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame++;
                    if (Projectile.frame == 7)
                    {
                        Projectile.damage = 50;
                    }
                    if (Projectile.frame >= Main.projFrames[Type])
                    {
                        Projectile.Kill();
                    }
                }
            }

            if (Projectile.ai[1] == 1)
            {
                Projectile.Center = Main.player[(int)Projectile.ai[0]].Center + positionOffset;
            }
            else if (Projectile.ai[1] == 2)
            {
                Projectile.Center = Main.npc[(int)Projectile.ai[0]].Center + positionOffset;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.GetDrawInfo(out var texture, out var offset, out var frame, out var origin, out int _);
            Main.spriteBatch.Draw(texture, Projectile.position + offset - Main.screenPosition, frame, Projectile.GetAlpha(lightColor), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}