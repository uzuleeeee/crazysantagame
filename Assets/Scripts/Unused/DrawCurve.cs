using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCurve : MonoBehaviour
{
    public Transform[] controlPoints;
    Vector3 gizmoPosition;

    public Transform cube;

    [Range(0, 1)]
    public float t = 0;
    [Range(-2, 2)]
    public float speed = 1;

    void Update() {
        t = Mathf.MoveTowards(t, 1, 5 * speed * Time.deltaTime);

        cube.position = Mathf.Pow(1 - t, 3) * controlPoints[0].position + 
                3 * Mathf.Pow(1 - t, 2) * t * controlPoints[1].position + 
                3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[2].position + 
                Mathf.Pow(t, 3) * controlPoints[3].position;
    }

    void OnDrawGizmos() {
        for (float t = 0; t <= 1; t += 0.05f) {
            gizmoPosition = Mathf.Pow(1 - t, 3) * controlPoints[0].position + 
                3 * Mathf.Pow(1 - t, 2) * t * controlPoints[1].position + 
                3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[2].position + 
                Mathf.Pow(t, 3) * controlPoints[3].position;

            Gizmos.DrawSphere(gizmoPosition, 0.25f);
        }
        
        for (int i = 0; i < 4; i++) {
            Gizmos.DrawSphere(controlPoints[i].position, 0.25f);
        }

        Gizmos.DrawLine(controlPoints[0].position, controlPoints[1].position);
        Gizmos.DrawLine(controlPoints[2].position, controlPoints[3].position);
    }
}
