using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseController : MonoBehaviour
{
    public GameObject currentHouse;

    HouseSwitcher houseSwitcher;
    public HousePhysicsController housePhysicsCon;
    HouseAppearanceController houseAppearanceCon;
    SiblingController siblingCon;

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
    }

    void GetControllers() {
        housePhysicsCon = currentHouse.GetComponentInChildren<HousePhysicsController>();
        houseAppearanceCon = currentHouse.GetComponentInChildren<HouseAppearanceController>();
        siblingCon = currentHouse.GetComponentInChildren<SiblingController>();
    }
}
