using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.NPCs
{
    public class CreeperGutted : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Creeper");
            DisplayName.AddTranslation(GameCulture.Chinese, "爬行者");
            Main.npcFrameCount[npc.type] = 3;
        }

        public override void SetDefaults()
        {
            npc.width = 30;
            npc.height = 30;
            npc.damage = 20;
            npc.defense = 10;
            npc.lifeMax = 100;
            npc.friendly = true;
            npc.netAlways = true;
            npc.HitSound = SoundID.NPCHit9;
            npc.DeathSound = SoundID.NPCDeath11;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.knockBackResist = 0.8f;
            npc.lavaImmune = true;
            for (int i = 0; i < npc.buffImmune.Length; i++)
                npc.buffImmune[i] = true;
            npc.aiStyle = -1;
        }

        public override void AI()
        {
            if (npc.localAI[0] == 0f)
            {
                npc.localAI[0] = 1f;
                npc.lifeMax *= (int)npc.ai[2];
                npc.defDamage *= (int)npc.ai[2];
                npc.defDefense *= (int)npc.ai[2];
                npc.life = npc.lifeMax;
            }

            npc.damage = npc.defDamage;
            npc.defense = npc.defDefense;

            Player player = Main.player[(int)npc.ai[0]];
            if (!player.active || player.dead || !player.GetModPlayer<FargoPlayer>().GuttedHeart)
            {
                npc.StrikeNPCNoInteraction(9999, 0f, 0);
                return;
            }

            Vector2 distance = player.Center - npc.Center;
            float length = distance.Length();
            if (length > 1000f)
            {
                npc.Center = player.Center;
                npc.velocity = Vector2.UnitX.RotatedByRandom(2 * Math.PI) * 8;
            }
            else if (length > 40f)
            {
                distance /= 10f;
                npc.velocity = (npc.velocity * 15f + distance) / 16f;
            }
            else
            {
                if (npc.velocity.Length() < 8)
                    npc.velocity *= 1.05f;
            }

            if (npc.ai[1]++ > 120f)
            {
                npc.ai[1] = 0f;
                npc.velocity = npc.velocity.RotatedByRandom(2 * Math.PI);

                if (player.whoAmI == Main.myPlayer && !SoulConfig.Instance.GetValue(SoulConfig.Instance.GuttedHeart))
                {
                    int n = npc.whoAmI;
                    npc.StrikeNPCNoInteraction(9999, 0f, 0);
                    if (Main.netMode != 0)
                        NetMessage.SendData(28, -1, -1, null, n, 9999f);
                    return;
                }
            }

            npc.position += player.position - player.oldPosition;
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.ai[2] <= 1)
                npc.frame.Y = 0;
            else if (npc.ai[2] <= 2)
                npc.frame.Y = frameHeight;
            else
                npc.frame.Y = frameHeight * 2;
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage *= 10;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if (projectile.type == ProjectileID.RottenEgg)
                return false;
            return null;
        }

        public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            damage /= 2;
            return true;
        }

        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            projectile.timeLeft = 0;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("CreeperHitbox"), npc.damage, 6f, (int)npc.ai[0]);

            if (npc.life <= 0)
            {
                //Main.PlaySound(npc.DeathSound, npc.Center);
                for (int i = 0; i < 20; i++)
                {
                    int d = Dust.NewDust(npc.position, npc.width, npc.height, 5);
                    Main.dust[d].velocity *= 2.5f;
                    Main.dust[d].scale += 0.5f;
                }
            }
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override bool PreNPCLoot()
        {
            return false;
        }
    }
}