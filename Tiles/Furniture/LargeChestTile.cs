﻿using Microsoft.Xna.Framework;
using RiskOfTerrain.Items;
using RiskOfTerrain.Items.Placeable;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Tiles.Furniture;

public class LargeChestTile : SecurityChestTile {
    public override Color MapColor => base.MapColor;

    public override bool RollSpawnChance(int i, int j, int tileType) {
        return WorldGen.genRand.NextBool(3);
    }

    public override void FillChest(int chestID, ref int index) {
        switch (WorldGen.genRand.NextFloat(1f)) {
            case <= 0.2f: {
                    var rolledItem = ChestDropInfo.RollChestItem(RORItem.RedTier, Main.chest[chestID].x, Main.chest[chestID].y, WorldGen.genRand);
                    if (rolledItem != null) {
                        Main.chest[chestID].item[index++].SetDefaults(rolledItem.ItemID);
                    }
                }
                break;

            default: {
                    var rolledItem = ChestDropInfo.RollChestItem(RORItem.GreenTier, Main.chest[chestID].x, Main.chest[chestID].y, WorldGen.genRand);
                    if (rolledItem != null) {
                        Main.chest[chestID].item[index++].SetDefaults(rolledItem.ItemID);
                    }
                }
                break;
        }
    }

    public override void SetStaticDefaults() {
        base.SetStaticDefaults();
    }

    public override int CalculateChestPrice() {
        return RiskOfTerrain.CalculateChestPrice(2);
    }

    public override void KillMultiTile(int i, int j, int frameX, int frameY) {
        Chest.DestroyChest(i, j);
    }

    public override int HoverItem(int i, int j, int left, int top) {
        if (Chest.IsLocked(left, top))
            return ItemID.GoldCoin;
        return ModContent.ItemType<LargeChest>();
    }
}