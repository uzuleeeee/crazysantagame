using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseController : MonoBehaviour
{
    public GameObject currentHouse;
    public Transform currentSanta;

    HouseSwitcher houseSwitcher;
    public HousePhysicsController housePhysicsCon;
    HouseAppearanceController houseAppearanceCon;
    SiblingController siblingCon;
    public SantaController santaCon;

    void Start() {
        houseSwitcher = GetComponent<HouseSwitcher>();

        houseSwitcher.CreateHouse();
        currentHouse = houseSwitcher.currentHouse;
        GetControllers();
    }

    void Update() {
        
    }

    public void Switch(float delay) {
        Invoke("ActuallySwitch", delay);
    }

    void ActuallySwitch() {
        houseSwitcher.Switch();
        currentHouse = houseSwitcher.currentHouse;
        GetControllers();
    }

    public void EnableHouse(float delay) {
        housePhysicsCon.EnableHouse();
        houseAppearanceCon.TurnBlack();
        siblingCon.ActivateSibling(delay);
        santaCon.Enable(1f);
    }

    void GetControllers() {
        Debug.Log("Get controalsdflasdfawefeasfsad");
        housePhysicsCon = currentHouse.GetComponentInChildren<HousePhysicsController>();
        houseAppearanceCon = currentHouse.GetComponentInChildren<HouseAppearanceController>();
        siblingCon = currentHouse.GetComponentInChildren<SiblingController>();
        santaCon = currentHouse.transform.GetChild(2).GetComponent<SantaController>();
        currentSanta = santaCon.transform;
    }
}
