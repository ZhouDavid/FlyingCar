using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {
	public Transform weaponHand;
	public Transform[] weapons;
	public int startType = -1;

	private Transform currentWeapon;

	void Start () {
		SetWeapon (startType);
	}

	void Update () {
	
	}

	public void SetWeapon(int type) {
		if (type == -1) {
			RemoveCurrentWeapon();
		} else {
			type %= weapons.Length;
			RemoveCurrentWeapon();
			currentWeapon = (Transform)Instantiate(weapons[type], weaponHand.position, weaponHand.rotation);
			currentWeapon.parent = weaponHand;
		}
	}

	void RemoveCurrentWeapon() {
		if (currentWeapon != null) {
			currentWeapon.parent = null;
			Destroy(currentWeapon.transform.gameObject);
		}
	}
}
