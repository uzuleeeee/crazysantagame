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

        //Debug.Log("Find santa");
        foundSanta = true;

        santaCon = GameObject.FindWithTag("Santa").GetComponent<SantaController>();
        originalHealth = santaCon.health;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(foundSanta);
        
        if (foundSanta) {
            health = santaCon.health;

            Debug.Log(health / originalHealth);

            if (health / originalHealth < 0.7f) {
                treeSpawner.StartSpawning(2, 3);
            }
        }
    }
}
