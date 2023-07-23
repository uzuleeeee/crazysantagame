using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public void EnableCursor(float delay) {
        Invoke("ActuallyEnableCursor", delay);
    }

    void ActuallyEnableCursor() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("enablelelee");
    }

    public void DisableCursor() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("DISABLE");
    }
}
