﻿using RiskOfTerrain.Buffs;
using RiskOfTerrain.Items;
using Terraria;
using Terraria.ID;

namespace RiskOfTerrain.Content.Accessories.Tier2;

public class HuntersHarpoon : ModAccessory {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        RORItem.GreenTier.Add(Type);
    }

    public override void SetDefaults() {
        Item.width = 24;
        Item.height = 24;
        Item.accessory = true;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.sellPrice(gold: 2);
    }

    public override void OnKillEnemy(EntityInfo entity, OnKillInfo info) {
        HuntersHarpoonBuff.AddStack(entity.entity, 120, 1);
    }
}