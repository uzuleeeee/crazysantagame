using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowController : MonoBehaviour
{
    AudioManager am;

    [Header("Player")]
    public Transform player;
    LayerMask playerLayerMask;
    int playerLayer;

    [Header("Movement")]
    Quaternion rotTarget;
    public float rotSpeed = 20;

    [Header("Arrow")]
    public GameObject arrow;
    public Transform arrowPoint; 

    [Header("Transition lerp")]
    public float transitionSpeed;
    public AnimationCurve transitionCurve;
    public Vector3 upPos;
    public Vector3 downPos, downRotation;
    float transitionCurrent, transitionTarget;

    [Header("Crossbow")]
    public Transform holder;
    Animator anim;
    int trDrawHash, trReleaseHash;
    
    void Awake()
    {
        am = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();

        anim = GetComponent<Animator>();
        holder = transform.parent;

        playerLayerMask = LayerMask.GetMask("Player");
        playerLayer = LayerMask.NameToLayer("Player");

        trDrawHash = Animator.StringToHash("TrDraw");
        trReleaseHash = Animator.StringToHash("TrRelease");
    }

    void Update()
    {
        rotTarget = Quaternion.LookRotation(player.position - holder.position + Vector3.up);

        transitionCurrent = Mathf.MoveTowards(transitionCurrent, transitionTarget, transitionSpeed * Time.deltaTime);
        holder.localPosition = Vector3.Lerp(downPos, upPos, transitionCurve.Evaluate(transitionCurrent));
        holder.rotation = Quaternion.Lerp(Quaternion.Euler(downRotation + new Vector3(0, holder.eulerAngles.y, 0)), rotTarget, transitionCurve.Evaluate(transitionCurrent));
    }

    public bool CanSeePlayer() {
        Ray ray = new Ray(arrowPoint.position, -20f * transform.forward);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, 30, playerLayerMask.value)) {
            if (hit.transform.gameObject.layer == playerLayer) {
                return true;
            }
        }        
        return false;
    }

    public void Draw() {
        anim.SetTrigger(trDrawHash);
        am.Play("BowDraw");
    }

    public void Release() {
        anim.SetTrigger(trReleaseHash);
        ShootArrow();
    }

    public bool IsReadyToDraw() {
        return transitionCurrent == 0;
    }

    public bool IsMoving() {
        return transitionCurrent != 0;
    }

    public bool IsReadyToRelease() {
        return transitionCurrent == 1;
    }

    void StartMove() {
        if (transitionTarget == 0) {
            transitionTarget = 1;
        } else {
            transitionTarget = 0;
        }
    }
    
    void ShootArrow() {
        Instantiate(arrow, arrowPoint.position, arrowPoint.rotation);
    }
}
