using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.Modules;
using Modding;
using System;
using System.Linq;

namespace UseWhatYouGet.IC {
	/// <summary>
	/// A module that causes specific doors and other events which require the player to
	/// have a specific charm equipped (ex: Floor in the abyss, Grimm fight) to only
	/// require that they've <i>obtained</i> the charm.
	/// </summary>
	public class CharmDoorsUnlockModule : Module {

		private static PlayerData PD => PlayerData.instance;

		public override void Initialize() {
			Events.AddFsmEdit(SceneNames.Abyss_06_Core, new("Blue Door", "Control"), BlueDoorRequirement);
		}

		public override void Unload() {
			Events.RemoveFsmEdit(SceneNames.Abyss_06_Core, new("Blue Door", "Control"), BlueDoorRequirement);
		}

		// FSM EDITS

		private void BlueDoorRequirement(PlayMakerFSM fsm) {
			bool hasBlueCharm =
				PD.GetBool(nameof(PD.gotCharm_8))
				|| PD.GetBool(nameof(PD.gotCharm_9))
				|| PD.GetBool(nameof(PD.gotCharm_27));

			if (!hasBlueCharm) return;

			FsmState init = fsm.GetState("Init"),
				opened = fsm.GetState("Opened");
			foreach (FsmTransition t in init.Transitions) {
				t.SetToState(opened);
			}
			PD.SetBool(nameof(PD.blueVineDoor), true);
		}

	}
}
