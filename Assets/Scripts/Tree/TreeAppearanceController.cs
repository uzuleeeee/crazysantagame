using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeAppearanceController : MonoBehaviour
{
    TreeController treeCon;
    Renderer treeRen;

    Material skinMat;

    float health;
    float skin;

    // Start is called before the first frame update
    void Start()
    {
        treeCon = GetComponent<TreeController>();
        treeRen = GetComponentInChildren<Renderer>();

        skinMat = treeRen.materials[0];
    }

    // Update is called once per frame
    void Update()
    {
        health = treeCon.health;

        skin = Mathf.Lerp(0.25f, 1, health);
        skinMat.SetFloat("_Progress", skin);
    }
}
