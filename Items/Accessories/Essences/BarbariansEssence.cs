using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace FargowiltasSouls.Items.Accessories.Essences
{
    public class BarbariansEssence : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Barbarian's Essence");
            Tooltip.SetDefault(
@"'This is only the beginning..' 
18% increased melee damage 
10% increased melee speed 
5% increased melee crit chance");
            DisplayName.AddTranslation(GameCulture.Chinese, "野蛮人精华");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'这才刚刚开始..'
增加18%近战伤害
增加10%近战速度
增加5%近战暴击率");
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color?(new Color(255, 111, 6));
                }
            }
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            item.value = 150000;
            item.rare = 4;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.meleeDamage += .18f;
            player.meleeSpeed += .1f;
            player.meleeCrit += 5;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            if (Fargowiltas.Instance.ThoriumLoaded)
            {
                //just thorium
                recipe.AddIngredient(ItemID.WarriorEmblem);
                recipe.AddIngredient(ItemID.ZombieArm);
                recipe.AddIngredient(ItemID.Trident);
                recipe.AddIngredient(ItemID.ChainKnife);
                recipe.AddIngredient(thorium.ItemType("RedHourglass"));
                recipe.AddIngredient(ItemID.StylistKilLaKillScissorsIWish);
                recipe.AddIngredient(ItemID.IceBlade);
                recipe.AddIngredient(ItemID.FalconBlade);
                recipe.AddIngredient(ItemID.Starfury);
                recipe.AddIngredient(thorium.ItemType("DrenchedDirk"));
                recipe.AddIngredient(thorium.ItemType("Whip"));
                recipe.AddIngredient(ItemID.BeeKeeper);
                recipe.AddIngredient(thorium.ItemType("EnergyStormPartisan"));
                recipe.AddRecipeGroup("FargowiltasSouls:AnyThoriumYoyo");
            }
            else
            {
                //no others
                recipe.AddIngredient(ItemID.WarriorEmblem);
                recipe.AddIngredient(ItemID.ZombieArm);
                recipe.AddIngredient(ItemID.Trident);
                recipe.AddIngredient(ItemID.ChainKnife);
                recipe.AddIngredient(ItemID.StylistKilLaKillScissorsIWish);
                recipe.AddIngredient(ItemID.IceBlade);
                recipe.AddIngredient(ItemID.FalconBlade);
                recipe.AddIngredient(ItemID.Starfury);
                recipe.AddIngredient(ItemID.BeeKeeper);
                recipe.AddIngredient(ItemID.Cascade);
            }

            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
