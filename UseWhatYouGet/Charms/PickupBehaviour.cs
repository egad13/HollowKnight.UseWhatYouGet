using ItemChanger;
using ItemChanger.Items;
using Osmi.Game;
using System.Collections.Generic;
using System.Linq;
using UseWhatYouGet.Rando;
using IC_CharmItem = ItemChanger.Items.CharmItem;

namespace UseWhatYouGet.Charms {
	internal class PickupBehaviour {

		private static UwygRandoSettings.CharmSettings Settings => UseWhatYouGetMod.GS.RandoSettings.Charms;
		private static Settings.LocalSettings SaveData => UseWhatYouGetMod.LS;

		public static void Hook() {
			if (!Settings.Enabled) { return; }

			AbstractItem.OnGiveGlobal += CharmGiven;
			if (Settings.KeepFull) {
				AbstractItem.AfterGiveGlobal += NotchGiven;
			}
			// TODO remove user's ability to remove auto-equipped charms. treat em all like voidheart.
		}

		public static void Unhook() {
			AbstractItem.OnGiveGlobal -= CharmGiven;
			AbstractItem.AfterGiveGlobal -= NotchGiven;
		}

		// MODHOOKS

		private static void CharmGiven(ReadOnlyGiveEventArgs args) {
			if (args.Item is not IC_CharmItem) { return; }
			var pd = PlayerData.instance;
			var itm = (IC_CharmItem)args.Item;

			SaveData.CharmHistory.Add(itm.charmNum);

			int slotsMax = pd.GetInt("charmSlots"),
				slotsUsed = pd.GetInt("charmSlotsFilled"),
				cost = Settings.AllowOvercharm ? 1 : CharmUtil.GetCharmCost(itm.charmNum);

			// Unequip oldest charm until there's room for a new one
			while((slotsMax - slotsUsed) < cost && SaveData.EquippedCharms.Count > 0) {
				var removedNum = SaveData.EquippedCharms[0];
				SaveData.EquippedCharms.RemoveAt(0);
				CharmUtil.UnequipCharm(removedNum);
				slotsUsed -= CharmUtil.GetCharmCost(removedNum);
			}

			// Equip new charm (if possible)
			if (cost <= slotsMax) {
				SaveData.EquippedCharms.Add(itm.charmNum);
				CharmUtil.EquipCharm(itm.charmNum);
			}
			
			CharmUtil.UpdateCharm();
		}

		private static void NotchGiven(ReadOnlyGiveEventArgs args) {
			if (args.Item is not NotchItem) { return; }

			// Rebuild the equipped charms list from the charm history, as full as possible
			int[] reversedHistory = SaveData.CharmHistory.Skip(0).Reverse().ToArray();
			int slotsMax = PlayerData.instance.GetInt("charmSlots"),
				cost = 0,
				index = 0;

			while (cost < slotsMax && index < reversedHistory.Length) {
				cost += CharmUtil.GetCharmCost(reversedHistory[index]);
				index++;
			}
			if (Settings.AllowOvercharm && index < reversedHistory.Length && slotsMax - cost >= 1) {
				index++;
			}

			// Requip all charms IF NECESSARY.
			List<int> newCharmList  = reversedHistory.Take(index).Reverse().ToList();

			if (SaveData.EquippedCharms.Equals(newCharmList)) { return; }

			SaveData.EquippedCharms = newCharmList;
			CharmUtil.UnequipAllCharms();
			CharmUtil.EquipCharms(newCharmList.ToArray());

			// It's done like this to preserve the ordering of the charms in the UI,
			// which makes it easier to understand what's happening.
		}

	}
}
