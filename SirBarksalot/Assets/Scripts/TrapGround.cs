using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class TrapGround : MonoBehaviour {

	private Rigidbody2D RB;
	private bool broken = false;
	private List<Collider2D> allowedColliders = new List<Collider2D>();

	public float holdTime = 0.5f;
	public GameObject allowed = null;
	public GameObject allowed2 = null;

	void Start () {
		RB = GetComponent<Rigidbody2D> ();
		if (allowed != null) {
			Collider2D[] colliders = allowed.GetComponents<Collider2D> ();
			foreach (Collider2D coll in colliders)
				allowedColliders.Add (coll);
		}
		if (allowed2 != null) {
			Collider2D[] colliders = allowed2.GetComponents<Collider2D> ();
			foreach (Collider2D coll in colliders)
				allowedColliders.Add (coll);
		}
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.collider.GetType ().Name != "CircleCollider2D" && collision.collider.GetType ().Name != "BoxCollider2D")
			return;
		if (!broken && collision.gameObject.tag != "TrapGround" && collision.gameObject.tag != "Ignore" && !allowedColliders.Contains<Collider2D> (collision.collider)) {
			StartCoroutine(BreakApart(RB,holdTime));
			broken = true;
		} else if(broken && collision.gameObject.tag != "Player" && collision.gameObject.tag != "TrapGround" && collision.gameObject.tag != "Ignore" && !allowedColliders.Contains<Collider2D> (collision.collider))
			Destroy (gameObject);
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.GetType ().Name != "BoxCollider2D")
			return;
		if (!broken && collider.gameObject.tag != "TrapGround" && collider.gameObject.tag != "Ignore" && !allowedColliders.Contains<Collider2D> (collider)) {
			StartCoroutine(BreakApart(RB,holdTime));
			broken = true;
		} else if(broken && collider.gameObject.tag != "Player" && collider.gameObject.tag != "TrapGround" && collider.gameObject.tag != "Ignore" && !allowedColliders.Contains<Collider2D> (collider))
			Destroy (gameObject);
	}

	IEnumerator BreakApart(Rigidbody2D rb, float waitTime){
		yield return new WaitForSeconds (waitTime);
		rb.isKinematic = false;
		yield return new WaitForSeconds (1f);
		Destroy(rb.gameObject);
	}
}
