using UnityEngine;
using System.Collections;

public class Pill : MonoBehaviour {

	public bool shrink = true;



	private AudioSource audioS;
	private bool done;

	// Use this for initialization
	void Start () {
		done = false;
		audioS = GetComponent<AudioSource> ();
	}

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.tag != "Player")
			return;
		if (done)
			return;
		done = true;
		if (shrink)
			coll.gameObject.GetComponent<PlatformerCharacter2D> ().Shrink();
		else
			coll.gameObject.GetComponent<PlatformerCharacter2D> ().Grow ();
		audioS.Play ();
		Destroy(gameObject.GetComponent<SpriteRenderer> ());
		Destroy(gameObject,0.8f);
	}
}
