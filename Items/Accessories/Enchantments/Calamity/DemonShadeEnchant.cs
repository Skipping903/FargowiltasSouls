﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using CalamityMod.CalPlayer;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Enchantments.Calamity
{
    public class DemonShadeEnchant : ModItem
    {
        private readonly Mod calamity = ModLoader.GetMod("CalamityMod");

        public override bool Autoload(ref string name)
        {
            return ModLoader.GetMod("CalamityMod") != null;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Demonshade Enchantment");
            Tooltip.SetDefault(
@"'Demonic power emanates from you…'
All attacks inflict Demon Flames
Shadowbeams and Demon Scythes fall from the sky on hit
A friendly red devil follows you around
Press Y to enrage nearby enemies with a dark magic spell for 10 seconds
This makes them do 1.5 times more damage but they also take five times as much damage
Summons a Levi pet");
            DisplayName.AddTranslation(GameCulture.Chinese, "魔影魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'你身上散发着恶魔之力...'
所有攻击造成恶魔之炎
被攻击时释放暗影射线和恶魔镰刀
一个友好的红魔鬼将跟随你
敌人触碰你时受到鬼魔伤害
站立不动你将吸收暗影能量并大幅增加生命恢复速度
按'Y'键用黑魔法激怒附近敌人10秒
这将使敌人造成1.5倍伤害, 同时受到5倍伤害");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 10;
            item.value = 50000000;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color(173, 52, 70);
                }
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!Fargowiltas.Instance.CalamityLoaded) return;

            CalamityPlayer modPlayer = player.GetModPlayer<CalamityPlayer>();
            //set bonus
            calamity.Call("SetSetBonus", player, "demonshade", true);

            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.calamityToggles.RedDevilMinion))
            {
                modPlayer.redDevil = true;
                if (player.whoAmI == Main.myPlayer)
                {
                    if (player.FindBuffIndex(calamity.BuffType("RedDevil")) == -1)
                    {
                        player.AddBuff(calamity.BuffType("RedDevil"), 3600, true);
                    }
                    if (player.ownedProjectileCounts[calamity.ProjectileType("RedDevil")] < 1)
                    {
                        Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, -1f, calamity.ProjectileType("RedDevil"), 10000, 0f, Main.myPlayer, 0f, 0f);
                    }
                }
            }

            FargoPlayer fargoPlayer = player.GetModPlayer<FargoPlayer>();
            fargoPlayer.DemonShadeEnchant = true;
            fargoPlayer.AddPet(SoulConfig.Instance.calamityToggles.LeviPet, hideVisual, calamity.BuffType("Levi"), calamity.ProjectileType("Levi"));
        }

        public override void AddRecipes()
        {
            if (!Fargowiltas.Instance.CalamityLoaded) return;

            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(calamity.ItemType("DemonshadeHelm"));
            recipe.AddIngredient(calamity.ItemType("DemonshadeBreastplate"));
            recipe.AddIngredient(calamity.ItemType("DemonshadeGreaves"));
            recipe.AddIngredient(calamity.ItemType("Earth"));
            recipe.AddIngredient(calamity.ItemType("Animus"));
            recipe.AddIngredient(calamity.ItemType("Contagion"));
            recipe.AddIngredient(calamity.ItemType("Megafleet"));
            recipe.AddIngredient(calamity.ItemType("SomaPrime"));
            recipe.AddIngredient(calamity.ItemType("Judgement"));
            recipe.AddIngredient(calamity.ItemType("Apotheosis"));
            recipe.AddIngredient(calamity.ItemType("RoyalKnives"));
            recipe.AddIngredient(calamity.ItemType("NanoblackReaperRogue"));
            recipe.AddIngredient(calamity.ItemType("TriactisTruePaladinianMageHammerofMight"));
            recipe.AddIngredient(calamity.ItemType("Levi"));

            recipe.AddTile(calamity, "DraedonsForge");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
