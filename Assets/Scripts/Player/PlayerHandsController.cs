using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandsController : MonoBehaviour
{
    HouseController houseCon;

    AudioManager am;

    Animator anim;
    bool hasWeapon;

    Camera cam;

    public Transform weaponLoc, weaponThrow;
    LayerMask weaponLayerMask;
    Renderer weaponRenderer;
    public GameObject weapon, weaponChild;
    Rigidbody weaponRb;

    public GameObject leftHand, rightHand;

    int inHandLayer, inMotionLayer, weaponLayer;
    int isRunningHash, hasWeaponHash, punchNumHash, punchHash, throwHash;

    int punchNum;

    bool justThrew = false;

    int lastMatIndex;

    void Start()
    {
        am = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
        houseCon = GameObject.FindWithTag("World").GetComponent<HouseController>();

        anim = GetComponent<Animator>();
        cam = Camera.main;

        hasWeapon = false;

        weaponLayerMask = LayerMask.GetMask("Weapon");

        inHandLayer = LayerMask.NameToLayer("In Hand");
        inMotionLayer = LayerMask.NameToLayer("In Motion");
        weaponLayer = LayerMask.NameToLayer("Weapon");

        isRunningHash = Animator.StringToHash("isRunning");
        hasWeaponHash = Animator.StringToHash("hasWeapon");
        punchNumHash = Animator.StringToHash("punchNum");
        punchHash = Animator.StringToHash("punch");
        throwHash = Animator.StringToHash("throw");
    }

    void Update()
    {
        hasWeapon = weapon != null;

        anim.SetBool(hasWeaponHash, hasWeapon);
        anim.SetBool(isRunningHash, Input.GetKey("w"));

        if (!justThrew) {
            Ray rayy = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitt;

            if (Physics.Raycast(rayy, out hitt, 4, weaponLayerMask.value)) {
                if (weaponChild != null) {
                    if (hitt.transform.GetChild(0).gameObject.GetInstanceID() != weaponChild.GetInstanceID()) {
                        weaponRenderer.materials[lastMatIndex].SetFloat("_Outline", 0);
                    }
                }

                if (!hasWeapon) {
                    weaponChild = hitt.transform.GetChild(0).gameObject;
                    weaponRenderer = weaponChild.GetComponent<Renderer>();
                    lastMatIndex = weaponRenderer.materials.Length - 1;
                    weaponRenderer.materials[lastMatIndex]?.SetFloat("_Outline", 1);
                }
            } else {
                if (weaponRenderer != null) weaponRenderer.materials[lastMatIndex].SetFloat("_Outline", 0);
            }
        }

        if (Input.GetMouseButtonDown(0)) {
            am.Play("PlayerSwing");
            if (!hasWeapon) {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 4, weaponLayerMask.value)) {
                    weapon = hit.transform.gameObject;
                    weaponChild = weapon.transform.GetChild(0).gameObject;

                    weaponRb = weapon.GetComponent<Rigidbody>();

                    weapon.transform.parent = weaponLoc;
                    weapon.transform.localPosition = new Vector3(0, 0, 0);
                    weapon.transform.localRotation = Quaternion.Euler(new Vector3(0, 10, 0));

                    weaponRb.isKinematic = true;

                    weaponChild.layer = inHandLayer;

                    anim.SetInteger(punchNumHash, 1);
                    hasWeapon = true;
                } else {
                    punchNum = Random.Range(2, 4);
                    anim.SetInteger(punchNumHash, punchNum);
                }
            }
            anim.SetBool(punchHash, true);
        }
        if (Input.GetMouseButtonUp(0)) {
            anim.SetBool(punchHash, false);
        }
        if (Input.GetMouseButtonDown(1) && hasWeapon) {
            //Debug.Log("player threw ------------------------");
            anim.SetBool(throwHash, true);
        }
    }
    
    public void Throw() {
        am.Play("PlayerThrow");

        weapon.transform.parent = houseCon.currentHouse.transform;
        weaponRb.isKinematic = false;
        weaponChild.layer = weaponLayer;
        weapon.transform.position = weaponThrow.position;
        weaponRb.AddForce(cam.transform.forward * 40f, ForceMode.Impulse);
        hasWeapon = false;
        weapon = null;
        anim.SetBool(throwHash, false);
        justThrew = true;
        Invoke(nameof(ResetJustThrew), 0.5f);
    }

    public void LetGoOfWeapon() {
        if (hasWeapon) {
            weapon.transform.parent = houseCon.currentHouse.transform;
            weaponRb.isKinematic = false;
            weaponChild.layer = weaponLayer;
            weapon.transform.position = weaponThrow.position;
            hasWeapon = false;
            weapon = null;
        }
    }

    public void InMotionOn() {
        if (weapon != null) {
            weaponChild.layer = inMotionLayer;
        } else {
            if (punchNum == 2) {
                leftHand.layer = inMotionLayer;
            } else if (punchNum == 3) {
                rightHand.layer = inMotionLayer;
            }
        }
    }

    public void InMotionOff() {
        if (weapon != null) {
            weaponChild.layer = inHandLayer; 
        } else {
            leftHand.layer = rightHand.layer = inHandLayer;
        }
    }

    void ResetJustThrew() {
        justThrew = false;
    }
}
