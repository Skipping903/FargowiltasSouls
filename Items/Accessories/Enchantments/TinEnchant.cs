using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class TinEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");
        public int timer;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tin Enchantment");

            string tooltip = 
@"'Return of the Crit'
Sets your critical strike chance to 4%
Every crit will increase it by 4%
Getting hit drops your crit back down";
            string tooltip_ch =
@"'暴击回归'
暴击率设为4%
每次暴击增加4%
被击中降低暴击率";

            Tooltip.SetDefault(tooltip);
            DisplayName.AddTranslation(GameCulture.Chinese, "锡魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, tooltip_ch);
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color(162, 139, 78);
                }
            }
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 1;
            item.value = 30000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().TinEffect();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.TinHelmet);
            recipe.AddIngredient(ItemID.TinChainmail);
            recipe.AddIngredient(ItemID.TinGreaves);
            
            if(Fargowiltas.Instance.ThoriumLoaded)
            {      
                recipe.AddIngredient(thorium.ItemType("TinBuckler"));
                recipe.AddIngredient(ItemID.TinShortsword);
                recipe.AddIngredient(ItemID.TinBroadsword);
                recipe.AddIngredient(ItemID.TinBow);
                recipe.AddIngredient(ItemID.TopazStaff);
                recipe.AddIngredient(ItemID.YellowPhaseblade);
                recipe.AddIngredient(ItemID.Daylight);
            }
            else
            {
                recipe.AddIngredient(ItemID.TinBow);
                recipe.AddIngredient(ItemID.TopazStaff);
                recipe.AddIngredient(ItemID.YellowPhaseblade);
                recipe.AddIngredient(ItemID.Daylight);
            }

            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
