using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TrapLiane : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			GameOverManager gomanager = other.GetComponent<GameOverManager> ();
			gomanager.isPlayerAlive = false;
			gomanager.reasonForDeath = "This one ain't green";
		}
	}
}
