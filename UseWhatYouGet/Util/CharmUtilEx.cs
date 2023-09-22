using Osmi.Game;

namespace UseWhatYouGet.Util {
	/// <summary>
	/// "Extensions" on <see cref="CharmUtil"/>'s functionality. Primarily for making
	/// equipping 0-notch charms possible even when all slots are filled.
	/// </summary>
	internal static class CharmUtilEx {

		private static PlayerData PD => PlayerData.instance;
		private static int Cost(int charm) => CharmUtil.GetCharmCost(charm);
		private static int CharmSlots => PD.GetInt(nameof(PD.charmSlots));
		private static int CharmSlotsFilled => PD.GetInt(nameof(PD.charmSlotsFilled));

		/// <summary>
		/// Checks if a charm can be equipped. Either it's a 0-notch charm and the player
		/// is not overcharmed, OR it's any other cost and there's at least 1 notch of space.
		/// </summary>
		/// <param name="charm">Charm ID</param>
		public static bool CanEquipCharm(int charm)
			=> !CharmUtil.EquippedCharm(charm)
				&& (
					(Cost(charm) >= 1 && CharmSlotsFilled < CharmSlots)
					|| (Cost(charm) == 0 && CharmSlotsFilled <= CharmSlots)
				);

		/// <summary>
		/// Checks if a charm would cause overcharming if equipped at this time.
		/// </summary>
		/// <param name="charm">Charm ID</param>
		public static bool WouldOvercharm(int charm)
			=> CharmSlotsFilled + Cost(charm) > CharmSlots;

		/// <summary>
		/// Equip a charm if either: it's a 0-notch charm and the player is not
		/// overcharmed, OR it's any other cost and there's at least 1 notch of space.
		/// </summary>
		/// <param name="charm">Charm ID</param>
		/// <returns>Whether the operation is successful or not</returns>
		/// <remarks>The caller should call <see cref="CharmUtil.UpdateCharm"/> after changes to charms</remarks>
		public static bool EquipCharm(int charm) {
			if (!CanEquipCharm(charm)) return false;

			PD.SetInt(nameof(PD.charmSlotsFilled), CharmSlotsFilled + Cost(charm));

			if (CharmSlotsFilled > CharmSlots) {
				PD.SetBool(nameof(PD.canOvercharm), true);
				PD.SetBool(nameof(PD.overcharmed), true);
			}

			PD.SetBool($"equippedCharm_{charm}", true);
			PD.EquipCharm(charm);

			return true;
		}

	}
}
