using UnityEngine;
using System.Collections;

public class TakeOverCamForBoss : MonoBehaviour {

	private Camera2DFollow camScript;
	private Camera cam;
	private GameObject player;


	// Use this for initialization
	void Start () {
		player = GameObject.Find("Chien");
		cam = GetComponent<Camera> ();
		camScript = GetComponent<Camera2DFollow> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (player.transform.position.x > 24f)
			Doyourshit ();
	}

	private void Doyourshit(){
		camScript.enabled = false;
		cam.transform.position = new Vector3 (41.75f, -23.7f, -10f);
		cam.orthographicSize = 10.75f;
	}
}