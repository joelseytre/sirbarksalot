using UnityEngine;
using System.Collections;

public class SecretPath : MonoBehaviour {

	public bool render = true;

	private SpriteRenderer[] SR;
	private AudioSource audioS;

	void OnTriggerStay2D(Collider2D other){
		if (other.tag == "Player") {
			render = false;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if(other.tag == "Player"){
			render = true;
		}
	}

	void Start(){
		audioS = gameObject.GetComponent<AudioSource> ();
		SR = gameObject.GetComponentsInChildren<SpriteRenderer> ();
	}

	void Update(){
		if (!render) {
			foreach (SpriteRenderer sr in SR) {
				sr.enabled = false;
			}
		} else {
			//SR = GetComponentsInChildren<SpriteRenderer> ();
			foreach (SpriteRenderer sr in SR) {
				sr.enabled = true;
			}
			audioS.Play ();
		}
	}
}