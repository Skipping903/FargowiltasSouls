using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Buffs.Masomode
{
    public class Guilty : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Guilty");
            Description.SetDefault("Weapons dulled by the guilt of slaying innocent critters");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            canBeCleared = true;
            DisplayName.AddTranslation(GameCulture.Chinese, "内疚");
            Description.AddTranslation(GameCulture.Chinese, "杀害无辜动物的内疚使你的武器变得迟钝");
        }

        public override bool Autoload(ref string name, ref string texture)
        {
            texture = "FargowiltasSouls/Buffs/PlaceholderDebuff";
            return true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.meleeDamage -= 0.25f;
            player.rangedDamage -= 0.25f;
            player.magicDamage -= 0.25f;
            player.minionDamage -= 0.25f;
            player.thrownDamage -= 0.25f;
        }
    }
}