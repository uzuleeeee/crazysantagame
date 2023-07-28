using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookshelfRandomizer : MonoBehaviour
{
    int numOfChildren, destroyAmount;

    // Start is called before the first frame update
    void Start()
    {
        numOfChildren = transform.childCount;
        destroyAmount = Random.Range(2, 5);
        for (int i = 0; i < numOfChildren / destroyAmount; i++) {
            transform.GetChild(Random.Range(0, numOfChildren - 2)).gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
