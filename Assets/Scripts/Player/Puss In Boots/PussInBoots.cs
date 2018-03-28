using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PussInBoots : Cat {

	public Collider2D rightAttackingPoint;
	public Collider2D leftAttackingPoint;
	public Collider2D rightParryPoint;
	public Collider2D leftParryPoint;
 

	public float parryingTime;

    public int climbSpeed = 2;

	public string parryGamepadButton;
	public string attackGamepadButton;
	public string returnToHubGamepadButton;

	public KeyCode returnToHubKey;
	public KeyCode attackKey;
	public KeyCode parryKey;

	public bool startedParryStance;
	public bool parryStanceActivated;
	public bool isParrying;
	public bool isClimbing;//Nick

	public Enemy enemyBeingParried;

	public AudioClip swordParry;
	public AudioClip swordHit;

	void Awake(){
		rightAttackingPoint.enabled = false;
		leftAttackingPoint.enabled = false;
		rightParryPoint.enabled = false;
		leftParryPoint.enabled = false;
	}

	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();
	}

	//Nick
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Climbable")
		{
			isClimbing = true;
            isFalling = false;
            isJumping = true;
			gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
			myRigidBody2D.velocity = new Vector3(0, 0, 0);
            animator.SetBool("climbing", true);
            animator.SetBool("walking", false);
            animator.SetBool("idle", false);  
			animator.SetBool("jumping", false);
        }
	}
	//Nick
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "Climbable")
		{
			isClimbing = false;
            isJumping = false;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
            animator.SetBool("climbing", false);
        }
	}

	// Update is called once per frame
	void Update () {

		if(Input.GetButtonDown(returnToHubGamepadButton) || Input.GetKeyDown(returnToHubKey)){
			ReturnToHub();
		}

		if((Input.GetButtonDown(freakoutGamepadButton) || Input.GetKeyDown(freakoutKey) ) && !isDying && !isJumping && !isAttacking && !isParrying && !startedParryStance && !parryStanceActivated && ready){
			freakoutMode = true;
			animator.SetBool("freakout",true);
		}

		if(!isDying && !freakoutMode){

			if(!isAttacking && !parryStanceActivated  && !startedParryStance  && !isParrying){

				if (Input.GetKey (moveRightKey) || (Input.GetAxis (moveHorizontalGamepadAxis) >= 0.5f)) {
					MoveRight ();
				} else if (Input.GetKey (moveLeftKey) || (Input.GetAxis (moveHorizontalGamepadAxis) <= -0.5f)) {
					MoveLeft ();
				} else {
					Idle();
				}

				//Nick
				if (isClimbing)
				{
                    if(Input.GetButton("Vertical") || Input.GetAxisRaw("Vertical") > 0.5f || (Input.GetAxis("Vertical") >= 0.5f ||
                        Input.GetButton("Vertical") || Input.GetAxisRaw("Vertical") < -0.5f || (Input.GetAxis("Vertical") <= -0.5f ||
                        Input.GetKey(moveRightKey) || (Input.GetAxis(moveHorizontalGamepadAxis) >= 0.5f || 
                        Input.GetKey(moveLeftKey) || (Input.GetAxis(moveHorizontalGamepadAxis) <= -0.5f)))))
                    {
                        animator.speed = 1;
                    }
                    else
                    {
                        animator.speed = 0;
                    }
                    //This is conjested and ugly like crazy but it's all to get the climbing animation to stop while he's not moving
                    

					if (Input.GetButton("Vertical") && Input.GetAxisRaw("Vertical") > 0 || (Input.GetAxis("Vertical") >= 0.5f))
					{
						myRigidBody2D.transform.position += Vector3.up * climbSpeed * Time.deltaTime;
                    }

					if (Input.GetButton("Vertical") && Input.GetAxisRaw("Vertical") < 0 || (Input.GetAxis("Vertical") <= -0.5f))
					{
						myRigidBody2D.transform.position += -Vector3.up * climbSpeed * Time.deltaTime;
                    }
				}



				if(Input.GetKeyDown (jumpKey) || Input.GetButtonDown(jumpGamepadButton)){

					if(!isFalling){

						if(!isJumping){
							Jump();
						} 
					}
				}

				if((Input.GetKeyDown (attackKey) || Input.GetButtonDown(attackGamepadButton)) && !isClimbing){
					StartAttack();
				}

				if((Input.GetKeyDown(parryKey ) || Input.GetButtonDown(parryGamepadButton)) && !isJumping && !isFalling && !isClimbing)
                {
					StartParryStance();
				}
			}


			if(myRigidBody2D.velocity.y < -1){
				isFalling = true;
			}
		}

		CheckInvulnerableTimeStamp ();

		if (invulnerable && !isParrying && !parryStanceActivated) {
			Flash ();
		}

		CheckIfDamageReceived ();
			
		CheckDeath ();

	}

	private void StartAttack(){

		isAttacking = true;
		animator.SetBool("attacking", true);
	}

	public void ActivateAttackCollider(){

		if(isLookingRight){
			rightAttackingPoint.enabled = true;
		} else {
			leftAttackingPoint.enabled = true;
		}
	}

	public void FinishAttack(){
		
		isAttacking = false;
		animator.SetBool("attacking", false);
		rightAttackingPoint.enabled = false;
		leftAttackingPoint.enabled = false;
	}

	public void CancelParryStance(){ //Cancels the instance if the player got hit during the preparation to parry or if the damage came from a different direction

		startedParryStance = false;
		parryStanceActivated = false;

		rightParryPoint.enabled = false;
		leftParryPoint.enabled = false;

		invulnerable = false;

		animator.SetBool("parryStance",false);
		animator.SetBool("idle",true);
	}

	void StartParryStance(){ //Starts the preparation to the parry stance

		StopCoroutine(StartParryTimer()); //Kills the last parry stance timer
		startedParryStance = true;

		//Start animation to prepare for stance
		animator.SetBool("idle",false);
		animator.SetBool("walking",false);
		animator.SetBool("parryStance",true);

	}

	public void ParryStance(){ //The player was not interrupted during preparation to the stance and the parry stance is active

		invulnerable = true;
		 invulnerableTimeStamp = Time.time + parryingTime; 
		startedParryStance = false;
		parryStanceActivated = true;
		 
		ActivateParryCollider(); // Activate the collider
		StartCoroutine(StartParryTimer()); //Starts the timer
	}

	public void ActivateParryCollider(){

		if(isLookingRight){
			rightParryPoint.enabled = true;
		} else {
			leftParryPoint.enabled = true;
		}
	}

	public void ParrySuccess(Enemy targetEnemy){ 

		
		rightParryPoint.enabled = false;
		leftParryPoint.enabled = false;
		parryStanceActivated = false;
		isParrying = true;
		enemyBeingParried = targetEnemy;

//		Time.timeScale = 0.5f;

		animator.SetBool("parryStance",false);
		animator.SetBool("parrySuccess",true);

		myAudioSource.PlayOneShot (swordParry);
	}

	public void RefreshTimeScale(){
		Time.timeScale = 1f;	
	}

	public void FinishedParrySuccess(){ //Function called by the animator to finish the "parry success" animation

		 
		invulnerable = false;
		isParrying = false;
		animator.SetBool("parrySuccess",false);
		animator.SetBool("idle",true);
		enemyBeingParried.ReceiveParry();
		CancelParryStance();
	}

	protected override void CheckIfDamageReceived()
	{
		if (receivedDamage && life > 0) {

			if(startedParryStance || parryStanceActivated){
				CancelParryStance();
			}
			animator.SetBool("damage",true);
			ToggleInvinsibility ();
		}
	}

	protected override void CheckInvulnerableTimeStamp()
	{
		if (invulnerableTimeStamp < Time.time && !isParrying ) {
			if(!freakoutMode){
				invulnerable = false;
				mySpriteRenderer.enabled = true;
			}
		}
	}



	IEnumerator StartParryTimer(){

		yield return new WaitForSeconds(parryingTime);
		if(parryStanceActivated){
			CancelParryStance();
		}

	}
}
