using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public GameObject tree;
    public Transform spawnPoint;

    Material treeMat;

    public int treeCount;
    int maxTreeCount;
    public float interval;
    bool startSpawn = false;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        treeMat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (startSpawn) {
            timer += Time.deltaTime;
            if (timer > interval) {
                timer = 0;
                SpawnTree(maxTreeCount);
            }
        }
    }

    public void StartSpawning(float interval, int maxTreeCount) {
        startSpawn = true;
        this.interval = interval;
        this.maxTreeCount = maxTreeCount;
    }

    void SpawnTree(int maxTreeCount) {
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
