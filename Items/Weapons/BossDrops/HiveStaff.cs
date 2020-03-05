﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ID;

namespace FargowiltasSouls.Items.Weapons.BossDrops
{
    public class HiveStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hive Staff");
            Tooltip.SetDefault("'The enslaved minions of a defeated foe..'");
            DisplayName.AddTranslation(GameCulture.Chinese, "蜂巢法杖");
            Tooltip.AddTranslation(GameCulture.Chinese, "'战败敌人的仆从..'");
        }

        public override void SetDefaults()
        {
            item.damage = 15;
            item.summon = true;
            item.width = 24;
            item.height = 24;
            item.useTime = 15;
            item.useAnimation = 15;
            item.useStyle = 1;
            item.noMelee = true;
            item.UseSound = SoundID.Item78;
            item.value = 50000;
            item.rare = 3;
            item.shoot = mod.ProjectileType("HiveSentry");
            item.shootSpeed = 20f;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 mouse = Main.MouseWorld;

            Projectile.NewProjectile(mouse.X, mouse.Y - 10, 0f, 0f, type, damage, knockBack, player.whoAmI);

            player.UpdateMaxTurrets();

            return false;
        }
    }
}