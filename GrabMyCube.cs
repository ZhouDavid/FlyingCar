using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Leap;
public class GrabMyCube : MonoBehaviour
{
    public GameObject cubePrefab;
    public HandController hc;
    private HandModel hm;
    /*public Text lblNoDeviceDetected;
    public Text lblLeftHandPosition;
    public Text lblLeftHandRotation;
    public Text lblRightHandPosition;
    public Text lblRightHandRotation;*/
    // Use this for initialization
    void Start()
    {
        hc.GetLeapController().EnableGesture(Gesture.GestureType.TYPECIRCLE);
        hc.GetLeapController().EnableGesture(Gesture.GestureType.TYPESWIPE);
        hc.GetLeapController().EnableGesture(Gesture.GestureType.TYPE_SCREEN_TAP);
        //hc.GetLeapController().Config.SetFloat("Gesture.Circle.MinRadius", 10.0f);
        //hc.GetLeapController().Config.SetFloat("Gesture.Circle.MinArc", 0.5f * Mathf.PI);
        //hc.GetLeapController().Config.Save();
        //cube = cubePrefab;
        if (this.cube == null)
        {
            this.cube = GameObject.Instantiate(this.cubePrefab,
                                               this.cubePrefab.transform.position,
                                               this.cubePrefab.transform.rotation) as GameObject;
        }
    }

    private GameObject cube = null;
    // Update is called once per frame
    Frame currentFrame;
    Frame lastFrame = null;
    Frame thisFrame = null;
    long difference = 0;
    void Update()
    {
        this.currentFrame = hc.GetFrame();
        GestureList gestures = this.currentFrame.Gestures();
        foreach (Gesture g in gestures)
        {
            Debug.Log(g.Type);

            if (g.Type == Gesture.GestureType.TYPECIRCLE)
            {
                // create the cube ...
                if (this.cube == null)
                {
                    this.cube = GameObject.Instantiate(this.cubePrefab,
                                                       this.cubePrefab.transform.position,
                                                       this.cubePrefab.transform.rotation) as GameObject;
                }
                //this.cube.transform.position = 
            }
            if (g.Type == Gesture.GestureType.TYPESWIPE)
            {
                if (this.cube != null)
                {
                    Destroy(this.cube);
                    this.cube = null;
                }
            }
        }

        foreach (var h in hc.GetFrame().Hands)
        {
            
            if (h.IsRight)
            {
                //this.lblRightHandPosition.text = string.Format("Right Hand Position: {0}", h.PalmPosition.ToUnity());
                //this.lblRightHandRotation.text = string.Format("Right Hand Rotation: ", h.Direction.Pitch, h.Direction.Yaw, h.Direction.Roll);

                if (this.cube != null)
                    this.cube.transform.rotation = Quaternion.EulerRotation(h.Direction.Pitch, h.Direction.Yaw, h.Direction.Roll);

                foreach (var f in h.Fingers)
                {
                    if (f.Type == Finger.FingerType.TYPE_INDEX)
                    {
                        // this code converts the tip position from leap motion to unity world position
                        Leap.Vector position = f.TipPosition;
                        Vector3 unityPosition = position.ToUnityScaled(false);
                        Vector3 worldPosition = hc.transform.TransformPoint(unityPosition);

                        //string msg = string.Format("Finger ID:{0} Finger Type: {1} Tip Position: {2}", f.Id, f.Type(), worldPosition);
                        //Debug.Log(msg);
                    }
                }

            }
            if (h.IsLeft)
            {
                //this.lblLeftHandPosition.text = string.Format("Left Hand Position: {0}", h.PalmPosition.ToUnity());
                //this.lblLeftHandRotation.text = string.Format("Left Hand Rotation: ", h.Direction.Pitch, h.Direction.Yaw, h.Direction.Roll);

                if (this.cube != null)
                    this.cube.transform.rotation = Quaternion.EulerRotation(h.Direction.Pitch, h.Direction.Yaw, h.Direction.Roll);
            }

        }

    }
}

