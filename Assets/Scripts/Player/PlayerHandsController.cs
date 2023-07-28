using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandsController : MonoBehaviour
{
    HouseController houseCon;
    PlayerMovementController playerMovementCon;

    AudioManager am;

    Animator anim;
    bool hasWeapon;

    Camera cam;

    public Transform weaponLoc, weaponThrow;
    LayerMask weaponLayerMask;
    Renderer weaponRenderer;
    public GameObject weapon, weaponChild;
    Rigidbody weaponRb;
    WeaponController weaponCon;
    float weaponSpeed = 1;

    public GameObject leftHand, rightHand;
    public Transform leftHandIK, rightHandIK;

    int inHandLayer, inMotionLayer, weaponLayer;
    int isRunningHash, hasWeaponHash, punchNumHash, punchHash, throwHash, isLatchingHash, speedHash;

    int punchNum;

    bool justThrew = false;

    int lastMatIndex;

    bool isLatching;
    public float armWidth = 0.5f;
    Vector3 latchRayStart;
    LayerMask latchLayer;
    public float latchRaycastLength = 3;
    bool foundLatchPosition = false;
    RaycastHit leftHit, rightHit;
    Vector3 leftHandPos, rightHandPos;

    void Start()
    {
        am = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
        houseCon = GameObject.FindWithTag("World").GetComponent<HouseController>();

        playerMovementCon = GetComponent<PlayerMovementController>();
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
        isLatchingHash = Animator.StringToHash("isLatching");
        speedHash = Animator.StringToHash("speed");

        latchRayStart = playerMovementCon.latchBottomRaycast;
        latchLayer = playerMovementCon.latchLayer;
    }

    void Update()
    {
        isLatching = playerMovementCon.GetIsLatching();
        anim.SetBool(isLatchingHash, isLatching);

        if (!isLatching) {
            foundLatchPosition = false;
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
                    anim.SetFloat(speedHash, 1);

                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 4, weaponLayerMask.value)) {
                        weapon = hit.transform.gameObject;
                        weaponChild = weapon.transform.GetChild(0).gameObject;
                        weaponCon = weaponChild.GetComponent<WeaponController>();
                        weaponSpeed = weaponCon.GetSpeed();

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
                } else {
                    anim.SetFloat(speedHash, weaponSpeed);
                }
                anim.SetBool(punchHash, true);
            }
            if (Input.GetMouseButtonUp(0)) {
                anim.SetBool(punchHash, false);
            }
            if (Input.GetMouseButtonDown(1) && hasWeapon) {
                anim.SetBool(throwHash, true);
            }
        } else {
            Debug.Log("Latchin ------------");
            anim.SetBool(isRunningHash, false);
            anim.SetBool(punchHash, false);
            anim.SetBool(throwHash, false);

            Physics.Raycast(transform.position + latchRayStart - transform.right * armWidth, transform.forward, out leftHit, latchRaycastLength, latchLayer);
            Physics.Raycast(transform.position + latchRayStart + transform.right * armWidth, transform.forward, out rightHit, latchRaycastLength, latchLayer);

            if (!foundLatchPosition) {
                leftHandPos = leftHit.point;
                rightHandPos = rightHit.point;
                foundLatchPosition = true;
            }

            if (foundLatchPosition) {
                leftHandIK.position = leftHandPos;
                rightHandIK.position = rightHandPos;
            }
        }
    }
    
    public void Throw() {
        am.Play("PlayerThrow");

        anim.SetFloat(speedHash, weaponSpeed);
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
