using UnityEngine;

public class SantaReleaseState : SantaBaseState
{
    Vector3 target;
    float time, interpolationPeriod;

    public override void StartState(SantaStateManager stateManager) {
        stateManager.SetAgentSpeed(2);

        time = 0;
        interpolationPeriod = Random.Range(0f, 3f);
    }

    public override void UpdateState(SantaStateManager stateManager) {
        target = stateManager.player.position;
        stateManager.SetViewTargetTo(target);
        stateManager.SetRotationTargetTo(target);
        stateManager.SetAgentDestination(target);

        time += Time.deltaTime;

        if (time >= interpolationPeriod) {
            if (stateManager.crossbowCtrl.CanSeePlayer()) {
                time = -10000000;
                stateManager.crossbowCtrl.Release();
            }
        }

        if (stateManager.crossbowCtrl.IsReadyToDraw()) {
            stateManager.SwitchState(stateManager.drawState);
        }
    }
}
