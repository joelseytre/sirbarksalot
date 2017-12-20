using UnityEngine;
using System.Collections;

public class Poisson : MonoBehaviour {

	private Transform LeftLimit;
	private Transform RightLimit;
	private float deltaPos;
	private float max;
	private float min;

	public float speed=1f;

	// Use this for initialization
	void Start () {
		RightLimit = transform.Find("RightLimit");
		LeftLimit = transform.Find("LeftLimit");
		max = RightLimit.position.x;
		min = LeftLimit.position.x;
		deltaPos = speed * 0.02f;
	}


	// Update is called once per frame
	void FixedUpdate(){
		if (transform.position.x < min)
			transform.RotateAround (transform.position, Vector3.up, 180);
		else if (transform.position.x > max)
			transform.RotateAround (transform.position, Vector3.up, 180);
		transform.Translate (deltaPos * Vector3.right);
	}
}
