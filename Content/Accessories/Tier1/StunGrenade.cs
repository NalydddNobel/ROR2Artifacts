using Microsoft.Xna.Framework;
using RiskOfTerrain.Items;
using RiskOfTerrain.Projectiles.Accessory.Utility;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Accessories.Tier1;

public class StunGrenade : ModAccessory {
    public override void SetStaticDefaults() {
        RORItem.WhiteTier.Add(Type);
    }

    public override void SetDefaults() {
        Item.width = 38;
        Item.height = 42;
        Item.accessory = true;
        Item.rare = ItemRarityID.Blue;
        Item.value = Item.sellPrice(gold: 1);
    }

    public override void OnHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, NPC.HitInfo hit) {
        if (victim.entity is NPC npc && npc.immortal)
            return;

        entity.GetProc(out float proc);
        if (Main.rand.NextFloat(1f) <= proc && entity.RollLuck(10) == 0 && entity.entity is Player player) {
            Projectile.NewProjectile(player.GetSource_Accessory(Item), victim.entity.Center, Vector2.Zero, ModContent.ProjectileType<StunGrenadeProj>(),
                0, 0f, entity.GetProjectileOwnerID(), -10f);
        }
    }
}