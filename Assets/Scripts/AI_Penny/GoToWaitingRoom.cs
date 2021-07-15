public class GoToWaitingRoom : GAction {
    public override bool PrePerform() {

        return true;
    }

    public override bool PostPerform() {

        // Inject waiting state to world states
        GWorld.Instance.GetWorldStatesHandle().ModifyState("Waiting", 1);
        // Character_Tier1 adds himself to the queue
        GWorld.Instance.GetQueue("Character_Tier1").AddResource(this.gameObject);
        // Inject a state into the agents beliefs
        beliefStateHandle.ModifyState("atHospital", 1);

        return true;
    }
}
