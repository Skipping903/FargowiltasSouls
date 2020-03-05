using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class MoltenEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Molten Enchantment");
            Tooltip.SetDefault(
@"'They shall know the fury of hell.' 
Nearby enemies are ignited
The closer they are to you the more damage they take
When you are hurt, you violently explode to damage nearby enemies");
            DisplayName.AddTranslation(GameCulture.Chinese, "熔融魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"他们将感受到地狱的愤怒.'
点燃附近敌人
敌人距离越近, 收到的伤害越多
死亡时剧烈爆炸, 造成大量伤害");
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color(193, 43, 43);
                }
            }
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 3;
            item.value = 50000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().MoltenEffect(20);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MoltenHelmet);
            recipe.AddIngredient(ItemID.MoltenBreastplate);
            recipe.AddIngredient(ItemID.MoltenGreaves);
            
            if(Fargowiltas.Instance.ThoriumLoaded)
            {      
                recipe.AddIngredient(thorium.ItemType("MeleeThorHammer"));
                recipe.AddIngredient(ItemID.MoltenHamaxe);
                recipe.AddIngredient(ItemID.Sunfury);
                recipe.AddIngredient(ItemID.DarkLance);
                recipe.AddIngredient(ItemID.PhoenixBlaster);
                recipe.AddIngredient(thorium.ItemType("MeteoriteClusterBomb"), 300);
                recipe.AddIngredient(ItemID.DemonsEye);
            }
            else
            {
                recipe.AddIngredient(ItemID.Sunfury);
                recipe.AddIngredient(ItemID.DarkLance);
                recipe.AddIngredient(ItemID.PhoenixBlaster);
                recipe.AddIngredient(ItemID.DemonsEye);
            }
            
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
