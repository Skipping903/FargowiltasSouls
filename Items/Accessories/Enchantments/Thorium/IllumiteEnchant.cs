using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Enchantments.Thorium
{
    public class IllumiteEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override bool Autoload(ref string name)
        {
            return ModLoader.GetMod("ThoriumMod") != null;
        }
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Illumite Enchantment");
            Tooltip.SetDefault(
@"'As if you weren't pink enough'
Every third attack will unleash an illumite missile
Effects of Jazz Music Player
Summons a pet Pink Slime");
            DisplayName.AddTranslation(GameCulture.Chinese, "荧光魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'好像还不够粉'
每3次攻击会发射荧光导弹
拥有粉色播放器的效果
召唤宠物粉红史莱姆");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 7;
            item.value = 200000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!Fargowiltas.Instance.ThoriumLoaded) return;

            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();
            ThoriumPlayer thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();
            modPlayer.IllumiteEnchant = true;
            //music player
            thoriumPlayer.accMusicPlayer = true;
            thoriumPlayer.accMP3Wind = true;
            //slime pet
            modPlayer.AddPet(SoulConfig.Instance.thoriumToggles.SlimePet, hideVisual, thorium.BuffType("PinkSlimeBuff"), thorium.ProjectileType("PinkSlime"));
        }
        
        private readonly string[] items =
        {
            "IllumiteMask",
            "IllumiteChestplate",
            "IllumiteGreaves",
            "TunePlayerLifeRegen",
            "CupidString",
            "ShusWrath",
            "HandCannon",
            "IllumiteBlaster",
            "IllumiteBarrage",
            "PinkSludge"
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
