using UnityEngine;
using System.Collections;
using Leap;

public class PlayerHand : ActionController {
    Controller controller = null;
    Frame frame = null;
    Hand leftHand = null;
    Hand rightHand = null;
    Finger leftThumb = null;
    Finger leftIndex = null;
    Finger leftMiddle = null;
    Finger leftRing = null;
    Finger leftPinky = null;
    Finger rightThumb = null;
    Finger rightIndex = null;
    Finger rightMiddle = null;
    Finger rightRing = null;
    Finger rightPinky = null;
    public GameObject handController;

    public GameObject kameWave;
    public GameObject kikohoWaveSmall;
    public GameObject kikohoWave;
    public GameObject sWave;
    public GameObject thunderWave;
    public GameObject spinWave;

    float kikohoTime = 0f;
    GameObject kikohoInstance = null;

    void Start() {
        base.Start();
        startHead();
    }

    void Update() {
        base.Update();
        updateHand();
    }

    void startHead() {
        controller = new Controller();

        controller.EnableGesture(Gesture.GestureType.TYPE_CIRCLE);
        controller.Config.SetFloat("Gesture.Circle.MinRadius", 10.0f);
        controller.Config.SetFloat("Gesture.Circle.MinArc", 0.5f * Mathf.PI);
        controller.Config.Save();

        controller.EnableGesture(Gesture.GestureType.TYPE_SWIPE);
        controller.Config.SetFloat("Gesture.Swipe.MinLength", 2.0f);
        controller.Config.SetFloat("Gesture.Swipe.MinVelocity", 100.0f);
        controller.Config.Save();

        controller.EnableGesture(Gesture.GestureType.TYPE_SCREEN_TAP);
        controller.Config.SetFloat("Gesture.ScreenTap.MinForwardVelocity", 30.0f);
        controller.Config.SetFloat("Gesture.ScreenTap.HistorySeconds", 0.5f);
        controller.Config.SetFloat("Gesture.ScreenTap.MinDistance", 1.0f);
        controller.Config.Save();

        controller.EnableGesture(Gesture.GestureType.TYPE_KEY_TAP);
        controller.Config.SetFloat("Gesture.KeyTap.MinDownVelocity", 20.0f);
        controller.Config.SetFloat("Gesture.KeyTap.HistorySeconds", 0.1f);
        controller.Config.SetFloat("Gesture.KeyTap.MinDistance", 1.0f);
        controller.Config.Save();
    }

    int getDirectionId(Vector vec) {
        float x = Mathf.Abs(vec.x);
        float y = Mathf.Abs(vec.y);
        float z = Mathf.Abs(vec.z);

        if (x > y && x > z) {
            return 0;
        }
        if (y > z) {
            return 1;
        }
        return 2;
    }

    void circleX(float angularSpeed) {

    }

    void circleY(float angularSpeed) {

    }

    void circleZ(float angularSpeed) {
        //SPIN
        if (Mathf.Abs(angularSpeed) > 10f) {
            if (leftHand != null && leftIndex.Direction.z < -0.6f && leftMiddle.Direction.z > 0f && leftRing.Direction.z > 0f && leftPinky.Direction.z > 0f) {
                gestureSpin(angularSpeed);
            } else if (rightHand != null && rightIndex.Direction.z < -0.6f && rightMiddle.Direction.z > 0f && rightRing.Direction.z > 0f && rightPinky.Direction.z > 0f) {
                gestureSpin(angularSpeed);
            }
        }
    }

    void swipeX(float speed) {

    }

    void swipeY(float speed) {

    }

    void swipeZ(float speed) {

    }

    void keyTap() {

    }

    void sreenTap() {

    }

