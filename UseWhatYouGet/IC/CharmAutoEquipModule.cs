using ItemChanger.Modules;
using Modding;
using Osmi.Game;
using System.Collections.Generic;
using System.Linq;
using UseWhatYouGet.Rando;

namespace UseWhatYouGet.IC {
	public class CharmAutoEquipModule : Module {

		private static UwygRandoSettings.CharmSettings Settings => UseWhatYouGetMod.GS.RandoSettings.Charms;
		private static Settings.LocalSettings SaveData => UseWhatYouGetMod.LS;

		public override void Initialize() {
			ModHooks.SetPlayerBoolHook += CharmGiven;
			if (Settings.KeepFull) {
				ModHooks.SetPlayerBoolHook += NotchGiven;
			}
		}

		public override void Unload() {
			ModHooks.SetPlayerBoolHook -= CharmGiven;
			ModHooks.SetPlayerBoolHook -= NotchGiven;
		}

		// HOOKS

		private static bool CharmGiven(string boolName, bool value) {
			if (!boolName.StartsWith("gotCharm") || !value) { return value; }

			var pd = PlayerData.instance;
			int charmNum = int.Parse(boolName.Split('_')[1]);

			SaveData.CharmHistory.Add(charmNum);

			List<int> currentCharms = pd.GetVariable<List<int>>("equippedCharms");

			int slotsMax = pd.GetInt("charmSlots"),
				slotsUsed = pd.GetInt("charmSlotsFilled"),
				cost = Settings.AllowOvercharm ? 1 : CharmUtil.GetCharmCost(charmNum);

			// Unequip oldest charm until there's room for a new one
			while ((slotsMax - slotsUsed) < cost && currentCharms.Count > 0) {
				var removedNum = currentCharms[0];
				CharmUtil.UnequipCharm(removedNum);
				slotsUsed -= CharmUtil.GetCharmCost(removedNum);
			}

			// Equip new charm (if possible)
			if (cost <= slotsMax) {
				CharmUtil.EquipCharm(charmNum);
			}

			CharmUtil.UpdateCharm();
			return value;
		}

		private static bool NotchGiven(string boolName, bool value) {
			if (!boolName.StartsWith("notch") && !boolName.StartsWith("salubraNotch")) { return value; }
			var pd = PlayerData.instance;

			// Rebuild the equipped charms list from the charm history, as full as possible
			int[] reversedHistory = SaveData.CharmHistory.Skip(0).Reverse().ToArray();
			int slotsMax = pd.GetInt("charmSlots"),
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
			List<int> newCharmList = reversedHistory.Take(index).Reverse().ToList();
			List<int> currentCharms = pd.GetVariable<List<int>>("equippedCharms");

			if (currentCharms.Equals(newCharmList)) { return value; }

			CharmUtil.UnequipAllCharms();
			CharmUtil.EquipCharms(newCharmList.ToArray());
			CharmUtil.UpdateCharm();
			return value;

			// It's done like this to preserve the ordering of the charms in the UI,
			// which makes it easier to understand what's happening.
		}

	}
}
