using RiskOfTerrain.Buffs.Debuff;
using RiskOfTerrain.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Accessories.Tier6;

public class ShatteringJustice : ModAccessory {
    public override void SetStaticDefaults() {
        RORItem.RedTier.Add((Type, () => {
            return Main.hardMode;
        }

        ));
    }

    public override void SetDefaults() {
        Item.width = 54;
        Item.height = 54;
        Item.accessory = true;
        Item.rare = ItemRarityID.LightPurple;
        Item.value = Item.sellPrice(gold: 5);
    }

    public override void ModifyHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, ref StatModifier damage, ref StatModifier knockBack, ref NPC.HitModifiers modifiers) {
        if (victim.entity is NPC target && target.ROR().shatterizationCount < 3 && target.ROR().timeSinceLastHit < 60) {
            target.ROR().shatterizationCount++;
            //target.netUpdate = true;
            target.AddBuff(ModContent.BuffType<ShatteredDebuff>(), 10800);
        }
    }
}