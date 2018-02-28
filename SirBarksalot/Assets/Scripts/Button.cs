using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {

	public float duration;
	public GameObject obj;
	public bool isPermanent=false;
	public bool pops=false;
	public GameObject pop;
	public Vector3 popLocation;
	public bool deactivates;
	public GameObject buttonToDeactivate;

	private string popStr;
	[HideInInspector]public bool activated;
	private Button toDeactivate;
	private AudioSource source;


	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
		activated = false;
		popStr = pop.name;
		if(deactivates)
			toDeactivate = buttonToDeactivate.GetComponent<Button> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player" && !activated) {
			if (isPermanent)
				Activate ();
			else
				StartCoroutine (ActivateTemp (duration));
			
		}
	}

	IEnumerator ActivateTemp(float dur){
		Activate ();
		yield return new WaitForSeconds (dur);
		Deactivate ();
	}

	public void Activate(){
		source.Play ();
		activated = true;
		obj.SetActive (false);
		this.transform.localScale -= 0.5f*Vector3.right*this.transform.localScale.x;
		this.transform.position += Vector3.right*transform.localScale.x / 2;
		bool canFind = (GameObject.Find (popStr)!=null||GameObject.Find (popStr+"(Clone)")!=null);
		if (pops && !canFind)
			Instantiate (pop, popLocation,Quaternion.Euler(0,0,0));
		if (deactivates && toDeactivate.activated)
			toDeactivate.Deactivate ();
	}

	public void Deactivate(){
		obj.SetActive (true);
		activated = false;
		this.transform.position -= Vector3.right*transform.localScale.x / 2;
		this.transform.localScale += Vector3.right*this.transform.localScale.x;
	}
}
