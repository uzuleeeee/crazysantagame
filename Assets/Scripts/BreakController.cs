using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakController : MonoBehaviour
{
    public HouseController houseCon;

    public bool secondary;

    public GameObject hitEffect;

    public GameObject broken;
    public float radius;
    public float force;
    Renderer weaponRenderer;

    bool spawned = false;

    int santaThrowColliderLayer, arrowLayer;

    // Start is called before the first frame update
    void Start()
    {
        houseCon = GameObject.FindWithTag("World").GetComponent<HouseController>();

        spawned = false;

        weaponRenderer = transform.GetChild(0).GetComponent<Renderer>();

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
        spawnedBroken.transform.parent = houseCon.currentHouse.transform;

        if (secondary) {
            spawnedBroken.transform.localScale = transform.parent.localScale;
        } else {
            spawnedBroken.transform.localScale = child.transform.localScale;
            Collider[] colliders = Physics.OverlapSphere(hitPoint, radius);
            
            foreach (Collider nearbyObject in colliders) {
                if (nearbyObject.gameObject.layer == LayerMask.NameToLayer("Pieces")) {
                    Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();

                    if (rb != null) {
                        rb.AddExplosionForce(force, hitPoint, radius);
                    }
                }
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

                //collisionInfo.gameObject.GetComponent<ArrowController>().DestroyArrow();
            }
        }
    }
}
