using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakController : MonoBehaviour
{
    public GameObject hitEffect;
    public GameObject broken;
    public float radius;
    public float force;

    bool spawned = false;

    int santaThrowColliderLayer, arrowLayer;

    // Start is called before the first frame update
    void Start()
    {
        spawned = false;

        santaThrowColliderLayer = LayerMask.NameToLayer("Santa Throw Collider");
        arrowLayer = LayerMask.NameToLayer("Arrow");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Break(Vector3 hitPoint, Vector3 hitEffectPoint) {
        spawned = true;

        GameObject child = gameObject.transform.GetChild(0).gameObject;
        GameObject spawnedBroken = Instantiate(broken, child.transform.position, child.transform.rotation);

        spawnedBroken.transform.localScale = child.transform.localScale;
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
            float mag = transform.GetComponent<Rigidbody>().velocity.magnitude;

            int layer = collisionInfo.gameObject.layer;
            if (layer == santaThrowColliderLayer) {
                if (mag > 5) {
                    Break(transform.position, collisionInfo.transform.position);
                }
            } else if (layer == arrowLayer) {
                if (mag > 0) {
                    Break(transform.position, collisionInfo.transform.position);
                }
            }
        }
    }
}
