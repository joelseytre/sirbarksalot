using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour {

	private AudioSource source;
	private GameObject player;
	private bool active;
	private GameManager gameManager;
	private Animator anim;

	public AudioClip levelCompleteSound;

	void Start(){
		if(GameObject.Find("GameManager") != null)
			gameManager = GameObject.Find ("GameManager").GetComponent<GameManager>();
		active = false;
		Camera mainCam = FindObjectOfType<Camera> ();
		source = mainCam.GetComponentInChildren<AudioSource> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		anim = player.GetComponent<Animator> ();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player" && !active)
		{
			if (gameManager !=null)
				gameManager.whatLevelsAreComplete [SceneManager.GetActiveScene ().buildIndex] = true;
			player.GetComponent<GameOverManager> ().isInvincible = true;
			player.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezePositionX;
			source.clip = levelCompleteSound;
			source.Play ();
			StartCoroutine (LoadMenu (Time.unscaledTime + 4.4f));
			active = true;
			anim.SetBool ("Victory", true);
		}
	}

	IEnumerator LoadMenu(float finishTime){
		yield return new WaitForSeconds (0.2f);
		Time.timeScale = 0.00001f;
		while (Time.unscaledTime < finishTime) {
			if (Input.GetKeyDown (KeyCode.Space))
				BackToMenu ();
			yield return null;
		}
		BackToMenu ();
	}

	private void BackToMenu(){
		anim.SetBool ("Victory", false);
		Time.timeScale = 1f;
		SceneManager.LoadScene ("Menu");
	}
}
