using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectController : MonoBehaviour
{
    Transform target;
    Cinemachine.CinemachineImpulseSource source;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 2);
        target = Camera.main.transform;
        source = GetComponent<Cinemachine.CinemachineImpulseSource>();
        source.GenerateImpulse(target.forward);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target);
    }
}
