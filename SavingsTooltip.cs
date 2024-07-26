using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SavingsTooltip
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public class SavingsTooltip : Mod {

	}

	public class ItemTooltips : GlobalItem {
		public string Test(Item item, out Color diff) {
            Main.LocalPlayer.GetItemExpectedPrice(item, out var calcForSelling, out var calcForBuying);
            string text = "";
            int priceDiff = (item.shopCustomPrice ?? item.value) - (int)calcForBuying;
            diff = (priceDiff < 1) ? Colors.RarityRed : Colors.RarityGreen;

            if (priceDiff < 0) {
                priceDiff *= -1;
            }
            if (priceDiff >= 1000000) {
                int plat = priceDiff / 1000000;
                priceDiff -= plat * 1000000;
				text += $"{plat} {Lang.inter[15].Value} ";
			}
            if (priceDiff >= 10000) {
				int gold = priceDiff / 10000;
                priceDiff -= gold * 10000;
				text += $"{gold} {Lang.inter[16].Value} ";
			}
            if (priceDiff >= 100) {
				int silv = priceDiff / 100;
                priceDiff -= silv * 100;
				text += $"{silv} {Lang.inter[17].Value} ";
			}
            if (priceDiff >= 1) {
				text += $"{priceDiff} {Lang.inter[18].Value} ";
			}
            return text.TrimEnd();
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            if (Main.LocalPlayer.talkNPC != -1 && item.value > 0 && item.isAShopItem) {
                int index = tooltips.FindIndex(line => line.Mod == "Terraria" && line.Name == "Price");
                if (index >= 0) {
                    string priceDiff = Test(item, out Color color);
                    string start = Language.GetTextValue($"Mods.SavingsTooltip.{(color == Colors.RarityGreen ? "Discounted" : "Overpriced")}");
                    var line = new TooltipLine(Mod, "SavingsTooltip", $"[i:{ItemID.SilverCoin}]{start}: {priceDiff}") {
                        OverrideColor = color
                    };
                    tooltips.Insert(index + 1, line);
                }
            }
        }
    }
}
