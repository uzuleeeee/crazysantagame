using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    Camera cam;
    public CinemachineVirtualCamera[] cvcs;
    public CinemachineBrain brain;

    public float speed;

    LayerMask houseLayerMask;
    Renderer houseRen;

    public Transform santa;
    public Transform secondCamPivot;

    public Transform tpcPivot;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

        houseLayerMask = LayerMask.GetMask("House");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(IsComplete());

        if (secondCamPivot != null) {
            secondCamPivot.LookAt(new Vector3(santa.position.x, secondCamPivot.position.y, santa.position.z));
        }

        SpinTPC();
    }

    public bool MainMenu() {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, houseLayerMask.value)) {
            houseRen = hit.transform.gameObject.GetComponent<Renderer>();
            houseRen.materials[1].SetFloat("_Outline", 1);
            return true;
        } else {
            if (houseRen != null) {
                houseRen.materials[1].SetFloat("_Outline", 0);
            }
            return false;
        }
    }

    public void Enter() {
        SetCamTo(3, 0);
    }

    public void FPC(float delay) {
        SetCamTo(0, delay);
    }

    public void Exit() {
        SetCamTo(1, 0);
        SetCamTo(2, 0.7f);
    }

    public bool IsComplete() {
        return !brain.IsBlending;
    }

    void SpinTPC() {
        tpcPivot.Rotate(0, speed * Time.deltaTime, 0);
    }

    void ResetTPCRot() {
        tpcPivot.eulerAngles = Vector3.zero;
    }

    void SetCamTo(int c, float delay) {
        StartCoroutine(ActuallySetCam(c, delay));
    }

    IEnumerator ActuallySetCam(int c, float delay) {
        yield return new WaitForSeconds(delay);
        for (int i = 0; i < cvcs.Length; i++) {
            cvcs[i].enabled = i == c;
        }
    }
}
