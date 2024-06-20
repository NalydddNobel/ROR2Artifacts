using Terraria;
using Terraria.ModLoader;

namespace RiskOfTerrain.NPCs;

public class MalachiteUrchin : ModNPC {
    public override bool IsLoadingEnabled(Mod mod) {
        return false;
    }

    public override void SetDefaults() {
        NPC.lifeMax = 5;
    }

    public override void AI() {

    }

    public bool DrawingElite;

    //public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    //{
    //    ArmorShaderData shader = GameShaders.Armor.GetShaderFromItemId(ItemID.GreenandBlackDye);
    //    Main.spriteBatch.End();
    //    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.Default, Main.Rasterizer, null, Main.Transform);
    //    shader.Apply(NPC);
    //    DrawingElite = true;
    //    return true;
    //}

    //public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    //{
    //    if (DrawingElite)
    //    {
    //        Main.spriteBatch.End();
    //        Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.Default, Main.Rasterizer, null, Main.Transform);
    //        DrawingElite = false;
    //    }
    //}
}