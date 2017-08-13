using UnityEngine;
using System.Collections;

public class PlayerDie : MonoBehaviour {
	public string levelName = "";
	public float fallDownY = -1f;
	public float fallDownDamage = 100f;
	private float dieTime = 0f;
	private float fallDownCd = 0f;

	void Start () {
	
	}

	void Update () {
		CheckFallDown ();

		ArgsController args = GetComponent<ArgsController> ();
		if (args != null && args.hp == 0f) {
            GetComponent<RemainStanding>().enabled = false;
			dieTime += Time.deltaTime;
			if (dieTime >= 5f) {
				Application.LoadLevel (levelName);
			}
		}
	}

	void CheckFallDown() {
		fallDownCd = Mathf.Max (fallDownCd - Time.deltaTime, 0f);
		
		if (transform.position.y < fallDownY) {
			ArgsController args = GetComponent<ArgsController>();
			if (args != null) {
				if (fallDownCd == 0f) {
					fallDownCd = 1f;
					args.BeingAttacked(fallDownDamage);
				}
			}
		}
	}
}
