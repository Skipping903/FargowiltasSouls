using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class NecroEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Necro Enchantment");
            Tooltip.SetDefault(
@"'Welcome to the bone zone' 
A Dungeon Guardian will occasionally annihilate a foe when struck by any attack
Summons a pet Skeletron Head");
            DisplayName.AddTranslation(GameCulture.Chinese, "死灵魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'欢迎来到骸骨领域'
地牢守卫者偶尔会在你受到攻击时消灭敌人
召唤一个小骷髅头");
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color(86, 86, 67);
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
            player.GetModPlayer<FargoPlayer>().NecroEffect(hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.NecroHelmet);
            recipe.AddIngredient(ItemID.NecroBreastplate);
            recipe.AddIngredient(ItemID.NecroGreaves);
            recipe.AddIngredient(ItemID.BoneSword);
            
            if(Fargowiltas.Instance.ThoriumLoaded)
            {      
                recipe.AddIngredient(thorium.ItemType("Slugger"));
                recipe.AddIngredient(ItemID.BoneGlove);
                recipe.AddIngredient(ItemID.Marrow);
                recipe.AddIngredient(thorium.ItemType("BoneFlayerTail"));
                recipe.AddIngredient(ItemID.TheGuardiansGaze);
            }
            else
            {
                recipe.AddIngredient(ItemID.Marrow);
                recipe.AddIngredient(ItemID.TheGuardiansGaze);
            }
            
            recipe.AddIngredient(ItemID.BoneKey);
            
            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
