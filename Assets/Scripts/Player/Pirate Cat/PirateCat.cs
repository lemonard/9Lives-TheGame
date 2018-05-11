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

	public bool isShooting;
	public GameObject bulletFeedbackPrefab;

	public int comboCounter = 0;
	public bool canCombo = false;
	public bool comboActivated = false;

	private int gunComboCounter = 0;
	private bool canGunCombo = false;
	private bool gunComboActivated = false;

	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();

	}
	// Update is called once per frame
	void Update () {
		if(!controlersDisabled){
			if(!isDying  && !freakoutMode){

				if((Input.GetButtonDown(freakoutGamepadButton) || Input.GetKeyDown(freakoutKey) ) && !isDying && !isJumping && !isAttacking && ready){
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
							canCombo = false;
							comboCounter++;
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

	void CanCombo(){
		canCombo = true;
	}

	void TryToChainAttack(){

		canCombo = false;

		if(comboActivated){

			DisableColliders();

			if(comboCounter == 2){

				comboActivated = false;
				animator.SetInteger("comboCounter", 2);

			}else if(comboCounter == 3){

				comboActivated = false;
				animator.SetInteger("comboCounter", 3);
			}

		}
	}


}

