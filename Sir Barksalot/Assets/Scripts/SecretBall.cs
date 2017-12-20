using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SecretBall : MonoBehaviour {

	private GameManager gameManager;
	private AudioSource audioS;
	private bool done;

	// Use this for initialization
	void Start () {
		done = false;
		if (GameObject.Find ("GameManager") != null)
			gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		else
			print ("pas de gameMangager...");
		audioS = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.tag != "Player")
			return;
		if (done)
			return;
		done = true;
		if (gameManager !=null)
			gameManager.whatSecretsAreFound [SceneManager.GetActiveScene ().buildIndex] = true;
		audioS.Play ();
		Destroy(gameObject.GetComponent<SpriteRenderer> ());
		Destroy(gameObject,0.8f);
	}
}