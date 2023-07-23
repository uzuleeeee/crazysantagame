using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SantaPlayerDetection))]
public class SantaPlayerDetectionEditor : Editor {
    void OnSceneGUI() {
        // Get detection data
        SantaPlayerDetection data = (SantaPlayerDetection)target;

        // Get left and right ends of fov
        Vector3 leftEndDir = data.DirFromAngle(-data.fov / 2, false);
        Vector3 rightEndDir = data.DirFromAngle(data.fov / 2, false);

        // Display the fov
        Handles.color = Color.yellow;
        Handles.DrawWireArc(data.transform.position, Vector3.up, leftEndDir, data.fov, data.maxSightDist);
        Handles.DrawLine(data.transform.position, data.transform.position + leftEndDir * data.maxSightDist);
        Handles.DrawLine(data.transform.position, data.transform.position + rightEndDir * data.maxSightDist);

        // Display the hearing regions
        Handles.color = Color.green;
        Handles.DrawWireArc(data.transform.position, Vector3.up, Vector3.forward, 360, data.maxCrouchDist);
        Handles.DrawWireArc(data.transform.position, Vector3.up, Vector3.forward, 360, data.maxWalkDist);
        Handles.DrawWireArc(data.transform.position, Vector3.up, Vector3.forward, 360, data.maxRunDist);
    
        // Display line to the last known location of player
        Handles.color = Color.red;
        Handles.DrawLine(data.transform.position, data.lastKnownLoc.position);
        Handles.DrawWireDisc(data.lastKnownLoc.position, Vector3.up, 0.5f);
    }
}