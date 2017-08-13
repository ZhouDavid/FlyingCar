using UnityEngine;
using System.Collections;

public class ActionController : MonoBehaviour {
    protected GameObject player;
    protected ArgsController args;

    protected float idleSpeed = 1f;
    protected float runSpeed = 1f;
    protected float damageSpeed = 1f;
    protected float deadSpeed = 1f;
    protected float turnSpeed = 250f;
    protected float jumpForce = 250f;

    private bool grounded = true;

    public float stepLength = 4f;
    public float attackCD = 0f;
    public float damageCD = 0f;
    public float runCD = 0f;
    public bool died = false;
    private float attackEffectTime = 0.2f;
    private float stepFallTime = 0.15f;
    private bool attackEffected = true;
    private bool stepFallEffected = true;
    private GameObject attackTarget = null;

    public AudioClip attackSound;
    public AudioClip stepSound;
    public AudioClip deadSound;

    public void Start() {
        player = GameObject.Find("player");
        args = GetComponent<ArgsController>();
        Idle();
    }

    public void Update() {
        attackCD = Mathf.Max(attackCD - args.attackSpeed * Time.deltaTime, 0f);
        damageCD = Mathf.Max(damageCD - damageSpeed * Time.deltaTime, 0f);
        runCD = Mathf.Max(runCD - runSpeed * Time.deltaTime, 0f);
        CheckAttackEffect();
    }

    public void Idle() {
        GetComponent<Animation>()["Idle"].wrapMode = WrapMode.Loop;
        GetComponent<Animation>()["Idle"].speed = idleSpeed;
        GetComponent<Animation>().CrossFade("Idle");
    }

    public void Run() {
        runSpeed = args.moveSpeed / stepLength;

        GetComponent<Animation>()["Run"].wrapMode = WrapMode.Loop;
        GetComponent<Animation>()["Run"].speed = runSpeed;
        GetComponent<Animation>().CrossFade("Run");

        if (runCD == 0f) {
            stepFallEffected = false;
            runCD = 0.5f;
        }
        if (!stepFallEffected && 0.5f - runCD >= stepFallTime) {
            stepFallEffected = true;
            GetComponent<AudioSource>().PlayOneShot(stepSound);
        }
    }

    public void Attack() {
        if (attackCD == 0f && GetComponent<Animation>().IsPlaying("Attack01") == false) {
            attackCD = 1f;
            attackEffected = false;
            attackTarget = GetTarget();

            GetComponent<Animation>()["Attack01"].wrapMode = WrapMode.Once;
            GetComponent<Animation>()["Attack01"].speed = args.attackSpeed;
            GetComponent<Animation>().CrossFade("Attack01");
        }
    }

    public void Damage() {
        GetComponent<Animation>()["Damage"].wrapMode = WrapMode.Once;
        GetComponent<Animation>()["Damage"].speed = damageSpeed;
        GetComponent<Animation>().CrossFade("Damage");
    }

    public void Dead() {
        GetComponent<Animation>()["Dead"].wrapMode = WrapMode.ClampForever;
        GetComponent<Animation>()["Dead"].speed = damageSpeed;
        GetComponent<Animation>().CrossFade("Dead");

        if (died == false) {
            died = true;
            GetComponent<AudioSource>().PlayOneShot(deadSound);
        }
    }

    public void MoveForward(float s) {
        transform.position = transform.position + transform.forward * s * Time.deltaTime;
    }

    public void MoveBackward(float s) {
        transform.position = transform.position - transform.forward * s * Time.deltaTime;
    }

    public void MoveLeft(float s) {
        transform.position = transform.position - transform.right * s * Time.deltaTime;
    }

    public void MoveRight(float s) {
        transform.position = transform.position + transform.right * s * Time.deltaTime;
    }

    public void Turn(float x) {
        transform.eulerAngles = new Vector3(GetComponent<RemainStanding>().rotX, transform.eulerAngles.y + turnSpeed * x * Time.deltaTime, 0f);
    }

    public void Jump() {
        if (grounded) {
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce);
            grounded = false;
        }
    }

    public GameObject GetForwardObj() {
        float unitHeight = transform.GetComponent<CapsuleCollider>().height;
        Ray ray = new Ray(transform.position + transform.up * (unitHeight / 2), transform.forward);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        if (hit.collider != null)
            return hit.collider.gameObject;

        return null;
    }

    public GameObject GetTarget() {
        GameObject obj = GetForwardObj();
        if (obj == null || Vector3.Distance(transform.position, obj.transform.position) > args.range)
            return null;
        return obj;
    }

    void OnCollisionEnter() {
        grounded = true;
    }

    void CheckAttackEffect() {
        if (attackCD > 0f && 1f - attackCD > attackEffectTime / args.attackSpeed && !attackEffected) {
            attackEffected = true;

            if (attackTarget != null) {
                ArgsController targetArgs = attackTarget.GetComponent<ArgsController>();
                if (targetArgs != null) {
                    GameObject wave = (GameObject)Instantiate(args.weaponWave, transform.position, transform.rotation);
                    WaveController waveController = wave.GetComponent<WaveController>();
                    waveController.target = attackTarget;
                    waveController.damage = args.att;

                    if (attackSound != null && !(GetComponent<AudioSource>().clip == attackSound && GetComponent<AudioSource>().isPlaying)) {
                        GetComponent<AudioSource>().clip = attackSound;
                        GetComponent<AudioSource>().Play();
                    }
                }
                attackTarget = null;
            }
        }
    }
}
