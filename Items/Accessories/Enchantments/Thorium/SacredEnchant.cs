using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Enchantments.Thorium
{
    public class SacredEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");
        public int timer;

        public override bool Autoload(ref string name)
        {
            return ModLoader.GetMod("ThoriumMod") != null;
        }
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sacred Enchantment");
            Tooltip.SetDefault(
@"'It glimmers with comforting power'
Healing potions heal 50% more life
Every 5 seconds you generate up to 3 holy crosses
When casting healing spells, a cross is used instead of mana
Effects of Karmic Holder
Summons a Li'l Cherub to periodically heal damaged allies
Summons a pet Life Spirit");
            DisplayName.AddTranslation(GameCulture.Chinese, "圣骑士魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'闪耀抚慰人心的力量'
生命药水额外回复50%生命值
每5秒产生一个圣十字架, 上限为3个
施放治疗法术时, 十字架将代替魔力消耗
召唤小天使周期性治疗队友
召唤宠物生命之灵");
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
            //sacred effect
            modPlayer.SacredEnchant = true;
            //lil cherub
            modPlayer.AddMinion(SoulConfig.Instance.thoriumToggles.CherubMinion, thorium.ProjectileType("Angel"), 0, 0f);
            //twinkle pet
            modPlayer.AddPet(SoulConfig.Instance.thoriumToggles.SpiritPet, hideVisual, thorium.BuffType("LifeSpiritBuff"), thorium.ProjectileType("LifeSpirit"));

            if (modPlayer.ThoriumSoul) return;

            //novice cleric set bonus
            thoriumPlayer.clericSet = true;
            thoriumPlayer.orbital = true;
            thoriumPlayer.orbitalRotation3 = Utils.RotatedBy(thoriumPlayer.orbitalRotation3, -0.05000000074505806, default(Vector2));
            timer++;
            if (thoriumPlayer.clericSetCrosses < 3)
            {
                if (timer > 300)
                {
                    thoriumPlayer.clericSetCrosses++;
                    timer = 0;
                    return;
                }
            }
            else
            {
                timer = 0;
            }

            //karmic holder
            thoriumPlayer.karmicHolder = true;
            if (thoriumPlayer.healStreak >= 0 && player.ownedProjectileCounts[thorium.ProjectileType("KarmicHolderPro")] < 1)
            {
                Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, thorium.ProjectileType("KarmicHolderPro"), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }
        
        private readonly string[] items =
        {
            "KarmicHolder",
            "LightBurstWand",
            "HallowedBludgeon",
            "AngelStaff",
            "Liberation",
            "Twinkle"
        };

        public override void AddRecipes()
        {
            if (!Fargowiltas.Instance.ThoriumLoaded) return;
            
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(thorium.ItemType("HallowedPaladinHelmet"));
            recipe.AddIngredient(thorium.ItemType("HallowedPaladinBreastplate"));
            recipe.AddIngredient(thorium.ItemType("HallowedPaladinLeggings"));
            recipe.AddIngredient(null, "NoviceClericEnchant");

            foreach (string i in items) recipe.AddIngredient(thorium.ItemType(i));

            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
