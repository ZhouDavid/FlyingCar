using UnityEngine;
using System.Collections;

public class StuffTrace : ObjTrace {
	public int myType;

	void Start () {
		base.Start ();
		deltaPhi = 120f;
		deltaTheta = 120f;
	}

	void Update () {
		base.Update ();
		if (!destroying)
			CheckGain ();
	}

	public override void Gain() {
		player.GetComponent<StuffController>().GetStuff(myType);
	}
}
