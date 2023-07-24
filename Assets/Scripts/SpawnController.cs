using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    SantaController santaCon;
    bool foundSanta;

    TreeSpawner treeSpawner;

    float health, originalHealth;

    // Start is called before the first frame update
    void Start()
    {
        treeSpawner = GetComponent<TreeSpawner>();
    }

    public void FindSanta() {
        foundSanta = true;

        santaCon = GameObject.FindWithTag("Santa").GetComponent<SantaController>();
        originalHealth = santaCon.health;
    }

    // Update is called once per frame
    void Update()
    {
        if (foundSanta) {
            health = santaCon.health;

            if (health / originalHealth < 0.7f) {
                treeSpawner.StartSpawning(2, 3);
            }
        }
    }
}
