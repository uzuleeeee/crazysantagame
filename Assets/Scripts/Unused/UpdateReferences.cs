using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class UpdateReferences : MonoBehaviour
{
    public Transform secondCamPivot, cameraPosition, secCamLookAt;

    GameObject cameras;

    // Start is called before the first frame update
    void Start()
    {
        //Find(1);
        UpdateReference();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateReference() {
        //GameObject.Find("GameController").GetComponent<GameController>().playerRDCon = transform.GetComponent<PlayerRagdollController>();
        cameras = GameObject.Find("Cameras");
        cameras.GetComponent<CameraController>().secondCamPivot = secondCamPivot;
        cameras.transform.GetChild(0).GetComponent<FirstPersonCamera>().playerTransform = transform;
        cameras.transform.GetChild(1).GetComponent<CinemachineVirtualCamera>().Follow = cameraPosition;
        cameras.transform.GetChild(2).GetComponent<CinemachineVirtualCamera>().Follow = cameraPosition;
        cameras.transform.GetChild(2).GetComponent<CinemachineVirtualCamera>().LookAt = secCamLookAt;
        GameObject.Find("Santa").GetComponent<SantaStateManager>().player = transform;
        //GameObject.Find("Crossbow").GetComponent<
    }
}
