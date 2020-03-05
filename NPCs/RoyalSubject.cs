using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.NPCs
{
    public class RoyalSubject : ModNPC
    {
        public override string Texture => "Terraria/NPC_222";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Royal Subject");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.QueenBee];
            DisplayName.AddTranslation(GameCulture.Chinese, "皇家工蜂");
        }

        public override void SetDefaults()
        {
            npc.width = 66;
            npc.height = 66;
            npc.aiStyle = 43;
            aiType = NPCID.QueenBee;
            npc.damage = 15;
            npc.defense = 8;
            npc.lifeMax = 800;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0f;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.timeLeft = NPC.activeTime * 30;
            npc.npcSlots = 7f;
            npc.scale = 0.5f;
            npc.buffImmune[BuffID.Poisoned] = true;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.7 * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.9);

            npc.lifeMax = (int)(npc.lifeMax * (1 + FargoSoulsWorld.BeeCount * .025));
            npc.damage = (int)(npc.damage * (1 + FargoSoulsWorld.BeeCount * .0125));
        }

        public override void AI()
        {
            if (!npc.GetGlobalNPC<FargoSoulsGlobalNPC>().masoBool[0])
            {
                npc.GetGlobalNPC<FargoSoulsGlobalNPC>().masoBool[0] = true;
                Main.npcTexture[npc.type] = Main.npcTexture[NPCID.QueenBee];
            }

            if (!FargoSoulsGlobalNPC.BossIsAlive(ref FargoSoulsGlobalNPC.beeBoss, NPCID.QueenBee)
                && !NPC.AnyNPCs(NPCID.QueenBee))
            {
                npc.StrikeNPCNoInteraction(9999, 0f, 0);
            }

            //tries to stinger, force into dash
            if (npc.ai[0] == 1 || npc.ai[0] == 3)
            {
                npc.ai[0] = 0f;
                npc.netUpdate = true;
            }

            npc.position -= npc.velocity / 3;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Poisoned, Main.rand.Next(60, 180));
            target.AddBuff(mod.BuffType("Infested"), 300);
            target.AddBuff(BuffID.BrokenArmor, 600);
            target.AddBuff(mod.BuffType("Swarming"), 300);
        }

        public override bool PreNPCLoot()
        {
            return false;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                //Main.PlaySound(npc.DeathSound, npc.Center);
                for (int i = 0; i < 20; i++)
                {
                    int d = Dust.NewDust(npc.position, npc.width, npc.height, 5);
                    Main.dust[d].velocity *= 3f;
                    Main.dust[d].scale += 0.75f;
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (npc.localAI[0] == 1)
            {
                if (npc.frameCounter > 4)
                {
                    npc.frame.Y += frameHeight;
                    npc.frameCounter = 0;
                }
                if (npc.frame.Y >= 4 * frameHeight)
                    npc.frame.Y = 0;
            }
            else
            {
                if (npc.frameCounter > 4)
                {
                    npc.frame.Y += frameHeight;
                    npc.frameCounter = 0;
                }
                if (npc.frame.Y < 4 * frameHeight)
                    npc.frame.Y = 4 * frameHeight;
                if (npc.frame.Y >= 12 * frameHeight)
                    npc.frame.Y = 4 * frameHeight;
            }
        }
    }
}