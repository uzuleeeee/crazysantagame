using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SantaController : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject fireworks;
    public Transform playerAim;

    AudioManager am;
    public string[] sounds;
    public Vector2 soundDurationRange;
    float soundDuration;
    float soundTimer = 0;

    public float health = 100;

    public Collider[] colliders;
    Rigidbody[] rbs;
    public Rigidbody crossbowRb;
    MoveWhenHit[] hitboxes;
    Animator[] anims;
    public SantaStateManager stateManager;
    NavMeshAgent agent;

    public bool dead = false;

    // Start is called before the first frame update
    void Awake()
    {
        am = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
        soundDuration = Random.Range(soundDurationRange.x, soundDurationRange.y);

        rbs = GetComponentsInChildren<Rigidbody>();
        hitboxes = GetComponentsInChildren<MoveWhenHit>();
        anims = GetComponentsInChildren<Animator>();
        stateManager = GetComponent<SantaStateManager>();
        agent = GetComponent<NavMeshAgent>();

        DisableRagdoll();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0) {
            EnableRagdoll();
        } else {
            playerAim.position = stateManager.player.position;

            soundTimer += Time.deltaTime;
            if (soundTimer > soundDuration) {
                soundTimer = 0;
                soundDuration = Random.Range(soundDurationRange.x, soundDurationRange.y);
                HoHoHo();
            }
        }
    }

    void HoHoHo() {
        am.Play(sounds[Random.Range(0, sounds.Length)]);
    }

    public void SwitchHouse() {
        Disable();
        Invoke("Enable", 6);
    }

    public void Enable(float delay = 0) {
        Invoke("ActuallyEnable", delay);
    }

    void ActuallyEnable() {
        gameObject.SetActive(true);
        //transform.position = spawnPoint.position;
        //transform.rotation = spawnPoint.rotation;
        stateManager.Reset();
    }

    public void Disable() {
        gameObject.SetActive(false);
    }

    void DisableRagdoll() {
        stateManager.enabled = true;
        agent.enabled = true;
        foreach (Animator anim in anims) {
            anim.enabled = true;
        }
        foreach (Collider col in colliders) {
            col.enabled = true;
        }
        foreach (Rigidbody rb in rbs) {
            rb.isKinematic = true;
            //rb.detectCollisions = false;
        }
        crossbowRb.isKinematic = true;
        crossbowRb.detectCollisions = false;
    }

    public void EnableRagdoll() {
        if (!dead) {
            dead = true;

            stateManager.enabled = false;
            agent.velocity = Vector3.zero;
            agent.enabled = false;
            foreach (Animator anim in anims) {
                anim.enabled = false;
            }
            foreach (Collider col in colliders) {
                col.enabled = false;
            }
            foreach (Rigidbody rb in rbs) {
                rb.isKinematic = false;
                rb.detectCollisions = true;
                rb.velocity = Vector3.zero;
            }
            crossbowRb.isKinematic = false;
            crossbowRb.detectCollisions = true;

            GameObject.FindWithTag("TimeController").GetComponent<TimeController>().BounceTime();
            StartCoroutine(SpawnFireworks());
        }
    }

    IEnumerator SpawnFireworks()
    {
        while (true) {
            Instantiate(fireworks, new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10)), transform.rotation);
            Instantiate(fireworks, new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10)), transform.rotation);
            Instantiate(fireworks, new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10)), transform.rotation);
            Instantiate(fireworks, new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10)), transform.rotation);
            Instantiate(fireworks, new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10)), transform.rotation);
            Instantiate(fireworks, new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10)), transform.rotation);
            am.Play("Celebrate");
            yield return new WaitForSeconds (Random.Range(2, 5));
        }
    }
}
