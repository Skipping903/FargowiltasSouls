using Terraria;
using Terraria.ModLoader;
using static Terraria.ID.ItemID;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Souls
{
    //[AutoloadEquip(EquipType.Back)]
    public class WorldShaperSoul : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("World Shaper Soul");
            Tooltip.SetDefault(
@"'Limitless possibilities'
Increased block and wall placement speed by 50% 
Near infinite block placement and mining reach
Mining speed tripled 
Shows the location of enemies, traps, and treasures
Auto paint and actuator effect 
Provides light and allows gravity control
Grants the ability to enable Builder Mode:
Anything that creates a tile will not be consumed and can be used much faster
No enemies can spawn
Effect can be disabled in Soul Toggles menu
Effects of the Cell Phone and Royal Gel
Summons a pet Magic Lantern");
            DisplayName.AddTranslation(GameCulture.Chinese, "铸世者之魂");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'无限的可能性'
增加50%放置物块及墙壁的速度
近乎无限的放置和采掘距离
四倍采掘速度
显示敌人,陷阱和宝藏
自动喷漆和制动器效果
提供光照和重力控制
获得开启建造模式的能力:
放置方块不会消耗
没有敌人生成
效果可以在灵魂切换菜单中禁用
拥有手机的效果
召唤一个魔法灯笼");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            item.value = 750000;
            item.rare = 11;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color?(new Color(255, 239, 2));
                }
            }
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
            //mining speed, spelunker, dangersense, light, hunter, pet
            modPlayer.MinerEffect(hideVisual, .66f);
            //placing speed up
            player.tileSpeed += 0.5f;
            player.wallSpeed += 0.5f;
            //toolbox
            Player.tileRangeX += 50;
            Player.tileRangeY += 50;
            //gizmo pack
            player.autoPaint = true;
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

            if (!Fargowiltas.Instance.ThoriumLoaded) return;

            //pets
            modPlayer.AddPet(SoulConfig.Instance.thoriumToggles.LanternPet, hideVisual, thorium.BuffType("SupportLanternBuff"), thorium.ProjectileType("SupportLantern"));
            modPlayer.AddPet(SoulConfig.Instance.thoriumToggles.BoxPet, hideVisual, thorium.BuffType("LockBoxBuff"), thorium.ProjectileType("LockBoxPet"));
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(null, Fargowiltas.Instance.ThoriumLoaded ? "GeodeEnchant" : "MinerEnchant");
            recipe.AddIngredient(Toolbelt);
            recipe.AddIngredient(Toolbox);
            recipe.AddIngredient(ArchitectGizmoPack);
            recipe.AddIngredient(ActuationAccessory);
            recipe.AddIngredient(LaserRuler);
            recipe.AddIngredient(RoyalGel);
            recipe.AddIngredient(PutridScent);
            recipe.AddIngredient(CellPhone);
            recipe.AddIngredient(GravityGlobe);
            recipe.AddIngredient(BonePickaxe);
            recipe.AddRecipeGroup("FargowiltasSouls:AnyDrax");
            recipe.AddIngredient(ShroomiteDiggingClaw);
            recipe.AddIngredient(DrillContainmentUnit);

            recipe.AddTile(mod, "CrucibleCosmosSheet");
                
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
