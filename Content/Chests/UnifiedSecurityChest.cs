using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using RiskOfTerrain.Common.ContentGen;
using RiskOfTerrain.Tiles;
using RiskOfTerrain.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Utilities;

namespace RiskOfTerrain.Content.Chests;

/// <summary>Abstract parent class for all naturally spawning risk-of-rain themed chests.</summary>
/// <param name="MapColor"></param>
public abstract class UnifiedSecurityChest(Color MapColor, float PriceMultiplier = 1f) : ModTile {
    internal Color mapColor = MapColor;
    internal float priceMultiplier = PriceMultiplier;

    public ModItem DropItem { get; private set; }
    internal InstancedLockedChest LockedTile;

    public const int FrameCount = 6;

    public static float SecondsForNewlyOpenedChest => 6f;
    public static float ChestCloseAnimationLength => 0.7f;
    public static float ChestAnimationLength => 1.3f;

    public static Dictionary<Point, float> _animations;

    public int FrameWidth { get; private set; }
    public int FrameHeight { get; private set; }

    public Asset<Texture2D> GlowTexture { get; private set; }

    internal abstract void HoverLocked(int i, int j, Player localPlayer);
    internal virtual bool TryUnlock(int i, int j, Player localPlayer) {
        return localPlayer.CanAfford(RiskOfTerrain.CalculateChestPrice(priceMultiplier), -1);
    }
    internal virtual void OnUnlock(int i, int j, Player localPlayer, UnifiedRandom RNG) { }

    public override void Load() {
        DropItem = new InstancedTileItem(this, value: Item.sellPrice(silver: 1));
        Mod.AddContent(DropItem);

        LockedTile = new InstancedLockedChest(this);
        Mod.AddContent(LockedTile);

        GlowTexture = ModContent.Request<Texture2D>($"{Texture}_Glow");
    }

    public override void SetStaticDefaults() {
        this.CloneStaticDefaults(TileID.Containers);
        Main.tileOreFinderPriority[Type] = 490;

        AdjTiles = [TileID.Containers];

        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
        TileObjectData.newTile.Origin = new Point16(0, 1);
        TileObjectData.newTile.CoordinateHeights = [16, 18];
        TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(Chest.FindEmptyChest, -1, 0, true);
        TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(Chest.AfterPlacement_Hook, -1, 0, false);
        TileObjectData.newTile.AnchorInvalidTiles = [TileID.MagicalIceBlock];
        TileObjectData.newTile.StyleHorizontal = true;
        TileObjectData.newTile.LavaDeath = false;
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);

        AddMapEntry(mapColor, CreateMapEntryName(), MapChestName);

        FrameWidth = TileObjectData.newTile.CoordinateFullWidth;
        FrameHeight = TileObjectData.newTile.CoordinateFullHeight;

