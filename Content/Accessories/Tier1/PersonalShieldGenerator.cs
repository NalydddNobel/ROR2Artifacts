﻿using RiskOfTerrain.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Accessories.Tier1;

[AutoloadEquip(EquipType.Neck)]
public class PersonalShieldGenerator : ModAccessory {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        RORItem.WhiteTier.Add(Type);
    }

    public override void SetDefaults() {
        Item.width = 24;
        Item.height = 22;
        Item.accessory = true;
        Item.rare = ItemRarityID.Blue;
        Item.value = Item.sellPrice(gold: 1);
    }

    public override void UpdateAccessory(Player player, bool hideVisual) {
        player.ROR().maxShield += 0.12f;
    }
}