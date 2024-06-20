﻿using RiskOfTerrain.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Accessories.Tier3;

public class LeechingSeed : ModAccessory {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        RORItem.GreenTier.Add((Type, () => {
            return NPC.downedBoss2;
        }

        ));
    }

    public override void SetDefaults() {
        Item.width = 24;
        Item.height = 24;
        Item.accessory = true;
        Item.rare = ItemRarityID.Orange;
        Item.value = Item.sellPrice(gold: 2);
    }

    public override void ModifyHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, ref StatModifier damage, ref StatModifier knockBack, ref NPC.HitModifiers modifiers) {
        if (entity.entity is NPC npc && npc.active && npc.life < npc.lifeMax) {
            npc.life++;
        }
        else if (entity.entity is Player player && player.active && player.statLife < player.statLifeMax && victim.entity is NPC victimNPC && !victimNPC.immortal && !victimNPC.CountsAsACritter && !victimNPC.SpawnedFromStatue) {
            player.statLife++;
        }
    }
}