using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public float damage = 5;
    float speed;

    // Start is called before the first frame update
    void Start()
    {
        speed = -0.1f * damage + 1.2f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetSpeed() {
        return speed;
    }
}
