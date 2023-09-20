using Newtonsoft.Json;
using RandomizerMod.Logging;
using RandomizerMod.RandomizerData;
using System.IO;

namespace UseWhatYouGet.Rando {
	internal static class RandoInterop {
		public static UwygRandoSettings Settings => UseWhatYouGetMod.GS.RandoSettings;

		public static void HookRandomizer() {
			ConnectionMenu.Hook();
			SettingsLog.AfterLogSettings += AddUwygSettings;
		}

		private static void AddUwygSettings(LogArguments arguments, TextWriter writer) {
			writer.WriteLine("Use What You Get Settings:");
			using JsonTextWriter jtw = new(writer) { CloseOutput = false };
			JsonUtil._js.Serialize(jtw, Settings);
			writer.WriteLine();
		}
	}
}
