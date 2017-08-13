using UnityEngine;
using System.Collections;

public class MonsterStuff : MonoBehaviour {
	public GameObject[] stuffs;

	void Start () {

	}

	void Update () {
	
	}

	public void BlastStuffs() {
		for (int i = 0; i < stuffs.Length; i++)
			Instantiate (stuffs[i], transform.position, new Quaternion());
	}
}
