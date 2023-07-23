using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    AudioManager am;
    CursorController cursorCon;
    public CameraController camCon;
    public PlayerRagdollController playerRDCon;
    public PostController postCon;
    public TimeController timeCon;
    public HouseController houseCon;
    public SantaController santaCon;
    public UpdateReferences updateRefs;
    public ArrowController arrowCon;

    Delete del;
    TreeController treeCon;

    bool changed = false;

    // Start is called before the first frame update
    void Start()
    {
        //camCon.Enter();
        del = GetComponent<Delete>();
        treeCon = GetComponent<TreeController>();
        am = GetComponentInChildren<AudioManager>();
        cursorCon = GetComponent<CursorController>();

        cursorCon.EnableCursor(0);
        //camCon.MainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (!changed) {
            if (camCon.MainMenu() && Input.GetMouseButtonDown(0)) {
                cursorCon.DisableCursor();
                Debug.Log("------------enter house");
                camCon.Enter();
                houseCon.EnableHouse();
                playerRDCon.DisableRagdoll();
                santaCon.Enable();
            }
        }

        if (playerRDCon.isHit() && !changed) {
            changed = true;
            //del.DeleteAll();
            camCon.Exit();
            timeCon.BounceTime();
            postCon.BouncePost();
            santaCon.Disable();
            houseCon.Switch(1.3f);
            cursorCon.EnableCursor(5.3f);
            Invoke("ResetChanged", 5.3f);
            am.Play("Static");
            treeCon.ResetElfPop();
        }
    }

    void ResetChanged() {
        changed = false;
        playerRDCon.ResetHit();
        del.ResetDelete();
    }
}
