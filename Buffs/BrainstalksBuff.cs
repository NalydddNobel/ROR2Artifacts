using Terraria;
using Terraria.ModLoader;

namespace RiskOfTerrain.Buffs;

public class BrainstalksBuff : ModBuff {
    public override void SetStaticDefaults() {
        Main.buffNoSave[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex) {
        player.GetAttackSpeed(DamageClass.Generic) += 0.7f;
    }
}