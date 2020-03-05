using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class StardustEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stardust Enchantment");
            Tooltip.SetDefault(
@"'The power of the Stand is yours' 
Double tap down to direct your empowered guardian
Press the Freeze Key to freeze time for 5 seconds
While time is frozen, your guardian will continue to attack
There is a 60 second cooldown for this effect, a sound effect plays when it's back");
            DisplayName.AddTranslation(GameCulture.Chinese, "星尘魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'屎蛋多帕瓦!'
双击'下'键指挥你的强化替身
按下时间冻结热键时停5秒
时间停止时, 替身仍可以攻击
60秒的冷却时间, 冷却结束时会播放音效");
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color(0, 174, 238);
                }
            }
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 10;
            item.value = 400000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().StardustEffect();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.StardustHelmet);
            recipe.AddIngredient(ItemID.StardustBreastplate);
            recipe.AddIngredient(ItemID.StardustLeggings);
            
            if(Fargowiltas.Instance.ThoriumLoaded)
            {      
                recipe.AddIngredient(ItemID.WingsStardust);
                recipe.AddIngredient(thorium.ItemType("TimeBook"));
                recipe.AddIngredient(thorium.ItemType("BlackCane"));
                recipe.AddIngredient(thorium.ItemType("ShadowOrbStaff"));
            }
            else
            {
                recipe.AddIngredient(ItemID.StardustPickaxe);
            }
            
            recipe.AddIngredient(ItemID.StardustCellStaff);
            recipe.AddIngredient(ItemID.StardustDragonStaff);
            recipe.AddIngredient(ItemID.RainbowCrystalStaff);

            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
