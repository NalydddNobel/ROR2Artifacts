using RiskOfTerrain.Buffs;
using RiskOfTerrain.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Accessories.Tier3;

public class PredatoryInstincts : ModAccessory {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        RORItem.GreenTier.Add((Type, () => {
            return Main.hardMode;
        }

        ));
    }

    public override void SetDefaults() {
        Item.width = 24;
        Item.height = 24;
        Item.accessory = true;
        Item.rare = ItemRarityID.Orange;
        Item.value = Item.sellPrice(gold: 1, silver: 50);
    }

    public override void UpdateAccessory(Player player, bool hideVisual) {
        player.GetCritChance(DamageClass.Generic) += 5;
    }

    public override void OnHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, NPC.HitInfo hit) {
        if (hit.Crit) {
            entity.AddBuff(ModContent.BuffType<PredatoryInstinctsBuff>(), 300);
        }
    }
}