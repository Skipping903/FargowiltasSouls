using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Enchantments.Thorium
{
    public class FleshEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override bool Autoload(ref string name)
        {
            return ModLoader.GetMod("ThoriumMod") != null;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flesh Enchantment");
            Tooltip.SetDefault(
@"'Symbiotically attached to your body'
Consecutive attacks against enemies might drop flesh, which grants bonus life and damage
Effects of Vampire Gland
Summons a pet Flying Blister");
            DisplayName.AddTranslation(GameCulture.Chinese, "血肉魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'与你共生'
连续攻击敌人时概率掉落肉, 拾取肉会获得额外生命并增加伤害
拥有吸血鬼试剂的效果
召唤宠物泡泡虫");
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
            //flesh set bonus
            thoriumPlayer.Symbiotic = true;
            //vampire gland
            thoriumPlayer.vampireGland = true;
            //blister pet
            modPlayer.AddPet(SoulConfig.Instance.thoriumToggles.BlisterPet, hideVisual, thorium.BuffType("BlisterBuff"), thorium.ProjectileType("BlisterPet"));
            modPlayer.FleshEnchant = true;
        }
        
        private readonly string[] items =
        {
            "FleshMask",
            "FleshBody",
            "FleshLegs",
            "VampireGland",
            "GrimFlayer",
            "ToothOfTheConsumer",
            "FleshMace",
            "BloodBelcher",
            "TlordsTsword",
            "BlisterSack"
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
