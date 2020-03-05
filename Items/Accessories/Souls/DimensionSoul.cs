using CalamityMod.CalPlayer;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items.Misc;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Souls
{
    [AutoloadEquip(EquipType.Wings)]
    public class DimensionSoul : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");
        private readonly Mod calamity = ModLoader.GetMod("CalamityMod");
        //air walker meme'
        public bool jumped;
        public bool canHover;
        public int hoverTimer;
        public int jumpTimer;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soul of Dimensions");
            DisplayName.AddTranslation(GameCulture.Chinese, "维度之魂");
            String tooltip = 
@"'The dimensions of Terraria at your fingertips'
Increases HP by 300
20% damage reduction
Increases life regeneration by 8
Grants immunity to knockback and several debuffs
Enemies are more likely to target you
Allows Supersonic running and infinite flight
Increases fishing skill substantially, All fishing rods will have 10 extra lures
Increased block and wall placement speed by 50% 
Near infinite block placement and mining reach, Mining speed quadrupled 
Shine, Spelunker, Hunter, and Dangersense effects
Auto paint and actuator effect
Grants the ability to enable Builder Mode
Effects of the Brain of Confusion, Star Veil, Sweetheart Necklace, Bee Cloak, and Spore Sac
Effects of Paladin's Shield, Frozen Turtle Shell, Arctic Diving Gear, Frog Legs, and Flying Carpet
Effects of Lava Waders, Angler Tackle Bag, Paint Sprayer, Presserator, Cell Phone, and Gravity Globe";

            String tooltip_ch =
@"'泰拉瑞亚维度触手可及'
增加300最大生命值
增加20%伤害减免
+8生命回复
免疫击退和诸多Debuff
敌人更有可能以你为目标
允许超音速奔跑和无限飞行
大幅提升钓鱼技能,所有鱼竿额外增加10个鱼饵
增加50%放置物块及墙壁的速度
近乎无限的放置和采掘距离, 四倍采掘速度
获得发光, 探索者, 猎人和危险感知效果
获得开启建造模式的能力
拥有混乱之脑, 星辰项链, 甜心项链, 蜜蜂斗篷和孢子囊的效果
拥有圣骑士护盾, 冰霜龟壳, 北极潜水装备, 蛙腿和飞毯的效果
拥有熔岩行走靴, 渔具包, 油漆喷雾器, 促动安装器, 手机和重力球的效果";

            if (thorium != null)
            {
                tooltip += @"
                Effects of Ocean's Retaliation and Cape of the Survivor
                Effects of Blast Shield and Terrarium Defender
                Effects of Air Walkers, Survivalist Boots, and Weighted Winglets";
                
                tooltip_ch += @"
                拥有海潮之噬和生存者披风的效果
                拥有爆炸盾和界元之庇护的效果
                拥有履空靴, 我命至上主义者之飞靴和举足轻重靴的效果";

            }

            if (calamity != null)
            {
                tooltip += "\nEffects of Asgardian Aegis";
                tooltip_ch += "\n拥有阿斯加德之庇护的效果";
            }

            Tooltip.SetDefault(tooltip);
            Tooltip.AddTranslation(GameCulture.Chinese, tooltip_ch);

            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 18));
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.accessory = true;
            item.defense = 15;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.value = 5000000;
            item.rare = -12;
            item.expert = true;
        }

        public override void UpdateInventory(Player player)
        {
            //cell phone
            player.accWatch = 3;
            player.accDepthMeter = 1;
            player.accCompass = 1;
            player.accFishFinder = true;
            player.accDreamCatcher = true;
            player.accOreFinder = true;
            player.accStopwatch = true;
            player.accCritterGuide = true;
            player.accJarOfSouls = true;
            player.accThirdEye = true;
            player.accCalendar = true;
            player.accWeatherRadio = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();

            //COLOSSUS
            player.statLifeMax2 += 300;
            player.endurance += 0.2f;
            player.lifeRegen += 8;
            //hand warmer, pocket mirror, ankh shield
            player.buffImmune[BuffID.Chilled] = true;
            player.buffImmune[BuffID.Frozen] = true;
            player.buffImmune[BuffID.Stoned] = true;
            player.buffImmune[BuffID.Weak] = true;
            player.buffImmune[BuffID.BrokenArmor] = true;
            player.buffImmune[BuffID.Bleeding] = true;
            player.buffImmune[BuffID.Poisoned] = true;
            player.buffImmune[BuffID.Slow] = true;
            player.buffImmune[BuffID.Confused] = true;
            player.buffImmune[BuffID.Silenced] = true;
            player.buffImmune[BuffID.Cursed] = true;
            player.buffImmune[BuffID.Darkness] = true;
            player.noKnockback = true;
            player.fireWalk = true;
            //brain of confusion
            player.brainOfConfusion = true;
            //charm of myths
            player.pStone = true;
            //bee cloak, sweet heart necklace, star veil
            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.StarCloak))
            {
                player.starCloak = true;
            }
            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.BeesOnHit))
            {
                player.bee = true;
            }
            player.panic = true;
            player.longInvince = true;
            //spore sac
            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.SporeSac))
            {
                player.SporeSac();
                player.sporeSac = true;
            }
            //flesh knuckles
            player.aggro += 400;
            //frozen turtle shell
            if (player.statLife <= player.statLifeMax2 * 0.5) player.AddBuff(BuffID.IceBarrier, 5, true);
            //paladins shield
            if (player.statLife > player.statLifeMax2 * .25)
            {
                player.hasPaladinShield = true;
                for (int k = 0; k < 255; k++)
                {
                    Player target = Main.player[k];

                    if (target.active && player != target && Vector2.Distance(target.Center, player.Center) < 400) target.AddBuff(BuffID.PaladinsShield, 30);
                }
            }

            //SUPERSONIC
            //frost spark plus super speed
            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.SupersonicSpeed) && !player.GetModPlayer<FargoPlayer>().noSupersonic)
            {
                player.maxRunSpeed += 15f;
                player.runAcceleration += .25f;
                player.autoJump = true;
                player.jumpSpeedBoost += 2.4f;
                player.jumpBoost = true;
                player.maxFallSpeed += 5f;
            }
            /*else
            {
                player.maxRunSpeed += 5f;
                player.runAcceleration += .1f;
            }*/

            player.moveSpeed += 0.5f;
            player.accRunSpeed = 12f;
            player.rocketBoots = 3;
            player.iceSkate = true;
            //arctic diving gear
            player.arcticDivingGear = true;
            player.accFlipper = true;
            player.accDivingHelm = true;
            //lava waders
            player.waterWalk = true;
            player.fireWalk = true;
            player.lavaImmune = true;
            //magic carpet
            player.carpet = true;
            //frog legs
            player.autoJump = true;
            player.jumpSpeedBoost += 2.4f;
            player.jumpBoost = true;
            //bundle
            if (player.wingTime == 0)
            {
                player.doubleJumpCloud = true;
                player.doubleJumpSandstorm = true;
                player.doubleJumpBlizzard = true;
                player.doubleJumpFart = true;
            }

            //FLIGHT MASTERY
            player.wingTimeMax = 999999;
            player.ignoreWater = true;
            player.wingTime = player.wingTimeMax;

            //TRAWLER
            //extra lures
            modPlayer.FishSoul2 = true;
            //modPlayer.AddPet("Zephyr Fish Pet", hideVisual, BuffID.ZephyrFish, ProjectileID.ZephyrFish);
            player.sonarPotion = true;
            player.fishingSkill += 50;
            player.cratePotion = true;
            player.accFishingLine = true;
            player.accTackleBox = true;
            player.accFishFinder = true;

            //WORLD SHAPER
            //placing speed up
            player.tileSpeed += 0.5f;
            player.wallSpeed += 0.5f;
            //toolbox
            Player.tileRangeX += 50;
            Player.tileRangeY += 50;
            //gizmo pack
            player.autoPaint = true;
            //pick speed
            player.pickSpeed -= 0.75f;
            //mining helmet
            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.MinerShine)) Lighting.AddLight(player.Center, 0.8f, 0.8f, 0f);
            //presserator
            player.autoActuator = true;
            //royal gel
            player.npcTypeNoAggro[1] = true;
            player.npcTypeNoAggro[16] = true;
            player.npcTypeNoAggro[59] = true;
            player.npcTypeNoAggro[71] = true;
            player.npcTypeNoAggro[81] = true;
            player.npcTypeNoAggro[138] = true;
            player.npcTypeNoAggro[121] = true;
            player.npcTypeNoAggro[122] = true;
            player.npcTypeNoAggro[141] = true;
            player.npcTypeNoAggro[147] = true;
            player.npcTypeNoAggro[183] = true;
            player.npcTypeNoAggro[184] = true;
            player.npcTypeNoAggro[204] = true;
            player.npcTypeNoAggro[225] = true;
            player.npcTypeNoAggro[244] = true;
            player.npcTypeNoAggro[302] = true;
            player.npcTypeNoAggro[333] = true;
            player.npcTypeNoAggro[335] = true;
            player.npcTypeNoAggro[334] = true;
            player.npcTypeNoAggro[336] = true;
            player.npcTypeNoAggro[537] = true;
            //builder mode
            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.BuilderMode))
                modPlayer.BuilderMode = true;
            //cell phone
            player.accWatch = 3;
            player.accDepthMeter = 1;
            player.accCompass = 1;
            player.accFishFinder = true;
            player.accDreamCatcher = true;
            player.accOreFinder = true;
            player.accStopwatch = true;
            player.accCritterGuide = true;
            player.accJarOfSouls = true;
            player.accThirdEye = true;
            player.accCalendar = true;
            player.accWeatherRadio = true;

            if (Fargowiltas.Instance.ThoriumLoaded) Thorium(player, hideVisual);

            if (Fargowiltas.Instance.CalamityLoaded) Calamity(player, hideVisual);
        }

        private void Thorium(Player player, bool hideVisual)
        {
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();
            ThoriumPlayer thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();
            //COLOSSUS
            //terrarium defender
            if (player.statLife < player.statLifeMax * 0.2f)
            {
                player.AddBuff(thorium.BuffType("TerrariumRegen"), 10, true);
                player.lifeRegen += 20;
            }
            if (player.statLife < player.statLifeMax * 0.25f)
            {
                player.AddBuff(thorium.BuffType("TerrariumDefense"), 10, true);
                player.statDefense += 20;
            }
            //blast shield
            thoriumPlayer.blastHurt = true;
            //cape of the survivor
            if (player.FindBuffIndex(thorium.BuffType("Corporeal")) < 0)
            {
                thoriumPlayer.spiritBand2 = true;
            }
            //sweet vengeance
            thoriumPlayer.sweetVengeance = true;
            //oceans retaliation
            thoriumPlayer.turtleShield2 = true;
            thoriumPlayer.SpinyShield = true;
            //TRAWLER
            MagmaBoundFishingLineMP magmaPlayer = player.GetModPlayer<MagmaBoundFishingLineMP>();
            magmaPlayer.magmaLine = true;
            //SUPERSONIC
            //terrarium particle sprinters dust
            if (Collision.SolidCollision(player.position, player.width, player.height + 4) && Math.Abs(player.velocity.X) >= 2)
            {
                for (int i = 0; i < 1; i++)
                {
                    int num = Dust.NewDust(new Vector2(player.position.X - 2f, player.position.Y + (float)player.height - 2f), player.width + 4, 4, 57, 0f, 0f, 100, default(Color), 1.4f);
                    Main.dust[num].noGravity = true;
                    Main.dust[num].noLight = true;
                    Dust dust = Main.dust[num];
                    dust.velocity *= 0f;
                }
                for (int j = 0; j < 1; j++)
                {
                    int num2 = Dust.NewDust(new Vector2(player.position.X - 2f, player.position.Y + (float)player.height - 2f), player.width + 4, 4, 61, 0f, 0f, 100, default(Color), 1.35f);
                    Main.dust[num2].noGravity = true;
                    Main.dust[num2].noLight = true;
                    Dust dust2 = Main.dust[num2];
                    dust2.velocity *= 0f;
                }
                for (int k = 0; k < 1; k++)
                {
                    int num3 = Dust.NewDust(new Vector2(player.position.X - 2f, player.position.Y + (float)player.height - 2f), player.width + 4, 4, 229, 0f, 0f, 100, default(Color), 1.15f);
                    Main.dust[num3].noGravity = true;
                    Main.dust[num3].noLight = true;
                    Dust dust3 = Main.dust[num3];
                    dust3.velocity *= 0f;
                }
                for (int l = 0; l < 1; l++)
                {
                    int num4 = Dust.NewDust(new Vector2(player.position.X - 2f, player.position.Y + (float)player.height - 2f), player.width + 4, 4, 60, 0f, 0f, 100, default(Color), 1.5f);
                    Main.dust[num4].noGravity = true;
                    Main.dust[num4].noLight = true;
                    Dust dust4 = Main.dust[num4];
                    dust4.velocity *= 0f;
                }
                for (int m = 0; m < 1; m++)
                {
                    int num5 = Dust.NewDust(new Vector2(player.position.X - 2f, player.position.Y + (float)player.height - 2f), player.width + 4, 4, 127, 0f, 0f, 100, default(Color), 1.75f);
                    Main.dust[num5].noGravity = true;
                    Main.dust[num5].noLight = true;
                    Dust dust5 = Main.dust[num5];
                    dust5.velocity *= 0f;
                }
                for (int n = 0; n < 1; n++)
                {
                    int num6 = Dust.NewDust(new Vector2(player.position.X - 2f, player.position.Y + (float)player.height - 2f), player.width + 4, 4, 59, 0f, 0f, 100, default(Color), 1.4f);
                    Main.dust[num6].noGravity = true;
                    Main.dust[num6].noLight = true;
                    Dust dust6 = Main.dust[num6];
                    dust6.velocity *= 0f;
                }
                for (int num7 = 0; num7 < 1; num7++)
                {
                    int num8 = Dust.NewDust(new Vector2(player.position.X - 2f, player.position.Y + (float)player.height - 2f), player.width + 4, 4, 62, 0f, 0f, 100, default(Color), 1.35f);
                    Main.dust[num8].noGravity = true;
                    Main.dust[num8].noLight = true;
                    Dust dust7 = Main.dust[num8];
                    dust7.velocity *= 0f;
                }
            }
            //air walkers
            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.thoriumToggles.AirWalkers))
            {
                if (player.controlDown)
                {
                    jumped = true;
                }
                else
                {
                    jumped = false;
                }
                if (!Collision.SolidCollision(player.position, player.width, player.height + 4))
                {
                    hoverTimer++;
                }
                else
                {
                    hoverTimer = 0;
                }
                if (hoverTimer >= 500)
                {
                    canHover = false;
                }
                else
                {
                    canHover = true;
                }
                if (canHover && !jumped && !Collision.SolidCollision(player.position, player.width, player.height + 4))
                {
                    player.maxFallSpeed = 0f;
                    player.fallStart = (int)(player.position.Y / 16f);
                    int num = Dust.NewDust(new Vector2(player.position.X - 2f, player.position.Y + (float)player.height - 2f), player.width + 4, 4, 229, 0f, 0f, 100, default(Color), 1.25f);
                    Main.dust[num].noGravity = true;
                    Main.dust[num].noLight = true;
                    Dust dust = Main.dust[num];
                    dust.velocity *= 0f;
                }
            }
            //survivalist boots
            if (Math.Abs(player.velocity.X) > 2f)
            {
                thoriumPlayer.lifeRegenPenaltyReduction += 0.1f;
                player.lifeRegen += 2;
                player.lifeRegenTime++;
                thoriumPlayer.manaRegenPenaltyReduction += 0.1f;
                player.manaRegenBonus += 2;
                player.manaRegenDelayBonus++;
                thoriumPlayer.inspirationRegenBonus += 0.03f;
            }
            //weighted winglets
            if (player.controlDown && !player.controlUp)
            {
                player.maxFallSpeed *= (player.wet ? 2.4f : 1.6f);
            }
            if (player.controlUp && !player.controlDown)
            {
                player.maxFallSpeed *= 0.4f;
                player.fallStart = (int)(player.position.Y / 16f);
            }

            //WORLD SHAPER
            //pets
            modPlayer.AddPet(SoulConfig.Instance.thoriumToggles.LanternPet, hideVisual, thorium.BuffType("SupportLanternBuff"), thorium.ProjectileType("SupportLantern"));
            modPlayer.AddPet(SoulConfig.Instance.thoriumToggles.BoxPet, hideVisual, thorium.BuffType("LockBoxBuff"), thorium.ProjectileType("LockBoxPet"));
        }

        private void Calamity(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.GetModPlayer<CalamityPlayer>();
            //tank soul
            //rampart of dieties
            modPlayer.dAmulet = true;
            //becase calamity made it itself for some reason no duplicate
            player.starCloak = false;
            //asgardian aegis
            modPlayer.dashMod = 4;
            modPlayer.elysianAegis = true;
            player.buffImmune[calamity.BuffType("BrimstoneFlames")] = true;
            player.buffImmune[calamity.BuffType("HolyLight")] = true;
            player.buffImmune[calamity.BuffType("GlacialState")] = true;
            //celestial tracers
            modPlayer.IBoots = !hideVisual;
            modPlayer.elysianFire = !hideVisual;
            modPlayer.cTracers = true;
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
            ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 1f; 
            ascentWhenRising = 0.3f; 
            maxCanAscendMultiplier = 1.5f;
            maxAscentMultiplier = 3f;
            constantAscend = 0.15f; 
        }

        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            speed = SoulConfig.Instance.GetValue(SoulConfig.Instance.SupersonicSpeed) ? 25f : 15f;
            acceleration *= 3.5f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(null, "ColossusSoul");
            recipe.AddIngredient(null, "SupersonicSoul");
            recipe.AddIngredient(null, "FlightMasterySoul");
            recipe.AddIngredient(null, "TrawlerSoul");
            recipe.AddIngredient(null, "WorldShaperSoul");
            recipe.AddIngredient(null, "MutatingEnergy", 10);

            if (Fargowiltas.Instance.CalamityLoaded)
            {
                recipe.AddIngredient(calamity.ItemType("CelestialTracers"));
            }

            recipe.AddTile(mod, "CrucibleCosmosSheet");
                
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
