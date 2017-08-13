using UnityEngine;
using System.Collections;

public class JewelTrace : ObjTrace {
	public string levelName;
	private float destoryCd = 0f;
	
	void Start () {
		base.Start ();
		deltaPhi = 60f;
		deltaTheta = -90f;
		destoryTime = 3f;
	}

	void Update () {
		base.Update ();
		if (!destroying) {
			CheckGain ();
		} else {
			destoryCd += Time.deltaTime;
			if (destoryCd > 2f) {
				Application.LoadLevel (levelName);
			}
		}
	}
	
	public override void Gain() {

	}
}
