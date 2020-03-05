using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace FargowiltasSouls.Items.Misc
{
	public class MutantsPact : ModItem
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mutant's Pact");
            Tooltip.SetDefault(@"Permanently reduces Mutant's shop prices by 30%");
		}

		public override void SetDefaults()
		{
            item.width = 20;
            item.height = 20;
            item.rare = 9;
            item.maxStack = 1;
            item.useStyle = 4;
            item.useAnimation = 17;
            item.useTime = 17;
            item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return !player.GetModPlayer<FargoPlayer>().MutantsPact;
        }

        public override bool UseItem(Player player)
        {
            if (player.itemAnimation > 0 && player.itemTime == 0)
            {
                player.GetModPlayer<FargoPlayer>().MutantsPact = true;
                Main.PlaySound(15, (int)player.position.X, (int)player.position.Y, 0);
            }
            return true;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine line2 in list)
            {
                if (line2.mod == "Terraria" && line2.Name == "ItemName")
                {
                    line2.overrideColor = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
                }
            }
        }
    }
}
