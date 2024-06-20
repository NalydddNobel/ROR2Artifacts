﻿using RiskOfTerrain.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Accessories.Tier5;

[AutoloadEquip(EquipType.HandsOff)]
public class RunaldsBand : ModAccessory {
    public override void SetStaticDefaults() {
        RORItem.GreenTier.Add((Type, () => {
            return Main.hardMode;
        }

        ));
    }

    public override void SetDefaults() {
        Item.width = 24;
        Item.height = 24;
        Item.accessory = true;
        Item.rare = ItemRarityID.Pink;
        Item.value = Item.sellPrice(gold: 3);
    }

    public static int procCooldown;

    public override void UpdateAccessory(Player player, bool hideVisual) {
        if (procCooldown < 600)
            procCooldown++;

        player.ROR().accRunalds = true;
    }
}