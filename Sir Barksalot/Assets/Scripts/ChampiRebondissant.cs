using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

//[RequireComponent(typeof (Platformer2DUserControl))]
public class ChampiRebondissant : MonoBehaviour {


    public float forceRebond = 2000;
	public float facteurSaut = 1.2f;
	public float timeWindowBigJump = 0.1f;
	public float timeToPreventSpam = 0.2f;
	public float facteurHorizontal=100;
	public float stopControlTime = 0.1f;
	public bool canBoostJump = true;

	public Sprite spriteBoost;
	public Sprite spriteNoBoost;

	private Rigidbody2D RB;
	private AudioSource source;
	private bool occupied = false;
	private bool unchangedJump = true; 
	private bool unchangedSound = true;
	private float sin,cos, angle;
	private int i;
	private Camera2DFollow camScript;
	private SpriteRenderer rend;
	private Transform jumpTrsf;

	void Start(){
		camScript = FindObjectOfType<Camera> ().GetComponent<Camera2DFollow>();
		source = GetComponent<AudioSource> ();
		Transform trsf = GetComponent<Transform> ();
		float orientation = trsf.localEulerAngles.z;
		angle = orientation*(Mathf.PI/180);
		cos = Mathf.Cos (angle);
		sin = Mathf.Sin (angle);
		rend = GetComponentInChildren<SpriteRenderer> ();
		if (canBoostJump)
			rend.sprite = spriteBoost;
		else
			rend.sprite = spriteNoBoost;
		GameObject jumpPoint = new GameObject();
		jumpPoint.name = "JumpPoint";
		jumpPoint.transform.parent = gameObject.transform;
		jumpPoint.transform.localPosition = new Vector3 (0, 1.5f, 0);
		jumpTrsf = jumpPoint.transform;
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.Space) && unchangedJump == true && canBoostJump == true)
			StartCoroutine(bigJump());
	}

    void OnTriggerEnter2D(Collider2D other)
	{
		if ((occupied && other.tag !="Croquette") || other.tag == "Champi" || other.tag == "TrapGround")
			return;
		if (other.tag == "Player") {
			StartCoroutine (stopControl (other, stopControlTime));
			if (!canBoostJump)
				StartCoroutine (camScript.FreezeCameraOnPlayerTillGrounded());
		}
		RB = other.GetComponent<Rigidbody2D> ();
		if (angle != 0) {
			RB.transform.position = jumpTrsf.position;
			RB.AddForce (new Vector2 (-sin, cos) * forceRebond);
			RB.velocity = new Vector2 (0, 0);
		} else {
			RB.velocity = new Vector2 (Mathf.Abs (cos) * RB.velocity.x, Mathf.Abs (sin) * RB.velocity.y);
			RB.AddForce (new Vector2 (-sin, cos) * forceRebond);
		}
		source.Play ();
		if (unchangedSound == false) {
			StartCoroutine (soundChange ());
		}
   }

	IEnumerator bigJump(){
		float temp = forceRebond;
		forceRebond = temp*facteurSaut;
		unchangedJump = false;
		unchangedSound = false;
		yield return new WaitForSeconds (timeWindowBigJump);
		forceRebond = temp;
		unchangedSound = true;
		yield return new WaitForSeconds (timeToPreventSpam);
		unchangedJump = true;
	}

	IEnumerator soundChange(){
		source.pitch = 1.5f;
		yield return new WaitForSeconds (0.7f);
		source.pitch = 1;
	}

	IEnumerator stopControl(Collider2D other, float stopTime){
		occupied = true;
		Platformer2DUserControl script = other.GetComponent<Platformer2DUserControl> ();
		script.enabled = false;
		script.i ++;
		yield return new WaitForSeconds (stopTime);
		script.i--;
		if(script.i==0)
			script.enabled = true;
		occupied = false;
	}
}