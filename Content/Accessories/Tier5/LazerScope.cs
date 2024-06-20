using RiskOfTerrain.Content.OnHitEffects;
using RiskOfTerrain.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Accessories.Tier5;

public class LazerScope : ModAccessory {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        RORItem.RedTier.Add(Type);
    }

    public override void SetDefaults() {
        Item.width = 32;
        Item.height = 38;
        Item.accessory = true;
        Item.rare = ItemRarityID.Pink;
        Item.value = Item.sellPrice(gold: 5);
    }

    public override void ModifyHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, ref StatModifier damage, ref StatModifier knockBack, ref NPC.HitModifiers modifiers) {
        modifiers.CritDamage += 1f;
    }

    public override void OnHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, NPC.HitInfo hit) {
        if (hit.Crit) {
            OnHitEffectSpawn.NewOnHitEffect(entity.entity, victim.entity, projOrItem, 0, false);
        }
    }
}