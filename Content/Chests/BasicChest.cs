using Microsoft.Xna.Framework;
using RiskOfTerrain.Content.Chests.RustedLockbox;
using RiskOfTerrain.Content.Items.Consumable;
using RiskOfTerrain.Items;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace RiskOfTerrain.Content.Chests;

public class BasicChest() : UnifiedSecurityChest(Color.Blue) {
    public readonly SoundStyle OpenSound = Sounds.Get("chests/open");

    public static readonly int FirstItemDelay = 45;
    public static readonly int NextItemDelay = 25;

    internal override void HoverLocked(int i, int j, Player localPlayer) {
        localPlayer.cursorItemIconEnabled = true;
        localPlayer.cursorItemIconID = ItemID.GoldCoin;
    }

    internal override bool TryUnlock(int i, int j, Player localPlayer) {
        return true;
    }

    internal override void OnUnlock(int i, int j, Player localPlayer, UnifiedRandom RNG) {
        SoundEngine.PlaySound(OpenSound, new Vector2(i, j) * 16f);

        if (Main.netMode == NetmodeID.MultiplayerClient) {
            return;
        }

        IEntitySource source = new EntitySource_TileInteraction(localPlayer, i, j);
        Vector2 spawnCoordinates = new Vector2(i - (Main.tile[i, j].TileFrameX / 18) + 1f, j - (Main.tile[i, j].TileFrameY / 18) + 1.25f) * 16f;

        Vector2 velocity = new Vector2(RNG.NextFloat(0.5f, 1.5f) * (RNG.NextBool() ? -1 : 1), -4f);

        float nextItemVelocityX = Math.Sign(velocity.X) * -0.9f;

        int amountSpawned = 0;
        foreach ((int type, int stack) in GetDrops(i, j, RNG)) {
            Projectile.NewProjectile(source, spawnCoordinates, velocity / 100f, ModContent.ProjectileType<ChestItemProjectile>(), 0, 0f, Main.myPlayer,
                ai0: type,
                ai1: (amountSpawned * NextItemDelay) + FirstItemDelay,
                ai2: stack);

            velocity.X += nextItemVelocityX;
            amountSpawned++;
        }
    }

    public virtual IEnumerable<(int type, int stack)> GetDrops(int x, int y, UnifiedRandom RNG) {
        ChestDropInfo rolledItem = RNG.NextFloat(1f) switch {
            <= 0.19f => ChestDropInfo.RollChestItem(RORItem.GreenTier, x, y, RNG),
            <= 0.20f => ChestDropInfo.RollChestItem(RORItem.RedTier, x, y, RNG),
            _ => ChestDropInfo.RollChestItem(RORItem.WhiteTier, x, y, RNG)
        };

#if DEBUG
        rolledItem = RNG.NextFloat(1f) switch {
            <= 0.33f => ChestDropInfo.RollChestItem(RORItem.GreenTier, x, y, RNG),
            <= 0.66f => ChestDropInfo.RollChestItem(RORItem.RedTier, x, y, RNG),
            _ => ChestDropInfo.RollChestItem(RORItem.WhiteTier, x, y, RNG)
        };
#endif

        if (rolledItem != null) {
            yield return (rolledItem.ItemID, 1);
        }

        if (WorldGen.genRand.NextBool(10)) {
            yield return (ModContent.ItemType<RustedKey>(), 1);
        }

        if (WorldGen.genRand.NextBool()) {
            yield return (ModContent.ItemType<PowerElixir>(), WorldGen.genRand.Next(2) + 1);
        }

        if (WorldGen.genRand.NextBool()) {
            yield return (ModContent.ItemType<BisonSteak>(), WorldGen.genRand.Next(2) + 1);
        }
    }
}
