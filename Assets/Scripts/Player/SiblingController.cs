using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiblingController : MonoBehaviour
{
    public GameObject sibling;
    public Transform arrowSpawn;
    public GameObject arrow;

    // Start is called before the first frame update
    void Start()
    {
        sibling.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateSibling(float delay = 0) {
        Invoke("ActuallyActivateSibling", delay);
    }

    public void ActuallyActivateSibling() {
        sibling.SetActive(true);
        Instantiate(arrow, arrowSpawn.position, arrowSpawn.rotation);
    }
}