        TileObjectData.addTile(Type);
    }

    private string MapChestName(string name, int i, int j) {
        GetChestLocation(i, j, out int left, out int top);

        int chest = Chest.FindChest(left, top);
        if (chest < 0) {
            return Language.GetTextValue("LegacyChestType.0");
        }

        if (Main.chest[chest].name == "") {
            return name;
        }

        return name + ": " + Main.chest[chest].name;
    }

    public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) {
        return true;
    }

    public override bool IsLockedChest(int i, int j) {
        return Main.tile[i, j].TileFrameX / FrameWidth == 1;
    }

    public override void NumDust(int i, int j, bool fail, ref int num) {
        num = fail ? 1 : 3;
    }

    public override void KillMultiTile(int i, int j, int frameX, int frameY) {
        Chest.DestroyChest(i, j);
    }

    public override bool RightClick(int i, int j) {
        Player player = Main.LocalPlayer;
        Main.mouseRightRelease = false;
        Tile tile = Main.tile[i, j];
        GetChestHoverLocation(i, j, Main.MouseWorld, in tile, out int chestX, out int chestY);

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

        bool isLocked = Chest.IsLocked(chestX, chestY);
        if (Main.netMode == NetmodeID.MultiplayerClient && !isLocked) {
            if (chestX == player.chestX && chestY == player.chestY && player.chest >= 0) {
                player.chest = -1;
                Recipe.FindRecipes();
                SoundEngine.PlaySound(SoundID.MenuClose);
            }
            else {
                NetMessage.SendData(MessageID.RequestChestOpen, -1, -1, null, chestX, chestY);
                Main.stackSplit = 600;
            }
        }
        else {
            if (isLocked) {
                if (TryUnlock(i, j, player) && Chest.Unlock(chestX, chestY)) {
                    if (Main.netMode == NetmodeID.MultiplayerClient) {
                        NetMessage.SendData(MessageID.LockAndUnlock, -1, -1, null, player.whoAmI, 1f, chestX, chestY);
                    }
                }
            }
            else {
                int chest = Chest.FindChest(chestX, chestY);
                if (chest >= 0) {
                    Main.stackSplit = 600;
                    if (chest == player.chest) {
                        player.chest = -1;
                        SoundEngine.PlaySound(SoundID.MenuClose);
                    }
                    else {
                        SoundEngine.PlaySound(player.chest < 0 ? SoundID.MenuOpen : SoundID.MenuTick);
                        player.OpenChest(chestX, chestY, chest);
                    }

                    Recipe.FindRecipes();
                }
            }
        }

        return true;
    }

    public virtual int HoverItem(int i, int j, int left, int top) {
        return DropItem.Type;
    }

    public override void MouseOver(int i, int j) {
        Player player = Main.LocalPlayer;
        Tile tile = Main.tile[i, j];
        GetChestHoverLocation(i, j, Main.MouseWorld, in tile, out int chestX, out int chestY);

        int chest = Chest.FindChest(chestX, chestY);
        player.cursorItemIconID = -1;
        if (chest < 0) {
            player.cursorItemIconText = Language.GetTextValue("LegacyChestType.0");
        }
        else {
            string defaultName = TileLoader.DefaultContainerName(tile.TileType, tile.TileFrameX, tile.TileFrameY);
            player.cursorItemIconText = Main.chest[chest].name.Length > 0 ? Main.chest[chest].name : defaultName;
            if (player.cursorItemIconText == defaultName) {
                player.cursorItemIconID = HoverItem(i, j, chestX, chestY);
                player.cursorItemIconText = "";
            }
        }

        player.noThrow = 2;
        player.cursorItemIconEnabled = true;
    }

    public override void MouseOverFar(int i, int j) {
        MouseOver(i, j);
        Player player = Main.LocalPlayer;
        if (player.cursorItemIconText == "") {
            player.cursorItemIconEnabled = false;
            player.cursorItemIconID = 0;
        }
    }

    public override bool PreDraw(int i, int j, SpriteBatch spriteBatch) {
        DrawChest(i, j, spriteBatch);
        return false;
    }

    internal void DrawChest(int i, int j, SpriteBatch spriteBatch) {
        try {
            Tile tile = Main.tile[i, j];
            GetChestLocation(i, j, tile, out int left, out int top);

            int chest = Chest.FindChest(left, top);
            Point xy = new Point(left, top);

            int frameNum = 0;

            _animations ??= new();

            if (_animations.TryGetValue(xy, out float animationAnchor)) {
                HandleClosing(left, top, xy, chest, animationAnchor);

                if (Main.GlobalTimeWrappedHourly < Math.Abs(animationAnchor)) {
                    _animations.Remove(xy);
                    return;
                }

                // Closing animation.
                if (animationAnchor < 0f) {
                    float compareTime = (Main.GlobalTimeWrappedHourly + animationAnchor) * 3f / ChestCloseAnimationLength;

                    frameNum = (int)(FrameCount - compareTime - 1);

                    if (frameNum <= 3) {
                        frameNum = 0;
                        _animations.Remove(xy);
                        _animations.Remove(new Point(left, top + 1));
                    }
                }

                // Opening animation.
                else {
                    float compareTime = (Main.GlobalTimeWrappedHourly - animationAnchor) / ChestAnimationLength;

                    // millisecond timings.
                    frameNum = compareTime switch {
                        < 0.09f => 1,
                        < 0.35f => 2,
                        < 0.55f => 3,
                        < 0.6f => 4,
                        _ => 5
                    };
                }
            }
            else if (chest != -1 && Main.chest[chest].frame != 0 && !_animations.ContainsKey(xy)) {
                _animations[xy] = Main.GlobalTimeWrappedHourly;
            }

            Texture2D texture = TextureAssets.Tile[tile.TileType].Value;
            Texture2D glowTexture = GlowTexture.Value;

            Vector2 drawCoordinates = new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + Helpers.TileDrawOffset;
            Rectangle frame = new Rectangle(tile.TileFrameX, 38 * frameNum + tile.TileFrameY, 16, 16);

            // Chests are shiny tiles, so this weird lighting effect is applied.
            Color lightColor = Lighting.GetColor(i, j) * 1.1f;

            // RGBA value for the 'glowmask' (which doesnt glow lolol)
            int colorMax = Math.Min(Math.Max(Math.Max(lightColor.R, lightColor.G), lightColor.B) * 6, byte.MaxValue);

            spriteBatch.Draw(texture, drawCoordinates, frame, lightColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(glowTexture, drawCoordinates, frame, new Color(colorMax, colorMax, colorMax, colorMax), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            if ((Chest.IsLocked(left, top) || TileLoader.GetTile(tile.TileType) is InstancedLockedChest) && !PurchasableChestInterface.PurchasePopups.ContainsKey(new Point(left, top))) {
                TrySpawnPopupText(left, top, priceMultiplier);
            }
        }
        catch {
        }

        void HandleClosing(int left, int top, Point xy, int chest, float animationAnchor) {
            if (animationAnchor <= 0f || (Main.GlobalTimeWrappedHourly - animationAnchor) <= ChestAnimationLength || (chest != -1 && Main.chest[chest].frame > 0)) {
                return;
            }

            if (_animations.TryGetValue(new Point(left, top + 1), out float newlyOpenedAnimationAnchor)) {
                if ((Main.GlobalTimeWrappedHourly - animationAnchor) < SecondsForNewlyOpenedChest) {
                    return;
                }
            }

            animationAnchor = _animations[xy] = -Main.GlobalTimeWrappedHourly;
        }
    }

    private static void TrySpawnPopupText(int left, int top, float priceMultiplier) {
        if (Vector2.Distance(Main.LocalPlayer.Center, new Vector2(left * 16f + 16f, top * 16f + 16f)) < 100f) {
            PurchasableChestInterface.PurchasePopups.Add(new Point(left, top), new PurchasableChestInterface.ChestPurchasePopup(left, top, RiskOfTerrain.CalculateChestPrice(priceMultiplier), 1, 1));
        }
    }

    internal class InstancedLockedChest(UnifiedSecurityChest parent) : ModTile {
        internal UnifiedSecurityChest _parent = parent;

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
            Main.tileOreFinderPriority[Type] = 490;

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

            AddMapEntry(_parent.mapColor, _parent.GetLocalization("MapEntry.Locked"));

            DustType = _parent.DustType;
            AdjTiles = [TileID.Containers];
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
                    SendUnlockChest(i, j, player.whoAmI);
                }
                else {
                    UnlockSecurityChest(i, j);
                }
            }

            return true;
        }

        public override void NumDust(int i, int j, bool fail, ref int num) {
            num = fail ? 1 : 3;
        }

        internal void UnlockSecurityChest(int i, int j) {
            Tile tile = Main.tile[i, j];
            int left = i - (tile.TileFrameX % 36 / 18);
            int top = j - (tile.TileFrameY % 36 / 18);

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

                // If chest spawned correctly.
                if (Main.chest.IndexInRange(chest)) {

                }

                // Create an RNG seed using the world's seed and the number of buried chests opened.
                int seed = Main.ActiveWorldFileData.Seed + ChestSystem.ChestSeed;

                ChestSystem.ChestSeed++;

                UnifiedRandom chestSpecificRNG = new UnifiedRandom(seed);

                // TODO -- Roll chest loot using special seed.
                _parent.OnUnlock(i, j, Main.LocalPlayer, chestSpecificRNG);

                if (Main.netMode == NetmodeID.Server) {
                    NetMessage.SendTileSquare(-1, left - 1, top - 1, 4, 4);
                    NetMessage.SendData(MessageID.ChestUpdates,
                        number: 100,
                        number2: Main.chest[chest].x, number3: Main.chest[chest].y + 1,
                        number4: 0f, number5: 0,
                        number6: chestType);
                }
            }

            if (Main.netMode != NetmodeID.Server) {
                _animations[new Point(left, top)] = _animations[new Point(left, top + 1)] = Main.GlobalTimeWrappedHourly;
            }
        }

        public override void RandomUpdate(int i, int j) {
            if (Main.tile[i, j].TileFrameX == 0 && Main.tile[i, j].TileFrameY == 0 && WorldGen.genRand.NextBool(100) && !RORTile.IsTileInView(i, j, 10)) {
                for (int x = i; x <= i + 1; x++) {
                    for (int y = j; y <= j + 1; y++) {
                        Tile t = Main.tile[x, y];
                        t.HasTile = false;
                    }
                }
                NetMessage.SendTileSquare(-1, i, j, 2, 2);
                //Chest.DestroyChest(i, j);
            }
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch) {
            _parent.DrawChest(i, j, spriteBatch);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected void GetChestLocation(int i, int j, out int chestX, out int chestY) {
        var tile = Main.tile[i, j];
        GetChestLocation(i, j, in tile, out chestX, out chestY);
    }
    protected void GetChestLocation(int i, int j, in Tile tileCache, out int chestX, out int chestY) {
        chestX = i - tileCache.TileFrameX % FrameWidth / 18;
        chestY = j - tileCache.TileFrameY % FrameHeight / 18;
    }
    protected void GetChestHoverLocation(int i, int j, Vector2 mouseWorld, in Tile tileCache, out int chestX, out int chestY) {
        GetChestHoverLocation(i, j, mouseWorld.X / 16.0 % 1.0, mouseWorld.Y / 16.0 % 1.0, in tileCache, out chestX, out chestY);
    }
    protected void GetChestHoverLocation(int i, int j, double mouseX, double mouseY, in Tile tileCache, out int chestX, out int chestY) {
        GetChestLocation(i, j, in tileCache, out chestX, out chestY);
    }

    protected void SendChestUpdate(int x, int y, int style) {
        NetMessage.SendData(MessageID.ChestUpdates, number: 100, number2: x, number3: y, number4: style, number5: 0, number6: Type);
    }

    #region Networking
    public static void SendUnlockChest(int i, int j, int plr, ushort? TileTypeOverride = null) {
        ModPacket packet = RiskOfTerrain.GetPacket(PacketType.UnlockSecurityChest);
        packet.Write((ushort)i);
        packet.Write((ushort)j);
        packet.Write(TileTypeOverride ?? Main.tile[i, j].TileType);
        packet.Send();
    }

    internal static void ReceiveUnlockChest(BinaryReader reader) {
        int i = reader.ReadUInt16();
        int j = reader.ReadUInt16();
        ushort type = reader.ReadUInt16();

        if (TileLoader.GetTile(type) is not InstancedLockedChest chest) {
            return;
        }

        if (Main.netMode == NetmodeID.Server) {
            SendUnlockChest(i, j, Main.myPlayer, TileTypeOverride: type);
        }

        chest.UnlockSecurityChest(i, j);
    }
    #endregion
}
