using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D))]
[RequireComponent (typeof(Animator))]

public class PlayerPlatforming : MonoBehaviour {

	public bool AllowInput;
	public bool isFacingRight;

	float jumpHeight = 3;
	float timeToJumpApex = .4f;
	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;
	
	float gravity;

	[SerializeField]
	float runSpeed = 6;
	float jumpVelocity = 7;
	float velocityXSmoothing; // not sure what this does

	public Vector3 velocity;
	public float friction;

	int FBShooterCd;
	int numJumps;
	bool canJump;
	
	[HideInInspector]
	public PlayerRaycasting playerController;

	Animator myAnimator;
    SpriteRenderer myRenderer;
	FireBallManager FBShooter;

	AudioScript AudioManager;

	//[SerializeField]
	public bool[] Unlocks = { false, false, false };

    public bool damageInvuln;
    int flickercounter = 0;
	

	// Use this for initialization
	void Start () {
		playerController = GetComponent<PlayerRaycasting> ();
		myAnimator = GetComponent<Animator> ();
		gravity = -(2 * jumpHeight) / Mathf.Pow (((timeToJumpApex > 0)?timeToJumpApex:1),2);
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        myRenderer = GetComponent<SpriteRenderer>();

		AudioManager = GetComponentInChildren<AudioScript> ();

        damageInvuln = false;
		canJump = true;
		FBShooterCd = 0;
		FBShooter = GameObject.FindGameObjectWithTag ("FireBallManager").GetComponent<FireBallManager> ();
		AllowInput = true;
		friction = 1;
	}
	
	// Update is called once per frame
	void Update () {

		// Stop moving vertically if the player raycast controller detects an object above or below
		if (playerController.collisionData.above || playerController.collisionData.below) {
			velocity.y = 0;
		}

		// So you always have a value from .1 to 1
		if (friction > 1) 
			friction = 1;
		if (friction < 0) {
			friction = .1f;
		}


		// Get the input axis
		float horez = Input.GetAxisRaw ("Horizontal");
		HandleAnimations (horez);


		Vector2 pinput = new Vector2(horez, Input.GetAxisRaw("Vertical"));

		if (AllowInput) {

			// Jumping
			handleJumping ();

			// Shooting
			handleShooting ();


			float targetVelocityX = pinput.x * runSpeed;
			velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (playerController.collisionData.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
			velocity.x *= friction;


			// Play the audio queue
			if(playerController.collisionData.below && Mathf.Abs(velocity.x) >= .3)
			{	
				//AudioManager.PlayerWalking.PlayOneShot(AudioManager.PlayerWalking.clip);
                if (!AudioManager.PlayerWalking1.isPlaying)
				{
					AudioManager.PlayerWalking1.mute = false;
					AudioManager.PlayerWalking1.Play();
                }
			}
			else
			{
				AudioManager.PlayerWalking1.mute = true;
			}

		} else {
			velocity.x = 0;
            myAnimator.SetLayerWeight(1, 0);
		}

		velocity.y += gravity * Time.deltaTime;
		playerController.Move(velocity * Time.deltaTime);
	}

	void HandleAnimations(float horizontal)
	{
		if (AllowInput) {
			// Animations
			Flip (horizontal);

			// LR Movement
			myAnimator.SetFloat ("MSpeed", Mathf.Abs (horizontal));
		

			if (velocity.y < -.2f) {
				myAnimator.SetBool ("Landing", true);
			} else {
				myAnimator.SetBool ("Landing", false);
			}

			// Jumping
			if (!playerController.collisionData.below) {
				myAnimator.SetLayerWeight (1, 1);
			} else {
				if(myAnimator.GetLayerWeight (1) > 0)
				{
					myAnimator.SetLayerWeight (1, 0);
					AudioManager.PlayerLanding.Play ();
				}
			}

            // If you've taken damage then flicker the opacity
            if(damageInvuln)
            {
                flickercounter++;
                Color c = Color.white;
                if (flickercounter > 3)
                {
                    c.a = .4f;
                }
                else if(flickercounter < 3)
                    c.a = .7f;

                if (flickercounter > 6)
                    flickercounter = 0;

                myRenderer.color = c;
            }

		} else {
			myAnimator.SetFloat ("MSpeed", 0);
		}


	}

	void handleShooting()
	{
		
		// Increment the counter so you can't fire multiple so many times
		FBShooterCd ++;
		
		// When the key is down, check to see if you can fire, if you can then fire and pass in information based on your right and your grounded
		if (Input.GetKeyDown (KeyCode.K)) {
			// Make sure there's a fb shooter
			if(FBShooter == null)
				FBShooter = GameObject.FindGameObjectWithTag("FireBallManager").GetComponent<FireBallManager>();
			
			// Generate fireballs if possible
			if(FBShooterCd > 10)
			{
				FBShooterCd = 0;
				if(isFacingRight)
					FBShooter.GenerateFireBall (playerController.collisionData.below, 1);
				else
					FBShooter.GenerateFireBall (playerController.collisionData.below, -1);
			}
			
		}
	}
	

	void handleJumping()
	{

			// Handle how many jumps you should get
			if (playerController.collisionData.below) {
				if (Unlocks [0])
					numJumps = 2;
				else
					numJumps = 1;
			}

			// Allow resetting of the canjump variable
			if (Input.GetKeyUp (KeyCode.Space))
				canJump = true;


			if (Input.GetKeyDown (KeyCode.Space) && playerController.collisionData.below && canJump) {
				myAnimator.SetTrigger ("Jump");
				canJump = false;
				numJumps --;
				velocity.y = jumpVelocity;
				AudioManager.PlayerJumping.Play ();
			}

			if (Input.GetKeyDown (KeyCode.Space) && numJumps > 0 && canJump && Unlocks [1]) {
				myAnimator.SetTrigger ("Jump");
				canJump = false;
				numJumps --;

				// Make sure you can only mid-air jump once. Falling off of a ledge shouldn't give you 2 jumps
				if(numJumps > 0)
				numJumps --;

				velocity.y = jumpVelocity * .85f;
				AudioManager.PlayerJumping.Play ();	
			}

	}
	
	void Flip(float horez)
	{
		// Handle the transformation between scales
		if (horez < 0 || !isFacingRight) {
			isFacingRight = false;
			
			
			Vector3 scale = transform.localScale;
			
			if(scale.x > 0)
			{
				scale.x *= -1;
				
				transform.localScale = scale;
			}
		} 
		
		if	(horez > 0 || isFacingRight)
		{
			isFacingRight  = true;
			
			Vector3 scale = transform.localScale;
			
			if(scale.x < 0)
			{
				scale.x *= -1;
				
				transform.localScale = scale;
			}
		}
		
		
	}

    // You don't call this from anywhere other then player health script. Otherwise the actual damage values wont change!
    public void TakeDamage()
    {
        damageInvuln = true;
     
        StartCoroutine(resetInvuln());
    }

    IEnumerator resetInvuln()
    {
        yield return new WaitForSeconds(3);
        damageInvuln = false;
        myRenderer.color = Color.white;
    }

}
