using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Enchantments.Thorium
{
    public class FeralFurEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override bool Autoload(ref string name)
        {
            return ModLoader.GetMod("ThoriumMod") != null;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Feral-Fur Enchantment");
            Tooltip.SetDefault(
@"'Let your inner animal out'
Critical strikes grant Alpha's Roar, briefly increasing the damage of your summoned minions");
            DisplayName.AddTranslation(GameCulture.Chinese, "兽皮魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'唤醒内心的野兽'
暴击获得野性咆哮效果, 并短暂增加召唤物伤害");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 4;
            item.value = 120000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!Fargowiltas.Instance.ThoriumLoaded) return;

            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();
            ThoriumPlayer thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();
            //feral set bonus
            modPlayer.FeralFurEnchant = true;
        }
        
        private readonly string[] items =
        {
            "FeralSkinHead",
            "FeralSkinChest",
            "FeralSkinLegs",
            "BlackCatEars",
            "Lullaby",
            "SacrificialDagger",
            "MeteorBarrier",
            "CrimsonSummon",
            "BloodCellStaff",
            "BackStabber"
        };

        public override void AddRecipes()
        {
            if (!Fargowiltas.Instance.ThoriumLoaded) return;
            
            ModRecipe recipe = new ModRecipe(mod);
            
            foreach (string i in items) recipe.AddIngredient(thorium.ItemType(i));

            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
