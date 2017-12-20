using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class AccrocheLiane : MonoBehaviour {

	private bool versLaDroite;
    private bool isHanging;
	private Animator anim;

    public float prop = 1000;
    
	private Collider2D coll;
	private Rigidbody2D RB;
    
	static Transform posLiane;
    
	Vector3 offset;


    	
    void Start ()
    {
        RB = GetComponent<Rigidbody2D>();
        isHanging = false;
		anim = GetComponent<Animator> ();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Liane" && isHanging == false)
        {
            Transform[] transforms;
            transforms = other.GetComponentsInChildren<Transform>();
            foreach (Transform trsf in transforms)
            {
                if (trsf.name == "Anchor")
                {
					coll = other;
					posLiane = trsf;
                    isHanging = true;
					anim.SetBool ("isHanging", true);

                }
            }
        }
    }
	// Update is called once per frame
	void Update () {
	    if(isHanging)
        {
            RB.constraints = RigidbodyConstraints2D.FreezeAll;
            versLaDroite = (RB.transform.position.x <= posLiane.position.x);
            RB.transform.position = posLiane.position;

            if(CrossPlatformInputManager.GetButtonDown("Jump"))
            {
                RB.constraints = RigidbodyConstraints2D.FreezeRotation;
                if (versLaDroite)
                    RB.AddForce(new Vector2(2, 1) * prop);
                else
                    RB.AddForce(new Vector2(-2, 1) * prop);
                isHanging = false;
				anim.SetBool ("isHanging", false);
				StartCoroutine (tempDisable (coll));
            }
        }
	}

	IEnumerator tempDisable(Collider2D collider2D){
		collider2D.enabled = false ;
		yield return new WaitForSeconds (0.5f);
		collider2D.enabled = true ;
	}

	public bool isHeHanging(){
		return isHanging;
	}
}
