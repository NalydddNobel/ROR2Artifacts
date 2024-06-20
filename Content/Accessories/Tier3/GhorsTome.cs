using Microsoft.Xna.Framework;
using RiskOfTerrain.Items;
using RiskOfTerrain.Projectiles.Accessory.Utility;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Accessories.Tier3;

public class GhorsTome : ModAccessory {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        RORItem.GreenTier.Add(Type);
    }

    public override void SetDefaults() {
        Item.width = 26;
        Item.height = 24;
        Item.accessory = true;
        Item.rare = ItemRarityID.Orange;
        Item.value = Item.sellPrice(gold: 2);
    }

    public override void OnKillEnemy(EntityInfo entity, OnKillInfo info) {
        if (info.value > 0 && entity.RollLuck(10) == 0 && entity.IsMe()) {
            Projectile.NewProjectile(entity.GetSource_Accessory(Item), info.Center, new Vector2(0f, -1f),
                ModContent.ProjectileType<GhorsTomeProj>(), 0, 0, entity.GetProjectileOwnerID(), ai1: info.value);
        }
    }
}