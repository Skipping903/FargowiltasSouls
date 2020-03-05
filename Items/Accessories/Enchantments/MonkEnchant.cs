﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class MonkEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Monk Enchantment");
            Tooltip.SetDefault(
@"'Hours of Meditation have led to this…'
Standing still for 1 second grants you a single use monk dash that works in any cardinal direction
Lightning Aura can now crit and strikes faster");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 7;
            item.value = 150000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().MonkEffect();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.MonkBrows);
            recipe.AddIngredient(ItemID.MonkShirt);
            recipe.AddIngredient(ItemID.MonkPants);
            recipe.AddIngredient(ItemID.MonkBelt);
            recipe.AddIngredient(ItemID.DD2LightningAuraT2Popper);
            recipe.AddIngredient(ItemID.MonkStaffT2);
            recipe.AddIngredient(ItemID.DaoofPow);

            //Schmelze (with Thorium)
            //Rocket Fist (with Thorium)

            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
