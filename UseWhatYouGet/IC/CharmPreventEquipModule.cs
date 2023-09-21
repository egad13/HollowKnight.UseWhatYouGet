using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.Modules;
using System.Linq;

namespace UseWhatYouGet.IC {
	public class CharmPreventEquipModule : Module {

		public override void Initialize() {
			Events.AddFsmEdit(new("Charms", "UI Charms"), DisableCharmEquipping);
		}

		public override void Unload() {
			Events.RemoveFsmEdit(new("Charms", "UI Charms"), DisableCharmEquipping);
		}

		private void DisableCharmEquipping(PlayMakerFSM fsm) {
			FsmState idleCollection = fsm.GetState("Idle Collection"),
				idleEquipped = fsm.GetState("Idle Equipped"),
				tink = fsm.GetState("Tink");

			// Custom states that make a "tink" sound and redirect back to cursor idle states
			FsmState preventEquip = new(fsm.Fsm) { Name = "Prevent Equip" };
			FsmState preventUnequip = new(fsm.Fsm) { Name = "Prevent Unequip" };
			if (tink.Actions.FirstOrDefault(x => x is AudioPlayerOneShotSingle)
					is AudioPlayerOneShotSingle tinkSound) {
				preventEquip.AddFirstAction(tinkSound);
				preventUnequip.AddFirstAction(tinkSound);
			}
			preventEquip.AddTransition("FINISHED", idleCollection);
			preventUnequip.AddTransition("FINISHED", idleEquipped);

			// Change the transitions for the equip/unequip button to the redirecting states
			idleCollection.Transitions.FirstOrDefault(x => x.EventName == "UI CONFIRM")
				?.SetToState(preventEquip);
			idleEquipped.Transitions.FirstOrDefault(x => x.EventName == "UI CONFIRM")
				?.SetToState(preventUnequip);
		}
	}
}
