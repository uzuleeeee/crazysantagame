using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyRealSantaAppearance : MonoBehaviour
{
    public Renderer santaRenderer;
    Renderer thisRenderer;

    // Start is called before the first frame update
    void Start()
    {
        thisRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        thisRenderer.materials[0].SetFloat("_Progress", santaRenderer.materials[0].GetFloat("_Progress"));
        thisRenderer.materials[3].SetFloat("_Progress", santaRenderer.materials[3].GetFloat("_Progress"));
    }
}
