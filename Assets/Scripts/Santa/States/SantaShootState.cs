using UnityEngine;

public class SantaShootState : SantaBaseState
{
    Vector3 target;
    float time, interpolationPeriod;

    public override void StartState(SantaStateManager stateManager) {
        stateManager.SetAgentSpeed(0);
    }

    public override void UpdateState(SantaStateManager stateManager) {
        target = stateManager.ragdoll.position;
        stateManager.SetViewTargetTo(target);
        stateManager.SetRotationTargetTo(target);
        stateManager.SetAgentDestination(target);

        stateManager.crossbowCtrl.Release();
        
        if (stateManager.crossbowCtrl.IsReadyToDraw()) {
            stateManager.SwitchState(stateManager.drawState);
        }
    }
}
