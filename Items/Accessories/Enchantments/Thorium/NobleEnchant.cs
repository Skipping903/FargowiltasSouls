using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Enchantments.Thorium
{
    public class NobleEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override bool Autoload(ref string name)
        {
            return ModLoader.GetMod("ThoriumMod") != null;
        }
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Noble Enchantment");
            Tooltip.SetDefault(
@"'Rich with culture'
Inspiration notes that drop are twice as potent and increase your symphonic damage briefly
Effects of Ring of Unity and Mix Tape");
            DisplayName.AddTranslation(GameCulture.Chinese, "贵族魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'富有, 并且有教养'
咒音Buff能额外持续5秒
拥有杂集磁带, 团结之戒和恶魔音箱的效果");
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

            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();
            ThoriumPlayer thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();
            
            //ring of unity
            thorium.GetItem("RingofUnity").UpdateAccessory(player, hideVisual);

            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.thoriumToggles.MixTape))
            {
                //mix tape
                modPlayer.MixTape = true;
            }
            
            if (modPlayer.ThoriumSoul) return;

            //noble set bonus
            thoriumPlayer.setNoble = true;
        }
        
        private readonly string[] items =
        {
            "NoblesHat",
            "NoblesJerkin",
            "NoblesLeggings",
            "RingofUnity",
            "BrassCap",
            "WaxyRosin",
            "JarOMayo",
            "Microphone",
            "Bongos",
            "Nocturne"
        };

        public override void AddRecipes()
        {
            if (!Fargowiltas.Instance.ThoriumLoaded) return;
            
            ModRecipe recipe = new ModRecipe(mod);
            
            foreach (string i in items) recipe.AddIngredient(thorium.ItemType(i));

            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
