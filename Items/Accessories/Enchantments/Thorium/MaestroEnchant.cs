using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Enchantments.Thorium
{
    public class MaestroEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override bool Autoload(ref string name)
        {
            return ModLoader.GetMod("ThoriumMod") != null;
        }
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Maestro Enchantment");
            Tooltip.SetDefault(
@"'I'll be Bach'
Pressing the Special Ability key will summon a chorus of music playing ghosts
While in combat, a rainbow of damaging symphonic symbols will follow your movement and stun enemies
Effects of Metronome and Purple Music Player");
            DisplayName.AddTranslation(GameCulture.Chinese, "指挥魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'我就是现代巴赫'
按下'特殊能力'键召唤亡灵合唱团
掉落的灵感音符双倍强度, 短暂增加音波伤害
拥有节拍器和粉色播放器的效果");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 8;
            item.value = 200000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!Fargowiltas.Instance.ThoriumLoaded) return;

            ThoriumPlayer thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();
            thoriumPlayer.setMaestro = true;

            if (player.GetModPlayer<FargoPlayer>().ThoriumSoul) return;

            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.thoriumToggles.Metronome))
            {
                thorium.GetItem("Metronome").UpdateAccessory(player, hideVisual);
            }

            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.thoriumToggles.MarchingBand))
            {
                thoriumPlayer.setMarchingBand = true;
            }
        }
        
        private readonly string[] items =
        {
            "Metronome",
            "ConductorsBaton",
            "Organ",
            "Clarinet",
            "FrenchHorn",
            "Saxophone"
        };

        public override void AddRecipes()
        {
            if (!Fargowiltas.Instance.ThoriumLoaded) return;
            
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(thorium.ItemType("MaestroWig"));
            recipe.AddIngredient(thorium.ItemType("MaestroSuit"));
            recipe.AddIngredient(thorium.ItemType("MaestroLeggings"));
            recipe.AddIngredient(null, "MarchingBandEnchant");

            foreach (string i in items) recipe.AddIngredient(thorium.ItemType(i));

            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
