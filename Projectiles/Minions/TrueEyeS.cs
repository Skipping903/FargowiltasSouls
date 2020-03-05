using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.Minions
{
    public class TrueEyeS : ModProjectile
    {
        public override string Texture => "Terraria/Projectile_650";

        private float localAI0;
        private float localAI1;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("True Eye of Cthulhu");
            Main.projFrames[projectile.type] = 4;
            //ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 32;
            projectile.height = 42;
            projectile.timeLeft *= 5;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;

            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (player.active && !player.dead && player.GetModPlayer<FargoPlayer>().TrueEyes)
                projectile.timeLeft = 2;

            if (projectile.damage == 0)
                projectile.damage = (int)(60f * player.minionDamage);

            //lighting effect?
            DelegateMethods.v3_1 = new Vector3(0.5f, 0.9f, 1f) * 1.5f;
            Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * 6f, 20f, new Utils.PerLinePoint(DelegateMethods.CastLightOpen));
            Utils.PlotTileLine(projectile.Left, projectile.Right, 20f, new Utils.PerLinePoint(DelegateMethods.CastLightOpen));
            Utils.PlotTileLine(player.Center, player.Center + player.velocity * 6f, 40f, new Utils.PerLinePoint(DelegateMethods.CastLightOpen));
            Utils.PlotTileLine(player.Left, player.Right, 40f, new Utils.PerLinePoint(DelegateMethods.CastLightOpen));

            if (projectile.ai[0] >= 0 && projectile.ai[0] < 200) //has target
            {
                NPC minionAttackTargetNpc = projectile.OwnerMinionAttackTargetNPC;
                if (minionAttackTargetNpc != null && projectile.ai[0] != minionAttackTargetNpc.whoAmI && minionAttackTargetNpc.CanBeChasedBy(projectile))
                    projectile.ai[0] = minionAttackTargetNpc.whoAmI;

                projectile.localAI[0]++;
                NPC npc = Main.npc[(int)projectile.ai[0]];
                if (npc.CanBeChasedBy(projectile))
                {
                    switch ((int)projectile.ai[1])
                    {
                        case 0: //true eye movement code
                            Vector2 newVel = npc.Center - projectile.Center + new Vector2(-200f, -200f);
                            if (newVel != Vector2.Zero)
                            {
                                newVel.Normalize();
                                newVel *= 24f;
                                projectile.velocity.X = (projectile.velocity.X * 29 + newVel.X) / 30;
                                projectile.velocity.Y = (projectile.velocity.Y * 29 + newVel.Y) / 30;
                            }
                            if (projectile.Distance(npc.Center) < 150f)
                            {
                                if (projectile.Center.X < npc.Center.X)
                                    projectile.velocity.X -= 0.25f;
                                else
                                    projectile.velocity.X += 0.25f;

                                if (projectile.Center.Y < npc.Center.Y)
                                    projectile.velocity.Y -= 0.25f;
                                else
                                    projectile.velocity.Y += 0.25f;
                            }

                            if (projectile.localAI[0] > 60f)
                            {
                                if (projectile.Distance(npc.Center) > 1500f) //give up if too far
                                    TargetEnemies();
                                projectile.localAI[0] = 0f;
                                projectile.ai[1]++;
                            }
                            break;

                        case 1: //slow down
                            projectile.velocity *= 0.9f;
                            if (projectile.velocity.Length() < 1f) //stop, FIRE LASER
                            {
                                projectile.velocity = Vector2.Zero;
                                projectile.localAI[0] = 0f;
                                projectile.ai[1]++;
                            }
                            break;

                        case 2: //shoot
                            if (projectile.localAI[0] == 7f)
                            {
                                Main.PlaySound(4, (int)projectile.Center.X, (int)projectile.Center.Y, 6, 1f, 0.0f);
                                ShootBolts(npc);
                            }
                            else if (projectile.localAI[0] == 14f)
                            {
                                ShootBolts(npc);
                            }
                            else if (projectile.localAI[0] > 21f)
                            {
                                projectile.localAI[0] = 0f;
                                projectile.ai[1]++;
                            }
                            break;

                        default:
                            projectile.ai[1] = 0f;
                            goto case 0;
                    }
                }
                else //forget target
                {
                    TargetEnemies();
                }

                if (projectile.rotation > 3.14159274101257)
                    projectile.rotation = projectile.rotation - 6.283185f;
                projectile.rotation = projectile.rotation <= -0.005 || projectile.rotation >= 0.005 ? projectile.rotation * 0.96f : 0.0f;
                if (++projectile.frameCounter >= 4)
                {
                    projectile.frameCounter = 0;
                    if (++projectile.frame >= Main.projFrames[projectile.type])
                        projectile.frame = 0;
                }
                UpdatePupil();
            }
            else
            {
                if (projectile.localAI[1]++ > 15f)
                    TargetEnemies();

                Vector2 vector2_1 = new Vector2(player.direction * -100f, 0f); //vanilla movement code
                Vector2 vector2_2 = player.MountedCenter + vector2_1;
                float num1 = Vector2.Distance(projectile.Center, vector2_2);
                if (num1 > 1500) //teleport when out of range
                    projectile.Center = player.Center + vector2_1;
                Vector2 vector2_3 = vector2_2 - projectile.Center;
                float num2 = 4f;
                if (num1 < num2)
                    projectile.velocity *= 0.25f;
                if (vector2_3 != Vector2.Zero)
                {
                    if (vector2_3.Length() < num2)
                        projectile.velocity = vector2_3;
                    else
                        projectile.velocity = vector2_3 * 0.1f;
                }
                if (projectile.velocity.Length() > 6) //when moving fast, rotate in direction of velocity
                {
                    float num3 = projectile.velocity.ToRotation() + 1.570796f;
                    if (Math.Abs(projectile.rotation - num3) >= 3.14159274101257)
                        projectile.rotation = num3 >= projectile.rotation ? projectile.rotation + 6.283185f : projectile.rotation - 6.283185f;
                    projectile.rotation = (projectile.rotation * 11f + num3) / 12f;
                    if (++projectile.frameCounter >= 4)
                    {
                        projectile.frameCounter = 0;
                        if (++projectile.frame >= Main.projFrames[projectile.type])
                            projectile.frame = 0;
                    }
                }
                else //when moving slow, calm down
                {
                    if (projectile.rotation > 3.14159274101257)
                        projectile.rotation = projectile.rotation - 6.283185f;
                    projectile.rotation = projectile.rotation <= -0.005 || projectile.rotation >= 0.005 ? projectile.rotation * 0.96f : 0f;
                    if (++projectile.frameCounter >= 6)
                    {
                        projectile.frameCounter = 0;
                        if (++projectile.frame >= Main.projFrames[projectile.type])
                            projectile.frame = 0;
                    }
                }

                UpdatePupil();
            }
            /*Main.NewText("local0 " + localAI0.ToString()
                + " local1 " + localAI1.ToString()
                + " ai0 " + projectile.ai[0].ToString()
                + " ai1 " + projectile.ai[1].ToString());*/
        }

        private void ShootBolts(NPC npc)
        {
            Vector2 spawn = projectile.Center - Vector2.UnitY * 6f;
            Vector2 vel = npc.Center + npc.velocity * 15f - spawn;
            if (vel != Vector2.Zero)
            {
                vel.Normalize();
                vel *= 8f;
                for (int i = -1; i <= 1; i++)
                {
                    Projectile.NewProjectile(spawn, vel.RotatedBy(Math.PI / 24 * i), mod.ProjectileType("PhantasmalBoltTrueEye"),
                        projectile.damage / 3 * 7, 6f, projectile.owner);
                }
            }
        }

        private void UpdatePupil()
        {
            Player player = Main.player[projectile.owner];
            float f1 = (float)(localAI0 % 6.28318548202515 - 3.14159274101257);
            float num13 = (float)Math.IEEERemainder(localAI1, 1.0);
            if (num13 < 0.0)
                ++num13;
            float num14 = (float)Math.Floor(localAI1);
            float max = 0.999f;
            int num15 = 0;
            float amount = 0.1f;
            float f2;
            float num18;
            float num19;
            if (projectile.ai[0] != -1f) //targeted an enemy
            {
                f2 = projectile.AngleTo(Main.npc[(int)projectile.ai[0]].Center);
                num15 = 2;
                num18 = MathHelper.Clamp(num13 + 0.05f, 0.0f, max);
                num19 = num14 + Math.Sign(-12f - num14);
            }
            else if (player.velocity.Length() > 3)
            {
                f2 = projectile.AngleTo(projectile.Center + player.velocity);
                num15 = 1;
                num18 = MathHelper.Clamp(num13 + 0.05f, 0.0f, max);
                num19 = num14 + Math.Sign(-10f - num14);
            }
            else
            {
                f2 = player.direction == 1 ? 0.0f : 3.141603f;
                num18 = MathHelper.Clamp(num13 + Math.Sign(0.75f - num13) * 0.05f, 0.0f, max);
                num19 = num14 + Math.Sign(0.0f - num14);
                amount = 0.12f;
            }
            Vector2 rotationVector2 = f2.ToRotationVector2();
            localAI0 = (float)(Vector2.Lerp(f1.ToRotationVector2(), rotationVector2, amount).ToRotation() + num15 * 6.28318548202515 + 3.14159274101257);
            localAI1 = num19 + num18;
        }

        private void TargetEnemies()
        {
            NPC minionAttackTargetNpc = projectile.OwnerMinionAttackTargetNPC;
            if (minionAttackTargetNpc != null && projectile.ai[0] != minionAttackTargetNpc.whoAmI && minionAttackTargetNpc.CanBeChasedBy(projectile))
            {
                projectile.ai[0] = minionAttackTargetNpc.whoAmI;
            }
            else
            {
                float maxDistance = 1000f;
                int possibleTarget = -1;
                for (int i = 0; i < 200; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy(projectile))// && Collision.CanHitLine(projectile.Center, 0, 0, npc.Center, 0, 0))
                    {
                        float npcDistance = projectile.Distance(npc.Center);
                        if (npcDistance < maxDistance)
                        {
                            maxDistance = npcDistance;
                            possibleTarget = i;
                        }
                    }
                }
                projectile.ai[0] = possibleTarget;
            }
            projectile.localAI[0] = 0f;
            projectile.localAI[1] = 0f;
            projectile.ai[1] = 0f;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(mod.BuffType("CurseoftheMoon"), 300);
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            int r = lightColor.R * 3 / 2;
            int g = lightColor.G * 3 / 2;
            int b = lightColor.B * 3 / 2;
            if (r > 255)
                r = 255;
            if (g > 255)
                g = 255;
            if (b > 255)
                b = 255;
            return new Color(r, g, b);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture2D13 = Main.projectileTexture[projectile.type];
            int num156 = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;

            Main.spriteBatch.Draw(texture2D13, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), projectile.GetAlpha(lightColor), projectile.rotation, origin2, projectile.scale, SpriteEffects.None, 0f);

            Texture2D pupil = mod.GetTexture("Projectiles/Minions/TrueEyePupil");
            Vector2 pupilOffset = new Vector2(localAI1 / 2f, 0f).RotatedBy(localAI0);
            pupilOffset += new Vector2(0f, -6f).RotatedBy(projectile.rotation);
            Vector2 pupilOrigin = pupil.Size() / 2f;
            Main.spriteBatch.Draw(pupil, pupilOffset + projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(pupil.Bounds), projectile.GetAlpha(lightColor), 0f, pupilOrigin, projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}