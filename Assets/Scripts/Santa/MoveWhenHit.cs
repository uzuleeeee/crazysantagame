using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class MoveWhenHit : MonoBehaviour
{
    AudioManager am;

    public SantaController santaCon;
    public TreeController treeCon;
    public Rig rigBuilder;
    bool santa;

    public Transform player;
    public Transform IK;

    Vector3 originalPos, newPos;
    Vector3 opposite;

    int inMotionLayer, weaponLayer;

    public GameObject hitEffect;

    static bool hit;
    float intensity;

    public float transitionSpeed;
    public float transitionCurrent, transitionTarget;
    public AnimationCurve transitionCurve;

    // Start is called before the first frame update
    void Start()
    {
        am = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();

        player = GameObject.FindWithTag("Weapon Direction").transform;

        santa = santaCon != null;

        intensity = 1;

        originalPos = IK.localPosition;

        inMotionLayer = LayerMask.NameToLayer("In Motion");
        weaponLayer = LayerMask.NameToLayer("Weapon");

        hit = false;

        transitionCurrent = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!santa) {
            transitionCurrent = Mathf.MoveTowards(transitionCurrent, 1, transitionSpeed * Time.deltaTime);
            rigBuilder.weight = transitionCurve.Evaluate(transitionCurrent);
        }
    }

    public void Bounce() {
        transitionCurrent = 0;
        transitionTarget = 1;
    }

    void OnTriggerEnter(Collider other) {
        int otherGameObjectLayer = other.transform.gameObject.layer;

        if (otherGameObjectLayer == inMotionLayer || otherGameObjectLayer == weaponLayer) {
            if (!hit) {
                Rigidbody otherRb = other.transform.parent.GetComponent<Rigidbody>();
                if (otherGameObjectLayer == weaponLayer && otherRb.velocity.magnitude < 20) return;
                hit = true; 
                Vector3 hitPoint = other.transform.position;
                Instantiate(hitEffect, hitPoint, Quaternion.Euler(Vector3.zero));
                
                Invoke(nameof(ResetHitBool), 0.5f);

                if (otherGameObjectLayer == inMotionLayer) {
                    opposite = transform.localPosition - transform.InverseTransformPoint(player.position);
                    intensity = 2;
                    //Debug.Log("Swing");
                    IK.localPosition = newPos;
                    if (santa) {
                        santaCon.health -= other.gameObject.GetComponent<WeaponController>().damage / 5;
                    } else {
                        treeCon.health -= other.gameObject.GetComponent<WeaponController>().damage * 2;
                        am.Play("TreeHit");
                        Bounce();
                    }
                } else if (otherGameObjectLayer == weaponLayer) {
                    opposite = transform.InverseTransformPoint(otherRb.velocity);
                    intensity *= (otherRb.velocity.magnitude * 0.1f);
                    //Debug.Log("Throw");
                    Invoke(nameof(Move), 0.1f);
                    if (santa) {
                        santaCon.health -= other.gameObject.GetComponent<WeaponController>().damage;
                    } else {
                        treeCon.health -= other.gameObject.GetComponent<WeaponController>().damage * 4;
                        am.Play("TreeHit");
                        Bounce();
                    }
                }
                
                newPos = originalPos + opposite.normalized * intensity;
                
                Invoke(nameof(ResetPosition), 0.2f);
            }
        }
    }

    void ResetHitBool() {
        hit = false;
    }

    void Move() {
        IK.localPosition = newPos;
    }

    void ResetPosition() {
        IK.localPosition = originalPos;
    }
}
