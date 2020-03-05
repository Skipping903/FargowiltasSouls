﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Forces.Thorium
{
    public class MuspelheimForce : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override bool Autoload(ref string name)
        {
            return ModLoader.GetMod("ThoriumMod") != null;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Force of Muspelheim");
            Tooltip.SetDefault(
@"'A blazing heat, the mark of Surtr...'
All armor bonuses from Sandstone, Danger, Flight, and Fungus
All armor bonuses from Feral Fur, Living Wood, Bulb, and Life Bloom
Effects of Night Shade Petal, Flawless Chrysalis, and Bee Booties");
            DisplayName.AddTranslation(GameCulture.Chinese, "穆斯贝尔海姆之力");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'炽热之火, 史尔特尔的标志...'
沙暴增强了你的靴子, 能够额外跳跃一次
免疫一些造成伤害的Debuff
暴击获得野性咆哮效果, 并短暂增加召唤物伤害
攻击有33%的概率治疗你
召唤具有追踪攻击能力的小树苗
拥有无暇之蛹和植物纤维绳索宝典的效果");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 11;
            item.value = 600000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!Fargowiltas.Instance.ThoriumLoaded) return;

            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();
            ThoriumPlayer thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();
            
            //life bloom effect
            modPlayer.LifeBloomEnchant = true;
            //chrysalis
            thoriumPlayer.cocoonAcc = true;
            //living wood set bonus
            thoriumPlayer.livingWood = true;
            //free boi
            modPlayer.LivingWoodEnchant = true;
            modPlayer.AddMinion(SoulConfig.Instance.thoriumToggles.SaplingMinion, thorium.ProjectileType("MinionSapling"), 25, 2f);

            //bulb set bonus
            modPlayer.BulbEnchant = true;
            //bee booties
            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.thoriumToggles.BeeBooties))
            {
                thorium.GetItem("BeeBoots").UpdateAccessory(player, hideVisual);
                player.moveSpeed -= 0.15f;
                player.maxRunSpeed -= 1f;
            }

            if (modPlayer.ThoriumSoul) return;

            //sandstone
            player.doubleJumpSandstorm = true;
            //danger
            player.buffImmune[BuffID.Frostburn] = true;
            player.buffImmune[BuffID.Poisoned] = true;
            player.buffImmune[BuffID.OnFire] = true;
            player.buffImmune[BuffID.Bleeding] = true;
            player.buffImmune[BuffID.Venom] = true;
            //night shade petal
            thoriumPlayer.nightshadeBoost = true;
            //flight
            modPlayer.wingTimeModifier += 1f;
            //fungus
            modPlayer.FungusEnchant = true;
            //feral fur
            modPlayer.FeralFurEnchant = true;

        }

        public override void AddRecipes()
        {
            if (!Fargowiltas.Instance.ThoriumLoaded) return;

            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(null, "SandstoneEnchant");
            recipe.AddIngredient(null, "DangerEnchant");
            recipe.AddIngredient(null, "FlightEnchant");
            recipe.AddIngredient(null, "FeralFurEnchant");
            recipe.AddIngredient(null, "FungusEnchant");
            recipe.AddIngredient(null, "LifeBloomEnchant");

            recipe.AddTile(TileID.LunarCraftingStation);

            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
