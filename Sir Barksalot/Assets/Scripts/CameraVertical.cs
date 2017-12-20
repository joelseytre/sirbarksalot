using UnityEngine;
using System.Collections;

public class CameraVertical : MonoBehaviour {

	public float yAxis = 0;
	public Camera C;

	private Camera2DFollow CF;
	private bool inside = false;


	void Start(){
		CF = C.GetComponent<Camera2DFollow> ();
	}

	void OnTriggerStay2D(Collider2D other){
		if(other.tag == "Player"){
			inside = true;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Player") {
			inside = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (inside) {
			CF.setYaxis (yAxis);
		} else {
			CF.setYaxis (0);
		}
	}
}
