using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class JungleEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");
        public int jungleCD = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jungle Enchantment");

            string tooltip =
@"'The wrath of the jungle dwells within'
Jumping will release a lingering spore explosion
All herb collection is doubled
Effects of Guide to Plant Fiber Cordage";

            string tooltip_ch =
@"'丛林之怒深藏其中'
受到伤害会释放出有毒的孢子爆炸
所有草药收获翻倍
拥有植物纤维绳索指南的效果";

            Tooltip.SetDefault(tooltip);
            DisplayName.AddTranslation(GameCulture.Chinese, "丛林魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, tooltip_ch);
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color(113, 151, 31);
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
            player.GetModPlayer<FargoPlayer>().JungleEffect();            
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.JungleHat);
            recipe.AddIngredient(ItemID.JungleShirt);
            recipe.AddIngredient(ItemID.JunglePants);
            recipe.AddIngredient(ItemID.CordageGuide);
            recipe.AddIngredient(ItemID.JungleRose);
            recipe.AddIngredient(ItemID.ThornChakram);

            if (Fargowiltas.Instance.ThoriumLoaded)
            {      
                recipe.AddIngredient(ItemID.PoisonedKnife, 300);
                recipe.AddIngredient(thorium.ItemType("MantisCane"));
                recipe.AddIngredient(thorium.ItemType("RivetingTadpole"));
            }

            recipe.AddIngredient(ItemID.Buggy);

            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
