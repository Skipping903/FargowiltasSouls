using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Enchantments.Thorium
{
    public class GraniteEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override bool Autoload(ref string name)
        {
            return ModLoader.GetMod("ThoriumMod") != null;
        }
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Granite Enchantment");
            Tooltip.SetDefault(
@"'Defensively energized'
Immune to intense heat and enemy knockback, but your movement speed is slowed down greatly
Effects of Eye of the Storm");
            DisplayName.AddTranslation(GameCulture.Chinese, "花岗岩魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'防御激增'
免疫火块灼烧和击退，但大幅度降低移动速度
拥有风暴之眼和充能音箱的效果");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 2;
            item.value = 60000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!Fargowiltas.Instance.ThoriumLoaded) return;

            ThoriumPlayer thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();
            //set bonus
            player.fireWalk = true;
            player.lavaImmune = true;
            player.buffImmune[24] = true;
            player.noKnockback = true;
            player.moveSpeed -= 0.5f;
            player.maxRunSpeed = 4f;

            //eye of the storm
            thorium.GetItem("EyeoftheStorm").UpdateAccessory(player, hideVisual);
        }
        
        private readonly string[] items =
        {
            "EyeoftheStorm",
            "GraniteSaber",
            "EnergyProjector",
            "BoulderProbe",
            "ShockAbsorber"
        };

        public override void AddRecipes()
        {
            if (!Fargowiltas.Instance.ThoriumLoaded) return;
            
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(thorium.ItemType("GraniteHelmet"));
            recipe.AddIngredient(thorium.ItemType("GraniteChestGuard"));
            recipe.AddIngredient(thorium.ItemType("GraniteGreaves"));
            recipe.AddIngredient(ItemID.NightVisionHelmet);

            foreach (string i in items) recipe.AddIngredient(thorium.ItemType(i));

            recipe.AddIngredient(thorium.ItemType("ObsidianStriker"), 300);

            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
