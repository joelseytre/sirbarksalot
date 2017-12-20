using UnityEngine;
using System.Collections;

public class Frog : MonoBehaviour {

	private bool seesPlayer;
	private bool tongueOut;
	public float activatedTime = 200f;
	public float restTime = 50f;
	public float idleTime = 5f;
	public float stretchTime = 50f;
	public float tongueSize=3.5f;
	public Sprite spriteBoucheFermée;
	public Sprite spriteBoucheOuverte;
	public GameObject gouttePoison;

	private SpriteRenderer tongueRend;
	private SpriteRenderer sprRend;
	private float deltaTime;
	private Transform bodyTrsf;
	private Transform spikesTrsf;
	private Transform tongueTrsf;
	private Transform trsf;
	private float pointer = 0;




	void OnTriggerStay2D(Collider2D other) {
		if (other.tag == "Player") {
			seesPlayer = true;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Player") {
			seesPlayer = false;
		}
	}

	// Use this for initialization
	void Start () {
		Transform[] transform = GetComponentsInChildren<Transform> ();
		foreach (Transform transf in transform) {
			if (transf.name == "Tongue")
				trsf = transf;
		seesPlayer = false;
		tongueOut = false;
		deltaTime = 0.0f;
		SpriteRenderer[] rends = gameObject.GetComponentsInChildren<SpriteRenderer> ();
		foreach (SpriteRenderer rend in rends)
			if (rend.tag == "FrogBody") {
				sprRend = rend;
				bodyTrsf = rend.GetComponent<Transform> ();
			} else if (rend.name == "TongueSpikes") {
				spikesTrsf = rend.GetComponent<Transform> ();
				tongueRend = rend;
			} else if (rend.tag == "FrogTongue") {
				tongueTrsf = rend.GetComponent<Transform> ();
			}
		}
	}

	void CheckSprite(Transform trsf){
		if (trsf.localScale.x <= 0.1f) {
			sprRend.sprite = spriteBoucheFermée;
			tongueRend.enabled = false;
			bodyTrsf.localPosition = new Vector3 (-0.6f, 0.4f, 0f);
		} else{
			sprRend.sprite = spriteBoucheOuverte;
			tongueRend.enabled = true;
			spikesTrsf.position = tongueTrsf.position + new Vector3(-0.14f,-0.12f,0);
			SpawnGoutte ();
			bodyTrsf.localPosition = new Vector3 (-0.475f, 0.35f, 0f);
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
//		print (seesPlayer);
		if (seesPlayer && deltaTime <= 0.0f) {
			deltaTime = activatedTime;
		} else if (deltaTime>stretchTime/2 && deltaTime <= (activatedTime - idleTime)) {
			tongueOut = true;
		} if (deltaTime <= stretchTime/2) {
			tongueOut = false;
		}
		deltaTime -= 1.0f;
		if (tongueOut) {
					CheckSprite (trsf);
					if(deltaTime<=(activatedTime-stretchTime)) {
						trsf.localScale = new Vector3 (tongueSize, 1.0f,1.0f);
						trsf.localPosition = new Vector3(-(tongueSize/2)-1.4f, .11f, 0.0f);
					} else {
						trsf.localScale += new Vector3 (tongueSize/restTime, 0.0f, 0.0f);
						trsf.localPosition += new Vector3(-tongueSize/(2*restTime), 0.0f, 0.0f);
					}
		} else {
		CheckSprite (trsf);
		if (deltaTime <= (stretchTime/2) && deltaTime>=0) {
			trsf.localScale -= new Vector3 (tongueSize/(restTime/2), 0.0f, 0.0f);
			trsf.localPosition -= new Vector3 (-tongueSize/restTime, 0.0f, 0.0f);
		} else {
			trsf.localScale = new Vector3 (0, 1.0f, 1.0f);
			trsf.localPosition = new Vector3 (-1.4f, .11f, 0.0f);					
			}			
		}	
	}		
	void SpawnGoutte(){
		if (Time.time < pointer + 1.2f)
			return;
		GameObject instanceGoutte = Instantiate (gouttePoison, spikesTrsf.position, Quaternion.identity) as GameObject;
		Destroy (instanceGoutte, 0.6f);
		pointer = Time.time;
	}
}
