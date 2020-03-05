using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Weapons.SwarmDrops
{
    public class SlimeSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Slime Slinging Slasher");
            Tooltip.SetDefault("'The reward for slaughtering many..'");
            DisplayName.AddTranslation(GameCulture.Chinese, "史莱姆抛射屠戮者");
            Tooltip.AddTranslation(GameCulture.Chinese, "'屠戮众多的奖励..'");
        }

        public override void SetDefaults()
        {
            item.damage = 120;
            item.melee = true;
            item.width = 40;
            item.height = 40;
            item.useTime = 12;
            item.useAnimation = 12;
            item.useStyle = 1;
            item.knockBack = 6;
            item.value = 50000;
            item.rare = 11;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("SlimeBall");
            item.shootSpeed = 12f;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockback)
        {
            int numberProjectiles = 5 + Main.rand.Next(6); // 5 to 10 shots
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 velocity = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(15)); // 15 degree spread.
                Projectile.NewProjectile(position.X, position.Y, velocity.X, velocity.Y, type, damage / 2, knockback, player.whoAmI);
            }

            return false;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Slimed, 180);
        }

        public override void AddRecipes()
        {
            if (Fargowiltas.Instance.FargowiltasLoaded)
            {
                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddIngredient(null, "SlimeKingsSlasher");
                recipe.AddIngredient(ModLoader.GetMod("Fargowiltas").ItemType("EnergizerSlime"));
                recipe.AddIngredient(ItemID.LunarBar, 10);
                recipe.AddIngredient(null, "LunarCrystal", 5);

                recipe.AddTile(mod, "CrucibleCosmosSheet");
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }
    }
}