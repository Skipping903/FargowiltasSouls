using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Misc
{
    public class LunarCrystal : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lunar Crystal");
            Tooltip.SetDefault("A fragment of the moon's power");
            DisplayName.AddTranslation(GameCulture.Chinese, "月之水晶");
            Tooltip.AddTranslation(GameCulture.Chinese, "月球能量的碎片");
        }

        public override void SetDefaults()
        {
            item.maxStack = 99;
            item.rare = 11;
            item.width = 12;
            item.height = 12;
            item.value = Item.sellPrice(0, 5, 0, 0);
        }
    }
}