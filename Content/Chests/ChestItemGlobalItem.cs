using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Chests;
public class ChestItemGlobalItem : GlobalItem {
    public bool FromChest { get; set; }

    public override bool InstancePerEntity => true;

    public override void SetDefaults(Item entity) {
        FromChest = false;
    }

    public override void Update(Item item, ref float gravity, ref float maxFallSpeed) {
        if (FromChest) {
            gravity = 0f;

            bool touching = false;
            for (int i = 0; i < Main.maxItems; i++) {
                Item otherItem = Main.item[i];
                if (otherItem.active && otherItem.position != item.position && otherItem.Hitbox.Intersects(item.Hitbox) && otherItem.TryGetGlobalItem(out ChestItemGlobalItem other) && other.FromChest) {
                    item.velocity.X += item.DirectionFrom(otherItem.Center).X * 0.5f;
                    touching = true;
                }
            }

            if (!touching) {
                item.velocity *= 0.9f;
            }

            if (item.position.HasNaNs()) {
                item.active = false;
            }
        }
    }

    public override bool PreDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI) {
        if (!FromChest) {
            return true;
        }

        ArmorShaderData colorOnly = GameShaders.Armor.GetSecondaryShader(ContentSamples.CommonlyUsedContentSamples.ColorOnlyShaderIndex, Main.LocalPlayer);
        Main.GetItemDrawFrame(item.type, out Texture2D texture, out Rectangle frame);
        Vector2 drawCoordiantes = item.Center + new Vector2(0f, (int)(MathF.Sin(Main.GlobalTimeWrappedHourly + whoAmI) * 4f)) - Main.screenPosition;
        Vector2 origin = frame.Size() / 2f;

        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.Default, Main.Rasterizer, null, Main.Transform);
        colorOnly.UseColor(ItemRarity.GetColor(item.rare));
        colorOnly.Apply();

        for (float f = 0f; f < MathHelper.TwoPi; f += MathHelper.PiOver2 + float.Epsilon) {
            Vector2 offset = (f + Main.GlobalTimeWrappedHourly).ToRotationVector2() * (MathF.Sin(Main.GlobalTimeWrappedHourly * 0.345f) * 2f + 4f);
            spriteBatch.Draw(texture, drawCoordiantes + offset, frame, ItemRarity.GetColor(item.OriginalRarity) * 0.5f, rotation, origin, scale, SpriteEffects.None, 0f);
        }

        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.Default, Main.Rasterizer, null, Main.Transform);

        spriteBatch.Draw(texture, drawCoordiantes, frame, Color.White, rotation, origin, scale, SpriteEffects.None, 0f);

        return false;
    }
}
