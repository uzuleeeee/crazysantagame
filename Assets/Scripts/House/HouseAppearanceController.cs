using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseAppearanceController : MonoBehaviour
{
    public GameObject[] turnBlack;

    List<Renderer[]> renderers = new List<Renderer[]>();
    Shader baseShader;
    public Material outlineMaterial;

    public float brightness;
    public float outline;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject obj in turnBlack) {
            renderers.Add(obj.GetComponentsInChildren<Renderer>());
        }
        baseShader = Shader.Find("Shader Graphs/Base Color");
    }

    // Update is called once per frame
    void Update()
    {
        ChangeBrightness(brightness);
    }

    public void TurnBlack() {
        //ChangeBrightness(0.4f);
    }

    void ChangeBrightness(float brightness) {
        foreach (Renderer[] renGroup in renderers) {
            foreach (Renderer ren in renGroup) {
                if (ren.material.shader == baseShader) {
                    ren.material.SetFloat("_Brightness", brightness);
                }
            }
        }

        outlineMaterial.SetFloat("_Brightness", outline);
    }
}
