using Terraria;
using Terraria.ModLoader;

namespace FargowiltasSouls.Items.Tiles
{
    public class TophatSquirrelBanner : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 14;
            item.height = 36;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.rare = 1;
            item.value = Item.buyPrice(0, 0, 10, 0);
            item.createTile = mod.TileType("FMMBanner");
            item.placeStyle = 0;
            //DisplayName.AddTranslation(GameCulture.Chinese, "高顶礼帽松鼠旗帜"); this broke for some reason :ech:
        }
    }
}