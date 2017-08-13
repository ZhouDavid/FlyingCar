using UnityEngine;
using System.Collections;

public class MonsterController : ActionController {
	void Start () {
		base.Start ();
	}

	void Update () {
		base.Update ();

		if (args.hp == 0f) {
			if (GetComponent<Animation>().IsPlaying("Dead") == false) {
				MonsterDead();
			}
			Dead();
		} else if (Active ()) {
			if (ShouldAttack()) {
				Attack ();
			} else if (GetComponent<Animation>().IsPlaying("Attack01") == false) {
				Run ();
				TurnToPlayer();
				MoveForward(args.moveSpeed);
			}
		} else {
			if (GetComponent<Animation>().IsPlaying("Attack01") == false && GetComponent<Animation>().IsPlaying("Damage") == false)
				Idle();
		}
	}

	void TurnToPlayer() {
		Vector3 dir = player.transform.position - transform.position;
		float rot = Mathf.Acos (Vector3.Dot (dir.normalized, Vector3.forward.normalized)) * Mathf.Rad2Deg;
		if (Vector3.Dot(Vector3.right, dir) < 0)
			rot = -rot;
		rot -= transform.eulerAngles.y;
		while (rot < 0f) rot += 360f;
		while (rot > 180f) rot -=360f;
		float deltaRot = Mathf.Min(turnSpeed * Time.deltaTime, Mathf.Abs(rot));
		if (rot < 0f)
			deltaRot = -deltaRot;
		transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y + deltaRot, 0f);
	}

	void MonsterDead() {
		GetComponent<MonsterStuff> ().BlastStuffs ();
		Destroy (gameObject, 5f);
	}

	bool Active() {
		if (player.GetComponent<ArgsController>().hp == 0)
			return false;
		if (damageCD > 0f)
			return true;
		float dist = Vector3.Distance (transform.position, player.transform.position);
		if (dist > args.view)
			return false;
		Vector3 dir = player.transform.position - transform.position;
		float angle = Mathf.Acos (Vector3.Dot (dir.normalized, transform.forward.normalized)) * Mathf.Rad2Deg;
		if (angle > args.fieldOfView / 2 + 90f / Mathf.Max(1f, dist))
			return false;

        return true;
		/*float unitHeight = transform.GetComponent<CapsuleCollider>().height;
		Ray ray = new Ray (transform.position + new Vector3 (0f, unitHeight / 2, 0f), dir);
		RaycastHit hit;
		Physics.Raycast (ray, out hit);
		if (hit.collider == null)
			return false;
		return (hit.collider.gameObject == player);*/
	}

	bool ShouldAttack() {
		return (GetTarget () == player) && player.GetComponent<ArgsController>().hp > 0;
	}
}
