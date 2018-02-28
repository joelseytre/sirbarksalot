using UnityEngine;
using System.Collections;

public class TrapPlant : MonoBehaviour {

	private bool seesPlayer;
	private bool closeTrap;
	private float deltaTime;
	private AudioSource source;
	private JointMotor2D moteurL, moteurR;
	private bool soundPlayed;
	private GameObject gobj;
	private BoxCollider2D deathZone;

	public float mSpeed;
	public float mTorque;
	public float trapTime = 200;
	public float trapRest = 50;
	public float trapWait = 5;
	public bool disable;




	void OnTriggerStay2D(Collider2D other) {
		if (other.tag == "Player") {
			seesPlayer = true;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Player") {
			seesPlayer = false;
		}
	}

	// Use this for initialization
	void Start () {
//		gobj = GetComponent<GameObject>;
		seesPlayer = false;
		closeTrap = false;
		deltaTime = 0.0f;
		moteurL = new JointMotor2D();
		moteurR = new JointMotor2D();
		mSpeed = 1000;
		mTorque = 1000;
		moteurL.maxMotorTorque = mTorque;
		moteurL.motorSpeed = mSpeed;
		moteurR.maxMotorTorque = mTorque;
		moteurR.motorSpeed = -mSpeed;
		source = GetComponent<AudioSource> ();
		soundPlayed = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		bool isLeafActive;
		if (seesPlayer && deltaTime <= 0.0f) {
			deltaTime = trapTime;
		} else if (seesPlayer && deltaTime <= (trapTime - trapWait)) {
			closeTrap = true;
		} if (deltaTime <= trapRest) {
			closeTrap = false;
		}
		deltaTime -= 1.0f;
		if (closeTrap) {
			if (deltaTime <= (trapTime - trapRest)) {
				moteurL.motorSpeed = mSpeed;
				moteurR.motorSpeed = -mSpeed;
			}
		} else if (deltaTime <= trapWait) {
				moteurL.motorSpeed = -mSpeed;
				moteurR.motorSpeed = mSpeed;
		}
		HingeJoint2D[] HJ;
		HJ = GetComponentsInChildren<HingeJoint2D> ();
		foreach (HingeJoint2D hj in HJ) {
			deathZone = hj.transform.Find ("Teeth").GetComponent<BoxCollider2D> ();

			if (hj.name == "Left") {
				
				hj.motor = moteurL;
				hj.useMotor = true;
			}
			if (hj.name == "Right") {
				hj.motor = moteurR;
				hj.useMotor = true;
			}

			if (hj.gameObject.name == "Left") {
				disable = (hj.jointAngle > 75f && hj.jointAngle < 100f);
//				print (disable);
			}

			if (hj.gameObject.name == "Right") {
				disable = (hj.jointAngle > -100f && hj.jointAngle < -75f);
//				print (disable);
			}

			isLeafActive = (Mathf.Abs (hj.jointAngle) >= 70);
			foreach (PolygonCollider2D coll in GetComponentsInChildren<PolygonCollider2D>()) {
				if (coll.name == "Leaf" || coll.name == "Slider") {
					coll.enabled = isLeafActive;
				}
			}

			if (isLeafActive && !soundPlayed){
				source.Play ();
				soundPlayed = true;
			}
			else if (!isLeafActive)
				soundPlayed = false;

			deathZone.enabled = !disable;
		}

	}
}
