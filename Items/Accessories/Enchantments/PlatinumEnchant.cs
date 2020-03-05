using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using Terraria.Localization;
using System.Collections.Generic;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class PlatinumEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Platinum Enchantment");

            string tooltip = @"'Its value is immeasurable'
10% chance for enemies to drop 4x loot
If the enemy has Midas, the chance and bonus is doubled";
            string tooltip_ch = @"'价值不可估量'
敌人10%概率4倍掉落
如果敌人带有点金手状态,概率和加成翻倍";

            if(thorium != null)
            {
                tooltip +=
@"
Summons a pet Glitter";
                tooltip_ch +=
@"
拥有铂金之庇护的效果
召唤一团闪光";
            }

            Tooltip.SetDefault(tooltip);
            DisplayName.AddTranslation(GameCulture.Chinese, "铂金魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, tooltip_ch);
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color(83, 103, 143);
                }
            }
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 4;
            item.value = 100000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();
            modPlayer.PlatinumEnchant = true;

            if (Fargowiltas.Instance.ThoriumLoaded)
                modPlayer.AddPet(SoulConfig.Instance.thoriumToggles.GlitterPet, hideVisual, thorium.BuffType("ShineDust"), thorium.ProjectileType("ShinyPet"));
        }

        private void Thorium(Player player, bool hideVisual)
        {
            ThoriumPlayer thoriumPlayer = (ThoriumPlayer)player.GetModPlayer(thorium, "ThoriumPlayer");
            
            
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.PlatinumHelmet);
            recipe.AddIngredient(ItemID.PlatinumChainmail);
            recipe.AddIngredient(ItemID.PlatinumGreaves);
            recipe.AddIngredient(ItemID.PlatinumCrown);
            
            if(Fargowiltas.Instance.ThoriumLoaded)
            {      
                recipe.AddIngredient(thorium.ItemType("PlatinumAegis"));
                recipe.AddIngredient(ItemID.DiamondRing);
                recipe.AddIngredient(ItemID.TaxCollectorsStickOfDoom);
                recipe.AddIngredient(ItemID.WhitePhasesaber);
                recipe.AddIngredient(ItemID.BeamSword);
                recipe.AddIngredient(thorium.ItemType("ShinyObject"));
            }
            else
            {
                recipe.AddIngredient(ItemID.DiamondRing);
                recipe.AddIngredient(ItemID.TaxCollectorsStickOfDoom);
                recipe.AddIngredient(ItemID.BeamSword);
            }

            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
