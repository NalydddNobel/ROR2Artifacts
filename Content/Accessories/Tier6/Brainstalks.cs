using RiskOfTerrain.Buffs;
using RiskOfTerrain.Content.Elites;
using RiskOfTerrain.Items;
using RiskOfTerrain.NPCs;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Accessories.Tier6;

public class Brainstalks : ModAccessory {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        RORItem.RedTier.Add((Type, () => {
            return Main.hardMode;
        }

        ));
    }

    public override void SetDefaults() {
        Item.width = 32;
        Item.height = 38;
        Item.accessory = true;
        Item.rare = ItemRarityID.LightPurple;
        Item.value = Item.sellPrice(gold: 5);
    }

    public override void OnKillEnemy(EntityInfo entity, OnKillInfo info) {
        NPC npc = Main.npc[info.whoAmI];

        for (int i = 0; i < RORNPC.RegisteredElites.Count; i++) {
            var l = new List<EliteNPCBase>(RORNPC.RegisteredElites);
            if (npc.GetGlobalNPC(l[i]).Active == true) {
                entity.AddBuff(ModContent.BuffType<BrainstalksBuff>(), 600);
            }
        }
    }
}
