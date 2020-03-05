using System;
using System.Collections.Generic;
using System.Linq;
using FargowiltasSouls.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Events;
using Terraria.Graphics.Shaders;
using Terraria.Localization;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.NPCs
{
    public class FargoSoulsGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        //debuffs
        public bool SBleed;
        public bool Shock;
        public bool Rotting;
        public bool LeadPoison;
        public bool SqueakyToy;
        public bool SolarFlare;
        public bool TimeFrozen;
        public bool HellFire;
        public bool Infested;
        public int MaxInfestTime;
        public float InfestedDust;
        public bool Needles;
        public bool Electrified;
        public bool CurseoftheMoon;
        public int lightningRodTimer;
        public bool Sadism;
        public bool OceanicMaul;
        public bool MutantNibble;
        public int LifePrevious = -1;
        public bool GodEater;
        public bool Suffocation;
        public bool Villain;
        public bool Lethargic;
        public int LethargicCounter;

        private int valhallaPlayer;
        private int valhallaCounter = 0;
        private int squireCounter = 0;
        public float originalKB;
        public bool SpecialEnchantImmune;

        //masochist doom
        public static byte masoStateML = 0;
        public bool[] masoBool = new bool[4];
        public bool FirstTick = false;
        private int Stop = 0;
        public bool PaladinsShield = false;
        public bool isMasoML;
        public bool isWaterEnemy;
        public int RegenTimer = 0;
        public int Counter = 0;
        public int Counter2 = 0;
        public int Timer = 600;
        public byte SharkCount = 0;
        private bool applyDebuff;

        public static int boss = -1;
        public static int slimeBoss = -1;
        public static int eyeBoss = -1;
        public static int eaterBoss = -1;
        public static int brainBoss = -1;
        public static int beeBoss = -1;
        public static int skeleBoss = -1;
        public static int wallBoss = -1;
        public static int retiBoss = -1;
        public static int spazBoss = -1;
        public static int destroyBoss = -1;
        public static int primeBoss = -1;
        public static int betsyBoss = -1;
        public static int fishBoss = -1;
        public static int cultBoss = -1;
        public static int moonBoss = -1;
        public static int guardBoss = -1;
        public static int fishBossEX = -1;
        public static bool spawnFishronEX;
        public static int abomBoss = -1;
        public static int mutantBoss = -1;

        public static bool Revengeance => CalamityMod.World.CalamityWorld.revenge;

        public override void ResetEffects(NPC npc)
        {
            TimeFrozen = false;
            SBleed = false;
            Shock = false;
            Rotting = false;
            LeadPoison = false;
            SqueakyToy = false;
            SolarFlare = false;
            HellFire = false;
            PaladinsShield = false;
            Infested = false;
            Needles = false;
            Electrified = false;
            CurseoftheMoon = false;
            Sadism = false;
            OceanicMaul = false;
            MutantNibble = false;
            GodEater = false;
            Suffocation = false;
        }

        public override void SetDefaults(NPC npc)
        {
            LoadSprites(npc);

            if (FargoSoulsWorld.MasochistMode)
            {
                npc.value = (int)(npc.value * 1.3);

                ResetRegenTimer(npc);

                switch (npc.type)
                {
                    case NPCID.GoblinWarrior:
                        npc.knockBackResist = 0;
                        break;

                    case NPCID.Plantera:
                        npc.lifeMax = (int)(npc.lifeMax * 1.25);
                        break;

                    case NPCID.PlanterasTentacle:
                        npc.lifeMax /= 4;
                        break;

                    case NPCID.Pixie:
                        npc.noTileCollide = true;
                        break;

                    case NPCID.DungeonGuardian:
                        npc.boss = true;
                        SpecialEnchantImmune = true;
                        break;

                    case NPCID.WyvernHead:
                        if (Main.hardMode)
                            npc.lifeMax *= 2;
                        else
                            npc.lifeMax /= 2;
                        Counter = Main.rand.Next(180);
                        break;

                    case NPCID.Moth:
                        npc.lifeMax *= 2;
                        break;

                    case NPCID.Salamander:
                    case NPCID.Salamander2:
                    case NPCID.Salamander3:
                    case NPCID.Salamander4:
                    case NPCID.Salamander5:
                    case NPCID.Salamander6:
                    case NPCID.Salamander7:
                    case NPCID.Salamander8:
                    case NPCID.Salamander9:
                        npc.Opacity /= 5;
                        break;

                    case NPCID.Mothron:
                    case NPCID.MothronSpawn:
                        npc.knockBackResist *= .1f;
                        break;

                    case NPCID.Butcher:
                        npc.knockBackResist = 0f;
                        break;

                    case NPCID.Tim:
                    case NPCID.RuneWizard:
                        npc.buffImmune[BuffID.OnFire] = true;
                        npc.lavaImmune = true;
                        npc.lifeMax *= 4;
                        break;

                    case NPCID.LostGirl:
                    case NPCID.Nymph:
                        npc.lavaImmune = true;
                        npc.buffImmune[BuffID.Confused] = true;
                        if (Main.hardMode)
                        {
                            npc.lifeMax *= 4;
                            npc.damage *= 2;
                            npc.defense *= 2;
                        }
                        break;

                    case NPCID.Shark:
                        Counter2 = Main.rand.Next(60);
                        break;

                    case NPCID.Piranha:
                        Counter2 = Main.rand.Next(120);
                        break;

                    case NPCID.DD2SkeletonT1:
                    case NPCID.DD2SkeletonT3:
                        Counter = Main.rand.Next(180);
                        break;

                    case NPCID.BloodFeeder:
                        npc.lifeMax *= 5;
                        break;

                    case NPCID.WanderingEye:
                        npc.lifeMax *= 2;
                        break;

                    case NPCID.BigEater:
                    case NPCID.EaterofSouls:
                    case NPCID.LittleEater:
                        Counter = 300;
                        break;

                    case NPCID.PirateShip:
                        npc.noTileCollide = true;
                        break;

                    case NPCID.PirateShipCannon:
                        Counter = Main.rand.Next(60);
                        npc.noTileCollide = true;
                        break;

                    case NPCID.MisterStabby:
                    case NPCID.AnglerFish:
                        npc.Opacity /= 5;
                        break;

                    case NPCID.SolarSolenian:
                        npc.knockBackResist = 0f;
                        break;

                    case NPCID.RainbowSlime:
                        npc.scale = 3f;
                        npc.lifeMax *= 5;
                        break;

                    case NPCID.Hellhound:
                        npc.lavaImmune = true;
                        break;

                    case NPCID.WalkingAntlion:
                        npc.knockBackResist = .4f;
                        break;

                    case NPCID.Tumbleweed:
                        npc.knockBackResist = .1f;
                        break;

                    case NPCID.DesertBeast:
                        npc.lifeMax *= 2;
                        npc.knockBackResist = 0f;
                        break;

                    case NPCID.DuneSplicerHead:
                    case NPCID.DuneSplicerBody:
                    case NPCID.DuneSplicerTail:
                        if (Main.hardMode)
                            npc.lifeMax *= 3;
                        else
                        {
                            npc.lifeMax /= 2;
                            npc.damage /= 2;
                        }
                        break;

                    case NPCID.StardustCellSmall:
                        npc.defense = 80;
                        break;

                    case NPCID.Parrot:
                        npc.noTileCollide = true;
                        break;

                    case NPCID.SolarSroller:
                        npc.lifeMax *= 2;
                        npc.scale += 0.5f;
                        break;

                    case NPCID.VileSpit:
                        npc.dontTakeDamage = true;
                        break;

                    case NPCID.ChaosBall:
                        npc.dontTakeDamage = Main.hardMode;
                        break;

                    case NPCID.DoctorBones:
                    case NPCID.Lihzahrd:
                    case NPCID.FlyingSnake:
                        npc.trapImmune = true;
                        break;

                    case NPCID.SandElemental:
                        npc.lifeMax *= 2;
                        break;

                    case NPCID.SolarCrawltipedeTail:
                        npc.trapImmune = true;
                        break;

                    case NPCID.SolarGoop:
                        npc.noTileCollide = true;
                        npc.buffImmune[BuffID.OnFire] = true;
                        npc.lavaImmune = true;
                        break;

                    case NPCID.Clown:
                        npc.lifeMax *= 2;
                        break;

                    case NPCID.LunarTowerSolar:
                    case NPCID.LunarTowerNebula:
                    case NPCID.LunarTowerStardust:
                    case NPCID.LunarTowerVortex:
                        npc.lifeMax *= 2;
                        npc.buffImmune[mod.BuffType("ClippedWings")] = true;
                        break;

                    case NPCID.CultistArcherWhite:
                        npc.chaseable = true;
                        npc.lavaImmune = false;
                        break;

                    case NPCID.Reaper:
                        Timer = 0;
                        break;

                    case NPCID.EnchantedSword:
                    case NPCID.CursedHammer:
                    case NPCID.CrimsonAxe:
                        npc.scale = 2f;
                        npc.lifeMax *= 4;
                        npc.defense *= 2;
                        npc.knockBackResist = 0f;
                        break;

                    case NPCID.Pumpking:
                    case NPCID.IceQueen:
                        npc.buffImmune[mod.BuffType("ClippedWings")] = true;
                        break;

                    case NPCID.Mimic:
                        npc.value /= 4;
                        goto case NPCID.Medusa;

                    case NPCID.PigronCorruption:
                    case NPCID.PigronCrimson:
                    case NPCID.PigronHallow:
                    case NPCID.RedDevil:
                    case NPCID.AngryNimbus:
                    case NPCID.IchorSticker:
                    case NPCID.SeekerHead:
                    case NPCID.MushiLadybug:
                    case NPCID.AnomuraFungus:
                    case NPCID.ZombieMushroom:
                    case NPCID.ZombieMushroomHat:
                    case NPCID.Medusa:
                        if (!Main.hardMode)
                        {
                            npc.lifeMax /= 2;
                            npc.defense = 0;
                        }
                        break;

                    #region maso bosses
                    case NPCID.ServantofCthulhu:
                        npc.lifeMax *= 5;
                        break;

                    case NPCID.BrainofCthulhu:
                        npc.lifeMax = (int)(npc.lifeMax * 1.25);
                        npc.scale += 0.25f;
                        break;

                    case NPCID.Creeper:
                        npc.lifeMax = (int)(npc.lifeMax * 1.25);
                        Timer = Main.rand.Next(600);
                        break;

                    case NPCID.WallofFlesh:
                        npc.defense *= 10;
                        Timer = 0;
                        Counter = 600;
                        break;
                    case NPCID.WallofFleshEye:
                        npc.buffImmune[mod.BuffType("ClippedWings")] = true;
                        break;
                    case NPCID.TheHungryII:
                        npc.noTileCollide = true;
                        break;

                    case NPCID.SkeletronPrime:
                        Timer = 0;
                        npc.trapImmune = true;
                        break;
                    case NPCID.PrimeCannon:
                    case NPCID.PrimeLaser:
                    case NPCID.PrimeSaw:
                    case NPCID.PrimeVice:
                        npc.lifeMax /= 2;
                        npc.trapImmune = true;
                        npc.buffImmune[mod.BuffType("ClippedWings")] = true;
                        break;

                    case NPCID.Golem:
                        npc.lifeMax *= 3;
                        npc.trapImmune = true;
                        break;
                    case NPCID.GolemHead:
                        npc.trapImmune = true;
                        npc.buffImmune[mod.BuffType("ClippedWings")] = true;
                        break;
                    case NPCID.GolemHeadFree:
                        Timer = 0;
                        Counter = 540;
                        break;
                    case NPCID.GolemFistLeft:
                    case NPCID.GolemFistRight:
                        npc.scale += 0.5f;
                        npc.trapImmune = true;
                        npc.buffImmune[mod.BuffType("ClippedWings")] = true;
                        break;

                    case NPCID.DukeFishron:
                        SpecialEnchantImmune = true;
                        if (spawnFishronEX)
                        {
                            masoBool[3] = true;
                            npc.GivenName = "Duke Fishron EX";
                            npc.damage = (int)(npc.damage * 1.5);
                            npc.buffImmune[mod.BuffType("FlamesoftheUniverse")] = true;
                            npc.buffImmune[mod.BuffType("LightningRod")] = true;
                        }
                        break;
                    case NPCID.DetonatingBubble:
                        npc.lavaImmune = true;
                        npc.buffImmune[BuffID.OnFire] = true;
                        if (!NPC.downedBoss3)
                            npc.noTileCollide = false;
                        break;
                    case NPCID.Sharkron:
                    case NPCID.Sharkron2:
                        npc.lifeMax *= 5;
                        npc.buffImmune[mod.BuffType("ClippedWings")] = true;
                        if (BossIsAlive(ref fishBossEX, NPCID.DukeFishron))
                        {
                            npc.lifeMax *= 2;
                            npc.buffImmune[mod.BuffType("FlamesoftheUniverse")] = true;
                            npc.buffImmune[mod.BuffType("LightningRod")] = true;
                            SpecialEnchantImmune = true;
                        }
                        npc.buffImmune[BuffID.OnFire] = true;
                        npc.lavaImmune = true;
                        break;

                    case NPCID.DD2Betsy:
                        npc.boss = true;
                        break;

                    case NPCID.CultistBoss:
                        npc.lifeMax = (int)(npc.lifeMax * 1.5);
                        Timer = 0;
                        break;
                    case NPCID.CultistBossClone:
                        npc.buffImmune[mod.BuffType("ClippedWings")] = true;
                        npc.damage = 75;
                        break;
                    case NPCID.AncientDoom:
                        npc.lifeMax *= 4;
                        break;
                    case NPCID.AncientLight:
                        npc.buffImmune[BuffID.OnFire] = true;
                        npc.lavaImmune = true;
                        masoBool[0] = BossIsAlive(ref moonBoss, NPCID.MoonLordCore);
                        break;

                    case NPCID.QueenBee:
                        npc.value /= 2;
                        break;

                    case NPCID.MoonLordCore:
                        isMasoML = true;
                        masoStateML = 0;
                        npc.value += Item.buyPrice(0, 20);
                        break;
                    case NPCID.MoonLordHand:
                    case NPCID.MoonLordHead:
                    case NPCID.MoonLordFreeEye:
                        npc.buffImmune[mod.BuffType("ClippedWings")] = true;
                        isMasoML = true;
                        break;
                    case NPCID.MoonLordLeechBlob:
                        npc.lifeMax *= 3;
                        goto case NPCID.MoonLordHand;

                    #endregion

                    default:
                        break;
                }
                
                // +2.5% hp each kill 
                // +1.25% damage each kill
                // max of 4x hp and 2.5x damage

                //pre hm get 8x and 5x
                switch (npc.type)
                {
                    #region boss scaling

                    case NPCID.EyeofCthulhu:
                    case NPCID.ServantofCthulhu:
                        npc.lifeMax = (int)(npc.lifeMax * (1 + FargoSoulsWorld.EyeCount * .025));
                        npc.damage = (int)(npc.damage * (1 + FargoSoulsWorld.EyeCount * .0125));
                        break;

                    case NPCID.KingSlime:
                        npc.lifeMax = (int)(npc.lifeMax * (1 + FargoSoulsWorld.SlimeCount * .025));
                        npc.damage = (int)(npc.damage * (1 + FargoSoulsWorld.SlimeCount * .0125));
                        break;
                    case NPCID.BlueSlime:
                    case NPCID.SlimeSpiked:
                        if (BossIsAlive(ref slimeBoss, NPCID.KingSlime))
                        {
                            npc.lifeMax = (int)(npc.lifeMax * (1 + FargoSoulsWorld.SlimeCount * .025));
                            npc.damage = (int)(npc.damage * (1 + FargoSoulsWorld.SlimeCount * .0125));
                        }
                        break;

                    case NPCID.EaterofWorldsHead:
                    case NPCID.EaterofWorldsBody:
                    case NPCID.EaterofWorldsTail:
                        npc.lifeMax = (int)(npc.lifeMax * (1 + FargoSoulsWorld.EaterCount * .025));
                        npc.damage = (int)(npc.damage * (1 + FargoSoulsWorld.EaterCount * .0125));
                        npc.buffImmune[BuffID.CursedInferno] = true;
                        break;
                    case NPCID.VileSpit:
                        if (BossIsAlive(ref eaterBoss, NPCID.EaterofWorldsHead))
                            npc.damage = (int)(npc.damage * (1 + FargoSoulsWorld.EaterCount * .0125));
                        break;
                    case NPCID.EaterofSouls:
                        if (BossIsAlive(ref eaterBoss, NPCID.EaterofWorldsHead))
                            npc.lifeMax /= 2;
                        break;

                    case NPCID.BrainofCthulhu:
                    case NPCID.Creeper:
                        npc.lifeMax = (int)(npc.lifeMax * (1 + FargoSoulsWorld.BrainCount * .025));
                        npc.damage = (int)(npc.damage * (1 + FargoSoulsWorld.BrainCount * .0125));
                        npc.buffImmune[BuffID.Ichor] = true;
                        break;

                    case NPCID.QueenBee:
                        npc.lifeMax = (int)(npc.lifeMax * (1 + FargoSoulsWorld.BeeCount * .025));
                        npc.damage = (int)(npc.damage * (1 + FargoSoulsWorld.BeeCount * .0125));
                        npc.buffImmune[BuffID.Poisoned] = true;
                        break;
                    case NPCID.Hornet:
                    case NPCID.HornetFatty:
                    case NPCID.HornetHoney:
                    case NPCID.HornetLeafy:
                    case NPCID.HornetSpikey:
                    case NPCID.HornetStingy:
                    case NPCID.Bee:
                    case NPCID.BeeSmall:
                        if (BossIsAlive(ref beeBoss, NPCID.QueenBee))
                        {
                            npc.lifeMax = (int)(npc.lifeMax * (1 + FargoSoulsWorld.BeeCount * .025));
                            npc.damage = (int)(npc.damage * (1 + FargoSoulsWorld.BeeCount * .0125));
                        }
                        npc.buffImmune[BuffID.Poisoned] = true;
                        break;

                    case NPCID.SkeletronHead:
                    case NPCID.SkeletronHand:
                        npc.lifeMax = (int)(npc.lifeMax * (1 + FargoSoulsWorld.SkeletronCount * .025));
                        npc.damage = (int)(npc.damage * (1 + FargoSoulsWorld.SkeletronCount * .0125));
                        break;

                    case NPCID.WallofFlesh:
                    case NPCID.WallofFleshEye:
                        npc.lifeMax = (int)(npc.lifeMax * (1 + FargoSoulsWorld.WallCount * .025));
                        npc.damage = (int)(npc.damage * (1 + FargoSoulsWorld.WallCount * .0125));
                        npc.buffImmune[BuffID.OnFire] = true;
                        break;
                    case NPCID.TheHungry:
                    case NPCID.TheHungryII:
                    case NPCID.LeechHead:
                    case NPCID.LeechBody:
                    case NPCID.LeechTail:
                        if (BossIsAlive(ref wallBoss, NPCID.WallofFlesh))
                        {
                            npc.lifeMax = (int)(npc.lifeMax * (1 + FargoSoulsWorld.WallCount * .025));
                            npc.damage = (int)(npc.damage * (1 + FargoSoulsWorld.WallCount * .0125));
                        }
                        npc.buffImmune[BuffID.OnFire] = true;
                        break;

                    case NPCID.TheDestroyer:
                    case NPCID.TheDestroyerBody:
                    case NPCID.TheDestroyerTail:
                    case NPCID.Probe:
                        npc.lifeMax = (int)(npc.lifeMax * (1 + FargoSoulsWorld.DestroyerCount * .025));
                        npc.damage = (int)(npc.damage * (1 + FargoSoulsWorld.DestroyerCount * .0125));
                        npc.buffImmune[BuffID.Suffocation] = true;
                        break;

                    case NPCID.SkeletronPrime:
                    case NPCID.PrimeCannon:
                    case NPCID.PrimeLaser:
                    case NPCID.PrimeSaw:
                    case NPCID.PrimeVice:
                        npc.lifeMax = (int)(npc.lifeMax * (1 + FargoSoulsWorld.PrimeCount * .025));
                        npc.damage = (int)(npc.damage * (1 + FargoSoulsWorld.PrimeCount * .0125));
                        npc.buffImmune[BuffID.Suffocation] = true;
                        break;

                    case NPCID.Retinazer:
                    case NPCID.Spazmatism:
                        npc.lifeMax = (int)(npc.lifeMax * (1 + FargoSoulsWorld.TwinsCount * .025));
                        npc.damage = (int)(npc.damage * (1 + FargoSoulsWorld.TwinsCount * .0125));
                        npc.buffImmune[BuffID.Suffocation] = true;
                        break;

                    case NPCID.Plantera:
                    case NPCID.PlanterasHook:
                    case NPCID.PlanterasTentacle:
                        npc.lifeMax = (int)(npc.lifeMax * (1 + FargoSoulsWorld.PlanteraCount * .025));
                        npc.damage = (int)(npc.damage * (1 + FargoSoulsWorld.PlanteraCount * .0125));
                        npc.buffImmune[BuffID.Poisoned] = true;
                        break;

                    case NPCID.Golem:
                    case NPCID.GolemFistLeft:
                    case NPCID.GolemFistRight:
                    case NPCID.GolemHead:
                    case NPCID.GolemHeadFree:
                        npc.lifeMax = (int)(npc.lifeMax * (1 + FargoSoulsWorld.GolemCount * .025));
                        npc.damage = (int)(npc.damage * (1 + FargoSoulsWorld.GolemCount * .0125));
                        npc.buffImmune[BuffID.Poisoned] = true;
                        break;

                    case NPCID.CultistBoss:
                    case NPCID.CultistBossClone:
                    case NPCID.AncientCultistSquidhead:
                    case NPCID.AncientDoom:
                    case NPCID.CultistDragonHead:
                    case NPCID.CultistDragonBody1:
                    case NPCID.CultistDragonBody2:
                    case NPCID.CultistDragonBody3:
                    case NPCID.CultistDragonBody4:
                    case NPCID.CultistDragonTail:
                        npc.lifeMax = (int)(npc.lifeMax * (1 + FargoSoulsWorld.CultistCount * .025));
                        npc.damage = (int)(npc.damage * (1 + FargoSoulsWorld.CultistCount * .0125));
                        npc.buffImmune[BuffID.Suffocation] = true;
                        break;

                    case NPCID.AncientLight:
                    case NPCID.SolarGoop:
                        npc.buffImmune[BuffID.Suffocation] = true;
                        if (BossIsAlive(ref moonBoss, NPCID.MoonLordCore))
                            npc.damage = (int)(npc.damage * (1 + FargoSoulsWorld.MoonlordCount * .0125));
                        else if (BossIsAlive(ref cultBoss, NPCID.CultistBoss))
                            npc.damage = (int)(npc.damage * (1 + FargoSoulsWorld.CultistCount * .0125));
                        break;

                    case NPCID.DukeFishron:
                        npc.buffImmune[BuffID.Suffocation] = true;
                        if (FargoSoulsWorld.downedFishronEX || !spawnFishronEX)
                        {
                            npc.lifeMax = (int)(npc.lifeMax * (1 + FargoSoulsWorld.FishronCount * .025));
                            npc.damage = (int)(npc.damage * (1 + FargoSoulsWorld.FishronCount * .0125));
                        }
                        break;
                    case NPCID.Sharkron:
                    case NPCID.Sharkron2:
                        npc.buffImmune[BuffID.Suffocation] = true;
                        if (FargoSoulsWorld.downedFishronEX || !BossIsAlive(ref fishBossEX, NPCID.DukeFishron))
                        {
                            npc.lifeMax = (int)(npc.lifeMax * (1 + FargoSoulsWorld.FishronCount * .025));
                            npc.damage = (int)(npc.damage * (1 + FargoSoulsWorld.FishronCount * .0125));
                        }
                        break;
                    case NPCID.DetonatingBubble:
                        npc.buffImmune[BuffID.Suffocation] = true;
                        if (FargoSoulsWorld.downedFishronEX || !BossIsAlive(ref fishBossEX, NPCID.DukeFishron))
                            npc.damage = (int)(npc.damage * (1 + FargoSoulsWorld.FishronCount * .0125));
                        break;

                    case NPCID.MoonLordCore:
                    case NPCID.MoonLordHand:
                    case NPCID.MoonLordHead:
                    case NPCID.MoonLordFreeEye:
                    case NPCID.MoonLordLeechBlob:
                        npc.buffImmune[BuffID.Suffocation] = true;
                        npc.lifeMax = (int)(npc.lifeMax * (1 + FargoSoulsWorld.MoonlordCount * .025));
                        npc.damage = (int)(npc.damage * (1 + FargoSoulsWorld.MoonlordCount * .0125));
                        break;

                    #endregion

                    #region enemy biome immunities

                    case NPCID.Hellbat:
                    case NPCID.LavaSlime:
                    case NPCID.FireImp:
                    case NPCID.Demon:
                    case NPCID.VoodooDemon:
                    case NPCID.BoneSerpentBody:
                    case NPCID.BoneSerpentHead:
                    case NPCID.BoneSerpentTail:
                    case NPCID.Lavabat:
                    case NPCID.RedDevil:
                    case NPCID.BurningSphere:
                        npc.buffImmune[BuffID.OnFire] = true;
                        break;

                    case NPCID.Harpy:
                    case NPCID.WyvernBody:
                    case NPCID.WyvernBody2:
                    case NPCID.WyvernBody3:
                    case NPCID.WyvernHead:
                    case NPCID.WyvernLegs:
                    case NPCID.WyvernTail:
                    case NPCID.AngryNimbus:
                        npc.buffImmune[BuffID.Suffocation] = true;
                        break;

                    case NPCID.BlueJellyfish:
                    case NPCID.Crab:
                    case NPCID.PinkJellyfish:
                    case NPCID.Piranha:
                    case NPCID.SeaSnail:
                    case NPCID.Shark:
                    case NPCID.Squid:
                    case NPCID.AnglerFish:
                    case NPCID.Arapaima:
                    case NPCID.BloodFeeder:
                    case NPCID.BloodJelly:
                    case NPCID.FungoFish:
                    case NPCID.GreenJellyfish:
                    case NPCID.Goldfish:
                    case NPCID.CorruptGoldfish:
                    case NPCID.CrimsonGoldfish:
                    case NPCID.WaterSphere:
                        isWaterEnemy = true;
                        break;

                    #endregion

                    default:
                        break;
                }

                //if (npc.boss) npc.npcSlots += 100;
            }
        }

        public override bool PreAI(NPC npc)
        {
            if (TimeFrozen)
            {
                npc.position = npc.oldPosition;
                npc.frameCounter = 0;
                return false;
            }

            if (Stop > 0)
            {
                Stop--;
                npc.position = npc.oldPosition;
                npc.frameCounter = 0;
            }

            if (npc.boss)
            {
                boss = npc.whoAmI;
            }

            if (!FirstTick)
            {
                switch (npc.type)
                {
                    case NPCID.WallofFlesh:
                    case NPCID.WallofFleshEye:
                    case NPCID.MoonLordCore:
                    case NPCID.MoonLordHand:
                    case NPCID.MoonLordHead:
                    case NPCID.MoonLordLeechBlob:
                    case NPCID.TargetDummy:
                    case NPCID.GolemFistLeft:
                    case NPCID.GolemFistRight:
                    case NPCID.GolemHead:
                        SpecialEnchantImmune = true;
                        break;

                    case NPCID.Squirrel:
                    case NPCID.SquirrelRed:
                        if (Main.rand.Next(8) == 0)
                            npc.Transform(mod.NPCType("TophatSquirrel"));
                        break;

                    default:
                        break;
                }

                //critters
                if (Main.player[Main.myPlayer].GetModPlayer<FargoPlayer>().WoodEnchant && npc.damage == 0 && !npc.townNPC && npc.lifeMax == 5)
                {
                    npc.defense = 9999;
                    npc.defDefense = 9999;
                }

                //transformations, hordes, and other first tick maso stuff
                if (FargoSoulsWorld.MasochistMode)
                {
                    switch (npc.type)
                    {
                        case NPCID.Zombie:
                            if (Main.rand.Next(8) == 0)
                                Horde(npc, 6);
                            if (Main.rand.Next(5) == 0)
                                npc.Transform(NPCID.ArmedZombie);
                            break;

                        case NPCID.ZombieEskimo: if (Main.rand.Next(5) == 0) npc.Transform(NPCID.ArmedZombieEskimo); break;
                        case NPCID.PincushionZombie: if (Main.rand.Next(5) == 0) npc.Transform(NPCID.ArmedZombiePincussion); break;
                        case NPCID.FemaleZombie: if (Main.rand.Next(5) == 0) npc.Transform(NPCID.ArmedZombieCenx); break;
                        case NPCID.SlimedZombie: if (Main.rand.Next(5) == 0) npc.Transform(NPCID.ArmedZombieSlimed); break;
                        case NPCID.TwiggyZombie: if (Main.rand.Next(5) == 0) npc.Transform(NPCID.ArmedZombieTwiggy); break;
                        case NPCID.SwampZombie: if (Main.rand.Next(5) == 0) npc.Transform(NPCID.ArmedZombieSwamp); break;
                        case NPCID.Skeleton: if (Main.rand.Next(5) == 0) npc.Transform(NPCID.BoneThrowingSkeleton); break;
                        case NPCID.HeadacheSkeleton: if (Main.rand.Next(5) == 0) npc.Transform(NPCID.BoneThrowingSkeleton2); break;
                        case NPCID.MisassembledSkeleton: if (Main.rand.Next(5) == 0) npc.Transform(NPCID.BoneThrowingSkeleton3); break;
                        case NPCID.PantlessSkeleton: if (Main.rand.Next(5) == 0) npc.Transform(NPCID.BoneThrowingSkeleton4); break;
                        case NPCID.JungleSlime: if (Main.rand.Next(5) == 0) npc.Transform(NPCID.SpikedJungleSlime); break;
                        case NPCID.IceSlime: if (Main.rand.Next(5) == 0) npc.Transform(NPCID.SpikedIceSlime); break;

                        case NPCID.EaterofSouls:
                        case NPCID.BigEater:
                        case NPCID.LittleEater:
                        case NPCID.Crimera:
                        case NPCID.BigCrimera:
                        case NPCID.LittleCrimera:
                            if (NPC.downedBoss2 && Main.rand.Next(5) == 0)
                                Horde(npc, 5);
                            break;

                        case NPCID.CaveBat:
                        case NPCID.JungleBat:
                        case NPCID.Hellbat:
                        case NPCID.IceBat:
                        case NPCID.GiantBat:
                        case NPCID.IlluminantBat:
                        case NPCID.Lavabat:
                        case NPCID.GiantFlyingFox:
                            if (Main.rand.Next(4) == 0)
                                Horde(npc, Main.rand.Next(5) + 1);
                            break;

                        case NPCID.Shark:
                            if (Main.rand.Next(4) == 0)
                                Horde(npc, 2);
                            break;

                        case NPCID.WyvernHead:
                            if (Main.rand.Next(4) == 0)
                                Horde(npc, 2);
                            break;

                        case NPCID.Wolf:
                            if (Main.rand.Next(3) == 0)
                                Horde(npc, 5);
                            break;

                        case NPCID.FlyingFish:
                            if (Main.rand.Next(4) == 0)
                                Horde(npc, 3);
                            break;

                        case NPCID.Ghost:
                            if (Main.rand.Next(5) == 0)
                                Horde(npc, 3);
                            break;

                        case NPCID.GreekSkeleton:
                            if (Main.rand.Next(3) == 0)
                                Horde(npc, 3);
                            break;

                        case NPCID.RedDevil:
                            if (Main.hardMode && Main.rand.Next(5) == 0)
                                Horde(npc, 5);
                            break;

                        case NPCID.MossHornet:
                            if (Main.rand.Next(4) == 0)
                                Horde(npc, 5);
                            break;

                        case NPCID.FlyingAntlion:
                            if (Main.rand.Next(3) == 0)
                                Horde(npc, 3);
                            break;

                        case NPCID.Probe:
                            if (Main.rand.Next(4) == 0 && !AnyBossAlive())
                                Horde(npc, 8);
                            break;

                        case NPCID.Crawdad:
                        case NPCID.GiantShelly:
                            if (Main.rand.Next(5) == 0) //pick a random salamander
                                npc.Transform(Main.rand.Next(498, 507));
                            break;

                        case NPCID.Salamander:
                        case NPCID.Salamander2:
                        case NPCID.Salamander3:
                        case NPCID.Salamander4:
                        case NPCID.GiantShelly2:
                            if (Main.rand.Next(5) == 0) //pick a random crawdad
                                npc.Transform(Main.rand.Next(494, 496));
                            break;

                        case NPCID.Salamander5:
                        case NPCID.Salamander6:
                        case NPCID.Salamander7:
                        case NPCID.Salamander8:
                        case NPCID.Crawdad2:
                            if (Main.rand.Next(5) == 0) //pick a random shelly
                                npc.Transform(Main.rand.Next(496, 498));
                            break;

                        case NPCID.Goldfish:
                        case NPCID.GoldfishWalker:
                        case NPCID.BlueJellyfish:
                            if (Main.rand.Next(6) == 0) //random sharks
                                npc.Transform(NPCID.Shark);
                            break;

                        //mimic swapping
                        case NPCID.BigMimicCorruption:
                            if (Main.rand.Next(4) == 0)
                                npc.Transform(NPCID.BigMimicCrimson);
                            break;
                        case NPCID.BigMimicCrimson:
                            if (Main.rand.Next(4) == 0)
                                npc.Transform(NPCID.BigMimicCorruption);
                            break;

                        case NPCID.VortexLarva:
                            if (Main.rand.Next(2) == 0)
                                npc.Transform(NPCID.VortexHornet);
                            break;

                        case NPCID.Bee:
                        case NPCID.BeeSmall:
                            if (Main.rand.Next(3) == 0)
                                switch ((Main.hardMode && !BossIsAlive(ref beeBoss, NPCID.QueenBee)) ? Main.rand.Next(16, 21) : Main.rand.Next(16))
                                {
                                    case 0: npc.Transform(NPCID.Hornet); break;
                                    case 1: npc.Transform(NPCID.HornetFatty); break;
                                    case 2: npc.Transform(NPCID.HornetHoney); break;
                                    case 3: npc.Transform(NPCID.HornetLeafy); break;
                                    case 4: npc.Transform(NPCID.HornetSpikey); break;
                                    case 5: npc.Transform(NPCID.HornetStingy); break;
                                    case 6: npc.Transform(NPCID.LittleHornetFatty); break;
                                    case 7: npc.Transform(NPCID.LittleHornetHoney); break;
                                    case 8: npc.Transform(NPCID.LittleHornetLeafy); break;
                                    case 9: npc.Transform(NPCID.LittleHornetSpikey); break;
                                    case 10: npc.Transform(NPCID.LittleHornetStingy); break;
                                    case 11: npc.Transform(NPCID.BigHornetFatty); break;
                                    case 12: npc.Transform(NPCID.BigHornetHoney); break;
                                    case 13: npc.Transform(NPCID.BigHornetLeafy); break;
                                    case 14: npc.Transform(NPCID.BigHornetSpikey); break;
                                    case 15: npc.Transform(NPCID.BigHornetStingy); break;
                                    case 16: npc.Transform(NPCID.MossHornet); break;
                                    case 17: npc.Transform(NPCID.BigMossHornet); break;
                                    case 18: npc.Transform(NPCID.GiantMossHornet); break;
                                    case 19: npc.Transform(NPCID.LittleMossHornet); break;
                                    case 20: npc.Transform(NPCID.TinyMossHornet); break;
                                }
                            break;

                        case NPCID.Hornet:
                        case NPCID.HornetFatty:
                        case NPCID.HornetHoney:
                        case NPCID.HornetLeafy:
                        case NPCID.HornetSpikey:
                        case NPCID.HornetStingy:
                            if (Main.hardMode && !BossIsAlive(ref beeBoss, NPCID.QueenBee))
                                switch (Main.rand.Next(5))
                                {
                                    case 0: npc.Transform(NPCID.MossHornet); break;
                                    case 1: npc.Transform(NPCID.BigMossHornet); break;
                                    case 2: npc.Transform(NPCID.GiantMossHornet); break;
                                    case 3: npc.Transform(NPCID.LittleMossHornet); break;
                                    case 4: npc.Transform(NPCID.TinyMossHornet); break;
                                }
                            break;

                        case NPCID.MeteorHead:
                            if (NPC.downedGolemBoss && Main.rand.Next(4) == 0)
                                npc.Transform(NPCID.SolarCorite);
                            break;

                        case NPCID.DemonEye:
                        case NPCID.DemonEyeOwl:
                        case NPCID.DemonEyeSpaceship:
                        case NPCID.CataractEye:
                        case NPCID.SleepyEye:
                        case NPCID.DialatedEye:
                        case NPCID.GreenEye:
                        case NPCID.PurpleEye:
                            if (Main.hardMode && Main.rand.Next(4) == 0)
                                npc.Transform(NPCID.WanderingEye);
                            break;

                        case NPCID.MothronEgg:
                            npc.Transform(NPCID.MothronSpawn);
                            break;

                        case NPCID.DiabolistRed:
                            if (Main.rand.Next(4) == 0)
                                npc.Transform(Main.rand.Next(2) == 0 ? NPCID.Necromancer : NPCID.RaggedCaster);
                            break;

                        case NPCID.DiabolistWhite:
                            if (Main.rand.Next(4) == 0)
                                npc.Transform(Main.rand.Next(2) == 0 ? NPCID.NecromancerArmored : NPCID.RaggedCasterOpenCoat);
                            break;

                        case NPCID.Necromancer:
                            if (Main.rand.Next(4) == 0)
                                npc.Transform(Main.rand.Next(2) == 0 ? NPCID.DiabolistRed : NPCID.RaggedCaster);
                            break;

                        case NPCID.NecromancerArmored:
                            if (Main.rand.Next(4) == 0)
                                npc.Transform(Main.rand.Next(2) == 0 ? NPCID.DiabolistWhite : NPCID.RaggedCasterOpenCoat);
                            break;

                        case NPCID.RaggedCaster:
                            if (Main.rand.Next(4) == 0)
                                npc.Transform(Main.rand.Next(2) == 0 ? NPCID.DiabolistRed : NPCID.Necromancer);
                            break;

                        case NPCID.RaggedCasterOpenCoat:
                            if (Main.rand.Next(4) == 0)
                                npc.Transform(Main.rand.Next(2) == 0 ? NPCID.DiabolistWhite : NPCID.NecromancerArmored);
                            break;

                        default:
                            break;
                    }
                }

                FirstTick = true;
            }

            if (Lethargic && ++LethargicCounter > 3)
            {
                LethargicCounter = 0;
                return false;
            }

            if (valhallaCounter > 0)
            {
                valhallaCounter--;
            }
            

            if (FargoSoulsWorld.MasochistMode)
            {
                if (RegenTimer > 0)
                    RegenTimer--;

                if (npc.position.Y / 16 < Main.worldSurface * 0.35f) //enemy in space
                    npc.AddBuff(BuffID.Suffocation, 2);
                else if (npc.position.Y / 16 > Main.maxTilesY - 200) //enemy in hell
                    npc.AddBuff(BuffID.OnFire, 2);

                if (npc.wet && !npc.noTileCollide && !isWaterEnemy && npc.HasPlayerTarget)
                {
                    npc.AddBuff(mod.BuffType("Lethargic"), 2);
                    if (Main.player[npc.target].ZoneCorrupt)
                        npc.AddBuff(BuffID.CursedInferno, 2);
                    if (Main.player[npc.target].ZoneCrimson)
                        npc.AddBuff(BuffID.Ichor, 2);
                    if (Main.player[npc.target].ZoneHoly)
                        npc.AddBuff(BuffID.Confused, 2);
                    if (Main.player[npc.target].ZoneJungle)
                        npc.AddBuff(BuffID.Poisoned, 2);
                }
                
                switch (npc.type)
                {
                    case NPCID.DD2EterniaCrystal:
                        if (DD2Event.Ongoing && DD2Event.TimeLeftBetweenWaves > 600)
                            DD2Event.TimeLeftBetweenWaves = 600;

                        //cant use HasValidTarget for this because that returns true even if betsy is targeting the crystal (npc.target seems to become -1)
                        if (BossIsAlive(ref betsyBoss, NPCID.DD2Betsy) && Main.npc[betsyBoss].HasPlayerTarget
                            && Main.player[Main.npc[betsyBoss].target].active && !Main.player[Main.npc[betsyBoss].target].dead && !Main.player[Main.npc[betsyBoss].target].ghost)
                        {
                            Counter = 180; //even if betsy targets crystal, wait 3 seconds before becoming fully vulnerable
                        }

                        if (Counter > 0)
                        {
                            Counter--;
                            npc.defense = 99999;
                        }
                        else
                        {
                            npc.defense = npc.defDefense;
                        }
                        break;

                    case NPCID.DesertBeast:
                        Aura(npc, 250, mod.BuffType("Infested"), false, 188);
                        break;

                    case NPCID.Tim:
                        Aura(npc, 450, BuffID.Silenced, true, 15, true);
                        Aura(npc, 150, BuffID.Cursed, false, 20, true);
                        goto case NPCID.DarkCaster;

                    case NPCID.RuneWizard:
                        if (npc.Distance(Main.player[Main.myPlayer].Center) < 1500f)
                        {
                            if (npc.Distance(Main.player[Main.myPlayer].Center) > 400f)
                                Main.player[Main.myPlayer].AddBuff(mod.BuffType("Hexed"), 2);

                            for (int i = 0; i < 20; i++)
                            {
                                Vector2 offset = new Vector2();
                                double angle = Main.rand.NextDouble() * 2d * Math.PI;
                                offset.X += (float)(Math.Sin(angle) * 400);
                                offset.Y += (float)(Math.Cos(angle) * 400);
                                Dust dust = Main.dust[Dust.NewDust(
                                    npc.Center + offset - new Vector2(4, 4), 0, 0,
                                    DustID.BubbleBlock, 0, 0, 100, default(Color), 1f
                                    )];
                                dust.velocity = npc.velocity;
                                if (Main.rand.Next(3) == 0)
                                    dust.velocity += Vector2.Normalize(offset) * 5f;
                                dust.noGravity = true;
                                dust.color = Color.GreenYellow;
                            }
                        }
                        Aura(npc, 200, BuffID.Suffocation, false, 119);
                        if (npc.Distance(Main.player[Main.myPlayer].Center) < 200)
                            Main.player[Main.myPlayer].AddBuff(mod.BuffType("Hexed"), 2);
                        if (++Counter > 300)
                        {
                            Counter = 0;
                            if (Main.netMode != 1 && npc.HasPlayerTarget)
                            {
                                Vector2 vel = npc.DirectionFrom(Main.player[npc.target].Center) * 8f;
                                for (int i = 0; i < 5; i++)
                                {
                                    int p = Projectile.NewProjectile(npc.Center, vel.RotatedBy(2 * Math.PI / 5 * i),
                                        ProjectileID.RuneBlast, 25, 0f, Main.myPlayer, 1);
                                    if (p != 1000)
                                        Main.projectile[p].timeLeft = 300;
                                }
                            }
                        }
                        goto case NPCID.DarkCaster;

                    case NPCID.CochinealBeetle:
                    case NPCID.CyanBeetle:
                    case NPCID.LacBeetle:
                        //Aura(npc, 400, mod.BuffType("Lethargic"), false, 60);
                        break;

                    case NPCID.EnchantedSword:
                    case NPCID.CursedHammer:
                    case NPCID.CrimsonAxe:
                        npc.position += npc.velocity / 2f;
                        Aura(npc, 300, BuffID.WitheredArmor, true, 119);
                        Aura(npc, 300, BuffID.WitheredWeapon, true, 14);
                        if (npc.ai[0] == 2f) //spinning up
                            npc.ai[1] += 6f * (1f - (float)npc.life / npc.lifeMax); //FINISH SPINNING FASTER
                        break;

                    case NPCID.Ghost:
                        Aura(npc, 100, BuffID.Cursed, false, 20, true);
                        break;

                    case NPCID.Snatcher:
                    case NPCID.ManEater:
                        if (npc.HasValidTarget && Main.player[npc.target].statLife < 100)
                            SharkCount = 2;
                        else
                            SharkCount = 0;
                        break;

                    case NPCID.AngryTrapper:
                        if (npc.HasValidTarget && Main.player[npc.target].statLife < 180)
                            SharkCount = 2;
                        else
                            SharkCount = 0;
                        break;

                    case NPCID.Mummy:
                    case NPCID.DarkMummy:
                    case NPCID.LightMummy:
                        Aura(npc, 500, BuffID.Slow, false, 0, true);
                        break;

                    case NPCID.Derpling:
                        Aura(npc, 600, BuffID.Confused, true, 15, true);
                        break;

                    case NPCID.Crimera:
                        npc.noTileCollide = true;
                        if (npc.noTileCollide && Framing.GetTileSafely(npc.Center).nactive() && Main.tileSolid[Framing.GetTileSafely(npc.Center).type])
                        {
                            int d = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Fire, npc.velocity.X, npc.velocity.Y);
                            Main.dust[d].noGravity = true;
                            npc.position -= npc.velocity / 2f;
                        }
                        break;

                    case NPCID.FaceMonster:
                        Aura(npc, 200, BuffID.Obstructed, false, 199);
                        break;

                    case NPCID.IlluminantBat:
                        if (masoBool[0])
                        {
                            if (!masoBool[1] && ++Counter2 > 15) //MP sync
                            {
                                masoBool[1] = true;
                                NetUpdateMaso(npc.whoAmI);
                            }
                            npc.alpha = 200;
                            if (npc.lifeMax > 100)
                                npc.lifeMax = 100;
                            if (npc.life > npc.lifeMax)
                                npc.life = npc.lifeMax;
                        }
                        if (npc.HasPlayerTarget && npc.Distance(Main.player[npc.target].Center) < 1000)
                        {
                            if (++Counter >= 600)
                            {
                                Counter = 0;
                                if (Main.netMode != 1 && NPC.CountNPCS(NPCID.IlluminantBat) < 20)
                                {
                                    int bat = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.IlluminantBat);
                                    if (bat < 200)
                                    {
                                        Main.npc[bat].velocity.X = Main.rand.Next(-5, 6);
                                        Main.npc[bat].velocity.Y = Main.rand.Next(-5, 6);
                                        Main.npc[bat].GetGlobalNPC<FargoSoulsGlobalNPC>().masoBool[0] = true;
                                        if (Main.netMode == 2)
                                            NetMessage.SendData(23, -1, -1, null, bat);
                                    }
                                }
                            }
                        }
                        break;

                    case NPCID.MeteorHead:
                        Counter++;
                        if (Counter >= 120)
                        {
                            Counter = 0;
                            int t = npc.HasPlayerTarget ? npc.target : npc.FindClosestPlayer();
                            if (t != -1 && npc.Distance(Main.player[t].Center) < 600 && Main.netMode != 1)
                            {
                                npc.velocity *= 5;
                                npc.netUpdate = true;
                            }
                        }
                        Aura(npc, 100, BuffID.Burning, false, DustID.Fire);
                        break;

                    case NPCID.BoneSerpentHead:
                        Counter++;
                        if (Counter >= 300)
                        {
                            Counter = 0;
                            int t = npc.HasPlayerTarget ? npc.target : npc.FindClosestPlayer();
                            if (t != -1 && npc.Distance(Main.player[t].Center) < 600 && Main.netMode != 1)
                            {
                                int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.BurningSphere);
                                if (n != 200 && Main.netMode == 2)
                                    NetMessage.SendData(23, -1, -1, null, n);
                            }
                        }
                        break;

                    case NPCID.Vulture:
                        if (npc.ai[0] != 0f)
                        {
                            Counter++;
                            if (Counter >= 300)
                                Shoot(npc, 30, 500, 10, ProjectileID.HarpyFeather, npc.damage / 4, 1, true);
                        }
                        break;

                    case NPCID.DoctorBones:
                        Counter++;
                        if (Counter >= 600)
                            Shoot(npc, 120, 1000, 14, ProjectileID.Boulder, npc.damage, 2);
                        break;

                    case NPCID.Crab:
                        Counter++;
                        if (Counter >= 300)
                            Shoot(npc, 30, 800, 14, ProjectileID.Bubble, npc.damage / 4, 1, false, true);
                        break;

                    case NPCID.ArmoredViking:
                        Counter++;
                        if (Counter >= 10)
                        {
                            Counter = 0;
                            if (Main.netMode != 1 && npc.HasPlayerTarget //collision check to reduce spam when not relevant
                                && Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))
                            {
                                Vector2 vel = npc.DirectionTo(Main.player[npc.target].Center) * 14f;
                                Projectile.NewProjectile(npc.Center, vel, mod.ProjectileType("IceSickleHostile"), npc.damage / 4, 0f, Main.myPlayer);
                            }
                        }
                        break;

                    case NPCID.Crawdad:
                    case NPCID.Crawdad2:
                        Counter++;
                        if (Counter >= 300)
                        {
                            Counter = 0;
                            int t = npc.HasPlayerTarget ? npc.target : npc.FindClosestPlayer();
                            if (t != -1)
                            {
                                Player player = Main.player[t];
                                if (npc.Distance(player.Center) < 800 & Main.netMode != 1)
                                {
                                    Vector2 velocity = Vector2.Normalize(player.Center - npc.Center) * 10;
                                    int bubble = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.DetonatingBubble);
                                    if (bubble < 200)
                                    {
                                        Main.npc[bubble].velocity = velocity;
                                        Main.npc[bubble].damage = npc.damage / 2;
                                        if (Main.netMode == 2)
                                            NetMessage.SendData(23, -1, -1, null, bubble);
                                    }
                                }
                            }
                        }
                        break;

                    case NPCID.WallCreeperWall:
                    case NPCID.BloodCrawlerWall:
                    case NPCID.JungleCreeperWall:
                        if (++Counter >= 360)
                            Shoot(npc, 60, 400, 14, ProjectileID.WebSpit, 9, 0);
                        break;

                    case NPCID.SeekerHead:
                        Counter++;
                        if (Counter >= 10)
                        {
                            Counter = 0;
                            int t = npc.HasPlayerTarget ? npc.target : npc.FindClosestPlayer();
                            if (t != -1 && npc.Distance(Main.player[t].Center) < 500)
                                Projectile.NewProjectile(npc.Center, npc.velocity, ProjectileID.EyeFire, npc.damage / 4, 0f, Main.myPlayer);
                        }
                        break;

                    case NPCID.Demon:
                        //Counter++;


                        if (npc.ai[0] == 100f)
                        {
                            //Counter = 0;
                            int t = npc.HasPlayerTarget ? npc.target : npc.FindClosestPlayer();
                            if (t != -1 && npc.Distance(Main.player[t].Center) < 800 && Main.netMode != 1)
                                FargoGlobalProjectile.XWay(6, npc.Center, ProjectileID.DemonSickle, 1, npc.damage / 4, .5f);
                        }
                        break;

                    case NPCID.VoodooDemon:
                        if (npc.lavaWet)
                        {
                            npc.buffImmune[BuffID.OnFire] = false;
                            npc.AddBuff(BuffID.OnFire, 600);
                        }
                        if (!npc.HasBuff(BuffID.OnFire))
                        {
                            Timer = 600;
                        }
                        else
                        {
                            Main.PlaySound(4, (int)npc.position.X, (int)npc.position.Y, 10, 1f, 0.5f);
                            Timer--;
                            if (Timer <= 0 && !BossIsAlive(ref wallBoss, NPCID.WallofFlesh) && npc.HasPlayerTarget && Main.netMode != 1)
                            {
                                int guide = NPC.FindFirstNPC(NPCID.Guide);
                                if (guide != -1 && Main.npc[guide].active)
                                {
                                    Main.npc[guide].StrikeNPC(9999, 0f, 0);
                                    NPC.SpawnWOF(Main.player[npc.target].Center);
                                    /*if (Main.netMode == 0)
                                        Main.NewText("Wall of Flesh has awoken!", 175, 75, 255);
                                    else if (Main.netMode == 2)
                                        NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("Wall of Flesh has awoken!"), new Color(175, 75, 255));*/
                                }
                                npc.Transform(NPCID.Demon);
                            }
                        }
                        break;

                    case NPCID.Piranha:
                        Counter++;
                        if (Counter >= 120)
                        {
                            Counter = 0;
                            int t = npc.HasPlayerTarget ? npc.target : npc.FindClosestPlayer();
                            if (t != -1 && Main.rand.Next(2) == 0)
                            {
                                Player player = Main.player[t];
                                if (player.bleed && Main.netMode != 1)
                                {
                                    if (player.ZoneJungle)
                                        masoBool[0] = true;
                                    if (NPC.CountNPCS(NPCID.Piranha) <= 6)
                                    {
                                        int piranha = NPC.NewNPC((int)npc.Center.X + Main.rand.Next(-20, 20), (int)npc.Center.Y + Main.rand.Next(-20, 20), NPCID.Piranha);
                                        if (piranha != 200 && Main.netMode == 2)
                                            NetMessage.SendData(23, -1, -1, null, piranha);
                                    }
                                }
                            }
                        }
                        if (masoBool[0] && npc.wet && ++Counter2 > 240) //initiate jump
                        {
                            Counter2 = 0;
                            int t = npc.HasPlayerTarget ? npc.target : npc.FindClosestPlayer();
                            if (Main.rand.Next(2) == 0 && t != -1 && Main.netMode != 1)
                            {
                                const float gravity = 0.3f;
                                const float time = 120f;
                                Vector2 distance = Main.player[t].Center - npc.Center;
                                distance.X = distance.X / time;
                                distance.Y = distance.Y / time - 0.5f * gravity * time;
                                npc.ai[1] = 120f;
                                npc.ai[2] = distance.X;
                                npc.ai[3] = distance.Y;
                                npc.netUpdate = true;
                            }
                        }
                        if (npc.ai[1] > 0f) //while jumping
                        {
                            npc.ai[1]--;
                            npc.noTileCollide = true;
                            npc.velocity.X = npc.ai[2];
                            npc.velocity.Y = npc.ai[3];
                            npc.ai[3] += 0.3f;
                            masoBool[0] = false;
                        }
                        else
                        {
                            npc.noTileCollide = false;
                        }
                        break;

                    case NPCID.Arapaima:
                        if (++Counter2 > 420) //initiate jump
                        {
                            Counter2 = 0;
                            int t = npc.HasPlayerTarget ? npc.target : npc.FindClosestPlayer();
                            if (npc.life < npc.lifeMax && t != -1 && Main.player[t].wet && Main.netMode != 1)
                            {
                                const float gravity = 0.3f;
                                const float time = 120f;
                                Vector2 distance = Main.player[t].Center - npc.Center;
                                distance.X = distance.X / time;
                                distance.Y = distance.Y / time - 0.5f * gravity * time;
                                npc.ai[1] = 120f;
                                npc.ai[2] = distance.X;
                                npc.ai[3] = distance.Y;
                                npc.netUpdate = true;
                            }
                        }
                        if (npc.ai[1] > 0f) //while jumping
                        {
                            npc.ai[1]--;
                            npc.noTileCollide = true;
                            npc.velocity.X = npc.ai[2];
                            npc.velocity.Y = npc.ai[3];
                            npc.ai[3] += 0.3f;
                        }
                        else
                        {
                            npc.noTileCollide = false;
                        }
                        break;

                    case NPCID.Shark:
                        if (npc.life < npc.lifeMax / 2 && --Counter2 < 0) //initiate jump
                        {
                            Counter2 = 360;
                            int t = npc.HasPlayerTarget ? npc.target : npc.FindClosestPlayer();
                            if (t != -1 && Main.netMode != 1)
                            {
                                const float gravity = 0.3f;
                                const float time = 90;
                                Vector2 distance = Main.player[t].Center - npc.Center;
                                distance.X = distance.X / time;
                                distance.Y = distance.Y / time - 0.5f * gravity * time;
                                npc.ai[1] = time;
                                npc.ai[2] = distance.X;
                                npc.ai[3] = distance.Y;
                                npc.netUpdate = true;
                            }
                        }
                        if (npc.ai[1] > 0f) //while jumping
                        {
                            npc.ai[1]--;
                            npc.noTileCollide = true;
                            npc.velocity.X = npc.ai[2];
                            npc.velocity.Y = npc.ai[3];
                            npc.ai[3] += 0.3f;
                        }
                        else
                        {
                            npc.noTileCollide = false;
                        }
                        goto case NPCID.SandShark;
                    case NPCID.SandShark:
                    case NPCID.SandsharkCorrupt:
                    case NPCID.SandsharkCrimson:
                    case NPCID.SandsharkHallow:
                        Counter++;
                        if (Counter >= 240)
                        {
                            Counter = 0;
                            int t = npc.HasPlayerTarget ? npc.target : npc.FindClosestPlayer();
                            if (t != -1 && SharkCount < 5)
                            {
                                Player player = Main.player[t];
                                if (player.bleed && Main.netMode != 1)
                                {
                                    SharkCount++;
                                    npc.netUpdate = true;
                                    if (Main.netMode == 2)
                                    {
                                        var netMessage = mod.GetPacket();
                                        netMessage.Write((byte)6);
                                        netMessage.Write((byte)npc.whoAmI);
                                        netMessage.Write(SharkCount);
                                        netMessage.Send();
                                    }
                                }
                            }
                        }
                        if (SharkCount > 0)
                            npc.damage = (int)(npc.defDamage * (1f + SharkCount / 2f));
                        break;

                    case NPCID.BlackRecluse:
                    case NPCID.BlackRecluseWall:
                        Counter++;
                        if (Counter >= 10)
                        {
                            Counter = 0;
                            int t = npc.HasPlayerTarget ? npc.target : npc.FindClosestPlayer();
                            if (t != -1)
                            {
                                Player player = Main.player[t];
                                int b = player.FindBuffIndex(BuffID.Webbed);
                                masoBool[0] = (b != -1); //remember if target is webbed until counter activates again
                                if (masoBool[0])
                                    player.AddBuff(mod.BuffType("Defenseless"), player.buffTime[b]);
                            }
                        }

                        if (masoBool[0])
                        {
                            npc.position += npc.velocity;
                            SharkCount = 1;
                        }
                        else
                        {
                            SharkCount = 0;
                        }
                        break;

                    case NPCID.EyeofCthulhu:
                        eyeBoss = npc.whoAmI;

                        Counter++;
                        if (Counter >= 600)
                        {
                            Counter = 0;
                            if (npc.life <= npc.lifeMax * 0.65 && NPC.CountNPCS(NPCID.ServantofCthulhu) < 12 && Main.netMode != 1)
                            {
                                Vector2 vel = new Vector2(2, 2);
                                for (int i = 0; i < 4; i++)
                                {
                                    int n = NPC.NewNPC((int)(npc.position.X + npc.width / 2), (int)(npc.position.Y + npc.height), NPCID.ServantofCthulhu);
                                    if (n != 200)
                                    {
                                        Main.npc[n].velocity = vel.RotatedBy(Math.PI / 2 * i);
                                        if (Main.netMode == 2)
                                            NetMessage.SendData(23, -1, -1, null, n);
                                    }
                                }
                            }
                        }

                        if (npc.life < npc.lifeMax / 2)
                        {
                            if (npc.ai[0] == 3 && (npc.ai[1] == 0 || npc.ai[1] == 5))
                            {
                                if (npc.ai[2] < 2)
                                {
                                    npc.ai[2]--;
                                    npc.alpha += 6;
                                    for (int i = 0; i < 3; i++)
                                    {
                                        int d = Dust.NewDust(npc.position, npc.width, npc.height, 229, 0f, 0f, 0, default(Color), 1.5f);
                                        Main.dust[d].noGravity = true;
                                        Main.dust[d].noLight = true;
                                        Main.dust[d].velocity *= 4f;
                                    }
                                    if (npc.alpha > 255)
                                    {
                                        npc.alpha = 255;
                                        if (Main.netMode != 1 && npc.HasPlayerTarget)
                                        {
                                            Vector2 distance = npc.Center - Main.player[npc.target].Center;
                                            npc.Center = Main.player[npc.target].Center;
                                            distance.X *= 1.5f;
                                            if (distance.X > 1200)
                                                distance.X = 1200;
                                            else if (distance.X < -1200)
                                                distance.X = -1200;
                                            if (distance.Y > 0)
                                                distance.Y *= -1;
                                            npc.position.X -= distance.X;
                                            npc.position.Y += distance.Y;
                                            npc.netUpdate = true;
                                            npc.ai[2] = 60;
                                            npc.ai[1] = 5f;//
                                        }
                                    }
                                }
                                else
                                {
                                    npc.alpha -= 6;
                                    if (npc.alpha < 0)
                                    {
                                        npc.alpha = 0;
                                    }
                                    else
                                    {
                                        npc.ai[2]--;
                                        npc.position -= npc.velocity / 2;
                                        for (int i = 0; i < 3; i++)
                                        {
                                            int d = Dust.NewDust(npc.position, npc.width, npc.height, 229, 0f, 0f, 0, default(Color), 1.5f);
                                            Main.dust[d].noGravity = true;
                                            Main.dust[d].noLight = true;
                                            Main.dust[d].velocity *= 4f;
                                        }
                                    }
                                }
                            }

                            npc.dontTakeDamage = npc.alpha > 50;

                            if (Counter2 > 0)
                            {
                                if (Counter2 % 6 == 0 && Main.netMode != 1)
                                    Projectile.NewProjectile(new Vector2(npc.Center.X + Main.rand.Next(-15, 15), npc.Center.Y), npc.velocity / 10, mod.ProjectileType("BloodScythe"), npc.damage / 4, 1f, Main.myPlayer);
                                Counter2--;
                                
                            }
                            if (npc.ai[1] == 3f) //during dashes in phase 2
                            {
                                Counter2 = 30;
                                masoBool[0] = false;
                                if (Main.netMode != 1)
                                    FargoGlobalProjectile.XWay(8, npc.Center, mod.ProjectileType("BloodScythe"), 1.5f, npc.damage / 4, 0);
                            }
                            /*if (++Timer > 600)
                            {
                                Timer = 0;
                                if (npc.HasValidTarget)
                                {
                                    Player player = Main.player[npc.target];
                                    Main.PlaySound(29, (int)player.position.X, (int)player.position.Y, 104, 1f, 0f);
                                    if (Main.netMode != 1)
                                    {
                                        Vector2 spawnPos = player.Center;
                                        int direction;
                                        if (player.velocity.X == 0f)
                                            direction = player.direction;
                                        else
                                            direction = Math.Sign(player.velocity.X);
                                        spawnPos.X += 600 * direction;
                                        spawnPos.Y -= 600;
                                        Vector2 speed = Vector2.UnitY;
                                        for (int i = 0; i < 30; i++)
                                        {
                                            Projectile.NewProjectile(spawnPos, speed, mod.ProjectileType("BloodScythe"), npc.damage / 4, 1f, Main.myPlayer);
                                            spawnPos.X += 72 * direction;
                                            speed.Y += 0.15f;
                                        }
                                    }
                                }
                            }*/
                        }
                        else
                        {
                            npc.alpha = 0;
                            npc.dontTakeDamage = false;
                        }
                        break;

                    case NPCID.Retinazer:
                        retiBoss = npc.whoAmI;
                        bool spazAlive = BossIsAlive(ref spazBoss, NPCID.Spazmatism);
                        bool targetAlive = npc.HasPlayerTarget && Main.player[npc.target].active;

                        if (!masoBool[0]) //start phase 2
                        {
                            masoBool[0] = true;
                            npc.ai[0] = 1f;
                            npc.ai[1] = 0.0f;
                            npc.ai[2] = 0.0f;
                            npc.ai[3] = 0.0f;
                            npc.netUpdate = true;
                        }

                        if (npc.ai[0] < 4f) //going to phase 3
                        {
                            if (npc.life <= npc.lifeMax / 2)
                            {
                                //npc.ai[0] = 4f;
                                npc.ai[0] = 604f; //initiate spin immediately
                                npc.netUpdate = true;
                                Main.PlaySound(15, (int)npc.Center.X, (int)npc.Center.Y, 0);
                            }
                        }
                        else
                        {
                            Player p = Main.player[Main.myPlayer];
                            const float auraDistance = 2000;
                            float range = npc.Distance(p.Center);
                            if (range > auraDistance && range < 10000)
                                p.AddBuff(BuffID.Burning, 2);

                            Vector2 dustPos = Vector2.Normalize(p.Center - npc.Center) * auraDistance;
                            for (int i = 0; i < 20; i++) //dust
                            {
                                int d = Dust.NewDust(npc.Center + dustPos.RotatedBy(Math.PI / 3 * (-0.5 + Main.rand.NextDouble())), 0, 0, DustID.Fire);
                                Main.dust[d].velocity = npc.velocity;
                                Main.dust[d].noGravity = true;
                                Main.dust[d].noLight = true;
                                Main.dust[d].scale++;
                            }

                            if (masoBool[3] && --Counter2 < 0) //when brought to 1hp, begin shooting dark stars
                            {
                                Counter2 = 240;
                                if (Main.netMode != 1 && npc.HasPlayerTarget)
                                {
                                    Vector2 distance = Main.player[npc.target].Center - npc.Center;
                                    distance.Normalize();
                                    distance *= 10f;
                                    for (int i = 0; i < 12; i++)
                                        Projectile.NewProjectile(npc.Center, distance.RotatedBy(2 * Math.PI / 12 * i),
                                            mod.ProjectileType("DarkStar"), npc.damage / 5, 0f, Main.myPlayer);
                                }
                            }

                            //dust code
                            if (Main.rand.Next(4) < 3)
                            {
                                int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, 90, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default(Color), 3.5f);
                                Main.dust[dust].noGravity = true;
                                Main.dust[dust].velocity *= 1.8f;
                                Main.dust[dust].velocity.Y -= 0.5f;
                                if (Main.rand.Next(4) == 0)
                                {
                                    Main.dust[dust].noGravity = false;
                                    Main.dust[dust].scale *= 0.5f;
                                }
                            }
                            SharkCount = 253;

                            //become vulnerable again when both twins at 1hp
                            if (npc.dontTakeDamage && (!BossIsAlive(ref spazBoss, NPCID.Spazmatism) || Main.npc[spazBoss].life == 1))
                                npc.dontTakeDamage = false;

                            //2*pi * (# of full circles) / (seconds to finish rotation) / (ticks per sec)
                            const float rotationInterval = 2f * (float)Math.PI * 1f / 4f / 60f;

                            npc.ai[0]++; //base value is 4
                            switch (Counter) //laser code idfk
                            {
                                case 0:
                                    if (!targetAlive)
                                        npc.ai[0]--; //stop counting up if player is dead
                                    if (npc.ai[0] > 604f)
                                    {
                                        npc.ai[0] = 4f;
                                        if (npc.HasPlayerTarget)
                                        {
                                            Counter++;
                                            npc.ai[3] = -npc.rotation;
                                            if (--npc.ai[2] > 295f)
                                                npc.ai[2] = 295f;
                                            masoBool[2] = (Main.player[npc.target].Center.X - npc.Center.X < 0);

                                            for (int i = 0; i < 72; i++) //warning dust ring
                                            {
                                                Vector2 vector6 = Vector2.UnitY * 60f;
                                                vector6 = vector6.RotatedBy((i - (72 / 2 - 1)) * 6.28318548f / 72) + npc.Center;
                                                Vector2 vector7 = vector6 - npc.Center;
                                                int d = Dust.NewDust(vector6 + vector7, 0, 0, 90, 0f, 0f, 0, default(Color), 3f);
                                                Main.dust[d].noGravity = true;
                                                Main.dust[d].velocity = vector7;
                                            }

                                            Main.PlaySound(36, (int)npc.Center.X, (int)npc.Center.Y, -1, 1f, 0f); //eoc roar
                                        }
                                        npc.netUpdate = true;
                                        if (Main.netMode == 2) //synchronize counter with clients
                                        {
                                            var netMessage = mod.GetPacket();
                                            netMessage.Write((byte)5);
                                            netMessage.Write((byte)npc.whoAmI);
                                            netMessage.Write(masoBool[2]);
                                            netMessage.Write(Counter);
                                            netMessage.Send();
                                        }
                                    }
                                    break;

                                case 1: //slowing down, beginning rotation
                                    npc.velocity *= 1f - (npc.ai[0] - 4f) / 120f;
                                    npc.localAI[1] = 0f;
                                    //if (--npc.ai[2] > 295f) npc.ai[2] = 295f;
                                    npc.ai[3] -= (npc.ai[0] - 4f) / 120f * rotationInterval * (masoBool[2] ? 1f : -1f);
                                    npc.rotation = -npc.ai[3];

                                    if (npc.ai[0] >= 124f) //FIRE LASER
                                    {
                                        if (Main.netMode != 1)
                                        {
                                            Vector2 speed = Vector2.UnitX.RotatedBy(npc.rotation);
                                            Projectile.NewProjectile(npc.Center, speed, mod.ProjectileType("PhantasmalDeathray"), npc.damage / 2, 0f, Main.myPlayer, 0f, npc.whoAmI);
                                        }
                                        Counter++;
                                        npc.ai[0] = 4f;
                                        npc.netUpdate = true;
                                        if (Main.netMode == 2) //synchronize counter with clients
                                        {
                                            var netMessage = mod.GetPacket();
                                            netMessage.Write((byte)5);
                                            netMessage.Write((byte)npc.whoAmI);
                                            netMessage.Write(masoBool[2]);
                                            netMessage.Write(Counter);
                                            netMessage.Send();
                                        }
                                    }
                                    return false;

                                case 2: //spinning full speed
                                    npc.velocity = Vector2.Zero;
                                    npc.localAI[1] = 0f;
                                    //if (--npc.ai[2] > 295f) npc.ai[2] = 295f;
                                    npc.ai[3] -= rotationInterval * (masoBool[2] ? 1f : -1f);
                                    npc.rotation = -npc.ai[3];

                                    if (npc.ai[0] >= 244f)
                                    {
                                        Counter++;
                                        npc.ai[0] = 4f;
                                        npc.netUpdate = true;
                                        if (Main.netMode == 2) //synchronize counter with clients
                                        {
                                            var netMessage = mod.GetPacket();
                                            netMessage.Write((byte)5);
                                            netMessage.Write((byte)npc.whoAmI);
                                            netMessage.Write(masoBool[2]);
                                            netMessage.Write(Counter);
                                            netMessage.Send();
                                        }
                                    }
                                    return false;

                                case 3: //laser done, slowing down spin, moving again
                                    npc.velocity *= (npc.ai[0] - 4f) / 60f;
                                    npc.localAI[1] = 0f;
                                    //if (--npc.ai[2] > 295f) npc.ai[2] = 295f;
                                    npc.ai[3] -= (1f - (npc.ai[0] - 4f) / 60f) * rotationInterval * (masoBool[2] ? 1f : -1f);
                                    npc.rotation = -npc.ai[3];

                                    if (npc.ai[0] >= 64f)
                                    {
                                        Counter = 0;
                                        npc.ai[0] = 4f;
                                        npc.netUpdate = true;
                                        if (Main.netMode == 2) //synchronize counter with clients
                                        {
                                            var netMessage = mod.GetPacket();
                                            netMessage.Write((byte)5);
                                            netMessage.Write((byte)npc.whoAmI);
                                            netMessage.Write(masoBool[2]);
                                            netMessage.Write(Counter);
                                            netMessage.Send();
                                        }
                                    }
                                    return false;

                                default:
                                    Counter = 0;
                                    npc.ai[0] = 4f;
                                    npc.netUpdate = true;
                                    if (Main.netMode == 2) //synchronize counter with clients
                                    {
                                        var netMessage = mod.GetPacket();
                                        netMessage.Write((byte)5);
                                        netMessage.Write((byte)npc.whoAmI);
                                        netMessage.Write(masoBool[2]);
                                        netMessage.Write(Counter);
                                        netMessage.Send();
                                    }
                                    break;
                            }

                            //npc.position += npc.velocity / 4f;

                            //if (Counter == 600 && Main.netMode != 1 && npc.HasPlayerTarget)
                            //{
                            //    Vector2 vector200 = Main.player[npc.target].Center - npc.Center;
                            //    vector200.Normalize();
                            //    float num1225 = -1f;
                            //    if (vector200.X < 0f)
                            //    {
                            //        num1225 = 1f;
                            //    }
                            //    vector200 = vector200.RotatedBy(-num1225 * 1.04719755f, default(Vector2));
                            //    Projectile.NewProjectile(npc.Center, vector200, mod.ProjectileType("PhantasmalDeathray"), npc.damage / 2, 0f, Main.myPlayer, num1225 * 0.0104719755f, npc.whoAmI);
                            //    npc.netUpdate = true;
                            //}
                        }

                        //become vulnerable again when both twins at 1hp
                        if (npc.dontTakeDamage && (!BossIsAlive(ref spazBoss, NPCID.Spazmatism) || Main.npc[spazBoss].life == 1))
                            npc.dontTakeDamage = false;

                        /*if (!BossIsAlive(ref spazBoss, NPCID.Spazmatism) && targetAlive)
                        {
                            Timer--;

                            if (Timer <= 0)
                            {
                                Timer = 600;
                                if (Main.netMode != 1)
                                {
                                    int spawn = NPC.NewNPC((int)npc.position.X + Main.rand.Next(-1000, 1000), (int)npc.position.Y + Main.rand.Next(-400, -100), NPCID.Spazmatism);
                                    if (spawn != 200)
                                    {
                                        Main.npc[spawn].life = Main.npc[spawn].lifeMax / 4;
                                        if (Main.netMode == 2)
                                        {
                                            NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("Spazmatism has been revived!"), new Color(175, 75, 255));
                                            NetMessage.SendData(23, -1, -1, null, spawn);
                                        }
                                        else
                                        {
                                            Main.NewText("Spazmatism has been revived!", 175, 75, 255);
                                        }
                                    }
                                }
                            }
                        }*/
                        break;

                    case NPCID.Spazmatism:
                        spazBoss = npc.whoAmI;
                        bool retiAlive = BossIsAlive(ref retiBoss, NPCID.Retinazer);

                        if (!masoBool[0]) //spawn in phase 2
                        {
                            masoBool[0] = true;
                            npc.ai[0] = 1f;
                            npc.ai[1] = 0.0f;
                            npc.ai[2] = 0.0f;
                            npc.ai[3] = 0.0f;
                            npc.netUpdate = true;
                        }

                        if (npc.ai[0] < 4f)
                        {
                            if (npc.life <= npc.lifeMax / 2) //going to phase 3
                            {
                                npc.ai[0] = 4f;
                                npc.netUpdate = true;
                                Main.PlaySound(15, (int)npc.Center.X, (int)npc.Center.Y, 0);

                                int index = npc.FindBuffIndex(BuffID.CursedInferno);
                                if (index != -1)
                                    npc.DelBuff(index); //remove cursed inferno debuff if i have it

                                npc.buffImmune[BuffID.CursedInferno] = true;
                                npc.buffImmune[BuffID.OnFire] = true;
                                npc.buffImmune[BuffID.ShadowFlame] = true;
                                npc.buffImmune[BuffID.Frostburn] = true;
                            }
                        }
                        else
                        {
                            npc.position += npc.velocity / 4f;

                            if (npc.ai[1] == 0f) //not dashing
                            {
                                if (retiAlive && (Main.npc[retiBoss].ai[0] < 4f || Main.npc[retiBoss].GetGlobalNPC<FargoSoulsGlobalNPC>().Counter == 0)) //reti is in normal AI
                                {
                                    npc.ai[1] = 1; //switch to dashing
                                    npc.ai[2] = 0;
                                    npc.ai[3] = 0;
                                    npc.netUpdate = true;
                                }

                                if (++Counter > 40)
                                {
                                    Counter = 0;
                                    if (Main.netMode != 1 && npc.HasPlayerTarget) //vanilla spaz p1 shoot fireball code
                                    {
                                        Vector2 Speed = Main.player[npc.target].Center - npc.Center;
                                        Speed.Normalize();
                                        int Damage;
                                        if (Main.expertMode)
                                        {
                                            Speed *= 14f;
                                            Damage = 22;
                                        }
                                        else
                                        {
                                            Speed *= 12f;
                                            Damage = 25;
                                        }
                                        Damage = (int)(Damage * (1 + FargoSoulsWorld.TwinsCount * .0125));
                                        Projectile.NewProjectile(npc.Center + Speed * 4f, Speed, ProjectileID.CursedFlameHostile, Damage, 0f, Main.myPlayer);
                                    }
                                }
                            }
                            else //dashing
                            {
                                if (retiAlive && Main.npc[retiBoss].ai[0] >= 4f && Main.npc[retiBoss].GetGlobalNPC<FargoSoulsGlobalNPC>().Counter != 0) //reti is doing the spin
                                {
                                    npc.ai[1] = 0; //switch to not dashing
                                    npc.netUpdate = true;
                                }
                                if (npc.HasValidTarget && ++Counter > 3) //cursed flamethrower when dashing
                                {
                                    Counter = 0;
                                    Projectile.NewProjectile(npc.Center, npc.velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-6f, 6f))) * 0.5f, ProjectileID.EyeFire, npc.damage / 4, 0f, Main.myPlayer);
                                }
                            }

                            if (masoBool[3] && --Counter2 < 0) //when brought to 1hp, begin shooting dark stars
                            {
                                Counter2 = 120;
                                if (Main.netMode != 1 && npc.HasPlayerTarget)
                                {
                                    Vector2 distance = Main.player[npc.target].Center - npc.Center;
                                    distance.Normalize();
                                    distance *= 14f;
                                    for (int i = 0; i < 8; i++)
                                        Projectile.NewProjectile(npc.Center, distance.RotatedBy(2 * Math.PI / 8 * i),
                                            mod.ProjectileType("DarkStar"), npc.damage / 5, 0f, Main.myPlayer);
                                }
                            }

                            //dust code
                            if (Main.rand.Next(4) < 3)
                            {
                                int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, 89, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default(Color), 3.5f);
                                Main.dust[dust].noGravity = true;
                                Main.dust[dust].velocity *= 1.8f;
                                Main.dust[dust].velocity.Y -= 0.5f;
                                if (Main.rand.Next(4) == 0)
                                {
                                    Main.dust[dust].noGravity = false;
                                    Main.dust[dust].scale *= 0.5f;
                                }
                            }
                            SharkCount = 254;
                        }

                        //become vulnerable again when both twins at 1hp
                        if (npc.dontTakeDamage && (!BossIsAlive(ref retiBoss, NPCID.Retinazer) || Main.npc[retiBoss].life == 1))
                            npc.dontTakeDamage = false;

                        /*if (!retiAlive && npc.HasPlayerTarget && Main.player[npc.target].active)
                        {
                            Timer--;

                            if (Timer <= 0)
                            {
                                Timer = 600;
                                if (Main.netMode != 1)
                                {
                                    int spawn = NPC.NewNPC((int)npc.position.X + Main.rand.Next(-1000, 1000), (int)npc.position.Y + Main.rand.Next(-400, -100), NPCID.Retinazer);
                                    if (spawn != 200)
                                    {
                                        Main.npc[spawn].life = Main.npc[spawn].lifeMax / 4;
                                        if (Main.netMode == 2)
                                        {
                                            NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("Retinazer has been revived!"), new Color(175, 75, 255));
                                            NetMessage.SendData(23, -1, -1, null, spawn);
                                        }
                                        else
                                        {
                                            Main.NewText("Retinazer has been revived!", 175, 75, 255);
                                        }
                                    }
                                }
                            }
                        }*/
                        break;

                    case NPCID.LunarTowerNebula:
                        Aura(npc, 5000, mod.BuffType("Atrophied"), false, 58);
                        Aura(npc, 5000, mod.BuffType("Jammed"));
                        Aura(npc, 5000, mod.BuffType("Antisocial"));
                        if (!masoBool[0])
                        {
                            masoBool[0] = true;
                            npc.damage += 100;
                            npc.defDamage += 100;
                            npc.netUpdate = true;
                            npc.buffImmune[mod.BuffType("ClippedWings")] = true;
                        }
                        if (npc.dontTakeDamage)
                        {
                            npc.life = npc.lifeMax;
                        }
                        else
                        {
                            if (++Counter > 180)
                            {
                                Counter = 0;
                                npc.TargetClosest(false);
                                for (int i = 0; i < 40; ++i)
                                {
                                    int d = Dust.NewDust(npc.position, npc.width, npc.height, 242, 0.0f, 0.0f, 0, new Color(), 1f);
                                    Dust dust = Main.dust[d];
                                    dust.velocity *= 4f;
                                    Main.dust[d].noGravity = true;
                                    Main.dust[d].scale += 1.5f;
                                }
                                if (npc.HasPlayerTarget && Main.netMode != 1 && npc.Distance(Main.player[npc.target].Center) < 5000)
                                {
                                    int x = (int)Main.player[npc.target].Center.X / 16;
                                    int y = (int)Main.player[npc.target].Center.Y / 16;
                                    for (int i = 0; i < 100; i++)
                                    {
                                        int newX = x + Main.rand.Next(10, 31) * (Main.rand.Next(2) == 0 ? 1 : -1);
                                        int newY = y + Main.rand.Next(-15, 16);
                                        Vector2 newPos = new Vector2(newX * 16, newY * 16);
                                        if (!Collision.SolidCollision(newPos, npc.width, npc.height))
                                        {
                                            npc.Center = newPos;
                                            break;
                                        }
                                    }
                                }
                                for (int i = 0; i < 40; ++i)
                                {
                                    int d = Dust.NewDust(npc.position, npc.width, npc.height, 242, 0.0f, 0.0f, 0, new Color(), 1f);
                                    Dust dust = Main.dust[d];
                                    dust.velocity *= 4f;
                                    Main.dust[d].noGravity = true;
                                    Main.dust[d].scale += 1.5f;
                                }
                                Main.PlaySound(SoundID.Item8, npc.Center);
                                npc.netUpdate = true;
                            }

                            if (++Timer > 60)
                            {
                                Timer = 0;
                                npc.TargetClosest(false);
                                if (npc.HasPlayerTarget && Main.netMode != 1 && npc.Distance(Main.player[npc.target].Center) < 5000)
                                {
                                    for (int i = 0; i < 3; i++)
                                    {
                                        Vector2 position = Main.player[npc.target].Center;
                                        position.X += Main.rand.Next(-150, 151);
                                        position.Y -= Main.rand.Next(600, 801);
                                        Vector2 speed = Main.player[npc.target].Center - position;
                                        speed.Normalize();
                                        speed *= 10f;
                                        Projectile.NewProjectile(position, speed, ProjectileID.NebulaLaser, 40, 0f, Main.myPlayer);
                                    }
                                }
                            }

                            masoBool[1] = true;
                        }
                        break;

                    case NPCID.LunarTowerSolar:
                        Aura(npc, 5000, mod.BuffType("ReverseManaFlow"), false, DustID.SolarFlare);
                        Aura(npc, 5000, mod.BuffType("Jammed"));
                        Aura(npc, 5000, mod.BuffType("Antisocial"));
                        if (!masoBool[0])
                        {
                            masoBool[0] = true;
                            npc.damage += 200;
                            npc.defDamage += 200;
                            npc.netUpdate = true;
                            npc.buffImmune[mod.BuffType("ClippedWings")] = true;
                        }
                        if (npc.dontTakeDamage)
                        {
                            npc.life = npc.lifeMax;
                        }
                        else
                        {
                            if (++Timer > 240)
                            {
                                Timer = 0;
                                npc.TargetClosest(false);
                                if (npc.HasPlayerTarget && Main.netMode != 1)
                                {
                                    const float rotate = (float)Math.PI / 4f;
                                    Vector2 speed = Main.player[npc.target].Center - npc.Center;
                                    speed.Normalize();
                                    speed *= 5f;
                                    for (int i = -2; i <= 2; i++)
                                        Projectile.NewProjectile(npc.Center, speed.RotatedBy(i * rotate), ProjectileID.CultistBossFireBall, 40, 0f, Main.myPlayer);
                                }
                            }
                            masoBool[1] = true;
                        }
                        break;

                    case NPCID.LunarTowerStardust:
                        if (!masoBool[0])
                        {
                            masoBool[0] = true;
                            npc.damage += 100;
                            npc.defDamage += 100;
                            npc.netUpdate = true;
                            npc.buffImmune[mod.BuffType("ClippedWings")] = true;
                        }
                        Aura(npc, 5000, mod.BuffType("Atrophied"), false, 20);
                        Aura(npc, 5000, mod.BuffType("Jammed"));
                        Aura(npc, 5000, mod.BuffType("ReverseManaFlow"));
                        if (npc.dontTakeDamage)
                        {
                            npc.life = npc.lifeMax;
                        }
                        else
                        {
                            if (++Timer > 420)
                            {
                                Timer = 0;
                                npc.TargetClosest(false);
                                if (npc.HasPlayerTarget && Main.netMode != 1)
                                {
                                    const float rotate = (float)Math.PI / 12f;
                                    Vector2 speed = Main.player[npc.target].Center - npc.Center;
                                    speed.Normalize();
                                    speed *= 8f;
                                    for (int i = 0; i < 24; i++)
                                    {
                                        Vector2 vel = speed.RotatedBy(rotate * i);
                                        int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.AncientLight, 0,
                                            0f, (Main.rand.NextFloat() - 0.5f) * 0.3f * 6.28318548202515f / 60f, vel.X, vel.Y);
                                        if (n < 200)
                                        {
                                            Main.npc[n].velocity = vel;
                                            Main.npc[n].netUpdate = true;
                                            if (Main.netMode == 2)
                                                NetMessage.SendData(23, -1, -1, null, n);
                                        }
                                    }
                                }
                            }
                            masoBool[1] = true;
                        }
                        break;

                    case NPCID.LunarTowerVortex:
                        if (!masoBool[0])
                        {
                            masoBool[0] = true;
                            npc.damage += 100;
                            npc.defDamage += 100;
                            npc.netUpdate = true;
                            npc.buffImmune[mod.BuffType("ClippedWings")] = true;
                        }
                        Aura(npc, 5000, mod.BuffType("Atrophied"), false, DustID.Vortex);
                        Aura(npc, 5000, mod.BuffType("ReverseManaFlow"));
                        Aura(npc, 5000, mod.BuffType("Antisocial"));
                        if (npc.dontTakeDamage)
                        {
                            npc.life = npc.lifeMax;
                            if (++Counter2 > 180)
                            {
                                Counter2 = 0;
                                npc.netUpdate = true;
                            }
                        }
                        else
                        {
                            if (++Counter > 360) //triggers "shield going down" animation
                            {
                                Counter = 0;
                                npc.ai[3] = 1f;
                                npc.netUpdate = true;
                            }

                            npc.reflectingProjectiles = npc.ai[3] != 0f;
                            if (npc.reflectingProjectiles) //dust
                            {
                                for (int i = 0; i < 20; i++)
                                {
                                    Vector2 offset = new Vector2();
                                    double angle = Main.rand.NextDouble() * 2d * Math.PI;
                                    offset.X += (float)(Math.Sin(angle) * npc.height / 2);
                                    offset.Y += (float)(Math.Cos(angle) * npc.height / 2);
                                    Dust dust = Main.dust[Dust.NewDust(
                                        npc.Center + offset - new Vector2(4, 4), 0, 0,
                                        DustID.Vortex, 0, 0, 100, Color.White, 1f
                                        )];
                                    dust.noGravity = true;
                                }
                            }

                            if (++Timer > 240)
                            {
                                Timer = 0;
                                npc.TargetClosest(false);
                                if (npc.HasPlayerTarget && Main.netMode != 1)
                                {
                                    Vector2 speed = Main.player[npc.target].Center + Main.player[npc.target].velocity * 15f - npc.Center;
                                    speed.Normalize();
                                    speed *= 4f;
                                    Projectile.NewProjectile(npc.Center, speed, ProjectileID.CultistBossLightningOrb, 30, 0f, Main.myPlayer);
                                }
                            }
                            masoBool[1] = true;
                        }
                        break;

                    case NPCID.CultistBoss:
                        cultBoss = npc.whoAmI;

                        if (!masoBool[0])
                        {
                            masoBool[0] = true;
                            if (Main.netMode != 1 && !NPC.downedAncientCultist)
                                Item.NewItem(npc.Hitbox, mod.ItemType("LunaticSigil"));
                        }

                        Timer++;
                        if (Timer >= 1200)
                        {
                            Timer = 0;

                            if (NPC.CountNPCS(NPCID.AncientCultistSquidhead) < 4 && Main.netMode != 1)
                            {
                                int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.AncientCultistSquidhead, 0, 0f, 0f, 0f, 0f, npc.target);
                                if (n != 200 && Main.netMode == 2)
                                    NetMessage.SendData(23, -1, -1, null, n);
                            }
                        }

                        if (npc.ai[3] == -1f)
                        {
                            if (npc.ai[1] >= 120f && npc.ai[1] < 419f) //skip summoning ritual LMAO
                            {
                                npc.ai[1] = 419f;
                                npc.netUpdate = true;
                            }
                        }
                        else
                        {
                            int damage = (int)(75 * (1 + FargoSoulsWorld.CultistCount * .0125)); //necessary because calameme
                            switch ((int)npc.ai[0])
                            {
                                case -1:
                                    if (npc.ai[1] == 419f)
                                    {
                                        npc.ai[0] = 0f;
                                        npc.ai[1] = 0f;
                                        npc.ai[3] = 11f;
                                        npc.netUpdate = true;
                                    }
                                    break;

                                case 2:
                                    if (npc.ai[1] == 3f && Main.netMode != 1) //ice mist, frost wave support
                                    {
                                        int t = npc.HasPlayerTarget ? npc.target : npc.FindClosestPlayer();
                                        if (t != -1 && Main.player[t].active)
                                        {
                                            for (int i = 0; i < 200; i++)
                                            {
                                                if (Main.npc[i].active && Main.npc[i].type == NPCID.CultistBossClone)
                                                {
                                                    Vector2 distance = Main.player[t].Center - Main.npc[i].Center;
                                                    distance.Normalize();
                                                    distance = distance.RotatedByRandom(Math.PI / 12);
                                                    distance *= Main.rand.NextFloat(6f, 9f);
                                                    Projectile.NewProjectile(Main.npc[i].Center, distance,
                                                        ProjectileID.FrostWave, damage / 3, 0f, Main.myPlayer);
                                                }
                                            }
                                        }
                                    }
                                    break;

                                case 3:
                                    if (npc.ai[1] == 3f && Main.netMode != 1) //fireballs, solar goop support
                                    {
                                        for (int i = 0; i < 200; i++)
                                        {
                                            if (Main.npc[i].active && Main.npc[i].type == NPCID.CultistBossClone)
                                            {
                                                int n = NPC.NewNPC((int)Main.npc[i].Center.X, (int)Main.npc[i].Center.Y, NPCID.SolarGoop);
                                                if (n < 200)
                                                {
                                                    Main.npc[n].velocity.X = Main.rand.Next(-10, 11);
                                                    Main.npc[n].velocity.Y = Main.rand.Next(-15, -4);
                                                    if (Main.netMode == 2)
                                                        NetMessage.SendData(23, -1, -1, null, n);
                                                }
                                            }
                                        }
                                    }
                                    break;

                                case 4:
                                    if (npc.ai[1] == 19f && npc.HasPlayerTarget && Main.netMode != 1) //lightning orb, lightning support
                                    {
                                        for (int i = 0; i < 200; i++)
                                        {
                                            if (Main.npc[i].active && Main.npc[i].type == NPCID.CultistBossClone)
                                            {
                                                Vector2 dir = Main.player[npc.target].Center - Main.npc[i].Center;
                                                float ai1New = Main.rand.Next(100);
                                                Vector2 vel = Vector2.Normalize(dir.RotatedByRandom(Math.PI / 4)) * 7f;
                                                Projectile.NewProjectile(Main.npc[i].Center, vel, ProjectileID.CultistBossLightningOrbArc,
                                                    damage / 15 * 6, 0, Main.myPlayer, dir.ToRotation(), ai1New);
                                            }
                                        }
                                    }
                                    break;

                                case 7:
                                    if (npc.ai[1] == 3f && Main.netMode != 1) //ancient light, phantasmal eye support
                                    {
                                        for (int i = 0; i < 200; i++)
                                        {
                                            if (Main.npc[i].active && Main.npc[i].type == NPCID.CultistBossClone)
                                            {
                                                Projectile.NewProjectile(Main.npc[i].Center, Main.npc[i].DirectionTo(Main.player[npc.target].Center) * 10f, ProjectileID.StardustJellyfishSmall, damage / 3, 0f, Main.myPlayer);
                                                /*Vector2 speed = Vector2.UnitX.RotatedByRandom(Math.PI);
                                                speed *= 6f;
                                                Projectile.NewProjectile(Main.npc[i].Center, speed,
                                                    ProjectileID.PhantasmalEye, damage / 3, 0f, Main.myPlayer);
                                                Projectile.NewProjectile(Main.npc[i].Center, speed.RotatedBy(Math.PI * 2 / 3),
                                                    ProjectileID.PhantasmalEye, damage / 3, 0f, Main.myPlayer);
                                                Projectile.NewProjectile(Main.npc[i].Center, speed.RotatedBy(-Math.PI * 2 / 3),
                                                    ProjectileID.PhantasmalEye, damage / 3, 0f, Main.myPlayer);*/
                                            }
                                        }
                                    }
                                    break;

                                case 8:
                                    if (npc.ai[1] == 3f) //ancient doom, nebula sphere support
                                    {
                                        int t = npc.HasPlayerTarget ? npc.target : npc.FindClosestPlayer();
                                        if (t != -1 && Main.player[t].active)
                                        {
                                            for (int i = 0; i < 200; i++)
                                            {
                                                if (Main.npc[i].active && Main.npc[i].type == NPCID.CultistBossClone)
                                                    Projectile.NewProjectile(Main.npc[i].Center, Vector2.Zero,
                                                        ProjectileID.NebulaSphere, damage / 15 * 6, 0f, Main.myPlayer);
                                            }
                                        }
                                    }
                                    break;

                                default:
                                    break;
                            }
                        }
                        break;

                    case NPCID.AncientDoom:
                        if (npc.localAI[3] == 0f)
                        {
                            npc.localAI[3] = 1f;
                            if (Main.netMode != 1)
                            {
                                Vector2 pivot = npc.Center + new Vector2(250f, 0f).RotatedByRandom(2 * Math.PI);
                                npc.ai[2] = pivot.X;
                                npc.ai[3] = pivot.Y;
                                npc.netUpdate = true;
                            }
                        }
                        if (npc.ai[2] > 0f && npc.ai[3] > 0f)
                        {
                            Vector2 pivot = new Vector2(npc.ai[2], npc.ai[3]);
                            npc.velocity = Vector2.Normalize(pivot - npc.Center).RotatedBy(Math.PI / 2) * 6f;
                        }
                        npc.damage = npc.alpha < 100 ? npc.defDamage : 0;
                        break;

                    case NPCID.KingSlime:
                        slimeBoss = npc.whoAmI;
                        npc.color = Main.DiscoColor * 0.3f;
                        if (masoBool[1])
                        {
                            if (npc.velocity.Y == 0f) //start attack
                            {
                                masoBool[1] = false;
                                if (Main.netMode != 1)
                                {
                                    for (int i = 0; i < 30; i++)
                                    {
                                        Projectile.NewProjectile(new Vector2(npc.Center.X + Main.rand.Next(-5, 5), npc.Center.Y - 15),
                                            new Vector2(Main.rand.NextFloat(-6, 6), Main.rand.NextFloat(-8, -5)),
                                            ProjectileID.SpikedSlimeSpike, npc.damage / 5, 0f, Main.myPlayer);
                                    }
                                }
                            }
                        }
                        else if (npc.velocity.Y > 0)
                        {
                            masoBool[1] = true;
                        }

                        if (npc.life < npc.lifeMax * .5f && npc.HasPlayerTarget)
                        {
                            Player p = Main.player[npc.target];

                            Counter++;
                            if (Counter >= 90) //slime rain
                            {
                                Counter = 0;
                                Main.PlaySound(SoundID.Item21, p.Center);
                                if (Main.netMode != 1)
                                {
                                    for (int i = 0; i < 5; i++)
                                    {
                                        Vector2 spawn = p.Center;
                                        spawn.X += Main.rand.Next(-200, 201);
                                        spawn.Y -= Main.rand.Next(600, 901);
                                        Vector2 speed = p.Center - spawn;
                                        speed.Normalize();
                                        speed *= 5f;
                                        speed = speed.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-5, 5)));
                                        Projectile.NewProjectile(spawn, speed, mod.ProjectileType("SlimeBallHostile"), npc.damage / 5, 0f, Main.myPlayer);
                                    }
                                }
                            }

                            if (++Timer > 300)
                            {
                                Timer = 0;
                                const float gravity = 0.15f;
                                const float time = 120f;
                                Vector2 distance = Main.player[npc.target].Center - npc.Center + Main.player[npc.target].velocity * 30f;
                                distance.X = distance.X / time;
                                distance.Y = distance.Y / time - 0.5f * gravity * time;
                                for (int i = 0; i < 10; i++)
                                {
                                    Projectile.NewProjectile(npc.Center, distance + Main.rand.NextVector2Square(-0.5f, 0.5f),
                                        mod.ProjectileType("SlimeSpike"), npc.damage / 5, 0f, Main.myPlayer);
                                }
                            }
                        }

                        if (!masoBool[0])
                        {
                            if (npc.HasPlayerTarget)
                            {
                                Player player = Main.player[npc.target];
                                if (player.active && !player.dead && player.Center.Y < npc.position.Y && npc.Distance(player.Center) < 1000f)
                                {
                                    Counter2++; //timer runs if player is above me and nearby
                                    if (Counter2 >= 600 && Main.netMode != 1) //go berserk
                                    {
                                        masoBool[0] = true;
                                        npc.netUpdate = true;
                                        NetUpdateMaso(npc.whoAmI);
                                        if (Main.netMode == 2)
                                            NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("King Slime has enraged!"), new Color(175, 75, 255));
                                        else
                                            Main.NewText("King Slime has enraged!", 175, 75, 255);
                                    }
                                }
                                else
                                {
                                    Counter2 = 0;
                                }
                            }
                        }
                        else //is berserk
                        {
                            if (!masoBool[2])
                            {
                                masoBool[2] = true;
                                Main.PlaySound(15, npc.Center, 0);
                            }

                            SharkCount = 1;
                            npc.damage = npc.defDamage * 5;
                            npc.defense = npc.defDefense * 2;

                            if (npc.HasPlayerTarget)
                            {
                                Player p = Main.player[npc.target];

                                Counter2++;
                                if (Counter2 >= 4) //spray random slime spikes
                                {
                                    Counter2 = 0;
                                    if (Main.netMode != 1)
                                    {
                                        Vector2 speed = p.Center - npc.Center;
                                        speed.Normalize();
                                        speed *= 16f + Main.rand.Next(-50, 51) * 0.04f;
                                        if (speed.X < 0)
                                            speed = speed.RotatedBy(MathHelper.ToRadians(Main.rand.Next(-15, 31)));
                                        else
                                            speed = speed.RotatedBy(MathHelper.ToRadians(Main.rand.Next(-30, 16)));
                                        Projectile.NewProjectile(npc.position.X + Main.rand.Next(npc.width), npc.position.Y + Main.rand.Next(npc.height), speed.X, speed.Y, ProjectileID.SpikedSlimeSpike, npc.damage / 4, 0f, Main.myPlayer);
                                    }
                                }

                                if (p.active && !p.dead)
                                {
                                    npc.noTileCollide = false;
                                    p.pulley = false;
                                    p.controlHook = false;
                                    if (p.mount.Active)
                                        p.mount.Dismount(p);

                                    p.AddBuff(BuffID.Slimed, 2);
                                    p.AddBuff(mod.BuffType("Crippled"), 2);
                                    p.AddBuff(mod.BuffType("ClippedWings"), 2);
                                }
                                else
                                {
                                    npc.noTileCollide = true;
                                }
                            }
                        }
                        break;

                    case NPCID.EaterofWorldsHead:
                        eaterBoss = npc.whoAmI;
                        boss = npc.whoAmI;
                        Counter++;
                        if (Counter >= 6) //cursed flamethrower, roughly same direction as head
                        {
                            Counter = 0;
                            if (Main.netMode != 1)
                            {
                                Vector2 velocity = new Vector2(5f, 0f).RotatedBy(npc.rotation - Math.PI / 2.0 + MathHelper.ToRadians(Main.rand.Next(-15, 16)));
                                Projectile.NewProjectile(npc.Center, velocity, ProjectileID.EyeFire, npc.damage / 6, 0f, Main.myPlayer);
                            }
                        }
                        break;

                    case NPCID.QueenBee:
                        beeBoss = npc.whoAmI;
                        
                        if (!masoBool[0] && npc.life < npc.lifeMax / 3 * 2 && npc.HasPlayerTarget)
                        {
                            masoBool[0] = true;

                            Vector2 vector72 = new Vector2(npc.position.X + (float)(npc.width / 2) + (float)(Main.rand.Next(20) * npc.direction), npc.position.Y + (float)npc.height * 0.8f);

                            int num594 = NPC.NewNPC((int)vector72.X, (int)vector72.Y, mod.NPCType("RoyalSubject"), 0, 0f, 0f, 0f, 0f, 255);
                            Main.npc[num594].velocity.X = (float)Main.rand.Next(-200, 201) * 0.002f;
                            Main.npc[num594].velocity.Y = (float)Main.rand.Next(-200, 201) * 0.002f;
                            Main.npc[num594].localAI[0] = 60f;
                            Main.npc[num594].netUpdate = true;

                            if (Main.netMode == 0)
                                Main.NewText("Royal Subject has awoken!", 175, 75, 255);
                            else if (Main.netMode == 2)
                                NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("Royal Subject has awoken!"), new Color(175, 75, 255));
                        }

                        if (!masoBool[1] && npc.life < npc.lifeMax / 3 && npc.HasPlayerTarget)
                        {
                            masoBool[1] = true;
                            
                            Vector2 vector72 = new Vector2(npc.position.X + (float)(npc.width / 2) + (float)(Main.rand.Next(20) * npc.direction), npc.position.Y + (float)npc.height * 0.8f);

                            int num594 = NPC.NewNPC((int)vector72.X, (int)vector72.Y, mod.NPCType("RoyalSubject"), 0, 0f, 0f, 0f, 0f, 255);
                            Main.npc[num594].velocity.X = (float)Main.rand.Next(-200, 201) * 0.1f;
                            Main.npc[num594].velocity.Y = (float)Main.rand.Next(-200, 201) * 0.1f;
                            Main.npc[num594].localAI[0] = 60f;
                            Main.npc[num594].netUpdate = true;

                            if (Main.netMode == 0)
                                Main.NewText("Royal Subject has awoken!", 175, 75, 255);
                            else if (Main.netMode == 2)
                                NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("Royal Subject has awoken!"), new Color(175, 75, 255));

                            NPC.SpawnOnPlayer(npc.target, mod.NPCType("RoyalSubject")); //so that both dont stack for being spawned from qb
                        }

                        if (!masoBool[2] && npc.life < npc.lifeMax / 2) //enable new attack and roar below 50%
                        {
                            masoBool[2] = true;
                            Main.PlaySound(15, npc.Center, 0);
                        }

                        if (NPC.AnyNPCs(mod.NPCType("RoyalSubject")))
                        {
                            npc.ai[0] = 3; //always shoot stingers mode
                            RegenTimer = 480;
                        }

                        //only while stationary mode
                        if (npc.ai[0] == 3f || npc.ai[0] == 1f)
                        {
                            if (masoBool[2] && ++Timer > 600)
                            {
                                if (Timer < 690) //slow down
                                {
                                    if (!masoBool[3])
                                    {
                                        masoBool[3] = true;
                                        npc.netUpdate = true;
                                        for (int i = 0; i < 36; i++)
                                        {
                                            Vector2 vector6 = Vector2.UnitY * 9f;
                                            vector6 = vector6.RotatedBy((i - (36 / 2 - 1)) * 6.28318548f / 36) + npc.Center;
                                            Vector2 vector7 = vector6 - npc.Center;
                                            int d = Dust.NewDust(vector6 + vector7, 0, 0, 87, 0f, 0f, 0, default(Color), 4f);
                                            Main.dust[d].noGravity = true;
                                            Main.dust[d].velocity = vector7;
                                        }
                                        Main.PlaySound(15, npc.Center, 0);
                                    }

                                    if (Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))
                                    {
                                        npc.velocity *= 0.975f;
                                    }
                                    else
                                    {
                                        Timer--; //stall this section until has line of sight
                                        break;
                                    }
                                }
                                else if (Timer < 840) //spray bees
                                {
                                    if (masoBool[3])
                                    {
                                        masoBool[3] = false;
                                        npc.netUpdate = true;
                                    }
                                    npc.velocity = Vector2.Zero;
                                    if (++Counter > 2)
                                    {
                                        Counter = 0;
                                        if (Main.netMode != 1)
                                        {
                                            Projectile.NewProjectile(npc.Center + Vector2.UnitY * 15, 12f * Vector2.UnitX.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-45, 45))), mod.ProjectileType("Bee"), npc.damage / 5, 0f, Main.myPlayer);
                                            Projectile.NewProjectile(npc.Center + Vector2.UnitY * 15, -12f * Vector2.UnitX.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-45, 45))), mod.ProjectileType("Bee"), npc.damage / 5, 0f, Main.myPlayer);
                                        }
                                    }
                                }
                                else if (Timer > 900) //wait for 1 second then return to normal AI
                                {
                                    Timer = 0;
                                    npc.netUpdate = true;
                                }

                                if (npc.netUpdate)
                                {
                                    if (Main.netMode == 2)
                                    {
                                        NetMessage.SendData(27, -1, -1, null, npc.whoAmI);
                                        NetUpdateMaso(npc.whoAmI);
                                    }
                                    npc.netUpdate = false;
                                }
                                return false;
                            }

                            Counter++;
                            if (Counter >= 90)
                            {
                                Counter = 0;
                                Counter2++;
                                if (Counter2 > 3)
                                {
                                    if (Main.netMode != 1)
                                        FargoGlobalProjectile.XWay(16, npc.Center, ProjectileID.Stinger, 6, 11, 1);
                                    Counter2 = 0;
                                }
                                else
                                {
                                    if (Main.netMode != 1)
                                        FargoGlobalProjectile.XWay(8, npc.Center, ProjectileID.Stinger, 6, 11, 1);
                                }
                            }
                        }
                        break;

                    case NPCID.SkeletronHead:
                        skeleBoss = npc.whoAmI;
                        if (!masoBool[0])
                        {
                            masoBool[0] = true;
                            if (Main.netMode != 1 && !NPC.downedBoss3)
                                Item.NewItem(npc.Hitbox, mod.ItemType("BloodiedSkull"));
                        }
                        if (Counter != 0)
                        {
                            Timer++;

                            if (Timer >= 3600)
                            {
                                Timer = 0;

                                bool otherHandStillAlive = false;
                                for (int i = 0; i < 200; i++) //look for hand that belongs to me
                                {
                                    if (Main.npc[i].active && Main.npc[i].type == NPCID.SkeletronHand && Main.npc[i].ai[1] == npc.whoAmI)
                                    {
                                        otherHandStillAlive = true;
                                        break;
                                    }
                                }

                                if (Main.netMode != 1)
                                {
                                    int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.SkeletronHand, npc.whoAmI, 0f, 0f, 0f, 0f, npc.target);
                                    if (n != 200)
                                    {
                                        Main.npc[n].ai[0] = (Counter == 1) ? 1f : -1f;
                                        Main.npc[n].ai[1] = npc.whoAmI;
                                        Main.npc[n].life = Main.npc[n].lifeMax / 4;
                                        Main.npc[n].netUpdate = true;
                                        if (Main.netMode == 2)
                                            NetMessage.SendData(23, -1, -1, null, n);
                                    }
                                }

                                if (!otherHandStillAlive)
                                {
                                    if (Counter == 1)
                                        Counter = 2;
                                    else
                                        Counter = 1;
                                }
                                else
                                {
                                    Counter = 0;
                                }
                            }
                        }

                        if (npc.ai[1] == 1f || npc.ai[1] == 2f) //spinning or DG mode
                        {
                            npc.localAI[2]++;
                            float ratio = (float)npc.life / npc.lifeMax;
                            float threshold = 20f + 100f * ratio;
                            if (npc.localAI[2] >= threshold) //spray bones
                            {
                                npc.localAI[2] = 0f;
                                if (threshold > 0 && npc.HasPlayerTarget && Main.netMode != 1)
                                {
                                    Vector2 speed = Vector2.Normalize(Main.player[npc.target].Center - npc.Center) * 6f;
                                    for (int i = 0; i < 8; i++)
                                    {
                                        Vector2 vel = speed.RotatedBy(Math.PI * 2 / 8 * i);
                                        vel += npc.velocity * (1f - ratio);
                                        vel.Y -= Math.Abs(vel.X) * 0.2f;
                                        Projectile.NewProjectile(npc.Center, vel, mod.ProjectileType("SkeletronBone"), npc.defDamage / 9 * 2, 0f, Main.myPlayer);
                                    }
                                }
                            }
                        }

                        if (npc.ai[1] == 2f)
                        {
                            npc.defense = 9999;
                            npc.damage = npc.defDamage * 15;
                        }
                        break;

                    case NPCID.SkeletronHand:
                        if (npc.life < npc.lifeMax / 2)
                        {
                            if (--Counter < 0)
                            {
                                Counter = (int)(60f + 120f * npc.life / npc.lifeMax);
                                if (npc.HasPlayerTarget && Main.netMode != 1)
                                {
                                    Vector2 speed = new Vector2(0f, -3f);
                                    for (int i = 0; i < 8; i++)
                                    {
                                        Vector2 vel = speed.RotatedBy(Math.PI * 2 / 8 * i);
                                        vel.Y -= Math.Abs(vel.X) * 0.2f;
                                        Projectile.NewProjectile(npc.Center, vel, mod.ProjectileType("SkeletronBone"), npc.damage / 4, 0f, Main.myPlayer);
                                    }
                                }
                            }
                            if (--Counter2 < 0)
                            {
                                Counter2 = 300;
                                if (npc.HasPlayerTarget && Main.netMode != 1)
                                {
                                    Vector2 speed = Main.player[npc.target].Center - npc.Center;
                                    speed.X += Main.rand.Next(-20, 21);
                                    speed.Y += Main.rand.Next(-20, 21);
                                    speed.Normalize();
                                    speed *= 3f;
                                    Projectile.NewProjectile(npc.Center, speed, ProjectileID.Skull, npc.damage / 4, 0, Main.myPlayer, -1f, 0f);
                                }
                            }
                        }

                        if (Main.npc[(int)npc.ai[1]].ai[1] == 1f || Main.npc[(int)npc.ai[1]].ai[1] == 2f) //spinning or DG mode
                        {
                            if (!masoBool[0])
                            {
                                masoBool[0] = true;
                                if (Main.netMode != 1 && npc.HasPlayerTarget) //throw undead miner
                                {
                                    float gravity = 0.4f; //shoot down
                                    const float time = 60f;
                                    Vector2 distance = Main.player[npc.target].Center - npc.Center;
                                    distance.X = distance.X / time;
                                    distance.Y = distance.Y / time - 0.5f * gravity * time;
                                    int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.BoneThrowingSkeleton);
                                    if (n != 200)
                                    {
                                        Main.npc[n].velocity = distance;
                                        if (Main.netMode == 2)
                                            NetMessage.SendData(23, -1, -1, null, n);
                                    }
                                }
                            }
                        }
                        else
                        {
                            masoBool[0] = false;
                        }
                        break;

                    case NPCID.WallofFlesh:
                        wallBoss = npc.whoAmI;

                        if (npc.ai[3] == 0f) //when spawned in, make one eye invul
                        {
                            for (int i = 0; i < 200; i++)
                            {
                                if (Main.npc[i].active && Main.npc[i].type == NPCID.WallofFleshEye && Main.npc[i].realLife == npc.whoAmI)
                                {
                                    Main.npc[i].ai[2] = -1f;
                                    Main.npc[i].netUpdate = true;
                                    npc.ai[3] = 1f;
                                    npc.netUpdate = true;
                                    break;
                                }
                            }
                        }
                        
                        if (masoBool[0]) //phase 2
                        {
                            if (++Counter > 600)
                            {
                                Counter = 0;
                                Counter2 = 0;
                                masoBool[1] = !masoBool[1];
                                masoBool[2] = false;
                                npc.netUpdate = true;
                            }
                            else if (Counter < 240) //special attacks
                            {
                                if (masoBool[1]) //cursed inferno attack
                                {
                                    if (++Counter2 > 5)
                                    {
                                        Counter2 = 0;
                                        if (!masoBool[2])
                                        {
                                            masoBool[2] = true;
                                            Timer = (int)(npc.Center.X + Math.Sign(npc.velocity.X) * 2500);
                                        }
                                        if (Math.Abs(npc.Center.X - Timer) > 800)
                                        {
                                            Vector2 spawnPos = new Vector2(Timer, npc.Center.Y);
                                            Main.PlaySound(SoundID.Item34, spawnPos);
                                            Timer += Math.Sign(npc.velocity.X) * -24; //wall of flame advances closer
                                            const int offsetY = 800;
                                            const int speed = 14;
                                            if (Main.netMode != 1)
                                            {
                                                Projectile.NewProjectile(spawnPos + Vector2.UnitY * offsetY, Vector2.UnitY * -speed, mod.ProjectileType("CursedFlamethrower"), npc.damage / 4, 0f, Main.myPlayer);
                                                Projectile.NewProjectile(spawnPos + Vector2.UnitY * offsetY / 2, Vector2.UnitY * speed, mod.ProjectileType("CursedFlamethrower"), npc.damage / 4, 0f, Main.myPlayer);
                                                Projectile.NewProjectile(spawnPos + Vector2.UnitY * -offsetY / 2, Vector2.UnitY * -speed, mod.ProjectileType("CursedFlamethrower"), npc.damage / 4, 0f, Main.myPlayer);
                                                Projectile.NewProjectile(spawnPos + Vector2.UnitY * -offsetY, Vector2.UnitY * speed, mod.ProjectileType("CursedFlamethrower"), npc.damage / 4, 0f, Main.myPlayer);

                                                Projectile.NewProjectile(spawnPos + Vector2.UnitY * offsetY, Vector2.UnitY * -speed, ProjectileID.CursedFlameHostile, npc.damage / 4, 0f, Main.myPlayer);
                                                Projectile.NewProjectile(spawnPos + Vector2.UnitY * -offsetY, Vector2.UnitY * speed, ProjectileID.CursedFlameHostile, npc.damage / 4, 0f, Main.myPlayer);
                                            }
                                        }
                                        else
                                        {
                                            Counter = 240; //immediately end
                                        }
                                    }
                                }
                                else //ichor attack
                                {
                                    if (++Counter2 > 10)
                                    {
                                        Counter2 = 0;
                                        if (Main.netMode != 1)
                                        {
                                            Vector2 target = npc.Center;
                                            target.X += Math.Sign(npc.velocity.X) * 1800f * Counter / 240f; //gradually targets further and further
                                            for (int i = 0; i < 4; i++)
                                            {
                                                Vector2 speed = target - npc.Center;
                                                speed.Y -= Math.Abs(speed.X) * 0.2f; //account for gravity
                                                //speed.Normalize(); speed *= 8f;
                                                speed /= 45f * 3f; //ichor has 3 updates per tick
                                                speed += npc.velocity / 3f;
                                                speed.X += Main.rand.Next(-20, 21) * 0.08f;
                                                speed.Y += Main.rand.Next(-20, 21) * 0.08f;
                                                Projectile.NewProjectile(npc.Center, speed, ProjectileID.GoldenShowerHostile, npc.damage / 5, 0f, Main.myPlayer);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (npc.life < npc.lifeMax / 2) //enter phase 2
                        {
                            masoBool[0] = true;
                            npc.netUpdate = true;
                            Main.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);
                        }

                        /*if (--Counter < 0)
                        {
                            Counter = 60 + (int)(120f * npc.life / npc.lifeMax);
                            if (Main.netMode != 1 && npc.HasPlayerTarget && Main.player[npc.target].active) //vanilla spaz p1 shoot fireball code
                            {
                                Vector2 Speed = Main.player[npc.target].Center - npc.Center;
                                if (Speed.X * npc.velocity.X > 0) //don't shoot fireballs behind myself
                                {
                                    Speed.Normalize();
                                    int Damage;
                                    Speed *= 10f;
                                    Damage = npc.damage / 12;
                                    Speed.X += Main.rand.Next(-40, 41) * 0.02f;
                                    Speed.Y += Main.rand.Next(-40, 41) * 0.02f;
                                    Speed += Main.player[npc.target].velocity / 5;
                                    Projectile.NewProjectile(npc.Center + Speed * 4f, Speed, ProjectileID.CursedFlameHostile, Damage, 0f, Main.myPlayer);
                                }
                            }
                        }

                        if (--Timer < 0) //ichor vomit
                        {
                            Timer = 300 + 300 * (int)((float)npc.life / npc.lifeMax);
                            if (npc.HasPlayerTarget && Main.netMode != 1 && Main.player[npc.target].active)
                            {
                                for (int i = 0; i < 10; i++)
                                {
                                    Vector2 speed = Main.player[npc.target].Center - npc.Center;
                                    speed.Y -= Math.Abs(speed.X) * 0.2f; //account for gravity
                                    speed.Normalize();
                                    speed *= 8f;
                                    speed += npc.velocity / 3f;
                                    speed.X += Main.rand.Next(-20, 21) * 0.08f;
                                    speed.Y += Main.rand.Next(-20, 21) * 0.08f;
                                    Projectile.NewProjectile(npc.Center, speed, ProjectileID.GoldenShowerHostile, npc.damage / 25, 0f, Main.myPlayer);
                                }
                            }
                        }*/

                        if (npc.HasPlayerTarget && (Main.player[npc.target].dead || Vector2.Distance(npc.Center, Main.player[npc.target].Center) > 3000))
                        {
                            npc.TargetClosest(true);
                            if (Main.player[npc.target].dead || Vector2.Distance(npc.Center, Main.player[npc.target].Center) > 3000)
                            {
                                npc.position.X += 20 * Math.Sign(npc.velocity.X); //move faster to despawn
                                if (!masoBool[3]) //drop a resummon
                                {
                                    masoBool[3] = true;
                                    if (Main.netMode != 1 && !Main.hardMode && Fargowiltas.Instance.FargowiltasLoaded)
                                        Item.NewItem(npc.Hitbox, mod.ItemType("FleshierDoll"));
                                }
                            }
                            else if (Math.Abs(npc.velocity.X) > 7f)
                            {
                                npc.position.X -= (Math.Abs(npc.velocity.X) - 7f) * Math.Sign(npc.velocity.X);
                            }
                        }
                        else if (Math.Abs(npc.velocity.X) > 7f)
                            npc.position.X -= (Math.Abs(npc.velocity.X) - 7f) * Math.Sign(npc.velocity.X);


                        //dont do with swarm active
                        if (Main.player[Main.myPlayer].active & !Main.player[Main.myPlayer].dead && Main.player[Main.myPlayer].ZoneUnderworldHeight)
                        {
                            float velX = npc.velocity.X;
                            if (velX > 5f)
                                velX = 5f;
                            else if (velX < -5f)
                                velX = -5f;

                            for (int i = 0; i < 10; i++) //dust
                            {
                                Vector2 dustPos = new Vector2(2000 * npc.direction, 0f).RotatedBy(Math.PI / 3 * (-0.5 + Main.rand.NextDouble()));
                                int d = Dust.NewDust(npc.Center + dustPos, 0, 0, DustID.Fire);
                                Main.dust[d].scale += 1f;
                                Main.dust[d].velocity.X = velX;
                                Main.dust[d].velocity.Y = npc.velocity.Y;
                                Main.dust[d].noGravity = true;
                                Main.dust[d].noLight = true;
                            }

                            if (++npc.localAI[1] > 15f)
                            {
                                npc.localAI[1] = 0f; //tongue the player if they're 2000-2800 units away
                                if (Math.Abs(2400 - npc.Distance(Main.player[Main.myPlayer].Center)) < 400)
                                {
                                    if (!Main.player[Main.myPlayer].tongued)
                                        Main.PlaySound(15, Main.player[Main.myPlayer].Center, 0);
                                    Main.player[Main.myPlayer].AddBuff(BuffID.TheTongue, 10);
                                }
                            }
                        }
                        break;

                    case NPCID.WallofFleshEye:
                        if (masoBool[3])
                            break;

                        if (npc.realLife != -1 && Main.npc[npc.realLife].GetGlobalNPC<FargoSoulsGlobalNPC>().masoBool[0]
                            && Main.npc[npc.realLife].GetGlobalNPC<FargoSoulsGlobalNPC>().Counter < 240)
                            npc.localAI[1] = 0; //dont fire during mouth's special attacks

                        const float maxTime = 540f;
                        if (++npc.ai[1] >= maxTime)
                        {
                            npc.ai[1] = 0f;
                            if (npc.ai[2] == 0f)
                                npc.ai[2] = 1f;
                            else
                                npc.ai[2] *= -1f;

                            if (npc.ai[2] > 0 && Main.netMode != 1) //FIRE LASER
                            {
                                Vector2 speed = Vector2.UnitX.RotatedBy(npc.ai[3]);
                                float ai0 = (npc.realLife != -1 && Main.npc[npc.realLife].velocity.X > 0) ? 1f : 0f;
                                if (Main.netMode != 1)
                                    Projectile.NewProjectile(npc.Center, speed, mod.ProjectileType("PhantasmalDeathrayWOF"), npc.damage / 4, 0f, Main.myPlayer, ai0, npc.whoAmI);
                            }
                            npc.netUpdate = true;
                        }

                        if (npc.ai[2] >= 0f)
                        {
                            npc.alpha = 175;
                            npc.dontTakeDamage = true;
                            if (npc.ai[1] <= 90)
                            {
                                masoBool[3] = true;
                                npc.AI();
                                masoBool[3] = false;
                                npc.localAI[1] = 0f;
                                npc.rotation = npc.ai[3];
                                return false;
                            }
                            else
                            {
                                npc.ai[2] = 1;
                            }
                        }
                        else
                        {
                            npc.alpha = 0;
                            npc.dontTakeDamage = false;
                            if (npc.ai[1] > maxTime - 180f)
                            {
                                if (Main.rand.Next(4) < 3) //dust telegraphs switch
                                {
                                    int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, 90, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 114, default(Color), 3.5f);
                                    Main.dust[dust].noGravity = true;
                                    Main.dust[dust].velocity *= 1.8f;
                                    Main.dust[dust].velocity.Y -= 0.5f;
                                    if (Main.rand.Next(4) == 0)
                                    {
                                        Main.dust[dust].noGravity = false;
                                        Main.dust[dust].scale *= 0.5f;
                                    }
                                }

                                const float stopTime = maxTime - 90f;
                                if (npc.ai[1] == stopTime) //shoot warning dust in phase 2
                                {
                                    int t = npc.HasPlayerTarget ? npc.target : npc.FindClosestPlayer();
                                    if (t != -1)
                                    {
                                        if (npc.Distance(Main.player[t].Center) < 3000)
                                            Main.PlaySound(15, (int)Main.player[t].position.X, (int)Main.player[t].position.Y, 0);
                                        npc.ai[2] = -2f;
                                        npc.ai[3] = (npc.Center - Main.player[t].Center).ToRotation();
                                        if (npc.realLife != -1 && Main.npc[npc.realLife].velocity.X > 0)
                                            npc.ai[3] += (float)Math.PI;

                                        float ai0 = (npc.realLife != -1 && Main.npc[npc.realLife].velocity.X > 0) ? 1f : 0f;
                                        Vector2 speed = Vector2.UnitX.RotatedBy(npc.ai[3]);
                                        if (Main.netMode != 1)
                                            Projectile.NewProjectile(npc.Center, speed, mod.ProjectileType("PhantasmalDeathrayWOFS"), 0, 0f, Main.myPlayer, ai0, npc.whoAmI);
                                    }
                                    npc.netUpdate = true;
                                }
                                else if (npc.ai[1] > stopTime)
                                {
                                    masoBool[3] = true;
                                    npc.AI();
                                    masoBool[3] = false;
                                    npc.localAI[1] = 0f;
                                    npc.rotation = npc.ai[3];
                                    return false;
                                }
                            }
                        }
                        break;

                    case NPCID.TheDestroyer:
                        destroyBoss = npc.whoAmI;
                        
                        if (!masoBool[0])
                        {
                            if (npc.life < (int)(npc.lifeMax * .75))
                            {
                                masoBool[0] = true;
                                Counter = 900;
                                npc.netUpdate = true;
                                if (npc.HasPlayerTarget)
                                    Main.PlaySound(15, (int)Main.player[npc.target].position.X, (int)Main.player[npc.target].position.Y, 0);
                            }
                            RegenTimer = 2;
                        }
                        else
                        {
                            if (npc.HasValidTarget && !Main.dayTime)
                            {
                                if (masoBool[1]) //spinning
                                {
                                    npc.netUpdate = true;
                                    npc.velocity += npc.velocity.RotatedBy(Math.PI / 2) * npc.velocity.Length() / Counter2;
                                    npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) + 1.57f;

                                    if (++npc.localAI[2] > 40) //shoot star spreads into the circle
                                    {
                                        npc.localAI[2] = 0;
                                        if (Main.netMode != 1)
                                        {
                                            Vector2 distance = Main.player[npc.target].Center - npc.Center;
                                            double angleModifier = MathHelper.ToRadians(5) * distance.Length() / 1800.0;
                                            distance.Normalize();
                                            distance *= 8f;
                                            int type = mod.ProjectileType("DarkStar");
                                            Projectile.NewProjectile(npc.Center, distance.RotatedBy(-angleModifier), type, npc.damage / 5, 0f, Main.myPlayer);
                                            Projectile.NewProjectile(npc.Center, distance, type, npc.damage / 5, 0f, Main.myPlayer);
                                            Projectile.NewProjectile(npc.Center, distance.RotatedBy(angleModifier), type, npc.damage / 5, 0f, Main.myPlayer);
                                        }
                                    }

                                    Vector2 pivot = npc.Center;
                                    pivot += Vector2.Normalize(npc.velocity.RotatedBy(Math.PI / 2)) * 600;

                                    if (++Timer > 100)
                                    {
                                        Timer = 0;
                                        if (Main.netMode != 1)
                                        {
                                            const int max = 6;
                                            for (int i = 0; i < max; i++)
                                            {
                                                Vector2 speed = npc.DirectionTo(pivot).RotatedBy(2 * Math.PI / max * i);
                                                Vector2 spawnPos = pivot - speed * 600;
                                                Projectile.NewProjectile(spawnPos, speed, mod.ProjectileType("DestroyerLaser"), npc.damage / 5, 0f, Main.myPlayer);
                                            }
                                        }
                                    }

                                    for (int i = 0; i < 20; i++) //arena dust
                                    {
                                        Vector2 offset = new Vector2();
                                        double angle = Main.rand.NextDouble() * 2d * Math.PI;
                                        offset.X += (float)(Math.Sin(angle) * 600);
                                        offset.Y += (float)(Math.Cos(angle) * 600);
                                        Dust dust = Main.dust[Dust.NewDust(pivot + offset - new Vector2(4, 4), 0, 0, 112, 0, 0, 100, Color.White, 1f)];
                                        dust.velocity = Vector2.Zero;
                                        if (Main.rand.Next(3) == 0)
                                            dust.velocity += Vector2.Normalize(offset) * 5f;
                                        dust.noGravity = true;
                                    }

                                    if (Main.LocalPlayer.active && !Main.LocalPlayer.dead) //arena effect
                                    {
                                        float distance = Main.LocalPlayer.Distance(pivot);
                                        if (distance > 600 && distance < 3000)
                                        {
                                            Vector2 movement = pivot - Main.LocalPlayer.Center;
                                            float difference = movement.Length() - 600;
                                            movement.Normalize();
                                            movement *= difference < 17f ? difference : 17f;
                                            Main.LocalPlayer.position += movement;

                                            for (int i = 0; i < 20; i++)
                                            {
                                                int d = Dust.NewDust(Main.LocalPlayer.position, Main.LocalPlayer.width, Main.LocalPlayer.height, 112, 0f, 0f, 0, default(Color), 2f);
                                                Main.dust[d].noGravity = true;
                                                Main.dust[d].velocity *= 5f;
                                            }
                                        }
                                    }

                                    if (++Counter > 300) //go back to normal AI
                                    {
                                        Counter = 0;
                                        Counter2 = 0;
                                        masoBool[1] = false;
                                        masoBool[2] = false;
                                        NetUpdateMaso(npc.whoAmI);
                                    }
                                }
                                else
                                {
                                    float num14 = 16f;    //max speed?
                                    float num15 = 0.1f;   //turn speed?
                                    float num16 = 0.15f;   //acceleration?

                                    Vector2 target = Main.player[npc.target].Center;
                                    if (masoBool[2]) //move MUCH faster, approach a position nearby
                                    {
                                        num15 = 0.4f;
                                        num16 = 0.5f;

                                        target += Main.player[npc.target].DirectionTo(npc.Center) * 600;

                                        if (++Counter > 300) //move way faster if still not in range
                                            num14 *= 2f;

                                        if (npc.Distance(target) < 50)
                                        {
                                            Counter = 0;
                                            Counter2 = (int)npc.Distance(Main.player[npc.target].Center);
                                            masoBool[1] = true;
                                            npc.velocity = 20 * npc.DirectionTo(Main.player[npc.target].Center).RotatedBy(-Math.PI / 2);
                                            NetUpdateMaso(npc.whoAmI);
                                            Main.PlaySound(15, (int)Main.player[npc.target].position.X, (int)Main.player[npc.target].position.Y, 0);
                                            if (Main.netMode != 1)
                                            {
                                                for (int i = 0; i < Main.maxProjectiles; i++)
                                                {
                                                    if (Main.projectile[i].active && Main.projectile[i].type == mod.ProjectileType("DarkStar"))
                                                        Main.projectile[i].Kill();
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (++Counter > 900) //change state
                                        {
                                            Counter = 0;
                                            masoBool[2] = true;
                                            NetUpdateMaso(npc.whoAmI);
                                        }
                                    }
                                    float num17 = target.X;
                                    float num18 = target.Y;

                                    float num21 = num17 - npc.Center.X;
                                    float num22 = num18 - npc.Center.Y;
                                    float num23 = (float)Math.Sqrt((double)num21 * (double)num21 + (double)num22 * (double)num22);

                                    //ground movement code but it always runs
                                    float num2 = (float)Math.Sqrt(num21 * num21 + num22 * num22);
                                    float num3 = Math.Abs(num21);
                                    float num4 = Math.Abs(num22);
                                    float num5 = num14 / num2;
                                    float num6 = num21 * num5;
                                    float num7 = num22 * num5;
                                    if ((npc.velocity.X > 0f && num6 > 0f || npc.velocity.X < 0f && num6 < 0f) && (npc.velocity.Y > 0f && num7 > 0f || npc.velocity.Y < 0f && num7 < 0f))
                                    {
                                        if (npc.velocity.X < num6)
                                            npc.velocity.X += num16;
                                        else if (npc.velocity.X > num6)
                                            npc.velocity.X -= num16;
                                        if (npc.velocity.Y < num7)
                                            npc.velocity.Y += num16;
                                        else if (npc.velocity.Y > num7)
                                            npc.velocity.Y -= num16;
                                    }
                                    if (npc.velocity.X > 0f && num6 > 0f || npc.velocity.X < 0f && num6 < 0f || npc.velocity.Y > 0f && num7 > 0f || npc.velocity.Y < 0f && num7 < 0f)
                                    {
                                        if (npc.velocity.X < num6)
                                            npc.velocity.X += num15;
                                        else if (npc.velocity.X > num6)
                                            npc.velocity.X -= num15;
                                        if (npc.velocity.Y < num7)
                                            npc.velocity.Y += num15;
                                        else if (npc.velocity.Y > num7)
                                            npc.velocity.Y -= num15;

                                        if (Math.Abs(num7) < num14 * 0.2f && (npc.velocity.X > 0f && num6 < 0f || npc.velocity.X < 0f && num6 > 0f))
                                        {
                                            if (npc.velocity.Y > 0f)
                                                npc.velocity.Y += num15 * 2f;
                                            else
                                                npc.velocity.Y -= num15 * 2f;
                                        }
                                        if (Math.Abs(num6) < num14 * 0.2f && (npc.velocity.Y > 0f && num7 < 0f || npc.velocity.Y < 0f && num7 > 0f))
                                        {
                                            if (npc.velocity.X > 0f)
                                                npc.velocity.X += num15 * 2f;
                                            else
                                                npc.velocity.X -= num15 * 2f;
                                        }
                                    }
                                    else if (num3 > num4)
                                    {
                                        if (npc.velocity.X < num6)
                                            npc.velocity.X += num15 * 1.1f;
                                        else if (npc.velocity.X > num6)
                                            npc.velocity.X -= num15 * 1.1f;

                                        if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < num14 * 0.5f)
                                        {
                                            if (npc.velocity.Y > 0f)
                                                npc.velocity.Y += num15;
                                            else
                                                npc.velocity.Y -= num15;
                                        }
                                    }
                                    else
                                    {
                                        if (npc.velocity.Y < num7)
                                            npc.velocity.Y += num15 * 1.1f;
                                        else if (npc.velocity.Y > num7)
                                            npc.velocity.Y -= num15 * 1.1f;

                                        if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < num14 * 0.5f)
                                        {
                                            if (npc.velocity.X > 0f)
                                                npc.velocity.X += num15;
                                            else
                                                npc.velocity.X -= num15;
                                        }
                                    }
                                    npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) + 1.57f;
                                    npc.netUpdate = true;
                                    npc.localAI[0] = 1f;

                                    float ratio = (float)npc.life / npc.lifeMax;
                                    if (ratio > 0.5f)
                                        ratio = 0.5f;
                                    npc.position += npc.velocity * (.5f - ratio);
                                }
                                
                                if (Main.netMode == 2 && npc.netUpdate && --npc.netSpam < 0) //manual mp sync control
                                {
                                    npc.netUpdate = false;
                                    npc.netSpam = 5;
                                    NetMessage.SendData(23, -1, -1, null, npc.whoAmI);
                                }
                                return false;
                            }
                        }
                        break;

                    case NPCID.DukeFishron:
                        fishBoss = npc.whoAmI;
                        if (masoBool[3]) //fishron EX
                        {
                            if (npc.Distance(Main.player[Main.myPlayer].Center) < 1800f)
                                Main.player[Main.myPlayer].AddBuff(mod.BuffType("OceanicSeal"), 2);
                            fishBossEX = npc.whoAmI;
                            npc.position += npc.velocity * 0.5f;
                            switch ((int)npc.ai[0])
                            {
                                case -1: //just spawned
                                    if (npc.ai[2] == 2 && Main.netMode != 1) //create spell circle
                                    {
                                        int ritual1 = Projectile.NewProjectile(npc.Center, Vector2.Zero,
                                            mod.ProjectileType("FishronRitual"), 0, 0f, Main.myPlayer, npc.lifeMax, npc.whoAmI);
                                        if (ritual1 == 1000) //failed to spawn projectile, abort spawn
                                            npc.active = false;
                                        Main.PlaySound(SoundID.Item84, npc.Center);
                                    }
                                    masoBool[2] = true;
                                    break;

                                case 0: //phase 1
                                    if (!masoBool[1])
                                        npc.dontTakeDamage = false;
                                    masoBool[2] = false;
                                    npc.ai[2]++;
                                    break;

                                case 1: //p1 dash
                                    Counter++;
                                    if (Counter > 5)
                                    {
                                        Counter = 0;
                                        if (Main.netMode != 1)
                                        {
                                            int n = NPC.NewNPC((int)npc.position.X + Main.rand.Next(npc.width), (int)npc.position.Y + Main.rand.Next(npc.height), NPCID.DetonatingBubble);
                                            if (n != 200 && Main.netMode == 2)
                                                NetMessage.SendData(23, -1, -1, null, n);
                                            n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("DetonatingBubbleEX"));
                                            if (n < 200)
                                            {
                                                Main.npc[n].velocity = npc.DirectionTo(Main.player[npc.target].Center);
                                                Main.npc[n].netUpdate = true;
                                                if (Main.netMode == 2)
                                                    NetMessage.SendData(23, -1, -1, null, n);
                                            }
                                        }
                                    }
                                    break;

                                case 2: //p1 bubbles
                                    if (npc.ai[2] == 0f && Main.netMode != 1)
                                        Projectile.NewProjectile(npc.Center, Vector2.Zero, ProjectileID.SharknadoBolt, 0, 0f, Main.myPlayer, 1f, npc.target + 1);
                                    break;

                                case 3: //p1 drop nados
                                    if (npc.ai[2] == 60f && Main.netMode != 1)
                                    {
                                        const int max = 32;
                                        float rotation = 2f * (float)Math.PI / max;
                                        for (int i = 0; i < max; i++)
                                        {
                                            int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("DetonatingBubbleEX"));
                                            if (n < 200)
                                            {
                                                Main.npc[n].velocity = Vector2.UnitY.RotatedBy(rotation * i);
                                                Main.npc[n].velocity.Normalize();
                                                Main.npc[n].netUpdate = true;
                                                if (Main.netMode == 2)
                                                    NetMessage.SendData(23, -1, -1, null, n);
                                            }
                                        }

                                        SpawnRazorbladeRing(npc, 18, 10f, npc.damage / 6, 1f);
                                    }
                                    break;

                                case 4: //phase 2 transition
                                    masoBool[1] = false;
                                    masoBool[2] = true;
                                    if (npc.ai[2] == 1 && Main.netMode != 1)
                                        Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("FishronRitual"), 0, 0f, Main.myPlayer, npc.lifeMax / 4, npc.whoAmI);
                                    if (npc.ai[2] >= 114)
                                    {
                                        Counter++;
                                        if (Counter > 6) //display healing effect
                                        {
                                            Counter = 0;
                                            int heal = (int)(npc.lifeMax * Main.rand.NextFloat(0.1f, 0.12f));
                                            npc.life += heal;
                                            int max = npc.ai[0] == 9 && !Fargowiltas.Instance.MasomodeEXLoaded ? npc.lifeMax / 2 : npc.lifeMax;
                                            if (npc.life > max)
                                                npc.life = max;
                                            CombatText.NewText(npc.Hitbox, CombatText.HealLife, heal);
                                        }
                                    }
                                    break;

                                case 5: //phase 2
                                    if (!masoBool[1])
                                        npc.dontTakeDamage = false;
                                    masoBool[2] = false;
                                    npc.ai[2]++;
                                    break;

                                case 6: //p2 dash
                                    goto case 1;

                                case 7: //p2 spin & bubbles
                                    npc.position -= npc.velocity * 0.5f;
                                    Counter++;
                                    if (Counter > 1)
                                    {
                                        Counter = 0;
                                        if (Main.netMode != 1)
                                        {
                                            int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("DetonatingBubbleEX"));
                                            if (n < 200)
                                            {
                                                Main.npc[n].velocity = npc.velocity.RotatedBy(Math.PI / 2);
                                                Main.npc[n].velocity.Normalize();
                                                Main.npc[n].netUpdate = true;
                                                if (Main.netMode == 2)
                                                    NetMessage.SendData(23, -1, -1, null, n);
                                            }
                                            n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("DetonatingBubbleEX"));
                                            if (n < 200)
                                            {
                                                Main.npc[n].velocity = npc.velocity.RotatedBy(-Math.PI / 2);
                                                Main.npc[n].velocity.Normalize();
                                                Main.npc[n].netUpdate = true;
                                                if (Main.netMode == 2)
                                                    NetMessage.SendData(23, -1, -1, null, n);
                                            }
                                        }
                                    }
                                    break;

                                case 8: //p2 cthulhunado
                                    if (Main.netMode != 1 && npc.ai[2] == 60)
                                    {
                                        Vector2 spawnPos = Vector2.UnitX * npc.direction;
                                        spawnPos = spawnPos.RotatedBy(npc.rotation);
                                        spawnPos *= npc.width + 20f;
                                        spawnPos /= 2f;
                                        spawnPos += npc.Center;
                                        Projectile.NewProjectile(spawnPos.X, spawnPos.Y, npc.direction * 2f, 8f, ProjectileID.SharknadoBolt, 0, 0f, Main.myPlayer);
                                        Projectile.NewProjectile(spawnPos.X, spawnPos.Y, npc.direction * -2f, 8f, ProjectileID.SharknadoBolt, 0, 0f, Main.myPlayer);
                                        Projectile.NewProjectile(spawnPos.X, spawnPos.Y, 0f, 2f, ProjectileID.SharknadoBolt, 0, 0f, Main.myPlayer);

                                        SpawnRazorbladeRing(npc, 12, 12.5f, npc.damage / 6, 0.75f);
                                        SpawnRazorbladeRing(npc, 12, 10f, npc.damage / 6, -2f);
                                    }
                                    break;

                                case 9: //phase 3 transition
                                    if (npc.ai[2] == 1f)
                                    {
                                        for (int i = 0; i < npc.buffImmune.Length; i++)
                                            npc.buffImmune[i] = true;
                                        while (npc.buffTime[0] != 0)
                                            npc.DelBuff(0);
                                        npc.defDamage = (int)(npc.defDamage * 1.2f);
                                    }
                                    goto case 4;

                                case 10: //phase 3
                                         //vanilla fishron has x1.1 damage in p3. p2 has x1.2 damage...
                                        //npc.damage = (int)(npc.defDamage * 1.2f * (Main.expertMode ? 0.6f * Main.damageMultiplier : 1f));
                                    masoBool[2] = false;
                                    Timer++;
                                    //if (Timer >= 60 + (int)(540.0 * npc.life / npc.lifeMax)) //yes that needs to be a double
                                    if (Timer >= 900)
                                    {
                                        Timer = 0;
                                        if (Main.netMode != 1) //spawn cthulhunado
                                            Projectile.NewProjectile(npc.Center, Vector2.Zero, ProjectileID.SharknadoBolt, 0, 0f, Main.myPlayer, 1f, npc.target + 1);
                                    }
                                    break;

                                case 11: //p3 dash
                                    Counter++;
                                    if (Counter > 1)
                                    {
                                        Counter = 0;
                                        if (Main.netMode != 1)
                                        {
                                            int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("DetonatingBubbleEX"));
                                            if (n < 200)
                                            {
                                                Main.npc[n].velocity = npc.velocity.RotatedBy(Math.PI / 2);
                                                Main.npc[n].velocity.Normalize();
                                                Main.npc[n].netUpdate = true;
                                                if (Main.netMode == 2)
                                                    NetMessage.SendData(23, -1, -1, null, n);
                                            }
                                            n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("DetonatingBubbleEX"));
                                            if (n < 200)
                                            {
                                                Main.npc[n].velocity = npc.velocity.RotatedBy(-Math.PI / 2);
                                                Main.npc[n].velocity.Normalize();
                                                Main.npc[n].netUpdate = true;
                                                if (Main.netMode == 2)
                                                    NetMessage.SendData(23, -1, -1, null, n);
                                            }
                                        }
                                    }
                                    goto case 10;

                                case 12: //p3 *teleports behind you*
                                    if (npc.ai[2] == 15f)
                                    {
                                        if (Main.netMode != 1)
                                        {
                                            SpawnRazorbladeRing(npc, 5, 9f, npc.damage / 6, 1f, true);
                                            SpawnRazorbladeRing(npc, 5, 9f, npc.damage / 6, -0.5f, true);
                                        }
                                    }
                                    else if (npc.ai[2] == 16f)
                                    {
                                        if (Main.netMode != 1)
                                        {
                                            /*Vector2 spawnPos = Vector2.UnitX * npc.direction;
                                            spawnPos = spawnPos.RotatedBy(npc.rotation);
                                            spawnPos *= npc.width + 20f;
                                            spawnPos /= 2f;
                                            spawnPos += npc.Center;
                                            Projectile.NewProjectile(spawnPos.X, spawnPos.Y, npc.direction * 2f, 8f, ProjectileID.SharknadoBolt, 0, 0f, Main.myPlayer);
                                            Projectile.NewProjectile(spawnPos.X, spawnPos.Y, npc.direction * -2f, 8f, ProjectileID.SharknadoBolt, 0, 0f, Main.myPlayer);*/

                                            const int max = 24;
                                            float rotation = 2f * (float)Math.PI / max;
                                            for (int i = 0; i < max; i++)
                                            {
                                                int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("DetonatingBubbleEX"));
                                                if (n < 200)
                                                {
                                                    Main.npc[n].velocity = npc.velocity.RotatedBy(rotation * i);
                                                    Main.npc[n].velocity.Normalize();
                                                    Main.npc[n].netUpdate = true;
                                                    if (Main.netMode == 2)
                                                        NetMessage.SendData(23, -1, -1, null, n);
                                                }
                                            }
                                        }
                                    }
                                    goto case 10;

                                default:
                                    break;
                            }
                        }
                        else //fishron regular
                        {
                            npc.position += npc.velocity * 0.25f;
                            switch ((int)npc.ai[0])
                            {
                                case -1: //just spawned
                                    /*if (npc.ai[2] == 1 && Main.netMode != 1) //create spell circle
                                    {
                                        int p2 = Projectile.NewProjectile(npc.Center, Vector2.Zero,
                                            mod.ProjectileType("FishronRitual2"), 0, 0f, Main.myPlayer, 0f, npc.whoAmI);
                                        if (p2 == 1000) //failed to spawn projectile, abort spawn
                                            npc.active = false;
                                    }*/
                                    npc.dontTakeDamage = true;
                                    break;

                                case 0: //phase 1
                                    if (!masoBool[1])
                                        npc.dontTakeDamage = false;
                                    break;

                                case 1: //p1 dash
                                    Counter++;
                                    if (Counter > 5)
                                    {
                                        Counter = 0;

                                        if (Main.netMode != 1)
                                        {
                                            int n = NPC.NewNPC((int)npc.position.X + Main.rand.Next(npc.width), (int)npc.position.Y + Main.rand.Next(npc.height), NPCID.DetonatingBubble);
                                            if (n != 200 && Main.netMode == 2)
                                                NetMessage.SendData(23, -1, -1, null, n);
                                        }
                                    }
                                    break;

                                case 2: //p1 bubbles
                                    break;

                                case 3: //p1 drop nados
                                    if (npc.ai[2] == 60f && Main.netMode != 1)
                                    {
                                        SpawnRazorbladeRing(npc, 12, 10f, npc.damage / 4, 1f);
                                    }
                                    break;

                                case 4: //phase 2 transition
                                    npc.dontTakeDamage = true;
                                    masoBool[1] = false;
                                    if (npc.ai[2] == 120)
                                    {
                                        int heal = npc.lifeMax - npc.life;
                                        npc.life = npc.lifeMax;
                                        CombatText.NewText(npc.Hitbox, CombatText.HealLife, heal);
                                    }
                                    break;

                                case 5: //phase 2
                                    if (!masoBool[1])
                                        npc.dontTakeDamage = false;
                                    break;

                                case 6: //p2 dash
                                    goto case 1;

                                case 7: //p2 spin & bubbles
                                    npc.position -= npc.velocity * 0.25f;
                                    Counter++;
                                    if (Counter > 1)
                                    {
                                        Counter = 0;
                                        if (Main.netMode != 1)
                                        {
                                            int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("DetonatingBubble"));
                                            if (n < 200)
                                            {
                                                Main.npc[n].velocity = npc.velocity.RotatedBy(Math.PI / 2);
                                                Main.npc[n].velocity *= -npc.spriteDirection;
                                                Main.npc[n].velocity.Normalize();
                                                Main.npc[n].netUpdate = true;
                                                if (Main.netMode == 2)
                                                    NetMessage.SendData(23, -1, -1, null, n);
                                            }
                                        }
                                    }
                                    break;

                                case 8: //p2 cthulhunado
                                    if (Main.netMode != 1 && npc.ai[2] == 60)
                                    {
                                        Vector2 spawnPos = Vector2.UnitX * npc.direction;
                                        spawnPos = spawnPos.RotatedBy(npc.rotation);
                                        spawnPos *= npc.width + 20f;
                                        spawnPos /= 2f;
                                        spawnPos += npc.Center;
                                        Projectile.NewProjectile(spawnPos.X, spawnPos.Y, 0f, 8f, ProjectileID.SharknadoBolt, 0, 0f, Main.myPlayer);

                                        SpawnRazorbladeRing(npc, 12, 10f, npc.damage / 4, 2f);
                                    }
                                    break;

                                case 9: //phase 3 transition
                                    npc.dontTakeDamage = true;
                                    npc.defDefense = 0;
                                    npc.defense = 0;
                                    masoBool[1] = false;
                                    if (npc.ai[2] == 120)
                                    {
                                        int max = Fargowiltas.Instance.MasomodeEXLoaded ? npc.lifeMax : npc.lifeMax / 2;
                                        int heal = max - npc.life;
                                        npc.life = max;
                                        CombatText.NewText(npc.Hitbox, CombatText.HealLife, heal);
                                    }
                                    break;

                                case 10: //phase 3
                                    Timer++;
                                    /*if (Timer >= 600) //spawn cthulhunado
                                    {
                                        Timer = 0;
                                        if (Main.netMode != 1)
                                            Projectile.NewProjectile(npc.Center, Vector2.Zero, ProjectileID.SharknadoBolt, 0, 0f, Main.myPlayer, 1f, npc.target + 1);
                                    }*/
                                    break;

                                case 11: //p3 dash
                                    Counter++;
                                    if (Counter > 2)
                                    {
                                        Counter = 0;
                                        if (Main.netMode != 1)
                                        {
                                            int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("DetonatingBubble"));
                                            if (n < 200)
                                            {
                                                Main.npc[n].velocity = npc.velocity.RotatedBy(Math.PI / 2);
                                                Main.npc[n].velocity.Normalize();
                                                Main.npc[n].netUpdate = true;
                                                if (Main.netMode == 2)
                                                    NetMessage.SendData(23, -1, -1, null, n);
                                            }
                                            n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("DetonatingBubble"));
                                            if (n < 200)
                                            {
                                                Main.npc[n].velocity = npc.velocity.RotatedBy(-Math.PI / 2);
                                                Main.npc[n].velocity.Normalize();
                                                Main.npc[n].netUpdate = true;
                                                if (Main.netMode == 2)
                                                    NetMessage.SendData(23, -1, -1, null, n);
                                            }
                                        }
                                    }
                                    goto case 10;

                                case 12: //p3 *teleports behind you*
                                    if (npc.ai[2] == 15f)
                                    {
                                        SpawnRazorbladeRing(npc, 6, 8f, npc.damage / 4, -0.75f);
                                    }
                                    /*else if (npc.ai[2] == 16f)
                                    {
                                        if (Main.netMode != 1)
                                        {
                                            Vector2 spawnPos = Vector2.UnitX * npc.direction;
                                            spawnPos = spawnPos.RotatedBy(npc.rotation);
                                            spawnPos *= npc.width + 20f;
                                            spawnPos /= 2f;
                                            spawnPos += npc.Center;
                                            Projectile.NewProjectile(spawnPos.X, spawnPos.Y, 0f, 8f, ProjectileID.SharknadoBolt, 0, 0f, Main.myPlayer);
                                        }
                                    }*/
                                    goto case 10;

                                default:
                                    break;
                            }
                        }
                        break;

                    case NPCID.MoonLordCore:
                        moonBoss = npc.whoAmI;
                        RegenTimer = 2;
                        npc.defense = masoStateML >= 0 && masoStateML <= 3 ? 0 : npc.defDefense;

                        if (!masoBool[3])
                        {
                            masoBool[3] = true;
                            if (Main.netMode != 1)
                            {
                                Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("LunarRitual"),
                                      (int)(100.0 * (1.0 + FargoSoulsWorld.MoonlordCount * .0125)), 0f, Main.myPlayer, 0f, npc.whoAmI);
                                Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("FragmentRitual"), 0, 0f, Main.myPlayer, 0f, npc.whoAmI);
                            }
                        }

                        if (!npc.dontTakeDamage)
                            Counter++; //phases transition twice as fast when core is exposed

                        if (Main.player[Main.myPlayer].active && !Main.player[Main.myPlayer].dead && masoStateML >= 0 && masoStateML <= 3)
                            Main.player[Main.myPlayer].AddBuff(mod.BuffType("NullificationCurse"), 2);

                        if (!masoBool[0])
                        {
                            masoBool[0] = npc.life < (int)(npc.lifeMax / 3.0 * 2.0); //remembers even if core goes above 66% hp
                            if (masoBool[0]) //roar
                                Main.PlaySound(15, Main.player[Main.myPlayer].Center, 0);
                        }
                        else //phase 3, went below 66% life
                        {
                            Timer++;
                            if (Timer >= 240)
                            {
                                Timer = 0;
                                if (Main.netMode != 1)
                                {
                                    switch (masoStateML)
                                    {
                                        case 0: //melee
                                            for (int i = 0; i < 3; i++)
                                            {
                                                NPC bodyPart = Main.npc[(int)npc.localAI[i]];

                                                if (bodyPart.active)
                                                {
                                                    if (i == 2 && bodyPart.type == NPCID.MoonLordHead)
                                                    {
                                                        Vector2 speed = new Vector2(0f, -12f).RotatedBy(MathHelper.ToRadians(-60));
                                                        for (int j = 0; j < 6; j++)
                                                        {
                                                            int n = NPC.NewNPC((int)bodyPart.Center.X, (int)bodyPart.Center.Y, NPCID.SolarGoop);
                                                            if (n < 200)
                                                            {
                                                                Main.npc[n].velocity = speed;
                                                                if (Main.netMode == 2)
                                                                    NetMessage.SendData(23, -1, -1, null, n);
                                                            }
                                                            speed = speed.RotatedBy(MathHelper.ToRadians(20));
                                                        }
                                                    }
                                                    else if (bodyPart.type == NPCID.MoonLordHand)
                                                    {
                                                        Vector2 speed = Main.player[npc.target].Center - bodyPart.Center;
                                                        speed.Normalize();
                                                        speed *= 6f;
                                                        int damage = (int)(25 * (1 + FargoSoulsWorld.MoonlordCount * .0125));
                                                        for (int j = 0; j < 4; j++)
                                                        {
                                                            Projectile.NewProjectile(bodyPart.Center, speed,
                                                                ProjectileID.CultistBossFireBall, damage, 0f, Main.myPlayer);
                                                            speed = speed.RotatedBy(Math.PI / 2);
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        case 1: //ranged
                                            /*for (int i = 0; i < 12; i++) //spawn lightning
                                            {
                                                Point tileCoordinates = Main.player[npc.target].Top.ToTileCoordinates();

                                                if (Main.rand.Next(2) == 0)
                                                {
                                                    tileCoordinates.X += Main.rand.Next(-40, 41);
                                                    tileCoordinates.Y += Main.rand.Next(30, 41) * (Main.rand.Next(2) == 0 ? 1 : -1);
                                                }
                                                else
                                                {
                                                    tileCoordinates.X += Main.rand.Next(30, 41) * (Main.rand.Next(2) == 0 ? 1 : -1);
                                                    tileCoordinates.Y += Main.rand.Next(-40, 41);
                                                }

                                                for (int index = 0; index < 10 && !WorldGen.SolidTile(tileCoordinates.X, tileCoordinates.Y) && tileCoordinates.Y > 10; ++index)
                                                    tileCoordinates.Y -= 1;

                                                Projectile.NewProjectile(tileCoordinates.X * 16 + 8, tileCoordinates.Y * 16 + 17, 0f, 0f, 578, 0, 1f, Main.myPlayer);
                                            }*/
                                            for (int i = 0; i < 3; i++) //shoot lightning bolt
                                            {
                                                NPC bodyPart = Main.npc[(int)npc.localAI[i]];

                                                if (bodyPart.active &&
                                                    ((i == 2 && bodyPart.type == NPCID.MoonLordHead) ||
                                                    bodyPart.type == NPCID.MoonLordHand))
                                                {
                                                    Vector2 dir = Main.player[npc.target].Center - bodyPart.Center;
                                                    float ai1New = Main.rand.Next(100);
                                                    Vector2 vel = Vector2.Normalize(dir.RotatedByRandom(Math.PI / 4)) * 6f;
                                                    int damage = (int)(30 * (1 + FargoSoulsWorld.MoonlordCount * .0125));
                                                    Projectile.NewProjectile(bodyPart.Center, vel, ProjectileID.CultistBossLightningOrbArc,
                                                        damage, 0, Main.myPlayer, dir.ToRotation(), ai1New);
                                                }
                                            }
                                            Projectile.NewProjectile(npc.Center, Vector2.Zero, ProjectileID.CultistBossLightningOrb,
                                                (int)(30 * (1 + FargoSoulsWorld.MoonlordCount * .0125)), 0f, Main.myPlayer);
                                            break;
                                        case 2: //magic
                                            for (int i = 0; i < 3; i++)
                                            {
                                                NPC bodyPart = Main.npc[(int)npc.localAI[i]];

                                                if (bodyPart.active &&
                                                    ((i == 2 && bodyPart.type == NPCID.MoonLordHead) ||
                                                    bodyPart.type == NPCID.MoonLordHand))
                                                {
                                                    Vector2 distance = Main.player[npc.target].Center - bodyPart.Center;
                                                    distance.Normalize();
                                                    distance *= 7f;
                                                    int damage = (int)(35 * (1 + FargoSoulsWorld.MoonlordCount * .0125));
                                                    for (int j = -1; j <= 1; j += 2) //aim above and below player
                                                    {
                                                        Vector2 speed = distance.RotatedBy(Math.PI / 6 * j);
                                                        for (int k = -2; k <= 2; k++) //fire a 5-spread each
                                                        {
                                                            Projectile.NewProjectile(bodyPart.Center, speed.RotatedBy(Math.PI / 32 * k),
                                                                ProjectileID.NebulaLaser, damage, 0f, Main.myPlayer);
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        case 3: //summoner
                                            for (int i = 0; i < 3; i++)
                                            {
                                                NPC bodyPart = Main.npc[(int)npc.localAI[i]];

                                                if (bodyPart.active &&
                                                    ((i == 2 && bodyPart.type == NPCID.MoonLordHead) ||
                                                    bodyPart.type == NPCID.MoonLordHand))
                                                {
                                                    Vector2 speed = Main.player[npc.target].Center - bodyPart.Center;
                                                    speed.Normalize();
                                                    speed *= 9f;
                                                    for (int j = -3; j <= 3; j++)
                                                    {
                                                        Vector2 vel = speed.RotatedBy(Math.PI / 6 * j);
                                                        int n = NPC.NewNPC((int)bodyPart.Center.X, (int)bodyPart.Center.Y, NPCID.AncientLight, 0, 0f, (Main.rand.NextFloat() - 0.5f) * 0.3f * 6.28318548202515f / 60f, vel.X, vel.Y);
                                                        if (n < 200)
                                                        {
                                                            Main.npc[n].velocity = vel;
                                                            Main.npc[n].netUpdate = true;
                                                            if (Main.netMode == 2)
                                                                NetMessage.SendData(23, -1, -1, null, n);
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        default: //phantasmal eye rings
                                            if (Main.netMode != 1)
                                            {
                                                const int max = 6;
                                                const int speed = 9;
                                                const float rotationModifier = 0.5f;
                                                int damage = (int)(40 * (1 + FargoSoulsWorld.MoonlordCount * .0125));
                                                float rotation = 2f * (float)Math.PI / max;
                                                Vector2 vel = Vector2.UnitY * speed;
                                                int type = mod.ProjectileType("MutantSphereRing");
                                                for (int i = 0; i < max; i++)
                                                {
                                                    vel = vel.RotatedBy(rotation);
                                                    Projectile.NewProjectile(npc.Center, vel, type, damage, 0f, Main.myPlayer, rotationModifier, speed);
                                                    Projectile.NewProjectile(npc.Center, vel, type, damage, 0f, Main.myPlayer, -rotationModifier, speed);
                                                }
                                                Main.PlaySound(SoundID.Item84, npc.Center);
                                            }
                                            break;
                                    }
                                }
                            }

                            /*if (--Counter2 < 0)
                            {
                                Counter2 = 600;
                                Main.PlaySound(15, (int)npc.Center.X, (int)npc.Center.Y, 0);
                                if (Main.netMode != 1 && npc.HasPlayerTarget)
                                {
                                    for (int i = 0; i < 3; i++)
                                    {
                                        NPC bodyPart = Main.npc[(int)npc.localAI[i]];
                                        if (bodyPart.active)
                                        {
                                            bodyPart.localAI[0] = (Main.player[npc.target].Center - bodyPart.Center).ToRotation();
                                            Vector2 speed = Vector2.UnitX.RotatedBy(bodyPart.localAI[0]);
                                            Projectile.NewProjectile(bodyPart.Center, speed, mod.ProjectileType("PhantasmalDeathrayMLSmall"), 0, 0f, Main.myPlayer, 0f, bodyPart.whoAmI);
                                        }
                                    }
                                }
                            }
                            else if (Counter2 == 540)
                            {
                                if (Main.netMode != 1)
                                {
                                    for (int i = 0; i < 3; i++)
                                    {
                                        NPC bodyPart = Main.npc[(int)npc.localAI[i]];
                                        if (bodyPart.active)
                                        {
                                            Vector2 speed = Vector2.UnitX.RotatedBy(bodyPart.localAI[0]);
                                            int damage = (int)(75 * (1 + FargoSoulsWorld.MoonlordCount * .0125));
                                            Projectile.NewProjectile(bodyPart.Center, speed, mod.ProjectileType("PhantasmalDeathrayML"), damage, 0f, Main.myPlayer, 0f, bodyPart.whoAmI);
                                        }
                                    }
                                }
                            }*/
                        }

                        if (npc.ai[0] == 2f) //moon lord is dead
                        {
                            if (!masoBool[1]) //check once when dead
                            {
                                masoBool[1] = true;
                                //stop all attacks (and become intangible lol) after i die
                                if (Main.netMode != 1 && NPC.CountNPCS(NPCID.MoonLordCore) == 1)
                                {
                                    masoStateML = 4;
                                    if (Main.netMode == 2) //sync damage phase with clients
                                    {
                                        var netMessage = mod.GetPacket();
                                        netMessage.Write((byte)4);
                                        netMessage.Write((byte)npc.whoAmI);
                                        netMessage.Write(Counter);
                                        netMessage.Write(masoStateML);
                                        netMessage.Send();
                                    }
                                }
                            }
                            Counter = 0;
                            Counter2 = 600;
                            Timer = 0;
                        }
                        else //moon lord isn't dead
                        {
                            if (++Counter > 1800)
                            {
                                Counter = 0;
                                if (Main.netMode != 1)
                                {
                                    if (++masoStateML > 4)
                                        masoStateML = 0;
                                    if (Main.netMode == 2) //sync damage phase with clients
                                    {
                                        var netMessage = mod.GetPacket();
                                        netMessage.Write((byte)4);
                                        netMessage.Write((byte)npc.whoAmI);
                                        netMessage.Write(Counter);
                                        netMessage.Write(masoStateML);
                                        netMessage.Send();
                                    }
                                }
                            }
                        }

                        switch (masoStateML)
                        {
                            case 0: Main.monolithType = 3; break;
                            case 1: Main.monolithType = 0; break;
                            case 2: Main.monolithType = 1; break;
                            case 3: Main.monolithType = 2; break;
                            default: break;
                        }
                        break;

                    case NPCID.Splinterling:
                        Counter++;
                        if (Counter >= 60)
                        {
                            Counter = 0;
                            if (Main.netMode != 1)
                                Projectile.NewProjectile(npc.Center.X, npc.Center.Y, Main.rand.Next(-3, 4), Main.rand.Next(-5, 0),
                                    Main.rand.Next(326, 329), npc.damage / 4, 0f, Main.myPlayer);
                        }
                        break;

                    case NPCID.Golem:
                        /*if (npc.ai[0] == 0f && npc.velocity.Y == 0f) //manipulating golem jump ai
                        {
                            if (npc.ai[1] > 0f)
                            {
                                npc.ai[1] += 5f; //count up to initiate jump faster
                            }
                            else
                            {
                                float threshold = -2f - (float)Math.Round(18f * npc.life / npc.lifeMax);

                                if (npc.ai[1] < threshold) //jump activates at npc.ai[1] == -1
                                    npc.ai[1] = threshold;
                            }
                        }*/

                        if (Main.player[Main.myPlayer].active && Main.player[Main.myPlayer].Distance(npc.Center) < 2000)
                            Main.player[Main.myPlayer].AddBuff(mod.BuffType("LowGround"), 2);

                        if (!masoBool[3])
                        {
                            npc.position.X += npc.velocity.X / 2f;
                            if (npc.velocity.Y < 0)
                            {
                                npc.position.Y += npc.velocity.Y * 0.5f;
                                if (npc.velocity.Y > -2)
                                    npc.velocity.Y = 20;
                            }
                        }

                        if (masoBool[0])
                        {
                            if (npc.velocity.Y == 0f)
                            {
                                masoBool[0] = false;
                                masoBool[3] = Main.tile[(int)npc.Center.X / 16, (int)npc.Center.Y / 16] != null &&
                                    Main.tile[(int)npc.Center.X / 16, (int)npc.Center.Y / 16].wall == WallID.LihzahrdBrickUnsafe;

                                if (Main.netMode != 1) //landing attacks
                                {
                                    Vector2 spawnPos = new Vector2(npc.position.X, npc.Center.Y);
                                    if (masoBool[3]) //in temple
                                    {
                                        Counter++;
                                        if (Counter == 1) //plant geysers
                                        {
                                            spawnPos.X -= npc.width * 7;
                                            for (int i = 0; i < 6; i++)
                                            {
                                                int tilePosX = (int)spawnPos.X / 16 + npc.width * i * 3 / 16;
                                                int tilePosY = (int)spawnPos.Y / 16;// + 1;

                                                /*if (Main.tile[tilePosX, tilePosY] == null)
                                                    Main.tile[tilePosX, tilePosY] = new Tile();

                                                while (!(Main.tile[tilePosX, tilePosY].nactive() && Main.tileSolid[(int)Main.tile[tilePosX, tilePosY].type]))
                                                {
                                                    tilePosY++;
                                                    if (Main.tile[tilePosX, tilePosY] == null)
                                                        Main.tile[tilePosX, tilePosY] = new Tile();
                                                }*/

                                                Projectile.NewProjectile(tilePosX * 16 + 8, tilePosY * 16 + 8, 0f, 0f, mod.ProjectileType("GolemGeyser"), npc.damage / 5, 0f, Main.myPlayer, npc.whoAmI);
                                            }
                                        }
                                        else if (Counter == 2) //rocks fall
                                        {
                                            Counter = 0;
                                            if (npc.HasPlayerTarget)
                                            {
                                                for (int i = -2; i <= 2; i++)
                                                {
                                                    int tilePosX = (int)Main.player[npc.target].Center.X / 16;
                                                    int tilePosY = (int)Main.player[npc.target].Center.Y / 16;// + 1;
                                                    tilePosX += 4 * i;

                                                    if (Main.tile[tilePosX, tilePosY] == null)
                                                        Main.tile[tilePosX, tilePosY] = new Tile();

                                                    while (!(Main.tile[tilePosX, tilePosY].nactive() && Main.tileSolid[Main.tile[tilePosX, tilePosY].type]))
                                                    {
                                                        tilePosY--;
                                                        if (Main.tile[tilePosX, tilePosY] == null)
                                                            Main.tile[tilePosX, tilePosY] = new Tile();
                                                    }

                                                    Vector2 spawn = new Vector2(tilePosX * 16 + 8, tilePosY * 16 + 8);
                                                    Projectile.NewProjectile(spawn, Vector2.Zero, mod.ProjectileType("GolemBoulder"), npc.damage / 5, 0f, Main.myPlayer);
                                                }
                                            }
                                        }
                                    }
                                    else //outside temple
                                    {
                                        spawnPos.X -= npc.width * 7;
                                        for (int i = 0; i < 6; i++)
                                        {
                                            int tilePosX = (int)spawnPos.X / 16 + npc.width * i * 3 / 16;
                                            int tilePosY = (int)spawnPos.Y / 16;// + 1;

                                            if (Main.tile[tilePosX, tilePosY] == null)
                                                Main.tile[tilePosX, tilePosY] = new Tile();

                                            while (!(Main.tile[tilePosX, tilePosY].nactive() && Main.tileSolid[(int)Main.tile[tilePosX, tilePosY].type]))
                                            {
                                                tilePosY++;
                                                if (Main.tile[tilePosX, tilePosY] == null)
                                                    Main.tile[tilePosX, tilePosY] = new Tile();
                                            }

                                            if (npc.HasPlayerTarget && Main.player[npc.target].position.Y > tilePosY * 16)
                                            {
                                                Projectile.NewProjectile(tilePosX * 16 + 8, tilePosY * 16 + 8, 6.3f, 6.3f,
                                                    ProjectileID.FlamesTrap, npc.damage / 5, 0f, Main.myPlayer);
                                                Projectile.NewProjectile(tilePosX * 16 + 8, tilePosY * 16 + 8, -6.3f, 6.3f,
                                                    ProjectileID.FlamesTrap, npc.damage / 5, 0f, Main.myPlayer);
                                            }

                                            Projectile.NewProjectile(tilePosX * 16 + 8, tilePosY * 16 + 8, 0f, -8f, ProjectileID.GeyserTrap, npc.damage / 5, 0f, Main.myPlayer);

                                            Projectile.NewProjectile(tilePosX * 16 + 8, tilePosY * 16 + 8 - 640, 0f, -8f, ProjectileID.GeyserTrap, npc.damage / 5, 0f, Main.myPlayer);
                                            Projectile.NewProjectile(tilePosX * 16 + 8, tilePosY * 16 + 8 - 640, 0f, 8f, ProjectileID.GeyserTrap, npc.damage / 5, 0f, Main.myPlayer);
                                        }
                                        if (npc.HasPlayerTarget)
                                        {
                                            for (int i = -2; i <= 2; i++)
                                            {
                                                int tilePosX = (int)Main.player[npc.target].Center.X / 16;
                                                int tilePosY = (int)Main.player[npc.target].Center.Y / 16;// + 1;
                                                tilePosX += 4 * i;

                                                if (Main.tile[tilePosX, tilePosY] == null)
                                                    Main.tile[tilePosX, tilePosY] = new Tile();

                                                for (int j = 0; j < 30; j++)
                                                {
                                                    if (Main.tile[tilePosX, tilePosY].nactive() && Main.tileSolid[Main.tile[tilePosX, tilePosY].type])
                                                        break;
                                                    tilePosY--;
                                                    if (Main.tile[tilePosX, tilePosY] == null)
                                                        Main.tile[tilePosX, tilePosY] = new Tile();
                                                }

                                                Vector2 spawn = new Vector2(tilePosX * 16 + 8, tilePosY * 16 + 8);
                                                Projectile.NewProjectile(spawn, Vector2.Zero, mod.ProjectileType("GolemBoulder"), npc.damage / 5, 0f, Main.myPlayer);
                                            }
                                        }
                                    }

                                    //golem's anti-air fireball spray (whenever he lands while player is below)
                                    /*if (npc.HasPlayerTarget && Main.player[npc.target].position.Y > npc.position.Y + npc.height)
                                    {
                                        float gravity = 0.2f; //shoot down
                                        const float time = 60f;
                                        Vector2 distance = Main.player[npc.target].Center - npc.Center;
                                        distance += Main.player[npc.target].velocity * 45f;
                                        distance.X = distance.X / time;
                                        distance.Y = distance.Y / time - 0.5f * gravity * time;
                                        if (Math.Sign(distance.Y) != Math.Sign(gravity))
                                            distance.Y = 0f; //cannot arc shots to hit someone on the same elevation
                                        int max = masoBool[3] ? 1 : 3;
                                        for (int i = -max; i <= max; i++)
                                        {
                                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y, distance.X + i * 1.5f, distance.Y,
                                                mod.ProjectileType("GolemFireball"), npc.damage / 5, 0f, Main.myPlayer, gravity, 0f);
                                        }
                                    }*/
                                }
                            }
                        }
                        else if (npc.velocity.Y > 0)
                        {
                            masoBool[0] = true;
                        }

                        if (++Counter2 >= 540 && npc.velocity.Y == 0) //spray spiky balls, only when on ground
                        {
                            Counter2 = 0;
                            if (Main.tile[(int)npc.Center.X / 16, (int)npc.Center.Y / 16] != null && //in temple
                                Main.tile[(int)npc.Center.X / 16, (int)npc.Center.Y / 16].wall == WallID.LihzahrdBrickUnsafe)
                            {
                                for (int i = 0; i < 8; i++)
                                    Projectile.NewProjectile(npc.position.X + Main.rand.Next(npc.width), npc.position.Y + Main.rand.Next(npc.height),
                                        Main.rand.NextFloat(-0.1f, 0.1f), Main.rand.NextFloat(-10, -6), ProjectileID.SpikyBallTrap, npc.damage / 5, 0f, Main.myPlayer);
                            }
                            else //outside temple
                            {
                                for (int i = 0; i < 16; i++)
                                    Projectile.NewProjectile(npc.position.X + Main.rand.Next(npc.width), npc.position.Y + Main.rand.Next(npc.height),
                                        Main.rand.NextFloat(-1f, 1f), Main.rand.Next(-20, -9), ProjectileID.SpikyBallTrap, npc.damage / 4, 0f, Main.myPlayer);
                            }
                        }

                        /*Counter2++;
                        if (Counter2 > 240) //golem's anti-air fireball spray (when player is above)
                        {
                            Counter2 = 0;
                            if (npc.HasPlayerTarget && Main.player[npc.target].position.Y < npc.position.Y
                                && Main.netMode != 1) //shoutouts to arterius
                            {
                                bool inTemple = Main.tile[(int)npc.Center.X / 16, (int)npc.Center.Y / 16] != null && //in temple
                                    Main.tile[(int)npc.Center.X / 16, (int)npc.Center.Y / 16].wall == WallID.LihzahrdBrickUnsafe;

                                float gravity = -0.2f; //normally floats up
                                //if (Main.player[npc.target].position.Y > npc.position.Y + npc.height) gravity *= -1f; //aim down if player below golem
                                const float time = 60f;
                                Vector2 distance = Main.player[npc.target].Center - npc.Center;
                                distance += Main.player[npc.target].velocity * 45f;
                                distance.X = distance.X / time;
                                distance.Y = distance.Y / time - 0.5f * gravity * time;
                                if (Math.Sign(distance.Y) != Math.Sign(gravity))
                                    distance.Y = 0f; //cannot arc shots to hit someone on the same elevation
                                int max = inTemple ? 1 : 3;
                                for (int i = -max; i <= max; i++)
                                {
                                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, distance.X + i, distance.Y,
                                        mod.ProjectileType("GolemFireball"), npc.damage / 5, 0f, Main.myPlayer, gravity, 0f);
                                }
                            }
                        }*/

                        if (Fargowiltas.Instance.CalamityLoaded && Revengeance)
                        {
                            if (masoBool[1])
                                npc.dontTakeDamage = false;
                            else
                                masoBool[1] = !NPC.AnyNPCs(NPCID.GolemHead);
                        }

                        if (!npc.dontTakeDamage)
                        {
                            npc.life += 4; //healing stuff
                            if (npc.life > npc.lifeMax)
                                npc.life = npc.lifeMax;
                            Timer++;
                            if (Timer >= 75)
                            {
                                Timer = Main.rand.Next(30);
                                CombatText.NewText(npc.Hitbox, CombatText.HealLife, 240);
                            }
                        }
                        break;

                    case NPCID.GolemFistLeft:
                    case NPCID.GolemFistRight:
                        if (npc.buffType[0] != 0)
                        {
                            npc.buffImmune[npc.buffType[0]] = true;
                            npc.DelBuff(0);
                        }

                        if (npc.ai[0] == 0f && masoBool[0] && Framing.GetTileSafely(npc.Center).wall != WallID.LihzahrdBrickUnsafe)
                        {
                            masoBool[0] = false;
                            if (Main.netMode != 1)
                                Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("FuseBomb"), npc.damage / 4, 0f, Main.myPlayer);
                        }
                        masoBool[0] = npc.ai[0] != 0f;

                        if (npc.velocity.Length() > 14)
                            npc.position -= Vector2.Normalize(npc.velocity) * (npc.velocity.Length() - 14);

                        if (npc.life < npc.lifeMax / 2 && NPC.golemBoss > -1 && NPC.golemBoss < 200 && Main.npc[NPC.golemBoss].active && Main.npc[NPC.golemBoss].type == NPCID.Golem)
                        {
                            npc.life = npc.lifeMax; //fully heal when below half health and golem still alive
                            Timer = 75; //immediately display heal
                        }
                        npc.life += 167;
                        if (npc.life > npc.lifeMax)
                            npc.life = npc.lifeMax;
                        Timer++;
                        if (Timer >= 75)
                        {
                            Timer = Main.rand.Next(30);
                            CombatText.NewText(npc.Hitbox, CombatText.HealLife, 9999);
                        }
                        break;

                    case NPCID.GolemHeadFree:
                        if (!masoBool[0]) //default mode
                        {
                            npc.position += npc.velocity * 0.25f;
                            npc.position.Y += npc.velocity.Y * 0.25f;

                            if (!npc.noTileCollide && npc.HasPlayerTarget && Collision.SolidCollision(npc.position, npc.width, npc.height)) //unstick from walls
                                npc.position += npc.DirectionTo(Main.player[npc.target].Center) * 4;

                            if (++Counter > 540)
                            {
                                Counter = 0;
                                Counter2 = 0;
                                masoBool[0] = true;
                                masoBool[2] = Framing.GetTileSafely(npc.Center).wall == WallID.LihzahrdBrickUnsafe;
                                npc.netUpdate = true;
                                if (Main.netMode == 2)
                                    NetUpdateMaso(npc.whoAmI);
                            }
                        }
                        else //deathray time
                        {
                            if (!(NPC.golemBoss > -1 && NPC.golemBoss < 200 && Main.npc[NPC.golemBoss].active && Main.npc[NPC.golemBoss].type == NPCID.Golem))
                            {
                                npc.StrikeNPCNoInteraction(9999, 0f, 0); //die if golem is dead
                                return false;
                            }

                            npc.noTileCollide = true;

                            const int fireTime = 120;
                            if (++Counter < fireTime) //move to above golem
                            {
                                Vector2 target = Main.npc[NPC.golemBoss].Center;
                                target.Y -= 250;
                                if (target.Y > Counter2) //counter2 stores lowest remembered golem position
                                    Counter2 = (int)target.Y;
                                target.Y = Counter2;
                                if (npc.HasPlayerTarget && Main.player[npc.target].position.Y < target.Y)
                                    target.Y = Main.player[npc.target].position.Y;
                                /*if (masoBool[2]) //in temple
                                {
                                    target.Y -= 250;
                                    if (target.Y > Counter2) //counter2 stores lowest remembered golem position
                                        Counter2 = (int)target.Y;
                                    target.Y = Counter2;
                                }
                                else if (npc.HasPlayerTarget)
                                {
                                    target.Y = Main.player[npc.target].Center.Y - 250;
                                }*/
                                npc.velocity = (target - npc.Center) / 30;
                            }
                            else if (Counter == fireTime) //fire deathray
                            {
                                npc.velocity = Vector2.Zero;
                                if (npc.HasPlayerTarget) //stores if player is on head's left at this moment
                                    masoBool[1] = Main.player[npc.target].Center.X < npc.Center.X;
                                npc.netUpdate = true;
                                if (Main.netMode != 1)
                                    Projectile.NewProjectile(npc.Center, Vector2.UnitY, mod.ProjectileType("PhantasmalDeathrayGolem"), npc.damage / 4, 0f, Main.myPlayer, 0f, npc.whoAmI);
                            }
                            else if (Counter < fireTime + 150)
                            {
                                npc.velocity.X += masoBool[1] ? -.18f : .18f;
                                
                                Tile tile = Framing.GetTileSafely(npc.Center); //stop if reached a wall, but only 1sec after started firing
                                if (Counter > fireTime + 60 & tile.nactive() && tile.type == TileID.LihzahrdBrick && tile.wall == WallID.LihzahrdBrickUnsafe)
                                {
                                    npc.velocity = Vector2.Zero;
                                    npc.netUpdate = true;
                                    Counter = 0;
                                    Counter2 = 0;
                                    masoBool[0] = false;
                                }
                            }
                            else
                            {
                                npc.velocity = Vector2.Zero;
                                npc.netUpdate = true;
                                Counter = 0;
                                Counter2 = 0;
                                masoBool[0] = false;
                            }

                            if (!masoBool[0] && Main.netMode != 1) //spray overhead lasers after dash
                            {
                                bool inTemple = Framing.GetTileSafely(npc.Center).wall == WallID.LihzahrdBrickUnsafe;
                                int max = inTemple ? 6 : 10;
                                int speed = inTemple ? -6 : -11;
                                for (int i = -max; i <= max; i++)
                                {
                                    int p = Projectile.NewProjectile(npc.Center, speed * Vector2.UnitY.RotatedBy(Math.PI / 2 / max * i), ProjectileID.EyeBeam, npc.damage / 5, 0f, Main.myPlayer);
                                    if (p != Main.maxProjectiles)
                                        Main.projectile[p].timeLeft = 1200;
                                }
                            }

                            if (npc.netUpdate)
                            {
                                npc.netUpdate = false;
                                if (Main.netMode == 2)
                                    NetUpdateMaso(npc.whoAmI);
                            }
                            return false;
                        }
                        break;

                    case NPCID.GolemHead:
                        npc.dontTakeDamage = false;
                        npc.life += 4;
                        if (npc.life > npc.lifeMax)
                            npc.life = npc.lifeMax;

                        Timer++;
                        if (Timer >= 75)
                        {
                            Timer = Main.rand.Next(30);
                            CombatText.NewText(npc.Hitbox, CombatText.HealLife, 240);
                        }
                        break;

                    case NPCID.FlyingSnake:
                        if (masoBool[0]) //after reviving
                        {
                            if (npc.buffType[0] != 0)
                                npc.DelBuff(0);
                            npc.position += npc.velocity;
                            npc.knockBackResist = 0f;
                            //npc.damage = npc.defDamage * 3 / 2;
                            SharkCount = 1;
                        }
                        break;

                    case NPCID.Lihzahrd:
                        Counter++;
                        if (Counter >= 200)
                        {
                            Counter = 0;
                            int t = npc.HasPlayerTarget ? npc.target : npc.FindClosestPlayer();
                            if (t != -1)
                            {
                                Player player = Main.player[t];
                                if (player.active && Main.netMode != 1)
                                {
                                    Vector2 velocity = player.Center - npc.Center;
                                    velocity.Normalize();
                                    velocity *= 12f;
                                    Projectile.NewProjectile(npc.Center, velocity, ProjectileID.PoisonDartTrap, 30, 0f, Main.myPlayer);
                                }
                            }
                        }
                        break;
                    case NPCID.LihzahrdCrawler:
                        if (++Counter2 > 30)
                        {
                            Counter2 = -90;
                            if (npc.HasPlayerTarget && Main.netMode != 1)
                            {
                                Vector2 vel = npc.DirectionTo(Main.player[npc.target].Center) * 10f;
                                Projectile.NewProjectile(npc.Center, vel, ProjectileID.Fireball, npc.damage / 5, 0f, Main.myPlayer);
                            }
                        }
                        goto case NPCID.Lihzahrd;

                    case NPCID.StardustCellSmall:
                        if (npc.ai[0] >= 240f)
                        {
                            if (Main.netMode == 1)
                                break;

                            int newType;
                            switch (Main.rand.Next(4))
                            {
                                case 0: newType = NPCID.StardustJellyfishBig; break;
                                case 1: newType = NPCID.StardustSpiderBig; break;
                                case 2: newType = NPCID.StardustWormHead; break;
                                case 3: newType = NPCID.StardustCellBig; break;
                                default: newType = NPCID.StardustCellBig; break;
                            }

                            npc.Transform(newType);
                        }
                        break;

                    case NPCID.Pumpking:
                        if (++Timer >= 12)
                        {
                            Timer = 0;
                            int t = npc.HasPlayerTarget ? npc.target : npc.FindClosestPlayer();
                            if (t != -1)
                            {
                                Player player = Main.player[t];
                                Vector2 distance = player.Center - npc.Center;
                                if (Math.Abs(distance.X) < npc.width && Main.netMode != 1) //flame rain if player roughly below me
                                    Projectile.NewProjectile(npc.Center.X, npc.position.Y, Main.rand.Next(-3, 4), Main.rand.Next(-4, 0), Main.rand.Next(326, 329), npc.damage / 5, 0f, Main.myPlayer);
                            }
                        }
                        break;

                    case NPCID.SolarCorite:
                        Aura(npc, 250, BuffID.Burning, false, DustID.Fire, true);
                        break;

                    case NPCID.NebulaHeadcrab:
                        Counter++;
                        if (Counter >= 300)
                        {
                            if (npc.ai[0] != 5f && npc.HasValidTarget && Main.netMode != 1) //if not latched on player
                                Projectile.NewProjectile(npc.Center, 6 * npc.DirectionTo(Main.player[npc.target].Center), ProjectileID.NebulaLaser, npc.damage / 4, 0, Main.myPlayer);
                            Counter = (short)Main.rand.Next(120);
                        }
                        break;

                    case NPCID.Plantera:
                        if (!masoBool[0]) //spawn protective crystal ring once
                        {
                            masoBool[0] = true;
                            if (Main.netMode != 1)
                            {
                                const int max = 5;
                                const float distance = 125f;
                                float rotation = 2f * (float)Math.PI / max;
                                for (int i = 0; i < max; i++)
                                {
                                    Vector2 spawnPos = npc.Center + new Vector2(distance, 0f).RotatedBy(rotation * i);
                                    int n = NPC.NewNPC((int)spawnPos.X, (int)spawnPos.Y, mod.NPCType("CrystalLeaf"), 0, npc.whoAmI, distance, 0, rotation * i);
                                    if (Main.netMode == 2 && n < 200)
                                        NetMessage.SendData(23, -1, -1, null, n);
                                }
                            }
                        }
                        if (!NPC.downedPlantBoss && Main.netMode != 1 && npc.HasPlayerTarget && !masoBool[1]
                            && Collision.CanHit(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))
                        {
                            masoBool[1] = true;
                            Item.NewItem(npc.Hitbox, mod.ItemType("PlanterasFruit"));
                        }

                        if (npc.life <= npc.lifeMax / 2) //phase 2
                        {
                            //Aura(npc, 700, mod.BuffType("IvyVenom"), true, 188);
                            masoBool[1] = true;
                            npc.defense += 21;

                            if (!masoBool[2])
                            {
                                masoBool[2] = true;
                                if (Main.netMode != 1)
                                {
                                    const int max = 10;
                                    const float distance = 250;
                                    float rotation = 2f * (float)Math.PI / max;
                                    for (int i = 0; i < max; i++)
                                    {
                                        Vector2 spawnPos = npc.Center + new Vector2(distance, 0f).RotatedBy(rotation * i);
                                        int n = NPC.NewNPC((int)spawnPos.X, (int)spawnPos.Y, mod.NPCType("CrystalLeaf"), 0, npc.whoAmI, distance, 0, rotation * i);
                                        if (Main.netMode == 2 && n < 200)
                                            NetMessage.SendData(23, -1, -1, null, n);
                                    }
                                }
                            }

                            Counter++;
                            if (Counter >= 30)
                            {
                                Counter = 0;

                                int t = npc.HasPlayerTarget ? npc.target : npc.FindClosestPlayer();
                                if (t != -1 && Main.netMode != 1)
                                {
                                    int damage = 22;
                                    int type = ProjectileID.SeedPlantera;

                                    if (Main.rand.Next(2) == 0)
                                    {
                                        damage = 27;
                                        type = ProjectileID.PoisonSeedPlantera;
                                    }
                                    else if (Main.rand.Next(6) == 0)
                                    {
                                        damage = 31;
                                        type = ProjectileID.ThornBall;
                                    }

                                    if (!Main.player[t].ZoneJungle)
                                        damage = damage * 2;
                                    else if (Main.expertMode)
                                        damage = damage * 9 / 10;

                                    Vector2 velocity = Main.player[t].Center - npc.Center;
                                    velocity.Normalize();
                                    velocity *= Main.expertMode ? 17f : 15f;

                                    int p = Projectile.NewProjectile(npc.Center + velocity * 3f, velocity, type, damage, 0f, Main.myPlayer);
                                    if (type != ProjectileID.ThornBall)
                                        Main.projectile[p].timeLeft = 300;
                                }
                            }

                            if (++Timer > 90)
                            {
                                Timer = 0;
                                if (Main.netMode != 1)
                                    Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("DicerPlantera"), npc.damage / 5, 0f, Main.myPlayer, 120, 300);
                            }

                            /*Timer++;
                            if (Timer >= 300)
                            {
                                Timer = 0;

                                int tentaclesToSpawn = 12;
                                for (int i = 0; i < 200; i++)
                                {
                                    if (Main.npc[i].active && Main.npc[i].type == NPCID.PlanterasTentacle && Main.npc[i].ai[3] == 0f)
                                    {
                                        tentaclesToSpawn--;
                                    }
                                }

                                for (int i = 0; i < tentaclesToSpawn; i++)
                                {
                                    NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.PlanterasTentacle, npc.whoAmI);
                                }
                            }*/

                            SharkCount = 0;

                            if (npc.HasPlayerTarget)
                            {
                                if (Main.player[npc.target].venom)
                                {
                                    npc.defense *= 2;
                                    Counter++;
                                    SharkCount = 1;

                                    if (RegenTimer > 120)
                                        RegenTimer = 120;
                                }
                            }

                            if (RegenTimer <= 2 && npc.life + 1 + npc.lifeMax / 25 >= npc.lifeMax / 2)
                            {
                                npc.life = npc.lifeMax / 2;
                                npc.lifeRegen = 0;
                                RegenTimer = 2;
                            }
                        }
                        break;

                    case NPCID.IceQueen:
                        Counter++;

                        short countCap = 14;
                        if (npc.life < npc.lifeMax * 3 / 4)
                            countCap--;
                        if (npc.life < npc.lifeMax / 2)
                            countCap -= 2;
                        if (npc.life < npc.lifeMax / 4)
                            countCap -= 3;
                        if (npc.life < npc.lifeMax / 10)
                            countCap -= 4;

                        if (Counter > countCap)
                        {
                            Counter = 0;
                            if (++Counter2 > 25)
                            {
                                Counter2 = 0;
                                if (Main.netMode != 1)
                                {
                                    int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.Flocko);
                                    if (Main.netMode == 2)
                                        NetMessage.SendData(23, -1, -1, null, n);
                                }
                            }
                            Vector2 speed = new Vector2(Main.rand.Next(-1000, 1001), Main.rand.Next(-1000, 1001));
                            speed.Normalize();
                            speed *= 12f;
                            Vector2 spawn = npc.Center;
                            spawn.Y -= 20f;
                            spawn += speed * 4f;
                            if (Main.netMode != 1)
                                Projectile.NewProjectile(spawn, speed, ProjectileID.FrostShard, 30, 0f, Main.myPlayer);
                        }
                        break;

                    case NPCID.Eyezor:
                        Counter++;
                        if (Counter >= 8)
                        {
                            Counter = 0;
                            int t = npc.HasPlayerTarget ? npc.target : npc.FindClosestPlayer();
                            if (t != -1)
                            {
                                Player player = Main.player[t];
                                if (player.active)
                                {
                                    Vector2 velocity = player.Center - npc.Center;
                                    velocity.Normalize();
                                    velocity *= 4f;
                                    if (Main.netMode != 1)
                                        Projectile.NewProjectile(npc.Center, velocity, ProjectileID.EyeFire, npc.damage / 5, 0f, Main.myPlayer);
                                }
                            }
                        }
                        break;

                    case NPCID.SkeletronPrime:
                        primeBoss = npc.whoAmI;
                        npc.dontTakeDamage = !masoBool[0];

                        if (npc.ai[0] != 2f) //in phase 1
                        {
                            if (npc.life < npc.lifeMax * .75) //enter phase 2
                            {
                                npc.ai[0] = 2f;
                                npc.ai[3] = 0f;
                                npc.netUpdate = true;

                                if (Main.netMode != 1)
                                {
                                    if (!NPC.AnyNPCs(NPCID.PrimeLaser)) //revive all dead limbs
                                    {
                                        int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.PrimeLaser, npc.whoAmI, 1f, npc.whoAmI, 0f, 150f, npc.target);
                                        if (n != 200 && Main.netMode == 2)
                                            NetMessage.SendData(23, -1, -1, null, n);
                                    }
                                    if (!NPC.AnyNPCs(NPCID.PrimeSaw))
                                    {
                                        int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.PrimeSaw, npc.whoAmI, 1f, npc.whoAmI, 0f, 0f, npc.target);
                                        if (n != 200 && Main.netMode == 2)
                                            NetMessage.SendData(23, -1, -1, null, n);
                                    }
                                    if (!NPC.AnyNPCs(NPCID.PrimeCannon))
                                    {
                                        int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.PrimeCannon, npc.whoAmI, -1f, npc.whoAmI, 0f, 150f, npc.target);
                                        if (n != 200 && Main.netMode == 2)
                                            NetMessage.SendData(23, -1, -1, null, n);
                                    }
                                    if (!NPC.AnyNPCs(NPCID.PrimeVice))
                                    {
                                        int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.PrimeVice, npc.whoAmI, -1f, npc.whoAmI, 0f, 0f, npc.target);
                                        if (n != 200 && Main.netMode == 2)
                                            NetMessage.SendData(23, -1, -1, null, n);
                                    }
                                }

                                Main.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);
                                break;
                            }

                            if (npc.ai[0] != 1f) //limb is dead and needs reviving
                            {
                                npc.ai[3]++;
                                if (npc.ai[3] > 1800f) //revive a dead limb
                                {
                                    npc.ai[3] = 0;
                                    npc.netUpdate = true;
                                    if (Main.netMode != 1)
                                    {
                                        int n = 200;
                                        switch ((int)npc.ai[0])
                                        {
                                            case 3: //laser
                                                n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.PrimeLaser, npc.whoAmI, 1f, npc.whoAmI, 0f, 150f, npc.target);
                                                break;
                                            case 4: //cannon
                                                n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.PrimeCannon, npc.whoAmI, -1f, npc.whoAmI, 0f, 0f, npc.target);
                                                break;
                                            case 5: //saw
                                                n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.PrimeSaw, npc.whoAmI, 1f, npc.whoAmI, 0f, 0f, npc.target);
                                                break;
                                            case 6: //vice
                                                n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.PrimeVice, npc.whoAmI, -1f, npc.whoAmI, 0f, 150f, npc.target);
                                                break;
                                            default:
                                                break;
                                        }
                                        if (n < 200)
                                        {
                                            Main.npc[n].life = Main.npc[n].lifeMax / 4;
                                            Main.npc[n].netUpdate = true;
                                        }
                                    }

                                    if (!NPC.AnyNPCs(NPCID.PrimeLaser)) //look for any other dead limbs
                                        npc.ai[0] = 3f;
                                    else if (!NPC.AnyNPCs(NPCID.PrimeCannon))
                                        npc.ai[0] = 4f;
                                    else if (!NPC.AnyNPCs(NPCID.PrimeSaw))
                                        npc.ai[0] = 5f;
                                    else if (!NPC.AnyNPCs(NPCID.PrimeVice))
                                        npc.ai[0] = 6f;
                                    else
                                        npc.ai[0] = 1f;
                                }
                            }

                            if (++Timer >= 360)
                            {
                                Timer = 0;

                                if (npc.HasPlayerTarget) //skeleton commando rockets LUL
                                {
                                    Vector2 speed = Main.player[npc.target].Center - npc.Center;
                                    speed.X += Main.rand.Next(-20, 21);
                                    speed.Y += Main.rand.Next(-20, 21);
                                    speed.Normalize();

                                    int damage = npc.damage / 4;

                                    if (Main.netMode != 1)
                                    {
                                        Projectile.NewProjectile(npc.Center, 3f * speed, ProjectileID.RocketSkeleton, damage, 0f, Main.myPlayer);
                                        Projectile.NewProjectile(npc.Center, 3f * speed.RotatedBy(MathHelper.ToRadians(5f)), ProjectileID.RocketSkeleton, damage, 0f, Main.myPlayer);
                                        Projectile.NewProjectile(npc.Center, 3f * speed.RotatedBy(MathHelper.ToRadians(-5f)), ProjectileID.RocketSkeleton, damage, 0f, Main.myPlayer);
                                    }

                                    Main.PlaySound(SoundID.Item11, npc.Center);
                                }
                            }
                        }
                        else //in phase 2
                        {
                            npc.dontTakeDamage = false;

                            int timeToShoot = 300;

                            if (npc.ai[1] == 1f && npc.ai[2] > 2f) //spinning
                            {
                                timeToShoot = 60;
                                if (npc.HasValidTarget)
                                    npc.position += npc.DirectionTo(Main.player[npc.target].Center) * 5;
                            }
                            else if (npc.ai[1] != 2f) //not spinning
                            {
                                npc.position += npc.velocity / 3f;
                            }

                            if (++Timer >= timeToShoot)
                            {
                                Timer = 0;
                                if (npc.HasPlayerTarget) //skeleton commando rockets LUL
                                {
                                    Vector2 speed = Main.player[npc.target].Center - npc.Center;
                                    speed.Normalize();

                                    int damage = npc.defDamage * 2 / 7;

                                    if (Main.netMode != 1)
                                    {
                                        Projectile.NewProjectile(npc.Center, 4f * speed, ProjectileID.RocketSkeleton, damage, 0f, Main.myPlayer);
                                        Projectile.NewProjectile(npc.Center, 4f * speed.RotatedBy(MathHelper.ToRadians(3f)), ProjectileID.RocketSkeleton, damage, 0f, Main.myPlayer);
                                        Projectile.NewProjectile(npc.Center, 4f * speed.RotatedBy(MathHelper.ToRadians(-3f)), ProjectileID.RocketSkeleton, damage, 0f, Main.myPlayer);
                                    }

                                    Main.PlaySound(SoundID.Item11, npc.Center);
                                }
                            }

                            if (!masoBool[1] && npc.ai[3] >= 0f) //spawn 4 more limbs
                            {
                                npc.ai[3]++;
                                if (npc.ai[3] >= 180f)
                                {
                                    masoBool[1] = true;
                                    npc.ai[3] = -1f;
                                    npc.netUpdate = true;
                                    if (Main.netMode != 1)
                                    {
                                        foreach (NPC l in Main.npc.Where(l => l.active && l.ai[1] == npc.whoAmI))
                                        {
                                            switch (l.type) //my first four limbs become super mode
                                            {
                                                case NPCID.PrimeCannon:
                                                case NPCID.PrimeLaser:
                                                case NPCID.PrimeSaw:
                                                case NPCID.PrimeVice:
                                                    l.GetGlobalNPC<FargoSoulsGlobalNPC>().masoBool[1] = true;
                                                    l.GetGlobalNPC<FargoSoulsGlobalNPC>().NetUpdateMaso(l.whoAmI);
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                        //now spawn the other four
                                        int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.PrimeLaser, npc.whoAmI, -1f, npc.whoAmI, 0f, 150f, npc.target);
                                        if (n != 200 && Main.netMode == 2)
                                            NetMessage.SendData(23, -1, -1, null, n);
                                        n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.PrimeSaw, npc.whoAmI, -1f, npc.whoAmI, 0f, 0f, npc.target);
                                        if (n != 200 && Main.netMode == 2)
                                            NetMessage.SendData(23, -1, -1, null, n);
                                        n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.PrimeCannon, npc.whoAmI, 1f, npc.whoAmI, 0f, 150f, npc.target);
                                        if (n != 200 && Main.netMode == 2)
                                            NetMessage.SendData(23, -1, -1, null, n);
                                        n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.PrimeVice, npc.whoAmI, 1f, npc.whoAmI, 0f, 0f, npc.target);
                                        if (n != 200 && Main.netMode == 2)
                                            NetMessage.SendData(23, -1, -1, null, n);
                                    }
                                    Main.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);
                                }
                            }
                        }
                        break;

                    case NPCID.VortexHornetQueen:
                        Timer++;
                        if (Timer >= 180)
                        {
                            Timer = Main.rand.Next(90);
                            if (Main.netMode != 1)
                                Projectile.NewProjectile(npc.Center, Vector2.Zero, 578, 0, 0f, Main.myPlayer);
                        }
                        break;

                    case NPCID.SolarCrawltipedeTail:
                        Counter++;
                        if (Counter >= 4)
                        {
                            Counter = 0;
                            int t = npc.HasPlayerTarget ? npc.target : npc.FindClosestPlayer();
                            if (t != -1)
                            {
                                Vector2 distance = Main.player[t].Center - npc.Center;
                                if (distance.Length() < 400f)
                                {
                                    distance.Normalize();
                                    distance *= 6f;
                                    int p = Projectile.NewProjectile(npc.Center, distance, ProjectileID.FlamesTrap, npc.damage / 4, 0f, Main.myPlayer);
                                    Main.projectile[p].friendly = false;
                                    Main.PlaySound(SoundID.Item34, npc.Center);
                                }
                            }
                        }
                        break;

                    case NPCID.Nailhead:
                        Counter++;
                        if (Counter >= 90)
                        {
                            Counter = (short)Main.rand.Next(60);
                            if (Main.netMode != 1)
                            {
                                //npc entire block is fucked
                                int length = Main.rand.Next(3, 6);
                                int[] numArray = new int[length];
                                int maxValue = 0;
                                for (int index = 0; index < (int)byte.MaxValue; ++index)
                                {
                                    if (Main.player[index].active && !Main.player[index].dead && Collision.CanHitLine(npc.position, npc.width, npc.height, Main.player[index].position, Main.player[index].width, Main.player[index].height))
                                    {
                                        numArray[maxValue] = index;
                                        ++maxValue;
                                        if (maxValue == length)
                                            break;
                                    }
                                }
                                if (maxValue > 1)
                                {
                                    for (int index1 = 0; index1 < 100; ++index1)
                                    {
                                        int index2 = Main.rand.Next(maxValue);
                                        int index3 = index2;
                                        while (index3 == index2)
                                            index3 = Main.rand.Next(maxValue);
                                        int num1 = numArray[index2];
                                        numArray[index2] = numArray[index3];
                                        numArray[index3] = num1;
                                    }
                                }

                                Vector2 vector2_1 = new Vector2(-1f, -1f);

                                for (int index = 0; index < maxValue; ++index)
                                {
                                    Vector2 vector2_2 = Main.npc[numArray[index]].Center - npc.Center;
                                    vector2_2.Normalize();
                                    vector2_1 += vector2_2;
                                }

                                vector2_1.Normalize();

                                for (int index = 0; index < length; ++index)
                                {
                                    float num1 = Main.rand.Next(8, 13);
                                    Vector2 vector2_2 = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                                    vector2_2.Normalize();

                                    if (maxValue > 0)
                                    {
                                        vector2_2 += vector2_1;
                                        vector2_2.Normalize();
                                    }
                                    vector2_2 *= num1;

                                    if (maxValue > 0)
                                    {
                                        --maxValue;
                                        vector2_2 = Main.player[numArray[maxValue]].Center - npc.Center;
                                        vector2_2.Normalize();
                                        vector2_2 *= num1;
                                    }

                                    Projectile.NewProjectile(npc.Center.X, npc.position.Y + npc.width / 4f, vector2_2.X, vector2_2.Y, ProjectileID.Nail, (int)(npc.damage * 0.15), 1f);
                                }
                            }
                        }
                        break;

                    case NPCID.VortexRifleman: //default: if (npc.localAI[2] >= 360f + Main.rand.Next(360) && etc)
                        if (npc.localAI[2] >= 180f + Main.rand.Next(180) && npc.Distance(Main.player[npc.target].Center) < 400f && Math.Abs(npc.DirectionTo(Main.player[npc.target].Center).Y) < 0.5f && Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))
                        {
                            npc.localAI[2] = 0f;
                            Vector2 vector2_1 = npc.Center;
                            vector2_1.X += npc.direction * 30f;
                            vector2_1.Y += 2f;

                            Vector2 vec = npc.DirectionTo(Main.player[npc.target].Center) * 7f;
                            if (vec.HasNaNs())
                                vec = new Vector2(npc.direction * 8f, 0f);

                            if (Main.netMode != 1)
                            {
                                int Damage = Main.expertMode ? 50 : 75;
                                for (int index = 0; index < 4; ++index)
                                {
                                    Vector2 vector2_2 = vec + Utils.RandomVector2(Main.rand, -0.8f, 0.8f);
                                    Projectile.NewProjectile(vector2_1.X, vector2_1.Y, vector2_2.X, vector2_2.Y, mod.ProjectileType("StormDiverBullet"), Damage, 1f, Main.myPlayer);
                                }
                            }

                            Main.PlaySound(SoundID.Item36, npc.Center);
                        }
                        break;

                    case NPCID.ElfCopter:
                        if (npc.localAI[0] >= 14f)
                        {
                            npc.localAI[0] = 0f;
                            if (Main.netMode != 1)
                            {
                                float num8 = Main.player[npc.target].Center.X - npc.Center.X;
                                float num9 = Main.player[npc.target].Center.Y - npc.Center.Y;
                                float num10 = num8 + Main.rand.Next(-35, 36);
                                float num11 = num9 + Main.rand.Next(-35, 36);
                                float num12 = num10 * (1f + Main.rand.Next(-20, 21) * 0.015f);
                                float num13 = num11 * (1f + Main.rand.Next(-20, 21) * 0.015f);
                                float num14 = 10f / (float)Math.Sqrt(num12 * num12 + num13 * num13);
                                float num15 = num12 * num14;
                                float num16 = num13 * num14;
                                float SpeedX = num15 * (1f + Main.rand.Next(-20, 21) * 0.0125f);
                                float SpeedY = num16 * (1f + Main.rand.Next(-20, 21) * 0.0125f);
                                Projectile.NewProjectile(npc.Center.X, npc.Center.Y, SpeedX, SpeedY, mod.ProjectileType("ElfCopterBullet"), 32, 0f, Main.myPlayer);
                            }
                        }
                        break;

                    case NPCID.TheDestroyerBody:
                    case NPCID.TheDestroyerTail:
                        if (npc.realLife >= 0 && npc.realLife < 200 && Main.npc[npc.realLife].life > 0 && npc.life > 0)
                        {
                            npc.defense = npc.defDefense;

                            if (Main.npc[npc.realLife].GetGlobalNPC<FargoSoulsGlobalNPC>().masoBool[1]) //spinning
                            {
                                if (!masoBool[0])
                                    masoBool[0] = true;

                                Counter2 = 180;
                                Vector2 pivot = Main.npc[npc.realLife].Center;
                                pivot += Vector2.Normalize(Main.npc[npc.realLife].velocity.RotatedBy(Math.PI / 2)) * 600;
                                if (npc.Distance(pivot) < 600) //make sure body doesnt coil into the circling zone
                                    npc.Center = pivot + npc.DirectionFrom(pivot) * 600;

                                //enrage if player is outside the ring
                                if (Main.npc[npc.realLife].HasValidTarget && Main.npc[npc.realLife].GetGlobalNPC<FargoSoulsGlobalNPC>().Counter > 30 && Main.player[Main.npc[npc.realLife].target].Distance(pivot) > 600 && Main.netMode != 1 && Main.rand.Next(120) == 0)
                                {
                                    Vector2 distance = Main.player[npc.target].Center - npc.Center;
                                    distance.X += Main.rand.Next(-200, 201);
                                    distance.Y += Main.rand.Next(-200, 201);

                                    double angleModifier = MathHelper.ToRadians(10) * distance.Length() / 1200.0;
                                    distance.Normalize();
                                    distance *= Main.rand.NextFloat(20f, 30f);

                                    int type = mod.ProjectileType("DarkStar");
                                    int p = Projectile.NewProjectile(npc.Center, distance.RotatedBy(-angleModifier), type, npc.damage / 3, 0f, Main.myPlayer);
                                    if (p != Main.maxProjectiles)
                                        Main.projectile[p].timeLeft = 150;
                                    p = Projectile.NewProjectile(npc.Center, distance, type, npc.damage / 3, 0f, Main.myPlayer);
                                    if (p != Main.maxProjectiles)
                                        Main.projectile[p].timeLeft = 150;
                                    p = Projectile.NewProjectile(npc.Center, distance.RotatedBy(angleModifier), type, npc.damage / 3, 0f, Main.myPlayer);
                                    if (p != Main.maxProjectiles)
                                        Main.projectile[p].timeLeft = 150;
                                }
                            }

                            if (Counter2 > 0) //no lasers or stars while or shortly after spinning
                            {
                                Counter2--;
                                npc.defense = 9999; //boosted defense during this same duration
                                if (npc.localAI[0] > 1350)
                                    npc.localAI[0] = 1350;
                            }
                            else if (npc.ai[2] != 0)
                            {
                                npc.localAI[0] = 0f;
                                int cap = Main.npc[npc.realLife].lifeMax / Main.npc[npc.realLife].life;
                                if (cap > 70) //prevent meme scaling at super low life
                                    cap = 70;
                                Counter += Main.rand.Next(2 + cap) + 1;
                                if (Counter >= Main.rand.Next(1400, 26000))
                                {
                                    Counter = 0;
                                    if (Main.netMode != 1 && npc.HasPlayerTarget)
                                    {
                                        Vector2 distance = Main.player[npc.target].Center - npc.Center;
                                        double angleModifier = MathHelper.ToRadians(5) * distance.Length() / 1800.0;
                                        distance.Normalize();
                                        distance *= 8f;
                                        int type = mod.ProjectileType("DarkStar");
                                        Projectile.NewProjectile(npc.Center, distance.RotatedBy(-angleModifier), type, npc.damage / 5, 0f, Main.myPlayer);
                                        Projectile.NewProjectile(npc.Center, distance.RotatedBy(angleModifier), type, npc.damage / 5, 0f, Main.myPlayer);
                                    }
                                }
                            }
                        }
                        else if (Main.netMode != 1)
                        {
                            npc.life = 0;
                            if (Main.netMode == 2)
                                NetMessage.SendData(23, -1, -1, null, npc.whoAmI);
                            npc.active = false;
                            //npc.checkDead();
                            break;
                        }

                        if (npc.buffType[0] != 0)
                            npc.DelBuff(0);
                        break;

                    /*case NPCID.Probe:
                        if (npc.HasValidTarget && ++Counter > 120)
                        {
                            Counter = 0;
                            Vector2 distance = Main.player[npc.target].Center - npc.Center;
                            double angleModifier = MathHelper.ToRadians(5) * distance.Length() / 1800.0;
                            distance.Normalize();
                            distance *= 8f;
                            Projectile.NewProjectile(npc.Center, distance, mod.ProjectileType("DarkStar"), npc.damage / 4, 0f, Main.myPlayer);
                        }
                        break;*/

                    case NPCID.TacticalSkeleton: //num3 = 120, damage = 40/50, num8 = 0
                        if (npc.ai[2] > 0f && npc.ai[1] <= 65f)
                        {
                            if (Main.netMode != 1)
                            {
                                for (int index = 0; index < 6; ++index)
                                {
                                    float num6 = Main.player[npc.target].Center.X - npc.Center.X;
                                    float num10 = Main.player[npc.target].Center.Y - npc.Center.Y;
                                    float num11 = 11f / (float)Math.Sqrt(num6 * num6 + num10 * num10);
                                    float num12;
                                    float num18 = num12 = num6 + Main.rand.Next(-40, 41);
                                    float num19;
                                    float num20 = num19 = num10 + Main.rand.Next(-40, 41);
                                    float SpeedX = num18 * num11;
                                    float SpeedY = num20 * num11;
                                    int damage = Main.expertMode ? 40 : 50;
                                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, SpeedX, SpeedY, mod.ProjectileType("TacticalSkeletonBullet"), damage, 0f, Main.myPlayer);
                                }
                            }
                            Main.PlaySound(SoundID.Item38, npc.Center);
                            npc.ai[2] = 0f;
                            npc.ai[1] = 0f;
                            npc.ai[3] = 0f; //specific to me
                            npc.netUpdate = true;
                        }
                        break;

                    case NPCID.SkeletonSniper: //num3 = 200, num8 = 0
                        if (npc.ai[2] > 0f && npc.ai[1] <= 105f)
                        {
                            if (Main.netMode != 1)
                            {
                                Vector2 speed = Main.player[npc.target].Center - npc.Center;
                                speed.X += Main.rand.Next(-40, 41) * 0.2f;
                                speed.Y += Main.rand.Next(-40, 41) * 0.2f;
                                speed.Normalize();
                                speed *= 11f;

                                int damage = Main.expertMode ? 80 : 100;
                                Projectile.NewProjectile(npc.Center, speed, mod.ProjectileType("SniperBullet"), damage, 0f, Main.myPlayer);
                            }
                            Main.PlaySound(SoundID.Item40, npc.Center);
                            npc.ai[2] = 0f;
                            npc.ai[1] = 0f;
                            npc.netUpdate = true;
                        }
                        break;

                    case NPCID.SkeletonArcher: //damage = 28/35, ID.VenomArrow
                        if (npc.ai[2] > 0f && npc.ai[1] <= 40f)
                        {
                            if (Main.netMode != 1)
                            {
                                Vector2 speed = Main.player[npc.target].Center - npc.Center;
                                speed.Y -= Math.Abs(speed.X) * 0.075f; //account for gravity (default *0.1f)
                                speed.X += Main.rand.Next(-24, 25);
                                speed.Y += Main.rand.Next(-24, 25);
                                speed.Normalize();
                                speed *= 11f;

                                int damage = Main.expertMode ? 28 : 35;
                                Projectile.NewProjectile(npc.Center, speed, mod.ProjectileType("SkeletonArcherArrow"), damage, 0f, Main.myPlayer);
                            }
                            Main.PlaySound(SoundID.Item5, npc.Center);
                            npc.ai[2] = 0f;
                            npc.ai[1] = 0f;
                            npc.netUpdate = true;
                        }
                        break;

                    case NPCID.SkeletonCommando: //num3 = 90, num5 = 4f, damage = 48/60, ID.RocketSkeleton
                        if (npc.ai[2] > 0f && npc.ai[1] <= 50f)
                        {
                            if (Main.netMode != 1)
                            {
                                Vector2 speed = Main.player[npc.target].Center - npc.Center;
                                speed.X += Main.rand.Next(-20, 21);
                                speed.Y += Main.rand.Next(-20, 21);
                                speed.Normalize();

                                int damage = Main.expertMode ? 48 : 60;
                                Projectile.NewProjectile(npc.Center, 4f * speed, ProjectileID.RocketSkeleton, damage, 0f, Main.myPlayer);
                                Projectile.NewProjectile(npc.Center, 3f * speed.RotatedBy(MathHelper.ToRadians(10f)), ProjectileID.RocketSkeleton, damage, 0f, Main.myPlayer);
                                Projectile.NewProjectile(npc.Center, 3f * speed.RotatedBy(MathHelper.ToRadians(-10f)), ProjectileID.RocketSkeleton, damage, 0f, Main.myPlayer);
                            }
                            Main.PlaySound(SoundID.Item11, npc.Center);
                            npc.ai[2] = 0f;
                            npc.ai[1] = 0f;
                            npc.netUpdate = true;
                        }
                        break;

                    case NPCID.ElfArcher: //num3 = 110, damage = 36/45, tsunami
                        if (npc.ai[2] > 0f && npc.ai[1] <= 60f)
                        {
                            if (Main.netMode != 1)
                            {
                                Vector2 speed = Main.player[npc.target].Center - npc.Center;
                                speed.Y -= Math.Abs(speed.X) * 0.1f; //account for gravity
                                speed.X += Main.rand.Next(-20, 21);
                                speed.Y += Main.rand.Next(-20, 21);
                                speed.Normalize();
                                Vector2 spinningpoint = speed;
                                speed *= 8f;

                                int damage = Main.expertMode ? 36 : 45;

                                //tsunami code lol
                                float num3 = 0.3141593f;
                                int num4 = 5;
                                spinningpoint *= 40f;
                                bool flag4 = Collision.CanHit(npc.Center, 0, 0, npc.Center + spinningpoint, 0, 0);
                                for (int index1 = 0; index1 < num4; ++index1)
                                {
                                    float num8 = index1 - (num4 - 1f) / 2f;
                                    Vector2 vector2_5 = spinningpoint.RotatedBy(num3 * num8);
                                    if (!flag4)
                                        vector2_5 -= spinningpoint;
                                    int p = Projectile.NewProjectile(npc.Center + vector2_5, speed, mod.ProjectileType("ElfArcherArrow"), damage, 0f, Main.myPlayer);
                                    Main.projectile[p].noDropItem = true;
                                }
                            }
                            Main.PlaySound(SoundID.Item5, npc.Center);
                            npc.ai[2] = 0f;
                            npc.ai[1] = 0f;
                            npc.netUpdate = true;
                        }
                        break;

                    case NPCID.PirateCrossbower: //num3 = 80, num5 = 16f, num8 = Math.Abs(num7) * .08f, damage = 32/40, num12 = 800f?
                        if (npc.ai[2] > 0f && npc.ai[1] <= 45f)
                        {
                            if (Main.netMode != 1)
                            {
                                Vector2 speed = Main.player[npc.target].Center - npc.Center;
                                speed.X += Main.rand.Next(-20, 21);
                                speed.Y += Main.rand.Next(-20, 21);
                                speed.Normalize();
                                speed *= 11f;

                                int damage = Main.expertMode ? 32 : 40;
                                Projectile.NewProjectile(npc.Center, speed, mod.ProjectileType("PirateCrossbowerArrow"), damage, 0f, Main.myPlayer);
                            }
                            Main.PlaySound(SoundID.Item5, npc.Center);
                            npc.ai[2] = 0f;
                            npc.ai[1] = 0f;
                            npc.netUpdate = true;
                        }
                        break;

                    case NPCID.PirateDeadeye: //num3 = 40, num5 = 14f, num8 = 0f, damage = 20/25, num12 = 550f?
                        if (npc.ai[2] > 0f && npc.ai[1] <= 25f)
                        {
                            if (Main.netMode != 1)
                            {
                                Vector2 speed = Main.player[npc.target].Center - npc.Center;
                                speed.X += Main.rand.Next(-20, 21);
                                speed.Y += Main.rand.Next(-20, 21);
                                speed.Normalize();
                                speed *= 14f;

                                int damage = Main.expertMode ? 20 : 25;
                                Projectile.NewProjectile(npc.Center, speed, mod.ProjectileType("PirateDeadeyeBullet"), damage, 0f, Main.myPlayer);
                            }
                            Main.PlaySound(SoundID.Item11, npc.Center);
                            npc.ai[2] = 0f;
                            npc.ai[1] = 0f;
                            npc.netUpdate = true;
                        }
                        break;

                    case NPCID.PirateCaptain: //60 delay for cannonball, 8 for bullets
                        if (npc.ai[2] > 0f && npc.localAI[2] >= 20f && npc.ai[1] <= 30)
                        {
                            //npc.localAI[2]++;
                            if (Main.netMode != 1)
                            {
                                Vector2 speed = Main.player[npc.target].Center - npc.Center;
                                speed.Y -= Math.Abs(speed.X) * 0.2f; //account for gravity
                                speed.X += Main.rand.Next(-20, 21);
                                speed.Y += Main.rand.Next(-20, 21);
                                speed.Normalize();
                                speed *= 11f;
                                npc.localAI[2] = 0f;
                                for (int i = 0; i < 15; i++)
                                {
                                    Vector2 cannonSpeed = speed;
                                    cannonSpeed.X += Main.rand.Next(-10, 11) * 0.3f;
                                    cannonSpeed.Y += Main.rand.Next(-10, 11) * 0.3f;
                                    Projectile.NewProjectile(npc.Center, cannonSpeed, ProjectileID.CannonballHostile, Main.expertMode ? 80 : 100, 0f, Main.myPlayer);
                                }
                            }
                            //npc.ai[2] = 0f;
                            //npc.ai[1] = 0f;
                            npc.netUpdate = true;
                        }
                        break;

                    case NPCID.SolarGoop:
                        Counter++;
                        if (Counter >= 300)
                        {
                            npc.life = 0;
                            npc.checkDead();
                            npc.active = false;
                        }

                        if (npc.HasPlayerTarget)
                        {
                            Vector2 speed = Main.player[npc.target].Center - npc.Center;
                            speed.Normalize();
                            speed *= 12f;

                            npc.velocity.X += speed.X / 100f;

                            if (npc.velocity.Length() > 16f)
                            {
                                npc.velocity.Normalize();
                                npc.velocity *= 16f;
                            }
                        }
                        else
                        {
                            npc.TargetClosest(false);
                        }

                        npc.dontTakeDamage = true;
                        break;

                    case NPCID.BrainofCthulhu:
                        brainBoss = npc.whoAmI;
                        if (npc.alpha == 0)
                        {
                            npc.damage = npc.defDamage;
                        }
                        else
                        {
                            npc.damage = 0;
                            if (npc.ai[0] != -2 && npc.HasPlayerTarget && npc.Distance(Main.player[npc.target].Center) < 300) //stay at a minimum distance
                            {
                                npc.Center = Main.player[npc.target].Center + Main.player[npc.target].DirectionTo(npc.Center) * 300;
                            }
                        }

                        if (!npc.dontTakeDamage) //vulnerable
                        {
                            if (npc.buffType[0] != 0) //constant debuff cleanse
                            {
                                npc.buffImmune[npc.buffType[0]] = true;
                                npc.DelBuff(0);
                            }
                            if (!masoBool[0]) //spawn illusions
                            {
                                masoBool[0] = true;
                                if (Main.netMode != 1)
                                {
                                    bool recolor = SoulConfig.Instance.BossRecolors && FargoSoulsWorld.MasochistMode;
                                    int type = recolor ? mod.NPCType("BrainIllusion2") : mod.NPCType("BrainIllusion");
                                    int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, type, npc.whoAmI, npc.whoAmI, -1, 1);
                                    if (n != 200 && Main.netMode == 2)
                                        NetMessage.SendData(23, -1, -1, null, n);
                                    n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, type, npc.whoAmI, npc.whoAmI, 1, -1);
                                    if (n != 200 && Main.netMode == 2)
                                        NetMessage.SendData(23, -1, -1, null, n);
                                    n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, type, npc.whoAmI, npc.whoAmI, 1, 1);
                                    if (n != 200 && Main.netMode == 2)
                                        NetMessage.SendData(23, -1, -1, null, n);
                                    n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("BrainClone"), npc.whoAmI);
                                    if (n != 200 && Main.netMode == 2)
                                        NetMessage.SendData(23, -1, -1, null, n);
                                }
                            }
                            if (--Counter < 0) //confusion timer
                            {
                                Counter = 600;
                                Main.PlaySound(15, (int)npc.Center.X, (int)npc.Center.Y, 0);
                                for (int i = 0; i < 36; i++) //dust ring
                                {
                                    Vector2 vector6 = Vector2.UnitY * 12f;
                                    vector6 = vector6.RotatedBy((i - (36 / 2 - 1)) * 6.28318548f / 36) + npc.Center;
                                    Vector2 vector7 = vector6 - npc.Center;
                                    int d = Dust.NewDust(vector6 + vector7, 0, 0, 90, 0f, 0f, 0, default(Color), 3f);
                                    Main.dust[d].noGravity = true;
                                    Main.dust[d].velocity = vector7;
                                }
                            }
                            else if (Counter == 540) //inflict confusion after telegraph
                            {
                                Projectile.NewProjectile(npc.Center, new Vector2(-10, 0), mod.ProjectileType("BrainofConfusion"), 0, 0, Main.myPlayer);
                                if (npc.Distance(Main.player[Main.myPlayer].Center) < 3000)
                                    Main.player[Main.myPlayer].AddBuff(BuffID.Confused, Main.expertMode && Main.expertDebuffTime > 1 ? 150 : 300);

                                if (npc.HasValidTarget && Main.netMode != 1) //laser spreads from each illusion
                                {
                                    Vector2 offset = npc.Center - Main.player[npc.target].Center;

                                    const int degree = 10;

                                    Vector2 spawnPos = Main.player[npc.target].Center;
                                    spawnPos.X += offset.X;
                                    spawnPos.Y += offset.Y;
                                    for (int i = -1; i <= 1; i++)
                                        Projectile.NewProjectile(spawnPos, Main.player[npc.target].DirectionFrom(spawnPos).RotatedBy(MathHelper.ToRadians(degree) * i), mod.ProjectileType("DestroyerLaser"), npc.damage / 4, 0f, Main.myPlayer);

                                    spawnPos = Main.player[npc.target].Center;
                                    spawnPos.X += offset.X;
                                    spawnPos.Y -= offset.Y;
                                    for (int i = -1; i <= 1; i++)
                                        Projectile.NewProjectile(spawnPos, Main.player[npc.target].DirectionFrom(spawnPos).RotatedBy(MathHelper.ToRadians(degree) * i), mod.ProjectileType("DestroyerLaser"), npc.damage / 4, 0f, Main.myPlayer);

                                    spawnPos = Main.player[npc.target].Center;
                                    spawnPos.X -= offset.X;
                                    spawnPos.Y += offset.Y;
                                    for (int i = -1; i <= 1; i++)
                                        Projectile.NewProjectile(spawnPos, Main.player[npc.target].DirectionFrom(spawnPos).RotatedBy(MathHelper.ToRadians(degree) * i), mod.ProjectileType("DestroyerLaser"), npc.damage / 4, 0f, Main.myPlayer);

                                    spawnPos = Main.player[npc.target].Center;
                                    spawnPos.X -= offset.X;
                                    spawnPos.Y -= offset.Y;
                                    for (int i = -1; i <= 1; i++)
                                        Projectile.NewProjectile(spawnPos, Main.player[npc.target].DirectionFrom(spawnPos).RotatedBy(MathHelper.ToRadians(degree) * i), mod.ProjectileType("DestroyerLaser"), npc.damage / 4, 0f, Main.myPlayer);
                                }
                            }
                        }
                        break;

                    case NPCID.Creeper:
                        if (++Timer >= 600)
                        {
                            int count = NPC.CountNPCS(NPCID.Creeper) - 1;
                            Timer = (20 - count) * 29;
                            if (Timer < 0)
                                Timer = 0;

                            if (npc.HasPlayerTarget && Main.netMode != 1)
                            {
                                Vector2 speed = Main.player[npc.target].Center - npc.Center;
                                speed.Y -= Math.Abs(speed.X) * 0.1f; //account for gravity
                                speed.X += Main.rand.Next(-10, 11);
                                speed.Y += Main.rand.Next(-30, 21);
                                speed.Normalize();
                                speed *= 10f;
                                Projectile.NewProjectile(npc.Center, speed, ProjectileID.GoldenShowerHostile, npc.damage / 4, 0f, Main.myPlayer);
                            }

                            npc.netUpdate = true;
                        }
                        break;

                    case NPCID.Pixie:
                        if (npc.HasPlayerTarget)
                        {
                            if (npc.velocity.Y < 0f && npc.position.Y < Main.player[npc.target].position.Y)
                                npc.velocity.Y = 0f;
                            if (Vector2.Distance(Main.player[npc.target].Center, npc.Center) < 200)
                                Counter++;
                        }
                        if (Counter >= 60)
                        {
                            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Navi").WithVolume(1f).WithPitchVariance(.5f), npc.Center);
                            Counter = 0;
                        }
                        Aura(npc, 100, mod.BuffType("SqueakyToy"));
                        break;

                    case NPCID.Clown:
                        if (!masoBool[0]) //roar when spawn
                        {
                            masoBool[0] = true;
                            Main.PlaySound(15, npc.Center, 0);
                            if (Main.netMode == 0)
                                Main.NewText("A Clown has begun ticking!", 175, 75, 255);
                            else if (Main.netMode == 2)
                                NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("A Clown has begun ticking!"), new Color(175, 75, 255));
                        }

                        Counter++;
                        if (Counter >= 240)
                        {
                            Counter = 0;
                            SharkCount++;
                            if (SharkCount >= 5)
                            {
                                npc.life = 0;
                                Main.PlaySound(npc.DeathSound, npc.Center);
                                npc.active = false;

                                bool bossAlive = false;
                                for (int i = 0; i < 200; i++)
                                {
                                    if (Main.npc[i].boss)
                                    {
                                        bossAlive = true;
                                        break;
                                    }
                                }

                                if (Main.netMode != 1)
                                {
                                    if (bossAlive)
                                    {
                                        Projectile.NewProjectile(npc.Center, Vector2.Zero, ProjectileID.BouncyGrenade, 60, 8f, Main.myPlayer);
                                    }
                                    else
                                    {
                                        for (int i = 0; i < 100; i++)
                                        {
                                            int type = ProjectileID.Grenade;
                                            int damage = 250;
                                            float knockback = 8f;
                                            switch (Main.rand.Next(10))
                                            {
                                                case 0:
                                                case 1:
                                                case 2: type = ProjectileID.HappyBomb; damage = 100; break;
                                                case 3:
                                                case 4:
                                                case 5:
                                                case 6: type = ProjectileID.BouncyGrenade; damage = 60; break;
                                                case 7:
                                                case 8:
                                                case 9: type = ProjectileID.StickyGrenade; damage = 60; break;
                                            }

                                            int p = Projectile.NewProjectile(npc.position.X + Main.rand.Next(npc.width), npc.position.Y + Main.rand.Next(npc.height), Main.rand.Next(-1000, 1001) / 100f, Main.rand.Next(-2000, 101) / 100f, type, damage, knockback, Main.myPlayer);
                                            Main.projectile[p].timeLeft += Main.rand.Next(180);
                                        }

                                        Projectile.NewProjectile(npc.Center, Vector2.Zero, ProjectileID.BouncyBomb, 100, 8f, Main.myPlayer);
                                    }
                                }

                                if (Main.netMode == 0)
                                    Main.NewText("A Clown has exploded!", 175, 75, 255);
                                else if (Main.netMode == 2)
                                    NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("A Clown has exploded!"), new Color(175, 75, 255));
                            }
                        }
                        break;

                    case NPCID.Paladin:
                        if (masoBool[0]) //small paladin
                        {
                            if (!masoBool[1] && ++Counter > 15)
                            {
                                masoBool[1] = true;
                                if (Main.netMode == 2) //MP sync
                                {
                                    var netMessage = mod.GetPacket();
                                    netMessage.Write((byte)3);
                                    netMessage.Write((byte)npc.whoAmI);
                                    netMessage.Write(npc.lifeMax);
                                    netMessage.Write(npc.scale);
                                    netMessage.Send();
                                    npc.netUpdate = true;
                                }
                            }
                        }
                        Aura(npc, 800f, BuffID.BrokenArmor, false, 246, true);
                        foreach (NPC n in Main.npc.Where(n => n.active && !n.friendly && n.type != NPCID.Paladin && n.Distance(npc.Center) < 800f))
                        {
                            n.GetGlobalNPC<FargoSoulsGlobalNPC>().PaladinsShield = true;
                            if (Main.rand.Next(2) == 0)
                            {
                                int d = Dust.NewDust(n.position, n.width, n.height, 246, 0f, -1.5f, 0, new Color());
                                Main.dust[d].velocity *= 0.5f;
                                Main.dust[d].noLight = true;
                            }
                        }
                        break;

                    case NPCID.Mimic:
                    case NPCID.PresentMimic:
                        npc.dontTakeDamage = false;
                        if (npc.justHit && Main.hardMode)
                            Counter = 20;
                        if (Counter != 0)
                        {
                            Counter--;
                            npc.dontTakeDamage = true;
                        }
                        break;
                        
                    case NPCID.PrimeSaw:
                        if (!masoBool[1] && ++Counter2 >= 2) //flamethrower in same direction that saw is pointing
                        {
                            Counter2 = 0;
                            Vector2 velocity = new Vector2(7f, 0f).RotatedBy(npc.rotation + Math.PI / 2.0);
                            if (Main.netMode != 1)
                                Projectile.NewProjectile(npc.Center, velocity, ProjectileID.FlamesTrap, npc.damage / 4, 0f, Main.myPlayer);
                            Main.PlaySound(SoundID.Item34, npc.Center);
                        }
                        goto case NPCID.PrimeLaser;

                    case NPCID.PrimeCannon:
                        if (npc.ai[2] != 0f)
                        {
                            if (npc.localAI[0] > 30f)
                            {
                                npc.localAI[0] = 0f;
                                if (Main.netMode != 1)
                                {
                                    Vector2 speed = new Vector2(16f, 0f).RotatedBy(npc.rotation + Math.PI / 2);
                                    Projectile.NewProjectile(npc.Center, speed, mod.ProjectileType("DarkStar"), npc.damage / 5, 0f, Main.myPlayer);
                                }
                            }
                        }
                        else
                        {
                            npc.localAI[0]++;
                            npc.ai[3]++;
                        }
                        goto case NPCID.PrimeLaser;

                    case NPCID.PrimeLaser:
                    case NPCID.PrimeVice:
                        if (!masoBool[0])
                        {
                            RegenTimer = 2;
                            if (Main.npc[(int)npc.ai[1]].type == NPCID.SkeletronPrime && Main.npc[(int)npc.ai[1]].ai[0] == 2f)
                            {
                                masoBool[0] = true;
                                npc.defDamage = npc.defDamage * 4 / 3;
                                npc.damage = npc.defDamage;
                                int heal = npc.lifeMax - npc.life;
                                npc.life = npc.lifeMax;
                                if (heal > 0)
                                    CombatText.NewText(npc.Hitbox, CombatText.HealLife, heal);
                                npc.netUpdate = true;
                            }
                        }
                        else
                        {
                            npc.dontTakeDamage = !(Fargowiltas.Instance.CalamityLoaded && Revengeance);
                            int ai1 = (int)npc.ai[1];
                            if (!(ai1 > -1 && ai1 < 200 && Main.npc[ai1].active && Main.npc[ai1].type == NPCID.SkeletronPrime))
                            {
                                npc.StrikeNPCNoInteraction(9999, 0f, 0); //die if prime gone
                                return false;
                            }
                            npc.target = Main.npc[ai1].target;
                            if (Main.npc[ai1].ai[1] == 3 || !npc.HasValidTarget) //return to normal AI
                                break;
                            if (masoBool[1]) //swipe AI
                            {
                                if (!masoBool[3])
                                {
                                    masoBool[3] = true;
                                    switch (npc.type)
                                    {
                                        case NPCID.PrimeCannon: Counter = -1; Counter2 = -1; break;
                                        case NPCID.PrimeLaser: Counter = 1; Counter2 = -1; break;
                                        case NPCID.PrimeSaw: Counter = -1; Counter2 = 1; break;
                                        case NPCID.PrimeVice: Counter = 1; Counter2 = 1; break;
                                        default: break;
                                    }
                                }
                                if (++npc.ai[2] < 180)
                                {
                                    Vector2 target = Main.player[npc.target].Center;
                                    target.X += 400 * Counter;
                                    target.Y += 400 * Counter2;
                                    npc.velocity = (target - npc.Center) / 30;
                                    if (npc.ai[2] == 140)
                                        for (int i = 0; i < 20; i++)
                                        {
                                            int d = Dust.NewDust(npc.position, npc.width, npc.height, 112, npc.velocity.X * .4f, npc.velocity.Y * .4f, 0, Color.White, 2);
                                            Main.dust[d].scale += 1f;
                                            Main.dust[d].velocity *= 3f;
                                            Main.dust[d].noGravity = true;
                                        }
                                }
                                else if (npc.ai[2] == 180)
                                {
                                    npc.velocity = npc.DirectionTo(Main.player[npc.target].Center) * 20;
                                    npc.netUpdate = true;
                                    Counter *= -1;
                                    Counter2 *= -1;
                                }
                                else if (npc.ai[2] < 240)
                                {
                                    npc.velocity *= 0.996f;
                                }
                                else
                                {
                                    npc.ai[2] = 0;
                                    npc.netUpdate = true;
                                }
                                npc.rotation = Main.npc[ai1].DirectionTo(npc.Center).ToRotation() - (float)Math.PI / 2;
                                if (npc.netUpdate)
                                {
                                    if (Main.netMode == 2)
                                    {
                                        NetMessage.SendData(23, -1, -1, null, npc.whoAmI);
                                        var netMessage = mod.GetPacket();
                                        netMessage.Write((byte)13);
                                        netMessage.Write((byte)npc.whoAmI);
                                        netMessage.Write(Counter);
                                        netMessage.Write(Counter2);
                                        netMessage.Send();
                                    }
                                    npc.netUpdate = false;
                                }
                                return false;
                            }
                            else if (Main.npc[ai1].ai[1] == 1 || Main.npc[ai1].ai[1] == 2) //other limbs while prime spinning
                            {
                                int d = Dust.NewDust(npc.position, npc.width, npc.height, 112, npc.velocity.X * .4f, npc.velocity.Y * .4f, 0, Color.White, 2);
                                Main.dust[d].noGravity = true;
                                if (!masoBool[2]) //AND STRETCH HIS ARMS OUT JUST FOR YOU
                                {
                                    int rotation = 0;
                                    switch(npc.type)
                                    {
                                        case NPCID.PrimeCannon: rotation = 0; break;
                                        case NPCID.PrimeLaser: rotation = 1; break;
                                        case NPCID.PrimeSaw: rotation = 2; break;
                                        case NPCID.PrimeVice: rotation = 3; break;
                                        default: break;
                                    }
                                    Vector2 offset = Main.player[Main.npc[ai1].target].Center - Main.npc[ai1].Center;
                                    offset = offset.RotatedBy(Math.PI / 2 * rotation + Math.PI / 4);
                                    offset = Vector2.Normalize(offset) * (offset.Length() + 200);
                                    if (offset.Length() < 600)
                                        offset = Vector2.Normalize(offset) * 600;
                                    Vector2 target = Main.npc[ai1].Center + offset;

                                    npc.velocity = (target - npc.Center) / 20;

                                    if (++Counter > 60)
                                    {
                                        masoBool[2] = true;
                                        Counter = (int)Main.npc[ai1].Distance(npc.Center);
                                        if (Counter < 300)
                                            Counter = 300;
                                        npc.localAI[3] = Main.npc[ai1].DirectionTo(npc.Center).ToRotation();
                                        
                                        if (Main.netMode == 2) //MP sync
                                        {
                                            var netMessage = mod.GetPacket();
                                            netMessage.Write((byte)12);
                                            netMessage.Write((byte)npc.whoAmI);
                                            netMessage.Write(masoBool[2]);
                                            netMessage.Write(Counter);
                                            netMessage.Write(npc.localAI[3]);
                                            netMessage.Send();
                                        }
                                    }
                                }
                                else //spinning
                                {
                                    float range = Counter; //extend further to hit player if beyond current range
                                    if (Main.npc[ai1].HasValidTarget && Main.npc[ai1].Distance(Main.player[Main.npc[ai1].target].Center) > range)
                                        range = Main.npc[ai1].Distance(Main.player[Main.npc[ai1].target].Center);
                                    
                                    npc.Center = Main.npc[ai1].Center + new Vector2(range, 0f).RotatedBy(npc.localAI[3]);
                                    const float rotation = 0.1f;
                                    npc.localAI[3] += rotation;
                                    if (npc.localAI[3] > (float)Math.PI)
                                    {
                                        npc.localAI[3] -= 2f * (float)Math.PI;
                                        npc.netUpdate = true;
                                    }
                                }
                                npc.rotation = Main.npc[ai1].DirectionTo(npc.Center).ToRotation() - (float)Math.PI / 2;
                                return false;
                            }

                            if (masoBool[2])
                            {
                                masoBool[2] = false;
                                Counter = 0;
                                if (Main.netMode == 2) //MP sync
                                {
                                    var netMessage = mod.GetPacket();
                                    netMessage.Write((byte)12);
                                    netMessage.Write((byte)npc.whoAmI);
                                    netMessage.Write(masoBool[2]);
                                    netMessage.Write(Counter);
                                    netMessage.Write(npc.localAI[3]);
                                    netMessage.Send();
                                }
                            }
                        }
                        break;

                    case NPCID.MoonLordHand:
                    case NPCID.MoonLordHead:
                        RegenTimer = 2;
                        npc.defense = masoStateML >= 0 && masoStateML <= 3 ? 0 : npc.defDefense;

                        if (npc.ai[0] == -2f) //eye socket is empty
                        {
                            if (npc.ai[1] == 0f //happens every 32 ticks
                                && Main.npc[(int)npc.ai[3]].ai[0] != 2f) //will stop when ML dies
                            {
                                Timer++;
                                if (Timer >= 29) //warning dust, reset timer
                                {
                                    bool fireLaser = true;
                                    for (int i = 0; i < Main.maxNPCs; i++) //find this ML's true eye (they're synced, so any is fine)
                                        if (Main.npc[i].active && Main.npc[i].type == NPCID.MoonLordFreeEye && Main.npc[i].ai[3] == npc.ai[3]) 
                                        {
                                            if (Main.npc[i].ai[0] == 4 && Main.npc[i].ai[1] > 800) //if free eyes are firing deathray, delay own ray
                                            {
                                                fireLaser = false;
                                                Timer = 27;
                                            }
                                            break;
                                        }
                                    if (fireLaser)
                                    {
                                        Timer = 0;
                                        int t = npc.HasPlayerTarget ? npc.target : npc.FindClosestPlayer();
                                        if (t != -1)
                                        {
                                            npc.localAI[0] = (Main.player[t].Center - npc.Center).ToRotation();
                                            /*Vector2 offset = Vector2.UnitX.RotatedBy(npc.localAI[0]) * 10f;
                                            for (int i = 0; i < 300; i++) //dust warning line for laser
                                            {
                                                int d = Dust.NewDust(npc.Center + offset * i, 1, 1, 111, 0f, 0f, 0, default(Color), 1.5f);
                                                Main.dust[d].noGravity = true;
                                                Main.dust[d].velocity *= 0.5f;
                                            }*/
                                            Projectile.NewProjectile(npc.Center, Vector2.UnitX.RotatedBy(npc.localAI[0]), mod.ProjectileType("PhantasmalDeathrayMLSmall"), 0, 0f, Main.myPlayer, 0, npc.whoAmI);
                                        }
                                    }
                                }
                                if (Timer == 3) //FIRE LASER
                                {
                                    int t = npc.HasPlayerTarget ? npc.target : npc.FindClosestPlayer();
                                    if (t != -1)
                                    {
                                        if (Main.netMode != 1)
                                        {
                                            float newRotation = (Main.player[t].Center - npc.Center).ToRotation();
                                            float difference = newRotation - npc.localAI[0];
                                            const float PI = (float)Math.PI;
                                            float rotationDirection = PI / 4f / 120f; //positive is CW, negative is CCW
                                            if (difference < -PI)
                                                difference += 2f * PI;
                                            if (difference > PI)
                                                difference -= 2f * PI;
                                            if (difference < 0f)
                                                rotationDirection *= -1f;
                                            Vector2 speed = Vector2.UnitX.RotatedBy(npc.localAI[0]);
                                            int damage = (int)(60 * (1 + FargoSoulsWorld.MoonlordCount * .0125));
                                            Projectile.NewProjectile(npc.Center, speed, mod.ProjectileType("PhantasmalDeathrayML"), damage, 0f, Main.myPlayer, rotationDirection, npc.whoAmI);
                                        }
                                    }
                                }
                                npc.netUpdate = true;
                            }
                        }
                        break;

                    case NPCID.MoonLordFreeEye:
                        if (!masoBool[0] & ++Counter > 2) //sync to other eyes of same core when spawned
                        {
                            masoBool[0] = true;
                            Counter = 0;
                            for (int i = 0; i < Main.maxNPCs; i++)
                                if (Main.npc[i].active && Main.npc[i].type == NPCID.MoonLordFreeEye && Main.npc[i].ai[3] == npc.ai[3] && i != npc.whoAmI)
                                {
                                    npc.ai[0] = Main.npc[i].ai[0];
                                    npc.ai[1] = Main.npc[i].ai[1];
                                    npc.ai[2] = Main.npc[i].ai[2];
                                    npc.ai[3] = Main.npc[i].ai[3];
                                    npc.localAI[0] = Main.npc[i].localAI[0];
                                    npc.localAI[1] = Main.npc[i].localAI[1];
                                    npc.localAI[2] = Main.npc[i].localAI[2];
                                    npc.localAI[3] = Main.npc[i].localAI[3];
                                    break;
                                }
                            npc.netUpdate = true;
                        }
                        break;

                    case NPCID.AncientLight:
                        npc.dontTakeDamage = true;
                        npc.immortal = true;
                        npc.chaseable = false;
                        if (npc.buffType[0] != 0)
                            npc.DelBuff(0);
                        if (masoBool[0])
                        {
                            if (npc.HasPlayerTarget)
                            {
                                Vector2 speed = Main.player[npc.target].Center - npc.Center;
                                speed.Normalize();
                                speed *= 9f;

                                npc.ai[2] += speed.X / 100f;
                                if (npc.ai[2] > 9f)
                                    npc.ai[2] = 9f;
                                if (npc.ai[2] < -9f)
                                    npc.ai[2] = -9f;
                                npc.ai[3] += speed.Y / 100f;
                                if (npc.ai[3] > 9f)
                                    npc.ai[3] = 9f;
                                if (npc.ai[3] < -9f)
                                    npc.ai[3] = -9f;
                            }
                            else
                            {
                                npc.TargetClosest(false);
                            }

                            Counter++;
                            if (Counter > 240)
                            {
                                npc.HitEffect(0, 9999);
                                npc.active = false;
                            }

                            npc.velocity.X = npc.ai[2];
                            npc.velocity.Y = npc.ai[3];
                        }
                        break;

                    case NPCID.DemonEye:
                    case NPCID.DemonEyeOwl:
                    case NPCID.DemonEyeSpaceship:
                    case NPCID.CataractEye:
                    case NPCID.SleepyEye:
                    case NPCID.DialatedEye:
                    case NPCID.GreenEye:
                    case NPCID.PurpleEye:
                        Counter++;
                        if (Counter == 360) //warning dust
                        {
                            for (int i = 0; i < 20; i++)
                            {
                                Vector2 vector6 = Vector2.UnitY * 5f;
                                vector6 = vector6.RotatedBy((i - (20 / 2 - 1)) * 6.28318548f / 20) + npc.Center;
                                Vector2 vector7 = vector6 - npc.Center;
                                int d = Dust.NewDust(vector6 + vector7, 0, 0, DustID.Fire);
                                Main.dust[d].noGravity = true;
                                Main.dust[d].velocity = vector7;
                                Main.dust[d].scale = 1.5f;
                            }
                        }
                        else if (Counter >= 420)
                        {
                            npc.TargetClosest();
                            Vector2 velocity = Vector2.Normalize(Main.player[npc.target].Center - npc.Center) * 10;
                            npc.velocity = velocity;
                            Counter = Main.rand.Next(-300, 0);
                        }

                        if (Math.Abs(npc.velocity.Y) > 5 || Math.Abs(npc.velocity.X) > 5)
                        {
                            int dustId = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + 2f), npc.width, npc.height + 5, DustID.Stone, npc.velocity.X * 0.2f,
                                npc.velocity.Y * 0.2f, 100, default(Color), 1f);
                            Main.dust[dustId].noGravity = true;
                            int dustId3 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + 2f), npc.width, npc.height + 5, DustID.Stone, npc.velocity.X * 0.2f,
                                npc.velocity.Y * 0.2f, 100, default(Color), 1f);
                            Main.dust[dustId3].noGravity = true;
                        }
                        break;

                    case NPCID.HoppinJack:
                        Counter++;
                        if (Counter >= 20 && npc.velocity.X != 0)
                        {
                            Counter = 0;
                            int p = Projectile.NewProjectile(npc.Center, Vector2.Zero, ProjectileID.MolotovFire, (int)(npc.damage / 2), 1f, Main.myPlayer);
                            Main.projectile[p].hostile = true;
                            Main.projectile[p].friendly = false;
                        }
                        break;

                    case NPCID.Antlion:
                        Counter++;
                        if (Counter >= 30)
                        {
                            foreach (Player p in Main.player.Where(x => x.active && !x.dead))
                            {
                                if (p.HasBuff(mod.BuffType("Stunned")) && npc.Distance(p.Center) < 250)
                                {
                                    Vector2 velocity = Vector2.Normalize(npc.Center - p.Center) * 5f;
                                    p.velocity += velocity;
                                }
                            }
                            Counter = 0;
                        }
                        break;

                    case NPCID.AngryNimbus:
                        Counter++;
                        if (Counter >= 360)
                        {
                            Counter = 0;
                            if (Main.netMode != 1)
                            {
                                Projectile.NewProjectile(new Vector2(npc.Center.X + 100, npc.Center.Y), Vector2.Zero, ProjectileID.VortexVortexLightning, 0, 1, Main.myPlayer, 0, 1);
                                Projectile.NewProjectile(new Vector2(npc.Center.X - 100, npc.Center.Y), Vector2.Zero, ProjectileID.VortexVortexLightning, 0, 1, Main.myPlayer, 0, 1);
                            }
                        }
                        break;

                    case NPCID.Unicorn:
                        Counter++;
                        if (Math.Abs(npc.velocity.X) > 1 && Counter >= 3)
                        {
                            int direction = npc.velocity.X > 0 ? 1 : -1;
                            int p = Projectile.NewProjectile(new Vector2(npc.Center.X - direction * (npc.width / 2), npc.Center.Y), npc.velocity, ProjectileID.RainbowBack, npc.damage / 3, 1);
                            if (p < 1000)
                            {
                                Main.projectile[p].friendly = false;
                                Main.projectile[p].hostile = true;
                                Main.projectile[p].GetGlobalProjectile<FargoGlobalProjectile>().Rainbow = true;
                            }
                            Counter = 0;
                        }
                        /*if (--Counter2 < 0)
                        {
                            Counter2 = 6;
                            if (Main.netMode != 1)
                            {
                                int p = Projectile.NewProjectile(npc.position.X + Main.rand.Next(npc.width), npc.position.Y + npc.height,
                                  npc.velocity.X * .4f + Main.rand.NextFloat(-0.5f, 0.5f), npc.velocity.Y * .4f + Main.rand.NextFloat(-0.5f, 0.5f),
                                  ProjectileID.HallowSpray, 0, 0f, Main.myPlayer, 8f);
                                if (p != 1000)
                                    Main.projectile[p].timeLeft = 12;
                            }
                        }*/
                        break;

                    case NPCID.Reaper:
                        Aura(npc, 40, mod.BuffType("MarkedforDeath"), false, 199);
                        Counter++;
                        if (Counter >= 420)
                        {
                            Counter = 0;
                            npc.TargetClosest();
                            Vector2 velocity = Vector2.Normalize(Main.player[npc.target].Center - npc.Center) * 10;
                            npc.velocity = velocity;
                            masoBool[0] = true;
                            Timer = 5;
                        }

                        if (masoBool[0])
                        {
                            Counter2++;
                            if (Counter2 >= 10)
                            {
                                int p = Projectile.NewProjectile(npc.Center, Vector2.Zero, ProjectileID.DeathSickle, (int)(npc.damage / 2), 1f, Main.myPlayer);
                                Main.projectile[p].hostile = true;
                                Main.projectile[p].friendly = false;

                                Counter2 = 0;
                                Timer--;

                                if (Timer <= 0)
                                    masoBool[0] = false;
                            }
                        }
                        break;

                    case NPCID.Werewolf:
                        Aura(npc, 200, mod.BuffType("Berserked"), false, 60);
                        break;

                    case NPCID.BloodZombie:
                        Aura(npc, 300, BuffID.Bleeding, false, 5, true);
                        break;

                    case NPCID.PossessedArmor:
                        Aura(npc, 400, BuffID.BrokenArmor, false, 37, true);
                        break;

                    case NPCID.ShadowFlameApparition:
                        Aura(npc, 100, mod.BuffType("Shadowflame"), false, DustID.Shadowflame);
                        break;

                    case NPCID.BlueJellyfish:
                    case NPCID.PinkJellyfish:
                    case NPCID.GreenJellyfish:
                    case NPCID.BloodJelly:
                        if (npc.wet && npc.ai[1] == 1f) //when they be electrocuting
                        {
                            Player p = Main.player[Main.myPlayer];
                            if (npc.Distance(p.Center) < 200 && p.wet && Collision.CanHitLine(p.Center, 2, 2, npc.Center, 2, 2))
                                p.AddBuff(BuffID.Electrified, 2);

                            for (int i = 0; i < 10; i++)
                            {
                                Vector2 offset = new Vector2();
                                double angle = Main.rand.NextDouble() * 2d * Math.PI;
                                offset.X += (float)(Math.Sin(angle) * 200);
                                offset.Y += (float)(Math.Cos(angle) * 200);
                                if (Framing.GetTileSafely(npc.Center + offset - new Vector2(4, 4)).liquid == 0) //dont display outside liquids
                                    continue;
                                Dust dust = Main.dust[Dust.NewDust(
                                    npc.Center + offset - new Vector2(4, 4), 0, 0,
                                    DustID.Electric, 0, 0, 100, Color.White, 1f
                                    )];
                                dust.velocity = npc.velocity;
                                if (Main.rand.Next(3) == 0)
                                    dust.velocity += Vector2.Normalize(offset) * -5f;
                                dust.noGravity = true;
                            }
                        }
                        break;

                    case NPCID.Wraith:
                        Aura(npc, 80, BuffID.Obstructed, false, 199);
                        break;

                    case NPCID.MartianSaucerCore:
                        Aura(npc, 250, BuffID.VortexDebuff, false, DustID.Vortex);
                        if (!npc.dontTakeDamage && npc.HasPlayerTarget && ++Counter > 240)
                        {
                            if (++Counter2 > 2)
                            {
                                Counter2 = 0;
                                Vector2 speed = 16f * npc.DirectionTo(Main.player[npc.target].Center).RotatedBy((Main.rand.NextDouble() - 0.5) * 0.785398185253143 / 2.0);
                                if (Main.netMode != 1)
                                    Projectile.NewProjectile(npc.Center, speed, ProjectileID.SaucerLaser, 15, 0f, Main.myPlayer);
                            }

                            if (Counter > 360)
                                Counter = 0;
                        }
                        break;

                    case NPCID.ToxicSludge:
                        Aura(npc, 200, BuffID.Poisoned, false, 188, true);
                        break;

                    case NPCID.GiantTortoise:
                    case NPCID.IceTortoise:
                        npc.reflectingProjectiles = npc.ai[0] == 3f; //while shell spinning 
                        break;

                    case NPCID.SpikeBall:
                    case NPCID.BlazingWheel:
                        if (!masoBool[1])
                        {
                            masoBool[1] = true;
                            if (Main.tile[(int)npc.Center.X / 16, (int)npc.Center.Y / 16] != null //in temple
                                && Main.tile[(int)npc.Center.X / 16, (int)npc.Center.Y / 16].wall == WallID.LihzahrdBrickUnsafe)
                            {
                                npc.damage = 150;
                                masoBool[0] = true;
                            }
                        }
                        if (masoBool[0])
                        {
                            if (++Counter > 1800)
                            {
                                if (Main.netMode != 1)
                                    Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("FuseBomb"), npc.damage / 4, 0f, Main.myPlayer);
                                npc.StrikeNPCNoInteraction(999999, 0f, 0);
                            }
                        }
                        break;

                    case NPCID.GraniteGolem:
                        if (npc.ai[2] < 0f) //while shielding, reflect
                            CustomReflect(npc, DustID.Granite, 2);
                        break;

                    case NPCID.BigMimicCorruption:
                    case NPCID.BigMimicCrimson:
                    case NPCID.BigMimicHallow:
                    case NPCID.BigMimicJungle:
                        if (masoBool[0])
                        {
                            if (npc.velocity.Y == 0f) //spawn smash
                            {
                                masoBool[0] = false;
                                Main.PlaySound(2, npc.Center, 14);
                                if (Main.netMode != 1)
                                {
                                    Projectile.NewProjectile(npc.Center, Vector2.Zero, ProjectileID.DD2OgreSmash, npc.damage / 4, 4, Main.myPlayer);
                                    /*int type;
                                    if (npc.type == NPCID.BigMimicHallow)
                                        type = ProjectileID.HallowSpray;
                                    else if (npc.type == NPCID.BigMimicCorruption)
                                        type = ProjectileID.CorruptSpray;
                                    else if (npc.type == NPCID.BigMimicCrimson)
                                        type = ProjectileID.CrimsonSpray;
                                    else
                                        type = ProjectileID.PureSpray;
                                    const int max = 16;
                                    for (int i = 0; i < max; i++)
                                    {
                                        Vector2 vel = new Vector2(0f, -4f).RotatedBy(2 * Math.PI / max * i);
                                        Projectile.NewProjectile(npc.Center, vel, type, 0, 0f, Main.myPlayer, 8f);
                                    }*/
                                }
                            }
                        }
                        else if (npc.velocity.Y > 0 && npc.noTileCollide) //mega jump
                        {
                            masoBool[0] = true;
                        }
                        if (npc.position.Y < Main.worldSurface * 16)
                        {
                            if (++Counter > 300)
                            {
                                Counter = 0;
                                masoBool[1] = !masoBool[1];
                            }
                            if (masoBool[1] && ++Counter2 > 6)
                            {
                                Counter2 = 0;
                                if (npc.HasPlayerTarget)
                                {
                                    Vector2 speed = Main.player[npc.target].Center - npc.Center;
                                    speed.X += Main.rand.Next(-80, 81);
                                    speed.Y += Main.rand.Next(-80, 81);
                                    speed.Normalize();
                                    speed *= 8f;
                                    if (Main.netMode != 1)
                                        Projectile.NewProjectile(npc.Center, speed, mod.ProjectileType("MimicCoin"), npc.damage / 5, 0f, Main.myPlayer, Main.rand.Next(3));

                                    Main.PlaySound(SoundID.Item11, npc.Center);
                                }
                            }
                        }
                        break;

                    case NPCID.DD2Betsy:
                        betsyBoss = npc.whoAmI;
                        if (npc.ai[0] == 6f && npc.ai[1] == 1f)
                        {
                            if (Main.netMode != 1 && NPC.CountNPCS(NPCID.DD2DarkMageT3) < 3)
                            {
                                int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.DD2DarkMageT3, npc.whoAmI);
                                if (n != 200 && Main.netMode == 2)
                                    NetMessage.SendData(23, -1, -1, null, n);
                            }
                            npc.netUpdate = true;
                        }
                        if (!DD2Event.Ongoing && npc.HasPlayerTarget && (!Main.player[npc.target].active || Main.player[npc.target].dead || npc.Distance(Main.player[npc.target].Center) > 3000))
                        {
                            int p = Player.FindClosest(npc.Center, 0, 0);
                            if (p < 0 || !Main.player[p].active || Main.player[p].dead || npc.Distance(Main.player[p].Center) > 3000)
                                npc.active = false;
                        }
                        break;

                    case NPCID.DungeonGuardian:
                        guardBoss = npc.whoAmI;
                        npc.damage = npc.defDamage;
                        npc.defense = npc.defDefense;
                        while (npc.buffType[0] != 0)
                            npc.DelBuff(0);
                        if (npc.velocity.Length() < 5f)
                        {
                            npc.velocity.Normalize();
                            npc.velocity *= 5f;
                        }
                        if (--Counter < 0)
                        {
                            Counter = 60;
                            if (npc.HasPlayerTarget && Main.netMode != 1)
                            {
                                Vector2 speed = Main.player[npc.target].Center - npc.Center;
                                speed.X += Main.rand.Next(-20, 21);
                                speed.Y += Main.rand.Next(-20, 21);
                                speed.Normalize();
                                speed *= 3f;
                                speed += npc.velocity * 2f;
                                Projectile.NewProjectile(npc.Center, speed, ProjectileID.Skull, npc.damage / 4, 0, Main.myPlayer, -1f, 0f);
                            }
                        }
                        if (++Counter2 > 6)
                        {
                            Counter2 = 0;
                            Vector2 speed = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                            speed.Normalize();
                            speed *= 6f;
                            speed += npc.velocity * 1.25f;
                            speed.Y -= Math.Abs(speed.X) * 0.2f;
                            if (Main.netMode != 1)
                                Projectile.NewProjectile(npc.Center, speed, mod.ProjectileType("SkeletronBone"), npc.damage / 4, 0f, Main.myPlayer);
                        }
                        break;

                    case NPCID.RainbowSlime:
                        if (masoBool[0]) //small rainbow slime
                        {
                            if (!masoBool[1] && ++Counter > 15)
                            {
                                masoBool[1] = true;
                                if (Main.netMode == 2) //MP sync
                                {
                                    var netMessage = mod.GetPacket();
                                    netMessage.Write((byte)3);
                                    netMessage.Write((byte)npc.whoAmI);
                                    netMessage.Write(npc.lifeMax);
                                    netMessage.Write(npc.scale);
                                    netMessage.Send();
                                    npc.netUpdate = true;
                                }
                            }
                        }

                        if (masoBool[2]) //shoot spikes whenever jumping
                        {
                            if (npc.velocity.Y == 0f) //start attack
                            {
                                masoBool[2] = false;
                                if (npc.HasPlayerTarget && Main.netMode != 1)
                                {
                                    const float gravity = 0.15f;
                                    const float time = 120f;
                                    Vector2 distance = Main.player[npc.target].Center - npc.Center;
                                    distance += Main.player[npc.target].velocity * 30f;
                                    distance.X = distance.X / time;
                                    distance.Y = distance.Y / time - 0.5f * gravity * time;
                                    for (int i = 0; i < 10; i++)
                                    {
                                        Projectile.NewProjectile(npc.Center, distance + Main.rand.NextVector2Square(-1f, 1f),
                                            mod.ProjectileType("RainbowSlimeSpike"), npc.damage / 8, 0f, Main.myPlayer);
                                    }
                                }
                            }
                        }
                        else if (npc.velocity.Y > 0)
                        {
                            masoBool[2] = true;
                        }
                        break;

                    case NPCID.MisterStabby:
                        if (masoBool[0])
                            npc.position.X += npc.velocity.X / 2;
                        break;

                    case NPCID.SnowmanGangsta:
                        if (++Counter > 300)
                        {
                            Counter = 0;
                            if (Main.netMode != 1 && npc.HasPlayerTarget)
                            {
                                for (int index = 0; index < 6; ++index)
                                {
                                    Vector2 Speed = Main.player[npc.target].Center - npc.Center;
                                    Speed.X += Main.rand.Next(-40, 41);
                                    Speed.Y += Main.rand.Next(-40, 41);
                                    Speed.Normalize();
                                    Speed *= 11f;
                                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, Speed.X, Speed.Y, ProjectileID.BulletSnowman, 20, 0f, Main.myPlayer);
                                }
                            }
                            Main.PlaySound(SoundID.Item38, npc.Center);
                        }
                        break;

                    case NPCID.SnowBalla:
                        if (npc.ai[2] == 8f)
                        {
                            npc.velocity.X = 0f;
                            npc.velocity.Y = 0f;
                            float num3 = 10f;
                            Vector2 vector2 = new Vector2(npc.position.X + npc.width * 0.5f - npc.direction * 12, npc.position.Y + npc.height * 0.25f);
                            float num4 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2f - vector2.X;
                            float num5 = Main.player[npc.target].position.Y - vector2.Y;
                            float num6 = (float)Math.Sqrt(num4 * num4 + num5 * num5);
                            float num7 = num3 / num6;
                            float SpeedX = num4 * num7;
                            float SpeedY = num5 * num7;
                            if (Main.netMode != 1)
                            {
                                int Damage = 35;
                                int Type = 109;
                                int p = Projectile.NewProjectile(vector2.X, vector2.Y, SpeedX, SpeedY, Type, Damage, 0f, Main.myPlayer);
                                Main.projectile[p].ai[0] = 2f;
                                Main.projectile[p].timeLeft = 300;
                                Main.projectile[p].friendly = false;
                                NetMessage.SendData(27, -1, -1, null, p);
                                npc.netUpdate = true;
                            }
                        }
                        break;

                    case NPCID.PirateShip:
                        if (npc.HasPlayerTarget)
                        {
                            if (npc.velocity.Y < 0f && npc.position.Y + npc.height < Main.player[npc.target].position.Y)
                                npc.velocity.Y = 0f;
                        }
                        break;

                    case NPCID.PirateShipCannon:
                        if (++Counter > 300)
                        {
                            Counter = 0;
                            masoBool[0] = !masoBool[0];
                        }
                        if (masoBool[0] && ++Counter2 > 10)
                        {
                            Counter2 = 0;
                            if (npc.HasPlayerTarget)
                            {
                                Vector2 speed = Main.player[npc.target].Center - npc.Center;
                                speed.X += Main.rand.Next(-40, 41);
                                speed.Y += Main.rand.Next(-40, 41);
                                speed.Normalize();
                                speed *= 14f;
                                if (Main.netMode != 1)
                                    Projectile.NewProjectile(npc.Center, speed, mod.ProjectileType("PirateDeadeyeBullet"), 15, 0f, Main.myPlayer);

                                Main.PlaySound(SoundID.Item11, npc.Center);
                            }
                        }
                        break;

                    case NPCID.GoblinSummoner:
                        Aura(npc, 200, mod.BuffType("Shadowflame"), false, DustID.Shadowflame);
                        if (++Counter > 180)
                        {
                            Counter = 0;
                            Main.PlaySound(SoundID.Item8, npc.Center);
                            if (npc.HasPlayerTarget && Main.netMode != 1)
                            {
                                for (int i = 0; i < 4; i++)
                                {
                                    Vector2 spawnPos = npc.Center + new Vector2(200f, 0f).RotatedBy(Math.PI / 2 * (i + 0.5));
                                    //Vector2 speed = Vector2.Normalize(Main.player[npc.target].Center - spawnPos) * 10f;
                                    int n = NPC.NewNPC((int)spawnPos.X, (int)spawnPos.Y, NPCID.ChaosBall);
                                    if (n != 200 && Main.netMode == 2)
                                        NetMessage.SendData(23, -1, -1, null, n);
                                    for (int j = 0; j < 20; j++)
                                    {
                                        int d = Dust.NewDust(spawnPos, 0, 0, DustID.Shadowflame);
                                        Main.dust[d].noGravity = true;
                                        Main.dust[d].scale += 0.5f;
                                        Main.dust[d].velocity *= 6f;
                                    }
                                }
                            }
                        }
                        break;

                    case NPCID.IceGolem:
                    case NPCID.Yeti:
                        if (++Counter > 60)
                        {
                            Counter = 0;
                            if (npc.HasPlayerTarget && Main.netMode != 1)
                            {
                                Projectile.NewProjectile(npc.Center, new Vector2(6f, 0f).RotatedByRandom(2 * Math.PI),
                                    mod.ProjectileType("FrostfireballHostile"), npc.damage / 5, 0f, Main.myPlayer, npc.target, 30f);
                            }
                        }
                        break;

                    case NPCID.EaterofSouls:
                        if (npc.noTileCollide && Framing.GetTileSafely(npc.Center).nactive() && Main.tileSolid[Framing.GetTileSafely(npc.Center).type]) //in a wall
                        {
                            int d = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Shadowflame, npc.velocity.X, npc.velocity.Y);
                            Main.dust[d].noGravity = true;
                            npc.position -= npc.velocity / 2f;
                        }
                        else //not in a wall
                        {
                            if (++Counter >= 300)
                                Shoot(npc, 30, 600, 12, ProjectileID.CursedFlameHostile, npc.damage / 4, 0);
                        }
                        goto case NPCID.Harpy;

                    case NPCID.Harpy:
                        if (!masoBool[0] && ++Counter2 > 15)
                        {
                            masoBool[0] = true;
                            Counter2 = 0;
                            if (Main.netMode != 1 && Main.rand.Next(2) == 0)
                            {
                                masoBool[1] = true;
                                NetUpdateMaso(npc.whoAmI);
                            }
                        }
                        npc.noTileCollide = masoBool[1];
                        break;

                    case NPCID.Hornet:
                    case NPCID.HornetFatty:
                    case NPCID.HornetHoney:
                    case NPCID.HornetLeafy:
                    case NPCID.HornetSpikey:
                    case NPCID.HornetStingy:
                    case NPCID.MossHornet:
                        if (npc.HasPlayerTarget)
                        {
                            bool shouldNotTileCollide = Main.player[npc.target].active && !Main.player[npc.target].dead
                                && Main.player[npc.target].GetModPlayer<FargoPlayer>().Swarming
                                && !Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0);
                            if (shouldNotTileCollide)
                                npc.noTileCollide = true;
                            else if (npc.noTileCollide && !Collision.SolidCollision(npc.position, npc.width, npc.height)) //still intangible, but should stop, and isnt on tiles
                                npc.noTileCollide = false;

                        }
                        break;

                    case NPCID.GoblinArcher:
                    case NPCID.GoblinPeon:
                    case NPCID.GoblinScout:
                    case NPCID.GoblinWarrior:
                        if (npc.HasPlayerTarget && (!Main.player[npc.target].active || Main.player[npc.target].dead))
                        {
                            npc.TargetClosest();
                            if (npc.HasPlayerTarget && (!Main.player[npc.target].active || Main.player[npc.target].dead))
                                npc.noTileCollide = true;
                        }
                        if (npc.noTileCollide)
                            npc.velocity.Y++;
                        break;

                    case NPCID.GoblinSorcerer:
                        if (npc.HasPlayerTarget && (!Main.player[npc.target].active || Main.player[npc.target].dead))
                        {
                            npc.TargetClosest();
                            if (npc.HasPlayerTarget && (!Main.player[npc.target].active || Main.player[npc.target].dead))
                                npc.noTileCollide = true;
                        }
                        if (npc.noTileCollide)
                            npc.velocity.Y++;
                        goto case NPCID.DarkCaster;

                    case NPCID.GoblinThief:
                        npc.position.X += npc.velocity.X;
                        goto case NPCID.GoblinArcher;

                    case NPCID.VileSpit:
                        /*if (--Counter2 < 0)
                        {
                            Counter2 = 12;
                            if (Main.netMode != 1)
                            {
                                int p = Projectile.NewProjectile(npc.Center, npc.velocity, ProjectileID.CorruptSpray, 0, 0f, Main.myPlayer, 8f);
                                if (p != 1000)
                                    Main.projectile[p].timeLeft = 12;
                            }
                        }*/
                        if (++Counter > 600)
                            npc.StrikeNPCNoInteraction(9999, 0f, 0);
                        break;

                    case NPCID.WanderingEye:
                        if (npc.life < npc.lifeMax / 2)
                        {
                            npc.knockBackResist = 0f;
                            if (++Counter > 20)
                            {
                                Counter = 0;
                                if (Main.netMode != 1)
                                    Projectile.NewProjectile(npc.Center, npc.velocity / 10, mod.ProjectileType("BloodScythe"), npc.damage / 4, 0f, Main.myPlayer);
                            }
                        }
                        break;

                    case NPCID.AnglerFish:
                        if (!masoBool[0]) //make light while invisible
                            Lighting.AddLight(npc.Center, 0.1f, 0.5f, 0.5f);
                        break;

                    case NPCID.Psycho: //alpha is controlled by vanilla ai so npc is necessary
                        if (Counter < 200)
                            Counter += 2;
                        if (npc.alpha < Counter)
                            npc.alpha = Counter;
                        break;

                    case NPCID.DD2SkeletonT1:
                    case NPCID.DD2SkeletonT3:
                        if (++Counter > 420)
                        {
                            Counter = 0;
                            if (Main.netMode != 1)
                            {
                                int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.ChaosBall);
                                if (n != 200 && Main.netMode == 2)
                                    NetMessage.SendData(23, -1, -1, null, n);
                            }
                        }
                        break;

                    case NPCID.DD2WitherBeastT2:
                    case NPCID.DD2WitherBeastT3:
                        Aura(npc, 300, BuffID.WitheredArmor, true, 119);
                        Aura(npc, 300, BuffID.WitheredWeapon, true, 14);
                        break;

                    case NPCID.DD2DarkMageT1:
                        Aura(npc, 600, mod.BuffType("Lethargic"), false, 254);
                        foreach (NPC n in Main.npc.Where(n => n.active && !n.friendly && n.type != npc.type && n.Distance(npc.Center) < 600))
                        {
                            n.GetGlobalNPC<FargoSoulsGlobalNPC>().PaladinsShield = true;
                            if (Main.rand.Next(2) == 0)
                            {
                                int d = Dust.NewDust(n.position, n.width, n.height, 254, 0f, -3f, 0, new Color(), 1.5f);
                                Main.dust[d].noGravity = true;
                                Main.dust[d].noLight = true;
                            }
                        }
                        break;
                    case NPCID.DD2DarkMageT3:
                        Aura(npc, 900, mod.BuffType("Lethargic"), false, 254);
                        foreach (NPC n in Main.npc.Where(n => n.active && !n.friendly && n.type != npc.type && n.Distance(npc.Center) < 900))
                        {
                            n.GetGlobalNPC<FargoSoulsGlobalNPC>().PaladinsShield = true;
                            if (Main.rand.Next(2) == 0)
                            {
                                int d = Dust.NewDust(n.position, n.width, n.height, 254, 0f, -3f, 0, new Color(), 1.5f);
                                Main.dust[d].noGravity = true;
                                Main.dust[d].noLight = true;
                            }
                        }
                        break;

                    case NPCID.DD2WyvernT1:
                    case NPCID.DD2WyvernT2:
                    case NPCID.DD2WyvernT3:
                        if (++Counter >= 180)
                        {
                            Counter = 0;
                            if (Main.netMode != 1)
                                Shoot(npc, 0, 300, 6, ProjectileID.Fireball, npc.damage / 4, 0f);
                        }
                        break;

                    case NPCID.DD2LightningBugT3:
                        Aura(npc, 400, mod.BuffType("LightningRod"), false, DustID.Vortex);
                        if (++Counter > 240)
                        {
                            Counter = 0;
                            if (Main.netMode != 1)
                                Projectile.NewProjectile(npc.Center, Vector2.Zero, 578, 0, 0f, Main.myPlayer);
                        }
                        break;

                    case NPCID.DD2KoboldFlyerT2:
                    case NPCID.DD2KoboldFlyerT3:
                        if (++Counter > 60)
                        {
                            Counter = 0;
                            if (Main.netMode != 1)
                                Projectile.NewProjectile(npc.Center, new Vector2(Main.rand.NextFloat(-2f, 2f), -5f),
                                    mod.ProjectileType("GoblinSpikyBall"), npc.damage / 5, 0f, Main.myPlayer);
                        }
                        break;

                    case NPCID.LavaSlime:
                        if (npc.velocity.Y < 0f)
                        {
                            masoBool[0] = true;
                        }
                        else if (npc.velocity.Y > 0f) //coming down
                        {
                            if (masoBool[0] && Main.netMode != 1 && npc.HasValidTarget && npc.position.Y > Main.player[npc.target].position.Y + 200
                                && npc.Center.ToTileCoordinates().Y > Main.maxTilesY - 200) //in hell and below player
                            {
                                masoBool[0] = false;
                                int tileX = (int)(npc.Center.X / 16f);
                                int tileY = (int)(npc.Center.Y / 16f);
                                Tile tile = Framing.GetTileSafely(tileX, tileY);
                                if (tile != null && !tile.active() && tile.liquid == 0)
                                {
                                    tile.liquidType(1);
                                    tile.liquid = 255;
                                    if (Main.netMode == 2)
                                        NetMessage.SendTileSquare(-1, tileX, tileY, 1);
                                    WorldGen.SquareTileFrame(tileX, tileY, true);
                                    npc.netUpdate = true;
                                }
                            }
                        }
                        else //npc vel y = 0
                        {
                            masoBool[0] = false;
                        }
                        break;

                    case NPCID.DD2OgreT2:
                    case NPCID.DD2OgreT3:
                        Aura(npc, 500, BuffID.Stinky, false, 188);
                        break;

                    case NPCID.Nymph:
                        npc.knockBackResist = 0f;
                        Aura(npc, 250, mod.BuffType("Lovestruck"), true, DustID.PinkFlame);
                        if (--Counter < 0)
                        {
                            Counter = 300;
                            if (Main.netMode != 1 && npc.HasPlayerTarget && npc.Distance(Main.player[npc.target].Center) < 1000)
                            {
                                Vector2 spawnVel = npc.DirectionFrom(Main.player[npc.target].Center) * 10f;
                                for (int i = -3; i < 3; i++)
                                    Projectile.NewProjectile(npc.Center, spawnVel.RotatedBy(Math.PI / 7 * i), mod.ProjectileType("FakeHeart2"), 20, 0f, Main.myPlayer, 30, 90 + 10 * i);
                            }
                        }
                        break;

                    case NPCID.DarkCaster:
                    case NPCID.FireImp:
                        if (Counter2 > 0)
                        {
                            if (Counter2 == 1)
                            {
                                Counter2 = 0;
                                if (Main.netMode != 1 && npc.HasPlayerTarget)
                                {
                                    npc.ai[0] = 1f;
                                    int num1 = (int)Main.player[npc.target].position.X / 16;
                                    int num2 = (int)Main.player[npc.target].position.Y / 16;
                                    int num3 = (int)npc.position.X / 16;
                                    int num4 = (int)npc.position.Y / 16;
                                    int num5 = 20;
                                    int num6 = 0;
                                    bool flag1 = false;
                                    if ((double)Math.Abs(npc.position.X - Main.player[npc.target].position.X) + (double)Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 2000.0)
                                    {
                                        num6 = 100;
                                        flag1 = true;
                                    }
                                    while (!flag1 && num6 < 100)
                                    {
                                        ++num6;
                                        int index1 = Main.rand.Next(num1 - num5, num1 + num5);
                                        for (int index2 = Main.rand.Next(num2 - num5, num2 + num5); index2 < num2 + num5; ++index2)
                                        {
                                            if ((index2 < num2 - 4 || index2 > num2 + 4 || (index1 < num1 - 4 || index1 > num1 + 4)) && (index2 < num4 - 1 || index2 > num4 + 1 || (index1 < num3 - 1 || index1 > num3 + 1)) && Main.tile[index1, index2].nactive())
                                            {
                                                bool flag2 = true;
                                                if ((npc.type == 32 || npc.type >= 281 && npc.type <= 286) && !Main.wallDungeon[(int)Main.tile[index1, index2 - 1].wall])
                                                    flag2 = false;
                                                else if (Main.tile[index1, index2 - 1].lava())
                                                    flag2 = false;
                                                if (flag2 && Main.tileSolid[(int)Main.tile[index1, index2].type] && !Collision.SolidTiles(index1 - 1, index1 + 1, index2 - 4, index2 - 1))
                                                {
                                                    npc.ai[1] = 20f;
                                                    npc.ai[2] = (float)index1;
                                                    npc.ai[3] = (float)index2;
                                                    flag1 = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    npc.netUpdate = true;
                                    if (Main.netMode == 2)
                                    {
                                        var netMessage = mod.GetPacket();
                                        netMessage.Write((byte)8);
                                        netMessage.Write((byte)npc.whoAmI);
                                        netMessage.Send();
                                    }
                                }
                            }
                            else
                            {
                                Counter2--;
                            }
                        }
                        //PrintAI(npc);
                        break;

                    case NPCID.SandElemental:
                        if (npc.HasValidTarget)
                            Main.player[npc.target].ZoneSandstorm = true;
                        if (++Counter > 60)
                        {
                            Counter = 0;
                            if (Main.netMode != 1 && !NPC.AnyNPCs(NPCID.DuneSplicerHead))
                            {
                                int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.DuneSplicerHead, npc.whoAmI, 0f, 0f, 0f, 0f, npc.target);
                                if (n != 200 && Main.netMode == 2)
                                    NetMessage.SendData(23, -1, -1, null, n);
                            }
                        }
                        break;

                    case NPCID.FungiSpore:
                        npc.dontTakeDamage = Counter < 60;
                        if (npc.dontTakeDamage)
                            Counter++;
                        break;

                    case NPCID.MothronSpawn:
                        Aura(npc, 300, mod.BuffType("SqueakyToy"));
                        break;

                    case NPCID.Poltergeist:
                        if (++Counter > 180)
                        {
                            Counter = 0;
                            if (Main.netMode != 1 && npc.HasPlayerTarget)
                            {
                                Vector2 vel = npc.DirectionTo(Main.player[npc.target].Center) * 4f;
                                int p = Projectile.NewProjectile(npc.Center, vel, ProjectileID.LostSoulHostile, npc.damage / 4, 0f, Main.myPlayer);
                                if (p != 1000)
                                    Main.projectile[p].timeLeft = 300;
                            }
                        }
                        break;

                    case NPCID.AngryBones:
                    case NPCID.AngryBonesBig:
                    case NPCID.AngryBonesBigHelmet:
                    case NPCID.AngryBonesBigMuscle:
                        if (++Counter > 180)
                        {
                            if (++Counter2 > 6)
                            {
                                Counter2 = 0;
                                Vector2 speed = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                                speed.Normalize();
                                speed *= 6f;
                                speed += npc.velocity * 1.25f;
                                speed.Y -= Math.Abs(speed.X) * 0.2f;
                                speed.Y -= 3f;
                                if (Main.netMode != 1)
                                    Projectile.NewProjectile(npc.Center, speed, ProjectileID.SkeletonBone, npc.damage / 4, 0f, Main.myPlayer);
                            }
                            if (Counter > 300)
                                Counter = 0;
                        }
                        break;

                    case NPCID.Butcher:
                        npc.position.X += npc.velocity.X;
                        break;

                    case NPCID.DesertScorpionWall:
                        if (++Counter > 240)
                        {
                            Counter = 0;
                            if (Main.netMode != 1 && npc.HasPlayerTarget)
                            {
                                Vector2 vel = npc.DirectionTo(Main.player[npc.target].Center) * 14;
                                Projectile.NewProjectile(npc.Center, vel, mod.ProjectileType("VenomSpit"), 9, 0, Main.myPlayer);
                            }
                        }
                        break;

                    case NPCID.Mothron:
                        if (--Counter < 0)
                        {
                            Counter = 20 + (int)(100f * npc.life / npc.lifeMax);
                            if (npc.HasPlayerTarget && Main.netMode != 1)
                            {
                                Vector2 spawnPos = npc.Center + new Vector2(30f * -npc.direction, 30f);
                                Vector2 vel = Main.player[npc.target].Center - spawnPos
                                    + new Vector2(Main.rand.Next(-80, 81), Main.rand.Next(-40, 41));
                                vel.Normalize();
                                vel *= 10f;
                                Projectile.NewProjectile(spawnPos, vel, ProjectileID.Stinger, npc.defDamage / 8, 0f, Main.myPlayer);
                            }
                        }

                        if (--Counter2 < 0)
                        {
                            Counter2 = 60 + (int)(120f * npc.life / npc.lifeMax);
                            if (npc.HasPlayerTarget && Main.netMode != 1)
                            {
                                Vector2 spawnPos = npc.Center;
                                spawnPos.X += 45f * npc.direction;
                                Vector2 vel = Vector2.Normalize(Main.player[npc.target].Center - spawnPos) * 9f;
                                Projectile.NewProjectile(spawnPos, vel, ProjectileID.EyeLaser, npc.defDamage / 5, 0f, Main.myPlayer);
                            }
                        }

                        npc.defense = npc.defDefense;
                        npc.reflectingProjectiles = npc.ai[0] >= 4f;
                        if (npc.reflectingProjectiles)
                        {
                            npc.defense *= 5;
                            if (npc.buffType[0] != 0)
                                npc.DelBuff(0);
                            int d = Dust.NewDust(npc.position, npc.width, npc.height, 228, npc.velocity.X * .4f, npc.velocity.Y * .4f, 0, Color.White, 3f);
                            Main.dust[d].velocity *= 6f;
                            Main.dust[d].noGravity = true;
                        }
                        break;

                    case NPCID.Moth:
                        npc.position += npc.velocity;
                        for (int i = 0; i < 2; i++)
                        {
                            int d = Dust.NewDust(npc.position, npc.width, npc.height, 70);
                            Main.dust[d].scale += 1f;
                            Main.dust[d].noGravity = true;
                            Main.dust[d].velocity *= 5f;
                        }
                        if (++Counter > 6)
                        {
                            Counter = 0;
                            if (Main.netMode != 1)
                                Projectile.NewProjectile(npc.Center, Main.rand.NextVector2Unit() * 12f,
                                    mod.ProjectileType("MothDust"), npc.damage / 5, 0f, Main.myPlayer);
                        }
                        break;

                    case NPCID.WyvernHead:
                        if (++Counter > 240)
                        {
                            Counter = 0;
                            if (Main.netMode != 1 && npc.velocity != Vector2.Zero)
                            {
                                const int max = 12;
                                Vector2 vel = Vector2.Normalize(npc.velocity) * 2f;
                                for (int i = 0; i < max; i++)
                                {
                                    Projectile.NewProjectile(npc.Center, vel.RotatedBy(2 * Math.PI / max * i),
                                        mod.ProjectileType("LightBall"), npc.damage / 5, 0f, Main.myPlayer, 0f, .012f * npc.direction);
                                }
                            }
                        }
                        break;

                    case NPCID.HeadlessHorseman:
                        if (++Counter > 180)
                        {
                            Counter = 0;
                            if (Main.netMode != 1 && npc.HasPlayerTarget)
                            {
                                Vector2 vel = (Main.player[npc.target].Center - npc.Center) / 60f;
                                if (vel.Length() < 12f)
                                    vel = Vector2.Normalize(vel) * 12f;
                                Projectile.NewProjectile(npc.Center, vel, mod.ProjectileType("HorsemansBlade"),
                                    npc.damage / 5, 0f, Main.myPlayer, npc.target);
                            }
                        }
                        break;

                    case NPCID.SwampThing:
                        if (++Counter > 300)
                        {
                            Counter = 0;
                            if (Main.netMode != 1 && npc.HasPlayerTarget)
                                Projectile.NewProjectile(npc.Center, npc.DirectionTo(Main.player[npc.target].Center) * 12f,
                                    ProjectileID.DD2OgreSpit, npc.damage / 4, 0, Main.myPlayer);
                        }
                        break;

                    case NPCID.Zombie:
                    case NPCID.BaldZombie:
                    case NPCID.FemaleZombie:
                    case NPCID.PincushionZombie:
                    case NPCID.SlimedZombie:
                    case NPCID.TwiggyZombie:
                    case NPCID.ZombiePixie:
                    case NPCID.ZombieRaincoat:
                    case NPCID.ZombieSuperman:
                    case NPCID.ZombieSweater:
                    case NPCID.ZombieXmas:
                    case NPCID.SwampZombie:
                    case NPCID.SmallSwampZombie:
                    case NPCID.BigSwampZombie:
                    case NPCID.ZombieDoctor:
                        if (npc.ai[2] >= 45f && npc.ai[3] == 0f && Main.netMode != 1)
                        {
                            int tileX = (int)(npc.position.X + npc.width / 2 + 15 * npc.direction) / 16;
                            int tileY = (int)(npc.position.Y + npc.height - 15) / 16 - 1;
                            Tile tile = Framing.GetTileSafely(tileX, tileY);
                            if (tile.type == TileID.ClosedDoor || tile.type == TileID.TallGateClosed)
                            {
                                WorldGen.KillTile(tileX, tileY);
                                if (Main.netMode == 2)
                                    NetMessage.SendData(17, -1, -1, null, 0, tileX, tileY);
                            }
                        }
                        break;

                    default:
                        break;
                }
            }

            return true;
        }

        public void NetUpdateMaso(int npc) //MAKE SURE THAT YOU CALL THIS FROM THE GLOBALNPC INSTANCE OF THE NPC ITSELF
        {
            if (Main.netMode == 0)
                return;

            var netMessage = mod.GetPacket();
            netMessage.Write((byte)2);
            netMessage.Write((byte)npc);
            netMessage.Write(masoBool[0]); //these are the variables of the instance THAT CALLS THIS METHOD
            netMessage.Write(masoBool[1]); //rule of thumb is to only call this method server-side
            netMessage.Write(masoBool[2]);
            netMessage.Write(masoBool[3]);
            netMessage.Write(Counter);
            netMessage.Write(Counter2);
            netMessage.Write(Timer);
            netMessage.Send();
        }

        private void Horde(NPC npc, int size)
        {
            for (int i = 0; i < size; i++)
            {
                Vector2 pos = new Vector2(npc.Center.X + Main.rand.NextFloat(-2f, 2f) * npc.width, npc.Center.Y);
                if (!Collision.SolidCollision(pos, npc.width, npc.height) && Main.netMode != 1)
                {
                    int j = NPC.NewNPC((int)pos.X + npc.width / 2, (int)pos.Y + npc.height / 2, npc.type);
                    if (j != 200)
                    {
                        NPC newNPC = Main.npc[j];
                        newNPC.velocity = Vector2.UnitX.RotatedByRandom(2 * Math.PI) * 5f;
                        newNPC.GetGlobalNPC<FargoSoulsGlobalNPC>().FirstTick = true;
                        if (Main.netMode == 2)
                            NetMessage.SendData(23, -1, -1, null, j);
                    }
                }
            }
        }

        private void Aura(NPC npc, float distance, int buff, bool reverse = false, int dustid = DustID.GoldFlame, bool checkDuration = false)
        {
            //works because buffs are client side anyway :ech:
            Player p = Main.player[Main.myPlayer];
            float range = npc.Distance(p.Center);
            if (reverse ? range > distance && range < 3000f : range < distance)
                p.AddBuff(buff, checkDuration && Main.expertMode && Main.expertDebuffTime > 1 ? 1 : 2);

            for (int i = 0; i < 20; i++)
            {
                Vector2 offset = new Vector2();
                double angle = Main.rand.NextDouble() * 2d * Math.PI;
                offset.X += (float)(Math.Sin(angle) * distance);
                offset.Y += (float)(Math.Cos(angle) * distance);
                Dust dust = Main.dust[Dust.NewDust(
                    npc.Center + offset - new Vector2(4, 4), 0, 0,
                    dustid, 0, 0, 100, Color.White, 1f
                    )];
                dust.velocity = npc.velocity;
                if (Main.rand.Next(3) == 0)
                    dust.velocity += Vector2.Normalize(offset) * (reverse ? 5f : -5f);
                dust.noGravity = true;
            }
        }

        private void Shoot(NPC npc, int delay, float distance, int speed, int proj, int dmg, float kb, bool recolor = false, bool hostile = false)
        {
            int t = npc.HasPlayerTarget ? npc.target : npc.FindClosestPlayer();
            if (t == -1)
                return;

            Player player = Main.player[t];
            //npc facing player target or if already started attack
            if (player.active && !player.dead && npc.direction == (Math.Sign(player.position.X - npc.position.X)) || Stop > 0)
            {
                //start the pause
                if (delay != 0 && Stop == 0)
                {
                    Stop = delay;
                }
                //half way through start attack
                else if (delay == 0 || Stop == delay / 2)
                {
                    Vector2 velocity = Vector2.Normalize(player.Center - npc.Center) * speed;
                    if (npc.Distance(player.Center) < distance)
                        velocity = Vector2.Normalize(player.Center - npc.Center) * speed;
                    else //player too far away now, just shoot straight ahead
                        velocity = new Vector2(npc.direction * speed, 0);

                    int p = Projectile.NewProjectile(npc.Center, velocity, proj, dmg, kb, Main.myPlayer);
                    if (p < 1000)
                    {
                        if (recolor)
                            Main.projectile[p].GetGlobalProjectile<FargoGlobalProjectile>().IsRecolor = true;
                        if (hostile)
                        {
                            Main.projectile[p].friendly = false;
                            Main.projectile[p].hostile = true;
                        }
                    }
                    Counter = 0;
                }
            }

        }

        private void CustomReflect(NPC npc, int dustID, int ratio = 1)
        {
            float distance = 2f * 16;

            Main.projectile.Where(x => x.active && x.friendly && !x.minion).ToList().ForEach(x =>
           {
               if (Vector2.Distance(x.Center, npc.Center) <= distance)
               {
                   for (int i = 0; i < 5; i++)
                   {
                       int dustId = Dust.NewDust(new Vector2(x.position.X, x.position.Y + 2f), x.width, x.height + 5, dustID, x.velocity.X * 0.2f, x.velocity.Y * 0.2f, 100, default(Color), 1.5f);
                       Main.dust[dustId].noGravity = true;
                   }

                    // Set ownership
                    x.hostile = true;
                   x.friendly = false;
                   x.owner = Main.myPlayer;
                   x.damage /= ratio;

                    // Turn around
                    x.velocity *= -1f;

                    // Flip sprite
                    if (x.Center.X > npc.Center.X * 0.5f)
                   {
                       x.direction = 1;
                       x.spriteDirection = 1;
                   }
                   else
                   {
                       x.direction = -1;
                       x.spriteDirection = -1;
                   }

                    //x.netUpdate = true;
                }
           });
        }

        public override Color? GetAlpha(NPC npc, Color drawColor)
        {
            if (SharkCount != 0)
            {
                switch (SharkCount)
                {
                    case 253:
                        drawColor.R = 255;
                        drawColor.G /= 2;
                        drawColor.B /= 2;
                        break;

                    case 254:
                        drawColor.R /= 2;
                        drawColor.G = 255;
                        drawColor.B /= 2;
                        break;

                    default:
                        drawColor.R = (byte)(SharkCount * 20 + 155);
                        drawColor.G /= (byte)(SharkCount + 1);
                        drawColor.B /= (byte)(SharkCount + 1);
                        break;
                }

                return drawColor;
            }

            if (valhallaCounter > 900)
            {
                drawColor = Color.SandyBrown;
                return drawColor;
            }

            return null;
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (LeadPoison)
            {
                if (Main.rand.Next(4) < 3)
                {
                    int dust = Dust.NewDust(new Vector2(npc.position.X - 2f, npc.position.Y - 2f), npc.width + 4, npc.height + 4, DustID.Lead, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default(Color), 1f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Dust expr_1CCF_cp_0 = Main.dust[dust];
                    expr_1CCF_cp_0.velocity.Y = expr_1CCF_cp_0.velocity.Y - 0.5f;
                    if (Main.rand.Next(4) == 0)
                    {
                        Main.dust[dust].noGravity = false;
                        Main.dust[dust].scale *= 0.5f;
                    }
                }
            }

            if (HellFire)
            {
                if (Main.rand.Next(4) < 3)
                {
                    int dust = Dust.NewDust(new Vector2(npc.position.X - 2f, npc.position.Y - 2f), npc.width + 4, npc.height + 4, DustID.SolarFlare, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].shader = GameShaders.Armor.GetSecondaryShader(56, Main.LocalPlayer);

                    Dust expr_1CCF_cp_0 = Main.dust[dust];
                    expr_1CCF_cp_0.velocity.Y = expr_1CCF_cp_0.velocity.Y - 0.5f;
                    if (Main.rand.Next(4) == 0)
                    {
                        Main.dust[dust].noGravity = false;
                        Main.dust[dust].scale *= 0.5f;
                    }
                }
            }

            /*if (Infested)
            {
                if (Main.rand.Next(4) < 3)
                {
                    int dust = Dust.NewDust(new Vector2(npc.position.X - 2f, npc.position.Y - 2f), npc.width + 4, npc.height + 4, 44, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, Color.LimeGreen, InfestedDust);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Dust expr_1CCF_cp_0 = Main.dust[dust];
                    expr_1CCF_cp_0.velocity.Y = expr_1CCF_cp_0.velocity.Y - 0.5f;
                    if (Main.rand.Next(4) == 0)
                    {
                        Main.dust[dust].noGravity = false;
                        Main.dust[dust].scale *= 0.5f;
                    }
                }

                Lighting.AddLight((int)(npc.position.X / 16f), (int)(npc.position.Y / 16f + 1f), 1f, 0.3f, 0.1f);
            }*/

            if (Suffocation)
                drawColor = Colors.RarityPurple;

            if (Villain)
            {
                if (Main.rand.Next(4) < 3)
                {
                    int dust = Dust.NewDust(new Vector2(npc.position.X - 2f, npc.position.Y - 2f), npc.width + 4, npc.height + 4, DustID.AncientLight, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Dust expr_1CCF_cp_0 = Main.dust[dust];
                    expr_1CCF_cp_0.velocity.Y = expr_1CCF_cp_0.velocity.Y - 0.5f;
                    if (Main.rand.Next(4) == 0)
                    {
                        Main.dust[dust].noGravity = false;
                        Main.dust[dust].scale *= 0.5f;
                    }
                }

                Lighting.AddLight((int)(npc.position.X / 16f), (int)(npc.position.Y / 16f + 1f), 1f, 0.3f, 0.1f);
            }

            if (Electrified)
            {
                if (Main.rand.Next(4) < 3)
                {
                    int dust = Dust.NewDust(new Vector2(npc.position.X - 2f, npc.position.Y - 2f), npc.width + 4, npc.height + 4, 229, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    if (Main.rand.Next(3) == 0)
                    {
                        Main.dust[dust].noGravity = false;
                        Main.dust[dust].scale *= 0.5f;
                    }
                }

                Lighting.AddLight((int)npc.Center.X / 16, (int)npc.Center.Y / 16, 0.3f, 0.8f, 1.1f);
            }

            if (CurseoftheMoon)
            {
                int d = Dust.NewDust(npc.Center, 0, 0, 229, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 3f;
                Main.dust[d].scale += 0.5f;

                if (Main.rand.Next(4) < 3)
                {
                    d = Dust.NewDust(npc.position, npc.width, npc.height, 229, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity.Y -= 1f;
                    Main.dust[d].velocity *= 2f;
                }
            }

            if (Sadism)
            {
                int d = Dust.NewDust(npc.Center, 0, 0, 86, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 3f;
                Main.dust[d].scale += 1f;

                if (Main.rand.Next(4) < 3)
                {
                    d = Dust.NewDust(npc.position, npc.width, npc.height, 86, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity.Y -= 1f;
                    Main.dust[d].velocity *= 2f;
                    Main.dust[d].scale += 0.5f;
                }
            }

            if (GodEater)
            {
                if (Main.rand.Next(7) < 6)
                {
                    int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, 86, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default(Color), 3.5f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.2f;
                    Main.dust[dust].velocity.Y -= 0.15f;
                    if (Main.rand.Next(4) == 0)
                    {
                        Main.dust[dust].noGravity = false;
                        Main.dust[dust].scale *= 0.5f;
                    }
                }
                Lighting.AddLight(npc.position, 0.15f, 0.03f, 0.09f);
            }
        }

        public override void OnHitPlayer(NPC npc, Player target, int damage, bool crit)
        {
            if (FargoSoulsWorld.MasochistMode)
            {
                switch (npc.type)
                {
                    case NPCID.BlueSlime:
                        switch (npc.netID)
                        {
                            case NPCID.BlackSlime:
                                target.AddBuff(BuffID.Slimed, 120);
                                target.AddBuff(BuffID.Darkness, 600);
                                break;

                            case NPCID.Pinky:
                                target.AddBuff(BuffID.Slimed, 120);
                                target.AddBuff(mod.BuffType("Stunned"), 120);
                                Vector2 velocity = Vector2.Normalize(target.Center - npc.Center) * 30;
                                target.velocity = velocity;
                                break;

                            default:
                                target.AddBuff(BuffID.Slimed, 120);
                                break;
                        }
                        break;

                    case NPCID.SlimeRibbonGreen:
                    case NPCID.SlimeRibbonRed:
                    case NPCID.SlimeRibbonWhite:
                    case NPCID.SlimeRibbonYellow:
                        target.AddBuff(BuffID.Slimed, 120);
                        break;

                    case NPCID.UmbrellaSlime:
                        target.AddBuff(BuffID.Slimed, 120);
                        target.AddBuff(BuffID.Wet, 600);
                        break;

                    case NPCID.IceSlime:
                    case NPCID.SpikedIceSlime:
                        target.AddBuff(BuffID.Slimed, 120);
                        target.AddBuff(BuffID.Frostburn, 120);
                        break;

                    case NPCID.JungleSlime:
                    case NPCID.SpikedJungleSlime:
                        target.AddBuff(BuffID.Slimed, 120);
                        target.AddBuff(BuffID.Poisoned, 180);
                        break;

                    case NPCID.MotherSlime:
                        target.AddBuff(BuffID.Slimed, 120);
                        target.AddBuff(mod.BuffType("Antisocial"), 1200);
                        break;

                    case NPCID.LavaSlime:
                        target.AddBuff(mod.BuffType("Oiled"), 900);
                        target.AddBuff(BuffID.OnFire, 300);
                        break;

                    case NPCID.DungeonSlime:
                        target.AddBuff(BuffID.Slimed, 120);
                        target.AddBuff(BuffID.Blackout, 300);
                        break;

                    case NPCID.KingSlime:
                        target.AddBuff(BuffID.Slimed, 120);
                        if (npc.velocity.Length() > 0)
                            AddBuffNoStack(target, mod.BuffType("Stunned"));
                        break;
                        
                    case NPCID.ToxicSludge:
                        target.AddBuff(BuffID.Slimed, 120);
                        target.AddBuff(mod.BuffType("Infested"), 360);
                        break;

                    case NPCID.CorruptSlime:
                        target.AddBuff(BuffID.Slimed, 120);
                        target.AddBuff(mod.BuffType("Rotting"), 1200);
                        break;

                    case NPCID.Crimslime:
                        target.AddBuff(BuffID.Slimed, 120);
                        target.AddBuff(mod.BuffType("Bloodthirsty"), 300);
                        break;

                    case NPCID.Gastropod:
                        target.AddBuff(BuffID.Slimed, 120);
                        target.AddBuff(mod.BuffType("Fused"), 1800);
                        break;

                    case NPCID.IlluminantSlime:
                        target.AddBuff(BuffID.Slimed, 120);
                        target.AddBuff(mod.BuffType("Purified"), 300);
                        break;

                    case NPCID.RainbowSlime:
                        target.AddBuff(BuffID.Slimed, 120);
                        target.AddBuff(mod.BuffType("FlamesoftheUniverse"), 240);
                        break;

                    case NPCID.DemonEye:
                    case NPCID.DemonEyeOwl:
                    case NPCID.DemonEyeSpaceship:
                    case NPCID.CataractEye:
                    case NPCID.SleepyEye:
                    case NPCID.DialatedEye:
                    case NPCID.GreenEye:
                    case NPCID.PurpleEye:
                        target.AddBuff(BuffID.Obstructed, 60);
                        break;

                    case NPCID.EaterofSouls:
                    case NPCID.Crimera:
                        target.AddBuff(BuffID.Weak, 600);
                        break;

                    case NPCID.EyeofCthulhu:
                    case NPCID.WanderingEye:
                        target.AddBuff(BuffID.Obstructed, 60);
                        target.AddBuff(mod.BuffType("Berserked"), 300);
                        break;

                    case NPCID.ServantofCthulhu:
                        target.AddBuff(BuffID.Obstructed, 30);
                        target.AddBuff(mod.BuffType("Berserked"), 150);
                        break;

                    case NPCID.QueenBee:
                        target.AddBuff(mod.BuffType("Infested"), 300);
                        target.AddBuff(mod.BuffType("Defenseless"), 300);
                        target.AddBuff(mod.BuffType("Crippled"), 120);
                        target.AddBuff(mod.BuffType("ClippedWings"), 300);
                        break;

                    case NPCID.WallofFlesh:
                    case NPCID.WallofFleshEye:
                        target.AddBuff(BuffID.Burning, 300);
                        target.AddBuff(mod.BuffType("Unstable"), 300);
                        break;

                    case NPCID.TheHungry:
                    case NPCID.TheHungryII:
                        target.AddBuff(BuffID.OnFire, 300);
                        target.AddBuff(mod.BuffType("ClippedWings"), 360);
                        target.AddBuff(mod.BuffType("Crippled"), 180);
                        break;

                    case NPCID.EaterofWorldsHead:
                    case NPCID.EaterofWorldsBody:
                    case NPCID.EaterofWorldsTail:
                        target.AddBuff(mod.BuffType("Rotting"), 600);
                        break;

                    case NPCID.CursedSkull:
                        if (Main.rand.Next(2) == 0)
                            target.AddBuff(BuffID.Cursed, 60);
                        break;

                    case NPCID.Snatcher:
                    case NPCID.ManEater:
                        target.AddBuff(BuffID.Bleeding, 300);
                        if (target.statLife + damage < 100)
                            target.KillMe(PlayerDeathReason.ByCustomReason(target.name + " was eaten alive by a Man Eater."), 999, 0);
                        break;

                    case NPCID.DevourerHead:
                        target.AddBuff(BuffID.BrokenArmor, 600);
                        break;

                    case NPCID.AngryTrapper:
                        target.AddBuff(BuffID.Bleeding, 300);
                        if (target.statLife + damage < 180)
                            target.KillMe(PlayerDeathReason.ByCustomReason(target.name + " was eaten alive by an Angry Trapper."), 999, 0);
                        break;

                    case NPCID.SkeletronHead:
                        target.AddBuff(mod.BuffType("Defenseless"), 300);
                        target.AddBuff(mod.BuffType("Lethargic"), 300);
                        break;

                    case NPCID.SkeletronHand:
                        target.AddBuff(mod.BuffType("Lethargic"), 300);
                        if (Main.rand.Next(2) == 0)
                            target.AddBuff(BuffID.Dazed, 60);
                        break;

                    case NPCID.CaveBat:
                        target.AddBuff(BuffID.Bleeding, 300);
                        target.AddBuff(BuffID.Rabies, 3600);
                        break;

                    case NPCID.Hellbat:
                        target.AddBuff(BuffID.OnFire, 240);
                        target.AddBuff(BuffID.Rabies, 3600);
                        break;

                    case NPCID.JungleBat:
                        target.AddBuff(BuffID.Poisoned, 240);
                        target.AddBuff(BuffID.Rabies, 3600);
                        break;

                    case NPCID.IceBat:
                        target.AddBuff(BuffID.Rabies, 3600);
                        if (target.HasBuff(BuffID.Chilled))
                            AddBuffNoStack(target, BuffID.Frozen);
                        break;

                    case NPCID.Lavabat:
                        target.AddBuff(BuffID.OnFire, 240);
                        target.AddBuff(BuffID.Burning, 240);
                        target.AddBuff(BuffID.Rabies, 3600);
                        break;

                    case NPCID.GiantBat:
                        target.AddBuff(BuffID.Confused, 240);
                        target.AddBuff(BuffID.Rabies, 3600);
                        break;

                    case NPCID.IlluminantBat:
                        target.AddBuff(mod.BuffType("MutantNibble"), 1800);
                        target.AddBuff(BuffID.Rabies, 3600);
                        break;

                    case NPCID.GiantFlyingFox:
                        target.AddBuff(mod.BuffType("Bloodthirsty"), 300);
                        target.AddBuff(BuffID.Rabies, 3600);
                        break;

                    case NPCID.VampireBat:
                    case NPCID.Vampire:
                        target.AddBuff(BuffID.Weak, 600);
                        target.AddBuff(BuffID.Rabies, 3600);
                        npc.life += damage;
                        CombatText.NewText(npc.Hitbox, CombatText.HealLife, damage);
                        npc.damage = (int)(npc.damage * 1.1f);
                        npc.defDamage = (int)(npc.defDamage * 1.1f);
                        npc.netUpdate = true;
                        break;

                    case NPCID.SnowFlinx:
                        target.AddBuff(mod.BuffType("Purified"), 600);
                        break;

                    case NPCID.Medusa:
                        target.AddBuff(mod.BuffType("Flipped"), 120);
                        AddBuffNoStack(target, BuffID.Stoned);
                        break;

                    case NPCID.SpikeBall:
                        target.AddBuff(BuffID.BrokenArmor, 1200);
                        if (masoBool[0])
                            target.AddBuff(mod.BuffType("Defenseless"),1200);
                        break;

                    case NPCID.BlazingWheel:
                        target.AddBuff(BuffID.OnFire, 180);
                        if (masoBool[0])
                            target.AddBuff(mod.BuffType("FlamesoftheUniverse"), 300);
                        break;

                    case NPCID.Shark:
                    case NPCID.SandShark:
                    case NPCID.SandsharkCorrupt:
                    case NPCID.SandsharkCrimson:
                    case NPCID.SandsharkHallow:
                    case NPCID.Piranha:
                        target.AddBuff(BuffID.Bleeding, 300);
                        break;

                    case NPCID.GraniteFlyer:
                    case NPCID.GraniteGolem:
                        AddBuffNoStack(target, BuffID.Stoned);
                        break;

                    case NPCID.LeechHead:
                        target.AddBuff(mod.BuffType("Crippled"), 180);
                        target.AddBuff(BuffID.Bleeding, 300);
                        target.AddBuff(BuffID.Rabies, 600);
                        break;

                    case NPCID.AnomuraFungus:
                    case NPCID.FungiSpore:
                        target.AddBuff(BuffID.Poisoned, 300);
                        break;

                    case NPCID.MushiLadybug:
                        target.AddBuff(BuffID.Darkness, 300);
                        target.AddBuff(BuffID.Blackout, 300);
                        break;

                    case NPCID.WaterSphere:
                        target.AddBuff(BuffID.Wet, 1200);
                        target.AddBuff(BuffID.Silenced, 120);
                        break;

                    case NPCID.GiantShelly:
                    case NPCID.GiantShelly2:
                        target.AddBuff(BuffID.Slow, 120);
                        break;

                    case NPCID.Squid:
                        target.AddBuff(BuffID.Obstructed, 120);
                        break;

                    case NPCID.BloodZombie:
                        target.AddBuff(mod.BuffType("Bloodthirsty"), 240);
                        break;

                    case NPCID.Drippler:
                        target.AddBuff(mod.BuffType("Rotting"), 600);
                        break;

                    case NPCID.ChaosBall:
                    case NPCID.ShadowFlameApparition:
                        target.AddBuff(mod.BuffType("Shadowflame"), 300);
                        break;

                    case NPCID.Tumbleweed:
                        target.AddBuff(mod.BuffType("Crippled"), 300);
                        break;

                    case NPCID.PigronCorruption:
                    case NPCID.PigronCrimson:
                    case NPCID.PigronHallow:
                        target.AddBuff(mod.BuffType("SqueakyToy"), 120);
                        target.GetModPlayer<FargoPlayer>().MaxLifeReduction += 50;
                        target.AddBuff(mod.BuffType("OceanicMaul"), 1800);
                        break;

                    case NPCID.CorruptBunny:
                    case NPCID.CrimsonBunny:
                    case NPCID.CorruptGoldfish:
                    case NPCID.CrimsonGoldfish:
                    case NPCID.CorruptPenguin:
                    case NPCID.CrimsonPenguin:
                    case NPCID.GingerbreadMan:
                        target.AddBuff(mod.BuffType("SqueakyToy"), 120);
                        break;

                    case NPCID.FaceMonster:
                        target.AddBuff(BuffID.Rabies, 900);
                        break;

                    case NPCID.SeaSnail:
                        target.AddBuff(BuffID.OgreSpit, 300);
                        break;

                    case NPCID.BrainofCthulhu:
                    case NPCID.Creeper:
                        target.AddBuff(BuffID.Poisoned, 120);
                        target.AddBuff(BuffID.Darkness, 120);
                        target.AddBuff(BuffID.Bleeding, 120);
                        target.AddBuff(BuffID.Slow, 120);
                        target.AddBuff(BuffID.Weak, 120);
                        target.AddBuff(BuffID.BrokenArmor, 120);
                        break;

                    case NPCID.SwampThing:
                        target.AddBuff(BuffID.OgreSpit, 300);
                        break;

                    case NPCID.Frankenstein:
                        target.AddBuff(mod.BuffType("LightningRod"), 600);
                        break;

                    case NPCID.Butcher:
                        target.AddBuff(mod.BuffType("Berserked"), 600);
                        target.AddBuff(BuffID.Bleeding, 600);
                        break;

                    case NPCID.ThePossessed:
                        target.AddBuff(mod.BuffType("Hexed"), 240);
                        break;

                    case NPCID.Wolf:
                        target.AddBuff(mod.BuffType("Crippled"), 300);
                        target.AddBuff(BuffID.Rabies, 900);
                        break;

                    case NPCID.Werewolf:
                        target.AddBuff(BuffID.Rabies, 1800);
                        break;

                    //all armored bones
                    case 269:
                    case 270:
                    case 271:
                    case 272:
                    case 273:
                    case 274:
                    case 275:
                    case 276:
                    case 277:
                    case 278:
                    case 279:
                    case 280:
                        target.AddBuff(mod.BuffType("Bloodthirsty"), 180);
                        break;

                    case NPCID.GiantTortoise:
                        target.AddBuff(mod.BuffType("Defenseless"), 300);
                        break;

                    case NPCID.IceTortoise:
                        target.AddBuff(mod.BuffType("Defenseless"), 300);
                        if (Main.rand.Next(3) == 0)
                            target.AddBuff(BuffID.Frozen, 60);
                        break;

                    case NPCID.AncientDoom:
                        target.AddBuff(mod.BuffType("MarkedforDeath"), 120);
                        target.AddBuff(mod.BuffType("Shadowflame"), 300);
                        break;
                    case NPCID.AncientLight:
                        target.AddBuff(mod.BuffType("Purified"), 300);
                        break;
                    case NPCID.CultistBossClone:
                        target.AddBuff(mod.BuffType("MarkedforDeath"), 180);
                        target.AddBuff(mod.BuffType("CurseoftheMoon"), 300);
                        break;
                    case NPCID.CultistBoss:
                        target.AddBuff(mod.BuffType("MarkedforDeath"), 180);
                        target.AddBuff(mod.BuffType("CurseoftheMoon"), 300);
                        break;

                    case NPCID.BigMossHornet:
                    case NPCID.GiantMossHornet:
                    case NPCID.LittleMossHornet:
                    case NPCID.MossHornet:
                    case NPCID.TinyMossHornet:
                        target.AddBuff(mod.BuffType("Infested"), 300);
                        target.AddBuff(mod.BuffType("Swarming"), 600);
                        break;

                    case NPCID.Paladin:
                        target.AddBuff(mod.BuffType("Lethargic"), 600);
                        break;

                    case NPCID.DukeFishron:
                        target.AddBuff(mod.BuffType("MutantNibble"), 600);
                        target.AddBuff(mod.BuffType("Defenseless"), 600);
                        target.AddBuff(BuffID.Rabies, 3600);
                        target.GetModPlayer<FargoPlayer>().MaxLifeReduction += 100;
                        target.AddBuff(mod.BuffType("OceanicMaul"), 3600);
                        break;

                    case NPCID.Sharkron:
                    case NPCID.Sharkron2:
                        target.AddBuff(mod.BuffType("Defenseless"), 600);
                        target.AddBuff(mod.BuffType("MutantNibble"), 300);
                        //target.AddBuff(BuffID.Rabies, 3600);
                        if (BossIsAlive(ref fishBossEX, NPCID.DukeFishron))
                        {
                            target.GetModPlayer<FargoPlayer>().MaxLifeReduction += 50;
                            target.AddBuff(mod.BuffType("OceanicMaul"), 1800);
                        }
                        break;

                    case NPCID.DetonatingBubble:
                        if (BossIsAlive(ref fishBossEX, NPCID.DukeFishron))
                        {
                            target.AddBuff(mod.BuffType("Defenseless"), 600);
                            target.GetModPlayer<FargoPlayer>().MaxLifeReduction += 50;
                            target.AddBuff(mod.BuffType("OceanicMaul"), 1800);
                        }
                        break;

                    case NPCID.Hellhound:
                        target.AddBuff(BuffID.Rabies, 3600);
                        target.AddBuff(mod.BuffType("MutantNibble"), 600);
                        break;

                    case NPCID.Mimic:
                    case NPCID.PresentMimic:
                        target.AddBuff(mod.BuffType("Purified"), 300);
                        break;

                    case NPCID.BigMimicCorruption:
                        target.AddBuff(BuffID.CursedInferno, 180);
                        target.AddBuff(mod.BuffType("Berserked"), 300);
                        goto case NPCID.Mimic;

                    case NPCID.BigMimicCrimson:
                        target.AddBuff(BuffID.Ichor, 180);
                        target.AddBuff(mod.BuffType("Berserked"), 300);
                        goto case NPCID.Mimic;

                    case NPCID.BigMimicHallow:
                        target.AddBuff(BuffID.Confused, 180);
                        target.AddBuff(mod.BuffType("Berserked"), 300);
                        goto case NPCID.Mimic;

                    case NPCID.BigMimicJungle:
                        target.AddBuff(BuffID.Suffocation, 180);
                        target.AddBuff(mod.BuffType("Berserked"), 300);
                        goto case NPCID.Mimic;

                    case NPCID.RuneWizard:
                        target.AddBuff(mod.BuffType("MarkedforDeath"), 180);
                        target.AddBuff(mod.BuffType("Unstable"), 30);
                        target.AddBuff(BuffID.Suffocation, 300);
                        break;

                    case NPCID.Reaper:
                        //target.AddBuff(mod.BuffType("Rotting"), Main.rand.Next(1200, 7200));
                        target.AddBuff(mod.BuffType("LivingWasteland"), 900);
                        target.AddBuff(mod.BuffType("MarkedforDeath"), 600);
                        break;

                    case NPCID.Plantera:
                        target.AddBuff(mod.BuffType("MutantNibble"), 600);
                        target.AddBuff(mod.BuffType("Infested"), 300);
                        target.AddBuff(mod.BuffType("IvyVenom"), 300);
                        break;

                    case NPCID.PlanterasHook:
                    case NPCID.PlanterasTentacle:
                    case NPCID.Spore:
                        target.AddBuff(mod.BuffType("Infested"), 300);
                        target.AddBuff(mod.BuffType("IvyVenom"), 300);
                        break;

                    case NPCID.ChaosElemental:
                        target.AddBuff(mod.BuffType("Unstable"), 240);
                        break;

                    case NPCID.GoblinThief:
                        if (target.whoAmI == Main.myPlayer && !target.GetModPlayer<FargoPlayer>().SecurityWallet && Main.rand.Next(2) == 0)
                        {
                            //try stealing mouse item, then selected item
                            if (!StealFromInventory(target, ref Main.mouseItem))
                                StealFromInventory(target, ref target.inventory[target.selectedItem]);

                            /*byte extraTries = 30;
                            for (int i = 0; i < 3; i++)
                            {
                                bool successfulSteal = StealFromInventory(target, ref target.inventory[Main.rand.Next(target.inventory.Length)]);

                                if (!successfulSteal && extraTries > 0)
                                {
                                    extraTries--;
                                    i--;
                                }
                            }*/
                        }
                        target.AddBuff(mod.BuffType("Midas"), 600);
                        if (Main.hardMode)
                            target.AddBuff(mod.BuffType("LivingWasteland"), 600);
                        break;

                    case NPCID.PirateCaptain:
                    case NPCID.PirateCorsair:
                    case NPCID.PirateCrossbower:
                    case NPCID.PirateDeadeye:
                    case NPCID.PirateShipCannon:
                    case NPCID.PirateDeckhand:
                        target.AddBuff(mod.BuffType("Midas"), 600);
                        target.AddBuff(mod.BuffType("LivingWasteland"), 600);
                        break;

                    case NPCID.Parrot:
                        target.AddBuff(mod.BuffType("SqueakyToy"), 120);
                        target.AddBuff(mod.BuffType("Midas"), 600);
                        target.AddBuff(mod.BuffType("LivingWasteland"), 600);
                        break;

                    case NPCID.GoblinPeon:
                    case NPCID.GoblinSorcerer:
                    case NPCID.GoblinWarrior:
                    case NPCID.GoblinArcher:
                    case NPCID.GoblinSummoner:
                    case NPCID.GoblinScout:
                        if (Main.hardMode)
                            target.AddBuff(mod.BuffType("LivingWasteland"), 600);
                        break;

                    case NPCID.ScutlixRider:
                    case NPCID.GigaZapper:
                    case NPCID.MartianEngineer:
                    case NPCID.MartianOfficer:
                    case NPCID.RayGunner:
                    case NPCID.GrayGrunt:
                    case NPCID.BrainScrambler:
                    case NPCID.MartianDrone:
                    case NPCID.MartianWalker:
                    case NPCID.MartianTurret:
                        target.AddBuff(BuffID.Electrified, 300);
                        target.AddBuff(mod.BuffType("LivingWasteland"), 600);
                        break;

                    case NPCID.Scutlix:
                        target.AddBuff(mod.BuffType("SqueakyToy"), 180);
                        target.AddBuff(mod.BuffType("LivingWasteland"), 600);
                        break;

                    case NPCID.Zombie:
                    case NPCID.ArmedZombie:
                    case NPCID.ArmedZombieCenx:
                    case NPCID.ArmedZombiePincussion:
                    case NPCID.ArmedZombieSlimed:
                    case NPCID.ArmedZombieSwamp:
                    case NPCID.ArmedZombieTwiggy:
                    case NPCID.BaldZombie:
                    case NPCID.FemaleZombie:
                    case NPCID.PincushionZombie:
                    case NPCID.SlimedZombie:
                    case NPCID.TwiggyZombie:
                    case NPCID.ZombiePixie:
                    case NPCID.ZombieRaincoat:
                    case NPCID.ZombieSuperman:
                    case NPCID.ZombieSweater:
                    case NPCID.ZombieXmas:
                    case NPCID.SwampZombie:
                    case NPCID.SmallSwampZombie:
                    case NPCID.BigSwampZombie:
                    case NPCID.ZombieDoctor:
                        target.AddBuff(mod.BuffType("Rotting"), 300);
                        break;

                    case NPCID.ZombieMushroom:
                    case NPCID.ZombieMushroomHat:
                        target.AddBuff(mod.BuffType("Infested"), 300);
                        goto case NPCID.Zombie;

                    case NPCID.ZombieEskimo:
                    case NPCID.ArmedZombieEskimo:
                        target.AddBuff(BuffID.Chilled, 240);
                        goto case NPCID.Zombie;

                    case NPCID.Corruptor:
                        target.AddBuff(BuffID.Weak, 600);
                        target.AddBuff(mod.BuffType("Rotting"), 900);
                        break;

                    case NPCID.Mummy:
                    case NPCID.LightMummy:
                    case NPCID.DarkMummy:
                        AddBuffNoStack(target, BuffID.Webbed);
                        break;

                    case NPCID.Derpling:
                        target.AddBuff(mod.BuffType("Lethargic"), 900);
                        break;

                    case NPCID.Spazmatism:
                        if (npc.ai[0] >= 4f)
                            target.AddBuff(BuffID.CursedInferno, 300);
                        break;

                    case NPCID.TheDestroyer:
                    case NPCID.TheDestroyerBody:
                    case NPCID.TheDestroyerTail:
                        target.AddBuff(mod.BuffType("LightningRod"), 600);
                        break;

                    case NPCID.SkeletronPrime:
                        target.AddBuff(mod.BuffType("Defenseless"), 480);
                        break;

                    case NPCID.PrimeVice:
                        AddBuffNoStack(target, mod.BuffType("Stunned"));
                        break;

                    case NPCID.DesertBeast:
                        target.AddBuff(mod.BuffType("Infested"), 600);
                        AddBuffNoStack(target, BuffID.Stoned);
                        break;

                    case NPCID.FlyingSnake:
                        target.AddBuff(mod.BuffType("Infested"), 300);
                        target.AddBuff(mod.BuffType("ClippedWings"), 600);
                        break;

                    case NPCID.Lihzahrd:
                    case NPCID.LihzahrdCrawler:
                        target.AddBuff(mod.BuffType("Infested"), 300);
                        target.AddBuff(mod.BuffType("Bloodthirsty"), 120);
                        break;

                    case NPCID.CultistDragonHead:
                        target.AddBuff(mod.BuffType("CurseoftheMoon"), 300);
                        target.AddBuff(mod.BuffType("MutantNibble"), 300);
                        break;

                    case NPCID.AncientCultistSquidhead:
                        if (Main.rand.Next(2) == 0)
                            target.AddBuff(mod.BuffType("Flipped"), 240);
                        else
                            target.AddBuff(mod.BuffType("Unstable"), 60);
                        break;

                    case NPCID.SolarCrawltipedeHead:
                        target.AddBuff(BuffID.OnFire, 900);
                        target.AddBuff(mod.BuffType("Defenseless"), 900);
                        break;

                    case NPCID.BoneLee:
                        target.AddBuff(BuffID.Obstructed, 60);
                        target.velocity.X = npc.velocity.Length() * npc.direction;
                        break;

                    case NPCID.MourningWood:
                        target.AddBuff(BuffID.OnFire, 180);
                        target.AddBuff(BuffID.CursedInferno, 180);
                        target.AddBuff(mod.BuffType("Shadowflame"), 180);
                        break;

                    case NPCID.Poltergeist:
                        target.AddBuff(BuffID.Silenced, 180);
                        break;

                    case NPCID.Pumpking:
                    case NPCID.PumpkingBlade:
                        target.AddBuff(BuffID.Weak, 900);
                        target.AddBuff(mod.BuffType("Rotting"), 900);
                        target.AddBuff(mod.BuffType("LivingWasteland"), 900);
                        break;

                    case NPCID.Flocko:
                        target.AddBuff(BuffID.Chilled, 180);
                        target.AddBuff(BuffID.Frostburn, 180);
                        break;

                    case NPCID.IceQueen:
                        target.AddBuff(BuffID.Chilled, 180);
                        target.AddBuff(BuffID.Frostburn, 180);
                        AddBuffNoStack(target, BuffID.Frozen);
                        break;

                    case NPCID.VortexLarva:
                    case NPCID.VortexHornet:
                    case NPCID.VortexHornetQueen:
                    case NPCID.VortexSoldier:
                    case NPCID.VortexRifleman:
                        target.AddBuff(mod.BuffType("LightningRod"), 600);
                        target.AddBuff(mod.BuffType("ClippedWings"), 300);
                        break;

                    case NPCID.ZombieElf:
                    case NPCID.ZombieElfBeard:
                    case NPCID.ZombieElfGirl:
                        target.AddBuff(mod.BuffType("Rotting"), 900);
                        break;

                    case NPCID.Krampus:
                        if (target.whoAmI == Main.myPlayer && !target.GetModPlayer<FargoPlayer>().SecurityWallet)
                        {
                            //try stealing mouse item, then selected item
                            if (!StealFromInventory(target, ref Main.mouseItem))
                                StealFromInventory(target, ref target.inventory[target.selectedItem]);

                            for (int i = 0; i < 15; i++)
                            {
                                int toss = Main.rand.Next(3, 8 + target.extraAccessorySlots); //pick random accessory slot
                                if (Main.rand.Next(3) == 0 && target.armor[toss + 10].stack > 0) //chance to pick vanity slot if accessory is there
                                    toss += 10;
                                if (StealFromInventory(target, ref target.armor[toss]))
                                    break;
                            }
                        }
                        break;

                    case NPCID.Harpy:
                        if (target.whoAmI == Main.myPlayer && !target.GetModPlayer<FargoPlayer>().SecurityWallet)
                        {
                            for (int j = 0; j < target.inventory.Length; j++)
                            {
                                Item item = target.inventory[j];
                                if (item.healLife > 0)
                                {
                                    StealFromInventory(target, ref target.inventory[j]);
                                    break;
                                }
                            }
                        }
                        break;
                    case NPCID.Wraith:
                        target.AddBuff(mod.BuffType("LivingWasteland"), 600);
                        if (target.whoAmI == Main.myPlayer && !target.GetModPlayer<FargoPlayer>().SecurityWallet)
                        {
                            for (int j = 0; j < target.inventory.Length; j++)
                            {
                                Item item = target.inventory[j];

                                if (item.type == ItemID.SoulofFlight || item.type == ItemID.SoulofFright || item.type == ItemID.SoulofLight || item.type == ItemID.SoulofMight || item.type == ItemID.SoulofNight || item.type == ItemID.SoulofSight)
                                {
                                    StealFromInventory(target, ref target.inventory[j]);
                                }
                            }
                        }
                        break;

                    case NPCID.Clown:
                        target.AddBuff(mod.BuffType("Fused"), 1800);
                        target.AddBuff(mod.BuffType("Hexed"), 600);
                        break;

                    case NPCID.UndeadMiner:
                        target.AddBuff(mod.BuffType("Lethargic"), 600);
                        target.AddBuff(BuffID.Blackout, 300);
                        target.AddBuff(BuffID.NoBuilding, 300);
                        if (target.whoAmI == Main.myPlayer && !target.GetModPlayer<FargoPlayer>().SecurityWallet)
                            for (int i = 0; i < 59; i++)
                                if (target.inventory[i].pick != 0 || target.inventory[i].hammer != 0 || target.inventory[i].axe != 0)
                                    StealFromInventory(target, ref target.inventory[i]);
                        break;

                    case NPCID.Golem:
                    case NPCID.GolemHead:
                    case NPCID.GolemHeadFree:
                    case NPCID.GolemFistLeft:
                    case NPCID.GolemFistRight:
                        target.AddBuff(BuffID.BrokenArmor, 600);
                        target.AddBuff(mod.BuffType("Defenseless"), 600);
                        target.AddBuff(BuffID.WitheredArmor, 600);
                        break;

                    case NPCID.DD2Betsy:
                        //target.AddBuff(BuffID.OnFire, Main.rand.Next(900, 1800));
                        //target.AddBuff(BuffID.Rabies, Main.rand.Next(3600, 7200));
                        //target.AddBuff(BuffID.Ichor, Main.rand.Next(600, 900));
                        //target.AddBuff(BuffID.Burning, Main.rand.Next(300, 600));
                        target.AddBuff(BuffID.WitheredArmor, 600);
                        target.AddBuff(BuffID.WitheredWeapon, 600);
                        target.AddBuff(mod.BuffType("MutantNibble"), 600);
                        break;

                    case NPCID.DD2WyvernT1:
                    case NPCID.DD2WyvernT2:
                    case NPCID.DD2WyvernT3:
                        target.AddBuff(mod.BuffType("MutantNibble"), 300);
                        target.AddBuff(BuffID.Rabies, 3600);
                        break;

                    case NPCID.DD2KoboldFlyerT2:
                    case NPCID.DD2KoboldFlyerT3:
                    case NPCID.DD2KoboldWalkerT2:
                    case NPCID.DD2KoboldWalkerT3:
                        target.AddBuff(mod.BuffType("Fused"), 1800);
                        break;

                    case NPCID.DD2OgreT2:
                    case NPCID.DD2OgreT3:
                        target.AddBuff(mod.BuffType("Stunned"), 30);
                        target.AddBuff(mod.BuffType("Defenseless"), 300);
                        target.AddBuff(BuffID.BrokenArmor, 300);
                        break;

                    case NPCID.DD2LightningBugT3:
                        target.AddBuff(BuffID.Electrified, 300);
                        AddBuffNoStack(target, BuffID.Webbed);
                        break;

                    case NPCID.DD2SkeletonT1:
                    case NPCID.DD2SkeletonT3:
                        target.AddBuff(mod.BuffType("Shadowflame"), 300);
                        target.AddBuff(mod.BuffType("Rotting"), 1200);
                        break;

                    case NPCID.DD2GoblinT1:
                    case NPCID.DD2GoblinT2:
                    case NPCID.DD2GoblinT3:
                        target.AddBuff(BuffID.Poisoned, 300);
                        target.AddBuff(BuffID.Bleeding, 300);
                        break;

                    case NPCID.SolarSpearman:
                    case NPCID.SolarSroller:
                        target.AddBuff(BuffID.OnFire, 600);
                        target.AddBuff(BuffID.Burning, 300);
                        break;

                    case NPCID.SolarCorite:
                        target.AddBuff(BuffID.Slow, 300);
                        target.AddBuff(BuffID.OnFire, 600);
                        break;

                    case NPCID.SolarSolenian:
                        target.AddBuff(BuffID.Slow, 300);
                        target.AddBuff(BuffID.OnFire, 600);
                        target.AddBuff(BuffID.Ichor, 900);
                        break;

                    case NPCID.SolarDrakomire:
                    case NPCID.SolarDrakomireRider:
                        target.AddBuff(BuffID.OnFire, 600);
                        target.AddBuff(BuffID.Rabies, 3600);
                        break;

                    case NPCID.DesertScorpionWalk:
                    case NPCID.DesertScorpionWall:
                        target.AddBuff(mod.BuffType("MarkedforDeath"), 180);
                        break;

                    case NPCID.MisterStabby:
                        target.AddBuff(mod.BuffType("MarkedforDeath"), 180);
                        target.AddBuff(BuffID.Chilled, 180);
                        break;

                    case NPCID.SnowBalla:
                    case NPCID.SnowmanGangsta:
                        target.AddBuff(BuffID.Chilled, 180);
                        target.AddBuff(BuffID.Frostburn, 300);
                        break;

                    case NPCID.NebulaHeadcrab:
                        target.AddBuff(mod.BuffType("Unstable"), 90);
                        target.AddBuff(mod.BuffType("Flipped"), 90);
                        break;
                        
                    case NPCID.NebulaBeast:
                        target.AddBuff(BuffID.Rabies, 3600);
                        target.AddBuff(BuffID.Silenced, 180);
                        break;

                    case NPCID.NebulaBrain:
                        target.AddBuff(BuffID.Silenced, 180);
                        break;

                    case NPCID.StardustCellBig:
                    case NPCID.StardustCellSmall:
                        target.AddBuff(mod.BuffType("SqueakyToy"), 120);
                        target.AddBuff(BuffID.Frostburn, 180);
                        break;

                    case NPCID.StardustWormHead:
                    case NPCID.StardustWormBody:
                    case NPCID.StardustWormTail:
                        target.AddBuff(mod.BuffType("Berserked"), 300);
                        target.AddBuff(BuffID.Frostburn, 180);
                        break;

                    case NPCID.StardustSpiderBig:
                    case NPCID.StardustSpiderSmall:
                        target.AddBuff(BuffID.Frostburn, 180);
                        break;

                    case NPCID.MoonLordFreeEye:
                    case NPCID.MoonLordHand:
                    case NPCID.MoonLordHead:
                    case NPCID.LunarTowerNebula:
                    case NPCID.LunarTowerSolar:
                    case NPCID.LunarTowerStardust:
                    case NPCID.LunarTowerVortex:
                        target.AddBuff(mod.BuffType("CurseoftheMoon"), 600);
                        break;

                    case NPCID.BoneSerpentHead:
                        target.AddBuff(BuffID.Burning, 300);
                        break;

                    case NPCID.Salamander:
                    case NPCID.Salamander2:
                    case NPCID.Salamander3:
                    case NPCID.Salamander4:
                    case NPCID.Salamander5:
                    case NPCID.Salamander6:
                    case NPCID.Salamander7:
                    case NPCID.Salamander8:
                    case NPCID.Salamander9:
                        target.AddBuff(BuffID.Poisoned, 300);
                        break;

                    case NPCID.VileSpit:
                        target.AddBuff(mod.BuffType("Rotting"), 240);
                        break;

                    case NPCID.DungeonGuardian:
                        target.AddBuff(mod.BuffType("GodEater"), 420);
                        target.AddBuff(mod.BuffType("FlamesoftheUniverse"), 420);
                        target.AddBuff(mod.BuffType("MarkedforDeath"), 420);
                        target.immune = false;
                        target.immuneTime = 0;
                        break;

                    case NPCID.EnchantedSword:
                        target.AddBuff(mod.BuffType("Purified"), 300);
                        break;

                    case NPCID.CursedHammer:
                        target.AddBuff(mod.BuffType("Defenseless"), 300);
                        break;

                    case NPCID.CrimsonAxe:
                        target.AddBuff(mod.BuffType("Infested"), 300);
                        break;

                    case NPCID.WalkingAntlion:
                        AddBuffNoStack(target, BuffID.Dazed);
                        break;

                    case NPCID.AnglerFish:
                        target.AddBuff(BuffID.Bleeding, 300);
                        break;

                    case NPCID.BloodFeeder:
                        target.AddBuff(BuffID.Bleeding, 300);
                        npc.life += damage * 2;
                        if (npc.life > npc.lifeMax)
                            npc.life = npc.lifeMax;
                        CombatText.NewText(npc.Hitbox, CombatText.HealLife, damage * 2);
                        npc.damage = (int)(npc.damage * 1.1f);
                        npc.defDamage = (int)(npc.defDamage * 1.1f);
                        npc.netUpdate = true;
                        break;

                    case NPCID.Psycho:
                        target.AddBuff(BuffID.Obstructed, 120);
                        break;

                    case NPCID.LostGirl:
                    case NPCID.Nymph:
                        target.AddBuff(BuffID.Slow, 240);
                        target.AddBuff(BuffID.Weak, 480);
                        target.AddBuff(mod.BuffType("Lovestruck"), 240);
                        npc.life += damage * 2;
                        if (npc.life > npc.lifeMax)
                            npc.life = npc.lifeMax;
                        CombatText.NewText(npc.Hitbox, CombatText.HealLife, damage * 2);
                        npc.netUpdate = true;
                        break;

                    case NPCID.FloatyGross:
                        target.AddBuff(BuffID.OgreSpit, 240);
                        break;

                    case NPCID.WyvernHead:
                    case NPCID.WyvernBody:
                    case NPCID.WyvernBody2:
                    case NPCID.WyvernBody3:
                    case NPCID.WyvernLegs:
                    case NPCID.WyvernTail:
                        target.AddBuff(mod.BuffType("Purified"), 300);
                        target.AddBuff(mod.BuffType("Crippled"), 240);
                        target.AddBuff(mod.BuffType("ClippedWings"), 240);
                        break;

                    case NPCID.DuneSplicerHead:
                    case NPCID.DuneSplicerBody:
                    case NPCID.DuneSplicerTail:
                        target.AddBuff(mod.BuffType("ClippedWings"), 300);
                        break;

                    case NPCID.BloodCrawler:
                    case NPCID.BloodCrawlerWall:
                    case NPCID.JungleCreeper:
                    case NPCID.JungleCreeperWall:
                    case NPCID.WallCreeper:
                    case NPCID.WallCreeperWall:
                        target.AddBuff(mod.BuffType("Infested"), 300);
                        break;

                    case NPCID.BlackRecluse:
                    case NPCID.BlackRecluseWall:
                        target.AddBuff(BuffID.Poisoned, 300);
                        target.AddBuff(mod.BuffType("Rotting"), 600);
                        target.AddBuff(mod.BuffType("Infested"), 300);
                        break;

                    case NPCID.DungeonSpirit:
                        target.AddBuff(BuffID.Cursed, 240);
                        break;

                    case NPCID.Mothron:
                        target.AddBuff(BuffID.Rabies, 3600);
                        AddBuffNoStack(target, mod.BuffType("Stunned"));
                        break;

                    case NPCID.MothronSpawn:
                        target.AddBuff(BuffID.Rabies, 1800);
                        target.AddBuff(mod.BuffType("Guilty"), 600);
                        break;

                    case NPCID.DeadlySphere:
                        target.AddBuff(BuffID.Darkness, 300);
                        target.AddBuff(BuffID.Blackout, 300);
                        break;

                    case NPCID.HeadlessHorseman:
                        target.AddBuff(BuffID.Cursed, 60);
                        target.AddBuff(mod.BuffType("LivingWasteland"), 600);
                        break;

                    case NPCID.Yeti:
                        AddBuffNoStack(target, BuffID.Frozen);
                        break;

                    default:
                        break;
                }
            }
        }

        private void SpawnRazorbladeRing(NPC npc, int max, float speed, int damage, float rotationModifier, bool reduceTimeleft = false)
        {
            if (Main.netMode == 1)
                return;
            float rotation = 2f * (float)Math.PI / max;
            Vector2 vel = Main.player[npc.target].Center - npc.Center;
            vel.Normalize();
            vel *= speed;
            int type = mod.ProjectileType("RazorbladeTyphoon");
            for (int i = 0; i < max; i++)
            {
                vel = vel.RotatedBy(rotation);
                int p = Projectile.NewProjectile(npc.Center, vel, type, damage, 0f, Main.myPlayer, rotationModifier * npc.spriteDirection, speed);
                if (reduceTimeleft && p < 1000)
                    Main.projectile[p].timeLeft /= 2;
            }
            Main.PlaySound(SoundID.Item84, npc.Center);
        }

        public void ResetRegenTimer(NPC npc)
        {
            //8 sec
            npc.GetGlobalNPC<FargoSoulsGlobalNPC>().RegenTimer = 480;
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            Player player = Main.player[Main.myPlayer];
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();
            int dmg;

            if (FargoSoulsWorld.MasochistMode)
            {
                if (!npc.dontTakeDamage && !npc.friendly && RegenTimer <= 0)
                {
                    npc.lifeRegen += 1 + npc.lifeMax / 25;
                }
            }

            //20 dps
            if (SBleed)
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                npc.lifeRegen -= 40;
                if (damage < 20)
                {
                    damage = 20;
                }

                if (Main.rand.Next(4) == 0)
                {
                    dmg = 20;
                    int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 40, 0f + Main.rand.Next(-5, 5), -5f, mod.ProjectileType("SuperBlood"), dmg, 0f, Main.myPlayer);
                    if (p < 1000)
                        Main.projectile[p].GetGlobalProjectile<FargoGlobalProjectile>().CanSplit = false;
                }
            }

            if (Rotting)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;

                npc.lifeRegen -= 100;

                if (damage < 5)
                    damage = 5;
            }

            if (LeadPoison)
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }

                npc.lifeRegen -= 10;

                for (int i = 0; i < 200; i++)
                {
                    NPC spread = Main.npc[i];

                    if (spread.active && !spread.townNPC && !spread.friendly && spread.lifeMax > 5 && !spread.HasBuff(mod.BuffType("LeadPoison")) && Vector2.Distance(npc.Center, spread.Center) < 50 && Collision.CanHitLine(npc.Center, 0, 0, spread.Center, 0, 0))
                    {
                        spread.AddBuff(mod.BuffType("LeadPoison"), 120);
                    }
                }
            }

            //50 dps
            if (SolarFlare)
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }

                npc.lifeRegen -= 100;

                if (damage < 10)
                {
                    damage = 10;
                }
            }

            //.5% dps
            if (HellFire)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;

                npc.lifeRegen -= npc.lifeMax / 100;

                if (damage < npc.lifeMax / 1000)
                    damage = npc.lifeMax / 1000;
            }

            if (Infested)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;

                npc.lifeRegen -= InfestedExtraDot(npc);

                if (damage < 8)
                    damage = 8;
            }
            else
            {
                MaxInfestTime = 0;
            }

            if (Electrified)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;

                npc.lifeRegen -= 4;
                if (npc.velocity != Vector2.Zero)
                    npc.lifeRegen -= 16;

                if (damage < 4)
                    damage = 4;
            }

            if (CurseoftheMoon)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;

                npc.lifeRegen -= 24;

                if (damage < 6)
                    damage = 6;
            }

            if (OceanicMaul)
            {
                if (RegenTimer < 2)
                    RegenTimer = 2;

                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;

                npc.lifeRegen -= 48;

                if (damage < 12)
                    damage = 12;
            }

            if (Sadism)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;

                npc.lifeRegen -= 170;

                if (damage < 70)
                    damage = 70;
            }

            if (MutantNibble)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;
                if (npc.lifeRegenCount > 0)
                    npc.lifeRegenCount = 0;

                if (npc.life > LifePrevious)
                    npc.life = LifePrevious;
                else
                    LifePrevious = npc.life;
            }
            else
            {
                LifePrevious = npc.life;
            }

            if (GodEater)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;
                npc.lifeRegen -= 4200;
                if (damage < 777)
                {
                    damage = 777;
                }
            }

            if (Suffocation)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;
                npc.lifeRegen -= 40;
            }
        }

        private int InfestedExtraDot(NPC npc)
        {
            int buffIndex = npc.FindBuffIndex(mod.BuffType("Infested"));
            if (buffIndex == -1)
                return 0;

            int timeLeft = npc.buffTime[buffIndex];
            if (MaxInfestTime <= 0)
                MaxInfestTime = timeLeft;
            float baseVal = (MaxInfestTime - timeLeft) / 30f; //change the denominator to adjust max power of DOT
            int dmg = (int)(baseVal * baseVal + 8);

            InfestedDust = baseVal / 15 + .5f;
            if (InfestedDust > 5f)
                InfestedDust = 5f;

            return dmg;
        }

        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();

            if (FargoSoulsWorld.MasochistMode)
            {
                spawnRate = (int)(spawnRate * 0.8);
                maxSpawns = (int)(maxSpawns * 1.5f);

                if (AnyBossAlive())
                {
                    maxSpawns = 0;
                }
            }

            if (modPlayer.Bloodthirsty)
            {
                //20x spawn rate
                spawnRate = (int)(spawnRate * 0.05);
                //20x max spawn
                maxSpawns = (int)(maxSpawns * 20f);
            }

            if (modPlayer.SinisterIcon)
            {
                spawnRate /= 2;
                maxSpawns *= 2;
            }

            if (modPlayer.BuilderMode)
            {
                maxSpawns = 0;
            }
        }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            //layers
            int y = spawnInfo.spawnTileY;
            bool cavern = y >= Main.maxTilesY * 0.4f && y <= Main.maxTilesY * 0.8f;
            bool underground = y > Main.worldSurface && y <= Main.maxTilesY * 0.4f;
            bool surface = y < Main.worldSurface && !spawnInfo.sky;
            bool wideUnderground = cavern || underground;
            bool underworld = spawnInfo.player.ZoneUnderworldHeight;
            bool sky = spawnInfo.sky;

            //times
            bool night = !Main.dayTime;
            bool day = Main.dayTime;

            //biomes
            bool noBiome = Fargowiltas.NoBiomeNormalSpawn(spawnInfo);
            bool ocean = spawnInfo.player.ZoneBeach;
            bool dungeon = spawnInfo.player.ZoneDungeon;
            bool meteor = spawnInfo.player.ZoneMeteor;
            bool spiderCave = spawnInfo.spiderCave;
            bool mushroom = spawnInfo.player.ZoneGlowshroom;
            bool jungle = spawnInfo.player.ZoneJungle;
            bool granite = spawnInfo.granite;
            bool marble = spawnInfo.marble;
            bool corruption = spawnInfo.player.ZoneCorrupt;
            bool crimson = spawnInfo.player.ZoneCrimson;
            bool snow = spawnInfo.player.ZoneSnow;
            bool hallow = spawnInfo.player.ZoneHoly;
            bool desert = spawnInfo.player.ZoneDesert;

            bool nebulaTower = spawnInfo.player.ZoneTowerNebula;
            bool vortexTower = spawnInfo.player.ZoneTowerVortex;
            bool stardustTower = spawnInfo.player.ZoneTowerStardust;
            bool solarTower = spawnInfo.player.ZoneTowerSolar;

            bool water = spawnInfo.water;

            //events
            bool goblinArmy = Main.invasionType == 1;
            bool frostLegion = Main.invasionType == 2;
            bool pirates = Main.invasionType == 3;
            bool oldOnesArmy = DD2Event.Ongoing && spawnInfo.player.ZoneOldOneArmy;
            bool frostMoon = surface && night && Main.snowMoon;
            bool pumpkinMoon = surface && night && Main.pumpkinMoon;
            bool solarEclipse = surface && day && Main.eclipse;
            bool martianMadness = Main.invasionType == 4;
            bool lunarEvents = NPC.LunarApocalypseIsUp && (nebulaTower || vortexTower || stardustTower || solarTower);
            bool monsterMadhouse = MMWorld.MMArmy;

            //no work?
            //is lava on screen
            bool nearLava = Collision.LavaCollision(spawnInfo.player.position, spawnInfo.spawnTileX, spawnInfo.spawnTileY);
            bool noInvasion = Fargowiltas.NoInvasion(spawnInfo);
            bool normalSpawn = !spawnInfo.playerInTown && noInvasion && !oldOnesArmy;

            bool sinisterIcon = spawnInfo.player.GetModPlayer<FargoPlayer>().SinisterIcon;

            //MASOCHIST MODE
            if (FargoSoulsWorld.MasochistMode)
            {
                //all the pre hardmode
                if (!Main.hardMode)
                {
                    //mutually exclusive world layers
                    if (surface)
                    {
                        if (night && normalSpawn)
                        {
                            if (noBiome)
                            {
                                pool[NPCID.CorruptBunny] = NPC.downedBoss1 ? .05f : .025f;
                                pool[NPCID.CrimsonBunny] = NPC.downedBoss1 ? .05f : .025f;
                            }

                            if (snow)
                            {
                                pool[NPCID.CorruptPenguin] = NPC.downedBoss1 ? .1f : .05f;
                                pool[NPCID.CrimsonPenguin] = NPC.downedBoss1 ? .1f : .05f;
                            }

                            if (ocean || Main.raining)
                            {
                                pool[NPCID.CorruptGoldfish] = NPC.downedBoss1 ? .1f : .05f;
                                pool[NPCID.CrimsonGoldfish] = NPC.downedBoss1 ? .1f : .05f;
                            }

                            if (NPC.downedBoss1)
                            {
                                if (jungle)
                                    pool[NPCID.DoctorBones] = .05f;

                                if (NPC.downedBoss3 && !sinisterIcon && !AnyBossAlive())
                                    pool[NPCID.EyeofCthulhu] = Main.bloodMoon ? .004f : .002f;
                            }
                        }

                        if (Main.slimeRain && NPC.downedBoss2 && !sinisterIcon && !AnyBossAlive())
                            pool[NPCID.KingSlime] = 0.04f;
                    }
                    else if (wideUnderground)
                    {
                        if (nearLava)
                        {
                            pool[NPCID.FireImp] = .05f;
                            pool[NPCID.LavaSlime] = .05f;
                        }

                        if (marble && NPC.downedBoss2)
                        {
                            pool[NPCID.Medusa] = .05f;
                        }

                        if (granite)
                        {
                            pool[NPCID.GraniteFlyer] = .1f;
                            pool[NPCID.GraniteGolem] = .1f;
                        }

                        if (cavern)
                        {
                            if (noBiome && NPC.downedBoss3)
                                pool[NPCID.DarkCaster] = .025f;
                        }
                    }
                    else if (underworld)
                    {
                        pool[NPCID.LeechHead] = .05f;
                        pool[NPCID.BlazingWheel] = .1f;
                        if (!BossIsAlive(ref wallBoss, NPCID.WallofFlesh))
                            pool[NPCID.RedDevil] = .025f;
                    }
                    else if (sky)
                    {
                        if (normalSpawn)
                        {
                            pool[NPCID.AngryNimbus] = .02f;
                            pool[NPCID.WyvernHead] = .005f;
                        }
                    }

                    //height-independent biomes
                    if (corruption)
                    {
                        if (NPC.downedBoss2)
                        {
                            pool[NPCID.SeekerHead] = .01f;
                            if (normalSpawn && NPC.downedBoss3 && !underworld && !sinisterIcon && !AnyBossAlive())
                                pool[NPCID.EaterofWorldsHead] = .002f;
                        }
                    }

                    if (crimson)
                    {
                        if (NPC.downedBoss2)
                        {
                            pool[NPCID.IchorSticker] = .01f;
                            if (normalSpawn && NPC.downedBoss3 && !underworld && !sinisterIcon && !AnyBossAlive())
                                pool[NPCID.BrainofCthulhu] = .002f;
                        }
                    }

                    if (mushroom)
                    {
                        pool[NPCID.FungiBulb] = .1f;
                        pool[NPCID.MushiLadybug] = .1f;
                        pool[NPCID.ZombieMushroom] = .1f;
                        pool[NPCID.ZombieMushroomHat] = .1f;
                        pool[NPCID.AnomuraFungus] = .1f;
                    }

                    if (ocean)
                    {
                        pool[NPCID.PigronCorruption] = .005f;
                        pool[NPCID.PigronCrimson] = .005f;
                        pool[NPCID.PigronHallow] = .005f;
                    }

                    if (!surface && normalSpawn)
                    {
                        pool[NPCID.Mimic] = .01f;
                        if (desert && NPC.downedBoss2)
                            pool[NPCID.DuneSplicerHead] = .005f;
                    }
                }
                else //all the hardmode
                {
                    //mutually exclusive world layers
                    if (surface && !lunarEvents)
                    {
                        if (day)
                        {
                            if (normalSpawn)
                            {
                                if (noBiome && !sinisterIcon && !AnyBossAlive())
                                    pool[NPCID.KingSlime] = Main.slimeRain ? .004f : .002f;

                                if (NPC.downedMechBossAny && (noBiome || dungeon))
                                    pool[NPCID.CultistArcherWhite] = .025f;
                            }
                        }
                        else //night
                        {
                            if (Main.bloodMoon)
                            {
                                pool[NPCID.ChatteringTeethBomb] = .1f;
                                if (!sinisterIcon && !AnyBossAlive())
                                    pool[NPCID.EyeofCthulhu] = .04f;

                                if (NPC.downedPlantBoss)
                                {
                                    if (!sinisterIcon && !AnyBossAlive())
                                    {
                                        pool[NPCID.Retinazer] = .02f;
                                        pool[NPCID.Spazmatism] = .02f;
                                        pool[NPCID.TheDestroyer] = .02f;
                                        pool[NPCID.SkeletronPrime] = .02f;
                                    }
                                }
                            }
                            else //not blood moon
                            {
                                if (noInvasion && !oldOnesArmy && !sinisterIcon)
                                    pool[NPCID.Clown] = 0.01f;

                                if (normalSpawn)
                                {
                                    if (NPC.downedBoss1)
                                    {
                                        if (noBiome)
                                        {
                                            pool[NPCID.CorruptBunny] = .05f;
                                            pool[NPCID.CrimsonBunny] = .05f;
                                        }

                                        if (snow)
                                        {
                                            pool[NPCID.CorruptPenguin] = .05f;
                                            pool[NPCID.CrimsonPenguin] = .05f;
                                        }

                                        if (ocean || Main.raining)
                                        {
                                            pool[NPCID.CorruptGoldfish] = .05f;
                                            pool[NPCID.CrimsonGoldfish] = .05f;
                                        }
                                    }

                                    if (!sinisterIcon && !AnyBossAlive())
                                        pool[NPCID.EyeofCthulhu] = .01f;

                                    if (NPC.downedMechBossAny)
                                        pool[NPCID.Probe] = 0.1f;

                                    if (NPC.downedPlantBoss) //GODLUL
                                    {
                                        if (!sinisterIcon && !AnyBossAlive())
                                        {
                                            pool[NPCID.Retinazer] = .001f;
                                            pool[NPCID.Spazmatism] = .001f;
                                            pool[NPCID.TheDestroyer] = .001f;
                                            pool[NPCID.SkeletronPrime] = .001f;
                                        }

                                        if (!spawnInfo.player.GetModPlayer<FargoPlayer>().SkullCharm)
                                        {
                                            pool[NPCID.SkeletonSniper] = .02f;
                                            pool[NPCID.SkeletonCommando] = .02f;
                                            pool[NPCID.TacticalSkeleton] = .02f;
                                        }
                                    }
                                }
                            }

                            if (NPC.downedMechBossAny && noInvasion)
                            {
                                #region night pumpkin moon, frost moon
                                if (noBiome)
                                {
                                    pool[NPCID.Scarecrow1] = .01f;
                                    pool[NPCID.Scarecrow2] = .01f;
                                    pool[NPCID.Scarecrow3] = .01f;
                                    pool[NPCID.Scarecrow4] = .01f;
                                    pool[NPCID.Scarecrow5] = .01f;
                                    pool[NPCID.Scarecrow6] = .01f;
                                    pool[NPCID.Scarecrow7] = .01f;
                                    pool[NPCID.Scarecrow8] = .01f;
                                    pool[NPCID.Scarecrow9] = .01f;
                                    pool[NPCID.Scarecrow10] = .01f;

                                    if (NPC.downedHalloweenKing && !sinisterIcon)
                                    {
                                        //pool[NPCID.HeadlessHorseman] = .01f;
                                        pool[NPCID.Pumpking] = .0025f;
                                    }
                                }
                                else //in some biome
                                {
                                    if (hallow)
                                    {
                                        pool[NPCID.PresentMimic] = .05f;
                                    }
                                    else if (crimson || corruption)
                                    {
                                        pool[NPCID.Splinterling] = .05f;

                                        if (NPC.downedHalloweenTree && !sinisterIcon)
                                        {
                                            pool[NPCID.MourningWood] = .0025f;
                                        }
                                    }

                                    if (snow)
                                    {
                                        pool[NPCID.ZombieElf] = .02f;
                                        pool[NPCID.ZombieElfBeard] = .02f;
                                        pool[NPCID.ZombieElfGirl] = .02f;
                                        pool[NPCID.Yeti] = .01f;

                                        pool[NPCID.ElfArcher] = .05f;
                                        pool[NPCID.ElfCopter] = .01f;

                                        if (NPC.downedChristmasTree && !sinisterIcon)
                                        {
                                            pool[NPCID.Everscream] = .0025f;
                                        }

                                        if (NPC.downedChristmasSantank && !sinisterIcon)
                                        {
                                            pool[NPCID.SantaNK1] = .0025f;
                                        }
                                    }
                                }
                                #endregion
                            }
                        }

                        if (hallow)
                        {
                            if (!Main.raining)
                                pool[NPCID.RainbowSlime] = .001f;
                            pool[NPCID.GingerbreadMan] = .05f;
                        }

                        if (snow && noInvasion)
                        {
                            if (!Main.raining)
                                pool[NPCID.IceGolem] = .01f;
                            /*pool[NPCID.SnowBalla] = .04f;
                            pool[NPCID.MisterStabby] = .04f;
                            pool[NPCID.SnowmanGangsta] = .04f;*/
                        }

                        if (ocean)
                        {
                            pool[NPCID.PigronCorruption] = .01f;
                            pool[NPCID.PigronCrimson] = .01f;
                            pool[NPCID.PigronHallow] = .01f;
                            if (NPC.downedFishron && !sinisterIcon && !AnyBossAlive())
                                pool[NPCID.DukeFishron] = .002f;
                        }
                        else if (desert)
                        {
                            pool[NPCID.DesertBeast] = .05f;
                        }

                        if (NPC.downedMechBossAny && Main.raining)
                        {
                            pool[NPCID.LightningBug] = .1f;
                        }

                        if (crimson)
                        {
                            pool[NPCID.IchorSticker] = .1f;
                        }
                    }
                    else if (wideUnderground)
                    {
                        if (nearLava)
                        {
                            pool[NPCID.FireImp] = .02f;
                            pool[NPCID.LavaSlime] = .02f;
                        }

                        if (cavern)
                        {
                            if (noBiome && NPC.downedBoss3)
                                pool[NPCID.DarkCaster] = .05f;
                        }

                        if (dungeon && night && normalSpawn && !sinisterIcon && !AnyBossAlive())
                            pool[NPCID.SkeletronHead] = .005f;

                        if (NPC.downedMechBossAny)
                        {
                            if (snow && !Main.dayTime) //frost moon underground
                            {
                                if (underground)
                                    pool[NPCID.Nutcracker] = .05f;

                                if (cavern)
                                {
                                    pool[NPCID.Krampus] = .025f;
                                    if (NPC.downedChristmasIceQueen && !sinisterIcon)
                                        pool[NPCID.IceQueen] = .0025f;
                                }
                            }
                        }

                        if (NPC.downedPlantBoss && !spawnInfo.player.GetModPlayer<FargoPlayer>().SkullCharm)
                        {
                            pool[NPCID.DiabolistRed] = .001f;
                            pool[NPCID.DiabolistWhite] = .001f;
                            pool[NPCID.Necromancer] = .001f;
                            pool[NPCID.NecromancerArmored] = .001f;
                            pool[NPCID.RaggedCaster] = .001f;
                            pool[NPCID.RaggedCasterOpenCoat] = .001f;
                        }

                        if (NPC.downedAncientCultist && dungeon && !sinisterIcon && !AnyBossAlive())
                            pool[NPCID.CultistBoss] = 0.002f;

                        if (spawnInfo.player.ZoneUndergroundDesert)
                        {
                            if (!hallow && !corruption && !crimson)
                            {
                                pool[NPCID.SandShark] = .2f;
                            }
                            else
                            {
                                if (hallow)
                                    pool[NPCID.SandsharkHallow] = .2f;
                                if (corruption)
                                    pool[NPCID.SandsharkCorrupt] = .2f;
                                if (crimson)
                                    pool[NPCID.SandsharkCrimson] = .2f;
                            }
                        }
                    }
                    else if (underworld)
                    {
                        pool[NPCID.LeechHead] = .025f;
                        pool[NPCID.BlazingWheel] = .05f;

                        if (!sinisterIcon && !BossIsAlive(ref wallBoss, NPCID.WallofFlesh))
                            pool[NPCID.TheHungryII] = .03f;

                        if (NPC.downedPlantBoss && !spawnInfo.player.GetModPlayer<FargoPlayer>().SkullCharm)
                        {
                            pool[NPCID.DiabolistRed] = .001f;
                            pool[NPCID.DiabolistWhite] = .001f;
                            pool[NPCID.Necromancer] = .001f;
                            pool[NPCID.NecromancerArmored] = .001f;
                            pool[NPCID.RaggedCaster] = .001f;
                            pool[NPCID.RaggedCasterOpenCoat] = .001f;
                        }

                        if (FargoSoulsWorld.downedBetsy && !sinisterIcon && !AnyBossAlive())
                            pool[NPCID.DD2Betsy] = .002f;
                    }
                    else if (sky)
                    {
                        if (normalSpawn)
                        {
                            pool[NPCID.AngryNimbus] = .05f;
                            
                            if (!AnyBossAlive())
                            {
                                if (NPC.downedGolemBoss)
                                {
                                    pool[NPCID.SolarCrawltipedeHead] = .03f;
                                    pool[NPCID.VortexHornetQueen] = .03f;
                                    pool[NPCID.NebulaBrain] = .03f;
                                    pool[NPCID.StardustJellyfishBig] = .03f;
                                    pool[NPCID.AncientCultistSquidhead] = .03f;
                                    pool[NPCID.CultistDragonHead] = .03f;
                                }
                                else if (NPC.downedMechBossAny)
                                {
                                    pool[NPCID.SolarCrawltipedeHead] = .01f;
                                    pool[NPCID.VortexHornetQueen] = .01f;
                                    pool[NPCID.NebulaBrain] = .01f;
                                    pool[NPCID.StardustJellyfishBig] = .01f;
                                }

                                if (NPC.downedMoonlord && !sinisterIcon)
                                {
                                    pool[NPCID.MoonLordCore] = 0.002f;
                                }
                            }
                        }
                    }

                    //height-independent biomes
                    if (corruption)
                    {
                        if (normalSpawn && !sinisterIcon && !AnyBossAlive())
                            pool[NPCID.EaterofWorldsHead] = .002f;
                    }

                    if (crimson)
                    {
                        if (normalSpawn && !sinisterIcon && !AnyBossAlive())
                            pool[NPCID.BrainofCthulhu] = .002f;
                    }

                    if (jungle)
                    {
                        if (normalSpawn && !sinisterIcon && !AnyBossAlive())
                            pool[NPCID.QueenBee] = .001f;

                        if (!surface)
                        {
                            pool[NPCID.BigMimicJungle] = .0025f;

                            if (NPC.downedGolemBoss && !sinisterIcon && !AnyBossAlive())
                                pool[NPCID.Plantera] = .0005f;
                        }
                    }

                    if (spawnInfo.lihzahrd)
                    {
                        pool[NPCID.BlazingWheel] = .1f;
                        pool[NPCID.SpikeBall] = .1f;
                    }

                    if (ocean && spawnInfo.water)
                    {
                        pool[NPCID.AnglerFish] = .1f;
                    }
                }

                //maybe make moon lord core handle these spawns...?
                /*if (BossIsAlive(ref moonBoss, NPCID.MoonLordCore))
                {
                    pool[NPCID.SolarCrawltipedeHead] = 1f;
                    pool[NPCID.StardustJellyfishBig] = 3f;
                    pool[NPCID.VortexRifleman] = 3f;
                    pool[NPCID.NebulaBrain] = 2f;

                    pool[NPCID.AncientCultistSquidhead] = 3f;
                    pool[NPCID.CultistDragonHead] = .5f;
                }*/
            }

            if (monsterMadhouse)
            {
                pool.Clear();
                if (MMWorld.MMPoints >= 0) //Goblin Army
                {

                }
                if (MMWorld.MMPoints >= 90 && MMWorld.MMPoints < 270) //OOA 1
                {

                }
                if (MMWorld.MMPoints >= 180) //Pirates
                {

                }
                if (MMWorld.MMPoints >= 270 && MMWorld.MMPoints < 630) //OOA2
                {

                }
                if (MMWorld.MMPoints >= 360) //Eclipse
                {

                }
                if (MMWorld.MMPoints >= 450) //Pumpkin Moon
                {

                }
                if (MMWorld.MMPoints >= 540) //Frost Moon
                {

                }
                if (MMWorld.MMPoints >= 540) //Martians
                {

                }
                if (MMWorld.MMPoints >= 630) //OOA3
                {

                }
                if (MMWorld.MMPoints >= 720) //Lunar Events
                {

                }
            }
        }

        private bool firstLoot = true;

        public override void NPCLoot(NPC npc)
        {
            Player player = Main.player[Main.myPlayer];
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();

            if (modPlayer.PlatinumEnchant && !npc.boss && firstLoot)
            {
                bool midas = npc.HasBuff(BuffID.Midas);
                int chance = 10;
                int bonus = 4;

                if (midas)
                {
                    chance /= 2;
                    bonus *= 2;
                }

                if (Main.rand.Next(chance) == 0)
                {
                    firstLoot = false;
                    for (int i = 1; i < bonus; i++)
                    {
                        npc.NPCLoot();
                    }

                    int num1 = 36;
                    for (int index1 = 0; index1 < num1; ++index1)
                    {
                        Vector2 vector2_1 = (Vector2.Normalize(npc.velocity) * new Vector2((float)npc.width / 2f, (float)npc.height) * 0.75f).RotatedBy((double)(index1 - (num1 / 2 - 1)) * 6.28318548202515 / (double)num1, new Vector2()) + npc.Center;
                        Vector2 vector2_2 = vector2_1 - npc.Center;
                        int index2 = Dust.NewDust(vector2_1 + vector2_2, 0, 0, DustID.PlatinumCoin, vector2_2.X * 2f, vector2_2.Y * 2f, 100, new Color(), 1.4f);
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].noLight = true;
                        Main.dust[index2].velocity = vector2_2;
                    }
                }
            }

            firstLoot = false;

            if (npc.type == NPCID.Golem && Main.rand.Next(10) == 0)
            {
                Item.NewItem(npc.Hitbox, mod.ItemType("ComputationOrb"));
            }

            if (FargoSoulsWorld.MasochistMode && !npc.SpawnedFromStatue)
            {
                switch (npc.type)
                {
                    case NPCID.BrainScrambler:
                        if (Main.rand.Next(100) == 0)
                            Item.NewItem(npc.Hitbox, ItemID.BrainScrambler);
                        break;

                    case NPCID.JungleBat:
                    case NPCID.IceBat:
                    case NPCID.Vampire:
                    case NPCID.VampireBat:
                    case NPCID.GiantFlyingFox:
                    case NPCID.Hellbat:
                    case NPCID.Lavabat:
                        if (Main.rand.Next(10) == 0)
                            Item.NewItem(npc.Hitbox, mod.ItemType("RabiesShot"));
                        break;

                    case NPCID.IlluminantBat:
                        if (Main.rand.Next(10) == 0)
                            Item.NewItem(npc.Hitbox, mod.ItemType("RabiesShot"));
                        goto case NPCID.IlluminantSlime;

                    case NPCID.IlluminantSlime:
                    case NPCID.EnchantedSword:
                        if (Main.rand.Next(3) == 0)
                            Item.NewItem(npc.Hitbox, mod.ItemType("VolatileEnergy"), 1);
                        break;

                    case NPCID.ChaosElemental:
                        Item.NewItem(npc.Hitbox, mod.ItemType("VolatileEnergy"), 3);

                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.TeleportationPotion, Main.rand.Next(0, 2) + 1);
                        break;

                    case NPCID.CorruptBunny:
                    case NPCID.CrimsonBunny:
                        if (Main.rand.Next(50) == 0)
                            Item.NewItem(npc.Hitbox, mod.ItemType("SqueakyToy"));
                        goto case NPCID.Bunny;

                    case NPCID.CorruptGoldfish:
                    case NPCID.CrimsonGoldfish:
                    case NPCID.CorruptPenguin:
                    case NPCID.CrimsonPenguin:
                        if (Main.rand.Next(50) == 0)
                            Item.NewItem(npc.Hitbox, mod.ItemType("SqueakyToy"));
                        break;

                    case NPCID.DesertBeast:
                    case NPCID.DesertScorpionWalk:
                    case NPCID.DesertScorpionWall:
                    case NPCID.DesertLamiaDark:
                    case NPCID.DesertLamiaLight:
                    case NPCID.DesertGhoul:
                    case NPCID.DesertGhoulCorruption:
                    case NPCID.DesertGhoulCrimson:
                    case NPCID.DesertGhoulHallow:
                        if (Main.rand.Next(3) == 0)
                            Item.NewItem(npc.Hitbox, ItemID.DesertFossil, Main.rand.Next(6) + 1);
                        break;

                    case NPCID.Crab:
                    case NPCID.Squid:
                    case NPCID.SeaSnail:
                        if (Main.rand.Next(10) == 0)
                            Item.NewItem(npc.position, npc.width, npc.height,
                                Main.rand.Next(2) == 0 ? ItemID.Starfish : ItemID.Seashell, Main.rand.Next(3) + 1);
                        break;

                    case NPCID.BlueArmoredBones:
                    case NPCID.BlueArmoredBonesMace:
                    case NPCID.BlueArmoredBonesNoPants:
                    case NPCID.BlueArmoredBonesSword:
                    case NPCID.HellArmoredBones:
                    case NPCID.HellArmoredBonesMace:
                    case NPCID.HellArmoredBonesSpikeShield:
                    case NPCID.HellArmoredBonesSword:
                    case NPCID.RustyArmoredBonesAxe:
                    case NPCID.RustyArmoredBonesFlail:
                    case NPCID.RustyArmoredBonesSword:
                    case NPCID.RustyArmoredBonesSwordNoArmor:
                        Item.NewItem(npc.Hitbox, ItemID.Bone);
                        break;

                    case NPCID.SkeletonSniper:
                    case NPCID.TacticalSkeleton:
                    case NPCID.SkeletonCommando:
                    case NPCID.DiabolistRed:
                    case NPCID.DiabolistWhite:
                    case NPCID.Necromancer:
                    case NPCID.NecromancerArmored:
                    case NPCID.RaggedCaster:
                    case NPCID.RaggedCasterOpenCoat:
                        Item.NewItem(npc.Hitbox, ItemID.Bone);
                        if (Main.rand.Next(100) == 0)
                            Item.NewItem(npc.Hitbox, mod.ItemType("SkullCharm"));
                        break;

                    case NPCID.BigMimicJungle:
                        if (Main.rand.Next(10) == 0)
                            Item.NewItem(npc.Hitbox, mod.ItemType("TribalCharm"));
                        goto case NPCID.BigMimicCrimson;

                    case NPCID.IceGolem:
                        if (Main.rand.Next(10) == 0)
                            Item.NewItem(npc.Hitbox, mod.ItemType("FrigidGemstone"));
                        if (Main.rand.Next(20) == 0)
                            Item.NewItem(npc.Hitbox, ItemID.BlizzardinaBottle);
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.TitanPotion, Main.rand.Next(2, 5) + 1);
                        break;

                    case NPCID.WyvernHead:
                        Item.NewItem(npc.Hitbox, ItemID.FloatingIslandFishingCrate);
                        if (Main.rand.Next(10) == 0)
                            Item.NewItem(npc.Hitbox, mod.ItemType("DragonFang"));
                        if (Main.rand.Next(20) == 0)
                            Item.NewItem(npc.Hitbox, ItemID.CloudinaBottle);
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.GravitationPotion, Main.rand.Next(2, 5) + 1);
                        break;

                    case NPCID.RainbowSlime:
                        if (masoBool[0] && Main.rand.Next(20) == 0)
                            Item.NewItem(npc.Hitbox, mod.ItemType("ConcentratedRainbowMatter"));
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.RegenerationPotion, Main.rand.Next(2, 5) + 1);
                        break;

                    case NPCID.SandElemental:
                        if (Main.rand.Next(10) == 0)
                            Item.NewItem(npc.Hitbox, mod.ItemType("SandsofTime"));
                        if (Main.rand.Next(20) == 0)
                            Item.NewItem(npc.Hitbox, ItemID.SandstorminaBottle);
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.WrathPotion, Main.rand.Next(2, 5) + 1);
                        break;
                        
                    case NPCID.GoblinPeon:
                    case NPCID.GoblinSorcerer:
                    case NPCID.GoblinThief:
                    case NPCID.GoblinWarrior:
                        if (NPC.downedGoblins && FargoSoulsWorld.forceMeteor)
                        {
                            FargoSoulsWorld.forceMeteor = false;
                            WorldGen.dropMeteor();
                            if (Fargowiltas.Instance.FargowiltasLoaded && !NPC.AnyNPCs(ModLoader.GetMod("Fargowiltas").NPCType("Abominationn")))
                            {
                                int p = Player.FindClosest(npc.Center, 0, 0);
                                if (p != -1)
                                    NPC.SpawnOnPlayer(p, ModLoader.GetMod("Fargowiltas").NPCType("Abominationn"));
                            }
                        }
                        break;

                    case NPCID.GoblinSummoner:
                        if (Main.rand.Next(10) == 0)
                            Item.NewItem(npc.Hitbox, mod.ItemType("WretchedPouch"));
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.SummoningPotion, Main.rand.Next(2, 5) + 1);
                        goto case NPCID.GoblinPeon;

                    case NPCID.PirateCaptain:
                        if (Main.rand.Next(15) == 0)
                            Item.NewItem(npc.Hitbox, mod.ItemType("GoldenDippingVat"));
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.AmmoReservationPotion, Main.rand.Next(2, 5) + 1);
                        break;

                    case NPCID.PirateShip:
                        if (Main.rand.Next(10) == 0)
                            Item.NewItem(npc.Hitbox, mod.ItemType("SecurityWallet"));
                        if (Main.rand.Next(100) == 0)
                            Item.NewItem(npc.Hitbox, ItemID.CoinGun);
                        if (Main.rand.Next(100) == 0)
                            Item.NewItem(npc.Hitbox, ItemID.LuckyCoin);
                        break;

                    case NPCID.Nymph:
                        if (Main.rand.Next(10) == 0)
                            Item.NewItem(npc.Hitbox, mod.ItemType("NymphsPerfume"));
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.LovePotion, Main.rand.Next(2, 5) + 1);
                        break;

                    case NPCID.MourningWood:
                        Item.NewItem(npc.Hitbox, ItemID.GoodieBag, Main.rand.Next(5) + 1);
                        if (Main.rand.Next(10) == 0)
                            Item.NewItem(npc.Hitbox, ItemID.BloodyMachete);
                        break;

                    case NPCID.Pumpking:
                        Item.NewItem(npc.Hitbox, ItemID.GoodieBag, Main.rand.Next(5) + 1);
                        if (Main.rand.Next(10) == 0)
                            Item.NewItem(npc.Hitbox, ItemID.BladedGlove);
                        if (Main.pumpkinMoon && Main.rand.Next(10) == 0)
                            Item.NewItem(npc.Hitbox, mod.ItemType("PumpkingsCape"));
                        break;

                    case NPCID.Everscream:
                    case NPCID.SantaNK1:
                        Item.NewItem(npc.Hitbox, ItemID.Present, Main.rand.Next(5) + 1);
                        break;

                    case NPCID.IceQueen:
                        Item.NewItem(npc.Hitbox, ItemID.Present, Main.rand.Next(5) + 1);
                        if (Main.snowMoon && Main.rand.Next(10) == 0)
                            Item.NewItem(npc.Hitbox, mod.ItemType("IceQueensCrown"));
                        break;

                    case NPCID.MartianSaucerCore:
                        if (Main.rand.Next(10) == 0)
                            Item.NewItem(npc.Hitbox, mod.ItemType("SaucerControlConsole"));
                        break;

                    case NPCID.LavaSlime:
                        if (Main.rand.Next(100) == 0)
                            Item.NewItem(npc.Hitbox, ItemID.LavaCharm);
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.WarmthPotion, Main.rand.Next(0, 2) + 1);
                        break;

                    case NPCID.BlackRecluse:
                    case NPCID.BlackRecluseWall:
                        if (Main.rand.Next(50) == 0)
                            Item.NewItem(npc.Hitbox, ItemID.SpiderEgg);
                        break;

                    case NPCID.DesertDjinn:
                        if (Main.rand.Next(50) == 0)
                            Item.NewItem(npc.Hitbox, ItemID.FlyingCarpet);
                        break;

                    case NPCID.DarkCaster:
                        if (Main.rand.Next(50) == 0)
                            Item.NewItem(npc.Hitbox, ItemID.WaterBolt);
                        break;

                    case NPCID.Tim:
                        if (Main.rand.Next(10) == 0)
                            Item.NewItem(npc.Hitbox, mod.ItemType("TimsConcoction"));
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.ManaRegenerationPotion, Main.rand.Next(2, 5) + 1);
                        break;

                    case NPCID.RuneWizard:
                        if (Main.rand.Next(10) == 0)
                            Item.NewItem(npc.Hitbox, mod.ItemType("MysticSkull"));
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.MagicPowerPotion, Main.rand.Next(2, 5) + 1);
                        break;

                    case NPCID.SnowBalla:
                    case NPCID.SnowmanGangsta:
                    case NPCID.MisterStabby:
                        if (Main.rand.Next(100) == 0)
                            Item.NewItem(npc.Hitbox, mod.ItemType("OrdinaryCarrot"));
                        break;

                    case NPCID.BigMossHornet:
                    case NPCID.GiantMossHornet:
                    case NPCID.LittleMossHornet:
                    case NPCID.MossHornet:
                    case NPCID.TinyMossHornet:
                        if (Main.rand.Next(2) == 0)
                            Item.NewItem(npc.Hitbox, ItemID.Stinger);
                        goto case NPCID.Hornet;

                    case NPCID.Hornet:
                    case NPCID.HornetFatty:
                    case NPCID.HornetHoney:
                    case NPCID.HornetLeafy:
                    case NPCID.HornetSpikey:
                    case NPCID.HornetStingy:
                        if (Main.rand.Next(10) == 0)
                            Item.NewItem(npc.Hitbox, ItemID.JungleGrassSeeds);
                        break;

                    case NPCID.FungiBulb:
                    case NPCID.GiantFungiBulb:
                    case NPCID.AnomuraFungus:
                    case NPCID.MushiLadybug:
                    case NPCID.ZombieMushroom:
                    case NPCID.ZombieMushroomHat:
                    case NPCID.FungoFish:
                        Item.NewItem(npc.Hitbox, ItemID.GlowingMushroom, Main.rand.Next(5) + 1);
                        if (Main.rand.Next(5) == 0)
                            Item.NewItem(npc.Hitbox, ItemID.MushroomGrassSeeds);
                        if (Main.rand.Next(20) == 0)
                            Item.NewItem(npc.Hitbox, ItemID.TruffleWorm);
                        break;

                    case NPCID.Demon:
                    case NPCID.RedDevil:
                        if (Main.rand.Next(50) == 0)
                            Item.NewItem(npc.Hitbox, ItemID.Blindfold);
                        break;

                    case NPCID.Piranha:
                        if (Main.rand.Next(50) == 0)
                            Item.NewItem(npc.Hitbox, ItemID.AdhesiveBandage);
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.FishingPotion, Main.rand.Next(0, 2) + 1);
                        break;

                    case NPCID.Derpling:
                        if (Main.rand.Next(50) == 0)
                            Item.NewItem(npc.Hitbox, ItemID.TrifoldMap);
                        break;

                    case NPCID.DD2DarkMageT1:
                    case NPCID.DD2DarkMageT3:
                        Item.NewItem(npc.Hitbox, ItemID.DefenderMedal, 5);
                        break;

                    case NPCID.DD2OgreT2:
                    case NPCID.DD2OgreT3:
                        Item.NewItem(npc.Hitbox, ItemID.DefenderMedal, 10);
                        break;

                    case NPCID.Clown:
                        Item.NewItem(npc.Hitbox, ItemID.PartyGirlGrenade, Main.rand.Next(10) + 1);
                        break;

                    #region potion drops

                    case NPCID.BlueSlime:
                        if (npc.netID == NPCID.YellowSlime && Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.RecallPotion, Main.rand.Next(0, 2) + 1);
                        break;

                    case NPCID.GoblinArcher:
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.ArcheryPotion, Main.rand.Next(0, 2) + 1);
                        goto case NPCID.GoblinPeon;

                    case NPCID.Antlion:
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.BuilderPotion, Main.rand.Next(0, 2) + 1);
                        break;

                    case NPCID.Mimic:
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.SpelunkerPotion, Main.rand.Next(2, 5) + 1);
                        break;

                    case NPCID.WallCreeperWall:
                    case NPCID.WallCreeper:
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.TrapsightPotion, Main.rand.Next(0, 2) + 1);
                        break;

                    case NPCID.Salamander:
                    case NPCID.Salamander2:
                    case NPCID.Salamander3:
                    case NPCID.Salamander4:
                    case NPCID.Salamander5:
                    case NPCID.Salamander6:
                    case NPCID.Salamander7:
                    case NPCID.Salamander8:
                    case NPCID.Salamander9:
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.InvisibilityPotion, Main.rand.Next(0, 2) + 1);
                        break;

                    case NPCID.GreekSkeleton:
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.IronskinPotion, Main.rand.Next(0, 2) + 1);
                        break;

                    case NPCID.Harpy:
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.FeatherfallPotion, Main.rand.Next(0, 2) + 1);
                        break;

                    case NPCID.Shark:
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.FlipperPotion, Main.rand.Next(0, 2) + 1);
                        break;

                    case NPCID.GraniteGolem:
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.EndurancePotion, Main.rand.Next(0, 2) + 1);
                        break;

                    case NPCID.UndeadMiner:
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.MiningPotion, Main.rand.Next(2, 5) + 1);
                        break;

                    case NPCID.Raven:
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.NightOwlPotion, Main.rand.Next(0, 2) + 1);
                        break;

                    case NPCID.BloodZombie:
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.RagePotion, Main.rand.Next(0, 2) + 1);
                        break;

                    case NPCID.PinkJellyfish:
                    case NPCID.BlueJellyfish:
                    case NPCID.GreenJellyfish:
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.ShinePotion, Main.rand.Next(0, 2) + 1);
                        break;

                    case NPCID.CaveBat:
                    case NPCID.GiantBat:
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.SonarPotion, Main.rand.Next(0, 2) + 1);
                        goto case NPCID.JungleBat;

                    case NPCID.BoneSerpentHead:
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.ObsidianSkinPotion, Main.rand.Next(0, 2) + 1);
                        break;

                    case NPCID.FireImp:
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.InfernoPotion, Main.rand.Next(0, 2) + 1);
                        break;

                    case NPCID.Drippler:
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.HeartreachPotion, Main.rand.Next(0, 2) + 1);
                        break;

                    case NPCID.GoblinScout:
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.BattlePotion, Main.rand.Next(2, 5) + 1);
                        goto case NPCID.GoblinPeon;

                    case NPCID.Bunny:
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.CalmingPotion, Main.rand.Next(0, 2) + 1);
                        break;

                    case NPCID.Arapaima:
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.CratePotion, Main.rand.Next(0, 2) + 1);
                        break;

                    case NPCID.ToxicSludge:
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.StinkPotion, Main.rand.Next(0, 2) + 1);
                        break;

                    case NPCID.Tumbleweed:
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.SwiftnessPotion, Main.rand.Next(0, 2) + 1);
                        break;

                    case NPCID.SpikedJungleSlime:
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.ThornsPotion, Main.rand.Next(0, 2) + 1);
                        break;

                    case NPCID.GiantWormHead:
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.WormholePotion, Main.rand.Next(0, 2) + 1);
                        break;

                    case NPCID.IceSlime:
                    case NPCID.SpikedIceSlime:
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.WaterWalkingPotion, Main.rand.Next(0, 2) + 1);
                        break;

                    case NPCID.Goldfish:
                    case NPCID.GoldfishWalker:
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.GillsPotion, Main.rand.Next(0, 2) + 1);
                        break;

                    case NPCID.Duck:
                    case NPCID.Duck2:
                    case NPCID.DuckWhite:
                    case NPCID.DuckWhite2:
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.HunterPotion, Main.rand.Next(0, 2) + 1);
                        break;

                    case NPCID.BigMimicCorruption:
                    case NPCID.BigMimicCrimson:
                    case NPCID.BigMimicHallow:
                        if (Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().TimsConcoction)
                            Item.NewItem(npc.Hitbox, ItemID.LifeforcePotion, Main.rand.Next(2, 5) + 1);
                        break;

                    #endregion

                    #region boss drops
                    case NPCID.KingSlime:
                        npc.DropItemInstanced(npc.position, npc.Size, ItemID.HerbBag, Main.rand.Next(5) + 1);
                        npc.DropItemInstanced(npc.position, npc.Size, ItemID.WoodenCrate, 5);
                        npc.DropItemInstanced(npc.position, npc.Size, mod.ItemType("SlimyShield"));
                        if (Main.netMode != 1 && !BossIsAlive(ref mutantBoss, mod.NPCType("MutantBoss")) && Fargowiltas.Instance.FargowiltasLoaded && !NPC.AnyNPCs(ModLoader.GetMod("Fargowiltas").NPCType("Mutant")))
                        {
                            int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModLoader.GetMod("Fargowiltas").NPCType("Mutant"));
                            if (n < 200 && Main.netMode == 2)
                                NetMessage.SendData(23, -1, -1, null, n);
                        }
                        break;

                    case NPCID.EyeofCthulhu:
                        npc.DropItemInstanced(npc.position, npc.Size, ItemID.LifeCrystal, Main.rand.Next(3) + 1);
                        npc.DropItemInstanced(npc.position, npc.Size, ItemID.WoodenCrate, 5);
                        npc.DropItemInstanced(npc.position, npc.Size, mod.ItemType("AgitatingLens"));
                        break;

                    case NPCID.EaterofWorldsHead:
                    case NPCID.EaterofWorldsBody:
                    case NPCID.EaterofWorldsTail:
                        bool dropItems = true;
                        for (int i = 0; i < 200; i++)
                        {
                            if (Main.npc[i].active && i != npc.whoAmI && (Main.npc[i].type == 13 || Main.npc[i].type == 14 || Main.npc[i].type == 15))
                            {
                                dropItems = false;
                                break;
                            }
                        }
                        if (dropItems)
                        {
                            npc.DropItemInstanced(npc.position, npc.Size, ItemID.CorruptFishingCrate, 5);
                            npc.DropItemInstanced(npc.position, npc.Size, mod.ItemType("CorruptHeart"));

                            //to make up for no loot until dead (lowest possible amount dropped ECH)
                            Item.NewItem(npc.Hitbox, ItemID.ShadowScale, 60);
                            Item.NewItem(npc.Hitbox, ItemID.DemoniteOre, 150);
                        }
                        break;

                    case NPCID.BrainofCthulhu:
                        npc.DropItemInstanced(npc.position, npc.Size, ItemID.CrimsonFishingCrate, 5);
                        npc.DropItemInstanced(npc.position, npc.Size, mod.ItemType("GuttedHeart"));

                        //to make up for no loot creepers (lowest possible amount dropped ECH)
                        Item.NewItem(npc.Hitbox, ItemID.TissueSample, 60);
                        Item.NewItem(npc.Hitbox, ItemID.CrimtaneOre, 150);
                        break;

                    case NPCID.SkeletronHead:
                        npc.DropItemInstanced(npc.position, npc.Size, ItemID.DungeonFishingCrate, 5);
                        npc.DropItemInstanced(npc.position, npc.Size, mod.ItemType("NecromanticBrew"));
                        break;

                    case NPCID.QueenBee:
                        npc.DropItemInstanced(npc.position, npc.Size, ItemID.JungleFishingCrate, 5);
                        npc.DropItemInstanced(npc.position, npc.Size, mod.ItemType("QueenStinger"));
                        break;

                    case NPCID.WallofFlesh:
                        npc.DropItemInstanced(npc.position, npc.Size, mod.ItemType("PungentEyeball"));
                        npc.DropItemInstanced(npc.position, npc.Size, ItemID.HallowedFishingCrate, 5);
                        if (Fargowiltas.Instance.FargowiltasLoaded)
                        {
                            npc.DropItemInstanced(npc.position, npc.Size, ModLoader.GetMod("Fargowiltas").ItemType("ShadowCrate"), 5);
                            if (!Main.player[Main.myPlayer].GetModPlayer<FargoPlayer>().MutantsDiscountCard)
                                npc.DropItemInstanced(npc.position, npc.Size, mod.ItemType("MutantsDiscountCard"));
                        }
                        break;

                    case NPCID.Retinazer:
                        if (!BossIsAlive(ref spazBoss, NPCID.Spazmatism))
                        {
                            npc.DropItemInstanced(npc.position, npc.Size, mod.ItemType("FusedLens"));
                            npc.DropItemInstanced(npc.position, npc.Size, ItemID.FallenStar, Main.rand.Next(10) + 1);
                            npc.DropItemInstanced(npc.position, npc.Size, ItemID.IronCrate, 5);
                        }
                        break;

                    case NPCID.Spazmatism:
                        if (!BossIsAlive(ref retiBoss, NPCID.Retinazer))
                        {
                            npc.DropItemInstanced(npc.position, npc.Size, mod.ItemType("FusedLens"));
                            npc.DropItemInstanced(npc.position, npc.Size, ItemID.FallenStar, Main.rand.Next(10) + 1);
                            npc.DropItemInstanced(npc.position, npc.Size, ItemID.IronCrate, 5);
                        }
                        break;

                    case NPCID.TheDestroyer:
                        npc.DropItemInstanced(npc.position, npc.Size, mod.ItemType("GroundStick"));
                        npc.DropItemInstanced(npc.position, npc.Size, ItemID.FallenStar, Main.rand.Next(10) + 1);
                        npc.DropItemInstanced(npc.position, npc.Size, ItemID.IronCrate, 5);
                        break;

                    case NPCID.SkeletronPrime:
                        npc.DropItemInstanced(npc.position, npc.Size, mod.ItemType("ReinforcedPlating"));
                        npc.DropItemInstanced(npc.position, npc.Size, ItemID.FallenStar, Main.rand.Next(10) + 1);
                        npc.DropItemInstanced(npc.position, npc.Size, ItemID.IronCrate, 5);
                        break;

                    case NPCID.Plantera:
                        npc.DropItemInstanced(npc.position, npc.Size, mod.ItemType("MagicalBulb"));
                        npc.DropItemInstanced(npc.position, npc.Size, ItemID.JungleFishingCrate, 5);
                        npc.DropItemInstanced(npc.position, npc.Size, ItemID.LifeFruit, Main.rand.Next(3) + 1);
                        npc.DropItemInstanced(npc.position, npc.Size, ItemID.ChlorophyteOre, Main.rand.Next(101) + 100);
                        break;

                    case NPCID.Golem:
                        npc.DropItemInstanced(npc.position, npc.Size, mod.ItemType("LihzahrdTreasureBox"));
                        npc.DropItemInstanced(npc.position, npc.Size, ItemID.GoldenCrate, 5);
                        break;

                    case NPCID.DD2Betsy:
                        npc.DropItemInstanced(npc.position, npc.Size, mod.ItemType("BetsysHeart"));
                        npc.DropItemInstanced(npc.position, npc.Size, ItemID.GoldenCrate, 5);
                        FargoSoulsWorld.downedBetsy = true;
                        break;

                    case NPCID.DukeFishron:
                        /*if (masoBool[3])
                        {
                            npc.DropItemInstanced(npc.position, npc.Size, mod.ItemType("CyclonicFin"));
                            npc.DropItemInstanced(npc.position, npc.Size, mod.ItemType("Sadism"), Main.rand.Next(10) + 1);
                        }
                        else
                        {*/
                        npc.DropItemInstanced(npc.position, npc.Size, mod.ItemType("MutantAntibodies"));
                        npc.DropItemInstanced(npc.position, npc.Size, ItemID.Bacon, Main.rand.Next(10) + 1);
                        npc.DropItemInstanced(npc.position, npc.Size, ItemID.GoldenCrate, 5);
                        if (Fargowiltas.Instance.FargowiltasLoaded && !Main.player[Main.myPlayer].GetModPlayer<FargoPlayer>().MutantsPact)
                            npc.DropItemInstanced(npc.position, npc.Size, mod.ItemType("MutantsPact"));
                        //}
                        break;

                    case NPCID.CultistBoss:
                        npc.DropItemInstanced(npc.position, npc.Size, mod.ItemType("CelestialRune"));
                        if (Main.player[Main.myPlayer].extraAccessorySlots == 1 || Main.netMode != 0)
                            npc.DropItemInstanced(npc.position, npc.Size, mod.ItemType("CelestialSeal"));
                        break;

                    case NPCID.MoonLordCore:
                        npc.DropItemInstanced(npc.position, npc.Size, mod.ItemType("GalacticGlobe"));
                        npc.DropItemInstanced(npc.position, npc.Size, mod.ItemType("LunarCrystal"), Main.rand.Next(10) + 5);
                        break;

                    case NPCID.DungeonGuardian:
                        npc.DropItemInstanced(npc.position, npc.Size, mod.ItemType("SinisterIcon"));
                        break;
                    #endregion

                    case NPCID.Painter:
                        if (FargoSoulsWorld.downedMutant && NPC.AnyNPCs(mod.NPCType("MutantBoss")))
                            Item.NewItem(npc.Hitbox, mod.ItemType("ScremPainting"));
                        break;

                    default:
                        break;
                }
            }

            //boss drops
            if (Main.rand.Next(FargoSoulsWorld.MasochistMode ? 3 : 10) == 0)
            {
                switch (npc.type)
                {
                    case NPCID.KingSlime:
                        Item.NewItem(npc.Hitbox, mod.ItemType("SlimeKingsSlasher"));
                        break;

                    case NPCID.EyeofCthulhu:
                        Item.NewItem(npc.Hitbox, mod.ItemType("EyeFlail"));
                        break;

                    case NPCID.EaterofWorldsHead:
                    case NPCID.EaterofWorldsBody:
                    case NPCID.EaterofWorldsTail:
                        bool dropItems = true;
                        for (int i = 0; i < 200; i++)
                        {
                            if (Main.npc[i].active && i != npc.whoAmI && (Main.npc[i].type == 13 || Main.npc[i].type == 14 || Main.npc[i].type == 15))
                            {
                                dropItems = false;
                                break;
                            }
                        }
                        if (dropItems)
                        {
                            Item.NewItem(npc.Hitbox, mod.ItemType("EaterStaff"));
                        }
                        break;

                    case NPCID.BrainofCthulhu:
                        Item.NewItem(npc.Hitbox, mod.ItemType("BrainStaff"));
                        break;

                    case NPCID.QueenBee:
                        Item.NewItem(npc.Hitbox, mod.ItemType("HiveStaff"));
                        break;

                    case NPCID.SkeletronHead:
                        Item.NewItem(npc.Hitbox, mod.ItemType("Bonezone"));
                        break;

                    case NPCID.WallofFlesh:
                        Item.NewItem(npc.Hitbox, mod.ItemType("FleshHand"));
                        break;

                    case NPCID.TheDestroyer:
                        Item.NewItem(npc.Hitbox, mod.ItemType("DestroyerGun"));
                        break;

                    case NPCID.SkeletronPrime:
                        Item.NewItem(npc.Hitbox, mod.ItemType("DarkStarCannon"));
                        break;

                    case NPCID.Retinazer:
                        if (!BossIsAlive(ref spazBoss, NPCID.Spazmatism))
                        {
                            Item.NewItem(npc.Hitbox, mod.ItemType("TwinRangs"));
                        }
                        break;

                    case NPCID.Spazmatism:
                        if (!BossIsAlive(ref retiBoss, NPCID.Retinazer))
                        {
                            Item.NewItem(npc.Hitbox, mod.ItemType("TwinRangs"));
                        }
                        break;

                    case NPCID.Plantera:
                        Item.NewItem(npc.Hitbox, mod.ItemType("Dicer"));
                        break;

                    case NPCID.Golem:
                        Item.NewItem(npc.Hitbox, mod.ItemType("GolemTome"));
                        break;

                    case NPCID.DukeFishron:
                        Item.NewItem(npc.Hitbox, mod.ItemType("FishStick"));
                        break;
                }
            }

            if (Fargowiltas.Instance.CalamityLoaded && Revengeance && FargoSoulsWorld.MasochistMode && Main.bloodMoon && Main.moonPhase == 0 && Main.raining && Main.rand.Next(10) == 0)
            {
                Mod calamity = ModLoader.GetMod("CalamityMod");

                if (npc.type == calamity.NPCType("DevourerofGodsHeadS"))
                {
                    Item.NewItem(npc.Hitbox, calamity.ItemType("CosmicPlushie"));
                }
            }
        }

        public override bool CheckDead(NPC npc)
        {
            Player player = Main.player[Main.myPlayer];
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();

            if (TimeFrozen)
            {
                npc.life = 1;
                return false;
            }

            if (npc.boss && BossIsAlive(ref mutantBoss, mod.NPCType("MutantBoss")) && npc.type != mod.NPCType("MutantBoss"))
            {
                npc.active = false;
                Main.PlaySound(npc.DeathSound, npc.Center);
                return false;
            }

            if (modPlayer.WoodEnchant && npc.damage == 0 && !npc.townNPC && npc.lifeMax == 5)
            {
                Projectile.NewProjectile(npc.Center, new Vector2(0, -4), ProjectileID.LostSoulFriendly, 20, 0, Main.myPlayer);
            }

            if (Needles && npc.lifeMax > 1 && Main.rand.Next(2) == 0)
            {
                int dmg = 15;
                int numNeedles = 8;

                if (modPlayer.LifeForce)
                {
                    dmg = 50;
                    numNeedles = 16;
                }

                Projectile[] projs = FargoGlobalProjectile.XWay(numNeedles, npc.Center, ProjectileID.PineNeedleFriendly, 5, modPlayer.HighestDamageTypeScaling(dmg), 5f);

                for (int i = 0; i < projs.Length; i++)
                {
                    if (projs[i] == null) continue;
                    Projectile p = projs[i];
                    p.GetGlobalProjectile<FargoGlobalProjectile>().IsRecolor = true;
                    p.magic = false;
                    p.melee = true;
                    p.GetGlobalProjectile<FargoGlobalProjectile>().CanSplit = false;
                }
            }

            if (FargoSoulsWorld.MasochistMode)
            {
                switch (npc.type)
                {
                    case NPCID.Drippler:
                        if (Main.rand.Next(3) == 0 && Main.netMode != 1)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                int n = NPC.NewNPC((int)(npc.position.X + npc.width / 2), (int)(npc.position.Y + npc.height), NPCID.DemonEye);
                                if (n != 200)
                                {
                                    Main.npc[n].velocity = new Vector2(Main.rand.Next(-3, 3), Main.rand.Next(-3, 3));
                                    Main.npc[n].netUpdate = true;
                                    if (Main.netMode == 2)
                                        NetMessage.SendData(23, -1, -1, null, n);
                                }
                            }
                        }
                        break;

                    case NPCID.GoblinPeon:
                    case NPCID.GoblinWarrior:
                    case NPCID.GoblinArcher:
                    case NPCID.GoblinScout:
                    case NPCID.GoblinSorcerer:
                    case NPCID.GoblinThief:
                        if (Main.netMode != 1)
                            Projectile.NewProjectile(npc.Center, new Vector2(Main.rand.NextFloat(-2f, 2f), -5), mod.ProjectileType("GoblinSpikyBall"), 15, 0, Main.myPlayer);
                        break;

                    case NPCID.AngryBones:
                    case NPCID.AngryBonesBig:
                    case NPCID.AngryBonesBigHelmet:
                    case NPCID.AngryBonesBigMuscle:
                        if (Main.rand.Next(5) == 0 && Main.netMode != 1)
                        {
                            bool spawnGuardian = Main.rand.Next(20) == 0 && Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().NecromanticBrew;
                            int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, spawnGuardian ? mod.NPCType("BabyGuardian") : NPCID.CursedSkull);
                            if (n < 200 && Main.netMode == 2)
                                NetMessage.SendData(23, -1, -1, null, n);
                        }
                        break;

                    //all armored bones
                    case 269:
                    case 270:
                    case 271:
                    case 272:
                    case 273:
                    case 274:
                    case 275:
                    case 276:
                    case 277:
                    case 278:
                    case 279:
                    case 280:
                        if (Main.netMode != 1 && Main.rand.Next(100) == 0 && Main.player[npc.lastInteraction].GetModPlayer<FargoPlayer>().NecromanticBrew)
                        {
                            int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("BabyGuardian"));
                            if (n < 200 && Main.netMode == 2)
                                NetMessage.SendData(23, -1, -1, null, n);
                        }
                        break;

                    case NPCID.DungeonSlime:
                        if (NPC.downedPlantBoss && Main.netMode != 1)
                        {
                            int paladin = NPC.NewNPC((int)(npc.position.X + npc.width / 2), (int)(npc.position.Y + npc.height), NPCID.Paladin);
                            if (paladin != 200)
                            {
                                Vector2 center = Main.npc[paladin].Center;
                                Main.npc[paladin].width = (int)(Main.npc[paladin].width * .65f);
                                Main.npc[paladin].height = (int)(Main.npc[paladin].height * .65f);
                                Main.npc[paladin].scale = .65f;
                                Main.npc[paladin].Center = center;
                                Main.npc[paladin].lifeMax /= 2;
                                Main.npc[paladin].life = Main.npc[paladin].lifeMax;
                                Main.npc[paladin].GetGlobalNPC<FargoSoulsGlobalNPC>().masoBool[0] = true;
                                Main.npc[paladin].velocity = new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 1));
                                if (Main.netMode == 2)
                                    NetMessage.SendData(23, -1, -1, null, paladin);
                            }
                        }
                        break;

                    case NPCID.BlueSlime:
                        switch (npc.netID)
                        {
                            case NPCID.YellowSlime:
                                if (Main.netMode != 1)
                                {
                                    for (int i = 0; i < 2; i++)
                                    {
                                        int spawn = NPC.NewNPC((int)(npc.position.X + npc.width / 2), (int)(npc.position.Y + npc.height), 1);
                                        if (spawn != 200)
                                        {
                                            Main.npc[spawn].SetDefaults(NPCID.PurpleSlime);
                                            Main.npc[spawn].velocity.X = npc.velocity.X * 2f;
                                            Main.npc[spawn].velocity.Y = npc.velocity.Y;

                                            NPC spawn2 = Main.npc[spawn];
                                            spawn2.velocity.X = spawn2.velocity.X + (Main.rand.Next(-20, 20) * 0.1f + i * npc.direction * 0.3f);
                                            NPC spawn3 = Main.npc[spawn];
                                            spawn3.velocity.Y = spawn3.velocity.Y - (Main.rand.Next(0, 10) * 0.1f + i);
                                            Main.npc[spawn].ai[0] = -1000 * Main.rand.Next(3);

                                            if (Main.netMode == 2)
                                                NetMessage.SendData(23, -1, -1, null, spawn);
                                        }
                                    }
                                }
                                break;

                            case NPCID.PurpleSlime:
                                if (Main.netMode != 1)
                                {
                                    for (int i = 0; i < 2; i++)
                                    {
                                        int spawn = NPC.NewNPC((int)(npc.position.X + npc.width / 2), (int)(npc.position.Y + npc.height), 1);

                                        if (spawn != 200)
                                        {
                                            Main.npc[spawn].SetDefaults(NPCID.RedSlime);
                                            Main.npc[spawn].velocity.X = npc.velocity.X * 2f;
                                            Main.npc[spawn].velocity.Y = npc.velocity.Y;

                                            NPC spawn2 = Main.npc[spawn];
                                            spawn2.velocity.X = spawn2.velocity.X + (Main.rand.Next(-20, 20) * 0.1f + i * npc.direction * 0.3f);
                                            NPC spawn3 = Main.npc[spawn];
                                            spawn3.velocity.Y = spawn3.velocity.Y - (Main.rand.Next(0, 10) * 0.1f + i);
                                            Main.npc[spawn].ai[0] = -1000 * Main.rand.Next(3);

                                            if (Main.netMode == 2)
                                                NetMessage.SendData(23, -1, -1, null, spawn);
                                        }
                                    }
                                }
                                break;

                            case NPCID.RedSlime:
                                if (Main.netMode != 1)
                                {
                                    for (int i = 0; i < 2; i++)
                                    {
                                        int spawn = NPC.NewNPC((int)(npc.position.X + npc.width / 2), (int)(npc.position.Y + npc.height), 1);
                                        Main.npc[spawn].SetDefaults(NPCID.GreenSlime);
                                        Main.npc[spawn].velocity.X = npc.velocity.X * 2f;
                                        Main.npc[spawn].velocity.Y = npc.velocity.Y;

                                        NPC spawn2 = Main.npc[spawn];
                                        spawn2.velocity.X = spawn2.velocity.X + (Main.rand.Next(-20, 20) * 0.1f + i * npc.direction * 0.3f);
                                        NPC spawn3 = Main.npc[spawn];
                                        spawn3.velocity.Y = spawn3.velocity.Y - (Main.rand.Next(0, 10) * 0.1f + i);
                                        Main.npc[spawn].ai[0] = -1000 * Main.rand.Next(3);

                                        if (Main.netMode == 2)
                                            NetMessage.SendData(23, -1, -1, null, spawn);
                                    }
                                }
                                break;

                            case NPCID.Pinky:
                                if (Main.netMode != 1)
                                {
                                    for (int i = 0; i < 3; i++)
                                    {
                                        int spawn = NPC.NewNPC((int)(npc.position.X + npc.width / 2), (int)(npc.position.Y + npc.height), 1);

                                        if (spawn != 200)
                                        {
                                            Main.npc[spawn].SetDefaults(i < 2 ? NPCID.YellowSlime : NPCID.MotherSlime);
                                            Main.npc[spawn].velocity.X = npc.velocity.X * 2f;
                                            Main.npc[spawn].velocity.Y = npc.velocity.Y;

                                            NPC spawn2 = Main.npc[spawn];
                                            spawn2.velocity.X = spawn2.velocity.X + (Main.rand.Next(-20, 20) * 0.1f + i * npc.direction * 0.3f);
                                            NPC spawn3 = Main.npc[spawn];
                                            spawn3.velocity.Y = spawn3.velocity.Y - (Main.rand.Next(0, 10) * 0.1f + i);
                                            Main.npc[spawn].ai[0] = -1000 * Main.rand.Next(3);

                                            if (Main.netMode == 2)
                                                NetMessage.SendData(23, -1, -1, null, spawn);
                                        }
                                    }
                                }
                                break;

                            default:
                                break;
                        }
                        break;

                    case NPCID.DrManFly:
                        if (Main.netMode != 1)
                        {
                            for (int i = 0; i < 10; i++)
                                Projectile.NewProjectile(npc.Center, new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11)),
                                    ProjectileID.DrManFlyFlask, npc.damage / 2, 1f, Main.myPlayer);
                        }
                        break;

                    case NPCID.EyeofCthulhu:
                        if (FargoSoulsWorld.EyeCount < FargoSoulsWorld.MaxCountPreHM && !FargoSoulsWorld.NoMasoBossScaling)
                            FargoSoulsWorld.EyeCount++;
                        break;

                    case NPCID.KingSlime:
                        if (FargoSoulsWorld.SlimeCount < FargoSoulsWorld.MaxCountPreHM && !FargoSoulsWorld.NoMasoBossScaling)
                            FargoSoulsWorld.SlimeCount++;
                        break;

                    case NPCID.Splinterling:
                        if (!NPC.downedPlantBoss)
                        {
                            npc.active = false;
                            Main.PlaySound(npc.DeathSound, npc.Center);
                            return false;
                        }
                        break;

                    case NPCID.EaterofWorldsHead:
                    case NPCID.EaterofWorldsBody:
                    case NPCID.EaterofWorldsTail:
                        if (BossIsAlive(ref mutantBoss, mod.NPCType("MutantBoss")))
                        {
                            npc.active = false;
                            Main.PlaySound(npc.DeathSound, npc.Center);
                            return false;
                        }

                        if (Main.netMode != 1)
                        {
                            int count = NPC.CountNPCS(NPCID.BigEater) + NPC.CountNPCS(NPCID.EaterofSouls) + NPC.CountNPCS(NPCID.LittleEater);

                            if (count < 40)
                            {
                                int type = Main.rand.Next(2) == 0 ? NPCID.EaterofSouls
                                : (Main.rand.Next(2) == 0 ? NPCID.BigEater : NPCID.LittleEater);
                                int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, type);
                                if (n < 200 && Main.netMode == 2)
                                    NetMessage.SendData(23, -1, -1, null, n);
                            }
                        }

                        if (Fargowiltas.Instance.FargowiltasLoaded && (bool)ModLoader.GetMod("Fargowiltas").Call("SwarmActive"))
                        {
                            break;
                        }
                        else
                        {
                            for (int i = 0; i < 200; i++)
                            {
                                if (Main.npc[i].active && i != npc.whoAmI && (Main.npc[i].type == 13 || Main.npc[i].type == 14 || Main.npc[i].type == 15))
                                {
                                    Main.PlaySound(npc.DeathSound, npc.Center);
                                    npc.active = false;
                                    return false;
                                }
                            }

                            if (FargoSoulsWorld.EaterCount < FargoSoulsWorld.MaxCountPreHM && !FargoSoulsWorld.NoMasoBossScaling)
                                FargoSoulsWorld.EaterCount++;
                        }
                        break;

                    case NPCID.BrainofCthulhu:
                        if (FargoSoulsWorld.BrainCount < FargoSoulsWorld.MaxCountPreHM && !FargoSoulsWorld.NoMasoBossScaling)
                            FargoSoulsWorld.BrainCount++;
                        break;

                    case NPCID.Creeper:
                        Main.PlaySound(npc.DeathSound, npc.Center);
                        npc.active = false;
                        return false;

                    case NPCID.QueenBee:
                        if (FargoSoulsWorld.BeeCount < FargoSoulsWorld.MaxCountPreHM && !FargoSoulsWorld.NoMasoBossScaling)
                            FargoSoulsWorld.BeeCount++;
                        break;

                    case NPCID.SkeletronHead:
                        if (npc.ai[1] != 2f)
                        {
                            Main.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);
                            npc.life = npc.lifeMax / 176;
                            if (npc.life < 50)
                                npc.life = 50;
                            npc.defense = 9999;
                            npc.damage = npc.defDamage * 15;
                            npc.ai[1] = 2f;
                            npc.netUpdate = true;
                            if (Main.netMode == 2)
                                NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("Skeletron has entered Dungeon Guardian form!"), new Color(175, 75, 255));
                            else if (Main.netMode == 0)
                                Main.NewText("Skeletron has entered Dungeon Guardian form!", 175, 75, 255);
                            return false;
                        }
                        else if (FargoSoulsWorld.SkeletronCount < FargoSoulsWorld.MaxCountPreHM && !FargoSoulsWorld.NoMasoBossScaling)
                        {
                            FargoSoulsWorld.SkeletronCount++;
                        }
                        break;

                    case NPCID.WallofFlesh:
                        if (FargoSoulsWorld.WallCount < FargoSoulsWorld.MaxCountPreHM && !FargoSoulsWorld.NoMasoBossScaling)
                            FargoSoulsWorld.WallCount++;
                        break;

                    case NPCID.TheDestroyer:
                        if (FargoSoulsWorld.DestroyerCount < FargoSoulsWorld.MaxCountHM && !FargoSoulsWorld.NoMasoBossScaling)
                            FargoSoulsWorld.DestroyerCount++;
                        break;

                    case NPCID.SkeletronPrime:
                        if (npc.ai[1] != 2f)
                        {
                            Main.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);
                            npc.life = npc.lifeMax / 420;
                            if (npc.life < 100)
                                npc.life = 100;
                            npc.defDefense = 9999;
                            npc.defense = 9999;
                            npc.defDamage *= 13;
                            npc.damage *= 13;
                            npc.ai[1] = 2f;
                            npc.netUpdate = true;
                            if (Main.netMode == 2)
                                NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("Skeletron Prime has entered Dungeon Guardian form!"), new Color(175, 75, 255));
                            else if (Main.netMode == 0)
                                Main.NewText("Skeletron Prime has entered Dungeon Guardian form!", 175, 75, 255);
                            return false;
                        }
                        else if (FargoSoulsWorld.PrimeCount < FargoSoulsWorld.MaxCountHM && !FargoSoulsWorld.NoMasoBossScaling)
                        {
                            FargoSoulsWorld.PrimeCount++;
                        }
                        break;

                    case NPCID.Retinazer:
                        if (BossIsAlive(ref spazBoss, NPCID.Spazmatism) && Main.npc[spazBoss].life > 1) //spaz still active
                        {
                            npc.life = 1;
                            npc.active = true;
                            if (Main.netMode != 1)
                            {
                                npc.netUpdate = true;
                                npc.dontTakeDamage = true;
                                masoBool[3] = true;
                                NetUpdateMaso(npc.whoAmI);
                            }
                            return false;
                        }

                        if (FargoSoulsWorld.TwinsCount < FargoSoulsWorld.MaxCountHM && !FargoSoulsWorld.NoMasoBossScaling && !NPC.AnyNPCs(NPCID.Spazmatism))
                            FargoSoulsWorld.TwinsCount++;
                        break;

                    case NPCID.Spazmatism:
                        if (BossIsAlive(ref retiBoss, NPCID.Retinazer) && Main.npc[retiBoss].life > 1) //reti still active
                        {
                            npc.life = 1;
                            npc.active = true;
                            if (Main.netMode != 1)
                            {
                                npc.netUpdate = true;
                                npc.dontTakeDamage = true;
                                masoBool[3] = true;
                                NetUpdateMaso(npc.whoAmI);
                            }
                            return false;
                        }

                        if (FargoSoulsWorld.TwinsCount < FargoSoulsWorld.MaxCountHM && !FargoSoulsWorld.NoMasoBossScaling && !NPC.AnyNPCs(NPCID.Retinazer))
                            FargoSoulsWorld.TwinsCount++;
                        break;

                    case NPCID.Plantera:
                        if (FargoSoulsWorld.PlanteraCount < FargoSoulsWorld.MaxCountHM && !FargoSoulsWorld.NoMasoBossScaling)
                            FargoSoulsWorld.PlanteraCount++;
                        break;

                    case NPCID.Golem:
                        if (FargoSoulsWorld.GolemCount < FargoSoulsWorld.MaxCountHM && !FargoSoulsWorld.NoMasoBossScaling)
                            FargoSoulsWorld.GolemCount++;
                        break;

                    case NPCID.DukeFishron:
                        if (npc.ai[0] <= 9)
                        {
                            npc.life = 1;
                            npc.active = true;
                            if (Main.netMode != 1) //something about wack ass MP
                            {
                                npc.netUpdate = true;
                                npc.dontTakeDamage = true;
                                masoBool[1] = true;
                                NetUpdateMaso(npc.whoAmI);
                            }

                            for (int index1 = 0; index1 < 100; ++index1) //gross vanilla dodge dust
                            {
                                int index2 = Dust.NewDust(npc.position, npc.width, npc.height, 31, 0.0f, 0.0f, 100, new Color(), 2f);
                                Main.dust[index2].position.X += Main.rand.Next(-20, 21);
                                Main.dust[index2].position.Y += Main.rand.Next(-20, 21);
                                Dust dust = Main.dust[index2];
                                dust.velocity *= 0.5f;
                                Main.dust[index2].scale *= 1f + Main.rand.Next(50) * 0.01f;
                                //Main.dust[index2].shader = GameShaders.Armor.GetSecondaryShader(npc.cWaist, npc);
                                if (Main.rand.Next(2) == 0)
                                {
                                    Main.dust[index2].scale *= 1f + Main.rand.Next(50) * 0.01f;
                                    Main.dust[index2].noGravity = true;
                                }
                            }
                            for (int i = 0; i < 5; i++) //gross vanilla dodge dust
                            {
                                int index3 = Gore.NewGore(npc.position + new Vector2(Main.rand.Next(npc.width), Main.rand.Next(npc.height)), new Vector2(), Main.rand.Next(61, 64), 1f);
                                Main.gore[index3].scale = 2f;
                                Main.gore[index3].velocity.X = Main.rand.Next(-50, 51) * 0.01f;
                                Main.gore[index3].velocity.Y = Main.rand.Next(-50, 51) * 0.01f;
                                Main.gore[index3].velocity *= 0.5f;

                                int index4 = Gore.NewGore(npc.position + new Vector2(Main.rand.Next(npc.width), Main.rand.Next(npc.height)), new Vector2(), Main.rand.Next(61, 64), 1f);
                                Main.gore[index4].scale = 2f;
                                Main.gore[index4].velocity.X = 1.5f + Main.rand.Next(-50, 51) * 0.01f;
                                Main.gore[index4].velocity.Y = 1.5f + Main.rand.Next(-50, 51) * 0.01f;
                                Main.gore[index4].velocity *= 0.5f;

                                int index5 = Gore.NewGore(npc.position + new Vector2(Main.rand.Next(npc.width), Main.rand.Next(npc.height)), new Vector2(), Main.rand.Next(61, 64), 1f);
                                Main.gore[index5].scale = 2f;
                                Main.gore[index5].velocity.X = -1.5f - Main.rand.Next(-50, 51) * 0.01f;
                                Main.gore[index5].velocity.Y = 1.5f + Main.rand.Next(-50, 51) * 0.01f;
                                Main.gore[index5].velocity *= 0.5f;

                                int index6 = Gore.NewGore(npc.position + new Vector2(Main.rand.Next(npc.width), Main.rand.Next(npc.height)), new Vector2(), Main.rand.Next(61, 64), 1f);
                                Main.gore[index6].scale = 2f;
                                Main.gore[index6].velocity.X = 1.5f - Main.rand.Next(-50, 51) * 0.01f;
                                Main.gore[index6].velocity.Y = -1.5f + Main.rand.Next(-50, 51) * 0.01f;
                                Main.gore[index6].velocity *= 0.5f;

                                int index7 = Gore.NewGore(npc.position + new Vector2(Main.rand.Next(npc.width), Main.rand.Next(npc.height)), new Vector2(), Main.rand.Next(61, 64), 1f);
                                Main.gore[index7].scale = 2f;
                                Main.gore[index7].velocity.X = -1.5f - Main.rand.Next(-50, 51) * 0.01f;
                                Main.gore[index7].velocity.Y = -1.5f + Main.rand.Next(-50, 51) * 0.01f;
                                Main.gore[index7].velocity *= 0.5f;
                            }

                            return false;
                        }
                        else
                        {
                            if (FargoSoulsWorld.FishronCount < FargoSoulsWorld.MaxCountHM && !FargoSoulsWorld.NoMasoBossScaling)
                                FargoSoulsWorld.FishronCount++;

                            if (fishBossEX == npc.whoAmI) //drop loot here (avoids the vanilla "fishron defeated" message)
                            {
                                FargoSoulsWorld.downedFishronEX = true;
                                if (Main.netMode == 0)
                                    Main.NewText("Duke Fishron EX has been defeated!", 50, 100, 255);
                                else if (Main.netMode == 2)
                                    NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("Duke Fishron EX has been defeated!"), new Color(50, 100, 255));

                                Main.PlaySound(npc.DeathSound, npc.Center);
                                npc.DropBossBags();
                                npc.DropItemInstanced(npc.position, npc.Size, mod.ItemType("CyclonicFin"));
                                /*int maxEX = Main.rand.Next(5) + 5;
                                for (int i = 0; i < maxEX; i++)
                                    npc.DropItemInstanced(npc.position, npc.Size, mod.ItemType("MutantScale"));*/

                                if (FargoSoulsWorld.downedAbom)
                                {
                                    /*int maxDoll = Main.rand.Next(5) + 5;
                                    for (int i = 0; i < maxDoll; i++)
                                        npc.DropItemInstanced(npc.position, npc.Size, mod.ItemType("AbominationnVoodooDoll"));*/

                                    int maxEnergy = Main.rand.Next(10) + 10;
                                    for (int i = 0; i < maxEnergy; i++)
                                        npc.DropItemInstanced(npc.position, npc.Size, mod.ItemType("MutatingEnergy"));
                                }
                                
                                for (int i = 0; i < 5; i++)
                                    Item.NewItem(npc.Hitbox, ItemID.Heart);
                                return false;
                            }
                        }
                        break;

                    case NPCID.CultistBoss:
                        if (FargoSoulsWorld.CultistCount < FargoSoulsWorld.MaxCountHM && !FargoSoulsWorld.NoMasoBossScaling)
                            FargoSoulsWorld.CultistCount++;
                        break;

                    case NPCID.MoonLordCore:
                        if (FargoSoulsWorld.MoonlordCount < FargoSoulsWorld.MaxCountHM && !FargoSoulsWorld.NoMasoBossScaling)
                            FargoSoulsWorld.MoonlordCount++;
                        break;

                    case NPCID.FlyingSnake:
                        if (!masoBool[0])
                        {
                            masoBool[0] = true;
                            npc.life = npc.lifeMax;
                            npc.active = true;
                            if (Main.netMode == 2)
                            {
                                npc.netUpdate = true;
                                NetUpdateMaso(npc.whoAmI);
                            }
                            return false;
                        }
                        break;

                    case NPCID.Lihzahrd:
                    case NPCID.LihzahrdCrawler:
                        if (Main.netMode != 1)
                            Projectile.NewProjectile(npc.Center, Vector2.UnitY * -6, ProjectileID.SpikyBallTrap, 30, 0f, Main.myPlayer);
                        break;

                    case NPCID.SkeletronHand:
                        NPC head = Main.npc[(int)npc.ai[1]];
                        if (head.active && head.type == NPCID.SkeletronHead && head.GetGlobalNPC<FargoSoulsGlobalNPC>().Counter == 0)
                        {
                            if (npc.ai[0] == 1f)
                                head.GetGlobalNPC<FargoSoulsGlobalNPC>().Counter = 1;
                            else
                                head.GetGlobalNPC<FargoSoulsGlobalNPC>().Counter = 2;
                        }
                        break;

                    case NPCID.GoblinSummoner:
                        if (Main.netMode != 1)
                        {
                            for (int i = 0; i < 50; i++)
                            {
                                Projectile.NewProjectile(npc.Center, new Vector2(Main.rand.Next(-500, 501) / 100f, Main.rand.Next(-1000, 1) / 100f),
                                    mod.ProjectileType("GoblinSpikyBall"), npc.damage / 8, 0, Main.myPlayer);
                            }
                        }
                        break;

                    case NPCID.Clown:
                        for (int i = 0; i < 30; i++)
                        {
                            int p = Projectile.NewProjectile(npc.position.X + Main.rand.Next(npc.width), npc.position.Y + Main.rand.Next(npc.height), Main.rand.Next(-500, 501) / 100f, Main.rand.Next(-1000, 101) / 100f, ProjectileID.BouncyGrenade, 200, 8f, Main.myPlayer);
                            Main.projectile[p].timeLeft -= Main.rand.Next(120);
                        }
                        break;

                    case NPCID.Shark:
                        if (Main.hardMode && Main.rand.Next(4) == 0 && Main.netMode != 1)
                            Projectile.NewProjectile(npc.Center, Vector2.Zero, ProjectileID.Cthulunado, npc.damage / 2, 0f, Main.myPlayer, 16, 11);
                        break;

                    case NPCID.PrimeCannon:
                    case NPCID.PrimeLaser:
                    case NPCID.PrimeVice:
                    case NPCID.PrimeSaw:
                        if (npc.ai[1] >= 0f && npc.ai[1] < 200f && Main.npc[(int)npc.ai[1]].type == NPCID.SkeletronPrime)
                        {
                            int ai1 = (int)npc.ai[1];
                            if (Main.npc[ai1].ai[0] == 1f)
                            {
                                switch (npc.type)
                                {
                                    case NPCID.PrimeLaser: Main.npc[ai1].ai[0] = 3f; break;
                                    case NPCID.PrimeCannon: Main.npc[ai1].ai[0] = 4f; break;
                                    case NPCID.PrimeSaw: Main.npc[ai1].ai[0] = 5f; break;
                                    case NPCID.PrimeVice: Main.npc[ai1].ai[0] = 6f; break;
                                    default: break;
                                }
                            }
                            Main.npc[ai1].GetGlobalNPC<FargoSoulsGlobalNPC>().masoBool[0] = true;
                            Main.npc[ai1].GetGlobalNPC<FargoSoulsGlobalNPC>().NetUpdateMaso(ai1);
                            Main.npc[ai1].netUpdate = true;
                        }
                        break;

                    case NPCID.ZombieMushroom:
                    case NPCID.ZombieMushroomHat:
                    case NPCID.AnomuraFungus:
                    case NPCID.MushiLadybug:
                    case NPCID.FungoFish:
                    case NPCID.FungiBulb:
                    case NPCID.GiantFungiBulb:
                        if (Main.netMode != 1)// && Main.hardMode)
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                int spore = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.FungiSpore);
                                if (spore < 200)
                                {
                                    Main.npc[spore].damage = npc.damage / 3;
                                    Main.npc[spore].velocity = new Vector2(Main.rand.NextFloat(-5, 5), Main.rand.NextFloat(-5, 5)) / 2;
                                    if (Main.netMode == 2)
                                        NetMessage.SendData(23, -1, -1, null, spore);
                                }
                            }
                        }
                        break;

                    case NPCID.RainbowSlime:
                        if (!masoBool[0])
                        {
                            npc.active = false;
                            Main.PlaySound(npc.DeathSound);
                            if (Main.netMode != 1)
                            {
                                for (int i = 0; i < 4; i++)
                                {
                                    int slimeIndex = NPC.NewNPC((int)(npc.position.X + npc.width / 2), (int)(npc.position.Y + npc.height), NPCID.RainbowSlime);
                                    if (slimeIndex < 200)
                                    {
                                        NPC slime = Main.npc[slimeIndex];

                                        slime.position = slime.Center;
                                        slime.width = (int)(slime.width / slime.scale);
                                        slime.height = (int)(slime.height / slime.scale);
                                        slime.scale = 1f;
                                        slime.Center = slime.position;

                                        slime.lifeMax /= 5;
                                        slime.life = slime.lifeMax;

                                        slime.GetGlobalNPC<FargoSoulsGlobalNPC>().masoBool[0] = true;
                                        slime.velocity = new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 1));

                                        if (Main.netMode == 2)
                                            NetMessage.SendData(23, -1, -1, null, slimeIndex);
                                    }
                                }
                            }
                            for (int i = 0; i < 20; i++)
                            {
                                int num469 = Dust.NewDust(new Vector2(npc.Center.X, npc.Center.Y), npc.width, npc.height, DustID.RainbowMk2, -npc.velocity.X * 0.2f, -npc.velocity.Y * 0.2f, 100, default(Color), 5f);
                                Main.dust[num469].noGravity = true;
                                Main.dust[num469].velocity *= 2f;
                                num469 = Dust.NewDust(new Vector2(npc.Center.X, npc.Center.Y), npc.width, npc.height, DustID.RainbowMk2, -npc.velocity.X * 0.2f, -npc.velocity.Y * 0.2f, 100, default(Color), 2f);
                                Main.dust[num469].velocity *= 2f;
                            }
                            return false;
                        }
                        break;

                    case NPCID.Medusa:
                    case NPCID.IchorSticker:
                    case NPCID.SeekerHead:
                    case NPCID.RedDevil:
                    case NPCID.WyvernHead:
                    case NPCID.DuneSplicerHead:
                    case NPCID.AngryNimbus:
                    case NPCID.PigronCorruption:
                    case NPCID.PigronCrimson:
                    case NPCID.PigronHallow:
                        if (!Main.hardMode) //in pre-hm, fake death
                        {
                            Item.NewItem(npc.Hitbox, ItemID.GoldCoin, 1 + Main.rand.Next(2));

                            npc.active = false;
                            Main.PlaySound(npc.DeathSound, npc.Center);
                            return false;
                        }
                        break;

                    case NPCID.Mimic:
                        Item.NewItem(npc.Hitbox, ItemID.GoldCoin, 1 + Main.rand.Next(2));

                        if (Main.netMode != 1)
                        {
                            int max = 5;
                            for (int i = 0; i < max; i++)
                                Projectile.NewProjectile(npc.position.X + Main.rand.Next(npc.width), npc.position.Y + Main.rand.Next(npc.height),
                                    Main.rand.Next(-30, 31) * .1f, Main.rand.Next(-40, -15) * .1f, mod.ProjectileType("FakeHeart"), 20, 0f, Main.myPlayer);
                        }
                        goto case NPCID.Medusa;

                    case NPCID.PresentMimic:
                    case NPCID.BigMimicCorruption:
                    case NPCID.BigMimicCrimson:
                    case NPCID.BigMimicHallow:
                    case NPCID.BigMimicJungle:
                        if (Main.netMode != 1)
                        {
                            int max = 5;
                            for (int i = 0; i < max; i++)
                                Projectile.NewProjectile(npc.position.X + Main.rand.Next(npc.width), npc.position.Y + Main.rand.Next(npc.height),
                                    Main.rand.Next(-30, 31) * .1f, Main.rand.Next(-40, -15) * .1f, mod.ProjectileType("FakeHeart"), 20, 0f, Main.myPlayer);
                        }
                        break;

                    case NPCID.Hornet:
                    case NPCID.HornetFatty:
                    case NPCID.HornetHoney:
                    case NPCID.HornetLeafy:
                    case NPCID.HornetSpikey:
                    case NPCID.HornetStingy:
                        if (BossIsAlive(ref beeBoss, NPCID.QueenBee))
                        {
                            npc.active = false;
                            Main.PlaySound(npc.DeathSound, npc.Center);
                            return false;
                        }
                        break;

                    case NPCID.BigEater:
                    case NPCID.EaterofSouls:
                    case NPCID.LittleEater:
                        if (BossIsAlive(ref eaterBoss, NPCID.EaterofWorldsHead))
                        {
                            npc.active = false;
                            Main.PlaySound(npc.DeathSound, npc.Center);
                            return false;
                        }
                        break;

                    case NPCID.Probe:
                        if (BossIsAlive(ref destroyBoss, NPCID.TheDestroyer))
                        {
                            npc.active = false;
                            Main.PlaySound(npc.DeathSound, npc.Center);
                            return false;
                        }
                        break;

                    case NPCID.IlluminantBat:
                        if (masoBool[0])
                        {
                            npc.active = false;
                            Main.PlaySound(npc.DeathSound, npc.Center);
                            return false;
                        }
                        break;

                    case NPCID.Gastropod:
                        if (Main.netMode != 1 && npc.HasPlayerTarget)
                        {
                            Vector2 vel = Vector2.Normalize(Main.player[npc.target].Center - npc.Center) * 7f;
                            for (int i = 0; i < 12; i++)
                                Projectile.NewProjectile(npc.Center, vel.RotatedBy(2 * Math.PI / 12 * i), ProjectileID.PinkLaser, 25, 0f, Main.myPlayer);
                        }
                        break;

                    case NPCID.DD2GoblinBomberT1:
                    case NPCID.DD2GoblinBomberT2:
                    case NPCID.DD2GoblinBomberT3:
                        if (Main.netMode != 1)
                        {
                            for (int i = 0; i < 3; i++)
                                Projectile.NewProjectile(npc.Center, new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-9f, -6f)),
                                  ProjectileID.DD2GoblinBomb, npc.damage / 4, 0, Main.myPlayer);
                        }
                        break;

                    case NPCID.PossessedArmor:
                        if (Main.rand.Next(2) == 0 && Main.netMode != 1)
                        {
                            int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.Ghost);
                            if (n != 200 && Main.netMode == 2)
                                NetMessage.SendData(23, -1, -1, null, n);
                        }
                        break;

                    case NPCID.Mummy:
                    case NPCID.DarkMummy:
                    case NPCID.LightMummy:
                        if (Main.rand.Next(5) == 0 && Main.netMode != 1)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.WallCreeper);
                                if (n != 200)
                                {
                                    Main.npc[n].velocity.X = Main.rand.NextFloat(-5f, 5f);
                                    Main.npc[n].velocity.Y = Main.rand.NextFloat(-10f, 0f);
                                    if (Main.netMode == 2)
                                        NetMessage.SendData(23, -1, -1, null, n);
                                }
                            }
                        }
                        break;

                    case NPCID.Skeleton:
                    case NPCID.HeadacheSkeleton:
                    case NPCID.MisassembledSkeleton:
                    case NPCID.PantlessSkeleton:
                    case NPCID.SkeletonTopHat:
                    case NPCID.SkeletonAstonaut:
                    case NPCID.SkeletonAlien:
                    case NPCID.BoneThrowingSkeleton:
                    case NPCID.BoneThrowingSkeleton2:
                    case NPCID.BoneThrowingSkeleton3:
                    case NPCID.BoneThrowingSkeleton4:
                        if (Main.netMode != 1)
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                Vector2 speed = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                                speed.Normalize();
                                speed *= Main.rand.NextFloat(3f, 6f);
                                speed.Y -= Math.Abs(speed.X) * 0.2f;
                                speed.Y -= 3f;
                                if (Main.netMode != 1)
                                    Projectile.NewProjectile(npc.Center, speed, ProjectileID.SkeletonBone, npc.damage / 4, 0f, Main.myPlayer);
                            }
                        }
                        break;

                    case NPCID.SolarSpearman:
                        int t = npc.HasPlayerTarget ? npc.target : npc.FindClosestPlayer();
                        if (t != -1 && Main.player[t].active && !Main.player[t].dead && Main.netMode != 1)
                        {
                            Vector2 velocity = Main.player[t].Center - npc.Center;
                            velocity.Normalize();
                            velocity *= 14f;
                            Projectile.NewProjectile(npc.Center, velocity, mod.ProjectileType("DrakanianDaybreak"), npc.damage / 4, 1f, Main.myPlayer);
                        }
                        Main.PlaySound(SoundID.Item1, npc.Center);
                        if (Main.rand.Next(2) == 0)
                        {
                            npc.Transform(NPCID.SolarSolenian);
                            return false;
                        }
                        goto case NPCID.SolarSolenian;

                    case NPCID.SolarSolenian:
                    case NPCID.SolarCorite:
                    case NPCID.SolarCrawltipedeHead:
                    case NPCID.SolarCrawltipedeBody:
                    case NPCID.SolarCrawltipedeTail:
                    case NPCID.SolarDrakomire:
                    case NPCID.SolarDrakomireRider:
                    case NPCID.SolarSroller:
                        if (NPC.TowerActiveSolar && NPC.ShieldStrengthTowerSolar > 0)
                        {
                            int p = NPC.FindFirstNPC(NPCID.LunarTowerSolar);
                            if (p != -1 && Main.npc[p].active && npc.lastInteraction != -1 && Main.player[npc.lastInteraction].Distance(Main.npc[p].Center) < 5000)
                            {
                                break;
                            }
                            else //if pillar active, but out of range, dont contribute to shield
                            {
                                Main.PlaySound(npc.DeathSound, npc.Center);
                                return false;
                            }
                        }
                        break;

                    case NPCID.VortexHornet:
                    case NPCID.VortexHornetQueen:
                    case NPCID.VortexLarva:
                    case NPCID.VortexRifleman:
                    case NPCID.VortexSoldier:
                        if (NPC.TowerActiveVortex && NPC.ShieldStrengthTowerVortex > 0)
                        {
                            int p = NPC.FindFirstNPC(NPCID.LunarTowerVortex);
                            if (p != -1 && Main.npc[p].active && npc.lastInteraction != -1 && Main.player[npc.lastInteraction].Distance(Main.npc[p].Center) < 5000)
                            {
                                break;
                            }
                            else //if pillar active, but out of range, dont contribute to shield
                            {
                                Main.PlaySound(npc.DeathSound, npc.Center);
                                return false;
                            }
                        }
                        break;

                    case NPCID.NebulaBrain:
                        if (npc.HasValidTarget)
                        {
                            Player target = Main.player[npc.target];
                            Vector2 boltVel = target.Center - npc.Center;
                            boltVel.Normalize();
                            boltVel *= 9;

                            for (int i = 0; i < (int)npc.localAI[2] / 60; i++)
                            {
                                Vector2 spawnPos = npc.position;
                                spawnPos.X += Main.rand.Next(npc.width);
                                spawnPos.Y += Main.rand.Next(npc.height);

                                Vector2 boltVel2 = boltVel.RotatedBy(MathHelper.ToRadians(Main.rand.Next(-20, 21)));
                                boltVel2 *= Main.rand.NextFloat(0.8f, 1.2f);

                                if (Main.netMode != 1)
                                    Projectile.NewProjectile(spawnPos, boltVel2, ProjectileID.NebulaLaser, 48, 0f, Main.myPlayer);
                            }
                        }
                        goto case NPCID.NebulaSoldier;

                    case NPCID.NebulaHeadcrab:
                        if (npc.ai[0] == 5f) //latched on player
                        {
                            npc.Transform(NPCID.NebulaBrain);
                            return false;
                        }
                        else if (Main.rand.Next(3) != 0) //die without contributing to pillar shield
                        {
                            Main.PlaySound(npc.DeathSound, npc.Center);
                            return false;
                        }
                        goto case NPCID.NebulaSoldier;

                    case NPCID.NebulaBeast:
                    case NPCID.NebulaSoldier:
                        if (NPC.TowerActiveNebula && NPC.ShieldStrengthTowerNebula > 0)
                        {
                            int p = NPC.FindFirstNPC(NPCID.LunarTowerNebula);
                            if (p != -1 && Main.npc[p].active && npc.lastInteraction != -1 && Main.player[npc.lastInteraction].Distance(Main.npc[p].Center) < 5000)
                            {
                                break;
                            }
                            else //if pillar active, but out of range, dont contribute to shield
                            {
                                Main.PlaySound(npc.DeathSound, npc.Center);
                                return false;
                            }
                        }
                        break;

                    case NPCID.StardustJellyfishBig:
                    case NPCID.StardustSoldier:
                    case NPCID.StardustSpiderBig:
                    case NPCID.StardustWormHead:
                        if (NPC.TowerActiveStardust && Main.netMode != 1 && NPC.CountNPCS(NPCID.StardustCellSmall) < 10)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.StardustCellSmall);
                                if (n < 200)
                                {
                                    Main.npc[n].velocity.X = Main.rand.Next(-10, 11);
                                    Main.npc[n].velocity.Y = Main.rand.Next(-10, 11);
                                    if (Main.netMode == 2)
                                        NetMessage.SendData(23, -1, -1, null, n);
                                }
                            }
                        }

                        if (NPC.TowerActiveStardust && NPC.ShieldStrengthTowerStardust > 0)
                        {
                            int p = NPC.FindFirstNPC(NPCID.LunarTowerStardust);
                            if (p != -1 && Main.npc[p].active && npc.lastInteraction != -1 && Main.player[npc.lastInteraction].Distance(Main.npc[p].Center) < 5000)
                            {
                                break;
                            }
                            else //if pillar active, but out of range, dont contribute to shield
                            {
                                Main.PlaySound(npc.DeathSound, npc.Center);
                                return false;
                            }
                        }
                        break;

                    default:
                        break;
                }
            }

            return true;
        }

        private void GrossVanillaDodgeDust(NPC npc)
        {
            for (int index1 = 0; index1 < 100; ++index1)
            {
                int index2 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 31, 0.0f, 0.0f, 100, new Color(), 2f);
                Main.dust[index2].position.X += Main.rand.Next(-20, 21);
                Main.dust[index2].position.Y += Main.rand.Next(-20, 21);
                Dust dust = Main.dust[index2];
                dust.velocity *= 0.4f;
                Main.dust[index2].scale *= 1f + Main.rand.Next(40) * 0.01f;
                //Main.dust[index2].shader = GameShaders.Armor.GetSecondaryShader(npc.cWaist, npc);
                if (Main.rand.Next(2) == 0)
                {
                    Main.dust[index2].scale *= 1f + Main.rand.Next(40) * 0.01f;
                    Main.dust[index2].noGravity = true;
                }
            }

            int index3 = Gore.NewGore(new Vector2(npc.Center.X - 24, npc.Center.Y - 24), new Vector2(), Main.rand.Next(61, 64), 1f);
            Main.gore[index3].scale = 1.5f;
            Main.gore[index3].velocity.X = Main.rand.Next(-50, 51) * 0.01f;
            Main.gore[index3].velocity.Y = Main.rand.Next(-50, 51) * 0.01f;
            Main.gore[index3].velocity *= 0.4f;

            int index4 = Gore.NewGore(new Vector2(npc.Center.X - 24, npc.Center.Y - 24), new Vector2(), Main.rand.Next(61, 64), 1f);
            Main.gore[index4].scale = 1.5f;
            Main.gore[index4].velocity.X = 1.5f + Main.rand.Next(-50, 51) * 0.01f;
            Main.gore[index4].velocity.Y = 1.5f + Main.rand.Next(-50, 51) * 0.01f;
            Main.gore[index4].velocity *= 0.4f;

            int index5 = Gore.NewGore(new Vector2(npc.Center.X - 24, npc.Center.Y - 24), new Vector2(), Main.rand.Next(61, 64), 1f);
            Main.gore[index5].scale = 1.5f;
            Main.gore[index5].velocity.X = -1.5f - Main.rand.Next(-50, 51) * 0.01f;
            Main.gore[index5].velocity.Y = 1.5f + Main.rand.Next(-50, 51) * 0.01f;
            Main.gore[index5].velocity *= 0.4f;

            int index6 = Gore.NewGore(new Vector2(npc.Center.X - 24, npc.Center.Y - 24), new Vector2(), Main.rand.Next(61, 64), 1f);
            Main.gore[index6].scale = 1.5f;
            Main.gore[index6].velocity.X = 1.5f - Main.rand.Next(-50, 51) * 0.01f;
            Main.gore[index6].velocity.Y = -1.5f + Main.rand.Next(-50, 51) * 0.01f;
            Main.gore[index6].velocity *= 0.4f;

            int index7 = Gore.NewGore(new Vector2(npc.Center.X - 24, npc.Center.Y - 24), new Vector2(), Main.rand.Next(61, 64), 1f);
            Main.gore[index7].scale = 1.5f;
            Main.gore[index7].velocity.X = -1.5f - Main.rand.Next(-50, 51) * 0.01f;
            Main.gore[index7].velocity.Y = -1.5f + Main.rand.Next(-50, 51) * 0.01f;
            Main.gore[index7].velocity *= 0.4f;
        }

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();

            if (modPlayer.CactusEnchant)
            {
                Needles = true;
            }

            if (FargoSoulsWorld.MasochistMode)
            {
                switch (npc.type)
                {
                    case NPCID.Salamander:
                    case NPCID.Salamander2:
                    case NPCID.Salamander3:
                    case NPCID.Salamander4:
                    case NPCID.Salamander5:
                    case NPCID.Salamander6:
                    case NPCID.Salamander7:
                    case NPCID.Salamander8:
                    case NPCID.Salamander9:
                        if (!masoBool[0])
                        {
                            masoBool[0] = true;
                            npc.Opacity *= 5;
                        }
                        break;

                    case NPCID.MisterStabby:
                    case NPCID.AnglerFish:
                        if (!masoBool[0])
                        {
                            masoBool[0] = true;
                            npc.Opacity *= 5;
                        }
                        break;

                    case NPCID.GiantShelly:
                    case NPCID.GiantShelly2:
                        player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " was impaled by a Giant Shelly."), damage / 4, 0);
                        break;

                    case NPCID.BoneLee:
                        if (Main.rand.Next(10) == 0)
                        {
                            Vector2 teleportTarget = player.Center;
                            float offset = 100f * -player.direction;
                            teleportTarget.X += offset;
                            teleportTarget.Y -= 50f;
                            if (!Collision.CanHit(teleportTarget, 1, 1, player.position, player.width, player.height))
                            {
                                teleportTarget.X -= offset * 2f;
                                if (!Collision.CanHit(teleportTarget, 1, 1, player.position, player.width, player.height))
                                    break;
                            }
                            GrossVanillaDodgeDust(npc);
                            npc.Center = teleportTarget;
                            npc.netUpdate = true;
                            GrossVanillaDodgeDust(npc);
                        }
                        break;

                    case NPCID.ForceBubble:
                        if (Main.rand.Next(2) == 0 && Main.netMode != 1)
                        {
                            Vector2 velocity = player.Center - npc.Center;
                            velocity.Normalize();
                            velocity *= 14f;
                            int Damage = Main.expertMode ? 28 : 35;
                            Projectile.NewProjectile(npc.Center, velocity, ProjectileID.MartianTurretBolt, Damage, 0f, Main.myPlayer);
                        }
                        break;

                    case NPCID.SolarSolenian:
                        if (npc.ai[2] <= -6f)
                        {
                            damage /= 3;
                            if (npc.HasPlayerTarget && npc.Distance(Main.player[npc.target].Center) > 250)
                                npc.ai[2] = -6f;
                        }
                        break;

                    case NPCID.SkeletronPrime:
                        if (npc.ai[0] != 2f) //in phase 1
                        {
                            int armCount = 0;
                            for (int i = 0; i < 200; i++)
                            {
                                if (Main.npc[i].active && Main.npc[i].type >= 128 && Main.npc[i].type <= 131 && Main.npc[i].ai[1] == npc.whoAmI)
                                    armCount++;
                            }
                            switch (armCount)
                            {
                                case 4:
                                    damage = 0;
                                    break;
                                case 3:
                                    damage = damage / 2;
                                    break;
                                case 2:
                                    damage = (int)(damage * 0.75);
                                    break;
                                case 1:
                                    damage = (int)(damage * 0.9);
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;

                    case NPCID.Golem:
                        damage = (int)(damage * 0.8);
                        break;

                    case NPCID.IceTortoise:
                        float reduction = (float)npc.life / npc.lifeMax;
                        if (reduction < 0.5f)
                            reduction = 0.5f;
                        damage = (int)(damage * reduction);
                        break;

                    case NPCID.MoonLordCore:
                        damage = damage * 2 / 3;
                        break;
                    case NPCID.MoonLordHead:
                        damage = damage * 2;
                        break;

                    case NPCID.GiantTortoise:
                        player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " was impaled by a Giant Tortoise."), damage / 2, 0);
                        break;

                    case NPCID.DukeFishron:
                        if (masoBool[2])
                            damage = 0;
                        break;

                    case NPCID.Plantera:
                        if (item.type == ItemID.FetidBaghnakhs)
                            damage /= 4;
                        break;

                    case NPCID.GoblinSummoner:
                        if (Main.rand.Next(3) == 0 && Main.netMode != 1)
                        {
                            Vector2 vel = new Vector2(9f, 0f).RotatedByRandom(2 * Math.PI);
                            for (int i = 0; i < 6; i++)
                            {
                                Vector2 speed = vel.RotatedBy(2 * Math.PI / 6 * (i + Main.rand.NextDouble() - 0.5));
                                float ai1 = Main.rand.Next(10, 80) * (1f / 1000f);
                                if (Main.rand.Next(2) == 0)
                                    ai1 *= -1f;
                                float ai0 = Main.rand.Next(10, 80) * (1f / 1000f);
                                if (Main.rand.Next(2) == 0)
                                    ai0 *= -1f;
                                Projectile.NewProjectile(npc.Center, speed, mod.ProjectileType("ShadowflameTentacleHostile"), npc.damage / 4, 0f, Main.myPlayer, ai0, ai1);
                            }
                        }
                        break;

                    case NPCID.DungeonGuardian:
                        damage = 1;
                        break;

                    case NPCID.Psycho:
                        Counter = 0;
                        break;

                    case NPCID.Nymph:
                        if (player.loveStruck)
                        {
                            npc.life += damage;
                            if (npc.life > npc.lifeMax)
                                npc.life = npc.lifeMax;
                            CombatText.NewText(npc.Hitbox, CombatText.HealLife, damage);
                            damage = 0;
                            npc.netUpdate = true;
                        }
                        break;

                    case NPCID.CultistBoss:
                        Counter2 += damage / 10;
                        break;

                    case NPCID.DarkCaster:
                    case NPCID.FireImp:
                    case NPCID.GoblinSorcerer:
                        if (Counter2 == 0)
                        {
                            Counter2 = 180;
                            if (Main.netMode == 1)
                            {
                                var netMessage = mod.GetPacket();
                                netMessage.Write((byte)7);
                                netMessage.Write((byte)npc.whoAmI);
                                netMessage.Write(Counter2);
                                netMessage.Send();
                            }
                        }
                        break;

                    case NPCID.Tim:
                    case NPCID.RuneWizard:
                        if (Counter2 == 0)
                        {
                            Counter2 = 90;
                            if (Main.netMode == 1)
                            {
                                var netMessage = mod.GetPacket();
                                netMessage.Write((byte)7);
                                netMessage.Write((byte)npc.whoAmI);
                                netMessage.Write(Counter2);
                                netMessage.Send();
                            }
                        }
                        break;

                    default:
                        break;
                }

                if (npc.catchItem != 0 && npc.lifeMax == 5)
                    player.AddBuff(mod.BuffType("Guilty"), 300);
            }
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[projectile.owner];
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();

            if (modPlayer.CactusEnchant)
                Needles = true;

            //bees ignore defense
            if (modPlayer.BeeEnchant && !modPlayer.TerrariaSoul && projectile.type == ProjectileID.GiantBee)
                damage = (int)(damage + npc.defense * .5);

            if (modPlayer.SpiderEnchant && projectile.minion && Main.rand.Next(101) <= modPlayer.SummonCrit)
            {
                crit = true;
            }
                

            if (FargoSoulsWorld.MasochistMode)
            {
                switch (npc.type)
                {
                    case NPCID.LunarTowerNebula:
                    case NPCID.LunarTowerSolar:
                    case NPCID.LunarTowerStardust:
                    case NPCID.LunarTowerVortex:
                        if (npc.Distance(Main.player[projectile.owner].Center) > 2500)
                            damage = 0;
                        break;

                    case NPCID.Salamander:
                    case NPCID.Salamander2:
                    case NPCID.Salamander3:
                    case NPCID.Salamander4:
                    case NPCID.Salamander5:
                    case NPCID.Salamander6:
                    case NPCID.Salamander7:
                    case NPCID.Salamander8:
                    case NPCID.Salamander9:
                        if (!masoBool[0])
                        {
                            masoBool[0] = true;
                            npc.Opacity *= 5;
                        }
                        break;

                    case NPCID.MisterStabby:
                    case NPCID.AnglerFish:
                        if (!masoBool[0])
                        {
                            masoBool[0] = true;
                            npc.Opacity *= 5;
                        }
                        break;

                    case NPCID.BoneLee:
                        if (Main.rand.Next(10) == 0 && npc.HasPlayerTarget)
                        {
                            Player target = Main.player[npc.target];
                            if (target.active && !target.dead)
                            {
                                Vector2 teleportTarget = target.Center;
                                float offset = 100f * -target.direction;
                                teleportTarget.X += offset;
                                teleportTarget.Y -= 50f;
                                if (!Collision.CanHit(teleportTarget, 1, 1, target.position, target.width, target.height))
                                {
                                    teleportTarget.X -= offset * 2f;
                                    if (!Collision.CanHit(teleportTarget, 1, 1, target.position, target.width, target.height))
                                        break;
                                }
                                GrossVanillaDodgeDust(npc);
                                npc.Center = teleportTarget;
                                npc.netUpdate = true;
                                GrossVanillaDodgeDust(npc);
                            }
                        }
                        break;

                    case NPCID.ForceBubble:
                        if (Main.rand.Next(2) == 0 && Main.netMode != 1)
                        {
                            int closest = npc.FindClosestPlayer();
                            if (closest != -1)
                            {
                                Player target = Main.player[closest];
                                Vector2 velocity = target.Center - npc.Center;
                                velocity.Normalize();
                                velocity *= 14f;
                                int Damage = Main.expertMode ? 28 : 35;
                                Projectile.NewProjectile(npc.Center, velocity, ProjectileID.MartianTurretBolt, Damage, 0f, Main.myPlayer);
                            }
                        }
                        break;

                    case NPCID.SolarSolenian:
                        if (npc.ai[2] <= -6f)
                        {
                            damage /= 3;
                            npc.ai[2] = -6f;
                        }
                        break;

                    case NPCID.TheDestroyer:
                    case NPCID.TheDestroyerBody:
                    case NPCID.TheDestroyerTail:
                        //if (projectile.type == ProjectileID.HallowStar) damage /= 4;
                        if (projectile.numHits > 0 && !projectile.minion)
                            damage = (int)(damage * (0.5 + 0.5 * 1 / projectile.numHits));
                        break;

                    case NPCID.Golem:
                        damage = (int)(damage * 0.8);
                        break;

                    case NPCID.GolemFistLeft:
                    case NPCID.GolemFistRight:
                        if (projectile.maxPenetrate != 1 && !projectile.minion)
                            projectile.active = false;
                        break;

                    case NPCID.SkeletronPrime:
                        if (npc.ai[0] != 2f)
                        {
                            int armCount = 0;
                            for (int i = 0; i < 200; i++)
                            {
                                if (Main.npc[i].active && Main.npc[i].type >= 128 && Main.npc[i].type <= 131 && Main.npc[i].ai[1] == npc.whoAmI)
                                    armCount++;
                            }
                            switch (armCount)
                            {
                                case 4:
                                    damage = 0;
                                    break;
                                case 3:
                                    damage = damage / 2;
                                    break;
                                case 2:
                                    damage = (int)(damage * 0.75);
                                    break;
                                case 1:
                                    damage = (int)(damage * 0.9);
                                    break;
                                default:
                                    break;
                            }
                        }
                        if (projectile.type == ProjectileID.HallowStar)
                            damage /= 4;
                        break;

                    case NPCID.IceTortoise:
                        float reduction = (float)npc.life / npc.lifeMax;
                        if (reduction < 0.5f)
                            reduction = 0.5f;
                        damage = (int)(damage * reduction);
                        break;

                    case NPCID.SkeletronHead:
                    case NPCID.SkeletronHand:
                        if (projectile.type == ProjectileID.Bee || projectile.type == ProjectileID.GiantBee)
                            damage /= 2;
                        break;

                    case NPCID.WallofFlesh:
                    case NPCID.WallofFleshEye:
                        if (projectile.type == ProjectileID.Bee || projectile.type == ProjectileID.GiantBee)
                            damage /= 2;
                        break;

                    case NPCID.MoonLordCore:
                        damage = damage * 2 / 3;
                        break;
                    case NPCID.MoonLordHead:
                        damage = damage * 2;
                        break;

                    case NPCID.CultistDragonBody1:
                    case NPCID.CultistDragonBody2:
                    case NPCID.CultistDragonBody3:
                    case NPCID.CultistDragonBody4:
                    case NPCID.CultistDragonTail:
                        if (projectile.maxPenetrate > 1)
                            damage /= projectile.maxPenetrate;
                        else if (projectile.maxPenetrate < 0)
                            damage /= 4;
                        break;

                    case NPCID.DukeFishron:
                        if (masoBool[2])
                            damage = 0;
                        break;

                    case NPCID.GoblinSummoner:
                        if (Main.rand.Next(4) == 0 && Main.netMode != 1)
                        {
                            Vector2 vel = new Vector2(12f, 0f).RotatedByRandom(2 * Math.PI);
                            for (int i = 0; i < 6; i++)
                            {
                                Vector2 speed = vel.RotatedBy(2 * Math.PI / 6 * (i + Main.rand.NextDouble() - 0.5));
                                float ai1 = Main.rand.Next(10, 80) * (1f / 1000f);
                                if (Main.rand.Next(2) == 0)
                                    ai1 *= -1f;
                                float ai0 = Main.rand.Next(10, 80) * (1f / 1000f);
                                if (Main.rand.Next(2) == 0)
                                    ai0 *= -1f;
                                Projectile.NewProjectile(npc.Center, speed, mod.ProjectileType("ShadowflameTentacleHostile"), npc.damage / 4, 0f, Main.myPlayer, ai0, ai1);
                            }
                        }
                        break;

                    case NPCID.DungeonGuardian:
                        damage = 1;
                        break;

                    case NPCID.Psycho:
                        Counter = 0;
                        break;

                    case NPCID.Nymph:
                        if (Main.player[projectile.owner].loveStruck)
                        {
                            npc.life += damage;
                            if (npc.life > npc.lifeMax)
                                npc.life = npc.lifeMax;
                            CombatText.NewText(npc.Hitbox, CombatText.HealLife, damage);
                            damage = 0;
                            npc.netUpdate = true;
                        }
                        break;

                    case NPCID.CultistBoss: //track damage types
                        if (projectile.melee || projectile.thrown)
                            Counter2 += damage / 10;
                        else if (projectile.ranged)
                            Timer += damage / 10;
                        else if (projectile.magic)
                            Counter += damage / 10;
                        else if (projectile.minion)
                            npc.localAI[3] += damage / 10;
                        break;

                    case NPCID.DarkCaster:
                    case NPCID.FireImp:
                    case NPCID.GoblinSorcerer:
                        if (Counter2 == 0)
                        {
                            Counter2 = 180;
                            if (Main.netMode == 1)
                            {
                                var netMessage = mod.GetPacket();
                                netMessage.Write((byte)7);
                                netMessage.Write((byte)npc.whoAmI);
                                netMessage.Write(Counter2);
                                netMessage.Send();
                            }
                        }
                        break;

                    case NPCID.Tim:
                    case NPCID.RuneWizard:
                        if (Counter2 == 0)
                        {
                            Counter2 = 90;
                            if (Main.netMode == 1)
                            {
                                var netMessage = mod.GetPacket();
                                netMessage.Write((byte)7);
                                netMessage.Write((byte)npc.whoAmI);
                                netMessage.Write(Counter2);
                                netMessage.Send();
                            }
                        }
                        break;

                    default:
                        break;
                }

                if (npc.catchItem != 0 && npc.lifeMax == 5 && projectile.friendly && !projectile.hostile)
                    player.AddBuff(mod.BuffType("Guilty"), 300);
            }
        }

        public override void ModifyHitPlayer(NPC npc, Player target, ref int damage, ref bool crit)
        {
            FargoPlayer modPlayer = target.GetModPlayer<FargoPlayer>();

            if (target.HasBuff(mod.BuffType("ShellHide")))
                damage *= 2;

            if (SqueakyToy)
            {
                damage = 1;
                modPlayer.Squeak(target.Center);
            }
        }

        private bool StealFromInventory(Player target, ref Item item)
        {
            if (!item.IsAir)
            {
                int i = Item.NewItem((int)target.position.X, (int)target.position.Y, target.width, target.height, item.type, 1, false, 0, false, false);
                Main.item[i].netDefaults(item.netID);
                Main.item[i].Prefix(item.prefix);
                Main.item[i].stack = item.stack;
                Main.item[i].velocity.X = Main.rand.Next(-20, 21) * 0.2f;
                Main.item[i].velocity.Y = Main.rand.Next(-20, 1) * 0.2f;
                Main.item[i].noGrabDelay = 100;
                Main.item[i].newAndShiny = false;

                if (Main.netMode == 1)
                    NetMessage.SendData(21, -1, -1, null, i, 0.0f, 0.0f, 0.0f, 0, 0, 0);

                item = new Item();

                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool StrikeNPC(NPC npc, ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            Player player = Main.player[Main.myPlayer];
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();

            if (FargoSoulsWorld.MasochistMode)
            {
                if (PaladinsShield)
                    damage /= 2;

                if (npc.realLife == -1)
                    ResetRegenTimer(npc);
                else
                    ResetRegenTimer(Main.npc[npc.realLife]);
            }

            if (OceanicMaul)
            {
                damage += 15;
                //damage *= 1.3;
            }
            if (CurseoftheMoon)
            {
                damage += 5;
                //damage *= 1.1;
            }
            if (Rotting)
            {
                damage += 5;
            }

            if (modPlayer.KnightEnchant && Villain && !npc.boss)
            {
                damage *= 1.5;
            }

            if (crit && modPlayer.ShroomEnchant && !modPlayer.TerrariaSoul && player.stealth == 0)
            {
                damage *= 1.5;
            }

            if (crit && modPlayer.Graze)
            {
                damage *= 1.0 + modPlayer.GrazeBonus;
            }

            //normal damage calc
            return true;
        }

        public override void OnHitByItem(NPC npc, Player player, Item item, int damage, float knockback, bool crit)
        {
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();

            if (modPlayer.ValhallaEnchant && SoulConfig.Instance.GetValue(SoulConfig.Instance.ValhallaEffect)
                 && valhallaCounter == 0)
            {
                squireCounter += 5;

                if (squireCounter >= 100)
                {
                    valhallaCounter = 1020;
                    valhallaPlayer = player.whoAmI;
                    squireCounter = 0;
                }
            }
            else if (modPlayer.SquireEnchant && SoulConfig.Instance.GetValue(SoulConfig.Instance.SquireKB)
                 && !npc.GetGlobalNPC<FargoSoulsGlobalNPC>().SpecialEnchantImmune && npc.knockBackResist < 1 && !npc.HasBuff(mod.BuffType("SquireKBDebuff")))
            {
                squireCounter += 5;

                if (squireCounter >= 100)
                {
                    originalKB = npc.knockBackResist;
                    npc.knockBackResist = 1f;
                    npc.AddBuff(mod.BuffType("SquireKBDebuff"), 1020);
                    squireCounter = 0;
                }
            }
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
        {
            FargoPlayer modPlayer = Main.player[projectile.owner].GetModPlayer<FargoPlayer>();

            if (modPlayer.ValhallaEnchant && SoulConfig.Instance.GetValue(SoulConfig.Instance.ValhallaEffect))
            {
                if (valhallaCounter == 0)
                {
                    squireCounter++;

                    if (squireCounter >= 100)
                    {
                        valhallaCounter = 1020;
                        valhallaPlayer = projectile.owner;
                        squireCounter = 0;
                    }
                }
                else if (valhallaCounter > 900)
                {
                    npc.immune[valhallaPlayer] = 2;
                }
            }
            else if (modPlayer.SquireEnchant && SoulConfig.Instance.GetValue(SoulConfig.Instance.SquireKB)
                 && !npc.GetGlobalNPC<FargoSoulsGlobalNPC>().SpecialEnchantImmune && npc.knockBackResist < 1 && !npc.HasBuff(mod.BuffType("SquireKBDebuff")))
            {
                squireCounter++;

                if (squireCounter >= 100)
                {
                    originalKB = npc.knockBackResist;
                    npc.knockBackResist = 1f;
                    npc.AddBuff(mod.BuffType("SquireKBDebuff"), 1020);
                    squireCounter = 0;
                }
            }
        }

        public override void OnHitNPC(NPC npc, NPC target, int damage, float knockback, bool crit)
        {
            FargoPlayer modPlayer = Main.player[Main.myPlayer].GetModPlayer<FargoPlayer>();

            if (!modPlayer.ThoriumSoul && modPlayer.KnightEnchant && !npc.friendly && target.townNPC)
            {
                Villain = true;
            }
        }

        public override bool? CanBeHitByItem(NPC npc, Player player, Item item)
        {
            if (isMasoML && masoStateML > 0 && masoStateML < 4 && !player.buffImmune[mod.BuffType("NullificationCurse")])
                return false;

            return null;
        }

        public override bool? CanBeHitByProjectile(NPC npc, Projectile projectile)
        {
            if (isMasoML && !Main.player[projectile.owner].buffImmune[mod.BuffType("NullificationCurse")])
            {
                switch (masoStateML)
                {
                    case 0: if (!projectile.melee) return false; break;
                    case 1: if (!projectile.ranged) return false; break;
                    case 2: if (!projectile.magic) return false; break;
                    case 3: if (!projectile.minion) return false; break;
                    default: break;
                }
            }

            return null;
        }

        public static bool BossIsAlive(ref int bossID, int bossType)
        {
            if (bossID != -1)
            {
                if (Main.npc[bossID].active && Main.npc[bossID].type == bossType)
                {
                    return true;
                }
                else
                {
                    bossID = -1;
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool AnyBossAlive()
        {
            if (boss == -1)
                return false;
            if (Main.npc[boss].active && (Main.npc[boss].boss || Main.npc[boss].type == NPCID.EaterofWorldsHead))
                return true;
            boss = -1;
            return false;
        }

        public static void PrintAI(NPC npc)
        {
            Main.NewText("ai: " + npc.ai[0].ToString() + " " + npc.ai[1].ToString() + " " + npc.ai[2].ToString() + " " + npc.ai[3].ToString()
                + ", local: " + npc.localAI[0].ToString() + " " + npc.localAI[1].ToString() + " " + npc.localAI[2].ToString() + " " + npc.localAI[3].ToString());
        }

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == NPCID.Steampunker)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType("RoombaPet"));
                shop.item[nextSlot].value = 50000;
                nextSlot++;
            }

            //for eater rocket
            if (type == NPCID.ArmsDealer && !NPC.downedPlantBoss)
            {
                shop.item[nextSlot].SetDefaults(ItemID.RocketI);
                shop.item[nextSlot].value = 500;
                nextSlot++;
            }
        }

        public override bool PreChatButtonClicked(NPC npc, bool firstButton)
        {
            if (FargoSoulsWorld.MasochistMode && npc.type == NPCID.Nurse && firstButton)
            {
                if (Main.player[Main.myPlayer].HasBuff(mod.BuffType("Recovering")))
                    return false;
                else if (AnyBossAlive())
                    Main.player[Main.myPlayer].AddBuff(mod.BuffType("Recovering"), 3600);
            }
            return true;
        }

        private void LoadSprites(NPC npc)
        {
            if (Main.dedServ || Main.netMode == 2)
                return;

            bool recolor = SoulConfig.Instance.BossRecolors && FargoSoulsWorld.MasochistMode;
            if (recolor || Fargowiltas.Instance.LoadedNewSprites)
            {
                Fargowiltas.Instance.LoadedNewSprites = true;
                switch (npc.type)
                {
                    case NPCID.ServantofCthulhu:
                    case NPCID.EaterofWorldsBody:
                    case NPCID.EaterofWorldsTail:
                    case NPCID.Creeper:
                    case NPCID.SkeletronHand:
                    case NPCID.WallofFleshEye:
                    case NPCID.TheHungry:
                    case NPCID.TheHungryII:
                    case NPCID.LeechHead:
                    case NPCID.LeechBody:
                    case NPCID.LeechTail:
                    case NPCID.TheDestroyerBody:
                    case NPCID.TheDestroyerTail:
                    case NPCID.Probe:
                    case NPCID.PrimeCannon:
                    case NPCID.PrimeSaw:
                    case NPCID.PrimeVice:
                    case NPCID.PrimeLaser:
                    case NPCID.PlanterasHook:
                    case NPCID.PlanterasTentacle:
                    case NPCID.Spore:
                    case NPCID.GolemFistLeft:
                    case NPCID.GolemFistRight:
                    case NPCID.GolemHead:
                    case NPCID.GolemHeadFree:
                    case NPCID.Sharkron:
                    case NPCID.Sharkron2:
                    case NPCID.CultistBossClone:
                    case NPCID.MoonLordFreeEye:
                    case NPCID.MoonLordHand:
                    case NPCID.MoonLordHead:
                    case NPCID.MoonLordLeechBlob:
                        Main.npcTexture[npc.type] = mod.GetTexture((recolor ? "NPCs/Resprites/" : "NPCs/Vanilla/") + "NPC_" + npc.type.ToString());
                        Main.NPCLoaded[npc.type] = true;
                        break;

                    case NPCID.KingSlime:
                        Main.npcTexture[npc.type] = mod.GetTexture((recolor ? "NPCs/Resprites/" : "NPCs/Vanilla/") + "NPC_" + npc.type.ToString());
                        Main.NPCLoaded[npc.type] = true;
                        Main.extraTexture[39] = mod.GetTexture(recolor ? "NPCs/Resprites/Extra_39" : "NPCs/Vanilla/Extra_39");
                        Main.ninjaTexture = mod.GetTexture(recolor ? "NPCs/Resprites/Ninja" : "NPCs/Vanilla/Ninja");
                        Main.npcHeadBossTexture[7] = mod.GetTexture((recolor ? "NPCs/Resprites/" : "NPCs/Vanilla/") + "NPC_Head_Boss_7");
                        Main.goreTexture[734] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_734");
                        Main.goreLoaded[734] = true;
                        break;

                    case NPCID.EyeofCthulhu:
                        Main.npcTexture[npc.type] = mod.GetTexture((recolor ? "NPCs/Resprites/" : "NPCs/Vanilla/") + "NPC_" + npc.type.ToString());
                        Main.NPCLoaded[npc.type] = true;
                        Main.npcHeadBossTexture[0] = mod.GetTexture((recolor ? "NPCs/Resprites/" : "NPCs/Vanilla/") + "NPC_Head_Boss_0");
                        Main.npcHeadBossTexture[1] = mod.GetTexture((recolor ? "NPCs/Resprites/" : "NPCs/Vanilla/") + "NPC_Head_Boss_1");
                        Main.goreTexture[6] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_6");
                        Main.goreTexture[7] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_7");
                        Main.goreTexture[8] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_8");
                        Main.goreTexture[9] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_9");
                        Main.goreTexture[10] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_10");
                        Main.goreLoaded[6] = true;
                        Main.goreLoaded[7] = true;
                        Main.goreLoaded[8] = true;
                        Main.goreLoaded[9] = true;
                        Main.goreLoaded[10] = true;
                        break;

                    case NPCID.EaterofWorldsHead:
                        Main.npcTexture[npc.type] = mod.GetTexture((recolor ? "NPCs/Resprites/" : "NPCs/Vanilla/") + "NPC_" + npc.type.ToString());
                        Main.NPCLoaded[npc.type] = true;
                        Main.npcHeadBossTexture[2] = mod.GetTexture((recolor ? "NPCs/Resprites/" : "NPCs/Vanilla/") + "NPC_Head_Boss_2");
                        //Main.goreTexture[14] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_14");
                        //Main.goreTexture[15] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_15");
                        Main.goreTexture[24] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_24");
                        Main.goreTexture[25] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_25");
                        Main.goreTexture[26] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_26");
                        Main.goreTexture[27] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_27");
                        Main.goreTexture[28] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_28");
                        Main.goreTexture[29] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_29");
                        //Main.goreLoaded[14] = true;
                        //Main.goreLoaded[15] = true;
                        Main.goreLoaded[24] = true;
                        Main.goreLoaded[25] = true;
                        Main.goreLoaded[26] = true;
                        Main.goreLoaded[27] = true;
                        Main.goreLoaded[28] = true;
                        Main.goreLoaded[29] = true;
                        break;

                    case NPCID.BrainofCthulhu:
                        Main.npcTexture[npc.type] = mod.GetTexture((recolor ? "NPCs/Resprites/" : "NPCs/Vanilla/") + "NPC_" + npc.type.ToString());
                        Main.NPCLoaded[npc.type] = true;
                        Main.npcHeadBossTexture[23] = mod.GetTexture((recolor ? "NPCs/Resprites/" : "NPCs/Vanilla/") + "NPC_Head_Boss_23");
                        Main.goreTexture[392] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_392");
                        Main.goreTexture[393] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_393");
                        Main.goreTexture[394] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_394");
                        Main.goreTexture[395] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_395");
                        Main.goreTexture[396] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_396");
                        Main.goreTexture[397] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_397");
                        Main.goreTexture[398] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_398");
                        Main.goreTexture[399] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_399");
                        Main.goreTexture[400] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_400");
                        Main.goreTexture[401] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_401");
                        Main.goreTexture[402] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_402");
                        Main.goreLoaded[392] = true;
                        Main.goreLoaded[393] = true;
                        Main.goreLoaded[394] = true;
                        Main.goreLoaded[395] = true;
                        Main.goreLoaded[396] = true;
                        Main.goreLoaded[397] = true;
                        Main.goreLoaded[398] = true;
                        Main.goreLoaded[399] = true;
                        Main.goreLoaded[400] = true;
                        Main.goreLoaded[401] = true;
                        Main.goreLoaded[402] = true;
                        break;

                    case NPCID.QueenBee:
                        Main.npcTexture[npc.type] = mod.GetTexture((recolor ? "NPCs/Resprites/" : "NPCs/Vanilla/") + "NPC_" + npc.type.ToString());
                        Main.NPCLoaded[npc.type] = true;
                        Main.npcHeadBossTexture[14] = mod.GetTexture((recolor ? "NPCs/Resprites/" : "NPCs/Vanilla/") + "NPC_Head_Boss_14");
                        Main.goreTexture[303] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_303");
                        Main.goreTexture[304] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_304");
                        Main.goreTexture[305] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_305");
                        Main.goreTexture[306] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_306");
                        Main.goreTexture[307] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_307");
                        Main.goreTexture[308] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_308");
                        Main.goreLoaded[303] = true;
                        Main.goreLoaded[304] = true;
                        Main.goreLoaded[305] = true;
                        Main.goreLoaded[306] = true;
                        Main.goreLoaded[307] = true;
                        Main.goreLoaded[308] = true;
                        break;

                    case NPCID.SkeletronHead:
                    case NPCID.DungeonGuardian:
                        Main.npcTexture[npc.type] = mod.GetTexture((recolor ? "NPCs/Resprites/" : "NPCs/Vanilla/") + "NPC_" + npc.type.ToString());
                        Main.NPCLoaded[npc.type] = true;
                        Main.boneArmTexture = mod.GetTexture(recolor ? "NPCs/Resprites/Arm_Bone" : "NPCs/Vanilla/Arm_Bone");
                        Main.npcHeadBossTexture[19] = mod.GetTexture((recolor ? "NPCs/Resprites/" : "NPCs/Vanilla/") + "NPC_Head_Boss_19");
                        Main.goreTexture[54] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_54");
                        Main.goreTexture[55] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_55");
                        Main.goreTexture[56] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_56");
                        Main.goreTexture[57] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_57");
                        Main.goreLoaded[54] = true;
                        Main.goreLoaded[55] = true;
                        Main.goreLoaded[56] = true;
                        Main.goreLoaded[57] = true;
                        break;

                    case NPCID.WallofFlesh:
                        Main.npcTexture[npc.type] = mod.GetTexture((recolor ? "NPCs/Resprites/" : "NPCs/Vanilla/") + "NPC_" + npc.type.ToString());
                        Main.NPCLoaded[npc.type] = true;
                        Main.chain12Texture = mod.GetTexture(recolor ? "NPCs/Resprites/Chain12" : "NPCs/Vanilla/Chain12");
                        Main.wofTexture = mod.GetTexture(recolor ? "NPCs/Resprites/WallOfFlesh" : "NPCs/Vanilla/WallOfFlesh");
                        Main.npcHeadBossTexture[22] = mod.GetTexture((recolor ? "NPCs/Resprites/" : "NPCs/Vanilla/") + "NPC_Head_Boss_22");
                        Main.goreTexture[132] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_132");
                        Main.goreTexture[133] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_133");
                        Main.goreTexture[134] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_134");
                        Main.goreTexture[135] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_135");
                        Main.goreTexture[136] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_136");
                        Main.goreTexture[137] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_137");
                        Main.goreTexture[138] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_138");
                        Main.goreTexture[139] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_139");
                        Main.goreTexture[140] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_140");
                        Main.goreTexture[141] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_141");
                        Main.goreTexture[142] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_142");
                        Main.goreLoaded[132] = true;
                        Main.goreLoaded[133] = true;
                        Main.goreLoaded[134] = true;
                        Main.goreLoaded[135] = true;
                        Main.goreLoaded[136] = true;
                        Main.goreLoaded[137] = true;
                        Main.goreLoaded[138] = true;
                        Main.goreLoaded[139] = true;
                        Main.goreLoaded[140] = true;
                        Main.goreLoaded[141] = true;
                        Main.goreLoaded[142] = true;
                        break;

                    case NPCID.TheDestroyer:
                        Main.npcTexture[npc.type] = mod.GetTexture((recolor ? "NPCs/Resprites/" : "NPCs/Vanilla/") + "NPC_" + npc.type.ToString());
                        Main.NPCLoaded[npc.type] = true;
                        Main.npcHeadBossTexture[25] = mod.GetTexture((recolor ? "NPCs/Resprites/" : "NPCs/Vanilla/") + "NPC_Head_Boss_25");
                        Main.goreTexture[156] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_156");
                        Main.goreLoaded[156] = true;
                        break;

                    case NPCID.Retinazer:
                        Main.npcTexture[npc.type] = mod.GetTexture((recolor ? "NPCs/Resprites/" : "NPCs/Vanilla/") + "NPC_" + npc.type.ToString());
                        Main.NPCLoaded[npc.type] = true;
                        Main.npcHeadBossTexture[15] = mod.GetTexture((recolor ? "NPCs/Resprites/" : "NPCs/Vanilla/") + "NPC_Head_Boss_15");
                        Main.npcHeadBossTexture[20] = mod.GetTexture((recolor ? "NPCs/Resprites/" : "NPCs/Vanilla/") + "NPC_Head_Boss_16");
                        Main.goreTexture[143] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_143");
                        Main.goreTexture[144] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_144");
                        Main.goreTexture[145] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_145");
                        Main.goreTexture[146] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_146");
                        Main.goreLoaded[142] = true;
                        Main.goreLoaded[144] = true;
                        Main.goreLoaded[145] = true;
                        Main.goreLoaded[146] = true;
                        break;

                    case NPCID.Spazmatism:
                        Main.npcTexture[npc.type] = mod.GetTexture((recolor ? "NPCs/Resprites/" : "NPCs/Vanilla/") + "NPC_" + npc.type.ToString());
                        Main.NPCLoaded[npc.type] = true;
                        Main.npcHeadBossTexture[16] = mod.GetTexture((recolor ? "NPCs/Resprites/" : "NPCs/Vanilla/") + "NPC_Head_Boss_20");
                        Main.npcHeadBossTexture[21] = mod.GetTexture((recolor ? "NPCs/Resprites/" : "NPCs/Vanilla/") + "NPC_Head_Boss_21");
                        Main.goreTexture[143] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_143");
                        Main.goreTexture[144] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_144");
                        Main.goreTexture[145] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_145");
                        Main.goreTexture[146] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_146");
                        Main.goreLoaded[142] = true;
                        Main.goreLoaded[144] = true;
                        Main.goreLoaded[145] = true;
                        Main.goreLoaded[146] = true;
                        break;

                    case NPCID.SkeletronPrime:
                        Main.npcTexture[npc.type] = mod.GetTexture((recolor ? "NPCs/Resprites/" : "NPCs/Vanilla/") + "NPC_" + npc.type.ToString());
                        Main.NPCLoaded[npc.type] = true;
                        Main.boneArm2Texture = mod.GetTexture(recolor ? "NPCs/Resprites/Arm_Bone_2" : "NPCs/Vanilla/Arm_Bone_2");
                        Main.npcHeadBossTexture[18] = mod.GetTexture((recolor ? "NPCs/Resprites/" : "NPCs/Vanilla/") + "NPC_Head_Boss_18");
                        Main.goreTexture[147] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_147");
                        Main.goreTexture[148] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_148");
                        Main.goreTexture[149] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_149");
                        Main.goreTexture[150] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_150");
                        Main.goreLoaded[147] = true;
                        Main.goreLoaded[148] = true;
                        Main.goreLoaded[149] = true;
                        Main.goreLoaded[150] = true;
                        break;

                    case NPCID.Plantera:
                        Main.npcTexture[npc.type] = mod.GetTexture((recolor ? "NPCs/Resprites/" : "NPCs/Vanilla/") + "NPC_" + npc.type.ToString());
                        Main.NPCLoaded[npc.type] = true;
                        Main.chain26Texture = mod.GetTexture(recolor ? "NPCs/Resprites/Chain26" : "NPCs/Vanilla/Chain27");
                        Main.chain27Texture = mod.GetTexture(recolor ? "NPCs/Resprites/Chain26" : "NPCs/Vanilla/Chain27");
                        Main.npcHeadBossTexture[11] = mod.GetTexture((recolor ? "NPCs/Resprites/" : "NPCs/Vanilla/") + "NPC_Head_Boss_11");
                        Main.npcHeadBossTexture[12] = mod.GetTexture((recolor ? "NPCs/Resprites/" : "NPCs/Vanilla/") + "NPC_Head_Boss_12");
                        Main.goreTexture[378] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_378");
                        Main.goreTexture[379] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_379");
                        Main.goreTexture[380] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_380");
                        Main.goreTexture[381] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_381");
                        Main.goreTexture[382] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_382");
                        Main.goreTexture[383] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_383");
                        Main.goreTexture[384] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_384");
                        Main.goreTexture[385] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_385");
                        Main.goreTexture[386] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_386");
                        Main.goreTexture[387] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_387");
                        Main.goreTexture[388] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_388");
                        Main.goreTexture[389] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_389");
                        Main.goreTexture[390] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_390");
                        Main.goreTexture[391] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_391");
                        Main.goreLoaded[378] = true;
                        Main.goreLoaded[379] = true;
                        Main.goreLoaded[380] = true;
                        Main.goreLoaded[381] = true;
                        Main.goreLoaded[382] = true;
                        Main.goreLoaded[383] = true;
                        Main.goreLoaded[384] = true;
                        Main.goreLoaded[385] = true;
                        Main.goreLoaded[386] = true;
                        Main.goreLoaded[387] = true;
                        Main.goreLoaded[388] = true;
                        Main.goreLoaded[389] = true;
                        Main.goreLoaded[390] = true;
                        Main.goreLoaded[391] = true;
                        break;

                    case NPCID.Golem:
                        Main.npcTexture[npc.type] = mod.GetTexture((recolor ? "NPCs/Resprites/" : "NPCs/Vanilla/") + "NPC_" + npc.type.ToString());
                        Main.NPCLoaded[npc.type] = true;
                        Main.golemTexture[1] = mod.GetTexture(recolor ? "NPCs/Resprites/GolemLights1" : "NPCs/Vanilla/GolemLights1");
                        Main.golemTexture[2] = mod.GetTexture(recolor ? "NPCs/Resprites/GolemLights2" : "NPCs/Vanilla/GolemLights2");
                        Main.golemTexture[3] = mod.GetTexture(recolor ? "NPCs/Resprites/GolemLights3" : "NPCs/Vanilla/GolemLights3");

                        //enabling this crashes on reload
                        //Main.npcHeadBossTexture[5] = mod.GetTexture((recolor ? "NPCs/Resprites/" : "NPCs/Vanilla/") + "NPC_Head_Boss_5");

                        //disabled because this changes appearance of landslide projectiles too
                        /*Main.goreTexture[360] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_360");
                        Main.goreTexture[361] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_361");
                        Main.goreTexture[362] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_362");
                        Main.goreTexture[363] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_363");
                        Main.goreTexture[364] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_364");
                        Main.goreTexture[365] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_365");
                        Main.goreTexture[366] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_366");
                        Main.goreTexture[367] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_367");
                        Main.goreTexture[368] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_368");
                        Main.goreTexture[369] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_369");
                        Main.goreTexture[370] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_370");
                        Main.goreLoaded[360] = true;
                        Main.goreLoaded[361] = true;
                        Main.goreLoaded[362] = true;
                        Main.goreLoaded[363] = true;
                        Main.goreLoaded[364] = true;
                        Main.goreLoaded[365] = true;
                        Main.goreLoaded[366] = true;
                        Main.goreLoaded[367] = true;
                        Main.goreLoaded[368] = true;
                        Main.goreLoaded[369] = true;
                        Main.goreLoaded[370] = true;*/
                        break;

                    case NPCID.DukeFishron:
                        Main.npcTexture[npc.type] = mod.GetTexture((recolor ? "NPCs/Resprites/" : "NPCs/Vanilla/") + "NPC_" + npc.type.ToString());
                        Main.NPCLoaded[npc.type] = true;
                        Main.npcHeadBossTexture[4] = mod.GetTexture((recolor ? "NPCs/Resprites/" : "NPCs/Vanilla/") + "NPC_Head_Boss_4");
                        Main.goreTexture[573] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_573");
                        Main.goreTexture[574] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_574");
                        Main.goreTexture[575] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_575");
                        Main.goreTexture[576] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_576");
                        Main.goreTexture[577] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_577");
                        Main.goreTexture[578] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_578");
                        Main.goreTexture[579] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_579");
                        Main.goreLoaded[573] = true;
                        Main.goreLoaded[574] = true;
                        Main.goreLoaded[575] = true;
                        Main.goreLoaded[576] = true;
                        Main.goreLoaded[577] = true;
                        Main.goreLoaded[578] = true;
                        Main.goreLoaded[579] = true;
                        break;

                    case NPCID.CultistBoss:
                        Main.npcTexture[npc.type] = mod.GetTexture((recolor ? "NPCs/Resprites/" : "NPCs/Vanilla/") + "NPC_" + npc.type.ToString());
                        Main.NPCLoaded[npc.type] = true;
                        Main.npcHeadBossTexture[24] = mod.GetTexture((recolor ? "NPCs/Resprites/" : "NPCs/Vanilla/") + "NPC_Head_Boss_24");
                        Main.npcHeadBossTexture[31] = mod.GetTexture((recolor ? "NPCs/Resprites/" : "NPCs/Vanilla/") + "NPC_Head_Boss_31");
                        Main.goreTexture[902] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_902");
                        Main.goreTexture[903] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_903");
                        Main.goreLoaded[902] = true;
                        Main.goreLoaded[903] = true;
                        break;

                    case NPCID.MoonLordCore:
                        Main.npcTexture[npc.type] = mod.GetTexture((recolor ? "NPCs/Resprites/" : "NPCs/Vanilla/") + "NPC_" + npc.type.ToString());
                        Main.NPCLoaded[npc.type] = true;
                        Main.extraTexture[13] = mod.GetTexture(recolor ? "NPCs/Resprites/Extra_13" : "NPCs/Vanilla/Extra_13");
                        Main.extraTexture[14] = mod.GetTexture(recolor ? "NPCs/Resprites/Extra_14" : "NPCs/Vanilla/Extra_14");
                        Main.extraTexture[15] = mod.GetTexture(recolor ? "NPCs/Resprites/Extra_15" : "NPCs/Vanilla/Extra_15");
                        Main.extraTexture[16] = mod.GetTexture(recolor ? "NPCs/Resprites/Extra_16" : "NPCs/Vanilla/Extra_16");
                        Main.extraTexture[17] = mod.GetTexture(recolor ? "NPCs/Resprites/Extra_17" : "NPCs/Vanilla/Extra_17");
                        Main.extraTexture[18] = mod.GetTexture(recolor ? "NPCs/Resprites/Extra_18" : "NPCs/Vanilla/Extra_18");
                        Main.extraTexture[19] = mod.GetTexture(recolor ? "NPCs/Resprites/Extra_19" : "NPCs/Vanilla/Extra_19");
                        Main.extraTexture[21] = mod.GetTexture(recolor ? "NPCs/Resprites/Extra_21" : "NPCs/Vanilla/Extra_21");
                        Main.extraTexture[22] = mod.GetTexture(recolor ? "NPCs/Resprites/Extra_22" : "NPCs/Vanilla/Extra_22");
                        Main.extraTexture[23] = mod.GetTexture(recolor ? "NPCs/Resprites/Extra_23" : "NPCs/Vanilla/Extra_23");
                        Main.extraTexture[24] = mod.GetTexture(recolor ? "NPCs/Resprites/Extra_24" : "NPCs/Vanilla/Extra_24");
                        Main.extraTexture[25] = mod.GetTexture(recolor ? "NPCs/Resprites/Extra_25" : "NPCs/Vanilla/Extra_25");
                        Main.extraTexture[26] = mod.GetTexture(recolor ? "NPCs/Resprites/Extra_26" : "NPCs/Vanilla/Extra_26");
                        Main.extraTexture[29] = mod.GetTexture(recolor ? "NPCs/Resprites/Extra_29" : "NPCs/Vanilla/Extra_29");
                        Main.npcHeadBossTexture[8] = mod.GetTexture((recolor ? "NPCs/Resprites/" : "NPCs/Vanilla/") + "NPC_Head_Boss_8");
                        /*Main.goreTexture[619] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_619");
                        Main.goreTexture[620] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_620");
                        Main.goreTexture[621] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_621");
                        Main.goreTexture[622] = mod.GetTexture((recolor ? "NPCs/Resprites/Gores/" : "NPCs/Vanilla/Gores/") + "Gore_622");
                        Main.goreLoaded[619] = true;
                        Main.goreLoaded[620] = true;
                        Main.goreLoaded[621] = true;
                        Main.goreLoaded[622] = true;*/
                        break;

                    default:
                        break;
                }
            }
        }

        public void AddBuffNoStack(Player player, int buff, int duration = 60)
        {
            if (!player.HasBuff(buff))
            {
                applyDebuff = !applyDebuff;
                if (applyDebuff)
                    player.AddBuff(buff, duration);
            }
        }
    }
}
