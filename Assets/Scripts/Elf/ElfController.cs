using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class ElfController : MonoBehaviour
{
    static int currentlyHit;

    NavMeshAgent agent;
    Transform player, playerHitDirection;
    public GameObject hitEffect;

    public float rotSpeed = 140;
    Quaternion rotTarget;

    bool hit = false;
    int inMotionLayer, weaponLayer, arrowLayer;

    Rigidbody[] rbs;
    Animator anim;
    Collider mainCollider;

    public AudioSource ree, jump;

    public static int justDeadCount;
    
    TreeController treeCon;

    // Start is called before the first frame update
    void Start()
    {
        treeCon = GameObject.FindWithTag("Tree").GetComponent<TreeController>();

        treeCon.ChangeElfPop(1);

        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
        playerHitDirection = GameObject.FindWithTag("Weapon Direction").transform;

        inMotionLayer = LayerMask.NameToLayer("In Motion");
        weaponLayer = LayerMask.NameToLayer("Weapon");
        arrowLayer = LayerMask.NameToLayer("Arrow");

        rbs = GetComponentsInChildren<Rigidbody>();
        anim = GetComponent<Animator>();
        mainCollider = GetComponent<Collider>();

        DisableRagdoll();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Just dead count: " + justDeadCount);

        while (player == null) {
            player = GameObject.FindWithTag("Player").transform;
        }

        if (!hit) {
            agent.SetDestination(player.position);
            RotateTo(player.position);
        }

        if (agent.isOnOffMeshLink) {
            jump.Play();
        }
    }

    void RotateTo(Vector3 target) {
        rotTarget = Quaternion.LookRotation(target - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotTarget, rotSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other) {
        if (justDeadCount < 1) {
            int otherGameObjectLayer = other.transform.gameObject.layer;

            Debug.Log(otherGameObjectLayer);

            if (otherGameObjectLayer == inMotionLayer || otherGameObjectLayer == weaponLayer || otherGameObjectLayer == arrowLayer) {
                if (!hit) {
                    Rigidbody otherRb = other.transform.parent.GetComponent<Rigidbody>();
                    if (otherGameObjectLayer == weaponLayer && otherRb.velocity.magnitude < 20) {
                        return;
                    }

                    justDeadCount++;

                    hit = true; 
                    Vector3 hitPoint = other.transform.position;
                    Instantiate(hitEffect, hitPoint, Quaternion.Euler(Vector3.zero));
                    EnableRagdoll();
                    ree.Play();
                }
            }
        }
    }

    void DisableRagdoll() {
        anim.enabled = true;
        agent.enabled = true;
        mainCollider.enabled = true;

        foreach (Rigidbody rb in rbs) {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }
    }

    void EnableRagdoll() {
        anim.enabled = false;
        agent.enabled = false;
        mainCollider.enabled = false;
        agent.velocity = Vector3.zero;

        Vector3 opposite = transform.position - player.position;
        transform.position += opposite.normalized;

        foreach (Rigidbody rb in rbs) {
            rb.isKinematic = false;
            rb.detectCollisions = true;
            rb.AddForce(opposite.normalized * 70, ForceMode.Impulse);;
        }
        Invoke("DecreaseJustDeadCount", 0.2f);
        treeCon.ChangeElfPop(-1);
    }

    void DecreaseJustDeadCount() {
        justDeadCount--;
    }
}
