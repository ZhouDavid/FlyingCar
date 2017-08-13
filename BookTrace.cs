using UnityEngine;
using System.Collections;

public class BookTrace : ObjTrace {
	public Texture content;
	
	void Start () {
		base.Start ();
		deltaPhi = 180f;
		deltaTheta = 45f;
	}
	
	void Update () {
		base.Update ();
		if (!destroying)
			CheckGain ();
	}

	public override void Gain() {
		player.GetComponent<BookUI>().GetABook(GetComponent<Renderer>().material.mainTexture, content);
	}
}
