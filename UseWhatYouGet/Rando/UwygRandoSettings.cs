using MenuChanger.Attributes;

namespace UseWhatYouGet.Rando {

    public class UwygRandoSettings {
		
		private const int MAX_EXTRAS = 5;

		[MenuLabel("Enable Connection")]
		public bool Enabled { get; set; } = false;

		public CharmSettings Charms { get; set; } = new();
		public class CharmSettings {
			public bool Enabled { get; set; } = false;

			[MenuIgnore]
			[MenuLabel("Allow Overcharm")]
			public bool AllowOvercharm { get; set; } = false;

			[MenuIgnore]
			[MenuLabel("Keep Notches Full")]
			public bool KeepFull { get; set; } = false;
		}

		[MenuIgnore]
		public SpellSettings Spells { get; set; } = new();
		public class SpellSettings {
			public bool Enabled { get; set; } = false;

			[MenuIgnore]
			[MenuLabel("Extra Fireballs")]
			[MenuRange(0, MAX_EXTRAS)]
			public int ExtraFireballs { get; set; } = 0;

			[MenuIgnore]
			[MenuLabel("Extra Screams")]
			[MenuRange(0, MAX_EXTRAS)]
			public int ExtraScreams { get; set; } = 0;

			[MenuIgnore]
			[MenuLabel("Extra Dives")]
			[MenuRange(0, MAX_EXTRAS)]
			public int ExtraDives { get; set; } = 0;
		}

		[MenuIgnore]
		public NartSettings Narts { get; set; } = new();
		public class NartSettings {
			public bool Enabled { get; set; } = false;

			[MenuIgnore]
			[MenuLabel("Extra Cyclone Slashes")]
			[MenuRange(0, MAX_EXTRAS)]
			public int ExtraCyclones { get; set; } = 0;

			[MenuIgnore]
			[MenuLabel("Extra Great Slashes")]
			[MenuRange(0, MAX_EXTRAS)]
			public int ExtraGreats { get; set; } = 0;

			[MenuIgnore]
			[MenuLabel("Extra Dash Slashes")]
			[MenuRange(0, MAX_EXTRAS)]
			public int ExtraDashes { get; set; } = 0;
		}

		[MenuIgnore]
		public NailDirSettings NailDirs { get; set; } = new();
		public class NailDirSettings {
			public bool Enabled { get; set; } = false;

			[MenuIgnore]
			[MenuLabel("Extra Lefts")]
			[MenuRange(0, MAX_EXTRAS)]
			public int ExtraLefts { get; set; } = 0;

			[MenuIgnore]
			[MenuLabel("Extra Rights")]
			[MenuRange(0, MAX_EXTRAS)]
			public int ExtraRights { get; set; } = 0;

			[MenuIgnore]
			[MenuLabel("Extra Downs")]
			[MenuRange(0, MAX_EXTRAS)]
			public int ExtraUps { get; set; } = 0;
		}
	}
}
