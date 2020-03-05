﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.Souls
{
    public class FrostIcicle : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frost Icicle");
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 14;
            projectile.height = 34;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 2;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();

            projectile.timeLeft++;

            if (player.dead)
            {
                modPlayer.FrostEnchant = false;
            }

            if (!(modPlayer.FrostEnchant || modPlayer.TerrariaSoul) || !SoulConfig.Instance.GetValue(SoulConfig.Instance.FrostIcicles))
            {
                projectile.Kill();
                return;
            }

            if (projectile.owner == Main.myPlayer)
            {
                //rotation mumbo jumbo
                float distanceFromPlayer = 32;

                projectile.position = player.Center + new Vector2(distanceFromPlayer, 0f).RotatedBy(projectile.ai[1]);
                projectile.position.X -= projectile.width / 2;
                projectile.position.Y -= projectile.height / 2;

                float rotation = .07f;
                projectile.ai[1] += rotation;
                if (projectile.ai[1] > (float)Math.PI)
                {
                    projectile.ai[1] -= 2f * (float)Math.PI;
                    projectile.netUpdate = true;
                }

                projectile.rotation = (Main.MouseWorld - projectile.Center).ToRotation() - 5;
            }
        }

        public override void Kill(int timeLeft)
        {
            Main.player[projectile.owner].GetModPlayer<FargoPlayer>().IcicleCount--;
        }
    }
}