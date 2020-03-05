using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Enchantments.Thorium
{
    public class SandstoneEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override bool Autoload(ref string name)
        {
            return ModLoader.GetMod("ThoriumMod") != null;
        }
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sandstone Enchantment");
            Tooltip.SetDefault(
@"'Enveloped by desert winds'
Desert winds will augment your boots, giving you a double jump");
            DisplayName.AddTranslation(GameCulture.Chinese, "砂石魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'沙暴环绕'
沙暴增强了你的靴子, 能够额外跳跃一次");
            //Thrown attacks might refresh your jump
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 1;
            item.value = 40000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!Fargowiltas.Instance.ThoriumLoaded) return;

            ThoriumPlayer thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();
            //set bonus
            player.doubleJumpSandstorm = true;
            if (Main.rand.Next(25) == 0)
            {
                Projectile.NewProjectile(player.Center.X - 4f, player.Center.Y, 0f, 0f, thorium.ProjectileType("SandstoneEffect"), 0, 0f, Main.myPlayer, 0f, 0f);
            }
        }

        public override void AddRecipes()
        {
            if (!Fargowiltas.Instance.ThoriumLoaded) return;
            
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(thorium.ItemType("hSandStoneHelmet"));
            recipe.AddIngredient(thorium.ItemType("iSandStoneMail"));
            recipe.AddIngredient(thorium.ItemType("jSandStoneGreaves"));
            recipe.AddIngredient(thorium.ItemType("Wreath"));
            recipe.AddIngredient(thorium.ItemType("BaseballBat"));
            recipe.AddIngredient(thorium.ItemType("StoneThrowingSpear"), 300);
            recipe.AddIngredient(thorium.ItemType("OceanTomahawk"), 300);
            recipe.AddIngredient(thorium.ItemType("gSandStoneThrowingKnife"), 300);
            recipe.AddIngredient(thorium.ItemType("TalonBurst"));
            recipe.AddIngredient(ItemID.BlackScorpion);

            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
