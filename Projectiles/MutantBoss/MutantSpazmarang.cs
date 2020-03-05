﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.MutantBoss
{
    public class MutantSpazmarang : ModProjectile
    {
        public override string Texture => "FargowiltasSouls/Projectiles/BossWeapons/Spazmarang";
        private int counter = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spazmarang");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.scale = 1.5f;
            projectile.penetrate = -1;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.aiStyle = -1;
            cooldownSlot = 1;
        }

        public override void AI()
        {
            if (++projectile.localAI[0] > projectile.ai[1])
                projectile.Kill();

            /*if (projectile.localAI[0] == (int)projectile.ai[1] / 2 && Main.netMode != 1)
            {
                Projectile.NewProjectile(projectile.Center, Vector2.Normalize(projectile.velocity).RotatedBy(Math.PI / 2) * 14, ProjectileID.CursedFlameHostile, projectile.damage, 0, Main.myPlayer);
                Projectile.NewProjectile(projectile.Center, Vector2.Normalize(projectile.velocity).RotatedBy(Math.PI / 2) * -14, ProjectileID.CursedFlameHostile, projectile.damage, 0, Main.myPlayer);
            }*/

            Vector2 acceleration = Vector2.Normalize(projectile.velocity).RotatedBy(Math.PI / 2) * projectile.ai[0];
            projectile.velocity += acceleration;

            projectile.rotation += 1f * Math.Sign(projectile.ai[0]);

            int dustId = Dust.NewDust(projectile.position, projectile.width, projectile.height, 75, projectile.velocity.X * 0.2f,
                projectile.velocity.Y * 0.2f, 100, default(Color), 2f);
            Main.dust[dustId].noGravity = true;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.CursedInferno, 120);
            target.AddBuff(mod.BuffType("MutantFang"), 180);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture2D13 = Main.projectileTexture[projectile.type];
            int num156 = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;

            Color color26 = lightColor;
            color26 = projectile.GetAlpha(color26);

            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[projectile.type]; i += 2)
            {
                Color color27 = color26;
                color27 *= (float)(ProjectileID.Sets.TrailCacheLength[projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[projectile.type];
                Vector2 value4 = projectile.oldPos[i];
                float num165 = projectile.oldRot[i];
                Main.spriteBatch.Draw(texture2D13, value4 + projectile.Size / 2f - Main.screenPosition + new Vector2(0, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, num165, origin2, projectile.scale, SpriteEffects.None, 0f);
            }

            Main.spriteBatch.Draw(texture2D13, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), projectile.GetAlpha(lightColor), projectile.rotation, origin2, projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}