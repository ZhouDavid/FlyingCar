using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour {
	public Texture[] stuffTexture;
	public Texture unknownTexture, noneTexture, panelTexture, texturePureWhite;
	private StuffController stuff;
	private ArgsController args;

	private float panelOffsetX = 0f;
	private float panelOffsetY = 0f;
	private float panelSizeX = 180f;
	private float panelSizeY = 420f;

	private float layoutOffsetX = 20f;
	private float layoutOffsetY = 60f;
	private float layoutSizeX = 25f;
	private float layoutSizeY = 25f;
	private float layoutSpaceX = 5f;
	private float layoutSpaceY = 5f;

	private float stuffOffsetX = 20f;
	private float stuffOffsetY = 250f;
	private float stuffSizeX = 25f;
	private float stuffSizeY = 25f;
	private float stuffSpace = 5f;

	private float argsOffsetX = 10f;
	private float argsOffsetY = 10f;
	private float argsSizeX = 300f;
	private float argsSizeY = 20f;
	private float weaponNameOffsetX = 30f;
	private float weaponNameOffsetY = 280f;
	private float weaponPicOffsetX = 30f;
	private float weaponPicOffsetY = 300f;
	private float zoomPixel = 3f;

	private bool showCursor = false;
	private int selectedStuff = 0;
    private float livingTime = 0f;
    private Vector3 originPosition;

	void Start () {
		args = GetComponent<ArgsController> ();
		stuff = GetComponent<StuffController> ();
        originPosition = transform.position;
	}

	void Update () {
        livingTime += Time.deltaTime;
		showCursor = GetComponent<PlayerController> ().showCursor;
        if (showCursor) {
            transform.position = originPosition;
        }
	}

	void OnGUI() {
		//GUI.color = GetMainColor ();

		//Rect panelRect = new Rect (panelOffsetX, panelOffsetY, panelSizeX, panelSizeY);
		//GUI.DrawTexture (panelRect, panelTexture);

		//DrawLayout ();
		//DrawStuff ();
		DrawArgs ();
	}

	void DrawLayout() {
		GUI.color = GetMainColor ();

		for (int r = 0; r < stuff.maxR; r++) {
			for (int c = 0; c < stuff.maxC; c++) {
				Texture texture = GetLayoutTexture(stuff.layout[r, c]);
				float offsetX = layoutOffsetX + c * (layoutSizeX + layoutSpaceX);
				float offsetY = layoutOffsetY + r * (layoutSizeY + layoutSpaceY);
				Rect rect = new Rect(offsetX, offsetY, layoutSizeX, layoutSizeY);
				if (showCursor && MouseHover(rect)) {
					if (Input.GetMouseButton(0)) {
						rect = Zoom(rect, -zoomPixel);
						stuff.InstallStuff(r, c, selectedStuff);
					} else if (Input.GetMouseButton(1)) {
						rect = Zoom(rect, -zoomPixel);
						stuff.RemoveStuff(r, c);
					} else {
						rect = Zoom(rect, zoomPixel);
					}
				}
				GUI.DrawTexture(rect, texture);
			}
		}
	}

	void DrawStuff() {
		GUI.color = GetMainColor ();

		for (int i = 0; i < stuff.maxStuff; i++) {
			Texture texture = GetStuffTexture(i);
			float offsetX = stuffOffsetX + i * (stuffSizeX + stuffSpace);
			float offsetY = stuffOffsetY;
			Rect rect = new Rect(offsetX, offsetY, stuffSizeX, stuffSizeY);
			if (showCursor && MouseHover(rect)) {
				if (Input.GetMouseButton(0)) {
					rect = Zoom(rect, -zoomPixel);
					selectedStuff = i;
				} else if (Input.GetMouseButtonDown(1)) {
					rect = Zoom(rect, -zoomPixel);
					selectedStuff = i;
					stuff.TradeStuff(i);
				} else {
					rect = Zoom(rect, zoomPixel);
				}
			}
			if (i == selectedStuff) {
				GUI.color = GetMainColor() * Color.green;
				rect = Zoom(rect, zoomPixel);
				GUI.DrawTexture(rect, texturePureWhite);
				GUI.color = GetMainColor();
				rect = Zoom(rect, -zoomPixel);
			}
			GUI.DrawTexture(rect, texture);
			GUI.color = GetMainColor() * Color.cyan;
			GUI.Label(rect, "" + stuff.number[i]);
			GUI.color = GetMainColor();
		}
	}

	void DrawArgs() {
		GUI.color = GetMainColor ();

		Rect rect;
		rect = new Rect (weaponNameOffsetX, weaponNameOffsetY, 200f, 20f);
		string weaponName = GetWeaponName ();
		GUI.Label(rect, weaponName);
		if (args.weaponTexture != null) {
			rect = new Rect (weaponPicOffsetX, weaponPicOffsetY, args.weaponTexture.width, args.weaponTexture.height);
			GUI.Label (rect, args.weaponTexture);
		}

		GUI.color = GetMainColor () * Color.red;
		rect = new Rect (argsOffsetX, argsOffsetY + 0 * argsSizeY, argsSizeX, argsSizeY);
		GUI.Label (rect, "HP = " + args.hp + " / " + args.maxHp + " (+ " + args.deltaHp + ")");

        GUI.color = GetMainColor() * Color.green;
        rect = new Rect(argsOffsetX, argsOffsetY + 1 * argsSizeY, argsSizeX, argsSizeY);
        GUI.Label(rect, "Living time = " + livingTime + " s");

        GUI.color = GetMainColor() * Color.yellow;
        rect = new Rect(argsOffsetX, argsOffsetY + 2 * argsSizeY, argsSizeX, argsSizeY);
        int monsterNum = 0;
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("monster");
        foreach (GameObject monster in monsters) {
            if (monster.GetComponent<ArgsController>().hp > 0f) {
                monsterNum++;
            }
        }
        GUI.Label(rect, "Monsters = " + monsterNum);

        /*GUI.color = GetMainColor () * Color.red;
		rect = new Rect (argsOffsetX, argsOffsetY + 1 * argsSizeY, argsSizeX, argsSizeY);
		GUI.Label (rect, "att = " + args.att + " (+ " + (args.attBonus - 1) * 100 + "%)");
		
		GUI.color = GetMainColor () * Color.yellow;
		rect = new Rect (argsOffsetX, argsOffsetY + 2 * argsSizeY, argsSizeX, argsSizeY);
		GUI.Label (rect, "def = " + args.def + " (+ " + (args.defBonus - 1) * 100 + "%)");
		
		GUI.color = GetMainColor () * Color.green;
		rect = new Rect (argsOffsetX, argsOffsetY + 3 * argsSizeY, argsSizeX, argsSizeY);
		GUI.Label (rect, "moveSpeed = " + args.moveSpeed + " (+ " + (args.speedBonus - 1) * 100 + "%)");

		GUI.color = GetMainColor () * Color.green;
		rect = new Rect (argsOffsetX, argsOffsetY + 4 * argsSizeY, argsSizeX, argsSizeY);
		GUI.Label (rect, "attSpeed = " + args.attackSpeed + " (+ " + (args.speedBonus - 1) * 100 + "%)");

		GUI.color = GetMainColor () * Color.cyan;
		rect = new Rect (argsOffsetX, argsOffsetY + 5 * argsSizeY, argsSizeX, argsSizeY);
		GUI.Label (rect, "range = " + args.range + " (+ " + (args.speedBonus - 1) * 100 + "%)");*/
    }

	string GetWeaponName() {
		string str = args.weaponName;
		if (str == "")
			return "";

		if (args.weaponQuality < 1.5f) {
			str = "木质" + str;
		} else if (args.weaponQuality < 2.5f) {
			str = "铜质" + str;
		} else if (args.weaponQuality < 3.5f) {
			str = "铁质" + str;
		} else if (args.weaponQuality < 4.5f) {
			str = "银质" + str;
		} else {
			str = "金质" + str;
		}

		if (args.weaponComplish < 0.8f) {
			str = "残缺的" + str;
		} else if (args.weaponComplish < 1f) {
			str = "破损的" + str;
		} else {
			str = "完整的" + str;
		}
		return str;
	}

	Color GetMainColor() {
		//if (showCursor)
			return Color.white;
		//return Color.white * 0.7f;
	}

	Texture GetStuffTexture(int type) {
		if (type >= 0 && type < stuff.maxStuff && stuff.number[type] >= 1) {
			return stuffTexture[type];
		}
		return unknownTexture;
	}

	Texture GetLayoutTexture(int type) {
		if (type >= 0 && type < stuff.maxStuff) {
			return stuffTexture[type];
		}
		return noneTexture;
	}

	bool MouseHover(Rect rect) {
		Vector2 mousePosition = Input.mousePosition;
		mousePosition = new Vector2 (mousePosition.x, Screen.height - mousePosition.y);
		if (rect.Contains(mousePosition))
			return true;
		return false;
	}

	Rect Zoom(Rect rect, float pixel) {
		return new Rect(rect.x - pixel, rect.y - pixel, rect.width + 2 * pixel, rect.height + 2 * pixel);
	}
}
