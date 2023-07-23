using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SantaStateManager : MonoBehaviour
{
    public NavMeshAgent agent;

    public CrossbowController crossbowCtrl;

    [Header("Player")]
    public Transform player;

    [Header("Movement")]
    public float rotSpeed = 140;
    Quaternion rotTarget;

    [Header("Orientation Targets")]
    public Transform viewTarget;
    Vector3 rotationTarget;

    [Header("States")]
    SantaBaseState currentState;
    public SantaDrawState drawState = new SantaDrawState();
    public SantaReleaseState releaseState = new SantaReleaseState();

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        currentState = drawState;
        currentState.StartState(this);
    }

    void Update()
    {
        currentState.UpdateState(this);
        RotateTo(rotationTarget);
    }

    public void Reset() {
        currentState = drawState;
        currentState.StartState(this);
    }

    public void SwitchState(SantaBaseState state) {
        currentState = state;
        state.StartState(this);
    }

    public void SetViewTargetTo(Vector3 newTarget) {
        viewTarget.position = newTarget;
    }

    public void SetRotationTargetTo(Vector3 newTarget) {
        rotationTarget = newTarget;
    }

    public void SetAgentSpeed(float newSpeed) {
        agent.speed = newSpeed;
    }

    public void SetAgentDestination(Vector3 newTarget) {
        agent.SetDestination(newTarget);
    } 

    void RotateTo(Vector3 target) {
        rotTarget = Quaternion.LookRotation(target - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotTarget, rotSpeed * Time.deltaTime);
    }
}
