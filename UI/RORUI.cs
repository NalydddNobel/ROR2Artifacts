﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.UI;

namespace RiskOfTerrain.UI
{
    public class RORUI : ModSystem
    {
        public class InterfaceLayers
        {
            public const string InterfaceLogic1_0 = "Vanilla: Interface Logic 1";
            public const string MultiplayerPlayerNames_1 = "Vanilla: MP Player Names";
            public const string EmoteBubbles_2 = "Vanilla: Emote Bubbles";
            public const string EntityMarkers_3 = "Vanilla: Entity Markers";
            public const string SmartCursorTargets_4 = "Vanilla: Smart Cursor Targets";
            public const string LaserRuler_5 = "Vanilla: Laser Ruler";
            public const string Ruler_6 = "Vanilla: Ruler";
            public const string GamepadLockOn_7 = "Vanilla: Gamepad Lock On";
            public const string TileGridOption_8 = "Vanilla: Tile Grid Option";
            public const string TownNPCHouseBanners_9 = "Vanilla: Town NPC House Banners";
            public const string HideUIToggle_10 = "Vanilla: Hide UI Toggle";
            public const string WireSelection_11 = "Vanilla: Wire Selection";
            public const string CaptureManagerCheck_12 = "Vanilla: Capture Manager Check";
            public const string IngameOptions_13 = "Vanilla: Ingame Options";
            public const string FancyUI_14 = "Vanilla: Fancy UI";
            public const string AchievementCompletePopups_15 = "Vanilla: Achievement Complete Popups";
            public const string EntityHealthBars_16 = "Vanilla: Entity Health Bars";
            public const string InvasionProgressBars_17 = "Vanilla: Invasion Progress Bars";
            public const string MapMinimap_18 = "Vanilla: Map / Minimap";
            public const string DiagnoseNet_19 = "Vanilla: Diagnose Net";
            public const string DiagnoseVideo_20 = "Vanilla: Diagnose Video";
            public const string SignTileBubble_21 = "Vanilla: Sign Tile Bubble";
            public const string HairWindow_22 = "Vanilla: Hair Window";
            public const string DresserWindow_23 = "Vanilla: Dresser Window";
            public const string NPCSignDialog_24 = "Vanilla: NPC / Sign Dialog";
            public const string InterfaceLogic2_25 = "Vanilla: Interface Logic 2";
            public const string ResourceBars_26 = "Vanilla: Resource Bars";
            public const string InterfaceLogic3_27 = "Vanilla: Interface Logic 3";
            public const string Inventory_28 = "Vanilla: Inventory";
            public const string InfoAccessoriesBar_29 = "Vanilla: Info Accessories Bar";
        }

        public const float invBackColorMultipler = 0.785f;
        public static readonly Color invBackColor = new Color(63, 65, 151, 255);
        public static Color InventoryBackColor => invBackColor * invBackColorMultipler;

        public static int itemSlotContext;

        public static UserInterface DynamicInterface { get; private set; }

        public override void Load()
        {
            LoadHooks();
            if (!Main.dedServ)
            {
                DynamicInterface = new UserInterface();
            }
        }
        private void LoadHooks()
        {
            On.Terraria.UI.ItemSlot.Draw_SpriteBatch_ItemArray_int_int_Vector2_Color += Hook_UpdateStaticContext;
        }

        private void Hook_UpdateStaticContext(On.Terraria.UI.ItemSlot.orig_Draw_SpriteBatch_ItemArray_int_int_Vector2_Color orig, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Item[] inv, int context, int slot, Vector2 position, Color lightColor)
        {
            itemSlotContext = context;
            orig(spriteBatch, inv, context, slot, position, lightColor);
        }

        public override void Unload()
        {
        }

        public override void UpdateUI(GameTime gameTime)
        {
            DynamicInterface.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            ManageUserInterfaceLayer(layers, DynamicInterface, InterfaceLayers.InfoAccessoriesBar_29, "RiskOfTerrain: Inventory Interface", InterfaceScaleType.UI);
        }
        private void ManageUserInterfaceLayer(List<GameInterfaceLayer> layers, UserInterface userInterface, string defaultLayer, string layerName, InterfaceScaleType scaleType = InterfaceScaleType.UI)
        {
            int layer = -1;
            if (userInterface.CurrentState is RORUIState state)
            {
                if (!state.ModifyInterfaceLayers(layers, ref scaleType))
                {
                    return;
                }
                layer = state.GetLayerIndex(layers);
            }
            if (layer == -1)
            {
                layer = layers.FindIndex((g) => g.Name.Equals(defaultLayer));
            }
            if (layer != -1)
            {
                layers.Insert(layer, new LegacyGameInterfaceLayer(layerName, () =>
                {
                    userInterface.Draw(Main.spriteBatch, Main._drawInterfaceGameTime);
                    return true;
                }, scaleType));
            }
        }

        public static void CloseAllInventoryRelatedUI()
        {
            CloseAllInventoryRelatedUI(Main.LocalPlayer);
        }
        public static void CloseAllInventoryRelatedUI(Player player)
        {
            player.chest = -1;
            player.sign = -1;
            player.SetTalkNPC(-1);
            if (player.whoAmI == Main.myPlayer)
            {
                Main.playerInventory = false;
                Main.recBigList = false;
                Main.CreativeMenu.CloseMenu();
                if (PlayerInput.GrappleAndInteractAreShared)
                {
                    PlayerInput.Triggers.JustPressed.Grapple = false;
                }
                DynamicInterface.SetState(null);
            }
        }

        public static void HoverItem(Item item, int context = -1)
        {
            Main.hoverItemName = item.Name;
            Main.HoverItem = item.Clone();
            Main.HoverItem.tooltipContext = context;
        }
    }
}