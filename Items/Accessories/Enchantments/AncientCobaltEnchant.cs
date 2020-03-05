﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class AncientCobaltEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Cobalt Enchantment");
            Tooltip.SetDefault(
@"'The jungle of old empowers you'
20% chance for your projectiles to explode into stingers
This can only happen once every second");
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
            player.GetModPlayer<FargoPlayer>().AncientCobaltEnchant = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.AncientCobaltHelmet);
            recipe.AddIngredient(ItemID.AncientCobaltBreastplate);
            recipe.AddIngredient(ItemID.AncientCobaltLeggings);
            recipe.AddIngredient(ItemID.AncientIronHelmet);
            recipe.AddIngredient(ItemID.Blowpipe);
            recipe.AddIngredient(ItemID.PoisonDart, 300);
            recipe.AddIngredient(ItemID.PoisonedKnife, 300);
            
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
