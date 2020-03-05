using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace FargowiltasSouls
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class FargoSoulsWorld : ModWorld
    {
        public static bool downedBetsy;
        private static bool _downedBoss;

        //masomode
        public const int MaxCountPreHM = 560;
        public const int MaxCountHM = 240;

        public static bool MasochistMode;
        public static bool downedFishronEX;
        public static bool downedAbom;
        public static bool downedMutant;
        public static bool AngryMutant;
        public static int EyeCount;
        public static int SlimeCount;
        public static int EaterCount;
        public static int BrainCount;
        public static int BeeCount;
        public static int SkeletronCount;
        public static int WallCount;
        public static int DestroyerCount;
        public static int PrimeCount;
        public static int TwinsCount;
        public static int PlanteraCount;
        public static int GolemCount;
        public static int FishronCount;
        public static int CultistCount;
        public static int MoonlordCount;

        public static bool downedMM;
        public static bool forceMeteor;
        public static int skipMutantP1;

        public static bool NoMasoBossScaling;

        public override void Initialize()
        {
            downedBetsy = false;
            _downedBoss = false;

            downedMM = false;

            //masomode
            MasochistMode = false;
            downedFishronEX = false;
            downedAbom = false;
            downedMutant = false;
            AngryMutant = false;
            EyeCount = 0;
            SlimeCount = 0;
            EaterCount = 0;
            BrainCount = 0;
            BeeCount = 0;
            SkeletronCount = 0;
            WallCount = 0;
            DestroyerCount = 0;
            PrimeCount = 0;
            TwinsCount = 0;
            PlanteraCount = 0;
            GolemCount = 0;
            FishronCount = 0;
            CultistCount = 0;
            MoonlordCount = 0;

            forceMeteor = true;
            skipMutantP1 = 0;

            NoMasoBossScaling = false;
        }

        public override TagCompound Save()
        {
            List<int> count = new List<int>
            {
                EyeCount,
                SlimeCount,
                EaterCount,
                BrainCount,
                BeeCount,
                SkeletronCount,
                WallCount,
                DestroyerCount,
                PrimeCount,
                TwinsCount,
                PlanteraCount,
                GolemCount,
                FishronCount,
                CultistCount,
                MoonlordCount
            };

            List<string> downed = new List<string>();
            if (downedBetsy) downed.Add("betsy");
            if (_downedBoss) downed.Add("boss");
            if (MasochistMode) downed.Add("masochist");
            if (downedFishronEX) downed.Add("downedFishronEX");
            if (downedAbom) downed.Add("downedAbom");
            if (downedMutant) downed.Add("downedMutant");
            if (AngryMutant) downed.Add("AngryMutant");
            if (downedMM) downed.Add("downedMadhouse");
            if (forceMeteor) downed.Add("forceMeteor");
            if (NoMasoBossScaling) downed.Add("NoMasoBossScaling");

            return new TagCompound
            {
                {"downed", downed}, {"count", count}, {"mutantP1", skipMutantP1}
            };
        }

        public override void Load(TagCompound tag)
        {
            if (tag.ContainsKey("count"))
            {
                IList<int> count = tag.GetList<int>("count");
                EyeCount = count[0];
                SlimeCount = count[1];
                EaterCount = count[2];
                BrainCount = count[3];
                BeeCount = count[4];
                SkeletronCount = count[5];
                WallCount = count[6];
                DestroyerCount = count[7];
                PrimeCount = count[8];
                TwinsCount = count[9];
                PlanteraCount = count[10];
                GolemCount = count[11];
                FishronCount = count[12];
                CultistCount = count[13];
                MoonlordCount = count[14];
            }

            IList<string> downed = tag.GetList<string>("downed");
            downedBetsy = downed.Contains("betsy");
            _downedBoss = downed.Contains("boss");
            MasochistMode = downed.Contains("masochist");
            downedFishronEX = downed.Contains("downedFishronEX");
            downedAbom = downed.Contains("downedAbom");
            downedMutant = downed.Contains("downedMutant");
            AngryMutant = downed.Contains("AngryMutant");
            downedMM = downed.Contains("downedMadhouse");
            forceMeteor = downed.Contains("forceMeteor");
            NoMasoBossScaling = downed.Contains("NoMasoBossScaling");

            if (tag.ContainsKey("mutantP1"))
                skipMutantP1 = tag.GetAsInt("mutantP1");
        }

        public override void NetReceive(BinaryReader reader)
        {
            EyeCount = reader.ReadInt32();
            SlimeCount = reader.ReadInt32();
            EaterCount = reader.ReadInt32();
            BrainCount = reader.ReadInt32();
            BeeCount = reader.ReadInt32();
            SkeletronCount = reader.ReadInt32();
            WallCount = reader.ReadInt32();
            DestroyerCount = reader.ReadInt32();
            PrimeCount = reader.ReadInt32();
            TwinsCount = reader.ReadInt32();
            PlanteraCount = reader.ReadInt32();
            GolemCount = reader.ReadInt32();
            FishronCount = reader.ReadInt32();
            CultistCount = reader.ReadInt32();
            MoonlordCount = reader.ReadInt32();
            skipMutantP1 = reader.ReadInt32();

            BitsByte flags = reader.ReadByte();
            downedBetsy = flags[0];
            _downedBoss = flags[1];
            MasochistMode = flags[2];
            downedFishronEX = flags[3];
            downedAbom = flags[4];
            downedMutant = flags[5];
            AngryMutant = flags[6];
            downedMM = flags[7];
            forceMeteor = flags[8];
            NoMasoBossScaling = flags[9];
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(EyeCount);
            writer.Write(SlimeCount);
            writer.Write(EaterCount);
            writer.Write(BrainCount);
            writer.Write(BeeCount);
            writer.Write(SkeletronCount);
            writer.Write(WallCount);
            writer.Write(DestroyerCount);
            writer.Write(PrimeCount);
            writer.Write(TwinsCount);
            writer.Write(PlanteraCount);
            writer.Write(GolemCount);
            writer.Write(FishronCount);
            writer.Write(CultistCount);
            writer.Write(MoonlordCount);
            writer.Write(skipMutantP1);

            BitsByte flags = new BitsByte
            {
                [0] = downedBetsy,
                [1] = _downedBoss,
                [2] = MasochistMode,
                [3] = downedFishronEX,
                [4] = downedAbom,
                [5] = downedMutant,
                [6] = AngryMutant,
                [7] = downedMM,
                [8] = forceMeteor,
                [9] = NoMasoBossScaling
            };

            writer.Write(flags);
        }

        public override void PostUpdate()
        {
            //Main.NewText(BuilderMode);

            #region commented

            //right when day starts
            /*if(/*Main.time == 0 && Main.dayTime && !Main.eclipse && FargoSoulsWorld.masochistMode)
			{
					Main.PlaySound(15, (int)player.position.X, (int)player.position.Y, 0, 1f, 0f);
					
					if (Main.netMode == 0)
					{
						Main.eclipse = true;
						//Main.NewText(Lang.misc[20], 50, 255, 130, false);
					}
					else
					{
						//NetMessage.SendData(61, -1, -1, "", player.whoAmI, -6f, 0f, 0f, 0, 0, 0);
					}
				
				
			}*/

            // if (this.itemTime == 0 && this.itemAnimation > 0 && item.type == 361 && Main.CanStartInvasion(1, true))
            // {
            // this.itemTime = item.useTime;
            // Main.PlaySound(15, (int)this.position.X, (int)this.position.Y, 0, 1f, 0f);
            // if (Main.netMode != 1)
            // {
            // if (Main.invasionType == 0)
            // {
            // Main.invasionDelay = 0;
            // Main.StartInvasion(1);
            // }
            // }
            // else
            // {
            // NetMessage.SendData(61, -1, -1, "", this.whoAmI, -1f, 0f, 0f, 0, 0, 0);
            // }
            // }
            // if (this.itemTime == 0 && this.itemAnimation > 0 && item.type == 602 && Main.CanStartInvasion(2, true))
            // {
            // this.itemTime = item.useTime;
            // Main.PlaySound(15, (int)this.position.X, (int)this.position.Y, 0, 1f, 0f);
            // if (Main.netMode != 1)
            // {
            // if (Main.invasionType == 0)
            // {
            // Main.invasionDelay = 0;
            // Main.StartInvasion(2);
            // }
            // }
            // else
            // {
            // NetMessage.SendData(61, -1, -1, "", this.whoAmI, -2f, 0f, 0f, 0, 0, 0);
            // }
            // }
            // if (this.itemTime == 0 && this.itemAnimation > 0 && item.type == 1315 && Main.CanStartInvasion(3, true))
            // {
            // this.itemTime = item.useTime;
            // Main.PlaySound(15, (int)this.position.X, (int)this.position.Y, 0, 1f, 0f);
            // if (Main.netMode != 1)
            // {
            // if (Main.invasionType == 0)
            // {
            // Main.invasionDelay = 0;
            // Main.StartInvasion(3);
            // }
            // }
            // else
            // {
            // NetMessage.SendData(61, -1, -1, "", this.whoAmI, -3f, 0f, 0f, 0, 0, 0);
            // }
            // }
            // if (this.itemTime == 0 && this.itemAnimation > 0 && item.type == 1844 && !Main.dayTime && !Main.pumpkinMoon && !Main.snowMoon && !DD2Event.Ongoing)
            // {
            // this.itemTime = item.useTime;
            // Main.PlaySound(15, (int)this.position.X, (int)this.position.Y, 0, 1f, 0f);
            // if (Main.netMode != 1)
            // {
            // Main.NewText(Lang.misc[31], 50, 255, 130, false);
            // Main.startPumpkinMoon();
            // }
            // else
            // {
            // NetMessage.SendData(61, -1, -1, "", this.whoAmI, -4f, 0f, 0f, 0, 0, 0);
            // }
            // }

            // if (this.itemTime == 0 && this.itemAnimation > 0 && item.type == 3601 && NPC.downedGolemBoss && Main.hardMode && !NPC.AnyDanger() && !NPC.AnyoneNearCultists())
            // {
            // Main.PlaySound(15, (int)this.position.X, (int)this.position.Y, 0, 1f, 0f);
            // this.itemTime = item.useTime;
            // if (Main.netMode == 0)
            // {
            // WorldGen.StartImpendingDoom();
            // }
            // else
            // {
            // NetMessage.SendData(61, -1, -1, "", this.whoAmI, -8f, 0f, 0f, 0, 0, 0);
            // }
            // }
            // if (this.itemTime == 0 && this.itemAnimation > 0 && item.type == 1958 && !Main.dayTime && !Main.pumpkinMoon && !Main.snowMoon && !DD2Event.Ongoing)
            // {
            // this.itemTime = item.useTime;
            // Main.PlaySound(15, (int)this.position.X, (int)this.position.Y, 0, 1f, 0f);
            // if (Main.netMode != 1)
            // {
            // Main.NewText(Lang.misc[34], 50, 255, 130, false);
            // Main.startSnowMoon();
            // }
            // else
            // {
            // NetMessage.SendData(61, -1, -1, "", this.whoAmI, -5f, 0f, 0f, 0, 0, 0);
            // }
            // }

            #endregion
        }

        public override void PostWorldGen()
        {
            /*WorldGen.PlaceTile(Main.spawnTileX - 1, Main.spawnTileY, TileID.GrayBrick, false, true);
            WorldGen.PlaceTile(Main.spawnTileX, Main.spawnTileY, TileID.GrayBrick, false, true);
            WorldGen.PlaceTile(Main.spawnTileX + 1, Main.spawnTileY, TileID.GrayBrick, false, true);
            Main.tile[Main.spawnTileX - 1, Main.spawnTileY].slope(0);
            Main.tile[Main.spawnTileX, Main.spawnTileY].slope(0);
            Main.tile[Main.spawnTileX + 1, Main.spawnTileY].slope(0);
            WorldGen.PlaceTile(Main.spawnTileX, Main.spawnTileY - 1, ModLoader.GetMod("Fargowiltas").TileType("RegalStatueSheet"), false, true);*/

            int positionX = Main.spawnTileX - 1; //offset by dimensions of statue
            int positionY = Main.spawnTileY - 4;
            bool placed = false;
            List<int> legalBlocks = new List<int> { TileID.Stone, TileID.Grass, TileID.Dirt, TileID.SnowBlock, TileID.IceBlock, TileID.ClayBlock };
            for (int offsetX = -50; offsetX <= 50; offsetX++)
            {
                for (int offsetY = -30; offsetY <= 10; offsetY++)
                {
                    int baseCheckX = positionX + offsetX;
                    int baseCheckY = positionY + offsetY;

                    bool canPlaceStatueHere = true;
                    for (int i = 0; i < 3; i++) //check no obstructing blocks
                        for (int j = 0; j < 4; j++)
                        {
                            Tile tile = Framing.GetTileSafely(baseCheckX + i, baseCheckY + j);
                            if (WorldGen.SolidOrSlopedTile(tile))
                            {
                                canPlaceStatueHere = false;
                                break;
                            }
                        }
                    for (int i = 0; i < 3; i++) //check for solid foundation
                    {
                        Tile tile = Framing.GetTileSafely(baseCheckX + i, baseCheckY + 4);
                        if (!WorldGen.SolidTile(tile) || !legalBlocks.Contains(tile.type))
                        {
                            canPlaceStatueHere = false;
                            break;
                        }
                    }

                    if (canPlaceStatueHere)
                    {
                        for (int i = 0; i < 3; i++) //MAKE SURE nothing in the way
                            for (int j = 0; j < 4; j++)
                                WorldGen.KillTile(baseCheckX + i, baseCheckY + j);

                        WorldGen.PlaceTile(baseCheckX, baseCheckY + 4, TileID.GrayBrick, false, true);
                        WorldGen.PlaceTile(baseCheckX + 1, baseCheckY + 4, TileID.GrayBrick, false, true);
                        WorldGen.PlaceTile(baseCheckX + 2, baseCheckY + 4, TileID.GrayBrick, false, true);
                        Main.tile[baseCheckX, baseCheckY + 4].slope(0);
                        Main.tile[baseCheckX + 1, baseCheckY + 4].slope(0);
                        Main.tile[baseCheckX + 2, baseCheckY + 4].slope(0);
                        WorldGen.PlaceTile(baseCheckX + 1, baseCheckY + 3, mod.TileType("MutantStatueGift"), false, true);

                        placed = true;
                        break;
                    }
                }
                if (placed)
                    break;
            }
        }
    }
}
