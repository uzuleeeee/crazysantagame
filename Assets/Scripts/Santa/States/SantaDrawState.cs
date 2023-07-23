using UnityEngine;

public class SantaDrawState : SantaBaseState
{
    Vector3 target;

    public override void StartState(SantaStateManager stateManager) {
        stateManager.SetAgentSpeed(0);
        stateManager.crossbowCtrl.Draw();
    }

    public override void UpdateState(SantaStateManager stateManager) {
        target = stateManager.player.position;
        stateManager.SetViewTargetTo(target);

        if (stateManager.crossbowCtrl.IsMoving()) {
            stateManager.SetRotationTargetTo(target);
        }

        if (stateManager.crossbowCtrl.IsReadyToRelease()) {
            stateManager.SwitchState(stateManager.releaseState);
        }
    }
}
