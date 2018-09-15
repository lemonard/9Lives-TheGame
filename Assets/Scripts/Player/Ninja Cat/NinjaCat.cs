using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaCat : Cat {

	public Transform rightFiringPoint;
	public Transform leftFiringPoint;
	public GameObject projectile;
	public Collider2D rightAttackingPoint;
	public Collider2D leftAttackingPoint;
	public GameObject wallJumpParticlePrefab;

	public KeyCode attackKey;
	public KeyCode gunKey;
	public string gunGamepadButton;
	public string attackGamepadButton;

	public bool isShooting;
	public bool wallSliding;
	public bool wallJumping;



	private bool slidingWallLeft; //Defines if the wall that the cat is sliding is on the left side or the right side

	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();

	}
	
	// Update is called once per frame
	void Update () {
		if (!controlersDisabled) {
			if (!isDying && !freakoutMode) {

				if ((Input.GetButtonDown (freakoutGamepadButton) || Input.GetKeyDown (freakoutKey)) && !isDying && !isJumping && !isAttacking && ready) {
					freakoutMode = true;
					freakoutManager.PlayFreakOutSound ();
					animator.SetBool ("freakout", true);
				}

				if (!isAttacking) {

					if(!wallSliding && !wallJumping){
						if (Input.GetKey (moveRightKey) || (Input.GetAxis (moveHorizontalGamepadAxis) >= 0.5f)) {
							movedLeft = false;
							movedRight = true;

						} else if (Input.GetKey (moveLeftKey) || (Input.GetAxis (moveHorizontalGamepadAxis) <= -0.5f)) {
							movedLeft = true;
							movedRight = false;

						} else {
							ResetMovementVariables();
						}
					}

					if(wallSliding){
						if(slidingWallLeft){

							if (Input.GetKey (moveRightKey) || (Input.GetAxis (moveHorizontalGamepadAxis) >= 0.5f)) {
								movedLeft = false;
								movedRight = true;
							}
						}else{

							if (Input.GetKey (moveLeftKey) || (Input.GetAxis (moveHorizontalGamepadAxis) <= -0.5f)) {
								movedLeft = true;
								movedRight = false;
							}
						}
					}

					if ((Input.GetKeyDown (jumpKey) || Input.GetButtonDown (jumpGamepadButton))) {
						if(!wallSliding){
							if (!isFalling) {

								if (!isJumping) {
									Jump ();
								} 
							}
						}else{
							WallJump ();
						}
					}

					if ( (Input.GetKeyDown (attackKey) || Input.GetButtonDown (attackGamepadButton)) && !isJumping && !isFalling && !isShooting && !wallSliding) {
						StartAttack();
					}


					if ( (Input.GetKeyDown (attackKey) || Input.GetButtonDown (attackGamepadButton)) && (isJumping || isFalling) && !isShooting && !wallSliding) {
						StartJumpAttack();
					}


				}else{
					ResetMovementVariables();
				}

				if (myRigidBody2D.velocity.y < -1) {
					isFalling = true;
				}

			}

			CheckInvulnerableTimeStamp ();
		} else {
			ResetMovementVariables();
		}


		if (invulnerable) {
			Flash ();
		}

		CheckIfDamageReceived ();
			
		CheckDeath ();

	}

	void FixedUpdate(){
		if(!isDying && !freakoutMode && !isAttacking && !wallJumping){
			if (movedRight) {
				MoveRight ();
			} else if (movedLeft) {
				MoveLeft ();
			} else {
				Idle ();
			}
		}
	}

	public override void IsGrounded ()
	{

		isJumping = false;
		isFalling = false;
		myRigidBody2D.velocity = new Vector2(myRigidBody2D.velocity.x,0);
		wallSliding = false;
		myRigidBody2D.gravityScale = 1f;
		animator.SetBool("jumping", false);
		animator.SetBool("sliding", false);
		animator.SetBool("wallJump",false);
		wallJumping = false;
	}

	void WallJump(){

		
		myRigidBody2D.gravityScale = 1f;
		animator.SetBool("jumping", true);
		animator.SetBool("wallJump",true);
		animator.SetBool("sliding", false);

		GameObject wallJumpParticle = Instantiate(wallJumpParticlePrefab, transform.position, Quaternion.identity);

		myRigidBody2D.velocity = new Vector3(myRigidBody2D.velocity.x,0,0);

		if(slidingWallLeft){
			myRigidBody2D.AddForce(new Vector3(jumpForce - 1, jumpForce,0), ForceMode2D.Impulse);
		}else{
			myRigidBody2D.AddForce(new Vector3(-jumpForce + 1, jumpForce,0), ForceMode2D.Impulse);
			wallJumpParticle.GetComponent<SpriteRenderer>().flipX = true;
		}

		wallJumping = true;
		isJumping = true;

	}

	void StartAttack(){


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

	void StartJumpAttack(){

		isShooting = true;
		animator.SetBool("shooting", true);

	}

	public void SpawnProjectile(){

		if(isLookingRight){
			
		} else {
			
		}
	}

	public void FinishJumpAttack(){
		
		isShooting = false;
		animator.SetBool("shooting", false);

	}

	void StartSliding(Collision2D wall){
		if(isJumping || wallJumping){
				isJumping = false;
				wallSliding = true;
				wallJumping = false;
				animator.SetBool("sliding", true);
				animator.SetBool("jumping", false);
				movedLeft = false;
				movedRight = false;
				myRigidBody2D.gravityScale = 0.5f;
				myRigidBody2D.velocity = new Vector3(myRigidBody2D.velocity.x,0,0);

			if (wall.gameObject.transform.position.x >= transform.position.x) {
				slidingWallLeft = false;
				isLookingRight = false;
				ChangeLookingDirection();

			} else if (wall.gameObject.transform.position.x < transform.position.x) {
				slidingWallLeft = true;
				isLookingRight = true;
				ChangeLookingDirection();
			}
		}
	}

	protected override void OnCollisionEnter2D(Collision2D other){
		base.OnCollisionEnter2D (other);

		if (other.gameObject.tag == "SlidingWall" && (isJumping || isFalling) && !isShooting) {
			print("Sliding Wall");

			StartSliding(other);
		}
	}

	protected void OnCollisionExit2D(Collision2D other){

		if (other.gameObject.tag == "SlidingWall") {

			print("Sliding Wall saiu");
			wallSliding = false;
			animator.SetBool("sliding", false);
			myRigidBody2D.gravityScale = 1f;


	
		}
	}
}
