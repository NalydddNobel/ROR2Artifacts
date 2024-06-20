using Microsoft.Xna.Framework;
using RiskOfTerrain.Items;
using RiskOfTerrain.Projectiles.Accessory.Damaging;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Accessories.Tier7;

public class BrilliantBehemoth : ModAccessory {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        RORItem.RedTier.Add((Type, () => {
            return Main.hardMode;
        }

        ));
    }

    public override void SetDefaults() {
        Item.width = 54;
        Item.height = 54;
        Item.accessory = true;
        Item.rare = ItemRarityID.Lime;
        Item.value = Item.sellPrice(gold: 5);
    }

    public override void ModifyHit(EntityInfo entity, EntityInfo victim, Entity projOrItem, ref StatModifier damage, ref StatModifier knockBack, ref NPC.HitModifiers modifiers) {
        int d;
        if (projOrItem is Item item) {
            d = (int)(item.damage * 0.05);
        }
        else if (projOrItem is Projectile projectile) {
            if (projectile.type == ModContent.ProjectileType<BehemothExplosion>()) {
                return;
            }
            d = (int)(projectile.damage * 0.05);
        }
        else {
            d = (int)(damage.Flat * 0.05f);
        }

        d = Math.Max(d, 1);

        SoundEngine.PlaySound(RiskOfTerrain.GetSounds("behemoth/item_proc_behemoth_0", 4).WithVolumeScale(0.2f), entity.entity.Center);

        Projectile.NewProjectile(entity.entity.GetSource_FromThis(), victim.Center, Vector2.Zero, ModContent.ProjectileType<BehemothExplosion>(), d, 0, entity.GetProjectileOwnerID());
    }
}