using UnityEngine;
using System.Collections;

public class ArgsController : MonoBehaviour {
	private ActionController action;
	private bool lighten = false;
	public Texture2D texturePureWhite;
	public Texture weaponTexture;
	public GameObject weaponWave;
	public string weaponName;
	public float hp = 100f;
	public float maxHp = 100f;
	public float deltaHp = 2f;
	public float att = 20f;
	public float def = 3f;
	public float range = 2f;
	public float moveSpeed = 4f;
	public float attackSpeed = 1f;

	public float view = 10f;
	public float fieldOfView = 150f;
	public float weaponComplish = 1f;
	public float weaponQuality = 1f;
	public float attBonus = 1f;
	public float defBonus = 1f;
	public float speedBonus = 1f;

	private GameObject player;

	void Start () {
		player = GameObject.Find ("player");
		action = GetComponent<ActionController> ();
	}

	void Update () {
		RestoreHealth ();
	}

	void OnGUI() {
		ShowStrand ();
	}

	void RestoreHealth() {
		if (hp > 0f)
			hp = Mathf.Min(hp + deltaHp * Time.deltaTime, maxHp);
	}

	void ShowStrand() {
		if (InsidePlayerView() == false)
			return;
		float unitHeight = transform.GetComponent<CapsuleCollider>().height;
		Vector3 worldPosition = transform.position + transform.up * (unitHeight + 0.1f);
		Vector2 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

		Vector3 unitRay = worldPosition - Camera.main.transform.position;
		if (Vector3.Dot(unitRay, Camera.main.transform.forward) < 0)
			return;
		
		RaycastHit hit;
		Physics.Raycast (Camera.main.ScreenPointToRay(screenPosition), out hit);
		if (hit.collider != null && hit.collider.gameObject != player) {
			Vector3 hitRay = hit.point - Camera.main.transform.position;
			if (hitRay.magnitude < unitRay.magnitude) {
				return;
			}
		}

		screenPosition = new Vector2(screenPosition.x, Screen.height - screenPosition.y);
		float hpWidth = hp / maxHp;
		GUI.color = GetStrandColor();
		GUI.DrawTexture (new Rect (screenPosition.x - 40, screenPosition.y - 3, 80, 6), texturePureWhite);
		GUI.color = GetStrandColor() * ((gameObject.name == "player") ? Color.blue : Color.red);
		GUI.DrawTexture (new Rect (screenPosition.x - 38, screenPosition.y - 2, 76 * hpWidth, 4), texturePureWhite);
		if (TargetedByPlayer()) {
			//GUI.color = Color.yellow;
			//GUI.Label(new Rect (screenPosition.x, screenPosition.y + 3, 80, 20), "Attack!");
		}
	}

	bool InsidePlayerView() {
		float dist = Vector3.Distance (player.transform.position, transform.position);
		return (dist <= player.GetComponent<ArgsController> ().view);
	}

	bool TargetedByPlayer() {
		return player.GetComponent<PlayerController>().GetTarget() == gameObject;
	}

	Color GetStrandColor() {
		//if (player == gameObject || TargetedByPlayer()) {
			return Color.white * 0.8f;
		//}
		
		//return Color.white * 0.3f;
	}

	public void BeingAttacked(float damage) {
		action.damageCD = 1f;
		if (GetComponent<Animation>().IsPlaying("Attack01") == false)
			action.Damage();
		hp = Mathf.Max (hp - Mathf.Max(damage - def, 0f) , 0f);
	}

	public void RestoreHp(float x) {
		hp = Mathf.Min (hp + x, maxHp);
	}
}
