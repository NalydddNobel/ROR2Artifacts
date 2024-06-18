using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Chests;

public class ChestItemProjectile : ModProjectile {
    public override string Texture => RiskOfTerrain.BlankTexture;

    public override void SetDefaults() {
        ProjectileID.Sets.TrailCacheLength[Type] = 20;
        ProjectileID.Sets.TrailingMode[Type] = 2;
        Projectile.width = 12;
        Projectile.height = 40;
        Projectile.ignoreWater = true;
        Projectile.tileCollide = false;
        Projectile.extraUpdates = 1;
        Projectile.timeLeft = 70 * Projectile.MaxUpdates;
    }

    public override void AI() {
        if (Projectile.localAI[0] == 0f && Projectile.velocity.Y == 0f) {
            Projectile.ai[0] = Main.rand.Next(ItemLoader.ItemCount);
            Projectile.velocity.Y = -4f;
            Projectile.velocity.X = Main.rand.NextFloat(0.5f, 1.5f) * (Main.rand.NextBool() ? -1 : 1);
            Projectile.netUpdate = true;
        }

        Projectile.localAI[0] = 1f;
        Projectile.velocity.X *= 0.98f;
        Projectile.velocity.Y += 0.1f;
        if (Projectile.velocity.Y > 0.4f) {
            Projectile.tileCollide = true;
        }

        Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.AncientLight, Projectile.velocity * 0.01f);
        d.noGravity = true;
        d.color = ItemRarity.GetColor(ContentSamples.ItemsByType[(int)Projectile.ai[0]].rare) with { A = 0 };
    }

    public override void OnKill(int timeLeft) {
        for (int i = 0; i < 140; i++) {
            Dust d = Dust.NewDustDirect(Projectile.position + Main.rand.NextVector2Circular(14f, 14f), Projectile.width, Projectile.height, DustID.TintableDustLighted);
            d.noGravity = true;
            d.alpha = 200;
            d.scale *= Main.rand.NextFloat(0.4f, 1.5f);
            d.fadeIn = d.scale + Main.rand.NextFloat(0.3f);
            d.velocity = Main.rand.NextVector2Circular(5f, 5f);
            d.color = ItemRarity.GetColor(ContentSamples.ItemsByType[(int)Projectile.ai[0]].rare) with { A = 0 };
        }

        if (Main.netMode != NetmodeID.MultiplayerClient) {
            int newItem = Item.NewItem(Projectile.GetSource_FromThis(), Projectile.Center, new Item((int)Projectile.ai[0]));
            Item item = Main.item[newItem];
            item.velocity = new Vector2(0f, -1f);
            item.GetGlobalItem<ChestItemGlobalItem>().FromChest = true;

            // TODO -- net sync
        }
    }

    public override bool PreDraw(ref Color lightColor) {
        Main.instance.LoadProjectile(ProjectileID.StardustTowerMark);
        Texture2D texture = TextureAssets.Projectile[ProjectileID.StardustTowerMark].Value;

        Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, ItemRarity.GetColor(ContentSamples.ItemsByType[(int)Projectile.ai[0]].rare), 0f, texture.Size() / 2f, Projectile.scale * 0.3f, SpriteEffects.None, 0f);
        return false;
    }
}
