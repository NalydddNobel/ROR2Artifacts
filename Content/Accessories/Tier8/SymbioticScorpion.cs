using Microsoft.Xna.Framework;
using RiskOfTerrain.Content.OnHitEffects;
using RiskOfTerrain.Items;
using Terraria;
using Terraria.ID;

namespace RiskOfTerrain.Content.Accessories.Tier8;

public class SymbioticScorpion : ModAccessory {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        RORItem.RedTier.Add((Type, () => {
            return Main.hardMode;
        }

        ));
    }

    public override void SetDefaults() {
        Item.width = 42;
        Item.height = 40;
        Item.accessory = true;
        Item.rare = ItemRarityID.Yellow;
        Item.value = Item.sellPrice(gold: 5);
    }

    public override void OnHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, NPC.HitInfo hit) {
        if (victim.entity is NPC target) {
            target.ROR().scorpionCount++;
            CombatText.NewText(new Rectangle((int)target.Center.X, (int)target.Center.Y, 0, 0), Color.Brown, target.ROR().scorpionCount, dot: true);
            OnHitEffectSpawn.NewOnHitEffect(entity.entity, victim.entity, projOrItem, 1, true);
        }
    }
}