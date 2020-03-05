using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class ShroomiteEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shroomite Enchantment");

            string tooltip =
@"'Made with real shrooms!'
Not moving puts you in stealth
While in stealth, crits deal 3x damage
Summons a pet Truffle";
            string tooltip_ch =
@"'真的是用蘑菇做的!'
站立不动时潜行
潜行时, 暴击造成3倍伤害
召唤一个小蘑菇人";

            Tooltip.SetDefault(tooltip); 
            DisplayName.AddTranslation(GameCulture.Chinese, "蘑菇魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, tooltip_ch);
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color(0, 140, 244);
                }
            }
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 8;
            item.value = 250000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().ShroomiteEffect(hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("FargowiltasSouls:AnyShroomHead");
            recipe.AddIngredient(ItemID.ShroomiteBreastplate);
            recipe.AddIngredient(ItemID.ShroomiteLeggings);
            
            if(Fargowiltas.Instance.ThoriumLoaded)
            {

                recipe.AddIngredient(ItemID.MushroomSpear);
                recipe.AddIngredient(thorium.ItemType("MyceliumGattlingPulser"));
                recipe.AddIngredient(thorium.ItemType("Funggat"));
                recipe.AddIngredient(ItemID.Uzi);
                recipe.AddIngredient(ItemID.TacticalShotgun);
                recipe.AddIngredient(thorium.ItemType("RedFragmentBlaster"));
            }
            else
            {
                recipe.AddIngredient(ItemID.MushroomSpear);
                recipe.AddIngredient(ItemID.Uzi);
                recipe.AddIngredient(ItemID.TacticalShotgun);
            }
            
            recipe.AddIngredient(ItemID.StrangeGlowingMushroom);
            
            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
