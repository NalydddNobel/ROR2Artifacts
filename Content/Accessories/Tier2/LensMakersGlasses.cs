using RiskOfTerrain.Graphics.PlayerLayers;
using RiskOfTerrain.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Accessories.Tier2;

[AutoloadEquip(EquipType.Face)]
public class LensMakersGlasses : ModAccessory {
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 1;
        RORItem.WhiteTier.Add((Type, () => {
            return NPC.downedBoss1;
        }

        ));
        FaceGlowMask.GlowMask.Add(Item.faceSlot, $"{Texture}_Face_Glow");
    }

    public override void SetDefaults() {
        Item.width = 26;
        Item.height = 26;
        Item.accessory = true;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.sellPrice(gold: 1);
    }

    public override void UpdateAccessory(Player player, bool hideVisual) {
        player.GetCritChance(DamageClass.Generic) += 10;
    }
}