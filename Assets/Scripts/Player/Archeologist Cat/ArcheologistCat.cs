using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcheologistCat : Cat {

	public int gunDamage = 2;
	public float gunDistance = 2;

	public float timeToChargeWhipAttack = 1.5f;
	public float chargingElapsedTime = 0;

	public KeyCode attackKey;
	public KeyCode gunKey;
	public string gunGamepadButton;
	public string attackGamepadButton;

	public GameObject chargingParticles;
	public GameObject chargeCompleteParticles;
	private GameObject currentChargingParticles;
	private Coroutine spawnChargingParticlesCoroutine;

	public WhipCollider rightAttackingPoint;
	public WhipCollider leftAttackingPoint;

	public bool isShooting;
	public GameObject bulletFeedbackPrefab;

	public bool isPulling;
	private GameObject currentObjectBeingPulled;

	public bool isCharging;
	public bool charged;

	private bool movedRight;
	private bool movedLeft;
	private float initialSpeed;
	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();
		initialSpeed = speed;
	}
	// Update is called once per frame
	void Update () {
		if(!controlersDisabled){
			if(!isDying  && !freakoutMode){

				if((Input.GetButtonDown(freakoutGamepadButton) || Input.GetKeyDown(freakoutKey) ) && !isDying && !isJumping && !isAttacking && ready && !isCharging){
					freakoutMode = true;
					freakoutManager.PlayFreakOutSound ();
					animator.SetBool("freakout",true);
				}

				if(!isAttacking && !isShooting){

					if(!isCharging && !isPulling){
						if (Input.GetKey (moveRightKey) || (Input.GetAxis (moveHorizontalGamepadAxis) >= 0.5f) ) {
							MoveRight();
						} else if (Input.GetKey (moveLeftKey) || (Input.GetAxis (moveHorizontalGamepadAxis) <= -0.5f)) {
							MoveLeft();
						} else {
							Idle();
						}
					}else if(isCharging && !isPulling){
						if (Input.GetKey (moveRightKey) || (Input.GetAxis (moveHorizontalGamepadAxis) >= 0.5f) ) {
							isLookingRight = true;
							ChangeLookingDirection();
						} else if (Input.GetKey (moveLeftKey) || (Input.GetAxis (moveHorizontalGamepadAxis) <= -0.5f)) {
							isLookingRight = false;
							ChangeLookingDirection();
						}
					}else if(!isCharging && isPulling){
						if (Input.GetKey (moveRightKey) || (Input.GetAxis (moveHorizontalGamepadAxis) >= 0.5f) ) {
							MoveRightWhilePulling();
						} else if (Input.GetKey (moveLeftKey) || (Input.GetAxis (moveHorizontalGamepadAxis) <= -0.5f)) {
							MoveLeftWhilePulling();
						}
					}

					if((Input.GetKeyDown (jumpKey) || Input.GetButtonDown(jumpGamepadButton)) && !isCharging && !isPulling){
						if(!isFalling){

							if(!isJumping){
								Jump();
							} 
						}
					}

					if((Input.GetKeyDown (attackKey) || Input.GetButtonDown(attackGamepadButton)) && !isJumping && !isPulling){
						movedLeft = false;
							movedRight = false;
						StartCharging();
					}

					if((Input.GetKey (attackKey) || Input.GetButton(attackGamepadButton)) && isCharging){
						movedLeft = false;
							movedRight = false;
						if(chargingElapsedTime < timeToChargeWhipAttack){
							chargingElapsedTime += Time.deltaTime;
						}else{
							if(!charged){
								CompleteCharging();
							}
						}
					}

					if((Input.GetKeyUp (attackKey) || Input.GetButtonUp(attackGamepadButton)) && !isJumping && !isPulling){
						movedLeft = false;
							movedRight = false;
						if(charged){
							StartChargedAttack();
						 }else{
						  	StartAttack();
						 }
					}

					if((Input.GetKeyDown(gunKey ) || Input.GetButtonDown(gunGamepadButton)) && !isJumping && !isFalling && !isPulling)
	                {
						movedLeft = false;
							movedRight = false;
						StartShootingGun();
					}

					if(isPulling){
						if((Input.GetKeyDown (attackKey) || Input.GetButtonDown(attackGamepadButton))){
							StopPulling();
						}
					}


				}

				if(myRigidBody2D.velocity.y < -1){
					isFalling = true;
				}
			}
		}

		CheckInvulnerableTimeStamp ();


		if (invulnerable) {
			Flash ();
		}

		CheckIfDamageReceived ();
			
		CheckDeath ();


	}

	void FixedUpdate(){

		if(movedRight){
			MoveRight();	
		}else if(movedLeft){
			MoveLeft();
		}

	}

	protected void MoveRightWhilePulling(){

		myRigidBody2D.transform.position += Vector3.right * speed * Time.deltaTime;

	}

	protected void MoveLeftWhilePulling(){

		myRigidBody2D.transform.position += Vector3.left * speed * Time.deltaTime;

	}

	void StartCharging(){
		if(spawnChargingParticlesCoroutine != null){
			StopCoroutine(spawnChargingParticlesCoroutine);
		}
		spawnChargingParticlesCoroutine = StartCoroutine(SpawnChargingParticle());
		isCharging = true;
		chargingElapsedTime = 0;
		animator.SetBool("charging", true);
	}

	void CompleteCharging(){
		RemoveChargingParticles();
		SpawnCompleteChargeParticle();
		print("ChargeComplete");
		charged = true;
	}

	void StartChargedAttack(){
		chargingElapsedTime = 0;
		charged = false;
		isCharging = false;
		isAttacking = true;
		animator.SetBool("chargeAttack", true);
		animator.SetBool("charging", false);
		rightAttackingPoint.charged = true;
		leftAttackingPoint.charged = true;
	}

	public void FinishChargedAttack(){

		charged = false;
		isAttacking = false;
		animator.SetBool("chargeAttack", false);
		animator.SetBool("idle", true);
		rightAttackingPoint.GetComponent<BoxCollider2D>().enabled = false;
		leftAttackingPoint.GetComponent<BoxCollider2D>().enabled = false;
		rightAttackingPoint.charged = false;
		leftAttackingPoint.charged = false;

	}

	void StartAttack(){
		if(spawnChargingParticlesCoroutine != null){
			StopCoroutine(spawnChargingParticlesCoroutine);
		}
		RemoveChargingParticles();
		chargingElapsedTime = 0;
		isCharging = false;
		charged = false;
		isAttacking = true;
		animator.SetBool("attacking", true);
		animator.SetBool("charging", false);
	}

	public void ActivateAttackCollider(){

		if(isLookingRight){
			rightAttackingPoint.GetComponent<BoxCollider2D>().enabled = true;
		} else {
			leftAttackingPoint.GetComponent<BoxCollider2D>().enabled = true;
		}
	}

	public void FinishAttack(){
		
		isAttacking = false;
		animator.SetBool("attacking", false);
		rightAttackingPoint.GetComponent<BoxCollider2D>().enabled = false;
		leftAttackingPoint.GetComponent<BoxCollider2D>().enabled = false;
	}

	void StartShootingGun(){
		animator.SetBool("shooting",true);
		isShooting = true;
		ShootGun();
	}

	void ShootGun(){
		
		Vector3 position = transform.position;
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

				Enemy enemyVariables = hit.collider.gameObject.GetComponent<Enemy>();

	            if (enemyVariables.canReceiveDamage)
	            {
	                if (!enemyVariables.invulnerable)
	                {

	                    enemyVariables.life -= gunDamage;
	                    enemyVariables.receivedDamage = true;

	                }
	            }
			}else if(hit.collider.tag == "EnemyProtection"){
				
			}else if(hit.collider.gameObject.GetComponent<StatueColorChangingSwitch>()){
				hit.collider.gameObject.GetComponent<StatueColorChangingSwitch>().ToggleActivation();
			}	
		}
	}

	void StopShooting(){
		animator.SetBool("shooting",false);
		isShooting = false;
	}

	public void StartPulling(Collider2D objectToPull){
		isPulling = true;
		isAttacking = false;
		animator.SetBool("pulling",true);
		speed = speed/2;
		currentObjectBeingPulled = objectToPull.gameObject;
		currentObjectBeingPulled.transform.parent = gameObject.transform;

		rightAttackingPoint.GetComponent<BoxCollider2D>().enabled = false;
		leftAttackingPoint.GetComponent<BoxCollider2D>().enabled = false;
		rightAttackingPoint.charged = false;
		leftAttackingPoint.charged = false;
	}

	void StopPulling(){
		isPulling = false;
		FinishChargedAttack();
		animator.SetBool("pulling",false);
		speed = initialSpeed;
		currentObjectBeingPulled.transform.parent = null;
		currentObjectBeingPulled = null;
	}

	void RemoveChargingParticles(){
		Destroy(currentChargingParticles);
		currentChargingParticles = null;
	}

	void SpawnCompleteChargeParticle(){
		Instantiate(chargeCompleteParticles, transform.position, Quaternion.identity);
	}

	IEnumerator SpawnChargingParticle(){
	     yield return new WaitForSeconds(0.2f);
		 currentChargingParticles = (GameObject)Instantiate(chargingParticles, transform.position, Quaternion.identity);
	}


}
