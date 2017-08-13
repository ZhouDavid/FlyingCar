using UnityEngine;
using System.Collections;

public class MagicCircle : MonoBehaviour {
	private GameObject player;
	private ArgsController args;
	public GameObject[] monsters;
	public GameObject effect;
	public float criticalDist = 10f;
	public float deltaHp = 0f;
	public AudioClip sound;
	public float cd = 0f;
	public int id;
    private float maxCD = 1f;
    private bool waiting = false;

	void Start () {
		player = GameObject.Find ("player");
		args = player.GetComponent<ArgsController> ();
	}

	void Update () {
        //if (Vector3.Distance(transform.position, player.transform.position) < criticalDist) {
        if (waiting == false && GameObject.FindGameObjectsWithTag("monster").Length == 0) {
            waiting = true;
        }

        if (waiting) {
			cd = Mathf.Max (cd - Time.deltaTime, 0f);
			if (cd == 0f) {
                waiting = false;
				cd = maxCD;

				if (sound != null) {
					GetComponent<AudioSource>().PlayOneShot(sound);
				}

				/*if (deltaHp > 0f) {
					args.hp = Mathf.Min (args.hp + deltaHp, args.maxHp);
				} else {
					args.BeingAttacked(-deltaHp);
				}*/

				if (monsters.Length > 0) {
                    if (monsters[id] != null) {
                        Instantiate(monsters[id], transform.position, transform.rotation);
                        if (effect != null)
                            Instantiate(effect, transform.position, transform.rotation);
                    }
					id++;
					if (id == monsters.Length) {
						id = 0;
					}
				}
			}
		}
	}
}
