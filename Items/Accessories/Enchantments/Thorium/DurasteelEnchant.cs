using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Enchantments.Thorium
{
    public class DurasteelEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override bool Autoload(ref string name)
        {
            return ModLoader.GetMod("ThoriumMod") != null;
        }
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Durasteel Enchantment");
            Tooltip.SetDefault(
@"'Masterfully forged by the Blacksmith'
12% damage reduction at Full HP
Grants immunity to shambler chain-balls
Effects of the Incandescent Spark, Spiked Bracers, and Greedy Magnet");
            DisplayName.AddTranslation(GameCulture.Chinese, "耐刚魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'精工打造'
满血时增加90%伤害减免
免疫蹒跚者的链球效果
拥有炽热火花, 尖刺索和贪婪磁铁的效果");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 3;
            item.value = 80000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!Fargowiltas.Instance.ThoriumLoaded) return;

            ThoriumPlayer thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();
            //durasteel effect
            if (player.statLife == player.statLifeMax2)
            {
                player.endurance += .12f;
            }

            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.thoriumToggles.IncandescentSpark))
            {
                thorium.GetItem("IncandescentSpark").UpdateAccessory(player, hideVisual);
            }

            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.thoriumToggles.GreedyMagnet))
            {
                thorium.GetItem("GreedyMagnet").HoldItem(player);
            }
            //ball n chain
            thoriumPlayer.ballnChain = true;

            if (player.GetModPlayer<FargoPlayer>().ThoriumSoul) return;

            //spiked bracers
            player.thorns += 0.25f;
        }
        
        private readonly string[] items =
        {
            "IncandescentSpark",
            "GreedyMagnet",
            "DurasteelRepeater",
            "SpudBomber",
            "ThiefDagger",
            "SeaMine"
        };

        public override void AddRecipes()
        {
            if (!Fargowiltas.Instance.ThoriumLoaded) return;
            
            ModRecipe recipe = new ModRecipe(mod);
            
            recipe.AddIngredient(thorium.ItemType("DurasteelHelmet"));
            recipe.AddIngredient(thorium.ItemType("DurasteelChestplate"));
            recipe.AddIngredient(thorium.ItemType("DurasteelGreaves"));
            recipe.AddIngredient(null, "DarksteelEnchant");
            
            foreach (string i in items) recipe.AddIngredient(thorium.ItemType(i));

            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
