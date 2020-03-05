using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class AnglerEnchantment : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Angler Enchantment");
            Tooltip.SetDefault(
@"'As long as they aren't all shoes, you can go home happily'
Increases fishing skill
All fishing rods will have 4 extra lures");
            DisplayName.AddTranslation(GameCulture.Chinese, "渔夫魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'只要不全是鞋子, 你可以高高兴兴地回家'
增加钓鱼技能
所有鱼竿将会增加4个额外的鱼饵");
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color(113, 125, 109);
                }
            }
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.value = 100000;
            item.rare = 5;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().FishSoul1 = true;
            player.fishingSkill += 10;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.AnglerHat);
            recipe.AddIngredient(ItemID.AnglerVest);
            recipe.AddIngredient(ItemID.AnglerPants);
            recipe.AddIngredient(ItemID.WoodFishingPole);
            recipe.AddIngredient(ItemID.ReinforcedFishingPole);
            recipe.AddIngredient(ItemID.FiberglassFishingPole);
            recipe.AddIngredient(ItemID.Rockfish);
            recipe.AddIngredient(ItemID.SawtoothShark);
            recipe.AddIngredient(ItemID.FrostDaggerfish, 200);
            recipe.AddRecipeGroup("FargowiltasSouls:AnyFishingTrash", 5);

            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}