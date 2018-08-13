using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateCat : Cat {

	public float yMovementSpeed;
	public int gunDamage = 2;
	public float gunDistance = 2;

	public AudioClip[] swordAttackSounds;
	public AudioClip[] gunShootingSounds;
	public AudioClip swordSharkSound;
	public AudioClip[] handCannonSound;
	public AudioClip bigCannonSound;

	public KeyCode attackKey;
	public KeyCode gunKey;
	public string gunGamepadButton;
	public string attackGamepadButton;

	public CutlassCollider rightAttackingPoint1;
	public CutlassCollider leftAttackingPoint1;
	public CutlassCollider rightAttackingPoint2;
	public CutlassCollider leftAttackingPoint2;
	public CutlassCollider rightAttackingPoint3;
	public CutlassCollider leftAttackingPoint3;

	public GameObject handCannonBallPrefab;
	public GameObject handCannonFiringPointRight;
	public GameObject handCannonFiringPointLeft;

	public GameObject giantCannonBallPrefab;
	public GameObject giantCannonFiringPointRight;
	public GameObject giantCannonFiringPointLeft;

	public Transform shotOrigin;

	public GameObject shadow;

	public bool isShooting;
	public bool beingLaunched;
	public bool inCannon;
	public bool isNearCannon;
	public bool knockedDown;
	public bool receivingDamage;
	public bool landing;

	public int amountOfHitsToBeKnockedDown = 3;
	public float knockedDownTime = 2f;

	protected float knockedDownTimeStamp = 0f;

	public GameObject bulletFeedbackPrefab;

	public int amountOfHitsTaken = 0;

	public bool canCombo = false;

	public int comboCounter = 0;
	public bool comboActivated = false;

	public int gunComboCounter = 0;
	public bool gunComboActivated = false;

	public KeyCode moveUpKey;

	private BeatEmUpCatReference myReference;

	public Transform shadowReference;
	// Use this for initialization
	protected override void Start ()
	{

		animator = GetComponent<Animator>();
		mySpriteRenderer = GetComponent<SpriteRenderer>();
		myAudioSource = GetComponent<AudioSource> ();
		currentCheckpoint = startingPoint.position;
		myRigidBody2D = GetComponentInParent<Rigidbody2D>();
		myRigidBody2D.velocity = new Vector2(myRigidBody2D.velocity.x,0);
		myReference = GetComponentInParent<BeatEmUpCatReference>();

		yMovementSpeed = speed / 3;
	}
	// Update is called once per frame
	void Update () {
		if(!controlersDisabled){
			if(!isDying  && !freakoutMode && !beingLaunched && !receivingDamage && !knockedDown && !inCannon && !landing){

				if((Input.GetButtonDown(freakoutGamepadButton) || Input.GetKeyDown(freakoutKey) ) && !isDying && !isJumping && !isAttacking && !isShooting && ready){
					freakoutMode = true;
					freakoutManager.PlayFreakOutSound ();
					animator.SetBool("freakout",true);
				}

				if(!isAttacking && !isShooting){

	
					if (Input.GetKey (moveRightKey) || (Input.GetAxis (moveHorizontalGamepadAxis) >= 0.5f) ) {
						MoveRight();

						if (Input.GetKey (moveUpKey) || (Input.GetAxis (moveVerticalGamepadAxis) >= 0.5f) ) {
							MoveUp();
						} else if (Input.GetKey (downKey) || (Input.GetAxis (moveVerticalGamepadAxis) <= -0.5f)) {
							MoveDown();
						}

					} else if (Input.GetKey (moveLeftKey) || (Input.GetAxis (moveHorizontalGamepadAxis) <= -0.5f)) {
						MoveLeft();

						if (Input.GetKey (moveUpKey) || (Input.GetAxis (moveVerticalGamepadAxis) >= 0.5f) ) {
							MoveUp();
						} else if (Input.GetKey (downKey) || (Input.GetAxis (moveVerticalGamepadAxis) <= -0.5f)) {
							MoveDown();
						}
					} else if (Input.GetKey (moveUpKey) || (Input.GetAxis (moveVerticalGamepadAxis) >= 0.5f) ) {
						MoveUp();
					} else if (Input.GetKey (downKey) || (Input.GetAxis (moveVerticalGamepadAxis) <= -0.5f)) {
						MoveDown();
					} else {
						Idle();
					}
				
					if((Input.GetKeyDown (jumpKey) || Input.GetButtonDown(jumpGamepadButton))){
						if(!isFalling){

							if(!isJumping){
								Jump();
							} 
						}
					}

					if((Input.GetKeyDown (attackKey) || Input.GetButtonDown(attackGamepadButton)) && !isJumping){
						if(!isNearCannon){
							StartAttack();
						}
					}


					if((Input.GetKeyDown(gunKey ) || Input.GetButtonDown(gunGamepadButton)) && !isJumping && !isFalling && !SkillCooldownIndicator.instance.onCooldown)
	                {
						StartShootingGun();
					}


				}

				if(isAttacking){
					
					if(canCombo){

						if((Input.GetKeyDown (attackKey) || Input.GetButtonDown(attackGamepadButton)) && !isJumping){
							comboActivated = true;
							gunComboActivated = false;
							canCombo = false;
							comboCounter++;
						}

						if((Input.GetKeyDown (gunKey) || Input.GetButtonDown(gunGamepadButton)) && !isJumping){

							comboCounter++;
							gunComboActivated = true;
							comboActivated = false;
							canCombo = false;

							isAttacking = false;

							if(gunComboCounter == 0){
								gunComboCounter++;
								isShooting = true;
							}else{
								isShooting = true;
							}
						}

					}

					if((animator.GetCurrentAnimatorStateInfo(0).IsName("Pirate Attack 1") || animator.GetCurrentAnimatorStateInfo(0).IsName("Pirate Attack 2") || animator.GetCurrentAnimatorStateInfo(0).IsName("Pirate Attack 3") ) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1){
						FinishAttack();
					}

				}

				if(isShooting){

					if(canCombo){

						if((Input.GetKeyDown (attackKey) || Input.GetButtonDown(attackGamepadButton)) && !isJumping){

							gunComboCounter++;
							comboActivated = true;
							gunComboActivated = false;
							canCombo = false;

							isShooting = false;

							if(comboCounter == 0){
								comboCounter++;
								isAttacking = true;
							}else{
								isAttacking = true;
							}

						}

						if((Input.GetKeyDown (gunKey) || Input.GetButtonDown(gunGamepadButton)) && !isJumping){
							gunComboActivated = true;
							comboActivated = false;
							canCombo = false;
							gunComboCounter++;
						}
					}

				}


				if(myRigidBody2D.velocity.y < -1){
					isFalling = true;
				}

			}

			if(receivingDamage){
				if(animator.GetCurrentAnimatorStateInfo(0).IsName("ReceiveDamage") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1){
					FinishDamageAnimation();
				}
			}

			if(knockedDown){
				if(knockedDownTimeStamp < Time.time && knockedDownTimeStamp != 0){
	            	StandUp();
	            }
	        }

		}else{
			Idle();
		}

		CheckIfDamageReceived ();
			
		CheckDeath ();


		if(!isJumping){
			shadow.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
		}



	}

	void FixedUpdate(){


	}

	protected override void MoveRight(){

		animator.SetBool("walking",true);
		animator.SetBool("idle",false);
		isWalking = true;
		isLookingRight = true;

		ChangeLookingDirection();

	
	}

	protected override void MoveLeft(){

		animator.SetBool("walking",true);
		animator.SetBool("idle",false);
		isWalking = true;
		isLookingRight = false; 


		ChangeLookingDirection();

	
	}
	protected override void MoveUp()
    {

		float newY = transform.position.y + (1 * yMovementSpeed * Time.deltaTime);
		float distanceRelativeToReference = newY - myReference.transform.position.y; 

		if(distanceRelativeToReference <= myReference.maxY){
			transform.position += Vector3.up * yMovementSpeed * Time.deltaTime;
			shadow.transform.position += Vector3.up * yMovementSpeed * Time.deltaTime;
		}

		animator.SetBool("walking",true);
		animator.SetBool("idle",false);
		isWalking = true;
      
    }
	protected override void MoveDown()
    {
		float newY = transform.position.y + (-1 * yMovementSpeed * Time.deltaTime);
		float distanceRelativeToReference = newY - myReference.transform.position.y; 

		if(distanceRelativeToReference >= myReference.minY){
			transform.position += Vector3.down * yMovementSpeed * Time.deltaTime;
			shadow.transform.position += Vector3.down * yMovementSpeed * Time.deltaTime;
		}

		animator.SetBool("walking",true);
		animator.SetBool("idle",false);
		isWalking = true;
  
    }

	protected override void Jump(){

		animator.SetBool("jumping",true);
		StartCoroutine(SetJustJumpedOff());
		justJumped = true;
		isJumping = true;
		isSliding = false;
			
	}

	protected override void CheckIfDamageReceived()
	{

		if (receivedDamage && life > 0) {

			Reset();
			receivedDamage = false;
			amountOfHitsTaken++;
			receivingDamage = true;


			if(amountOfHitsTaken < amountOfHitsToBeKnockedDown){
				if(animator.GetBool("damage")){
					animator.Play("ReceiveDamage",0,0f);
				}else{
					animator.SetBool("damage",true);
				}

			}else{
				if(!isDying){
					KnockDown();
				}
			}
		}
	}


	protected void FinishDamageAnimation(){
		
		animator.SetBool("damage",false);
		receivingDamage = false;
	}

	public void KnockDown(){
		animator.SetBool("damage",true);
		amountOfHitsTaken = 0;
		knockedDown = true;
		invulnerable = true;
		receivingDamage = false;
		animator.SetBool("knockedDown", true);
		if(sourceOfDamagePosition.x > transform.position.x){ //Enemy is on the right

			myRigidBody2D.velocity = new Vector2( -2f , myRigidBody2D.velocity.y + 4);
	
		}else{

			myRigidBody2D.velocity = new Vector2( 2f , myRigidBody2D.velocity.y + 4);	
	
		}
	}

	protected void FinishedKnockDownAnimation(){

		animator.SetBool("damage",false);
		animator.SetBool("knockedDown", false);
		knockedDownTimeStamp = Time.time + knockedDownTime;
	}

	public void StandUp(){
		knockedDownTimeStamp = 0;
		animator.SetBool("damage",false);
		animator.SetBool("knockedOnFloor", false);
		animator.SetBool("standUp", true);
	}

	protected void FinishStandUpAnimation(){
		animator.SetBool("standUp", false);
		invulnerable = false;
		knockedDown = false;
	}

	protected override void Idle ()
	{
		base.Idle ();
		FinishAttack();
		StopShooting();

	}

	protected override void OnCollisionEnter2D(Collision2D other){
		base.OnCollisionEnter2D (other);

	}

	protected override void CheckDeath ()
	{
		if(life <= 0 ){
			isDying = true;
			animator.SetBool("dying",true);
			animator.SetInteger("comboCounter", 1);
			animator.SetInteger("gunComboCounter", 1);
		}
	}

	public override void IsGrounded ()
	{

//		isJumping = false;
//		isFalling = false;
//		if(beingLaunched){
//
//			myRigidBody2D.gravityScale = 1;
//			myRigidBody2D.velocity = new Vector2(0,0);
//			beingLaunched = false;
//			invulnerable = false;
//			animator.SetBool("launched", false);
//
//		}else if(knockedDown){
//
//			animator.SetBool("knockedOnFloor", true);
//			animator.SetBool("knockedDown", false);
//		}else{
//			myRigidBody2D.velocity = new Vector2(myRigidBody2D.velocity.x,0);
//		}
//
//		animator.SetBool("jumping", false);
	}


	void StartAttack(){

		comboCounter++;
		isAttacking = true;
		animator.SetBool("attack", true);


	}

	public void ActivateAttackCollider(){

		if(comboCounter == 1){

			if(isLookingRight){
				rightAttackingPoint1.GetComponent<BoxCollider2D>().enabled = true;
			} else {
				leftAttackingPoint1.GetComponent<BoxCollider2D>().enabled = true;
			}

			int randomIndex = Random.Range(0,swordAttackSounds.Length);
			myAudioSource.PlayOneShot(swordAttackSounds[randomIndex]);
		
		}else if(comboCounter == 2){

			if(isLookingRight){
				rightAttackingPoint2.GetComponent<BoxCollider2D>().enabled = true;
			} else {
				leftAttackingPoint2.GetComponent<BoxCollider2D>().enabled = true;
			}

			int randomIndex = Random.Range(0,swordAttackSounds.Length);
			myAudioSource.PlayOneShot(swordAttackSounds[randomIndex]);
		
		}else if(comboCounter >= 3){

			if(isLookingRight){
				rightAttackingPoint3.GetComponent<BoxCollider2D>().enabled = true;
			} else {
				leftAttackingPoint3.GetComponent<BoxCollider2D>().enabled = true;
			}

			myAudioSource.PlayOneShot(swordSharkSound);
		}
			
	}

	void Reset(){
		animator.SetBool("idle", false);
		animator.SetBool("attack", false);
		animator.SetBool("walking", false);
		animator.SetBool("shooting", false);
		animator.SetBool("shooting", false);
		animator.SetInteger("comboCounter", 1);
		animator.SetInteger("gunComboCounter", 1);

		DisableColliders();

		isAttacking = false;
		isWalking = false;
		isShooting = false;
		comboActivated = false;
		gunComboActivated = false;
		comboCounter = 0;
		gunComboCounter = 0;
    }

	public void DisableColliders(){

		rightAttackingPoint1.GetComponent<BoxCollider2D>().enabled = false;
		leftAttackingPoint1.GetComponent<BoxCollider2D>().enabled = false;

		rightAttackingPoint2.GetComponent<BoxCollider2D>().enabled = false;
		leftAttackingPoint2.GetComponent<BoxCollider2D>().enabled = false;

		rightAttackingPoint3.GetComponent<BoxCollider2D>().enabled = false;
		leftAttackingPoint3.GetComponent<BoxCollider2D>().enabled = false;

	}

	public void FinishAttack(){

		comboActivated = false;
		comboCounter = 0;
		isAttacking = false;
		animator.SetInteger("comboCounter", 1);
		animator.SetBool("attack", false);

		DisableColliders();
	}

	void StartShootingGun(){

		SkillCooldownIndicator.instance.SpendSkillUse();
		gunComboCounter++;
		isShooting = true;
		animator.SetBool("shooting",true);
	}

	void ShootGun(){
		
		Vector3 position = shotOrigin.position;
		Vector2 direction = Vector2.left;
		float distance = gunDistance;

		if(isLookingRight){
			direction = Vector2.right;
		}

		RaycastHit2D hit = Physics2D.Raycast(position,direction,distance,LayerMask.GetMask("Enemies","Ground","EnemyProtection","LaserInteractableScenario","Scenario","Beat Em Up Character"));
		Debug.DrawRay(position, direction, Color.green);

		if(hit.collider != null){
			if(hit.collider.transform.position.x < transform.position.x){
				Instantiate(bulletFeedbackPrefab,new Vector3(hit.point.x,hit.point.y,0),Quaternion.Euler(new Vector3(0,-180,0)));
			}else{
				Instantiate(bulletFeedbackPrefab,new Vector3(hit.point.x,hit.point.y,0),Quaternion.identity);
			}

			if(hit.collider.tag == "Enemy"){

				Enemy enemyVariables;

				if (hit.collider.gameObject.GetComponentInParent<Enemy> ()) {
					enemyVariables = hit.collider.gameObject.GetComponentInParent<Enemy> ();
				} else {
					enemyVariables = hit.collider.gameObject.GetComponent<Enemy> ();
				}

	            if (enemyVariables.canReceiveDamage)
	            {
	                if (!enemyVariables.invulnerable)
	                {

	                    enemyVariables.life -= gunDamage;
	                    enemyVariables.receivedDamage = true;
	                    if(enemyVariables.GetComponent<CombableEnemy>()){
	                    	enemyVariables.GetComponent<CombableEnemy>().sourceOfDamagePosition = transform.position;
	                    }

	                }
	            }
			}else if(hit.collider.tag == "EnemyProtection"){
				
			}	
		}

		int randomIndex = Random.Range(0,gunShootingSounds.Length);
		myAudioSource.PlayOneShot(gunShootingSounds[randomIndex]);
	}

	void ShootHandCannon(){

		GameObject cannonBall;

		if(isLookingRight){
			cannonBall = Instantiate(handCannonBallPrefab,handCannonFiringPointRight.transform.position, Quaternion.identity);
			cannonBall.GetComponentInChildren<HandCannonBall>().goRight = true;
		}else{
			cannonBall = Instantiate(handCannonBallPrefab,handCannonFiringPointLeft.transform.position, Quaternion.identity);
			cannonBall.GetComponentInChildren<HandCannonBall>().goRight = false;
		}

		cannonBall.GetComponentInChildren<HandCannonBall>().positionToExplode = shadow.transform;

		int randomIndex = Random.Range(0,handCannonSound.Length);
		myAudioSource.PlayOneShot(handCannonSound[randomIndex]);
	}

	void ShootGiantCannon(){

		GameObject cannonBall;

		if(isLookingRight){
			cannonBall = Instantiate(giantCannonBallPrefab,giantCannonFiringPointRight.transform.position, Quaternion.identity);
			cannonBall.GetComponent<GiantCannonShot>().goRight = true;
		}else{
			cannonBall = Instantiate(giantCannonBallPrefab,giantCannonFiringPointLeft.transform.position, Quaternion.identity);
			cannonBall.GetComponent<GiantCannonShot>().goRight = false;
		}

		invulnerable = true;

		myAudioSource.PlayOneShot(bigCannonSound);
	}

	void StopShooting(){

		gunComboActivated = false;

		gunComboCounter = 0;

		animator.SetInteger("gunComboCounter", 1);

		animator.SetBool("shooting",false);
		isShooting = false;
		invulnerable = false;
	}

	void CanCombo(){
		canCombo = true;
	}

	void TryToChainAttack(){

		canCombo = false;

		if(comboActivated){

			animator.SetBool("attack", true);
			animator.SetBool("shooting", false);
			animator.SetInteger("gunComboCounter", gunComboCounter);

			gunComboActivated = false;
			comboActivated = false;
			
			DisableColliders();


			if (Input.GetKey (moveRightKey) || (Input.GetAxis (moveHorizontalGamepadAxis) >= 0.5f) ) {
				isLookingRight = true;
				ChangeLookingDirection();
			} else if (Input.GetKey (moveLeftKey) || (Input.GetAxis (moveHorizontalGamepadAxis) <= -0.5f)) {
				isLookingRight = false;
				ChangeLookingDirection();	
			}

			if(comboCounter == 1){

				animator.SetInteger("comboCounter", 1);


			}else if(comboCounter == 2){

				animator.SetInteger("comboCounter", 2);

			}else if(comboCounter >= 3){

				animator.SetInteger("comboCounter", 3);
			}




		}else if(gunComboActivated){

			if(SkillCooldownIndicator.instance.skillRemainingUses > 0){
				SkillCooldownIndicator.instance.SpendSkillUse();
				animator.SetBool("shooting", true);
				animator.SetBool("attack", false);
				animator.SetInteger("comboCounter", comboCounter);

				gunComboActivated = false;
				comboActivated = false;

				if (Input.GetKey (moveRightKey) || (Input.GetAxis (moveHorizontalGamepadAxis) >= 0.5f) ) {
					isLookingRight = true;
					ChangeLookingDirection();
				} else if (Input.GetKey (moveLeftKey) || (Input.GetAxis (moveHorizontalGamepadAxis) <= -0.5f)) {
					isLookingRight = false;
					ChangeLookingDirection();	
				}

				if(gunComboCounter == 1){

					
					animator.SetInteger("gunComboCounter", 1);

				}else if(gunComboCounter == 2){


					animator.SetInteger("gunComboCounter", 2);

				}else if(gunComboCounter == 3){
		
					animator.SetInteger("gunComboCounter", 3);
				}

			}else{
				gunComboActivated = false;
				comboActivated = false;
			}
		}
	}

	public void PrepareToBeLaunched(bool launchRight){
		if(launchRight){
			isLookingRight = true;
		}else{
			isLookingRight = false;
		}

		ChangeLookingDirection();

		mySpriteRenderer.enabled = false;
		shadow.GetComponent<SpriteRenderer>().enabled = false;
		invulnerable = true;
		inCannon = true;
		myRigidBody2D.velocity = new Vector2(0,0);

	}

	public void LaunchAnimation(){

		mySpriteRenderer.enabled = true;
		animator.SetBool("launched", true);
	}

	void FinishLandingAnimation(){
		animator.SetBool("landing", false);
		landing = false;
		shadow.GetComponent<SpriteRenderer>().enabled = true;

	}
}

