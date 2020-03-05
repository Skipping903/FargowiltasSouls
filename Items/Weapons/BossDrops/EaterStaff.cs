using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Weapons.BossDrops
{
    public class EaterStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eater of Worlds Staff");
            Tooltip.SetDefault(
                @"An old foe beaten into submission..
Summons 4 segments for each minion slot");
            DisplayName.AddTranslation(GameCulture.Chinese, "世界吞噬者法杖");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"一个被迫屈服的老对手..
每个召唤栏召唤4段身体");
        }

        public override void SetDefaults()
        {
            item.mana = 10;
            item.damage = 10;
            item.useStyle = 1;
            item.shootSpeed = 10f;
            item.shoot = mod.ProjectileType("EaterHead");
            item.width = 26;
            item.height = 28;
            item.UseSound = SoundID.Item44;
            item.useAnimation = 36;
            item.useTime = 36;
            item.rare = 2;
            item.noMelee = true;
            item.knockBack = 2f;
            item.buffType = mod.BuffType("EaterMinion");
            item.buffTime = 3600;
            item.summon = true;
            item.value = 40000;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            //to fix tail disapearing meme
            float slotsUsed = 0;

            Main.projectile.Where(x => x.active && x.owner == player.whoAmI && x.minionSlots > 0).ToList().ForEach(x => { slotsUsed += x.minionSlots; });

            if (player.maxMinions - slotsUsed < 1) return false;

            int headCheck = -1;
            int tailCheck = -1;

            for (int i = 0; i < 1000; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == player.whoAmI)
                {
                    if (headCheck == -1 && proj.type == mod.ProjectileType("EaterHead")) headCheck = i;
                    if (tailCheck == -1 && proj.type == mod.ProjectileType("EaterTail")) tailCheck = i;
                    if (headCheck != -1 && tailCheck != -1) break;
                }
            }

            //initial spawn
            if (headCheck == -1 && tailCheck == -1)
            {
                int current = Projectile.NewProjectile(position.X, position.Y, 0, 0, mod.ProjectileType("EaterHead"), damage, knockBack, player.whoAmI, 0f, 0f);

                int previous = 0;

                for (int i = 0; i < 4; i++)
                {
                    current = Projectile.NewProjectile(position.X, position.Y, 0, 0, mod.ProjectileType("EaterBody"), damage, knockBack, player.whoAmI, current, 0f);
                    previous = current;
                }

                current = Projectile.NewProjectile(position.X, position.Y, 0, 0, mod.ProjectileType("EaterTail"), damage, knockBack, player.whoAmI, current, 0f);

                Main.projectile[previous].localAI[1] = current;
                Main.projectile[previous].netUpdate = true;
            }
            //spawn more body segments
            else
            {
                int previous = (int) Main.projectile[tailCheck].ai[0];
                int current = 0;

                for (int i = 0; i < 4; i++)
                {
                    current = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("EaterBody"), damage, knockBack, player.whoAmI,
                        Projectile.GetByUUID(Main.myPlayer, previous), 0f);

                    previous = current;
                }

                Main.projectile[current].localAI[1] = tailCheck;

                Main.projectile[tailCheck].ai[0] = current;
                Main.projectile[tailCheck].netUpdate = true;
                Main.projectile[tailCheck].ai[1] = 1f;
            }

            return false;
        }
    }
}