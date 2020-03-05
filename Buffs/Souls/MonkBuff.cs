﻿using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace FargowiltasSouls.Buffs.Souls
{
    public class MonkBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Meditation");
            Description.SetDefault("You have a one use Monk Dash");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex]++;

            if (player.mount.Active)
            {
                return;
            }
            
            int direction = 0;
            bool vertical = false;

            //down
            if ((player.controlDown && player.releaseDown))
            {
                if (player.doubleTapCardinalTimer[0] > 0 && player.doubleTapCardinalTimer[0] != 15)
                {
                    direction = 1;
                    vertical = true;
                }
            }
            //up
            else if ((player.controlUp && player.releaseUp))
            {
                if (player.doubleTapCardinalTimer[1] > 0 && player.doubleTapCardinalTimer[1] != 15)
                {
                    direction = -1;
                    vertical = true;
                }
            }
            //right
            else if ((player.controlRight && player.releaseRight))
            {
                if (player.doubleTapCardinalTimer[2] > 0 && player.doubleTapCardinalTimer[2] != 15)
                {
                    direction = 1;
                    vertical = false;
                }
            }
            //left
            else if ((player.controlLeft && player.releaseLeft))
            {
                if (player.doubleTapCardinalTimer[3] > 0 && player.doubleTapCardinalTimer[3] != 15)
                {
                    direction = -1;
                    vertical = false;
                }
            }

            if (direction != 0)
            {
                MonkDash(player, vertical, direction);
                player.buffTime[buffIndex] = 0;
            }
        }

        private void MonkDash(Player player, bool vertical, int direction)
        {
            //horizontal
            if (!vertical)
            {
                player.GetModPlayer<FargoPlayer>().MonkDashing = 20;
                player.velocity.X = 25 * (float)direction;
            }
            else
            {
                player.GetModPlayer<FargoPlayer>().MonkDashing = -20;
                player.velocity.Y = 35 * (float)direction;
            }

            player.dashDelay = -1;

            //dash dust n stuff
            for (int num17 = 0; num17 < 20; num17++)
            {
                int num18 = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, 31, 0f, 0f, 100, default(Color), 2f);
                Dust expr_CDB_cp_0 = Main.dust[num18];
                expr_CDB_cp_0.position.X = expr_CDB_cp_0.position.X + (float)Main.rand.Next(-5, 6);
                Dust expr_D02_cp_0 = Main.dust[num18];
                expr_D02_cp_0.position.Y = expr_D02_cp_0.position.Y + (float)Main.rand.Next(-5, 6);
                Main.dust[num18].velocity *= 0.2f;
                Main.dust[num18].scale *= 1f + (float)Main.rand.Next(20) * 0.01f;
                //Main.dust[num18].shader = GameShaders.Armor.GetSecondaryShader(player.cShoe, this);
            }
            int num19 = Gore.NewGore(new Vector2(player.position.X + (float)(player.width / 2) - 24f, player.position.Y + (float)(player.height / 2) - 34f), default(Vector2), Main.rand.Next(61, 64), 1f);
            Main.gore[num19].velocity.X = (float)Main.rand.Next(-50, 51) * 0.01f;
            Main.gore[num19].velocity.Y = (float)Main.rand.Next(-50, 51) * 0.01f;
            Main.gore[num19].velocity *= 0.4f;
            num19 = Gore.NewGore(new Vector2(player.position.X + (float)(player.width / 2) - 24f, player.position.Y + (float)(player.height / 2) - 14f), default(Vector2), Main.rand.Next(61, 64), 1f);
            Main.gore[num19].velocity.X = (float)Main.rand.Next(-50, 51) * 0.01f;
            Main.gore[num19].velocity.Y = (float)Main.rand.Next(-50, 51) * 0.01f;
            Main.gore[num19].velocity *= 0.4f;
        }
    }
}