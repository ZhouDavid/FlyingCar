using UnityEngine;
using System.Collections;

public class RemainStanding : MonoBehaviour {
	GameObject player;
	ArgsController args;
	private float rotSpeed = 1.5f;
	private float minRotX = -30f;
	private float maxRotX = 15f;
	public float rotX = 0f;

	void Start () {
		player = GameObject.Find ("player");
		args = player.GetComponent<ArgsController> ();
	}

	void Update () {
		transform.eulerAngles = new Vector3 (rotX, transform.eulerAngles.y, 0f);
		if (gameObject == player && args.hp > 0f && player.GetComponent<PlayerController>().showCursor == false) {
			rotX -= rotSpeed * Input.GetAxis("Mouse Y");
			if (rotX < minRotX) rotX = minRotX;
			if (rotX > maxRotX) rotX = maxRotX;
		}
	}
}
