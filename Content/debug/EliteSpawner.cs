using Microsoft.Xna.Framework;
using RiskOfTerrain.Content.Elites;
using RiskOfTerrain.NPCs;
using System.Collections.Generic;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RiskOfTerrain.Content.debug;

public class EliteSpawner : ModItem {
    public override string Texture => ModContent.GetInstance<Accessories.Tier1.BustlingFungus>().Texture;

    public override void SetDefaults() {
        Item.CloneDefaults(ItemID.RegenerationPotion);
        Item.consumable = false;
    }

    public int addedPrefixIndex = 0;

    public override bool? UseItem(Player player) {
        if (player.altFunctionUse == 2) {
            if (addedPrefixIndex == 8) {
                addedPrefixIndex = 0;
            }
            else {
                addedPrefixIndex++;
            }

            int i = CombatText.NewText(new Rectangle((int)player.Center.X, (int)player.Center.Y, 1, 1), Color.BlanchedAlmond, 0);
            string t = "you ucked up";

            switch (addedPrefixIndex) {
                case 0:
                    t = "Blazing";
                    break;
                case 1:
                    t = "Celestine";
                    break;
                case 2:
                    t = "Ghostly";
                    break;
                case 3:
                    t = "Glacial";
                    break;
                case 4:
                    t = "Malachite";
                    break;
                case 5:
                    t = "Mending";
                    break;
                case 6:
                    t = "Overloading";
                    break;
                case 7:
                    t = "Perfected";
                    break;
                case 8:
                    t = "???";
                    break;
                case 9:
                    t = "Voidtouched";
                    break;
                default:
                    break;
            }

            Main.combatText[i].text = t;
            return true;
        }
        else {
            for (int i = 0; i < Main.maxNPCs; i++) {
                if (Main.npc[i].active && Main.npc[i].Hitbox.Intersects(new Rectangle((int)Main.MouseWorld.X - 5, (int)Main.MouseWorld.Y - 5, 10, 10))) {
                    Projectile.NewProjectile(player.GetSource_FromThis(), Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<EliteSpawningProj>(), 0, 0, ai0: addedPrefixIndex, ai1: i, Owner: player.whoAmI);
                    Main.combatText[i].text = $"Converting {Main.npc[i].FullName} Id:{i} to elite";
                    return true;
                }
            }

            return false;

        }
    }

    public override bool AltFunctionUse(Player player) {
        return true;
    }

    //public override void AddRecipes()
    //{
    //    CreateRecipe()
    //        //makes it so shimmering ELITINIZER will give you BUNGUS
    //        .AddCustomShimmerResult(ModContent.ItemType<Accessories.T1Common.BustlingFungus>())
    //        .AddIngredient(ModContent.ItemType<Accessories.T1Common.BustlingFungus>())
    //        .AddTile(LiquidID.Shimmer)
    //        .Register();
    //}
}

public class EliteSpawningProj : ModProjectile {
    public override void SetDefaults() {
        Projectile.timeLeft = 2;
        Projectile.alpha = 255;
        Projectile.damage = 0;
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.penetrate = -1;
        Projectile.width = 10;
        Projectile.height = 10;
    }

    public override string Texture => RiskOfTerrain.BlankTexture;

    public override void AI() {
        if (Main.netMode != NetmodeID.MultiplayerClient) {
            // Validate that npc is still active
            var npcId = (int)Projectile.ai[1];
            if (Main.npc[npcId].active) {
                // Check so the npc is not already elite

                var target = Main.npc[npcId];
                var l = new List<EliteNPCBase>(RORNPC.RegisteredElites);
                if (!target.GetGlobalNPC(l[(int)Projectile.ai[0]]).Active) {
                    target.GetGlobalNPC(l[(int)Projectile.ai[0]]).Active = true;
                    ChatHelper.SendChatMessageToClient(NetworkText.FromLiteral($"Server converted npc to elite {target.whoAmI} - {target.lifeMax}"), new Color(255, 240, 20), Projectile.owner);
                }
            }
            else {
                ChatHelper.SendChatMessageToClient(NetworkText.FromLiteral("Target is already elite"), new Color(255, 240, 20), Projectile.owner);
            }
        }
    }
}

/*MULTIPLAYER BUGS
* overloading elites crash the client when struck, dont have real health, and dont attach bombs
* ifrits distinction doesnt work on some players?
* his reassurance health is reset when healing npcs (like how tesla damage was reset)
* his reassurance heal line doesnt show up for the holder
* silence two strike bombs only show up on client
* focus crystal numbers are only purple on client
* blazing plops dont have knockback off
* infusion max health upgrade doesnt show for other players
* behemoth projectile doesnt rotate on spawn for other players
* ghosts never spawn
* ghosts tend to vanish w/ an index out of bounds error
* ghosts dont get their projectiles ghosted (probably just make happiest mask a "buggy in multiplayer" item
* other players see that those who hold headstompers have jump boost applied regardless of whether or not it is on cooldown
* soulbound catalyst just doesnt work at all when viewed by other players
* artifacts dont display in chat that you activated them
* artifact of dissonance does not work at all
* enigma doesnt work on the other player
* artifact of soul does not work at all
* artifact of spite does not work at all
* artifact of honor does not work at all
* when you do the arti jump the other player sees you teleport
* there are a bunch of "read underflow" errors that started popping up recently as a result of something i did which i cannot remember
*/

/*GENERAL BUGS
* Sentient Meat Hook pulls SOTS advisor
* War Banner / Focus Crystal circles laggy on some pcs
- Mending elites do not spawn
* Blazing aspect self inflicts dmg? maybe mp
- Shits way too expensive to open chests
- Meat hook hooks Skeletron Prime
- roll of pennies uses item.newitem which doesnt work in mp and instead i should use player.quickspawnitem
*/