using RiskOfTerrain.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Accessories.Tier1;

[AutoloadEquip(EquipType.Waist)]
public class RepulsionArmorPlate : ModAccessory
{
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 1;
        RORItem.WhiteTier.Add((Type, () =>
        {
            return NPC.downedSlimeKing;
        }

        ));
    }

    public override void SetDefaults()
    {
        Item.width = 24;
        Item.height = 22;
        Item.accessory = true;
        Item.rare = ItemRarityID.Blue;
        Item.value = Item.sellPrice(gold: 1);
        Item.defense = 10;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.ROR().accRepulsionPlate += 5;
    }
}