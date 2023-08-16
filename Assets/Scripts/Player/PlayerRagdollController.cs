using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRagdollController : MonoBehaviour
{
    bool hit = false;

    Rigidbody[] rbs;
    Animator anim;
    PlayerController playerCon;
    PlayerHandsController playerHandsCon;

    public Transform crossbow;

    public Transform headMesh;

    public GameObject playerBody;

    GameObject currentBody;

    public GameObject ragdoll;

    Rigidbody rb;

    public Transform spawnPoint;
    
    int arrowLayer;

    // Start is called before the first frame update
    void Start()
    {
        rbs = GetComponentsInChildren<Rigidbody>();
        anim = GetComponent<Animator>();
        playerCon = GetComponent<PlayerController>();
        playerHandsCon = GetComponent<PlayerHandsController>();

        arrowLayer = LayerMask.NameToLayer("Arrow");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DisableRagdoll() {
        transform.position = spawnPoint.position;
        gameObject.SetActive(true);
    }

    void EnableRagdoll(Vector3 velocity) {
        gameObject.SetActive(false);
        currentBody = Instantiate(ragdoll, transform.position, transform.rotation);
        rb = currentBody.GetComponent<Rigidbody>();

        rb.AddForce(velocity.normalized * 550f, ForceMode.Impulse);

        playerHandsCon.LetGoOfWeapon();
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.layer == arrowLayer) {
            Rigidbody otherRb = other.gameObject.GetComponent<Rigidbody>();
            if (otherRb != null) {
                float mag = otherRb.velocity.magnitude;
                if (mag > 0) {
                    hit = true;

                    Vector3 velocity = transform.position - crossbow.position;
                    EnableRagdoll(new Vector3(velocity.x, 0, velocity.z));
                }
            }
        }
    }

    public bool isHit() {
        return hit;
    }

    public void ResetHit() {
        hit = false;
    }

    public void Reset(Transform newCrossbow) {
        crossbow = newCrossbow;
    }
}
