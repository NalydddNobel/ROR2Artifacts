using Microsoft.Xna.Framework;
using RiskOfTerrain.Items;
using RiskOfTerrain.Projectiles.Accessory.Damaging;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Accessories.Tier4;

public class WillOTheWisp : ModAccessory {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        RORItem.GreenTier.Add((Type, () => {
            return Main.hardMode;
        }

        ));
    }

    public override void SetDefaults() {
        Item.width = 24;
        Item.height = 24;
        Item.accessory = true;
        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.sellPrice(gold: 2);
    }

    public override void OnKillEnemy(EntityInfo entity, OnKillInfo info) {
        Projectile.NewProjectile(entity.entity.GetSource_FromThis(), new Vector2(info.Center.X, info.Center.Y - 15), Vector2.Zero, ModContent.ProjectileType<WilloExplosion>(), 35, 0, Owner: Main.LocalPlayer.whoAmI);
        //make it spawn higher, make it owned by player, make it not have sprite issues, make it not spawn from critters
    }
}