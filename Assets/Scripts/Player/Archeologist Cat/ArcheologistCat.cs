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

	public Transform particleOrigin;

	public bool isShooting;
	public GameObject bulletFeedbackPrefab;

	public bool isPulling;
	private GameObject currentObjectBeingPulled;
	public bool isPushing;
	public bool isObjectBeingPushedOnTheRight;

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

					if(!isCharging && !isPulling && !isPushing){
						if (Input.GetKey (moveRightKey) || (Input.GetAxis (moveHorizontalGamepadAxis) >= 0.5f) ) {
							MoveRight();
						} else if (Input.GetKey (moveLeftKey) || (Input.GetAxis (moveHorizontalGamepadAxis) <= -0.5f)) {
							MoveLeft();
						} else {
							Idle();
						}
					}else if(isCharging && !isPulling && !isPushing){
						if (Input.GetKey (moveRightKey) || (Input.GetAxis (moveHorizontalGamepadAxis) >= 0.5f) ) {
							isLookingRight = true;
							ChangeLookingDirection();
						} else if (Input.GetKey (moveLeftKey) || (Input.GetAxis (moveHorizontalGamepadAxis) <= -0.5f)) {
							isLookingRight = false;
							ChangeLookingDirection();
						}
					}else if(!isCharging && isPulling && !isPushing){
						if (Input.GetKey (moveRightKey) || (Input.GetAxis (moveHorizontalGamepadAxis) >= 0.5f)) {
							MoveRightWhilePulling ();
						} else if (Input.GetKey (moveLeftKey) || (Input.GetAxis (moveHorizontalGamepadAxis) <= -0.5f)) {
							MoveLeftWhilePulling ();
						} else if (!Input.GetKey (moveRightKey) && !Input.GetKey (moveLeftKey) && !(Input.GetAxis (moveHorizontalGamepadAxis) >= 0.5f) && !(Input.GetAxis (moveHorizontalGamepadAxis) <= -0.5f)) {
							isWalking = false;
						}
					}else if(!isCharging && !isPulling && isPushing){
						if (Input.GetKey (moveRightKey) || (Input.GetAxis (moveHorizontalGamepadAxis) >= 0.5f)) {
							MoveRightWhilePushing ();
						} else if (Input.GetKey (moveLeftKey) || (Input.GetAxis (moveHorizontalGamepadAxis) <= -0.5f)) {
							MoveLeftWhilePushing ();
						} else if (!Input.GetKey (moveRightKey) && !Input.GetKey (moveLeftKey) && !(Input.GetAxis (moveHorizontalGamepadAxis) >= 0.5f) && !(Input.GetAxis (moveHorizontalGamepadAxis) <= -0.5f)) {
							isWalking = false;
						}
					}

					if((Input.GetKeyDown (jumpKey) || Input.GetButtonDown(jumpGamepadButton)) && !isCharging && !isPulling){
						if(!isFalling){

							if(!isJumping){
								if (isPushing) {
									StopPushing ();
								}
								Jump();
							} 
						}
					}

					if((Input.GetKeyDown (attackKey) || Input.GetButtonDown(attackGamepadButton)) && !isJumping && !isPulling && !isPushing){
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
						if (!isWalking) {
							animator.speed = 0;
						}

						if((Input.GetKeyUp (attackKey) || Input.GetButtonUp(attackGamepadButton))){
							StopPulling();
						}
					}

					if (isPushing) {
						if (!isWalking) {
							animator.speed = 0;
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

		if(movedRight){
			MoveRight();	
		}else if(movedLeft){
			MoveLeft();
		}

	}

	protected override void OnCollisionEnter2D(Collision2D other){
		base.OnCollisionEnter2D (other);

		if (other.gameObject.tag == "ButtomPressingObjects" && !isPulling && !isJumping && !isFalling && !isAttacking && !isCharging && !isShooting && !isPushing) {
			if (other.gameObject.transform.position.x >= transform.position.x) {
				isObjectBeingPushedOnTheRight = true;
			} else if (other.gameObject.transform.position.x < transform.position.x) {
				isObjectBeingPushedOnTheRight = false;
			}

			StartPushing ();
		}
	}

	protected override void CheckIfDamageReceived ()
	{
		if (receivedDamage && life > 0) {
			//animator.SetBool("damage",true);
			ToggleInvinsibility ();
			if(isCharging){
				CancelCharging();
			}
			if(charged){
				charged = false;
			}
		}
	}

	protected void MoveRightWhilePulling(){
		if (currentObjectBeingPulled.transform.position.x > transform.position.x) {
			StopPulling ();
		} else {
			myRigidBody2D.transform.position += Vector3.right * speed * Time.deltaTime;
			isWalking = true;
			animator.speed = 1;
		}

	}

	protected void MoveLeftWhilePulling(){
		if (currentObjectBeingPulled.transform.position.x < transform.position.x) {
			StopPulling ();
		} else {
			myRigidBody2D.transform.position += Vector3.left * speed * Time.deltaTime;
			isWalking = true;
			animator.speed = 1;
		}

	}

	protected void MoveRightWhilePushing(){
		if (!isObjectBeingPushedOnTheRight) {
			StopPushing ();
		} else {
			myRigidBody2D.transform.position += Vector3.right * speed * Time.deltaTime;
			isWalking = true;
			animator.speed = 1;
		}
	}

	protected void MoveLeftWhilePushing(){
		if (isObjectBeingPushedOnTheRight) {
			StopPushing ();
		} else {
			myRigidBody2D.transform.position += Vector3.left * speed * Time.deltaTime;
			isWalking = true;
			animator.speed = 1;
		}
	}

	void StartCharging(){
		if(spawnChargingParticlesCoroutine != null){
			StopCoroutine(spawnChargingParticlesCoroutine);
		}
		spawnChargingParticlesCoroutine = StartCoroutine(SpawnChargingParticle());
		isCharging = true;
		chargingElapsedTime = 0;
	}

	void CancelCharging(){
		if(spawnChargingParticlesCoroutine != null){
			StopCoroutine(spawnChargingParticlesCoroutine);
		}
		animator.SetBool("charging", false);
		isCharging = false;
		RemoveChargingParticles();
	}

	void CompleteCharging(){
		RemoveChargingParticles();
		SpawnCompleteChargeParticle();
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
				
			}else if(hit.collider.gameObject.GetComponent<StatueColorChangingSwitch>()){
				hit.collider.gameObject.GetComponent<StatueColorChangingSwitch>().ToggleActivation();
			}else if(hit.collider.gameObject.GetComponent<Torch>()){
				hit.collider.gameObject.GetComponent<Torch>().Activate();
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

		GetComponent<PullingWhipRenderer> ().RenderWhip (isLookingRight, objectToPull.gameObject);

		rightAttackingPoint.GetComponent<BoxCollider2D>().enabled = false;
		leftAttackingPoint.GetComponent<BoxCollider2D>().enabled = false;
		rightAttackingPoint.charged = false;
		leftAttackingPoint.charged = false;
	}

	void StopPulling(){
		animator.speed = 1;
		isPulling = false;
		FinishChargedAttack();

		GetComponent<PullingWhipRenderer> ().DestroyWhip();

		animator.SetBool("pulling",false);
		speed = initialSpeed;
		currentObjectBeingPulled.GetComponent<PullableObject> ().UnwrapObject ();
		currentObjectBeingPulled.transform.parent = null;
		currentObjectBeingPulled = null;
	}

	void RemoveChargingParticles(){
		Destroy(currentChargingParticles);
		currentChargingParticles = null;
	}

	public void StartPushing(){
		isPushing = true;
		animator.SetBool ("pushing", true);

		rightAttackingPoint.GetComponent<BoxCollider2D>().enabled = false;
		leftAttackingPoint.GetComponent<BoxCollider2D>().enabled = false;
		rightAttackingPoint.charged = false;
		leftAttackingPoint.charged = false;
	}

	public void StopPushing(){
		animator.speed = 1;
		isPushing = false;
		animator.SetBool ("pushing", false);
		speed = initialSpeed;
	}

	void SpawnCompleteChargeParticle(){
		Instantiate(chargeCompleteParticles, particleOrigin.position, Quaternion.identity);
	}

	IEnumerator SpawnChargingParticle(){
	     yield return new WaitForSeconds(0.2f);
		animator.SetBool("charging", true);
		currentChargingParticles = (GameObject)Instantiate(chargingParticles, particleOrigin.position, Quaternion.identity,gameObject.transform);
	}


}
