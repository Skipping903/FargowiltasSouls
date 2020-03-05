using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Enchantments.Thorium
{
    public class BronzeEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");
        public int timer;

        public override bool Autoload(ref string name)
        {
            return ModLoader.GetMod("ThoriumMod") != null;
        }
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bronze Enchantment");
            Tooltip.SetDefault(
@"'You have the favor of Zeus'
Attacks have a chance to cause a lightning bolt to strike
Effects of Olympic Torch, Champion's Rebuttal, and Spartan Sandals");
            DisplayName.AddTranslation(GameCulture.Chinese, "青铜魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'宙斯的青睐'
攻击有概率释放闪电链
拥有奥林匹克圣火, 反击之盾, 斯巴达凉鞋和斯巴达音箱的效果");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 2;
            item.value = 60000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!Fargowiltas.Instance.ThoriumLoaded) return;

            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();
            ThoriumPlayer thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();
            //lightning
            modPlayer.BronzeEnchant = true;
            //rebuttal
            thoriumPlayer.championShield = true;
            //sandles
            thorium.GetItem("SpartanSandles").UpdateAccessory(player, hideVisual);
            player.moveSpeed -= 0.15f;
            player.maxRunSpeed -= 1f;
            //olympic torch
            thoriumPlayer.olympicTorch = true;
        }

        public override void AddRecipes()
        {
            if (!Fargowiltas.Instance.ThoriumLoaded) return;
            
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(thorium.ItemType("BronzeHelmet"));
            recipe.AddIngredient(thorium.ItemType("BronzeBreastplate"));
            recipe.AddIngredient(thorium.ItemType("BronzeGreaves"));
            recipe.AddIngredient(thorium.ItemType("OlympicTorch"));
            recipe.AddIngredient(thorium.ItemType("ChampionsBarrier"));
            recipe.AddIngredient(thorium.ItemType("SpartanSandles"));
            recipe.AddIngredient(thorium.ItemType("ChampionBlade"));
            recipe.AddIngredient(thorium.ItemType("SpikyCaltrop"), 300);
            recipe.AddIngredient(thorium.ItemType("BronzeThrowing"), 300);
            recipe.AddIngredient(thorium.ItemType("GraniteThrowingAxe"), 300);

            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
