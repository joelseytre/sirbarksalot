using UnityEngine;
using System.Collections;

public class MouvementLiane : MonoBehaviour
{

	private HingeJoint2D HJ;
	private bool done=false;

	public float maxAngle=30;
	public float mSpeed=100;
	public float mTorque=1000;
	public bool isMoving=false;


	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			isMoving = true;
			if (!done) {
				done = true;
				if (other.GetComponent<Rigidbody2D>().velocity.x >= 0)
					mSpeed *= -1;
			}
		}
	}
		
	void Update()
	{
		if (isMoving) {
			JointMotor2D moteur = new JointMotor2D ();
			moteur.motorSpeed = mSpeed;
			moteur.maxMotorTorque = mTorque;

			HJ = GetComponent<HingeJoint2D> ();
			if ((HJ.jointAngle >= maxAngle && mSpeed > 0 && HJ.jointAngle <= 180) || (HJ.jointAngle >= -180 && HJ.jointAngle <= -maxAngle && mSpeed < 0)) {
				mSpeed *= -1;
			}
			HJ.motor = moteur;
		}
	}
}


