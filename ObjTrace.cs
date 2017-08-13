using UnityEngine;
using System.Collections;

public class ObjTrace : MonoBehaviour {
	protected GameObject player;
	protected float height = 0.2f;
	protected float deltaPhi = 60f;
	protected float deltaTheta = 120f;
	protected float gainDist = 1f;
	protected float phi = 0;
	protected float theta = 0;
	protected bool centreRecorded = false;
	protected Vector3 centrePosition;
	public AudioClip gainSound = null;
	public bool addForce = true;
	protected bool destroying = false;
	protected float destoryTime = 0.5f;

	public void Start () {
		player = GameObject.Find ("player");
		if (addForce) {
			GetComponent<Rigidbody>().AddForce (Random.Range (-100, 100 + 1), 360, Random.Range (-100, 100 + 1));
			StartCoroutine(RecordCentrePosition (1f));
		} else {
			StartCoroutine(RecordCentrePosition (0f));
		}
	}

	IEnumerator RecordCentrePosition(float waitTime) {
		yield return new WaitForSeconds (waitTime);
		centreRecorded = true;
		centrePosition = transform.position;
	}

	public void Update () {
		theta += deltaTheta * Time.deltaTime;
		transform.eulerAngles = new Vector3 (0f, theta, 0f);
		if (centreRecorded) {
			phi -= deltaPhi * Time.deltaTime;
			transform.position = centrePosition + new Vector3 (0f, height * Mathf.Sin(phi * Mathf.Deg2Rad), 0f);
		}
	}

	public void CheckGain() {
		Vector3 pos = transform.position;
		pos = new Vector3 (pos.x, 0f, pos.z);
		Vector3 playerPos = player.transform.position;
		playerPos = new Vector3 (playerPos.x, 0f, playerPos.z);
		if (centreRecorded && Vector3.Distance(pos, playerPos) <= gainDist) {
			if (gainSound != null) {
				GetComponent<AudioSource>().PlayOneShot(gainSound);
			}
			Gain ();
			Destroy(gameObject, destoryTime);
			destroying = true;
		}
	}

	public virtual void Gain () {

	}
}
