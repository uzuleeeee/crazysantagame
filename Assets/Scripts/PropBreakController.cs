using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropBreakController : MonoBehaviour
{
    public float probOfBreaking = 0.5f;

    public GameObject hitEffect;
    public GameObject broken;
    public float radius;
    public float force;

    bool spawned = false;

    int arrowLayer;

    // Start is called before the first frame update
    void Start()
    {
        spawned = false;
        arrowLayer = LayerMask.NameToLayer("Arrow");
    }

    public void Break(Vector3 hitPoint, Vector3 hitEffectPoint) {
        spawned = true;

        GameObject spawnedBroken = Instantiate(broken, transform.position, transform.rotation);
        
        foreach (Rigidbody rb in spawnedBroken.GetComponentsInChildren<Rigidbody>()) {
            if (rb != null) {
                rb.AddExplosionForce(force, hitPoint, radius);
            }
        }

        Instantiate(hitEffect, hitEffectPoint, Quaternion.Euler(Vector3.zero));

        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collisionInfo) {
        if (!spawned) {
            int layer = collisionInfo.gameObject.layer;

            float mag = transform.GetComponent<Rigidbody>().velocity.sqrMagnitude;
            if (layer == arrowLayer) {
                if (mag > 0) {
                    Break(transform.position, collisionInfo.transform.position);
                }
            }
        }
    }
}
