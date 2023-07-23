using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeDieController : MonoBehaviour
{
    public GameObject hitEffect;
    public GameObject broken;
    public float radius;
    public float force;

    TreeSpawner treeSpawner;

    // Start is called before the first frame update
    void Start()
    {
        treeSpawner = GameObject.FindWithTag("TreeSpawner").GetComponent<TreeSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Die() {
        Instantiate(broken, transform.position, Quaternion.Euler(new Vector3(-90, transform.rotation.y, transform.rotation.z)));

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
            
        foreach (Collider nearbyObject in colliders) {
            if (nearbyObject.gameObject.layer == LayerMask.NameToLayer("ElfHousePiece")) {
                Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();

                if (rb != null) {
                    rb.AddExplosionForce(force, transform.position, radius);
                }
            }
        }

        treeSpawner.treeCount--;

        Destroy(gameObject);
    }
}
