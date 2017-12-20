using UnityEngine;
using System.Collections;

public class SlidingSlope : MonoBehaviour {

	public float disablingTime=1f;

	private Platformer2DUserControl playerController;
	private static bool isEffective;


	// Use this for initialization
	void Start () {
		playerController = GameObject.FindGameObjectWithTag ("Player").GetComponent<Platformer2DUserControl> ();
		isEffective = false;
	}

	void OnTriggerStay2D(Collider2D other){
		if (other.tag == "Player") {
			playerController.enabled = false;
			isEffective = true;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Player") {
			StartCoroutine (BackToPlayer (disablingTime));
			isEffective = false;
		}
	}

	IEnumerator BackToPlayer(float time){
		yield return new WaitForSeconds (time);
		if(!isEffective)
			playerController.enabled = true;
	}
}
