using RiskOfTerrain.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Accessories.Tier1;

[AutoloadEquip(EquipType.Shoes)]
public class SoldiersSyringe : ModAccessory {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        RORItem.WhiteTier.Add(Type);
    }

    public override void SetDefaults() {
        Item.width = 18;
        Item.height = 36;
        Item.accessory = true;
        Item.rare = ItemRarityID.Blue;
        Item.value = Item.sellPrice(silver: 50);
    }

    public override void UpdateAccessory(Player player, bool hideVisual) {
        player.GetAttackSpeed(DamageClass.Generic) += 0.1f;
    }
}