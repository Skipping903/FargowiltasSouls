using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Enchantments.Thorium
{
    public class PyromancerEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override bool Autoload(ref string name)
        {
            return ModLoader.GetMod("ThoriumMod") != null;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pyromancer Enchantment");
            Tooltip.SetDefault(
@"'Your magma fortified army's molten gaze shall be feared'
Attacks will heavily burn and damage all adjacent enemies
Pressing the 'Special Ability' key will unleash an echo of Slag Fury's power");
            DisplayName.AddTranslation(GameCulture.Chinese, "炎法魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'你那熔岩加护的军队炽热的注视令人畏惧'
攻击将焚烧目标, 并伤害所有相邻的敌人
按下'特殊能力'键释放熔火之灵的余烬");
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

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color?(new Color(255, 128, 0));
                }
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!Fargowiltas.Instance.ThoriumLoaded) return;

            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();
            ThoriumPlayer thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();
            //pyro magic set
            modPlayer.PyroEnchant = true;
            //pyro summon bonus
            thoriumPlayer.napalmSet = true;
        }
        
        private readonly string[] items =
        {
            "PyroSummonHat",
            "PyromancerCowl",
            "PyromancerTabard",
            "PyromancerLeggings",
            "StalagmiteBook",
            "DevilDagger",
            "MortarStaff",
            "MoltenBanner",
            "AlmanacofDespair",
            "NorthLight"
        };

        public override void AddRecipes()
        {
            if (!Fargowiltas.Instance.ThoriumLoaded) return;
            
            ModRecipe recipe = new ModRecipe(mod);
            
            foreach (string i in items) recipe.AddIngredient(thorium.ItemType(i));

            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
