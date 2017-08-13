using UnityEngine;
using System.Collections;

public class HandWave : MonoBehaviour {
    public Vector3 direction;
    public float speed;
    public float value;
    public float agile = 1f;
    public string name;

    public GameObject endEffect;

    private float destroyTime = 10f;
    private GameObject target = null;
    private bool stop = false;

    void Start () {
        direction.Normalize();
	}

    private void getTarget() {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("monster");

        float maxDanger = 0f;

        foreach (GameObject monster in monsters) {
            if (monster.GetComponent<ArgsController>().hp > 0f) {
                Vector3 aimDirection = monster.transform.position + new Vector3(0f, 1f, 0f) - transform.position;
                float danger = Vector3.Dot(aimDirection.normalized, direction) - aimDirection.magnitude / 50f;
                if (danger > maxDanger) {
                    maxDanger = danger;
                    target = monster;
                }
            }
        }
    }

	void Update () {
        getTarget();

        destroyTime -= Time.deltaTime;
        if (destroyTime <= 0f) {
            Destroy(gameObject);
            return;
        }

        float moveDist = speed * Time.deltaTime;
        if (target != null && target.GetComponent<ArgsController>().hp > 0f) {
            Vector3 aimDirection = target.transform.position + new Vector3(0f, 1f, 0f) - transform.position;
            if (Vector3.Dot(aimDirection, direction) < 0f) {
                if (!stop) {
                    if (destroyTime > 1f) {
                        destroyTime = 1f;
                    }
                }
                stop = true;
            }
            float dist = aimDirection.magnitude;

            float k = Mathf.Min(1f, agile * Time.deltaTime);
            direction = (k * aimDirection + (1f - k) * direction).normalized;

            if (dist < moveDist) {
                hitTarget();
                Destroy(gameObject);
                return;
            }

        }
        
        if (!stop) {
            transform.position = transform.position + direction * moveDist;
        }
	}

    void hitTarget() {
        if (endEffect != null) {
            Instantiate(endEffect, target.transform.position, transform.rotation);
        }

        if (name == "KAME") {
            target.GetComponent<ArgsController>().BeingAttacked(20 + 50 * value);
            Vector3 force = (transform.position - Camera.main.transform.position).normalized * 1f;
            target.transform.GetComponent<Rigidbody>().AddForce(force);
        } else if (name == "KIKOHO") {
            target.GetComponent<ArgsController>().BeingAttacked(2000 + 1000 * value);
            Vector3 force = new Vector3(Random.Range(-200f, 200f), 200f, Random.Range(-200f, 200f)) + (transform.position - Camera.main.transform.position).normalized * 100f;
            target.transform.GetComponent<Rigidbody>().AddForce(force);

            GameObject[] monsters = GameObject.FindGameObjectsWithTag("monster");
            foreach (GameObject monster in monsters) {
                if (monster != target) {
                    float dist = Vector3.Distance(monster.transform.position, target.transform.position);
                    if (dist < 20f) {
                        monster.GetComponent<ArgsController>().BeingAttacked(500f - 20f * dist);
                        force = new Vector3(Random.Range(-100f, 100f), 100f, Random.Range(-100f, 100f)) + (monster.transform.position - target.transform.position).normalized * 50f;
                        monster.transform.GetComponent<Rigidbody>().AddForce(force);
                    }
                }
            }
        } else if (name == "SWAVE") {
            target.GetComponent<ArgsController>().BeingAttacked(5 + 10 * value);
        } else if (name == "THUNDER") {
            target.GetComponent<ArgsController>().BeingAttacked(20 + 30 * value);
            Vector3 force = new Vector3(Random.Range(-100f, 100f), 100f, Random.Range(-100f, 100f)) + (transform.position - Camera.main.transform.position).normalized * 50f;
            target.transform.GetComponent<Rigidbody>().AddForce(force);

            GameObject[] monsters = GameObject.FindGameObjectsWithTag("monster");
            foreach (GameObject monster in monsters) {
                if (monster != target) {
                    float dist = Vector3.Distance(monster.transform.position, target.transform.position);
                    if (dist < 10f) {
                        monster.GetComponent<ArgsController>().BeingAttacked(20f - dist);
                        force = new Vector3(Random.Range(-50f, 50f), 50f, Random.Range(-50f, 50f)) + (monster.transform.position - target.transform.position).normalized * 50f;
                        monster.transform.GetComponent<Rigidbody>().AddForce(force);
                    }
                }
            }
        } else if (name == "POISON") {

        } else if (name == "SPIN") {
            target.GetComponent<ArgsController>().BeingAttacked(2);
            target.transform.eulerAngles = target.transform.eulerAngles + new Vector3(0f, value * 0.5f, 0f);
        }
    }
}
