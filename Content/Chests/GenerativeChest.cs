using Microsoft.Xna.Framework;
using RiskOfTerrain.Common.ContentGen;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Utilities;

namespace RiskOfTerrain.Content.Chests;

public abstract class GenerativeChest(Color MapColor) : ModTile {
    internal Color mapColor = MapColor;

    internal InstancedLockedGenerativeChest LockedTile;

    internal abstract void HoverLocked(int i, int j, Player localPlayer);
    internal abstract bool TryUnlock(int i, int j, Player localPlayer);
    internal virtual void OnUnlock(int i, int j, Player localPlayer, UnifiedRandom RNG) { }

    public override void Load() {
        LockedTile = new InstancedLockedGenerativeChest(this);
        Mod.AddContent(LockedTile);
    }

    internal class InstancedLockedGenerativeChest(GenerativeChest parent) : ModTile {
        internal GenerativeChest _parent = parent;

        public override string Name => $"{_parent.Name}Locked";
        public override string Texture => _parent.Texture;

        public override void Load() {
            ModItem item = new InstancedTileItem(this);
            Mod.AddContent(item);
        }

        public override void SetStaticDefaults() {
            Main.tileSpelunker[Type] = true;
            Main.tileContainer[Type] = true;
            Main.tileShine2[Type] = true;
            Main.tileShine[Type] = 1200;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileOreFinderPriority[Type] = 500;

            TileID.Sets.HasOutlines[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileID.Sets.AvoidedByMeteorLanding[Type] = true;
            TileID.Sets.PreventsSandfall[Type] = true;
            TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsTileReplaceIfOnTopOfIt[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Origin = new Point16(0, 1);
            TileObjectData.newTile.CoordinateHeights = [16, 18];
            TileObjectData.newTile.AnchorInvalidTiles = [TileID.MagicalIceBlock];
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);

            AddMapEntry(_parent.mapColor, CreateMapEntryName());

            DustType = _parent.DustType;
            AdjTiles = [TileID.Containers];
        }

        public override void PostSetupTileMerge() {
            //for (int i = 0; i < Main.tileMerge.Length; i++) {
            //    Main.tileMerge[i][Type] = true;
            //}
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) {
            return true;
        }

        public override void MouseOver(int i, int j) {
            Player player = Main.LocalPlayer;
            _parent.HoverLocked(i, j, Main.LocalPlayer);
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
        }

        public override bool RightClick(int i, int j) {
            Player player = Main.LocalPlayer;
            Main.mouseRightRelease = false;
            Tile tile = Main.tile[i, j];

            player.CloseSign();
            player.SetTalkNPC(-1);
            Main.npcChatCornerItem = 0;
            Main.npcChatText = "";
            if (Main.editChest) {
                SoundEngine.PlaySound(SoundID.MenuTick);
                Main.editChest = false;
                Main.npcChatText = string.Empty;
            }

            if (player.editedChestName) {
                NetMessage.SendData(MessageID.SyncPlayerChest, -1, -1, NetworkText.FromLiteral(Main.chest[player.chest].name), player.chest, 1f);
                player.editedChestName = false;
            }

            if (_parent.TryUnlock(i, j, player)) {
                if (Main.netMode == NetmodeID.MultiplayerClient) {
                    // Send packet to unlock chest.
                }
                else {
                    UnlockGenerativeChest(i, j);
                }
            }

            return true;
        }

        public override void NumDust(int i, int j, bool fail, ref int num) {
            num = fail ? 1 : 3;
        }

        internal void UnlockGenerativeChest(int i, int j) {
            Tile tile = Main.tile[i, j];
            int left = i - tile.TileFrameX % 36 / 18;
            int top = j - tile.TileFrameY % 36 / 18;

            //WorldGen.KillTile(i, j, noItem: true);
            SoundEngine.PlaySound(SoundID.Unlock, new Vector2(i, j).ToWorldCoordinates());
            for (int x = left; x < left + 2; x++) {
                for (int y = top; y < top + 2; y++) {
                    Vector2 dustCoordinates = new Vector2(i * 16, j * 16);
                    for (int k = 0; k < 4; k++) {
                        Dust.NewDust(dustCoordinates, 16, 16, _parent.DustType);
                    }

                    Tile chestTile = Framing.GetTileSafely(x, y);
                    if (chestTile.TileType == Type && Main.netMode != NetmodeID.MultiplayerClient) {
                        chestTile.HasTile = false;
                    }
                }
            }

            if (Main.netMode != NetmodeID.MultiplayerClient) {
                ushort chestType = _parent.Type;
                int chest = WorldGen.PlaceChest(left, top + 1, chestType);

                if (Main.chest.IndexInRange(chest)) {
                    // Create an RNG seed using the world's seed and the number of buried chests opened.
                    int seed = Main.ActiveWorldFileData.Seed + ChestSystem.ChestSeed;

                    ChestSystem.ChestSeed++;

                    UnifiedRandom chestSpecificRNG = new UnifiedRandom(seed);

                    // TODO -- Roll chest loot using special seed.
                    _parent.OnUnlock(i, j, Main.LocalPlayer, chestSpecificRNG);
                }

                if (Main.netMode == NetmodeID.Server) {
                    NetMessage.SendTileSquare(-1, left - 1, top - 1, 4, 4);
                    NetMessage.SendData(MessageID.ChestUpdates,
                        number: 100,
                        number2: Main.chest[chest].x, number3: Main.chest[chest].y + 1,
                        number4: 0f, number5: 0,
                        number6: chestType);
                }
            }
        }
    }
}
