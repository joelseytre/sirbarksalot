using System;
using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class Camera2DFollow : MonoBehaviour
    {
	public float xMargin = 1.5f; // Distance in the x axis the player can move before the camera follows.
	public float yMargin = 0.5f; // Distance in the y axis the player can move before the camera follows.
	public Transform target;
    public float dampingX = 0.65f;
	public float dampingY = 0.25f;
	public float lookAheadFactor = 9f;
    public float lookAheadReturnSpeed = 5f;
    public float lookAheadMoveThreshold = 0.1f;
	public float offsetY = 1;
	public float offsetYSum = 3;
	public float yAxis;

	private bool occupied = false;
	private float offsetY_stored;
    private Vector3 m_OffsetZ;
    private Vector3 m_LastTargetPosition;
    private Vector3 m_CurrentVelocity;
    private Vector3 m_LookAheadPos;
	private Vector3 targetX;
	private Vector3 targetY;
	private BackgroundParallax backgroundParallaxScript;
	private PlatformerCharacter2D playerScript;
	private AccrocheLiane playerAccrocheLianeScript;

        // Use this for initialization
	private void Start()
	{		
		playerAccrocheLianeScript = GameObject.Find ("Chien").GetComponent<AccrocheLiane> ();
		playerScript = GameObject.Find ("Chien").GetComponent<PlatformerCharacter2D> ();
		yAxis = 0;
		m_LastTargetPosition = target.position+ offsetY*Vector3.up;
		m_OffsetZ = (transform.position - target.position).z*Vector3.forward;
        transform.parent = null;
		offsetY_stored = offsetY;
		backgroundParallaxScript = GetComponent<BackgroundParallax> ();
        }

		private bool CheckXMargin()
		{
			// Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
			return Mathf.Abs(transform.position.x - target.position.x) > xMargin;
		}


		private bool CheckYMargin()
		{
			// Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
			return Mathf.Abs (transform.position.y - target.position.y-offsetY) > yMargin;
		}

        // Update is called once per frame
		void Update()
		{
			float v = CrossPlatformInputManager.GetAxis ("Vertical");
			float h = CrossPlatformInputManager.GetAxis ("Horizontal");
			if (v != 0 && Math.Abs (h) < .1f) {
				offsetY = offsetY_stored+ v * offsetYSum + yAxis;
			} else {
				offsetY = offsetY_stored + yAxis;
			}


			if (CheckXMargin ()) {
				// only update lookahead pos if accelerating or changed direction
				float xMoveDelta = (target.position - m_LastTargetPosition).x;
				bool updateLookAheadTarget = Mathf.Abs (xMoveDelta) > lookAheadMoveThreshold;

				if (updateLookAheadTarget) 
					m_LookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign (xMoveDelta);
				 else 
					m_LookAheadPos = Vector3.MoveTowards (m_LookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
				
				
				targetX = (target.position.x - transform.position.x) * Vector3.right + m_LookAheadPos;
				} else
				targetX = Vector3.zero;

				if (CheckYMargin ()) 
				targetY = (target.position.y + offsetY - transform.position.y) * Vector3.up;
				else
					targetY = Vector3.zero;

				Vector3 aheadTargetPosX = transform.position.x*Vector3.right + targetX;
				Vector3 aheadTargetPosY = transform.position.y*Vector3.up + targetY;
				Vector3 newPosX = Vector3.SmoothDamp(transform.position.x*Vector3.right, aheadTargetPosX, ref m_CurrentVelocity, dampingX);
				Vector3 newPosY = Vector3.SmoothDamp(transform.position.y*Vector3.up, aheadTargetPosY, ref m_CurrentVelocity, dampingY);

			transform.position = newPosX+newPosY+m_OffsetZ;

			m_LastTargetPosition = target.position + offsetY*Vector3.up;
        }

		public IEnumerator FreezeCameraOnPlayerTillGrounded(){
		if (!occupied) {
//			float temp1 = xMargin;
			//				float temp2 = yMargin;
			float temp3 = dampingX;
			float temp4 = dampingY;
			float temp5 = lookAheadFactor;

			float temp6 = backgroundParallaxScript.parallaxScale;
			float temp7 = backgroundParallaxScript.smoothing;

			//				xMargin = 0;
			//				yMargin = 0;
			dampingX = 0.1f;
			dampingY = 0.1f;
			lookAheadFactor = 0;

			backgroundParallaxScript.parallaxScale = 0;
			backgroundParallaxScript.smoothing = 10;

			//				transform.position = target.position;

			occupied = true;

			//pour éviter de détecter immédiatement le sol
			yield return new WaitForSeconds (0.1f);

			while (occupied == true) {

//				

				if (playerScript.isGrounded () || playerAccrocheLianeScript.isHeHanging()) {
//				xMargin = temp1;
//				yMargin = temp2;
					dampingX = temp3;
					dampingY = temp4;
					lookAheadFactor = temp5;

					backgroundParallaxScript.parallaxScale = temp6;
					backgroundParallaxScript.smoothing = temp7;

					occupied = false;
				}
				yield return null; 
			}
		}
	}

	public void setYaxis(float a){
		this.yAxis = a;
	}
}
