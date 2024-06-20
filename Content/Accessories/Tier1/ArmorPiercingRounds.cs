using RiskOfTerrain.Items;
using RiskOfTerrain.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Accessories.Tier1;

public class ArmorPiercingRounds : ModAccessory {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        RORItem.WhiteTier.Add((Type, () => {
            return NPC.downedBoss2;
        }

        ));
    }

    public override void SetDefaults() {
        Item.width = 24;
        Item.height = 22;
        Item.accessory = true;
        Item.rare = ItemRarityID.Blue;
        Item.value = Item.sellPrice(gold: 1);
    }

    public override void ModifyHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, ref StatModifier damage, ref StatModifier knockBack, ref NPC.HitModifiers modifiers) {
        if (victim.entity is NPC target && (target.boss || RORNPC.CountsAsBoss.Contains(target.type)))
            modifiers.ScalingBonusDamage += 0.1f;

    }
}