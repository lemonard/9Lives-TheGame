using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateCat : Cat {

	public int gunDamage = 2;
	public float gunDistance = 2;


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

	public bool isShooting;
	public bool wasThrown;
	public GameObject bulletFeedbackPrefab;

	public int amountOfHitsTaken;

	public bool canCombo = false;

	public int comboCounter = 0;
	public bool comboActivated = false;

	public int gunComboCounter = 0;
	public bool gunComboActivated = false;

	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();

	}
	// Update is called once per frame
	void Update () {
		if(!controlersDisabled){
			if(!isDying  && !freakoutMode){

				if((Input.GetButtonDown(freakoutGamepadButton) || Input.GetKeyDown(freakoutKey) ) && !isDying && !isJumping && !isAttacking && !isShooting && ready){
					freakoutMode = true;
					freakoutManager.PlayFreakOutSound ();
					animator.SetBool("freakout",true);
				}

				if(!isAttacking && !isShooting){

	
					if (Input.GetKey (moveRightKey) || (Input.GetAxis (moveHorizontalGamepadAxis) >= 0.5f) ) {
						MoveRight();
					} else if (Input.GetKey (moveLeftKey) || (Input.GetAxis (moveHorizontalGamepadAxis) <= -0.5f)) {
						MoveLeft();
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
						StartAttack();
					}


					if((Input.GetKeyDown(gunKey ) || Input.GetButtonDown(gunGamepadButton)) && !isJumping && !isFalling)
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

			CheckInvulnerableTimeStamp ();
		}


		if (invulnerable) {
			Flash ();
		}

		CheckIfDamageReceived ();
			
		CheckDeath ();

	}

	void FixedUpdate(){


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

	protected override void CheckIfDamageReceived ()
	{
		if (receivedDamage && life > 0) {
			//animator.SetBool("damage",true);
			ToggleInvinsibility ();

		}
	}

	protected override void CheckDeath ()
	{
		if(life <= 0 ){
			isDying = true;
			animator.SetBool("dying",true);
			animator.SetInteger("comboCounter", 1);
		}
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

		}else if(comboCounter == 2){

			if(isLookingRight){
				rightAttackingPoint2.GetComponent<BoxCollider2D>().enabled = true;
			} else {
				leftAttackingPoint2.GetComponent<BoxCollider2D>().enabled = true;
			}

		}else if(comboCounter == 3){

			if(isLookingRight){
				rightAttackingPoint3.GetComponent<BoxCollider2D>().enabled = true;
			} else {
				leftAttackingPoint3.GetComponent<BoxCollider2D>().enabled = true;
			}
		}


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

		RaycastHit2D hit = Physics2D.Raycast(position,direction,distance,LayerMask.GetMask("Enemies","Ground","EnemyProtection","LaserInteractableScenario","Scenario"));
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

	                }
	            }
			}else if(hit.collider.tag == "EnemyProtection"){
				
			}	
		}
	}

	void ShootHandCannon(){

		GameObject cannonBall;

		if(isLookingRight){
			cannonBall = Instantiate(handCannonBallPrefab,handCannonFiringPointRight.transform.position, Quaternion.identity);
			cannonBall.GetComponent<HandCannonBall>().goRight = true;
		}else{
			cannonBall = Instantiate(handCannonBallPrefab,handCannonFiringPointLeft.transform.position, Quaternion.identity);
			cannonBall.GetComponent<HandCannonBall>().goRight = false;
		}


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


	}

	void StopShooting(){

		gunComboActivated = false;

		gunComboCounter = 0;

		animator.SetInteger("gunComboCounter", 1);

		animator.SetBool("shooting",false);
		isShooting = false;
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

			}else if(comboCounter == 3){

				animator.SetInteger("comboCounter", 3);
			}




		}else if(gunComboActivated){

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

		
		}
	}


}

