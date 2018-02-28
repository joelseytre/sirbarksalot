using UnityEngine;
using System.Collections;
//using UnityEditor.SceneManagement;

public class StartScene : MonoBehaviour {
	
	public int level = 0;
	public Sprite spriteTBD;
	public Sprite spriteDone;

	private string thatScene = "Scene ";
	private GameManager gameManager;
	private SpriteRenderer rend;

	void Start(){
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		thatScene += level.ToString ();
		rend = gameObject.GetComponent<SpriteRenderer> ();
	}

	private void OnTriggerEnter2D(){
		if (level == 0)
			UnityEngine.SceneManagement.SceneManager.LoadScene ("Menu");
		else if(level ==9)
			UnityEngine.SceneManagement.SceneManager.LoadScene ("Boss");
		else
			UnityEngine.SceneManagement.SceneManager.LoadScene (thatScene);
	}

	void Update(){
		if (gameManager.whatLevelsAreComplete [level])
			rend.sprite = spriteDone;
		else
			rend.sprite = spriteTBD;
		if (gameManager.whatSecretsAreFound [level])
			return;
	}
}
