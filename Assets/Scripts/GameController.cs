using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    AudioManager am;
    CursorController cursorCon;
    public TextController textCon;
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
        del = GetComponent<Delete>();
        treeCon = GetComponent<TreeController>();
        am = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
        cursorCon = GetComponent<CursorController>();
        cursorCon.EnableCursor(0);
        camCon.SetSanta(houseCon.currentSanta);
    }

    // Update is called once per frame
    void Update()
    {
        if (!changed) {
            if (camCon.MainMenu() && Input.GetMouseButtonDown(0)) {
                cursorCon.DisableCursor();
                camCon.Enter();
                houseCon.EnableHouse(1f);
                playerRDCon.DisableRagdoll();
                playerRDCon.Reset(houseCon.currentSanta.GetChild(1).GetChild(0));
                //santaCon = houseCon.currentSanta.gameObject.GetComponent<SantaController>();
                camCon.SetSanta(houseCon.currentSanta);
                //santaCon.Enable(1f);
                timeCon.StopTime(1.2f);
                camCon.FPC(1.1f);
                //textCon.TextSetActive(true, 0.9f);
                //textCon.TextSetActive(false, 2f);
            }
        }

        if (playerRDCon.isHit() && !changed) {
            changed = true;
            //del.DeleteAll();
            camCon.Exit();
            timeCon.BounceTime();
            postCon.BouncePost();
            //santaCon.Disable();
            houseCon.Switch(1.3f);
            cursorCon.EnableCursor(5.8f);
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
