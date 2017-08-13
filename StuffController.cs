using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

public class StuffController : MonoBehaviour {
	public Texture[] weaponTexture;
	public GameObject emptyWave;
	public GameObject[] weaponWave;
	public AudioClip emptySound;
	public AudioClip[] weaponSound;
	public int maxStuff = 5;
	public int maxR = 6;
	public int maxC = 5;

	public int[] number;
	public int[,] layout;
	private int weaponNumber = 16;
	private float lastTradeTime = 0f;

	public class WeaponArgs {
		public string name = "";
		public string expression = "00000" + "00000" + "00000" + "00000" + "00000" + "00000";
		public Texture texture = null;
		public GameObject weaponWave = null;
		public AudioClip weaponSound = null;
		public float maxHp = 100f;
		public float deltaHp = 2f;
		public float att = 20f;
		public float def = 3f;
		public float range = 2f;
		public float moveSpeed = 4f;
		public float attackSpeed = 1f;

		public void SetArgs(float _maxHp, float _deltaHp, float _att, float _def, float _range, float _moveSpeed, float _attackSpeed) {
			maxHp = _maxHp;
			deltaHp = _deltaHp;
			att = _att;
			def = _def;
			range = _range;
			moveSpeed = _moveSpeed;
			attackSpeed = _attackSpeed;
		}
	}
	private WeaponArgs[] weaponArgs;

	void Start () {
		layout = new int[maxR, maxC];

		InputExpression ();

		for (int r = 0; r < maxR; r++) {
			for (int c = 0; c < maxC; c++) {
				layout[r, c] = -1;
			}
		}
	}

	void Update () {
	
	}

	void InputExpression() {
		weaponArgs = new WeaponArgs[weaponNumber];
		for (int i = 0; i < weaponNumber; i++) {
			weaponArgs[i] = new WeaponArgs();
			weaponArgs[i].texture = weaponTexture[i];
			weaponArgs[i].weaponWave = weaponWave[i];
			weaponArgs[i].weaponSound = weaponSound[i];
		}

		weaponArgs[0].name = "斧头";
		weaponArgs[0].expression =
			"01010" +
			"01110" +
			"01110" +
			"01010" +
			"01000" +
			"01000";
		weaponArgs [0].SetArgs (200f, 3f, 45f, 12f, 2.5f, 3.5f, 0.8f);
		
		weaponArgs[1].name = "尖刺斧头";
		weaponArgs[1].expression =
			"01010" +
			"01110" +
			"11111" +
			"01110" +
			"01010" +
			"01000";
		weaponArgs [1].SetArgs (300f, 4f, 80f, 25f, 2.5f, 3.3f, 0.8f);
		
		weaponArgs[2].name = "棒子";
		weaponArgs[2].expression =
			"01110" +
			"01110" +
			"01110" +
			"00100" +
			"00100" +
			"00100";
		weaponArgs [2].SetArgs (200f, 3f, 35f, 12f, 2.7f, 3.8f, 1.2f);

		weaponArgs[3].name = "重型棒子";
		weaponArgs[3].expression =
			"11111" +
			"01110" +
			"01110" +
			"00100" +
			"00100" +
			"01110";
		weaponArgs [3].SetArgs (300f, 4f, 50f, 18f, 2.7f, 3.6f, 1.1f);
		
		weaponArgs[4].name = "狼牙棒";
		weaponArgs[4].expression =
			"11110" +
			"11110" +
			"11110" +
			"01100" +
			"01100" +
			"01100";
		weaponArgs [4].SetArgs (400f, 5f, 70f, 22f, 2.7f, 3.5f, 1f);
		
		weaponArgs[5].name = "咸鱼棒";
		weaponArgs[5].expression =
			"00110" +
			"01111" +
			"01111" +
			"01110" +
			"01100" +
			"11110";
		weaponArgs [5].SetArgs (500f, 6f, 75f, 25f, 2.8f, 3.7f, 1.4f);

		weaponArgs[6].name = "短刀";
		weaponArgs[6].expression =
			"00000" +
			"00000" +
			"00100" +
			"00100" +
			"01110" +
			"00100";
		weaponArgs [6].SetArgs (200f, 3f, 25f, 8f, 2f, 4f, 2f);
		
		weaponArgs[7].name = "短弯刀";
		weaponArgs[7].expression =
			"01000" +
			"00100" +
			"00110" +
			"01100" +
			"00100" +
			"00000";
		weaponArgs [7].SetArgs (300f, 4f, 35f, 12f, 2.2f, 3.9f, 2f);
		
		weaponArgs[8].name = "短锯刀";
		weaponArgs[8].expression =
			"00000" +
			"10000" +
			"11000" +
			"10000" +
			"11100" +
			"10000";
		weaponArgs [8].SetArgs (400f, 5f, 50f, 15f, 2.2f, 3.9f, 1.8f);

		weaponArgs[9].name = "锅铲";
		weaponArgs[9].expression =
			"00000" +
			"01110" +
			"01110" +
			"00100" +
			"00100" +
			"00100";
		weaponArgs [9].SetArgs (400f, 5f, 55f, 10f, 2f, 4f, 2.5f);
		
		weaponArgs[10].name = "重锤";
		weaponArgs[10].expression =
			"11111" +
			"11111" +
			"11111" +
			"00100" +
			"00100" +
			"00100";
		weaponArgs [10].SetArgs (500f, 6f, 150f, 20f, 3.5f, 2.5f, 0.6f);
		
		weaponArgs[11].name = "弯拐杖";
		weaponArgs[11].expression =
			"00100" +
			"01110" +
			"00010" +
			"00100" +
			"00010" +
			"00100";
		weaponArgs [11].SetArgs (200f, 3f, 45f, 15f, 7f, 3.8f, 0.67f);
		
		weaponArgs[12].name = "水晶拐杖";
		weaponArgs[12].expression =
			"01010" +
			"01110" +
			"01110" +
			"00100" +
			"00100" +
			"00100";
		weaponArgs [12].SetArgs (300f, 4f, 55f, 16f, 7.5f, 3.8f, 0.67f);
		
		weaponArgs[13].name = "山型拐杖";
		weaponArgs[13].expression =
			"10101" +
			"01110" +
			"01110" +
			"00100" +
			"00100" +
			"01000";
		weaponArgs [13].SetArgs (400f, 5f, 65f, 17f, 7.5f, 3.8f, 0.6f);
		
		weaponArgs[14].name = "猫爪拐杖";
		weaponArgs[14].expression =
			"01110" +
			"11111" +
			"01110" +
			"00100" +
			"01000" +
			"00100";
		weaponArgs [14].SetArgs (500f, 6f, 80f, 18f, 7.5f, 3.8f, 0.6f);
		
		weaponArgs[15].name = "长剑";
		weaponArgs[15].expression =
			"00010" +
			"00010" +
			"00010" +
			"00111" +
			"00010" +
			"00010";
		weaponArgs [15].SetArgs (500f, 6f, 100f, 25f, 3.2f, 3f, 1f);
	}

