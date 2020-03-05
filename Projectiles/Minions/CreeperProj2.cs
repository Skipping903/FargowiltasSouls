using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.Minions
{
    public class CreeperProj2 : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            //ProjectileID.Sets.Homing[projectile.type] = true;
            //ProjectileID.Sets.MinionShot[projectile.type] = true;

            projectile.minion = true;
            projectile.timeLeft = 360;
            projectile.penetrate = 6;
            projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            if (projectile.localAI[0] == 0f)
            {
                Main.PlaySound(SoundID.Item20, projectile.position);
                projectile.localAI[0] = 1f;
            }

            int dust = Dust.NewDust(projectile.Center, 0, 0, 66, 0f, 0f, 100, new Color(244, 66, 113), 1.5f);
            Main.dust[dust].velocity *= 0.1f;
            if (projectile.velocity == Vector2.Zero)
            {
                Main.dust[dust].velocity.Y -= 1f;
                Main.dust[dust].scale = 1.2f;
            }
            else
            {
                Main.dust[dust].velocity += projectile.velocity * 0.2f;
            }

            /*Main.dust[dust].position.X = projectile.Center.X + 4f + Main.rand.Next(-2, 3);
            Main.dust[dust].position.Y = projectile.Center.Y + Main.rand.Next(-2, 3);*/
            Main.dust[dust].noGravity = true;

            if (--projectile.ai[0] < 0)
            {
                projectile.netUpdate = true;
                projectile.ai[0] = 60;

                float maxDistance = 1500;
                int possibleTarget = -1;
                for (int i = 0; i < 200; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy(projectile) && Collision.CanHitLine(projectile.Center, 0, 0, npc.Center, 0, 0))
                    {
                        float npcDistance = projectile.Distance(npc.Center);
                        if (npcDistance < maxDistance)
                        {
                            maxDistance = npcDistance;
                            possibleTarget = i;
                        }
                    }
                }

                NPC minionAttackTargetNpc = projectile.OwnerMinionAttackTargetNPC;
                if (minionAttackTargetNpc != null && projectile.ai[0] != minionAttackTargetNpc.whoAmI && minionAttackTargetNpc.CanBeChasedBy(projectile))
                    possibleTarget = minionAttackTargetNpc.whoAmI;

                projectile.ai[1] = possibleTarget;
            }

            if (projectile.ai[1] != -1)
            {
                int ai1 = (int)projectile.ai[1];
                if (Main.npc[ai1].CanBeChasedBy())
                {
                    double num4 = (Main.npc[ai1].Center - projectile.Center).ToRotation() - projectile.velocity.ToRotation();
                    if (num4 > Math.PI)
                        num4 -= 2.0 * Math.PI;
                    if (num4 < -1.0 * Math.PI)
                        num4 += 2.0 * Math.PI;
                    projectile.velocity = projectile.velocity.RotatedBy(num4 * 0.2f);
                }
                else
                {
                    projectile.ai[1] = -1;
                    projectile.netUpdate = true;
                }
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (!Main.player[projectile.owner].HeldItem.summon)
                damage /= 4;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture2D13 = Main.projectileTexture[projectile.type];
            int num156 = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;

            SpriteEffects effects = projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Main.spriteBatch.Draw(texture2D13, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), projectile.GetAlpha(lightColor), projectile.rotation, origin2, projectile.scale, effects, 0f);
            return false;
        }
    }
}