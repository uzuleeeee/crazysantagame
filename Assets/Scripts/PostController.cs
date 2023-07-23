using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostController : MonoBehaviour
{
    public float intensity;

    Volume volume;
    ColorAdjustments color;
    FilmGrain filmGrain;

    float originalContrast, originalGrainIntensity, originalGrainResponse;

    public float transitionSpeed;
    public float transitionCurrent, transitionTarget;
    public AnimationCurve transitionCurve;
    public AnimationCurve grainIntensityCurve;
    public AnimationCurve grainResponseCurve;

    // Start is called before the first frame update
    void Start()
    {
        volume = GetComponent<Volume>();

        volume.profile.TryGet<ColorAdjustments>(out color);
        volume.profile.TryGet<FilmGrain>(out filmGrain);

        originalContrast = color.contrast.value;
        originalGrainIntensity = filmGrain.intensity.value;
        originalGrainResponse = filmGrain.response.value;
    }

    // Update is called once per frame
    void Update()
    {
        transitionCurrent = Mathf.MoveTowards(transitionCurrent, transitionTarget, transitionSpeed * Time.deltaTime);
        color.contrast.value = transitionCurve.Evaluate(transitionCurrent) * intensity + originalContrast;
        filmGrain.intensity.value = transitionCurve.Evaluate(transitionCurrent) * -0.5f + originalGrainIntensity;
        filmGrain.response.value = transitionCurve.Evaluate(transitionCurrent) + originalGrainResponse;
    }

    public void BouncePost() {
        transitionCurrent = 0;
        transitionTarget = 1;
    }
}
