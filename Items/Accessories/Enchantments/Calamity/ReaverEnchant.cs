﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace FargowiltasSouls.Items.Accessories.Enchantments.Calamity
{
    public class ReaverEnchant : ModItem
    {
        private readonly Mod calamity = ModLoader.GetMod("CalamityMod");

        public override bool Autoload(ref string name)
        {
            return ModLoader.GetMod("CalamityMod") != null;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Reaver Enchantment");
            Tooltip.SetDefault(
@"'A thorny death awaits your enemies...'
Melee projectiles explode on hit
While using a ranged weapon you have a 10% chance to fire a powerful rocket
Your magic projectiles emit a burst of spore gas on enemy hits
Summons a reaver orb that emits spore gas when enemies are near
You emit a cloud of spores when you are hit
Rage activates when you are damaged
Effects of Fabled Tortoise Shell");
            DisplayName.AddTranslation(GameCulture.Chinese, "掠夺者魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'痛苦的死亡等待着你的敌人...'
近战抛射物造成爆炸
远程攻击有10%概率发射一个强力火箭
魔法抛射物命中敌人时释放孢子爆炸
召唤收割者之球向附近敌人发射孢子云
被攻击时释放孢子云
被攻击时激活'愤怒'
拥有寓言龟壳的效果");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 7;
            item.value = 400000;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color(54, 164, 66);
                }
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!Fargowiltas.Instance.CalamityLoaded) return;

            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.calamityToggles.ReaverEffects))
            {
                calamity.Call("SetSetBonus", player, "reaver_melee", true);
                calamity.Call("SetSetBonus", player, "reaver_ranged", true);
                calamity.Call("SetSetBonus", player, "reaver_magic", true);
                calamity.Call("SetSetBonus", player, "reaver_rogue", true);
            }
            
            if (player.GetModPlayer<FargoPlayer>().Eternity) return;

            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.calamityToggles.ReaverMinion))
            {
                calamity.Call("SetSetBonus", player, "reaver_summon", true);
                if (player.whoAmI == Main.myPlayer)
                {
                    if (player.FindBuffIndex(calamity.BuffType("ReaverOrb")) == -1)
                    {
                        player.AddBuff(calamity.BuffType("ReaverOrb"), 3600, true);
                    }
                    if (player.ownedProjectileCounts[calamity.ProjectileType("ReaverOrb")] < 1)
                    {
                        Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, -1f, calamity.ProjectileType("ReaverOrb"), (int)(80f * player.minionDamage), 0f, Main.myPlayer, 0f, 0f);
                    }
                }
            }

            //fabled tortoise shell
            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.calamityToggles.FabledTurtleShell))
            {
                calamity.GetItem("FabledTortoiseShell").UpdateAccessory(player, hideVisual);
                player.statDefense += 35;
            }
            
        }

        public override void AddRecipes()
        {
            if (!Fargowiltas.Instance.CalamityLoaded) return;

            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddRecipeGroup("FargowiltasSouls:AnyReaverHelmet");
            recipe.AddIngredient(calamity.ItemType("ReaverScaleMail"));
            recipe.AddIngredient(calamity.ItemType("ReaverCuisses"));
            recipe.AddIngredient(calamity.ItemType("FabledTortoiseShell"));
            recipe.AddIngredient(calamity.ItemType("EvilSmasher"));
            recipe.AddIngredient(calamity.ItemType("Animosity"));
            recipe.AddIngredient(calamity.ItemType("Tumbleweed"));
            recipe.AddIngredient(calamity.ItemType("SandSharknadoStaff"));
            recipe.AddIngredient(calamity.ItemType("Leviatitan"));
            recipe.AddIngredient(calamity.ItemType("Keelhaul"));
            recipe.AddIngredient(calamity.ItemType("Triploon"));
            recipe.AddIngredient(calamity.ItemType("MagnaStriker"));
            recipe.AddIngredient(calamity.ItemType("PearlGod"));
            recipe.AddIngredient(calamity.ItemType("ConferenceCall"));

            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
