﻿using Microsoft.Xna.Framework;
using RiskOfTerrain.Content.Accessories.VoidVariants;
using RiskOfTerrain.Items;
using RiskOfTerrain.Projectiles.Accessory.Utility;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Accessories.Tier1;

[AutoloadEquip(EquipType.Front)]
public class BustlingFungus : ModAccessory, ItemHooks.IUpdateItemDye {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        RORItem.WhiteTier.Add(Type);
    }

    public override void SetDefaults() {
        Item.width = 24;
        Item.height = 22;
        Item.accessory = true;
        Item.rare = ItemRarityID.Blue;
        Item.value = Item.sellPrice(gold: 1);
    }

    public override void UpdateAccessory(Player player, bool hideVisual) {
        player.ROR().accBungus = true;
    }

    public override void PostUpdate(EntityInfo entity) {
        if (entity.IdleTime() > 60 && entity.entity is Player player) {
            for (int i = 0; i < Main.maxProjectiles; i++) {
                if (Main.projectile[i].active && Main.projectile[i].type == ModContent.ProjectileType<BustlingFungusProj>() && entity.OwnsThisProjectile(Main.projectile[i])) {
                    UpdateProjectile(entity.entity, Main.projectile[i]);
                    return;
                }
            }
            UpdateProjectile(entity.entity, Projectile.NewProjectileDirect(player.GetSource_Accessory(Item), entity.entity.Center, Vector2.Zero,
                ModContent.ProjectileType<BustlingFungusProj>(), 0, 0f, entity.GetProjectileOwnerID()));
        }
    }

    public static void UpdateProjectile(Entity entity, Projectile projectile) {
        projectile.scale = MathHelper.Lerp(projectile.scale, 312f, 0.2f);
        projectile.Center = entity.Center;
        var bungus = (BustlingFungusProj)projectile.ModProjectile;
        bungus.accessoryActive = true;
        bungus.regenPercent = 0.2f;
    }

    void ItemHooks.IUpdateItemDye.UpdateItemDye(Player player, bool isNotInVanitySlot, bool isSetToHidden, Item armorItem, Item dyeItem) {
        player.ROR().cBungus = dyeItem.dye;
    }

    public override void AddRecipes() {
        CreateRecipe()
            .AddIngredient(ModContent.ItemType<WeepingFungus>())
            .AddCondition(Condition.NearShimmer)
            .Register();
    }
}