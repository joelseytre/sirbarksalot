using UnityEngine;
using System.Collections;

public class KillThePlayer : MonoBehaviour {

	void Start(){

	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			GameObject go = GameObject.FindGameObjectWithTag("Player");
			GameOverManager gomanager = go.GetComponent<GameOverManager> ();
			gomanager.isPlayerAlive = false;
			switch (gameObject.tag) {
			case "FrogTongue":
				gomanager.reasonForDeath = "Is only the tip poisonous?";
				break;
			case "Frog":
				gomanager.reasonForDeath = "Touching that creature doesn't seem a healthy thing to do";
				break;
			case "TrapPlant":
				gomanager.reasonForDeath = "Snap!";
				break;
			case "KillZone":
				gomanager.reasonForDeath = "A true sir doesn't swim!";
				break;
			case "TrapLiane":
				gomanager.reasonForDeath = "This one ain't green";
				break;
			}

		}
	}
}
