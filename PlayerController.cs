using UnityEngine;
using System.Collections;

public class PlayerController : ActionController {
    public bool showCursor = true;

    void Start() {
        base.Start();
        Cursor.visible = showCursor;
    }

    void Update() {
        base.Update();
        InputControl();
        RareBook();
    }

    void OnGUI() {

    }

    void InputControl() {
        Screen.lockCursor = (showCursor == false);
        if (Input.GetKeyDown(KeyCode.R)) {
            showCursor ^= true;
            Cursor.visible = showCursor;
        }

        if (args.hp == 0f) {
            Dead();
            return;
        }

        if (Input.GetAxis("Mouse X") != 0f) {
            if (showCursor == false) {
                Turn(Input.GetAxis("Mouse X"));
            }
        }

        float speedRate = GetComponent<Animation>().IsPlaying("Attack01") ? 0.2f : 1f;
        bool moved = false;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            moved = true;
            bool otherDirection = (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow));
            MoveForward((otherDirection ? args.moveSpeed * 0.6f : args.moveSpeed) * speedRate);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            moved = true;
            MoveBackward(args.moveSpeed * 0.6f * speedRate);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            moved = true;
            MoveLeft(args.moveSpeed * 0.6f * speedRate);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            moved = true;
            MoveRight(args.moveSpeed * 0.6f * speedRate);
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            moved = true;
            Jump();
        }

        if (Input.GetButton("Fire1") || Input.GetKey(KeyCode.Tab)) {
            if (showCursor == false) {
                Attack();
            }
        }
        else if (GetComponent<Animation>().IsPlaying("Attack01") == false) {
            if (moved) {
                Run();
            }
            else {
                if (GetComponent<Animation>().IsPlaying("Damage") == false)
                    Idle();
            }
        }
    }

    void RareBook() {
        if (Input.GetKey(KeyCode.L)) {
            if (Input.GetKey(KeyCode.Alpha1))
                Application.LoadLevel("level1");
            if (Input.GetKey(KeyCode.Alpha2))
                Application.LoadLevel("level2");
            if (Input.GetKey(KeyCode.Alpha3))
                Application.LoadLevel("level3");
            if (Input.GetKey(KeyCode.Alpha4))
                Application.LoadLevel("level4");
            if (Input.GetKey(KeyCode.Alpha5))
                Application.LoadLevel("level5");
        }

        if (Input.GetKey(KeyCode.K)) {
            GameObject[] monsters = GameObject.FindGameObjectsWithTag("monster");
            foreach (GameObject monster in monsters) {
                monster.GetComponent<ArgsController>().BeingAttacked(10000f);
            }
        }
    }
}
