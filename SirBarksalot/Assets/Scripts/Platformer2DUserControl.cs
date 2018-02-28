//using System;
using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;

[RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
    private PlatformerCharacter2D m_Character;
    private bool m_Jump;
	private float timeSinceAction;
	private Animator anim;
	private bool busy;
	private GameManager gameManager;

	[HideInInspector] public int i;

	private void Awake()
    {
		m_Character = GetComponent<PlatformerCharacter2D> ();
		anim = GetComponent<Animator> ();
		busy = false;
		if (GameObject.Find ("GameManager") != null)
			gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		else
			print ("pas de gameManager");
    }


        private void Update()
	{
		if (!m_Jump) {
			// Read the jump input in Update so button presses aren't missed.
			m_Jump = CrossPlatformInputManager.GetButtonDown ("Jump");
		}
		if (CrossPlatformInputManager.GetAxis ("Cancel") != 0){
			if (SceneManager.GetActiveScene ().name == "Menu")
				Application.Quit ();
			else {
				SceneManager.LoadScene ("Menu");
				Time.timeScale = 1;
				if (gameManager != null)
					gameManager.deathToll++;
			}
		}
    }


        private void FixedUpdate()
        {
        // Read the inputs.
		bool crouch = Input.GetKey(KeyCode.LeftShift);
		float h = CrossPlatformInputManager.GetAxis ("Horizontal");
		// Pass all parameters to the character control script.
		m_Character.Move(h, crouch, m_Jump);   


		if (h != 0 || m_Jump == true) {
			timeSinceAction = Time.time;
			anim.SetBool ("Hello", false);
			anim.SetBool ("Gratte", false);
		}

		if(m_Jump == true)
			StartCoroutine(LaunchAnim(0.5f, "Jump"));

		if(Time.time - timeSinceAction >= 2.5f && !busy)
			LaunchIdleAnim ();

		m_Jump = false;
	}

	private void LaunchIdleAnim(){
		busy = true;
		float random = Random.Range(0, 2);
		if (random == 0)
			StartCoroutine(LaunchAnim (3.5f, "Hello"));
		else
			StartCoroutine(LaunchAnim (5.5f, "Gratte"));
	}

	private IEnumerator LaunchAnim(float duration, string animation){
		anim.SetBool (animation, true);
		yield return new WaitForSeconds (duration);
		anim.SetBool (animation, false);
		timeSinceAction = Time.time;
		busy = false;
	}


}
    