    void checkGesture() {
        //KAME
        if (leftHand != null && leftIndex.Direction.z < -0.6f && leftMiddle.Direction.z < -0.6f && leftRing.Direction.z < -0.6f && leftPinky.Direction.z < -0.6f) {
            if (rightHand != null && rightIndex.Direction.z < -0.6f && rightMiddle.Direction.z < -0.6f && rightRing.Direction.z < -0.6f && rightPinky.Direction.z < -0.6f) {
                float wristDist = Vector3.Distance(leftHand.WristPosition.ToUnity(), rightHand.WristPosition.ToUnity());
                if (wristDist < 60f) {
                    gestureKAME((60f - wristDist) / 60f);
                }
            }
        }

        //KIKOHO
        bool isKIKOHO = false;
        if (leftHand != null && leftIndex.Direction.y > 0.2f && leftMiddle.Direction.y > 0.2f && leftRing.Direction.y > 0.2f && leftPinky.Direction.y > 0.2f) {
            if (rightHand != null && rightIndex.Direction.y > 0.2f && rightMiddle.Direction.y > 0.2f && rightRing.Direction.y > 0.2f && rightPinky.Direction.y > 0.2f) {
                float indexDist = Vector3.Distance(leftIndex.TipPosition.ToUnity(), rightIndex.TipPosition.ToUnity());
                float thumbDist = Vector3.Distance(leftThumb.TipPosition.ToUnity(), rightThumb.TipPosition.ToUnity());
                if (indexDist < 30f && thumbDist < 40f) {
                    isKIKOHO = true;
                    gestureKIKOHO((70f - indexDist - thumbDist) / 70f);
                }
            }
        }
        if (!isKIKOHO) {
            kikohoTime = 0f;
            if (kikohoInstance != null) {
                Destroy(kikohoInstance);
                kikohoInstance = null;
            }
        }

        //THUNDER
        if ((rightHand != null && rightHand.PalmNormal.y < -0.5f && rightHand.PalmVelocity.y < -1000f) || (leftHand != null && leftHand.PalmNormal.y < -0.5f && leftHand.PalmVelocity.y < -1000f)) {
            float strength = 0f;
            if (rightHand != null) {
                strength -= rightHand.PalmVelocity.y;
            }
            if (leftHand != null) {
                strength -= leftHand.PalmVelocity.y;
            }
            gestureTHUNDER(Mathf.Min(1f, strength / 3000f));
        }

        //POISON
        if ((rightHand != null && rightHand.PalmNormal.y > 0.5f && rightHand.PalmVelocity.y > 500f) || (leftHand != null && leftHand.PalmNormal.y > 0.5f && leftHand.PalmVelocity.y > 500f)) {
            float strength = 0f;
            if (rightHand != null) {
                strength += rightHand.PalmVelocity.y;
            }
            if (leftHand != null) {
                strength += leftHand.PalmVelocity.y;
            }
            Debug.Log("POISON : " + strength);
        }


        //SWAVE
        if (rightHand != null && rightIndex.Direction.z < -0.6f && rightMiddle.Direction.z < -0.6f && rightRing.Direction.z > 0f && rightPinky.Direction.z > 0f) {
            float dist = Vector3.Distance(rightIndex.TipPosition.ToUnity(), rightMiddle.TipPosition.ToUnity());
            if (dist < 30f) {
                float strengh = Mathf.Max(0f, dist / 30f);
                gestureSWAVE(true, strengh);
            }
        }
        if (leftHand != null && leftIndex.Direction.z < 0.6f && leftMiddle.Direction.z < -0.6f && leftRing.Direction.z > 0f && leftPinky.Direction.z > 0f) {
            float dist = Vector3.Distance(leftIndex.TipPosition.ToUnity(), leftMiddle.TipPosition.ToUnity());
            if (dist < 30f) {
                float strengh = Mathf.Max(0f, dist / 30f);
                gestureSWAVE(false, strengh);
            }
        }
    }

    void updateHand() {
        frame = controller.Frame();
        updateHandComponent();
        moveByHand();
        stdGesture();
        checkGesture();
    }

    void updateHandComponent() {
        leftHand = null;
        rightHand = null;
        leftThumb = null;
        leftIndex = null;
        leftMiddle = null;
        leftRing = null;
        leftPinky = null;
        rightThumb = null;
        rightIndex = null;
        rightMiddle = null;
        rightRing = null;
        rightPinky = null;

        foreach (Hand hand in frame.Hands) {
            if (hand.IsRight) {
                rightHand = hand;
            }
            else if (hand.IsLeft) {
                leftHand = hand;
            }
        }

        if (leftHand != null) {
            foreach (Finger finger in leftHand.Fingers) {
                if (finger.Type == Finger.FingerType.TYPE_THUMB) {
                    leftThumb = finger;
                } else if (finger.Type == Finger.FingerType.TYPE_INDEX) {
                    leftIndex = finger;
                } else if (finger.Type == Finger.FingerType.TYPE_MIDDLE) {
                    leftMiddle = finger;
                } else if (finger.Type == Finger.FingerType.TYPE_RING) {
                    leftRing = finger;
                } else if (finger.Type == Finger.FingerType.TYPE_PINKY) {
                    leftPinky = finger;
                }
            }
        }

        if (rightHand != null) {
            foreach (Finger finger in rightHand.Fingers) {
                if (finger.Type == Finger.FingerType.TYPE_THUMB) {
                    rightThumb = finger;
                } else if (finger.Type == Finger.FingerType.TYPE_INDEX) {
                    rightIndex = finger;
                } else if (finger.Type == Finger.FingerType.TYPE_MIDDLE) {
                    rightMiddle = finger;
                } else if (finger.Type == Finger.FingerType.TYPE_RING) {
                    rightRing = finger;
                } else if (finger.Type == Finger.FingerType.TYPE_PINKY) {
                    rightPinky = finger;
                }
            }
        }
    }

