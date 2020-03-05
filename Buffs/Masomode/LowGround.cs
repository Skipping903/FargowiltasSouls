using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Buffs.Masomode
{
    public class LowGround : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Low Ground");
            Description.SetDefault("Cannot stand on platforms or liquids");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override bool Autoload(ref string name, ref string texture)
        {
            texture = "FargowiltasSouls/Buffs/PlaceholderDebuff";
            return true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<FargoPlayer>().LowGround = true;
            for (int i = -2; i <= 2; i++)
            {
                Vector2 pos = player.Center;
                pos.X += i * 16;
                pos.Y += player.height / 2;
                if (player.mount.Active)
                    pos.Y += player.mount.HeightBoost;
                pos.Y += 8;

                Tile tile = Framing.GetTileSafely((int)(pos.X / 16), (int)(pos.Y / 16));
                if (tile.type == TileID.Platforms || tile.type == TileID.PlanterBox)
                    tile.inActive(true);
            }
        }
    }
}