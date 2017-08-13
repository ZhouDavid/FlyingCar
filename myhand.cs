using UnityEngine;
using System.Collections;
using Leap;

public class myhand : MonoBehaviour {
    Controller controller = null;
    float speed = 5;
    Frame frame = null;
	// Use this for initialization
	void Start () {
        controller = new Controller();

        controller.EnableGesture(Gesture.GestureType.TYPE_CIRCLE);
       /* controller.Config.SetFloat("Gesture.Circle.MinRadius", 10.0f);
        controller.Config.SetFloat("Gesture.Circle.MinArc", 0.5f * Mathf.PI);
        controller.Config.Save();*/

        controller.EnableGesture(Gesture.GestureType.TYPE_SWIPE);
       /* controller.Config.SetFloat("Gesture.Swipe.MinLength", 2.0f);
        controller.Config.SetFloat("Gesture.Swipe.MinVelocity", 100.0f);
        controller.Config.Save();*/

        controller.EnableGesture(Gesture.GestureType.TYPE_SCREEN_TAP);
        /*controller.Config.SetFloat("Gesture.ScreenTap.MinForwardVelocity", 30.0f);
        controller.Config.SetFloat("Gesture.ScreenTap.HistorySeconds", 0.5f);
        controller.Config.SetFloat("Gesture.ScreenTap.MinDistance", 1.0f);*/

        controller.EnableGesture(Gesture.GestureType.TYPE_KEY_TAP);
        /*controller.Config.SetFloat("Gesture.KeyTap.MinDownVelocity", 20.0f);
        controller.Config.SetFloat("Gesture.KeyTap.HistorySeconds", 0.1f);
        controller.Config.SetFloat("Gesture.KeyTap.MinDistance", 1.0f);
        controller.Config.Save();*/

	}
	
	// Update is called once per frame
	void Update () {
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * speed;
        transform.Translate(x, 0, z);
        frame = controller.Frame();
        for (int i = 0; i < frame.Gestures().Count; i++)
        {
            Gesture gesture = frame.Gestures()[i];
            if (gesture.Type == Gesture.GestureType.TYPE_SWIPE)
                Debug.Log(i + " TYPE_SWIPE");
            if (gesture.Type == Gesture.GestureType.TYPE_CIRCLE)
                Debug.Log(i + " TYPE_CIRCLE");
            if (gesture.Type == Gesture.GestureType.TYPE_SCREEN_TAP)
                Debug.Log(i + " TYPE_SCREEN_TAP");
            if (gesture.Type == Gesture.GestureType.TYPE_KEY_TAP)
                Debug.Log(i + " TYPE_KEY_TAP");

        }
	}
}
