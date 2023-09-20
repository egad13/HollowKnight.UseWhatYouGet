using MenuChanger;
using MenuChanger.Extensions;
using MenuChanger.MenuElements;
using MenuChanger.MenuPanels;
using RandomizerMod;
using RandomizerMod.Menu;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;
using static RandomizerMod.Localization;

namespace UseWhatYouGet.Rando {

	public class ConnectionMenu {

		private const int VSPACE_SMALL = 50;
		private const int VSPACE_SMALLMED = 90;
		private const int VSPACE_MED = 200;
		private const int VSPACE_LARGE = 350;
		private const int HSPACE_LARGE = 350;
		private const int HSPACE_XLARGE = 450;
		private const int HSPACE_XXLARGE = 750;

		private SmallButton pageRootButton;
		private SmallButton readmeControl;
		private MenuPage uwygPage;

		private MenuElementFactory<UwygRandoSettings> toplevelMef;
		private MenuElementFactory<UwygRandoSettings.CharmSettings> charmsMef;
		private MenuElementFactory<UwygRandoSettings.SpellSettings> spellsMef;
		private MenuElementFactory<UwygRandoSettings.NartSettings> nartsMef;
		private MenuElementFactory<UwygRandoSettings.NailDirSettings> nailDirMef;

		internal static ConnectionMenu? Instance { get; private set; }

		public static void Hook() {
			RandomizerMenuAPI.AddMenuPage(ConstructMenu, HandleButton);
			MenuChangerMod.OnExitMainMenu += () => Instance = null;
		}

		private static bool HandleButton(MenuPage landingPage, out SmallButton button) {
			button = Instance!.pageRootButton;
			return true;
		}

		private static void ConstructMenu(MenuPage connectionPage) {
			Instance = new(connectionPage);
		}

		private ConnectionMenu(MenuPage connectionPage) {
			uwygPage = new MenuPage(Localize("Use What You Get"), connectionPage);
			VerticalItemPanel baseVPanel = new(uwygPage, new Vector2(0, 400), VSPACE_MED, true);

			toplevelMef = new(uwygPage, RandoInterop.Settings);
			Localize(toplevelMef);
			charmsMef = new(uwygPage, RandoInterop.Settings.Charms);
			//spellsMef = new(uwygPage, RandoInterop.Settings.Spells);
			//nartsMef = new(uwygPage, RandoInterop.Settings.Narts);
			//nailDirMef = new(uwygPage, RandoInterop.Settings.NailDirs);

			// TOP LEVEL SETTINGS
			MenuLabel toplevelLabel = new(uwygPage, Localize("Use What You Get"));

			readmeControl = new(uwygPage, Localize("Open Readme"));
			readmeControl.OnClick += OpenReadme;

			ToggleButton enabledControl = (ToggleButton)toplevelMef.ElementLookup[nameof(UwygRandoSettings.Enabled)];
			enabledControl.SelfChanged += EnabledChanged;

			VerticalItemPanel toplevelSettingsHolder = new(uwygPage, Vector2.zero, VSPACE_SMALL * 1.2f, false, toplevelMef.Elements);
			toplevelSettingsHolder.Insert(0, toplevelLabel);
			toplevelSettingsHolder.Insert(1, readmeControl);
			baseVPanel.Add(toplevelSettingsHolder);

			// SUB-CATEGORIES
			baseVPanel.Add(
				new GridItemPanel(uwygPage, Vector2.zero, 1, VSPACE_LARGE, HSPACE_LARGE, false,
					MakeCategoryPanel("Charms", charmsMef)
					//MakeCategoryPanel("Spells", spellsMef),
					//MakeCategoryPanel("Nail Arts", nartsMef),
					//MakeCategoryPanel("Nail Directions", nailDirMef)
				)
			);

			// FINAL SETUP
			baseVPanel.ResetNavigation();
			baseVPanel.SymSetNeighbor(Neighbor.Down, uwygPage.backButton);

			pageRootButton = new(connectionPage, Localize("Use What You Get"));
			pageRootButton.AddHideAndShowEvent(connectionPage, uwygPage);
			SetTopLevelButtonColor();
		}

		// EVENT HANDLERS

		private void EnabledChanged(IValueElement obj) {
			SetTopLevelButtonColor();
		}

		private void OpenReadme() {
			string readmeFileName = Path.Combine(
				Path.GetDirectoryName(UseWhatYouGetMod.Instance.GetType().Assembly.Location),
				"README.md"
			);

			try {
				Process.Start(readmeFileName);
			}
			catch (Win32Exception ex) {
				if (ex.NativeErrorCode == 2) {
					LogHelper.LogError($"Error opening Readme: File {readmeFileName} was not found.");
				}
				else {
					LogHelper.LogError(ex.ToString());
				}
				readmeControl.Text.text = Localize("Couldn't Open Readme; See ModLog");
				readmeControl.Lock();
			}
			catch (Exception ex2) {
				LogHelper.LogError(ex2.ToString());
				readmeControl.Text.text = Localize("Couldn't Open Readme; See ModLog");
				readmeControl.Lock();
			}
		}

		// UTILS

		private void SetTopLevelButtonColor() {
			pageRootButton.Text.color = RandoInterop.Settings.Enabled ? Colors.TRUE_COLOR : Colors.DEFAULT_COLOR;
		}

		private VerticalItemPanel MakeCategoryPanel<T>(string title, MenuElementFactory<T> mef) {
			MenuLabel label = new(uwygPage, Localize(title));
			label.Text.lineSpacing = 2f;
			Localize(mef);

			VerticalItemPanel panel = new(uwygPage, Vector2.zero, VSPACE_SMALL, false,
				label,
				mef.Elements[0]
			);

			int idx = 1;

			while (idx < mef.Elements.Length && mef.Elements[idx].ValueType != typeof(int)) {
				panel.Add(mef.Elements[idx]);
				idx++;
			}
			if (idx >= mef.Elements.Length) { return panel; }

			panel.Add(new MenuLabel(uwygPage, "")); // purely for spacing purposes
			panel.Add(
				new VerticalItemPanel(uwygPage, Vector2.zero, VSPACE_SMALLMED, false,
					mef.Elements.Skip(idx).ToArray()
				)
			);
			return panel;
		}

	}

}
