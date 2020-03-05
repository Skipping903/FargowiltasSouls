﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class SquireEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Squire Enchantment");
            Tooltip.SetDefault(
@"'Squire, will you hurry?'
Continually attacking an enemy will eventually remove its knockback immunity for 2 seconds
There is a 15 second cooldown per enemy
Ballista pierces more targets and panics when you take damage");
            DisplayName.AddTranslation(GameCulture.Chinese, "精金魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'谁需要瞄准?'
第8个抛射物将会分裂成3个
分裂出的抛射物同样可以分裂");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 8;
            item.value = 150000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().SquireEnchant = true;
            player.setSquireT2 = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SquireGreatHelm);
            recipe.AddIngredient(ItemID.SquirePlating);
            recipe.AddIngredient(ItemID.SquireGreaves);
            recipe.AddIngredient(ItemID.SquireShield);
            recipe.AddIngredient(ItemID.DD2BallistraTowerT2Popper);
            recipe.AddIngredient(ItemID.DD2SquireDemonSword);
            recipe.AddIngredient(ItemID.RedPhasesaber);

//Doom Fire Axe (with Thorium)
//Dragon's Tooth (with Thorium)
//Rapier (with Thorium)
//Warp Slicer (with Thorium)
//Scalper (with Thorium)

            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
