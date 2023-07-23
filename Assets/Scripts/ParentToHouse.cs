using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentToHouse : MonoBehaviour
{
    HouseController houseCon;

    // Start is called before the first frame update
    void Start()
    {
        houseCon = GameObject.FindWithTag("World").GetComponent<HouseController>();
        transform.parent = houseCon.currentHouse.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
