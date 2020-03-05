﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.Souls
{
    public class AncientShadowOrb : ModProjectile
    {
        public override string Texture => "Terraria/Projectile_18";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Shadow Orb");
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 32;
            projectile.height = 32;
            projectile.friendly = true;
            Main.projPet[projectile.type] = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 18000;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;



            ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();

            if (player.dead || !(modPlayer.AncientShadowEnchant || modPlayer.TerrariaSoul) || !SoulConfig.Instance.GetValue(SoulConfig.Instance.AncientShadow))
            {
                modPlayer.AncientShadowEnchant = false;
                projectile.Kill();
                return;
            }

            float num395 = Main.mouseTextColor / 200f - 0.35f;
            num395 *= 0.2f;
            projectile.scale = num395 + 0.95f;

            if (projectile.owner == Main.myPlayer)
            {
                //rotation mumbo jumbo
                float distanceFromPlayer = 150;

                Lighting.AddLight(projectile.Center, 0.1f, 0.4f, 0.2f);

                projectile.position = player.Center + new Vector2(distanceFromPlayer, 0f).RotatedBy(projectile.ai[1]);
                projectile.position.X -= projectile.width / 2;
                projectile.position.Y -= projectile.height / 2;
                float rotation = 0.03f;
                projectile.ai[1] -= rotation;
                if (projectile.ai[1] > (float)Math.PI)
                {
                    projectile.ai[1] -= 2f * (float)Math.PI;
                    projectile.netUpdate = true;
                }
                projectile.rotation = projectile.ai[1] + (float)Math.PI / 2f;


                //wait for CD
                if (projectile.ai[0] != 0f)
                {
                    projectile.ai[0] -= 1f;

                    if (projectile.ai[0] == 0)
                    {
                        const int num226 = 18; //dusts indicate charged up
                        for (int num227 = 0; num227 < num226; num227++)
                        {
                            Vector2 vector6 = Vector2.UnitX.RotatedBy(projectile.rotation) * 6f;
                            vector6 = vector6.RotatedBy(((num227 - (num226 / 2 - 1)) * 6.28318548f / num226), default(Vector2)) + projectile.Center;
                            Vector2 vector7 = vector6 - projectile.Center;
                            int num228 = Dust.NewDust(vector6 + vector7, 0, 0, DustID.Shadowflame, 0f, 0f, 0, default(Color), 2f);
                            Main.dust[num228].noGravity = true;
                            Main.dust[num228].velocity = vector7;
                        }
                    }

                    return;
                }


                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile proj = Main.projectile[i];

                    if (proj.active && proj.owner == projectile.owner && proj.type != mod.ProjectileType("AncientShadowBall") && !proj.minion && proj.damage > 0 && proj.Hitbox.Intersects(projectile.Hitbox))
                    {
                        FargoGlobalProjectile.XWay(10, projectile.Center, mod.ProjectileType("AncientShadowBall"), 6, 30, 0);
                        projectile.ai[0] = 300;

                        break;
                    }
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (projectile.ai[0] != 0f)
            {
                return true;
            }


            //Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }
    }
}