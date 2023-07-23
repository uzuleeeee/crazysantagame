using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delete : MonoBehaviour
{
    public static bool delete = false;
    public bool deleteThis = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (delete && deleteThis) {
            Debug.Log("DESTORY: " + gameObject.name);
            //Destroy(gameObject);
        }
    }
    
    public void DeleteAll() {
        Invoke("ActuallyDeleteAll", 1f);
    }

    void ActuallyDeleteAll() {
        delete = true;
    }

    public void ResetDelete() {
        delete = false;
    }
}
