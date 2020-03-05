using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using Terraria.Localization;
using ThoriumMod.NPCs;

namespace FargowiltasSouls.Items.Accessories.Enchantments.Thorium
{
    public class WarlockEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override bool Autoload(ref string name)
        {
            return ModLoader.GetMod("ThoriumMod") != null;
        }
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Warlock Enchantment");
            Tooltip.SetDefault(
@"'Better than a wizard'
Critical strikes will generate up to 15 shadow wisps
Pressing the 'Special Ability' key will unleash every stored shadow wisp towards your cursor's position
Effects of Demon Tongue and Dark Effigy
Summons a Li'l Devil to attack enemies");
            DisplayName.AddTranslation(GameCulture.Chinese, "术士魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'比巫师更强'
暴击产生至多15个暗影魂火
按下'特殊能力'键向光标方向释放所有存留的暗影魂火
拥有恶魔之舌的效果
召唤小恶魔攻击敌人");
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
            //set bonus
            thoriumPlayer.warlockSet = true;
            modPlayer.WarlockEnchant = true;
            //lil devil
            modPlayer.AddMinion(SoulConfig.Instance.thoriumToggles.DevilMinion, thorium.ProjectileType("Devil"), 20, 2f);

            if (modPlayer.ThoriumSoul) return;

            //demon tongue
            thoriumPlayer.darkAura = true;
            thoriumPlayer.radiantLifeCost = 2;

            //dark effigy
            thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();

            for (int i = 0; i < 200; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && (npc.shadowFlame || npc.GetGlobalNPC<ThoriumGlobalNPC>().lightLament) && npc.DistanceSQ(player.Center) < 1000000f)
                {
                    thoriumPlayer.effigy++;
                }
            }
            if (thoriumPlayer.effigy > 0)
            {
                player.AddBuff(thorium.BuffType("EffigyRegen"), 2, true);
            }
        }
        
        private readonly string[] items =
        {
            "DemonTongue",
            "Effigy",
            "Omen",
            "ShadowStaff",
            "NecroticStaff",
            "DevilStaff"
        };

        public override void AddRecipes()
        {
            if (!Fargowiltas.Instance.ThoriumLoaded) return;
            
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(thorium.ItemType("WarlockHood"));
            recipe.AddIngredient(thorium.ItemType("WarlockGarb"));
            recipe.AddIngredient(thorium.ItemType("WarlockLeggings"));
            recipe.AddIngredient(null, "EbonEnchant");

            foreach (string i in items) recipe.AddIngredient(thorium.ItemType(i));

            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
