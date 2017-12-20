using UnityEngine;
using System.Collections;

public class Zoziau : MonoBehaviour {

	private Transform bec;
	private Transform target;
	private bool attacking=false;
	private SpriteRenderer spriteRenderer;
	private AudioSource source;

	public GameObject lune;
	public float distanceDetection = 10f;
	public float waitTime = 1.5f;
	public float maxThrow = 40f;
	public Sprite sprite1;
	public Sprite sprite2;


	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		bec = transform.Find ("Bec");
		target = GameObject.Find ("Chien").transform;
		source = GetComponent<AudioSource> ();
	}

	void Update(){
		if ((Vector2.Distance (target.position, bec.position) <= distanceDetection) && !attacking && target.position.y <= (bec.position.y + 0.5f)) {
			attacking = true;
			StopAllCoroutines();
			StartCoroutine(Throw(waitTime, target));
		} else if (Vector2.Distance (target.position, bec.position) > distanceDetection) {
			attacking = false;
			StopAllCoroutines();
		}
	}

	IEnumerator Throw(float waitTime, Transform target){
		yield return new WaitForSeconds (waitTime);
		Quaternion randomRotation = Quaternion.Euler (0f, 0f, Random.Range (0f, 360f));
		GameObject instanceLune = Instantiate (lune, bec.position, randomRotation) as GameObject;
		source.Play ();
		attacking = false;
		float ratio = Mathf.Abs(target.position.x-bec.position.x)/10;
		float throwSpeed = Mathf.Lerp (0, maxThrow, ratio)*Mathf.Sign(target.position.x - bec.position.x);
		instanceLune.GetComponentInChildren<Rigidbody2D>().velocity = new Vector2(throwSpeed, 0);
		float randomTorque = Random.Range(-30, 30);
		instanceLune.GetComponentInChildren<Rigidbody2D> ().AddTorque (randomTorque);
		ChangeAppearance ();
		Invoke ("ChangeAppearance", 0.5f);
	}

	void ChangeAppearance(){
		if (spriteRenderer.sprite == sprite1)
			spriteRenderer.sprite = sprite2;
		else
			spriteRenderer.sprite = sprite1;
	}
}
