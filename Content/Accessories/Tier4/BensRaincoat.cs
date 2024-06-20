using RiskOfTerrain.Buffs;
using RiskOfTerrain.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Accessories.Tier6;

public class BensRaincoat : ModAccessory {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        RORItem.RedTier.Add((Type, () => {
            return NPC.downedBoss2;
        }

        ));
    }

    public override void SetDefaults() {
        Item.width = 32;
        Item.height = 38;
        Item.accessory = true;
        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.sellPrice(gold: 5);
    }

    int savedBuffCount = 0;

    public override void UpdateAccessory(Player player, bool hideVisual) {
        if (savedBuffCount < player.CountBuffs()) {
            if (Main.debuff[player.buffType[savedBuffCount]] && !player.HasBuff(ModContent.BuffType<BensRaincoatBuff>()) && player.buffType[savedBuffCount] != BuffID.PotionSickness) {
                player.DelBuff(savedBuffCount);
                player.AddBuff(ModContent.BuffType<BensRaincoatBuff>(), 1200);

                var ror = player.ROR();
                ror.barrierLife += 50;
                if (ror.barrierLife > player.statLifeMax2) {
                    ror.barrierLife = player.statLifeMax2;
                }
                player.statLife += 50;
            }
        }

        savedBuffCount = player.CountBuffs();
    }
}
