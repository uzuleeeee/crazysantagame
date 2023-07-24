using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HousePhysicsController : MonoBehaviour
{
    public Collider[] colliders;
    public Rigidbody[] rbs;

    // Start is called before the first frame update
    void Awake()
    {
        colliders = GetComponentsInChildren<Collider>();
        rbs = GetComponentsInChildren<Rigidbody>();

        DisableHouse();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableHouse() {
        foreach (Collider col in colliders) {
            if (col != null) {
                col.enabled = true;
            }
        }

        foreach (Rigidbody rb in rbs) {
            if (rb != null) {
                rb.isKinematic = false;
                rb.detectCollisions = true;
            }
        }
    }

    void DisableHouse() {
        foreach (Collider col in colliders) {
            col.enabled = false;
        }

        foreach (Rigidbody rb in rbs) {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }
    }
}
