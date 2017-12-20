using UnityEngine;
using System.Collections;

public class AppearWhenAllLevelsComplete : MonoBehaviour {

	public bool alwaysShow;


	private GameManager gameManager;
	private TextMesh text;
	private BoxCollider2D trigger;
	private bool b;

	// Use this for initialization
	void Start () {
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		text = GetComponentInChildren<TextMesh> ();
		trigger = GetComponentInChildren<BoxCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!alwaysShow) {
			b = (gameManager.CountLevels () >= 7) && gameManager.isBossLevelAvailable;
			text.gameObject.SetActive (b);
			trigger.gameObject.SetActive (b);
		}
	}
}
