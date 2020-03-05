using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Souls
{
    //[AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff)]
    public class OlympiansSoul : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");
        private readonly Mod fargos = ModLoader.GetMod("Fargowiltas");
        private readonly Mod calamity = ModLoader.GetMod("CalamityMod");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Olympian's Soul");

            string tooltip =
@"'Strike with deadly precision'
30% increased throwing damage
20% increased throwing speed
15% increased throwing critical chance and velocity";
            string tooltip_ch =
@"'致命的精准打击'
增加30%投掷伤害
增加20%投掷速度
增加15%投掷暴击率和抛射物速度";

            if (thorium != null)
            {
                tooltip += "\nEffects of Guide to Expert Throwing - Volume III, Mermaid's Canteen, and Deadman's Patch";
                tooltip_ch += "\n拥有投手大师指导:卷三,美人鱼水壶和亡者眼罩的效果";
            }

            if (calamity != null)
            {
                tooltip += "\nEffects of Nanotech\nBonuses also effect rogue damage";
                tooltip_ch += "\n拥有纳米技术的效果\n加成同样影响盗贼伤害";
            }

            Tooltip.SetDefault(tooltip);
            DisplayName.AddTranslation(GameCulture.Chinese, "奥林匹斯之魂");
            Tooltip.AddTranslation(GameCulture.Chinese, tooltip_ch);
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            item.value = 1000000;
            item.rare = 11;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color?(new Color(85, 5, 230));
                }
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            //throw speed
            player.GetModPlayer<FargoPlayer>().ThrowSoul = true;
            player.thrownDamage += 0.3f;
            player.thrownCrit += 15;
            player.thrownVelocity += 0.15f;

            if (Fargowiltas.Instance.ThoriumLoaded) Thorium(player);

            if (Fargowiltas.Instance.CalamityLoaded) Calamity(player);
        }

        private void Thorium(Player player)
        {
            ThoriumPlayer thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();
            thoriumPlayer.throwGuide2 = true;
            //dead mans patch
            thoriumPlayer.deadEyeBool = true;
            //mermaid canteen
            thoriumPlayer.throwerExhaustionMax += 1125;
            thoriumPlayer.canteenCadet = true;
        }

        private void Calamity(Player player)
        {
            calamity.Call("AddRogueDamage", player, 0.3f);
            calamity.Call("AddRogueCrit", player, 15);
            calamity.Call("AddRogueVelocity", player, 0.15f);

            player.GetModPlayer<CalamityMod.CalPlayer.CalamityPlayer>().nanotech = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(null, "SlingersEssence");

            if (Fargowiltas.Instance.ThoriumLoaded)
            {
                recipe.AddIngredient(Fargowiltas.Instance.CalamityLoaded ? calamity.ItemType("Nanotech") : thorium.ItemType("MagnetoGrip"));
                recipe.AddIngredient(thorium.ItemType("ThrowingGuideVolume3"));
                recipe.AddIngredient(thorium.ItemType("MermaidCanteen"));
                recipe.AddIngredient(thorium.ItemType("DeadEyePatch"));
                recipe.AddIngredient(fargos != null ? fargos.ItemType("BananarangThrown") : ItemID.Bananarang, 5);
                recipe.AddIngredient(thorium.ItemType("HotPot"));
                recipe.AddIngredient(thorium.ItemType("VoltHatchet"));
                recipe.AddIngredient(thorium.ItemType("SparkTaser"));
                recipe.AddIngredient(thorium.ItemType("PharaohsSlab"));
                recipe.AddIngredient(thorium.ItemType("TerraKnife"));
                recipe.AddIngredient(fargos != null ? fargos.ItemType("VampireKnivesThrown") : ItemID.VampireKnives);
                recipe.AddIngredient(fargos != null ? fargos.ItemType("PaladinsHammerThrown") : ItemID.PaladinsHammer);
                recipe.AddIngredient(fargos != null ? fargos.ItemType("TerrarianThrown") : ItemID.Terrarian);
            }
            else
            {
                if(Fargowiltas.Instance.CalamityLoaded)
                    recipe.AddIngredient( calamity.ItemType("Nanotech"));
                else
                    recipe.AddIngredient(fargos != null ? fargos.ItemType("MagicDaggerThrown") : ItemID.MagicDagger);

                recipe.AddIngredient(fargos != null ? fargos.ItemType("BananarangThrown") : ItemID.Bananarang, 5);
                recipe.AddIngredient(fargos != null ? fargos.ItemType("AmarokThrown") : ItemID.Amarok);
                recipe.AddIngredient(fargos != null ? fargos.ItemType("ShadowFlameKnifeThrown") : ItemID.ShadowFlameKnife);
                recipe.AddIngredient(fargos != null ? fargos.ItemType("FlyingKnifeThrown") : ItemID.FlyingKnife);
                recipe.AddIngredient(fargos != null ? fargos.ItemType("LightDiscThrown") : ItemID.LightDisc, 5);
                recipe.AddIngredient(fargos != null ? fargos.ItemType("FlowerPowThrown") : ItemID.FlowerPow);
                recipe.AddIngredient(fargos != null ? fargos.ItemType("ToxicFlaskThrown") : ItemID.ToxicFlask);
                recipe.AddIngredient(fargos != null ? fargos.ItemType("VampireKnivesThrown") : ItemID.VampireKnives);
                recipe.AddIngredient(fargos != null ? fargos.ItemType("PaladinsHammerThrown") : ItemID.PaladinsHammer);
                recipe.AddIngredient(fargos != null ? fargos.ItemType("PossessedHatchetThrown") : ItemID.PossessedHatchet);
                recipe.AddIngredient(fargos != null ? fargos.ItemType("TheEyeOfCthulhuThrown") : ItemID.TheEyeOfCthulhu);
                recipe.AddIngredient(fargos != null ? fargos.ItemType("TerrarianThrown") : ItemID.Terrarian);
            }

            recipe.AddTile(mod, "CrucibleCosmosSheet");
                
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
