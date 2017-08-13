using UnityEngine;
using System.Collections;

public class WaveController : MonoBehaviour {
	public GameObject bloodEffect;
	public GameObject target;
	public float damage;
	private float speed = 10f;
	private bool destroying = false;

	void Start () {
		transform.position = transform.position + new Vector3 (0f, 0.5f, 0f);
	}

	void Update () {
		if (target == null) {
			Destroy(gameObject);
			return;
		}
		if (destroying)
			return;
		Vector3 centrePos = target.transform.position + new Vector3 (0f, 0.5f, 0f);
		float dist = Vector3.Distance (centrePos, transform.position);
		if (dist <= speed * Time.deltaTime) {
			destroying = true;
			target.GetComponent<ArgsController>().BeingAttacked(damage);
			Vector3 bloodPosition = transform.position - new Vector3(0f, 0.5f, 0f);
			Instantiate(bloodEffect, bloodPosition, transform.rotation);
			Destroy(gameObject, 1f);
		} else {
			Vector3 dir = (centrePos - transform.position).normalized;
			transform.position = transform.position + dir * speed * Time.deltaTime;
		}
	}
}
