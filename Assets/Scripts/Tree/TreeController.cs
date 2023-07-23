using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class TreeController : MonoBehaviour
{
    public float health = 50;
    float originalHealth;

    Transform player;
    NavMeshAgent agent;
    Animator anim;

    public float sqrDist = 100;
    public float sqrOutDist = 144;
    float outDist = 12;
    float sqrCurrentDist;
    Vector3 oppositeDir, newPos;

    public float rotSpeed = 250;
    Quaternion rotTarget;
    Vector3 lookTarget;

    public GameObject elf;
    public Transform elfSpawnPoint;

    Material starMat, treeMat;
    public float transitionSpeed;
    float transitionCurrent, transitionTarget;
    public AnimationCurve transitionCurve;

    int runHash;

    public AudioSource[] sources;

    GameObject spawnedElf;

    TreeDieController treeDieCon;

    float agentSitVelocityThreshold = 0.1f;

    public static float elfPop;

    // Start is called before the first frame update
    void Start()
    {
        originalHealth = health;

        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        runHash = Animator.StringToHash("Run");

        treeMat = GetComponentInChildren<Renderer>().materials[0];
        starMat = GetComponentInChildren<Renderer>().materials[1];

        outDist = Mathf.Sqrt(sqrOutDist);

        treeDieCon = GetComponent<TreeDieController>();

        InvokeRepeating("SpawnElf", 3, 5);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Elf pop: " + elfPop);

        if (health <= 0) {
            treeDieCon.Die();
            return;
        }

        Debug.Log(Mathf.Lerp(0.9f, 1, 14));
        treeMat.SetFloat("_Progress", Mathf.Lerp(0.9f, 1, health / originalHealth));

        oppositeDir = transform.position - player.position;
        sqrCurrentDist = oppositeDir.sqrMagnitude;

        if (agent.velocity.sqrMagnitude > agentSitVelocityThreshold) {
            anim.SetBool(runHash, true);
            lookTarget = newPos;
            sources[0].Play();
        } else {
            anim.SetBool(runHash, false);
            lookTarget = player.position;
            sources[0].Play();
            if (sqrCurrentDist < sqrDist) {
                MoveAway();
            } else if (sqrCurrentDist > sqrOutDist) {
                MoveCloser();
            }
        }

        RotateTo(lookTarget);

        SetStarBrightness();
    }

    void SpawnElf() {
        if (elfPop < 20) {
            if (agent.velocity.sqrMagnitude <= agentSitVelocityThreshold) {
                spawnedElf = Instantiate(elf, elfSpawnPoint.position, elfSpawnPoint.rotation);
                NavMeshHit hit;
                if (NavMesh.SamplePosition(elfSpawnPoint.position, out hit, 2f, 0)) {
                    spawnedElf.transform.position = hit.position;
                }
                BounceStarBrightness();
            }
        }
    }

    public void ResetElfPop() {
        elfPop = 0;
    }

    public void ChangeElfPop(int amount) {
        elfPop += amount;
    }

    void SetStarBrightness() {
        transitionCurrent = Mathf.MoveTowards(transitionCurrent, transitionTarget, transitionSpeed * Time.deltaTime);
        starMat.SetFloat("_Brightness", transitionCurve.Evaluate(transitionCurrent));
    }

    void BounceStarBrightness() {
        transitionCurrent = 0;
        transitionTarget = 1;
    }

    void MoveAway() {
        agent.stoppingDistance = 0;
        newPos = transform.position + new Vector3(oppositeDir.x, 0, oppositeDir.z);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(newPos, out hit, 10f, NavMesh.AllAreas)) {
            newPos = hit.position;
            float angle = 0;
            if ((newPos - transform.position).sqrMagnitude < 0.5f) {
                newPos = Quaternion.Euler(0, angle, 0) * newPos;
                angle += Time.deltaTime;
            }
            agent.SetDestination(newPos);
        }
    }

    void MoveCloser() {
        agent.stoppingDistance = outDist;
        newPos = player.position;

        agent.SetDestination(newPos);
    }

    void RotateTo(Vector3 target) {
        rotTarget = Quaternion.LookRotation(transform.position - target);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotTarget, rotSpeed * Time.deltaTime);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(newPos, 0.5f);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, 5);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, outDist);
    }
}
