using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiblingRagdoll : MonoBehaviour
{
    Rigidbody[] rbs;
    Rigidbody rb;
    
    int arrowLayer;

    // Start is called before the first frame update
    void Start()
    {
        rbs = GetComponentsInChildren<Rigidbody>();
        rb = GetComponent<Rigidbody>();

        foreach (Rigidbody rb in rbs) {
            rb.isKinematic = true;
        }
        rb.isKinematic = false;

        arrowLayer = LayerMask.NameToLayer("Arrow");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void EnableRagdoll() {
        Debug.Log("enabled ragdoll");

        foreach (Rigidbody rb in rbs) {
            rb.isKinematic = false;
        }

        rb.AddForce(-transform.forward * 500f, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Collision" + other.gameObject.layer + ", " + other.gameObject.name);
        if (other.gameObject.layer == arrowLayer) {
            EnableRagdoll();
        }
    }
}
