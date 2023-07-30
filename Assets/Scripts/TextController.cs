using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextController : MonoBehaviour
{
    public GameObject text;
    bool state;

    // Start is called before the first frame update
    void Start()
    {
        TextSetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void TextSetActive(bool state, float delay = 0) {
        StartCoroutine(ActuallyTextSetActive(state, delay));
    }

    IEnumerator ActuallyTextSetActive(bool state, float delay)
    {
        yield return new WaitForSeconds(delay);
        text.SetActive(state);
    }
}
