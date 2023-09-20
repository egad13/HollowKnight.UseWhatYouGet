using ItemChanger;
using Modding;
using Newtonsoft.Json;
using Osmi.Game;
using System;
using System.Reflection;
using UseWhatYouGet.Settings;
using IC_CharmItem = ItemChanger.Items.CharmItem;

namespace UseWhatYouGet {

	public class UseWhatYouGetMod : Mod, ILocalSettings<LocalSettings>, IGlobalSettings<GlobalSettings> {
		private static UseWhatYouGetMod? _instance;

		internal static UseWhatYouGetMod Instance {
			get {
				if (_instance == null) {
					throw new InvalidOperationException($"An instance of {nameof(UseWhatYouGetMod)} was never constructed");
				}
				return _instance;
			}
		}

		private static readonly string[] dependencies = new string[] {
			"Randomizer 4",
			"Osmi"
		};

		public override string GetVersion() => GetType().Assembly.GetName().Version.ToString();

		public static LocalSettings LS = new();
		public void OnLoadLocal(LocalSettings ls) => LS = ls;
		public LocalSettings OnSaveLocal() => LS;

		public static GlobalSettings GS = new();
		public void OnLoadGlobal(GlobalSettings gs) => GS = gs;
		public GlobalSettings OnSaveGlobal() => GS;

		public UseWhatYouGetMod() : base("UseWhatYouGet") {
			_instance = this;
		}

		public override void Initialize() {
			Log("Initializing");

			HookIC();
			if (ModHooks.GetMod("Randomizer 4") is Mod) {
				Rando.RandoInterop.HookRandomizer();
			}

			Log("Initialized");
		}

		private void HookIC() {
			ItemChanger.Events.OnItemChangerHook += EnterGameHook;
			ItemChanger.Events.OnItemChangerUnhook += ExitGameHook;
		}

		private void EnterGameHook() {
			if (GS.RandoSettings.Enabled) {

				if (GS.RandoSettings.Charms.Enabled) {
					AbstractItem.OnGiveGlobal += AddNewCharm;
					// TODO remove user's ability to remove auto-equipped charms. treat em all like voidheart.
				}

			}
			Log("===============================================================");
			Log($"UWYG RANDO SETTINGS:\n{JsonConvert.SerializeObject(GS.RandoSettings)}");
			Log("===============================================================");
		}

		private void ExitGameHook() {
			AbstractItem.OnGiveGlobal -= AddNewCharm;
		}

		private void AutoEquip(int charmNum) {
			CharmUtil.EquipCharm(charmNum);
			LS.EquippedCharms.Add(charmNum);
			string bleh = "";
			for (int i = 0; i < LS.EquippedCharms.Count; i++) {
				bleh += $"{LS.EquippedCharms[i]}, ";
			}
			Log($"Equipment in order: [{bleh}]");
		}

		private int AutoUnequip() {
			var charmNum = LS.EquippedCharms[0];
			CharmUtil.UnequipCharm(charmNum);
			LS.EquippedCharms.Remove(charmNum);
			return charmNum;
		}

		private void AddNewCharm(ReadOnlyGiveEventArgs args) {
			if (args.Item is not IC_CharmItem) { return; }
			var pd = PlayerData.instance;
			var itm = (IC_CharmItem) args.Item;

			Log("===============================================================");
			Log($"CHARM GIVEN: name '{itm.name}' | charmNum {itm.charmNum}");
			Log($"> COST: {pd.GetInt($"charmCost_{itm.charmNum}")}");
			RecursiveLogAllProps(itm);
			Log("===============================================================");

			LS.CharmHistory.Add(itm.charmNum);

			int slotsMax = pd.GetInt("charmSlots"),
				slotsUsed = pd.GetInt("charmSlotsFilled"),
				cost = pd.GetInt($"charmCost_{itm.charmNum}");

			if (slotsMax >= cost) {
				// unequip oldest charm until there's room for the new one
				Log($"slotMax {slotsMax}, slotUsed {slotsUsed}, cost {cost}");
				while ((slotsMax - slotsUsed) < cost) {
					Log($"Unequipping {LS.EquippedCharms[0]}");
					var removed = AutoUnequip();
					slotsUsed -= pd.GetInt($"charmCost_{removed}");
				}

				AutoEquip(itm.charmNum);

				CharmUtil.UpdateCharm();
			}
		}





		// DEBUGGING ///////////////////////////////////////////////////

		public void RecursiveLogAllProps(object o, string indent = "") {
			BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
			int maxDepth = 8;

			try {
				FieldInfo[] fields = o.GetType().GetFields(flags);

				foreach (FieldInfo fi in fields) {
					var val = fi.GetValue(o);

					try {
                        Log(indent + $"Field > {fi.Name} ({fi.FieldType}) = {val}");
					}
					catch {
                        Log(indent + $"Field > couldn't print {fi.Name}");
					}

					if (indent.Length < maxDepth && ShouldRecurseInto(val)) {
						RecursiveLogAllProps(val, indent + "--");
					}

				}
			}
			catch {
				Log(indent + "Field > couldn't get fields");
			}

			try {
				PropertyInfo[] props = o.GetType().GetProperties(flags);

				foreach (PropertyInfo pi in props) {
					var val = pi.GetValue(o);

					try {
						Log(indent + $"Field > {pi.Name} ({pi.PropertyType}) = {val}");
					}
					catch {
						Log(indent + $"Field > couldn't print {pi.Name}");
					}

					if (indent.Length < maxDepth && ShouldRecurseInto(val)) {
						RecursiveLogAllProps(val, indent + "--");
					}
				}
			}
			catch {
				Log(indent + "Property > couldn't get propeties");
			}
		}

		private bool ShouldRecurseInto(object o) {
			return !(
				o is Boolean
				|| o is Byte
				|| o is SByte
				|| o is Int16
				|| o is UInt16
				|| o is Int32
				|| o is UInt32
				|| o is Int64
				|| o is UInt64
				|| o is IntPtr
				|| o is UIntPtr
				|| o is Char
				|| o is Double
				|| o is Decimal
				|| o is Single
				|| o is String
			);
		}

	}
}
