using UnityEngine;
using System.Collections;

public class SkinController : MonoBehaviour {
	public Texture[] textureColor;
	public int startType = -1;

	void Start () {
		SetSkin (startType);
	}

	void Update () {
	
	}

	public void SetSkin(int type) {
		if (type != -1) {
			type %= textureColor.Length;
			GetComponent<Renderer>().material.mainTexture = textureColor[type];
		}
	}
}
