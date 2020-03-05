using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Enchantments.Thorium
{
    public class OceanEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override bool Autoload(ref string name)
        {
            return ModLoader.GetMod("ThoriumMod") != null;
        }
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ocean Enchantment");
            Tooltip.SetDefault(
@"'For swimming with the fishes'
Allows you to breathe underwater
Grants the ability to swim
Effects of Sea Breeze Pendant and Bubble Magnet");
            DisplayName.AddTranslation(GameCulture.Chinese, "海洋魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'与鱼同游'
获得水下呼吸能力
获得游泳能力
拥有海洋通行证和泡泡磁铁的效果");
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
            //set bonus, breath underwater
            if (player.breath <= player.breathMax + 2)
            {
                player.breath = player.breathMax + 3;
            }
            //sea breeze pendant
            player.accFlipper = true;

            if (player.wet || thoriumPlayer.drownedDoubloon)
            {
                player.AddBuff(thorium.BuffType("AquaticAptitude"), 60, true);
                player.GetModPlayer<FargoPlayer>().AllDamageUp(.1f);
            }

            //bubble magnet
            thoriumPlayer.bubbleMagnet = true;
        }
        
        private readonly string[] items =
        {
            "OceanHelmet",
            "OceanChestGuard",
            "OceanGreaves",
            "SeaBreezePendant",
            "BubbleMagnet",
            "OceanSlasher",
            "OceanPolearm",
            "StarfishWand"
        };

        public override void AddRecipes()
        {
            if (!Fargowiltas.Instance.ThoriumLoaded) return;
            
            ModRecipe recipe = new ModRecipe(mod);
            
            foreach (string i in items) recipe.AddIngredient(thorium.ItemType(i));

            recipe.AddIngredient(ItemID.Swordfish);
            recipe.AddIngredient(ItemID.Tuna);

            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
