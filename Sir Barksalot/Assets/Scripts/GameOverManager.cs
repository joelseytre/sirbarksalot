using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour {

	public bool isPlayerAlive;
	public bool isInvincible=false;
	public string reasonForDeath;
	public AudioClip gameOverSound;

	private float restartTime;
	private float restartDelay;
	private AudioSource source;
	private bool justOnce;
	private Transform trsf;
	private Camera mainCam;
	private Transform player;
	private bool storeSecretValue;
	private GameManager gameManager;
	private Animator anim;



	// Use this for initialization
	void Start () {
		if (GameObject.Find ("GameManager") != null) {
			gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
			storeSecretValue = gameManager.whatSecretsAreFound [SceneManager.GetActiveScene ().buildIndex];
		}
		isPlayerAlive = true;
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		reasonForDeath = "";
		restartDelay = 5;
		mainCam = FindObjectOfType<Camera> ();
		trsf = mainCam.GetComponent<Transform> ();
		source = mainCam.GetComponentInChildren<AudioSource> ();
		justOnce = false;
		anim = player.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isInvincible == true) {
			if (isPlayerAlive == false) {
				Debug.Log ("t'es mort, en théorie");
				source.clip = gameOverSound;
				source.Play();
				isPlayerAlive = true;
			}
		}
		if (isPlayerAlive == false) {
			if (!justOnce) {
				source.clip = gameOverSound;
				source.Play();
				justOnce = true;
				if (gameManager != null)
					gameManager.deathToll++;
				else
					print ("pas de gameManager...");
				Time.timeScale = 0.00001f;
				restartTime = Time.realtimeSinceStartup + restartDelay;
				trsf.Translate ((player.position - trsf.position)/2f);
				anim.SetBool ("isDead", true);
			}
			if (Time.realtimeSinceStartup > restartTime || Input.GetKeyDown(KeyCode.Space)) {
				Restart ();
			}
		}
//		print (mainCam.aspect);
	}

	private void Restart(){
		anim.SetBool ("isDead", false);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name) ;
		isPlayerAlive = true;
		reasonForDeath = "";
		Time.timeScale = 1;
		if (gameManager != null)
			gameManager.whatSecretsAreFound [SceneManager.GetActiveScene ().buildIndex] = storeSecretValue;
	}
}
