﻿using FargowiltasSouls.NPCs;
using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Buffs.Souls
{
    public class DisruptedFocus : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Disrupted Focus");
            Description.SetDefault("Weapon speed reduced");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            canBeCleared = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<FargoPlayer>().AttackSpeed -= .2f;
        }
    }
}