    void moveByHand() {
        /*bool isTurnLeft = false;
        bool isTurnRight = false;
        if (rightHand != null) {
            if (rightIndex.Direction.z > 0f && rightMiddle.Direction.z > 0f && rightRing.Direction.z > 0f && rightPinky.Direction.z > 0f) {
                if (rightThumb.Direction.x < -0.8f) {
                    isTurnLeft = true;
                } else if (rightThumb.Direction.x > -0.4f) {
                    isTurnRight = true;
                }
            }
        }
        if (leftHand != null) {
            if (leftIndex.Direction.z > 0f && leftMiddle.Direction.z > 0f && leftRing.Direction.z > 0f && leftPinky.Direction.z > 0f) {
                if (leftThumb.Direction.x > 0.8f) {
                    isTurnRight = true;
                } else if (leftThumb.Direction.x < 0.4f) {
                    isTurnLeft = true;
                }
            }
        }

        if (isTurnLeft) {
            Turn(-0.5f);
        }
        if (isTurnRight) {
            Turn(0.5f);
        }*/

        if ((rightHand != null && rightHand.PalmPosition.x < -100f) || (leftHand != null && leftHand.PalmPosition.x < -150f)) {
            //MoveLeft(args.moveSpeed);
            Turn(-0.2f);
        }
        if ((rightHand != null && rightHand.PalmPosition.x > 150f) || (leftHand != null && leftHand.PalmPosition.x > 100f)) {
            //MoveRight(args.moveSpeed);
            Turn(+0.2f);
        }
        
        /*if ((rightHand != null && rightHand.PalmPosition.z < -50f) || (leftHand != null && leftHand.PalmPosition.z < -50f)) {
            MoveForward(args.moveSpeed);
        }
        if ((rightHand != null && rightHand.PalmPosition.z > 100f) || (leftHand != null && leftHand.PalmPosition.z > 100f)) {
            MoveBackward(args.moveSpeed);
        }

        if ((rightHand != null && rightHand.PalmNormal.y < -0.5f && rightHand.PalmVelocity.y > 500f) || (leftHand != null && leftHand.PalmNormal.y < -0.5f && leftHand.PalmVelocity.y > 500f)) {
            Jump();
        }*/
    }

    void stdGesture() {
        for (int i = 0; i < frame.Gestures().Count; i++) {
            Gesture gesture = frame.Gestures()[i];

            if (gesture.Type == Gesture.GestureType.TYPE_CIRCLE) {
                CircleGesture circleGesture = new CircleGesture(gesture);

                float radius = circleGesture.Radius;
                bool closewise = circleGesture.Pointable.Direction.AngleTo(circleGesture.Normal) <= Mathf.PI / 2;
                float speed = circleGesture.Pointable.TipVelocity.Magnitude;
                float angularSpeed = closewise ? speed / radius : -speed / radius;

                int directionId = getDirectionId(circleGesture.Normal);
                if (directionId == 0) {
                    circleX(angularSpeed);
                }
                else if (directionId == 1) {
                    circleY(angularSpeed);
                }
                else if (directionId == 2) {
                    circleZ(angularSpeed);
                }
            }
            else if (gesture.Type == Gesture.GestureType.TYPE_SWIPE) {
                SwipeGesture swipeGesture = new SwipeGesture(gesture);

                Vector direction = swipeGesture.Direction;
                float speed = swipeGesture.Speed;
                int directionId = getDirectionId(direction);
                if (directionId == 0) {
                    swipeX(speed);
                }
                else if (directionId == 1) {
                    swipeY(speed);
                }
                else if (directionId == 2) {
                    swipeZ(speed);
                }
            }
            else if (gesture.Type == Gesture.GestureType.TYPE_SCREEN_TAP) {
                sreenTap();
            }
            else if (gesture.Type == Gesture.GestureType.TYPE_KEY_TAP) {
                KeyTapGesture keyTapGesture = new KeyTapGesture(gesture);

                int id = keyTapGesture.Id;

                keyTap();
            }
        }
    }

