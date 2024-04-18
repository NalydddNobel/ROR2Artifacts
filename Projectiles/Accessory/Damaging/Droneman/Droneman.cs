using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Projectiles.Accessory.Damaging.Droneman
{
    public class DronemanHead : ModProjectile
    {
        public int gunDirection;
        public int closest = -1;
        public int gunIndex;
        public static Asset<Texture2D> glowmask;

        public override void Load()
        {
            glowmask = ModContent.Request<Texture2D>("RiskOfTerrain/Projectiles/Accessory/Damaging/Droneman/DronemanHead-glow");
        }

        public override void Unload()
        {
            glowmask = null;
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 102;
            Projectile.height = 52;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            gunIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<DronemanGun>(), 0, 0, Projectile.owner, Projectile.whoAmI);
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Projectile.timeLeft = 2;

            if (!Main.projectile[gunIndex].active)
            {
                gunIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<DronemanGun>(), 0, 0, Projectile.owner, Projectile.whoAmI);
            }

            if (!player.ROR().dronemanOut)
            {
                Projectile.Kill();
            }

            if (closest != -1 && Main.npc[closest].active && Main.npc[closest] != null && Main.npc[closest].life > 0)
            {
                if (Main.npc[closest].direction == 1)
                {
                    Projectile.velocity = new Vector2(((Main.npc[closest].Center.X - 100) - Projectile.Center.X) / 100, ((Main.npc[closest].Center.Y - 50) - Projectile.Center.Y) / 100);
                }

                if (Main.npc[closest].direction == -1)
                {
                    Projectile.velocity = new Vector2(((Main.npc[closest].Center.X + 100) - Projectile.Center.X) / 100, ((Main.npc[closest].Center.Y - 50) - Projectile.Center.Y) / 100);
                }

                Projectile.spriteDirection = Math.Sign(Projectile.velocity.X);

                if (Math.Abs(Projectile.velocity.X) < 1f)
                {
                    Projectile.spriteDirection = Main.npc[closest].direction;
                }

                int distanceToTarget = (int)(Main.npc[closest].Center.X - Projectile.Center.X);
                gunDirection = Math.Sign(distanceToTarget);

                Main.projectile[gunIndex].ai[1] = 0;
            }

            if (closest != -1 && (!Main.npc[closest].active || Main.npc[closest] == null || Main.npc[closest].life < 1))
            {
                closest = Projectile.FindTargetWithLineOfSight();
            }

            if (closest == -1)
            {
                closest = Projectile.FindTargetWithLineOfSight();

                if (player.direction == 1)
                {
                    Projectile.velocity = new Vector2(((player.Center.X - 100) - Projectile.Center.X) / 100, ((player.Center.Y - 50) - Projectile.Center.Y) / 100);
                }

                if (player.direction == -1)
                {
                    Projectile.velocity = new Vector2(((player.Center.X + 100) - Projectile.Center.X) / 100, ((player.Center.Y - 50) - Projectile.Center.Y) / 100);
                }

                Projectile.spriteDirection = Math.Sign(Projectile.velocity.X);

                if (Math.Abs(Projectile.velocity.X) < 1f)
                {
                    Projectile.spriteDirection = player.direction;
                }

                gunDirection = Projectile.spriteDirection;

                Main.projectile[gunIndex].ai[1] = 1;
            }

            Main.projectile[gunIndex].ai[2] = closest;

            // if no target
            // move to behind and above player

            // if target
            // move towards the opposite side of the target that youre currently on
            // direct the gun and handle to aim and shoot at the target

            //set rotation and wing position to a certain amount based on horizontal velocity

            Projectile.rotation = Math.Clamp(Projectile.velocity.X / 10, MathHelper.ToRadians(-15), MathHelper.ToRadians(15));

            if (gunDirection == Projectile.spriteDirection)
            {
                if (Math.Abs(Projectile.rotation) != MathHelper.ToRadians(15))
                {
                    Projectile.frame = 0;
                }
                else
                {
                    Projectile.frame = 1;
                }
            }
            else
            {
                if (Math.Abs(Projectile.rotation) != MathHelper.ToRadians(15))
                {
                    Projectile.frame = 2;
                }
                else
                {
                    Projectile.frame = 3;
                }
            }

            Main.projectile[gunIndex].spriteDirection = gunDirection;
        }

        public override void PostDraw(Color lightColor)
        {
            SpriteEffects fx;

            if (Projectile.spriteDirection == -1)
            {
                fx = SpriteEffects.FlipHorizontally;
            }
            else
            {
                fx = SpriteEffects.None;
            }

            Main.EntitySpriteDraw(glowmask.Value, Projectile.Center - Main.screenPosition, new Rectangle(0, 52 * Projectile.frame, Projectile.width, Projectile.height), Color.White, Projectile.rotation, new Vector2(Projectile.width / 2, Projectile.height / 2), 1f, fx);
        }

        public override void OnKill(int timeLeft)
        {
            Main.projectile[gunIndex].Kill();
        }
    }

    public class DronemanGun : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.width = 52;
            Projectile.height = 24;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = false;
            Projectile.hide = true;
        }

        public int tick;

        public override void AI()
        {
            Projectile.timeLeft = 2;

            Projectile droneman = Main.projectile[(int)Projectile.ai[0]];

            Projectile.Center = droneman.Center + new Vector2(0, 22).RotatedBy(droneman.rotation);

            if (Projectile.ai[2] == -1)
            {
                Projectile.rotation = droneman.rotation + MathHelper.ToRadians(15 * Projectile.spriteDirection);
            }
            else
            {
                float rotToEnemy = Vector2.Normalize(Main.npc[(int)Projectile.ai[2]].Center - Projectile.Center).ToRotation();
                float rotCopy = rotToEnemy;

                if (rotToEnemy < MathHelper.ToRadians(-90) || rotToEnemy > MathHelper.ToRadians(270))
                {
                    rotToEnemy += MathHelper.ToRadians(180);
                    Projectile.spriteDirection = -1;
                }
                else if (rotToEnemy > MathHelper.ToRadians(90) || rotToEnemy < MathHelper.ToRadians(-270))
                {
                    rotToEnemy -= MathHelper.ToRadians(180);
                    Projectile.spriteDirection = -1;
                }

                Projectile.rotation = MathHelper.Lerp(Projectile.rotation, rotToEnemy, 0.07f);

                if (tick % 8 == 0)
                {
                    int bullet = Projectile.NewProjectile(Main.player[Projectile.owner].GetSource_FromThis(), Projectile.Center, new Vector2(9, 0).RotatedBy(rotCopy), ProjectileID.Bullet, 40, 3, Projectile.owner);
                    Projectile b = Main.projectile[bullet];
                    b.friendly = true;
                    b.hostile = false;
                    b.DamageType = DamageClass.Summon;
                    b.tileCollide = false;
                    b.damage = (int)Main.player[Projectile.owner].GetTotalDamage(DamageClass.Summon).ApplyTo(90);
                    b.ArmorPenetration = 5;

                    if (Projectile.frame == 0)
                    {
                        Projectile.frame = 1;
                    }
                    else
                    {
                        Projectile.frame = 0;
                    }

                    SoundEngine.PlaySound(SoundID.Item11.WithVolumeScale(0.3f), Projectile.Center);
                }
                tick++;
            }
        }

        //102, 52
        // 51, 26

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindProjectiles.Add(Projectile.whoAmI);
        }
    }
}