using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour {

	public GameObject croquette;
	public float leftThrow=16f;
	public float upThrow=5f;
	public bool test;
	public Sprite sprite1;
	public Sprite sprite2;

	private AudioSource source;
	private SpriteRenderer spriteRenderer;
	private int count;
	private Vector2[] throwPossibilities= new Vector2[]{};
	private bool throwing;
	private float lastThrow;
	private int maxHP = 3;
	private int bossPhase;
	private int lastBossPhase;
	private GameObject player;
	private Vector3 hand;
	[HideInInspector]public int currentHP;

	//à gauche:
	private Vector2[] Array1=new Vector2[] {new Vector2(16,13),new Vector2(8,13),new Vector2(5,13),new Vector2(20,8),new Vector2(12,8),new Vector2(6,8),new Vector2(25,-3),new Vector2(15,-3),new Vector2(6,-3),new Vector2(0,-3)};
	//à droite:
	//touche le boss
	private Vector2[] Array2=new Vector2[] {new Vector2(-14,-12), new Vector2(-17,12)};
	//touche pas le boss
	private Vector2[] Array3=new Vector2[] {new Vector2(-12,10),new Vector2(-5,10),new Vector2(-25,10),new Vector2(-25,-12),new Vector2(-15,8)};
	//pour que le joueur se casse
	private Vector2[] Array4=new Vector2[] {new Vector2(-8,15),new Vector2(-8,-2), new Vector2(-8,10)};

	public float timeWaitingToThrow;

	// Use this for initialization
	void Awake () {
		player = GameObject.FindGameObjectWithTag ("Player");
		lastThrow = Time.time;	
		timeWaitingToThrow = 2f;
		throwing = false;
		currentHP = maxHP;
		bossPhase = 0;
		hand = GameObject.Find("Hand").GetComponent<Transform>().position;
		spriteRenderer = GetComponent<SpriteRenderer> ();
		source = GetComponent<AudioSource> ();
	}
				
	// Update is called once per frame
	void Update () {
//		print ((float)Screen.width / (float)Screen.height);
		PhaseCheck();

		if (lastBossPhase != bossPhase)
			lastThrow = Mathf.Max(Time.time-1f,lastThrow);

		switch (bossPhase) {
		case 0:
			throwing = false;
			lastThrow = Time.time - 1f;
			break;
		case 1:
			throwPossibilities = Array1;
			timeWaitingToThrow = 2.5f;
			throwing = true;
			gameObject.transform.localScale = new Vector3(1,1,1);
			break;
		case 2:
			throwPossibilities = Array2;
			timeWaitingToThrow = 2.5f;
			throwing = true;
			gameObject.transform.localScale = new Vector3 (-1, 1, 1);
			break;
		case 3:
			throwPossibilities = Array3;
			timeWaitingToThrow = 2.5f;
//			pour des raisons de "WaitToThrow"
//			throwing = true;
			gameObject.transform.localScale = new Vector3(-1,1,1);
			break;
		case 4:
			throwPossibilities = Array4;
			timeWaitingToThrow = 0.5f;
//			pour des raisons de "WaitToThrow"
//			throwing = true;
			gameObject.transform.localScale = new Vector3(-1,1,1);
			break;
		}

		if (lastBossPhase != bossPhase) {
			hand = GameObject.Find ("Hand").GetComponent<Transform> ().position;
		}

		//interlude avant la phase où le boss spam le joueur pour qu'il se casse
		if (lastBossPhase == 2 && bossPhase == 4)
//			print ("lol");
			StartCoroutine(WaitToThrow (4f));

		lastBossPhase=bossPhase;

		if (throwing && (Time.time - lastThrow >= timeWaitingToThrow))
			ThrowingShitEveryday ();
	}

	public void takeDamage(){
		currentHP--;
		if (currentHP == 0) {
			Destroy(gameObject);
		}
	}

	private void Throw(Vector2 throwCar){
		Quaternion randomRotation = Quaternion.Euler (0f, 0f, Random.Range (0f, 360f));
		GameObject instanceCroquette=Instantiate (croquette, hand, randomRotation) as GameObject;
		throwCar.x *= -1;
		instanceCroquette.GetComponent<Rigidbody2D>().velocity = throwCar;
//		print (throwCar);
		float randomTorque = Random.Range(-60, 60);
		instanceCroquette.GetComponentInChildren<Rigidbody2D> ().AddTorque (randomTorque);
		ChangeAppearance ();
		Invoke ("ChangeAppearance", 0.5f);
		source.Play ();
	}

	IEnumerator WaitToThrow(float wait){
		throwing = false;
		yield return new WaitForSeconds (wait);
		throwing = true;
	}

	private void ThrowingShitEveryday (){
		lastThrow = Time.time;
		int rand = (int)Mathf.Floor (Random.Range (0, throwPossibilities.Length));
		if (bossPhase <= 3)
			count++;
		else if (bossPhase == 4){
			Throw (throwPossibilities [count%3]);
			count++;
			return;}
		Vector2 throwCarac= new Vector2(leftThrow,upThrow);
		if(test)
			Throw (throwCarac);
		else
			Throw (throwPossibilities[rand]);
	}

	void PhaseCheck(){
		if (player.transform.position.x <= 24f)
			bossPhase = 0;
		else if (player.transform.position.x > 24f && (player.transform.position.x <= 42.9f || player.transform.position.y <= -27.1f)) {
			count = 0;
			bossPhase = 1;
		}
		else if(player.transform.position.x>42.9f && player.transform.position.y>-27.1f){
			if (count > 2) {
				if (count == 3)
					bossPhase = 2;
				else
					bossPhase = 4;
			} else
				bossPhase = 3;
		}
	}

	void ChangeAppearance(){
		if (spriteRenderer.sprite == sprite1)
			spriteRenderer.sprite = sprite2;
		else
			spriteRenderer.sprite = sprite1;
	}
}
