﻿using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;


namespace FargowiltasSouls.Buffs
{
    public class RoombaPetBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Roomba");
            Description.SetDefault("This Roomba is following you");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
            DisplayName.AddTranslation(GameCulture.Chinese, "扫地机器人");
            Description.AddTranslation(GameCulture.Chinese, "这个扫地机器人在跟着你");
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<FargoPlayer>().RoombaPet = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType("RoombaPetProj")] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, mod.ProjectileType("RoombaPetProj"), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }
    }
}