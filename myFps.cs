using UnityEngine;
using System.Collections;

public class myFps : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
    public void Turn(float x)
    {
        float turnSpeed = 250f;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + turnSpeed * x * Time.deltaTime, 0f);
    }
	// Update is called once per frame
	void Update () {
	
	}
}
