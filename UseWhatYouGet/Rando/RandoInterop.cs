using ItemChanger;
using Newtonsoft.Json;
using RandomizerMod.Logging;
using RandomizerMod.RandomizerData;
using RandomizerMod.RC;
using System.IO;
using UseWhatYouGet.IC;

namespace UseWhatYouGet.Rando {
	internal static class RandoInterop {
		public static UwygRandoSettings Settings => UseWhatYouGetMod.GS.RandoSettings;

		public static void HookRandomizer() {
			ConnectionMenu.Hook();
			SettingsLog.AfterLogSettings += AddUwygSettings;
			RandoController.OnExportCompleted += OnExportCompleted;
		}

		private static void AddUwygSettings(LogArguments arguments, TextWriter writer) {
			writer.WriteLine("Use What You Get Settings:");
			using JsonTextWriter jtw = new(writer) { CloseOutput = false };
			JsonUtil._js.Serialize(jtw, Settings);
			writer.WriteLine();
		}

		private static void OnExportCompleted(RandoController controller) {
			if (!Settings.Enabled) { return; }

			if (Settings.Charms.Enabled) {
				ItemChangerMod.Modules.GetOrAdd<CharmAutoEquipModule>();
				ItemChangerMod.Modules.GetOrAdd<CharmPreventEquipModule>();
			}
		}
	}
}
