using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour {

	private Animator anim;
	private Text reasonForDeath;
	private bool done;
	private GameOverManager gomanager;
	private GameManager gameManager;
	private Image secretItem;

	void Awake(){
		gomanager = GameObject.FindGameObjectWithTag("Player").GetComponent<GameOverManager> ();
		if (GameObject.Find ("GameManager") != null)
			gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		secretItem = GameObject.Find ("SecretItemUI").GetComponent<Image>();
		secretItem.enabled = false;
		anim = GetComponent<Animator> ();
		done = false;
		Text[] texts = GetComponentsInChildren<Text> ();
		foreach (Text text in texts)
			if (text.name == "ReasonForDeath")
				reasonForDeath = text;
	}

	void Update () {
		if (gomanager != null && gomanager.isPlayerAlive == false && !done){
			done = true;
			anim.SetTrigger ("GameOver");
			reasonForDeath.text = gomanager.reasonForDeath;
		}
		if (gameManager != null && gameManager.whatSecretsAreFound [SceneManager.GetActiveScene ().buildIndex])
			secretItem.enabled = true;
	}
}
