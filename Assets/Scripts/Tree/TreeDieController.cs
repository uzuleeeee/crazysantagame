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

    public void Die(Vector3 hitPoint) {
        GameObject brokenTree = Instantiate(broken, transform.position, Quaternion.Euler(new Vector3(-90, transform.rotation.y, transform.rotation.z)));

        foreach (Rigidbody rb in brokenTree.GetComponentsInChildren<Rigidbody>()) {
            if (rb != null) {
                rb.AddExplosionForce(force, hitPoint, radius);
            }
        }

        treeSpawner.treeCount--;

        Destroy(gameObject);
    }
}
