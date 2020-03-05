using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Enchantments.Thorium
{
    public class LivingWoodEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override bool Autoload(ref string name)
        {
            return ModLoader.GetMod("ThoriumMod") != null;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Living Wood Enchantment");
            Tooltip.SetDefault(
@"'Become one with nature'
Summons a living wood sapling and its attacks will home in on enemies");
            DisplayName.AddTranslation(GameCulture.Chinese, "生命木魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'与自然融为一体'
召唤具有追踪攻击能力的小树苗
拥有植物纤维绳索宝典的效果");
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

            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();
            ThoriumPlayer thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();
            //set bonus
            thoriumPlayer.livingWood = true;
            //free boi
            modPlayer.LivingWoodEnchant = true;
            modPlayer.AddMinion(SoulConfig.Instance.thoriumToggles.SaplingMinion, thorium.ProjectileType("MinionSapling"), 10, 2f);
        }

        public override void AddRecipes()
        {
            if (!Fargowiltas.Instance.ThoriumLoaded) return;
            
            ModRecipe recipe = new ModRecipe(mod);
            
            recipe.AddIngredient(thorium.ItemType("LivingWoodMask"));
            recipe.AddIngredient(thorium.ItemType("LivingWoodChestguard"));
            recipe.AddIngredient(thorium.ItemType("LivingWoodBoots"));
            recipe.AddIngredient(thorium.ItemType("LivingWoodSprout"));
            recipe.AddIngredient(ItemID.SlimeStaff);
            recipe.AddIngredient(ItemID.Blowpipe);
            recipe.AddIngredient(ItemID.LeafWand);
            recipe.AddIngredient(thorium.ItemType( "ChiTea"), 5);
            recipe.AddIngredient(ItemID.TreeNymphButterfly);
            recipe.AddIngredient(ItemID.Grasshopper);

            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