    void gestureKAME(float strength) {
        GameObject kame = Instantiate(kameWave);
        HandWave script = kame.GetComponent<HandWave>();

        script.name = "KAME";
        kame.transform.position = handController.transform.TransformPoint(((leftHand.WristPosition + rightHand.WristPosition) / 2).ToUnityScaled(false));
        script.direction = handController.transform.TransformDirection((leftHand.Arm.Direction.ToUnity() + rightHand.Arm.Direction.ToUnity()).normalized);
        script.speed = 40f;
        script.agile = 1f;
        script.value = strength;
    }

    void gestureKIKOHO(float strength) {
        if (kikohoTime > 3f) {
            kikohoTime = 0f;
            if (kikohoInstance != null) {
                Destroy(kikohoInstance);
                kikohoInstance = null;
            }
            GameObject kikoho = Instantiate(kikohoWave);
            HandWave script = kikoho.GetComponent<HandWave>();

            script.name = "KIKOHO";
            kikoho.transform.position = handController.transform.TransformPoint(((leftHand.PalmPosition + rightHand.PalmPosition) / 2).ToUnityScaled(false));
            script.direction = (kikoho.transform.position - Camera.main.transform.position).normalized;//handController.transform.TransformDirection((leftHand.Arm.Direction.ToUnity() + rightHand.Arm.Direction.ToUnity()).normalized);
            script.speed = 25f;
            script.agile = 1f;
            script.value = strength;
        } else {
            kikohoTime += Time.deltaTime;
            if (kikohoInstance == null) {
                kikohoInstance = Instantiate(kikohoWaveSmall);
            }
            kikohoInstance.transform.position = handController.transform.TransformPoint(((leftHand.PalmPosition + rightHand.PalmPosition) / 2).ToUnityScaled(false));
        }
    }

    void gestureSWAVE(bool isRight, float strength) {
        GameObject swave = Instantiate(sWave);
        HandWave script = swave.GetComponent<HandWave>();

        script.name = "SWAVE";
        if (isRight) {
            swave.transform.position = handController.transform.TransformPoint(rightIndex.TipPosition.ToUnityScaled(false));
            script.direction = handController.transform.TransformDirection(rightIndex.Direction.ToUnity().normalized);
        } else {
            swave.transform.position = handController.transform.TransformPoint(leftIndex.TipPosition.ToUnityScaled(false));
            script.direction = handController.transform.TransformDirection(leftIndex.Direction.ToUnity().normalized);
        }
        script.speed = 25f;
        script.agile = 5f;
        script.value = strength;
    }

    void gestureTHUNDER(float strength) {
        GameObject thunder = Instantiate(thunderWave);
        HandWave script = thunder.GetComponent<HandWave>();

        script.name = "THUNDER";
        thunder.transform.position = handController.transform.position;
        Vector dir = new Vector();
        if (leftHand != null) {
            dir += leftHand.Arm.Direction;
        }
        if (rightHand != null) {
            dir += rightHand.Arm.Direction;
        }
        script.direction = handController.transform.TransformDirection(dir.ToUnity().normalized);
        script.speed = 100f;
        script.agile = 100f;
        script.value = strength;
    }

    void gestureSpin(float strength) {
        GameObject spin = Instantiate(spinWave);
        HandWave script = spin.GetComponent<HandWave>();

        script.name = "SPIN";
        spin.transform.position = handController.transform.position;
        Vector dir = new Vector();
        if (leftHand != null) {
            dir += leftIndex.Direction;
        }
        if (rightHand != null) {
            dir += rightIndex.Direction;
        }
        script.direction = handController.transform.TransformDirection(dir.ToUnity().normalized);
        script.speed = 100f;
        script.agile = 100f;
        script.value = strength;
    }
}
