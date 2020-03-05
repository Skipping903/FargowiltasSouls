using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Enchantments.Thorium
{
    public class DragonEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override bool Autoload(ref string name)
        {
            return ModLoader.GetMod("ThoriumMod") != null;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dragon Enchantment");
            Tooltip.SetDefault(
@"'Made from mythical scales'
Your attacks have a chance to unleash an explosion of Dragon's Flame
Effects of Dragon Talon Necklace
Summons a pet Wyvern");
            DisplayName.AddTranslation(GameCulture.Chinese, "绿龙魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'由神秘鳞片制成'
攻击有概率释放龙焰爆炸
拥有龙爪项链的效果
召唤宠物小飞龙");
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
            //dragon 
            thoriumPlayer.dragonSet = true;
            //dragon tooth necklace
            player.armorPenetration += 15;
            //wyvern pet
            modPlayer.AddPet(SoulConfig.Instance.thoriumToggles.WyvernPet, hideVisual, thorium.BuffType("WyvernPetBuff"), thorium.ProjectileType("WyvernPet"));
            modPlayer.DragonEnchant = true;
        }
        
        private readonly string[] items =
        {
            "DragonMask",
            "DragonBreastplate",
            "DragonGreaves",
            "DragonTalonNecklace",
            "DragonsBreath",
            "EbonyTail",
            "DragonkinStaff"
        };

        public override void AddRecipes()
        {
            if (!Fargowiltas.Instance.ThoriumLoaded) return;
            
            ModRecipe recipe = new ModRecipe(mod);

            foreach (string i in items) recipe.AddIngredient(thorium.ItemType(i));

            recipe.AddIngredient(thorium.ItemType("CorrupterBalloon"), 300);
            recipe.AddIngredient(ItemID.ClingerStaff);
            recipe.AddIngredient(thorium.ItemType("CloudyChewToy"));

            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
