using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Enchantments.Thorium
{
    public class TerrariumEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");
        public int timer;

        public override bool Autoload(ref string name)
        {
            return ModLoader.GetMod("ThoriumMod") != null;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terrarium Enchantment");
            Tooltip.SetDefault(
@"'All will fall before your might...'
The energy of Terraria seeks to protect you
Shortlived Divermen will occasionally spawn when hitting enemies
Critical strikes ring a bell over your head, slowing all nearby enemies briefly
Effects of Crietz and Band of Replenishmen
Effects of Terrarium Surround Sound and Fan Letter");
            DisplayName.AddTranslation(GameCulture.Chinese, "元素之灵魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'万物都臣服于你的力量...'
泰拉瑞亚的能量试图保护你
攻击敌人时偶尔会召唤暂时存在的潜水员
暴击短暂缓慢所有附近敌人
拥有精准项链和大恢复戒指的效果
拥有粉丝的信函和界元音箱的效果");
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color?(new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB));
                }
            }
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 10; //rainbow
            item.value = 250000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!Fargowiltas.Instance.ThoriumLoaded) return;

            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();
            ThoriumPlayer thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();
            //terrarium set bonus
            timer++;
            if (timer > 60)
            {
                Projectile.NewProjectile(player.Center.X + 14f, player.Center.Y - 20f, 0f, 2f, thorium.ProjectileType("TerraRed"), 50, 0f, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(player.Center.X + 9f, player.Center.Y - 20f, 0f, 2f, thorium.ProjectileType("TerraOrange"), 50, 0f, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(player.Center.X + 4f, player.Center.Y - 20f, 0f, 2f, thorium.ProjectileType("TerraYellow"), 50, 0f, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(player.Center.X, player.Center.Y - 20f, 0f, 2f, thorium.ProjectileType("TerraGreen"), 50, 0f, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(player.Center.X - 4f, player.Center.Y - 20f, 0f, 2f, thorium.ProjectileType("TerraBlue"), 50, 0f, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(player.Center.X - 9f, player.Center.Y - 20f, 0f, 2f, thorium.ProjectileType("TerraIndigo"), 50, 0f, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(player.Center.X - 14f, player.Center.Y - 20f, 0f, 2f, thorium.ProjectileType("TerraPurple"), 50, 0f, Main.myPlayer, 0f, 0f);
                timer = 0;
            }
            //subwoofer
            thoriumPlayer.accSubwooferTerrarium = true;

            //diverman meme
            modPlayer.ThoriumEnchant = true;
            //crietz
            thoriumPlayer.crietzAcc = true;
            //band of replenishment
            thoriumPlayer.BandofRep = true;
            //jester bonus
            modPlayer.JesterEnchant = true;
            //fan letter
            thoriumPlayer.bardResourceMax2 += 2;
        }
        
        private readonly string[] items =
        {
            "TerrariumSubwoofer",
            "EssenceofFlame",
            "TerrariumSaber",
            "SickThrow",
            "TerrariumBomber",
            "TerrariumKnife"
        };

        public override void AddRecipes()
        {
            if (!Fargowiltas.Instance.ThoriumLoaded) return;
            
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(thorium.ItemType("TerrariumHelmet"));
            recipe.AddIngredient(thorium.ItemType("TerrariumBreastPlate"));
            recipe.AddIngredient(thorium.ItemType("TerrariumGreaves"));
            recipe.AddIngredient(null, "ThoriumEnchant");

            foreach (string i in items) recipe.AddIngredient(thorium.ItemType(i));

            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
