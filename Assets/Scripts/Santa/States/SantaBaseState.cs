using UnityEngine;

public abstract class SantaBaseState
{
    public abstract void StartState(SantaStateManager stateManager);

    public abstract void UpdateState(SantaStateManager stateManager);
}
