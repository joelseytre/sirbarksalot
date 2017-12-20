using UnityEngine;
using System.Collections;

public class TriggerSoundLevelBoss : MonoBehaviour {

	public AudioClip leadup;
	public AudioClip main;

	private AudioSource source;
	private GameObject player;
	private bool inactive;

	// Use this for initialization
	void Start () {
		inactive = true;
		player = GameObject.Find ("Chien");
		source = GetComponent<AudioSource> ();
		source.clip = leadup;
		source.Play ();
		source.volume = 1f;
	}
	
	// Update is called once per frame
	void Update () {
		if (inactive && player.transform.position.x > 24f) {
			source.volume = 0.4f;
			inactive = false;
			source.clip = main;
			source.Play ();
		}
	}
}
