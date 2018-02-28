using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public bool[] whatLevelsAreComplete;
	public bool[] whatSecretsAreFound;
	public int deathToll;
	public AudioClip levelCompleteSound;
	public bool isBossLevelAvailable;
	public bool showFPS;

	private Text countText;
	private Text tollText;
	private Text playedText;
	private bool notDoneYet = true;
	private Animator anim;
	private Text GameOverText;
	private AudioSource source;
	private bool notFinishedYet;
	private float finalTime=0;

	void Awake(){
		if (instance == null) {
			whatLevelsAreComplete = new bool[SceneManager.sceneCountInBuildSettings];
			whatSecretsAreFound = new bool[SceneManager.sceneCountInBuildSettings];
			instance = this;
			deathToll = 0;
			for (int i = 0; i < SceneManager.sceneCount; i++) {
				whatLevelsAreComplete [i] = false;
				whatSecretsAreFound [i] = false;
			}
		}
		else if (instance!= this)
			Destroy(gameObject);
		DontDestroyOnLoad(gameObject);
		anim = GameObject.Find ("Canvas").GetComponent<Animator> ();
		GameOverText = GameObject.Find ("GameOverText").GetComponent<Text> ();
		source = FindObjectOfType<Camera> ().GetComponentInChildren<AudioSource> ();
		}

	void Update(){

//		in case the menu scene reloaded...
		if (anim == null) {
			anim = GameObject.Find ("Canvas").GetComponent<Animator> ();
			GameOverText = GameObject.Find ("GameOverText").GetComponent<Text> ();
			source = FindObjectOfType<Camera> ().GetComponentInChildren<AudioSource> ();
		}

		string sceneName = SceneManager.GetActiveScene ().name;
		if ((countText == null || tollText == null) && sceneName =="Menu")
			notDoneYet = true;
		if (sceneName == "Menu" && notDoneYet) {
			notDoneYet = false;
			SetUpMenu ();
			notFinishedYet = true;
		} else if (sceneName  != "Menu" && !notDoneYet) {
			notDoneYet = true;
		}

		if (GameObject.Find ("PlayedTime") != null && sceneName != "Menu")
			playedText = GameObject.Find ("PlayedTime").GetComponent<Text> ();
		if (GameObject.Find ("DeathToll") != null)
			tollText = GameObject.Find ("DeathToll").GetComponent<Text> ();

		if (playedText != null) {
			if (finalTime == 0)
				playedText.text = "Time Played: " + convertTime (Time.fixedTime);
			else
				playedText.text = "Total Time Played: " + convertTime (finalTime);
		}

		if (tollText != null)
			tollText.text = "Death toll: " + deathToll;

		if (isBossLevelAvailable) {
			if (sceneName == "Boss" && notFinishedYet) {
				if (GameObject.Find ("Boss") == null) {
					notFinishedYet = false;
					StartCoroutine (LoadCredits ());
					whatLevelsAreComplete [9] = true;
				}
			}
		} else {
			if (sceneName == "Menu" && notFinishedYet) {
				if (CountSecretsFound () >= 7) {
					notFinishedYet = false;
					StartCoroutine (LoadCredits ());
				}
			}
		}
		if(showFPS)
			print (1.0f / Time.deltaTime);
	}

	public string convertTime(float totalTime){
		int sec = (int)(totalTime % 60);
		int minutes = (int)(totalTime / 60) % 60;
		int hours = (int)(totalTime / 3600);

		return hours + ":" + minutes + ":" + sec;
	}

	private IEnumerator LoadCredits(){
		finalTime = Time.fixedTime;
		anim.SetBool ("Finished", true);
		GameOverText.text = "CONGRATULATIONS!";
		source.pitch = 1.3f;
		source.clip = levelCompleteSound;
		source.Play ();
		yield return new WaitForSeconds (3f);
		source.pitch = 1.3f;
		anim.SetBool ("Finished", false);
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Credits");
	}

	public int CountSecretsFound(){
		int count = 0;
		for(int i=0;i<whatSecretsAreFound.Length;i++)
			if (whatSecretsAreFound [i]) 
				count++;
		return count;
	}

	public int CountLevels(){
		int count = 0;
		for (int i = 0; i < whatLevelsAreComplete.Length; i++)
			if (whatLevelsAreComplete [i])
				count++;
		return count;
	}

	private int CountHowMuchObjectsCalled(string str){
	int namecount=0;
	GameObject[] allObjs = Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
	foreach (GameObject gobj in allObjs) {
		if(gobj.name == str)
			namecount++;
		}
	return namecount;
	}

	void SetUpMenu(){
		if (GameObject.Find ("SecretCount") != null)
			countText = GameObject.Find ("SecretCount").GetComponent<Text> ();
		else
			print ("pas de secret count");

		if (GameObject.Find ("DeathToll") != null)
			tollText = GameObject.Find ("DeathToll").GetComponent<Text> ();
		else
			print ("pas de death toll");

		if (GameObject.Find ("PlayedTime") != null)
			playedText = GameObject.Find ("PlayedTime").GetComponent<Text> ();
		else
			print ("pas de PlayedTime");

		tollText.text = "Death toll: " + deathToll;
		countText.text = "Secrets found: " + this.CountSecretsFound () + "/7";
	}
}
