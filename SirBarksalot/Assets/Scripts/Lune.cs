using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Lune : MonoBehaviour {



	private bool isDieing=false;
	private AudioSource source;
	private CircleCollider2D thisCollider;
	private Animator anim;

	public float explosionRadius = 1.35f;
	public bool explodeOnImpact=true;
	public float fuse;

	void Awake(){
		source = GetComponent<AudioSource> ();
		thisCollider = GetComponent<CircleCollider2D> ();
		Invoke ("Explode", fuse);
		anim = GetComponentInChildren<Animator> ();
	}

	void OnCollisionEnter2D(Collision2D collision){
		if (collision.gameObject.tag == "Croquette" || (!explodeOnImpact && collision.gameObject.tag !="Player" && collision.gameObject.tag !="Boss"))
			return;
		Explode ();
	}

	void Explode(){
		if (isDieing)
			return;
		isDieing = true;
		Rigidbody2D RB = GetComponent<Rigidbody2D> ();
		RB.isKinematic = true;
		thisCollider.enabled = false;
		source.Play ();
		Collider2D[] colliders = Physics2D.OverlapCircleAll (transform.position, explosionRadius);
		GameObject go = GameObject.FindGameObjectWithTag("Player");
		GameOverManager gomanager = go.GetComponent<GameOverManager> ();
		foreach (Collider2D collider in colliders) {
			if (collider.tag == "Player") {
				gomanager.isPlayerAlive = false;
				gomanager.reasonForDeath = "That just blew my mind!";
			}
			if (collider.tag == "Boss")
				collider.GetComponent<Boss>().takeDamage();
		}
		anim.SetBool ("isExploding", true);
//		SpriteRenderer[] rends = GetComponentsInChildren<SpriteRenderer> ();
//		foreach (SpriteRenderer rend in rends){
//			rend.enabled = !rend.enabled;
//		}
		StartCoroutine (WaitAndDestroy (0.4f));
	}

	IEnumerator WaitAndDestroy(float waitTime){
		Destroy (gameObject.GetComponentInChildren<SpriteRenderer>());
		yield return new WaitForSeconds (waitTime);
		Destroy (gameObject.GetComponentInChildren<SpriteRenderer>());
		yield return new WaitForSeconds (1.5f);
		Destroy (gameObject);
	}
}