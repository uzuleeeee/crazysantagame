using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SantaAppearanceController : MonoBehaviour
{
    SantaController santaCon;
    Renderer santaRen;

    Material skinMat, ribsMat, heartMat;

    float health;
    float skin, ribs;
    float popDuration;

    bool popped;

    // Start is called before the first frame update
    void Start()
    {
        santaCon = GetComponent<SantaController>();
        santaRen = transform.GetChild(0).GetComponentInChildren<Renderer>();

        skinMat = santaRen.materials[0];
        ribsMat = santaRen.materials[3];
        heartMat = santaRen.materials[4];
    }

    // Update is called once per frame
    void Update()
    {
        health = santaCon.health;

        skin = Mathf.Lerp(0, 1, (health - 20) / 80);
        ribs = Mathf.Lerp(0.96f, 0.99f, health / 20);

        skinMat.SetFloat("_Progress", skin);
        ribsMat.SetFloat("_Progress", ribs);

        if (health <= 0) {
            heartMat.SetFloat("_Pop", 1);
            Pop();
        }
    }

    void Pop() {
        popDuration += Time.deltaTime;
        heartMat.SetFloat("_Pop_amount", popDuration * 6);
    }
}
