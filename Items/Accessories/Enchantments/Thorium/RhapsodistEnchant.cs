using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Enchantments.Thorium
{
    public class RhapsodistEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override bool Autoload(ref string name)
        {
            return ModLoader.GetMod("ThoriumMod") != null;
        }
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhapsodist Enchantment");
            Tooltip.SetDefault(
@"'Allow your song to inspire an army, Prove to all that your talent is second to none'
Inspiration notes that drop will become more potent
Additionally, they give a random level 1 empowerment to all nearby allies
Pressing the 'Special Ability' key will grant you infinite inspiration and increased symphonic damage and playing speed
It also overloads all nearby allies with every empowerment III for 15 seconds
These effects needs to recharge for 1 minute");
            DisplayName.AddTranslation(GameCulture.Chinese, "狂想曲魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'歌曲振奋军队, 向所有人证明你的才华独一无二'
凋落的灵感音符强度增加
此外, 给予附近所有队友随机的1级咒音增幅
按下'特殊能力'键获得无限灵感, 增加音波伤害和演奏速度
同时也会超载附近队友, 给予他们所有种类的3级咒音增幅
该能力需充能1分钟");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 10;
            item.value = 400000;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color?(new Color(255, 128, 0));
                }
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!Fargowiltas.Instance.ThoriumLoaded) return;

            ThoriumPlayer thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();
            //notes heal more and give random empowerments
            thoriumPlayer.armInspirator = true;
            //hotkey buff allies 
            thoriumPlayer.setInspirator = true;
            //hotkey buff self
            thoriumPlayer.setSoloist = true;
        }
        
        private readonly string[] items =
        {
            "SoloistHat",
            "RallyHat",
            "RhapsodistChestWoofer",
            "RhapsodistBoots",
            "SirensAllure",
            "TerrariumAutoharp",
            "Sousaphone",
            "Holophonor",
            "EdgeofImagination",
            "BlackMIDI"
        };

        public override void AddRecipes()
        {
            if (!Fargowiltas.Instance.ThoriumLoaded) return;
            
            ModRecipe recipe = new ModRecipe(mod);
            
            foreach (string i in items) recipe.AddIngredient(thorium.ItemType(i));

            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
