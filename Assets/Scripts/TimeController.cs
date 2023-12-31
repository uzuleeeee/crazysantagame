using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public float transitionSpeed;
    float transitionCurrent, transitionTarget;
    public AnimationCurve transitionCurve;

    float duration;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 1) {
            transitionCurrent = Mathf.MoveTowards(transitionCurrent, 1, transitionSpeed * Time.deltaTime);
            Time.timeScale += transitionCurve.Evaluate(transitionCurrent) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0, 1);
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }
    }

    public void BounceTime() {
        Time.timeScale = 0.05f;
        transitionCurrent = 0;
    }

    public void StopTime(float delay = 0f) {
        Invoke("ActuallyStopTime", delay);
    }

    void ActuallyStopTime() {
        Time.timeScale = 0.04f;
        transitionCurrent = 0;
    }
}
