using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Enchantments.Thorium
{
    public class MarchingBandEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override bool Autoload(ref string name)
        {
            return ModLoader.GetMod("ThoriumMod") != null;
        }
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Marching Band Enchantment");
            Tooltip.SetDefault(
@"'Step to the beat'
While in combat, a rainbow of damaging symphonic symbols will follow your movement and stun enemies");
            DisplayName.AddTranslation(GameCulture.Chinese, "仪仗队魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'脚步合拍'
掉落的灵感音符双倍强度, 短暂增加音波伤害");
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

            ThoriumPlayer thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();

            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.thoriumToggles.MarchingBand))
            {
                //marching band set 
                thoriumPlayer.setMarchingBand = true;
            }
            
        }
        
        private readonly string[] items =
        {
            "MarchingBandCap",
            "MarchingBandUniform",
            "MarchingBandLeggings",
            "FullScore",
            "Cymbals",
            "Violin",
            "Chimes",
            "Trombone",
            "SummonerWarhorn",
            "Tuba"
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
