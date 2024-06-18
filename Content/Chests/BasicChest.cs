using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;

namespace RiskOfTerrain.Content.Chests;

public class BasicChest() : GenerativeChest(Color.Blue) {
    internal override void HoverLocked(int i, int j, Player localPlayer) {
        localPlayer.cursorItemIconEnabled = true;
        localPlayer.cursorItemIconID = ItemID.GoldCoin;
    }

    internal override bool TryUnlock(int i, int j, Player localPlayer) {
        return true;
    }

    internal override void OnUnlock(int i, int j, Player localPlayer, UnifiedRandom RNG) {
        base.OnUnlock(i, j, localPlayer, RNG);
    }
}
