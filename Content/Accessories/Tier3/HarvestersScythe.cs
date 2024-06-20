using RiskOfTerrain.Items;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace RiskOfTerrain.Content.Accessories.Tier3;

public class HarvestersScythe : ModAccessory {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        RORItem.GreenTier.Add((Type, () => {
            return Main.hardMode;
        }

        ));
    }

    public override void SetDefaults() {
        Item.width = 20;
        Item.height = 20;
        Item.accessory = true;
        Item.rare = ItemRarityID.Orange;
        Item.value = Item.sellPrice(gold: 2);
    }

    public override void OnHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, NPC.HitInfo hit) {
        if (victim.entity is NPC npc2 && npc2.immortal)
            return;

        if (hit.Crit) {
            int heal = 12;
            if (entity.entity is Player player) {
                player.Heal(Math.Min(heal, Math.Max((int)player.lifeSteal, 1)));
                if (player.lifeSteal > 0f) {
                    player.lifeSteal = Math.Max(player.lifeSteal - Math.Clamp(player.statLifeMax2 - player.statLife, 0, heal), 0f);
                }
            }
            else if (entity.entity is NPC npc) {
                npc.life += Math.Clamp(npc.lifeMax - npc.life, 0, heal);
                npc.HealEffect(heal);
            }

            SoundEngine.PlaySound(RiskOfTerrain.GetSounds("healscythe/heal", 5, 0.1f, 0f, 0.1f), entity.entity.Center);
        }
    }
}