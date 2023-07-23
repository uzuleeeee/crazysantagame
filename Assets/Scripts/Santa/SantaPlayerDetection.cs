using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SantaPlayerDetection : MonoBehaviour
{
    [Range(0, 360)]
    public float fov;

    public LayerMask obstacleMask;

    float distToPlayer;

    public float maxCrouchDist = 4;
    public float maxWalkDist = 8;
    public float maxRunDist = 15;
    public float maxSightDist = 25;
    public Transform player;
    public PlayerController playerController;

    public Transform lastKnownLoc;

    void Start()
    {
        
    }

    void Update() {
        
    }

    void FixedUpdate()
    {
        distToPlayer = Vector3.Distance(transform.position, player.position);
        LookForPlayer();
        HearForPlayer();
    }

    void FindPlayer() {
        if (!LookForPlayer()) {
            HearForPlayer();
        }
    }

    bool LookForPlayer() {
        if (distToPlayer < maxSightDist) {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < fov / 2) {
                if (!Physics.Raycast(transform.position, dirToPlayer, distToPlayer, obstacleMask)) {
                    lastKnownLoc.position = player.position;
                    return true;
                }
            }
        }
        return false;
    }

    void HearForPlayer() {
        if (playerController.horizontalInput + playerController.verticalInput != 0) {
            if (distToPlayer < maxRunDist && distToPlayer > maxWalkDist) {
                if (playerController.state == PlayerController.MovementState.running) {
                    lastKnownLoc.position = player.position;
                }
            } else if (distToPlayer < maxWalkDist && distToPlayer > maxCrouchDist) {
                if (playerController.state == PlayerController.MovementState.running || 
                    playerController.state == PlayerController.MovementState.walking) {
                    lastKnownLoc.position = player.position;
                }
            } else if (distToPlayer < maxCrouchDist) {
                if (playerController.state == PlayerController.MovementState.running || 
                    playerController.state == PlayerController.MovementState.walking || 
                    playerController.state == PlayerController.MovementState.crouching) {
                    lastKnownLoc.position = player.position;
                }
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
        if (!angleIsGlobal) {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
