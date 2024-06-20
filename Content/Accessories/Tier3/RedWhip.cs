﻿using RiskOfTerrain.Items;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace RiskOfTerrain.Content.Accessories.Tier3;

public class RedWhip : ModAccessory {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        RORItem.GreenTier.Add(Type);
    }

    public override void SetDefaults() {
        Item.width = 24;
        Item.height = 24;
        Item.accessory = true;
        Item.rare = ItemRarityID.Orange;
        Item.value = Item.sellPrice(gold: 2);
    }

    public int whipActive;
    public bool hideVisual;

    public override void UpdateAccessory(Player player, bool hideVisual) {
        this.hideVisual = hideVisual;
    }

    //ripped right from cautious slug mf
    public override void UpdateLifeRegeneration(EntityInfo entity) {
        if (whipActive == 120) {
            if (Main.netMode != NetmodeID.Server) {
                SoundEngine.PlaySound(RiskOfTerrain.GetSounds("whip/item_proc_whip_", 4).WithVolumeScale(0.4f), entity.entity.Center);
            }
        }

        if (whipActive > 120) {
            if (entity.InDanger()) {
                whipActive = 0;
            }

            if (entity.entity is Player player) {
                player.moveSpeed *= 1.3f;
                player.maxRunSpeed *= 1.3f;
            }

            return;
        }

        if (!entity.InDanger()) {
            whipActive++;
        }
    }
}