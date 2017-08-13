using UnityEngine;
using System.Collections;
using Leap;

public class moveleap : MonoBehaviour {
    public GameObject me;
    protected float turnSpeed = 250f;
    public myFps fpsControl;
    Controller controller = null;
    Frame frame = null;
    Hand leftHand = null;
    Hand rightHand = null;
    void updateHand(){
        leftHand = null;
        rightHand = null;
        foreach (Hand hand in frame.Hands)
        {
            if (hand.IsRight)
            {
                rightHand = hand;
            }
            else if (hand.IsLeft)
            {
                leftHand = hand;
            }
        }
    }
	// Use this for initialization
	void Start () {
        controller = new Controller();
	}
	
	// Update is called once per frame
    public void Turn(float x)
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + turnSpeed * x * Time.deltaTime, 0f);
    }
	void Update () {
        frame = controller.Frame();
        Vector3 fix =new Vector3(0, 0.2f, 0);
        transform.position = me.transform.position + me.transform.forward + fix * 0.5f;
        //fpsControl.Turn(-0.2f);
        transform.rotation = me.transform.rotation;
        updateHand();
        if (leftHand != null && rightHand != null) {
            Vector line = leftHand.PalmPosition - rightHand.PalmPosition;
            float arg;
            if (Mathf.Abs(line.x) > 0.001f)
                arg = Mathf.Atan(line.y / line.x);
            else
                arg = 0;
            //Debug.Log("hand line" + (line) + " " +arg);
            fpsControl.Turn(arg * -0.2f);
            
        }
        
	}
}
