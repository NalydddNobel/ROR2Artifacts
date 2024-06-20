using Microsoft.Xna.Framework;
using RiskOfTerrain.Items;
using RiskOfTerrain.Projectiles.Accessory.Damaging;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Accessories.Tier4;

public class AtGMissileMk1 : ModAccessory {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        RORItem.GreenTier.Add((Type, () => {
            return NPC.downedMechBossAny;
        }

        ));
    }

    public override void SetDefaults() {
        Item.width = 26;
        Item.height = 24;
        Item.accessory = true;
        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.sellPrice(gold: 2, silver: 50);
    }

    public override void OnHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, NPC.HitInfo hit) {
        if (victim.entity is NPC npc && npc.immortal)
            return;

        entity.GetProc(out float proc);
        if (Main.rand.NextFloat(1f) <= proc && entity.RollLuck(10) == 0 && entity.entity is Player player) {
            Projectile.NewProjectile(player.GetSource_Accessory(Item), entity.entity.Center, new Vector2(0f, -12f).RotatedBy(Main.rand.NextFloat(-0.1f, 0.1f)), ModContent.ProjectileType<AtGMissileProj>(),
                (int)(hit.Damage * proc), 0f, entity.GetProjectileOwnerID(), -10f);
        }
    }
}