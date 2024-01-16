using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinetRandomizer : MonoBehaviour
{
    int numOfChildren, destroyAmount;

    // Start is called before the first frame update
    void Start()
    {
        numOfChildren = transform.childCount;
        destroyAmount = 2;
        for (int i = 0; i < numOfChildren / destroyAmount; i++) {
            transform.GetChild(Random.Range(1, numOfChildren)).gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
