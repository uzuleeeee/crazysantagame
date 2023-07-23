using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustBrightness : MonoBehaviour
{

    public float progress = 0;

    Renderer[] renderers;
    Shader baseShader;
    public Material outlineMaterial;

    // Start is called before the first frame update
    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        baseShader = Shader.Find("Shader Graphs/Base Color");
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Renderer ren in renderers) {
            if (ren.material.shader == baseShader) {
                ren.material.SetFloat("_Brightness", progress);
            }
        }

        outlineMaterial.SetFloat("_Brightness", progress);
    }
}
