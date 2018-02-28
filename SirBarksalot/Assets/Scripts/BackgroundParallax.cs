using UnityEngine;
using System.Collections;

public class BackgroundParallax : MonoBehaviour
{
	
	public float parallaxScale;					// The proportion of the camera's movement to move the backgrounds by.
	public float smoothing;						// How smooth the parallax effect should be.

	private Transform background;				// Array of all the backgrounds to be parallaxed.
	private Transform cam;						// Shorter reference to the main camera's transform.
	private Vector3 previousCamPos;				// The postion of the camera in the previous frame.


	void Awake ()
	{
		// Setting up the reference shortcut.
		cam = Camera.main.transform;
		if (GetComponentInChildren<SpriteRenderer> ()!= null)
			background = GetComponentInChildren<SpriteRenderer> ().GetComponent<Transform> ();
		else
			print ("pas de background");
	}


	void Start ()
	{
		// The 'previous frame' had the current frame's camera position.
		previousCamPos = cam.position;
	}


	void Update ()
	{
		if (background == null)
			return;
		// The parallax is the opposite of the camera movement since the previous frame multiplied by the scale.
		float parallaxX = Mathf.Clamp(( cam.position.x-previousCamPos.x) * parallaxScale,-smoothing,smoothing);
		float parallaxY = ( cam.position.y-previousCamPos.y) * parallaxScale;

		// ... set a target x position which is their current position plus the parallax multiplied by the reduction.
		float backgroundTargetPosX = background.position.x - parallaxX;
		float backgroundTargetPosY = background.position.y - parallaxY;

		// Create a target position which is the background's current position but with it's target x position.
		Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgroundTargetPosY, background.position.z);

		// Lerp the background's position between itself and it's target position.
		background.position = backgroundTargetPos;

		// Set the previousCamPos to the camera's position at the end of this frame.
		previousCamPos = cam.position;

	}
}