	public void GetStuff(int type) {
		if (type >= 0 && type < maxStuff) {
			number[type]++;
		}
	}

	public void RemoveStuff(int r, int c) {
		if (layout[r, c] != -1) {
			number[layout[r, c]]++;
			layout[r, c] = -1;
			UpdateWeapon();
		}
	}

	public void TradeStuff(int type) {
		if (Time.time == lastTradeTime)
			return;
		lastTradeTime = Time.time;
		if (number[type] >= 1) {
			number[type]--;
			if (Random.Range(0, 5) == 0) {
				GetComponent<ArgsController>().RestoreHp(50f * (type + 1));
			} else if (Random.Range(0, 10) == 0) {
				if (type + 1 < maxStuff)
					number[type + 1]++;
			}
		}
	}

	public void InstallStuff(int r, int c, int type) {
		if (type >= 0 && type < maxStuff && number[type] >= 1) {
			RemoveStuff (r, c);
			number[type]--;
			layout[r, c] = type;
			UpdateWeapon();
		}
	}

	void UpdateWeapon() {
		int id = -1;
		float maxFit = 0.6f;
		for (int i = 0; i < weaponNumber; i++) {
			int cnt = 0;
			for (int j = 0; j < weaponArgs[i].expression.Length; j++)
				if (weaponArgs[i].expression[j] == '1') {
					cnt++;
				}

			float fit = 0;
			for (int r = 0; r < maxR; r++) {
				for (int c = 0; c < maxC; c++) {
					if (layout[r, c] != -1) {
						if (weaponArgs[i].expression[r * maxC + c] == '1')
							fit += 1f;
						else
						    fit -= 1f;
					}
				}
			}
			fit /= cnt;

			if (fit > maxFit) {
				maxFit = fit;
				id = i;
			}
		}



		if (id == -1)
			SetWeapon(-1, 1f);
		else
			SetWeapon (id, maxFit);
	}

	void SetWeapon(int type, float complished) {
		GetComponent<WeaponController> ().SetWeapon (type);

		int totalCnt = 0, totalQuality = 0;
		int attCnt = 0, attQ = 0;
		int defCnt = 0, defQ = 0;
		int speedCnt = 0, speedQ = 0;
		for (int r = 0; r < maxR; r++) {
			for (int c = 0; c < maxC; c++) {
				if (layout[r, c] != -1) {
					totalCnt++;
					totalQuality += layout[r, c] + 1;
					if (r < maxR / 3) {
						attCnt++;
						attQ += layout[r, c] + 1;
					} else if (r < maxR * 2 / 3) {
						defCnt++;
						defQ += layout[r, c] + 1;
					} else {
						speedCnt++;
						speedQ += layout[r, c] + 1;
					}
				}
			}
		}
		float quality = (totalCnt == 0) ? 1f : (float)totalQuality / totalCnt;
		float attBonus = (attCnt == 0) ? 1f : (float)attQ / attCnt;
		float defBonus = (defCnt == 0) ? 1f : (float)defQ / defCnt;
		float speedBonus = (speedCnt == 0) ? 1f : Mathf.Pow ((float)speedQ / speedCnt, 0.3f);
		float k = Mathf.Pow(complished * quality, 0.5f);

		ArgsController args = GetComponent<ArgsController> ();
		WeaponArgs weapon = (type == -1) ? (new WeaponArgs()) : weaponArgs [type];
		args.weaponComplish = complished;
		args.weaponQuality = quality;
		args.attBonus = attBonus;
		args.defBonus = defBonus;
		args.speedBonus = speedBonus;
		args.weaponTexture = weapon.texture;
		if (type == -1) {
			args.weaponWave = emptyWave;
			GetComponent<PlayerController>().attackSound = emptySound;
		} else {
			args.weaponWave = weapon.weaponWave;
			GetComponent<PlayerController>().attackSound = weapon.weaponSound;
		}
		args.weaponName = weapon.name;

		float hpRate = (float)args.hp / args.maxHp;
		args.maxHp = weapon.maxHp * k;
		args.hp = args.maxHp * hpRate;

		args.deltaHp = weapon.deltaHp;
		args.att = weapon.att * k * attBonus;
		args.def = weapon.def * k * defBonus;
		args.range  = weapon.range * speedBonus;
		args.moveSpeed = weapon.moveSpeed * speedBonus;
		args.attackSpeed = weapon.attackSpeed * speedBonus;
	}
}
