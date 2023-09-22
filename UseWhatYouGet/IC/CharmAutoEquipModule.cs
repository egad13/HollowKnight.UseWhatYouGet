using ItemChanger.Modules;
using Modding;
using Osmi.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using UseWhatYouGet.Rando;
using UseWhatYouGet.Util;

namespace UseWhatYouGet.IC {
	public class CharmAutoEquipModule : Module {

		private static PlayerData PD => PlayerData.instance;
		private static UwygRandoSettings.CharmSettings Settings => UseWhatYouGetMod.GS.RandoSettings.Charms;
		private static Settings.LocalSettings SaveData => UseWhatYouGetMod.LS;

		public override void Initialize() {
			ModHooks.SetPlayerBoolHook += CharmGiven;
			if (Settings.KeepFull) {
				On.PlayerData.IncrementInt += NotchIncrementedHook;
				On.PlayerData.DecrementInt += NotchDecrementedHook;
				On.PlayerData.SetInt += NotchSetHook;
			}
		}

		public override void Unload() {
			ModHooks.SetPlayerBoolHook -= CharmGiven;
			On.PlayerData.IncrementInt -= NotchIncrementedHook;
			On.PlayerData.DecrementInt -= NotchDecrementedHook;
			On.PlayerData.SetInt -= NotchSetHook;
		}

		// HOOKS

		private static bool CharmGiven(string boolName, bool value) {
			if (!boolName.StartsWith("gotCharm") || !value) return value;
			int charmNum = int.Parse(boolName.Split('_')[1]);

			SaveData.CharmHistory.Add(charmNum);

			List<int> currentCharms = PD.GetVariable<List<int>>(nameof(PD.equippedCharms));

			int slotsMax = PD.GetInt(nameof(PD.charmSlots)),
				slotsUsed = PD.GetInt(nameof(PD.charmSlotsFilled)),
				cost =
					Settings.AllowOvercharm ? Math.Min(1, CharmUtil.GetCharmCost(charmNum))
					: CharmUtil.GetCharmCost(charmNum);

			// Unequip oldest charm until there's room for a new one
			while ((slotsMax - slotsUsed) < cost && currentCharms.Count > 0) {
				var removedNum = currentCharms[0];
				CharmUtil.UnequipCharm(removedNum);
				slotsUsed -= CharmUtil.GetCharmCost(removedNum);
			}

			// Equip new charm (if possible)
			if (cost <= slotsMax) CharmUtilEx.EquipCharm(charmNum);
			CharmUtil.UpdateCharm();
			return value;
		}

		private static void NotchIncrementedHook(On.PlayerData.orig_IncrementInt orig, PlayerData pd, string intName) {
			orig(pd, intName);
			if (intName == nameof(pd.charmSlots)) ReEquipCharms();
		}
		private static void NotchDecrementedHook(On.PlayerData.orig_DecrementInt orig, PlayerData pd, string intName) {
			orig(pd, intName);
			if (intName == nameof(pd.charmSlots)) ReEquipCharms();
		}
		private static void NotchSetHook(On.PlayerData.orig_SetInt orig, PlayerData pd, string intName, int value) {
			orig(pd, intName, value);
			if (intName == nameof(pd.charmSlots)) ReEquipCharms();
		}

		private static void ReEquipCharms() {
			// Rebuild the equipped charms list from the charm history into a full loadout.
			// This makes ordering of the charms in the UI follow the pick up history.
			var history = SaveData.CharmHistory.AsEnumerable().Reverse();
			bool succeeded = true;

			CharmUtil.UnequipAllCharms();
			foreach(int charm in history) {
				if (!succeeded || !CharmUtilEx.CanEquipCharm(charm)
					|| (!Settings.AllowOvercharm && CharmUtilEx.WouldOvercharm(charm))) {
					break;
				}
				succeeded = CharmUtilEx.EquipCharm(charm);
			}
			PD.GetVariable<List<int>>(nameof(PD.equippedCharms)).Reverse();
			CharmUtil.UpdateCharm();
		}

	}
}
