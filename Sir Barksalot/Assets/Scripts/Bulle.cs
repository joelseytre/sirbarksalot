using UnityEngine;
using System.Collections;

public class Bulle : MonoBehaviour {

	private Transform UpLimit;
	private Transform DownLimit;
	private float deltaPos;
	private float delta;
	private float max;

	public float speed=1f;

	// Use this for initialization
	void Start () {
		UpLimit = transform.Find("UpLimit");
		DownLimit = transform.Find("DownLimit");
		max = UpLimit.position.y;
		delta = UpLimit.position.y - DownLimit.position.y;
		deltaPos = speed*0.02f;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (transform.position.y > max)
			transform.Translate (-delta* Vector3.up);
		else {
			transform.Translate (Vector3.up * deltaPos);
		}
	}
}
