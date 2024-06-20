﻿using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace RiskOfTerrain.Buffs;

public class MedkitBuff : ModBuff {
    public override void SetStaticDefaults() {
        Main.buffNoSave[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex) {
        if (player.buffTime[buffIndex] == 1) {
            player.Heal(10 + (int)(player.statLifeMax2 * 0.05f));
            SoundEngine.PlaySound(RiskOfTerrain.GetSound("medkit", 0.1f));
        }
    }

    public override bool RightClick(int buffIndex) {
        return false; // Prevents removing manually
    }
}