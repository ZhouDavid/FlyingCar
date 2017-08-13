using UnityEngine;
using System.Collections;

public class cubeColor : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Renderer>().material.color = Color.red;
	}
	
	// Update is called once per frame
	void Update () {
        //GetComponent<Renderer>().material.color = Color.yellow;
	}
}
