using UnityEngine;
using System.Collections;

public class MagicCat : Cat {

	//	//Childs objects
	public Transform rightFiringPoint;
	public Transform leftFiringPoint;
	public GameObject projectile;
	public GameObject magicPulsePrefab;

	public string shootMagicGamepadButton;
	public string magicPulseGamepadButton;
	public string returnToHubGamepadButton;

    public Vector3 tempPos;

	public KeyCode shootKey;
	public KeyCode magicPulseKey;
	public KeyCode returnToHubKey;

	public bool canLevitate;
    public int levitateDelay = 3;
	public float levitateCooldown = 3;
    public bool levitate = false;
    public float doubleJump = 0;

	public float yVelocity;

	public bool isPulsing;

	private float levitateCooldownTimeStamp = 0;
	private Coroutine levitateCoroutine;
	// Use this for initialization

	protected override void Start ()
	{
		base.Start ();
		canLevitate = true;
	}


	
	// Update is called once per frame
	void Update () {

		yVelocity = myRigidBody2D.velocity.y;

		if(!controlersDisabled){
			DefaultControl();
		} 

	}

	void DefaultControl(){

		if(Input.GetButtonDown(returnToHubGamepadButton) || Input.GetKeyDown(returnToHubKey)){
			ReturnToHub();
		}

		if((Input.GetButtonDown(freakoutGamepadButton) || Input.GetKeyDown(freakoutKey) ) && !isDying && !isAttacking && !isJumping && !isPulsing && ready){
			freakoutMode = true;
			animator.SetBool("freakout",true);
		}

		if(!isDying && !freakoutMode){

			if(!isAttacking && !isPulsing){

                if (Input.GetKey(moveRightKey) || (Input.GetAxis(moveHorizontalGamepadAxis) >= 0.5f))
                {
                    MoveRight();
                }
                else if (Input.GetKey(moveLeftKey) || (Input.GetAxis(moveHorizontalGamepadAxis) <= -0.5f))
                {
                    MoveLeft();
                }
                else
                {
                    Idle();
                }

				if ((Input.GetKeyDown (downKey) && levitate) || Input.GetAxis (moveVerticalGamepadAxis) <= -0.5f && levitate) {
					CancelLevitate ();
				}

//                if (Input.GetKey(jumpKey) && levitate || Input.GetAxis(moveVerticalGamepadAxis) >= 0.5f && levitate)
//                {
//                    MoveUp();
//                }
//
//                if (Input.GetKey(downKey) && levitate || Input.GetAxis(moveVerticalGamepadAxis) <= -0.5f && levitate)
//                {
//                    MoveDown();
//                }

				if(Input.GetKeyDown (jumpKey) || Input.GetButtonDown(jumpGamepadButton)){

					if (!levitate && canLevitate && (isJumping || isFalling)) {
						Levitate ();
					}

					if(!isFalling){

						if (!isJumping) {
							Jump ();
						}
					}


				}

//				if((Input.GetKey (jumpKey) || Input.GetButton(jumpGamepadButton)) && isJumping && !levitate && !finishedJump){
//					ContinueJump();
//				}
//
//				if((Input.GetKeyUp (jumpKey) || Input.GetButtonUp(jumpGamepadButton)) && isJumping){
//					jumpTimeCounter = 0;
//					finishedJump = true;
//				}

                if (Input.GetKeyDown (shootKey) || Input.GetButtonDown(shootMagicGamepadButton)){
					
					StartProjectile();
				}

				if((Input.GetKeyDown (magicPulseKey) || Input.GetButtonDown(magicPulseGamepadButton)) && !isJumping && !isFalling){ 
					StartMagicPulse();
				}
			}

			if(myRigidBody2D.velocity.y < -1){
				isFalling = true;
			}
		}

		CheckInvulnerableTimeStamp ();

		if (levitateCooldownTimeStamp < Time.time) {
			canLevitate = true;
		}

		if (invulnerable) {
			Flash ();
		}

		CheckIfDamageReceived ();

		CheckDeath ();

		if(isAttacking){
			if(animator.GetCurrentAnimatorStateInfo(0).IsName("MagicCatAttaking") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1){
				FinishProjectile();
			}
		}

		if(isPulsing){
			if(animator.GetCurrentAnimatorStateInfo(0).IsName("MagicCatPulse") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1){
				FinishMagicPulse();
			}
			if(animator.GetCurrentAnimatorStateInfo(0).IsName("MagicCatJump") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1){
				FinishMagicPulse();
			}
		}
	}

	protected override void OnCollisionEnter2D (Collision2D other)
	{
		if(other.gameObject.tag == "Ground" || other.gameObject.tag == "InvisiblePlatform" || other.gameObject.tag == "Enemy"){
			if(!levitate){
				CheckIfGrounded();
			}
		}
	}

//    private void FixedUpdate()
//    {
//        if (levitate)
//        {
//            animator.SetBool("levitate", true);
//			myRigidBody2D.gravityScale = 0;
//            myRigidBody2D.velocity = Vector2.zero;
//            StartCoroutine(LevitateOff());
//        }
//    }

	void Levitate(){
		
		levitate = true;
		animator.SetBool("levitate", true);
		myRigidBody2D.gravityScale = 0;
		myRigidBody2D.velocity = Vector2.zero;
		levitateCoroutine = StartCoroutine(LevitateOff());
	}
    
	void CancelLevitate(){

		StopCoroutine (levitateCoroutine);
		canLevitate = false;
		levitateCooldownTimeStamp = Time.time + levitateCooldown;
		levitate = false;
		animator.SetBool("levitate", false);
		myRigidBody2D.gravityScale = 1;
		CheckIfGrounded();
		BroomCooldownIndicator.instance.StartCooldown();
	}

    IEnumerator LevitateOff()
    {
		
        yield return new WaitForSeconds(levitateDelay);
		print ("Leviate off called");
		canLevitate = false;
		levitateCooldownTimeStamp = Time.time + levitateCooldown;
		levitate = false;
		animator.SetBool("levitate", false);
		myRigidBody2D.gravityScale = 1;
		CheckIfGrounded();
		BroomCooldownIndicator.instance.StartCooldown();
       
    }


//	void LateUpdate(){
//
//		transform.position = new Vector3(Mathf.Round(transform.position.x),Mathf.Round(transform.position.y),0);	
//	}


	private void StartProjectile(){

		
		isAttacking = true;
		animator.SetBool("projectile", true);

	}

	public void FireProjectile(){

		animator.SetBool("idle", false);
		if(isLookingRight){
			Instantiate(projectile,rightFiringPoint.position,Quaternion.identity);
		} else {
			Instantiate(projectile,leftFiringPoint.position,Quaternion.identity);
		}
	}

	public void FinishProjectile(){
		
		isAttacking = false;
		animator.SetBool("idle", true);
		animator.SetBool("projectile", false);
	}

	void StartMagicPulse(){

		isPulsing = true;
		animator.SetBool("magicPulse",true);
		animator.SetBool("idle",false);
		animator.SetBool("walking",false);
	}

	public void CreateMagicPulse(){
		Instantiate(magicPulsePrefab,transform.position,Quaternion.identity);
	}

	public void FinishMagicPulse(){

		animator.SetBool("magicPulse",false);
		animator.SetBool("walking",false);
		animator.SetBool("idle",true);

		isPulsing = false;
	}


}
