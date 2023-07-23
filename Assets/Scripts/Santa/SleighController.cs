using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleighController : MonoBehaviour
{
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(speed * Time.deltaTime, 0, 0);
    }
}
