using Microsoft.Xna.Framework;
using RiskOfTerrain.Buffs.Debuff;
using RiskOfTerrain.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RiskOfTerrain.Items.CharacterSets.Artificer;

[AutoloadEquip(EquipType.Legs)]
public class ArtificerLegs : ModItem {
    public override bool IsLoadingEnabled(Mod mod) {
        return false;
    }
    public override void SetStaticDefaults() {
        ArmorIDs.Legs.Sets.HidesTopSkin[Item.legSlot] = true;
        ArmorIDs.Legs.Sets.HidesBottomSkin[Item.legSlot] = true;
    }

    public override void SetDefaults() {
        Item.width = 22;
        Item.height = 14;
        Item.value = Item.sellPrice(gold: 5);
        Item.rare = ItemRarityID.Yellow;
        Item.defense = 6;
    }

    public bool jumpReady = true;
    public int jumpCooldown = 600;

    public override void UpdateEquip(Player player) {
        player.statManaMax2 += 30;
        player.noFallDmg = true;

        if (!player.HasBuff(ModContent.BuffType<ArmorEffectCooldown>()) && RORKeybinds.ArmorEffectKey.JustPressed) {
            player.velocity.Y -= 20;
            ROREffects.Shake.Set(4);
            SoundEngine.PlaySound(RiskOfTerrain.GetSounds("artinanobomb/mage_m2_impact_elec_v2_", 4));
            Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<PlasmaBoltImpact>(), 30, 4, player.whoAmI);
            Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<IonSurgeVisual>(), 0, 0, player.whoAmI);
            player.AddBuff(ModContent.BuffType<ArmorEffectCooldown>(), 600);
        }
    }

    public override void ModifyTooltips(List<TooltipLine> tooltips) {
        tooltips.Insert(RORItem.GetIndex(tooltips, "Consumable"), new TooltipLine(Mod, "Consumable",
            Language.GetTextValueWith("Mods.RiskOfTerrain.Items.ArtificerLegs.KeybindTooltip", new { Keybind = $"[{Helpers.GetKeyName(RORKeybinds.ArmorEffectKey)}]" })));
    }
}