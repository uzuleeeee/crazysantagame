using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseSwitcher : MonoBehaviour
{
    public float transitionSpeed;
    public AnimationCurve transitionCurve;
    float transitionCurrent, transitionTarget;
    Quaternion rotCurrent, rotTarget;
    float angle;

    int currentHouseNum = -1;

    // Reset real house
    public GameObject house;
    public GameObject currentHouse;

    // Start is called before the first frame update
    void Start()
    {
        rotCurrent = rotTarget = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transitionCurrent = Mathf.MoveTowards(transitionCurrent, transitionTarget, transitionSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(rotCurrent, rotTarget, transitionCurve.Evaluate(transitionCurrent));
    }

    public void Switch() {
        RotateEarth();
        CreateHouse();
    }

    public void CreateHouse() {
        currentHouseNum++;
        Destroy(currentHouse, 5);

        currentHouse = Instantiate(house, Vector3.zero, Quaternion.Euler(Vector3.zero));
        currentHouse.transform.parent = transform;
        currentHouse.transform.localPosition = Vector3.zero;
        currentHouse.transform.localRotation = Quaternion.Euler(new Vector3(currentHouseNum * 45, 0, 0));
    }

    void RotateEarth() {
        angle -= 45;
        transitionCurrent = 0;
        transitionTarget = 1;
        rotCurrent = Quaternion.Euler(transform.localEulerAngles);
        rotTarget = Quaternion.Euler(angle, 0, 0);
    }
}