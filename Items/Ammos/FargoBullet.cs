using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Ammos
{
    public class FargoBullet : ModItem
    {
        Mod fargos = ModLoader.GetMod("Fargowiltas");

        public override bool Autoload(ref string name)
        {
            return ModLoader.GetMod("Fargowiltas") != null;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Amalgamated Bullet Pouch");
            Tooltip.SetDefault("Chases after your enemy\n" +
                               "Bounces several times\n" +
                               "Each impact causes an explosion of crystal shards\n" +
                               "Inflicts several debuffs");
            DisplayName.AddTranslation(GameCulture.Chinese, "混合子弹袋");
            Tooltip.AddTranslation(GameCulture.Chinese, 
                               "追踪敌人\n" +
                               "弹跳多次\n" +
                               "每次撞击都会造成魔晶碎片爆炸\n" +
                               "造成多种Debuff");
        }

        public override void SetDefaults()
        {
            item.damage = 20;
            item.ranged = true;
            item.width = 26;
            item.height = 26;
            item.knockBack = 4f; //same as explosive
            item.rare = 10;
            item.shoot = mod.ProjectileType("FargoBulletProj");
            item.shootSpeed = 15f; // same as high velocity bullets                 
            item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            if (!Fargowiltas.Instance.FargowiltasLoaded) return;
            
            ModRecipe recipe = new ModRecipe(mod);
            //recipe.AddIngredient(ItemID.EndlessMusketPouch);
            recipe.AddIngredient(fargos, "SilverPouch");
            recipe.AddIngredient(fargos, "MeteorPouch");
            recipe.AddIngredient(fargos, "CursedPouch");
            recipe.AddIngredient(fargos, "IchorPouch");
            recipe.AddIngredient(fargos, "CrystalPouch");
            recipe.AddIngredient(fargos, "VelocityPouch");
            recipe.AddIngredient(fargos, "VenomPouch");
            recipe.AddIngredient(fargos, "ExplosivePouch");
            recipe.AddIngredient(fargos, "GoldenPouch");
            recipe.AddIngredient(fargos, "PartyPouch");
            recipe.AddIngredient(fargos, "ChlorophytePouch");
            recipe.AddIngredient(fargos, "NanoPouch");
            recipe.AddIngredient(fargos, "LuminitePouch");
            recipe.AddIngredient(mod.ItemType("Sadism"), 15);
            recipe.AddTile(mod, "CrucibleCosmosSheet");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
