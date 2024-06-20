﻿using Microsoft.Xna.Framework;
using RiskOfTerrain.Items;
using RiskOfTerrain.Projectiles.Accessory.Visual;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Accessories.Tier2;
public class FocusCrystal : ModAccessory, ItemHooks.IUpdateItemDye {
    public static Entity HitNPCForMakingDamageNumberPurpleHack;
    public static Color HitColor;
    public static Color HitColorCrit;

    public bool hideVisual;

    public override void Load() {
        HitColor = new Color(244, 105, 200, 255);
        HitColorCrit = HitColor * 1.4f;
        On_CombatText.NewText_Rectangle_Color_string_bool_bool += CombatText_NewText_Rectangle_Color_string_bool_bool;
    }

    private static int CombatText_NewText_Rectangle_Color_string_bool_bool(On_CombatText.orig_NewText_Rectangle_Color_string_bool_bool orig, Rectangle location, Color color, string text, bool dramatic, bool dot) {
        if (HitNPCForMakingDamageNumberPurpleHack != null && location.X == (int)HitNPCForMakingDamageNumberPurpleHack.position.X && location.Y == (int)HitNPCForMakingDamageNumberPurpleHack.position.Y) {
            if (color == CombatText.DamagedHostileCrit || color == CombatText.DamagedFriendlyCrit) {
                color = HitColorCrit;
            }
            else {
                color = HitColor;
            }
            HitNPCForMakingDamageNumberPurpleHack = null;
        }
        return orig(location, color, text, dramatic, dot);
    }

    public override void Unload() {
        HitNPCForMakingDamageNumberPurpleHack = null;
    }

    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        RORItem.WhiteTier.Add(Type);
    }

    public override void SetDefaults() {
        Item.width = 24;
        Item.height = 22;
        Item.accessory = true;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.sellPrice(gold: 1);
    }

    public override void UpdateAccessory(Player player, bool hideVisual) {
        this.hideVisual = hideVisual;
    }

    public override void PostUpdate(EntityInfo entity) {
        if (!hideVisual && entity.entity is Player player) {
            for (int i = 0; i < Main.maxProjectiles; i++) {
                if (Main.projectile[i].active && Main.projectile[i].type == ModContent.ProjectileType<FocusCrystalProj>() && entity.OwnsThisProjectile(Main.projectile[i])) {
                    UpdateProjectile(entity, Main.projectile[i]);
                    return;
                }
            }
            UpdateProjectile(entity, Projectile.NewProjectileDirect(player.GetSource_Accessory(Item), entity.entity.Center, Vector2.Zero,
                ModContent.ProjectileType<FocusCrystalProj>(), 0, 0f, entity.GetProjectileOwnerID()));
        }
    }

    public void UpdateProjectile(EntityInfo entity, Projectile projectile) {
        projectile.scale = MathHelper.Lerp(projectile.scale, 480f, 0.2f);
        projectile.Center = entity.entity.Center;
        var focusCrystal = (FocusCrystalProj)projectile.ModProjectile;
        focusCrystal.accessoryVisible = true;
    }


    void ItemHooks.IUpdateItemDye.UpdateItemDye(Player player, bool isNotInVanitySlot, bool isSetToHidden, Item armorItem, Item dyeItem) {
        player.ROR().cFocusCrystal = dyeItem.dye;
    }

    public override void ModifyHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, ref StatModifier damage, ref StatModifier knockBack, ref NPC.HitModifiers modifiers) {
        if (entity.entity.Distance(victim.entity.Hitbox.ClosestDistance(entity.entity.Center)) < 240f) {
            modifiers.ScalingBonusDamage += 0.25f;
            HitNPCForMakingDamageNumberPurpleHack = victim.entity;
        }
    }
}
