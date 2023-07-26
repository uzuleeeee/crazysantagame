using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakController : MonoBehaviour
{
    public float probOfBreaking = 0.5f;

    public GameObject hitEffect;
    public GameObject broken;
    public float radius;
    public float force;

    public bool swing;

    bool spawned = false;

    int santaThrowColliderLayer, arrowLayer, santaHitBoxLayer;

    // Start is called before the first frame update
    void Start()
    {
        spawned = false;

        santaHitBoxLayer = LayerMask.NameToLayer("Santa Hitbox");
        santaThrowColliderLayer = LayerMask.NameToLayer("Santa Throw Collider");
        arrowLayer = LayerMask.NameToLayer("Arrow");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Break(Vector3 hitPoint, Vector3 hitEffectPoint, bool withProbability = false) {
        if (!withProbability) {
            ActuallyBreak(hitPoint, hitEffectPoint);
        } else  {
            float generatedProb = Random.Range(0f, 1f);
            if (generatedProb < probOfBreaking) {
                ActuallyBreak(hitPoint, hitEffectPoint);
            }
        }
    }

    void ActuallyBreak(Vector3 hitPoint, Vector3 hitEffectPoint) {
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
            int layer = collisionInfo.gameObject.layer;

            if (!swing) {
                float mag = transform.GetComponent<Rigidbody>().velocity.sqrMagnitude;
                if (layer == santaThrowColliderLayer) {
                    if (mag > 25) {
                        Break(transform.position, collisionInfo.transform.position);
                    }
                } else if (layer == arrowLayer) {
                    if (mag > 0) {
                        Break(transform.position, collisionInfo.transform.position);
                    }
                }
            } else {
                if (layer == santaHitBoxLayer) {
                    Break(transform.position, collisionInfo.transform.position);
                }
            }
        }
    }
}
