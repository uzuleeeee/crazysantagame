using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    AudioManager am;

    public float speed = 100f;

    public static bool destroy = false;

    Rigidbody rb;
    Cinemachine.CinemachineImpulseSource source;

    int elfLayer;

    public bool isMain = false;

    // Start is called before the first frame update
    void Start()
    {
        am = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();

        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.up * speed, ForceMode.Impulse);
        source = GetComponent<Cinemachine.CinemachineImpulseSource>();
        source.GenerateImpulse(Camera.main.transform.forward);

        elfLayer = LayerMask.NameToLayer("Ragdoll");

        transform.parent = GameObject.FindWithTag("World").transform;

        am.Play("BowRelease");
    }

    void FixedUpdate() {

    }

    // Update is called once per frame
    void Update()
    {
        float mag = rb.velocity.magnitude;

        if (!isMain && destroy) {
            Destroy(gameObject);
        }
/*
        if (mag < 10) {
            Debug.Log("10, " + mag);
        } else if (mag < 20) {
            Debug.Log("20, " + mag);
        } else if (mag < 30) {
            Debug.Log("30, " + mag);       
        } else if (mag < 40) {
            Debug.Log("40, " + mag);
        } else if (mag < 50) {
            Debug.Log("50, " + mag);
        } else if (mag >= 50) {
            Debug.Log(">50, " + mag);
        }
        */
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>() == null && collision.gameObject.layer != elfLayer) {
            Debug.Log(collision.gameObject.name);
            Freeze();
            am.Play("ArrowHit");
        }
        //Freeze();
        /*
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player")) {
            if (collision.transform.childCount > 0) {
                if (collision.transform.GetChild(0).gameObject.layer == LayerMask.NameToLayer("Weapon")) {
                    Debug.Log("Weapon");
                } else {
                    Freeze();
                }
            } else {
                Freeze();
            }
        }
        */
    }

    void Freeze() {
        rb.detectCollisions = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        Vector3 normal = transform.up.normalized;
        transform.position += new Vector3(normal.x * 2, normal.y * 2, normal.z * 2);
    }

    public void SetDestroyTrue(float delay) {
        Invoke("ActuallySetDestroyTrue", delay);
    }

    void ActuallySetDestroyTrue() {
        destroy = true;
    }

    public void SetDestroyFalse(float delay) {
        Invoke("ActuallySetDestroyFalse", delay);
    }
    
    void ActuallySetDestroyFalse() {
        destroy = false;
    }
}
