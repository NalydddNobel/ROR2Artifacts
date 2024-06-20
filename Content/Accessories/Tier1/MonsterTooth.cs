using Microsoft.Xna.Framework;
using RiskOfTerrain.Items;
using RiskOfTerrain.Projectiles.Accessory.Utility;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.Accessories.Tier1
{
    public class MonsterTooth : ModAccessory
    {
        public int killDelay;

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            RORItem.WhiteTier.Add((Type, () => NPC.downedSlimeKing));
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(silver: 50);
        }

        public override void UpdateEquip(Player player)
        {
            if (killDelay > 0)
                killDelay--;
        }

        public override void OnKillEnemy(EntityInfo entity, OnKillInfo info)
        {
            if (entity.IsMe() && entity.entity is Player player)
            {
                Projectile.NewProjectile(player.GetSource_Accessory(Item), info.Center, new Vector2(0f, -2f), ModContent.ProjectileType<MonsterToothProj>(), 0, 0, entity.GetProjectileOwnerID());
            }
        }
    }
}