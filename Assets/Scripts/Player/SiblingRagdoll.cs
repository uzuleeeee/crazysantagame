using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiblingRagdoll : MonoBehaviour
{
    Rigidbody[] rbs;
    Rigidbody rb;
    Collider col;
    
    int arrowLayer;

    // Start is called before the first frame update
    void Start()
    {
        rbs = GetComponentsInChildren<Rigidbody>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

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
        foreach (Rigidbody rb in rbs) {
            rb.isKinematic = false;
        }
        rb.AddForce(-transform.forward * 500f, ForceMode.Impulse);
        Destroy(gameObject, 0.2f);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == arrowLayer) {
            EnableRagdoll();
        }
    }
}
