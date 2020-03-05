using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Buffs.Boss
{
    public class MutantPresence : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Mutant Presence");
            Description.SetDefault("Defense, damage reduction, and life regen reduced; almost all soul toggles disabled; Chaos State effect");
            DisplayName.AddTranslation(GameCulture.Chinese, "突变驾到");
            Description.AddTranslation(GameCulture.Chinese, "减少防御, 伤害减免和生命回复; 关闭所有魂效果; 附带混沌效果");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            longerExpertDebuff = false;
            canBeCleared = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            //also halves defense, DR, and cripples life regen
            player.GetModPlayer<FargoPlayer>().noDodge = true;
            player.GetModPlayer<FargoPlayer>().noSupersonic = true;
            player.GetModPlayer<FargoPlayer>().MutantPresence = true;
            player.moonLeech = true;
            player.chaosState = true;
        }
    }
}