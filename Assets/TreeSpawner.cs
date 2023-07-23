using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public GameObject tree;
    public Transform spawnPoint;

    public int treeCount;

    Material treeMat;

    public float maxTreeCount = 1;

    // Start is called before the first frame update
    void Start()
    {
        treeMat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        maxTreeCount += Time.deltaTime / 100;
    }

    public void StartSpawning() {
        InvokeRepeating("SpawnTree", 10, 10);
    }

    void SpawnTree() {
        if (treeCount < maxTreeCount) {
            Instantiate(tree, spawnPoint.position, spawnPoint.rotation);
            Shake();
            treeCount++;
        }
    }

    void Shake() {
        treeMat.SetFloat("_Shake", 1);
        Invoke("Unshake", 1);
    }

    void Unshake() {
        treeMat.SetFloat("_Shake", 0);
    }
}
