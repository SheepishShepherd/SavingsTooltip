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
		public string CalculateDiscount(Item item, out int priceDiff, out bool isDiscounted) {
			Main.LocalPlayer.GetItemExpectedPrice(item, out _, out long calcForBuying); // the price the player pays
			priceDiff = item.GetStoreValue() - (int)calcForBuying; // the price difference of the shop item value and the player buy price
			isDiscounted = true; // default to a discount (or retail value)

			if (priceDiff == 0) {
				return Language.GetTextValue($"Mods.SavingsTooltip.NoDifference");
			}
			else {
				if (priceDiff < 0) {
					priceDiff *= -1; // priceDiff must be displayed as a postive value
					isDiscounted = false; // if the priceDiff was negative before, it becomes a markup
				}
				return Language.GetTextValue($"Mods.SavingsTooltip.{(isDiscounted ? "Off" : "Markup")}", ((float)priceDiff / (float)item.GetStoreValue() * 100).ToString("0"));
			}
		}

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
			if (Main.LocalPlayer.talkNPC != -1 && item.value > 0 && item.isAShopItem) {
				int index = tooltips.FindIndex(line => line.Mod == "Terraria" && line.Name == "Price");
				if (index >= 0) {
					string priceDiff = CalculateDiscount(item, out int coins, out bool discounted);
					if (coins > 0)
						priceDiff += $" ({Main.ValueToCoins(coins)})";

					var line = new TooltipLine(Mod, "SavingsTooltip", $"[i:{ItemID.CopperCoin}] {priceDiff}") {
						OverrideColor = discounted ? Colors.RarityGreen : Colors.RarityRed
					};
					tooltips.Insert(index + 1, line);
				}
			}
		}
    }
}
