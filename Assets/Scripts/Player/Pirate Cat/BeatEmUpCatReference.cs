using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatEmUpCatReference : MonoBehaviour {

	private Rigidbody2D myRigidBody2D;

	private PirateCat myCat;

	public float maxY;
	public float minY = 0;


	private bool movedRight;
	private bool movedLeft;

	void Awake(){
		myRigidBody2D = GetComponent<Rigidbody2D>();
		myCat = GetComponentInChildren<PirateCat>();

	}
	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {
		if(!myCat.controlersDisabled){
			if(!myCat.isDying  && !myCat.freakoutMode && !myCat.beingLaunched && !myCat.receivingDamage && !myCat.knockedDown && !myCat.landing && !myCat.isAttacking && !myCat.isShooting){
		
				if (Input.GetKey (myCat.moveRightKey) || (Input.GetAxis (myCat.moveHorizontalGamepadAxis) >= 0.5f) ) {
					movedRight = true;
					movedLeft = false;
				} else if (Input.GetKey (myCat.moveLeftKey) || (Input.GetAxis (myCat.moveHorizontalGamepadAxis) <= -0.5f)) {
					movedLeft = true;
					movedRight = false;
				}else{
					movedRight = false;
					movedLeft = false;
				}
				
				if((Input.GetKeyDown (myCat.jumpKey) || Input.GetButtonDown(myCat.jumpGamepadButton))){
					if(!myCat.isFalling){
					
						if(!myCat.isJumping){
							Jump();
						} 
					}
				}
			}else{
				movedLeft = false;
				movedRight = false;
			}
		}else{
			movedLeft = false;
			movedRight = false;
		}

//		if(myCat.isJumping){
//			float jumpDistance = myCat.transform.position.y - catYPositionBeforeJumping;
//			myCat.shadow.transform.position = new Vector3(myCat.transform.position.x, myCat.transform.position.y - jumpDistance, myCat.transform.position.z);
//		}
	}

	protected void MoveRight(){


		myRigidBody2D.transform.position += Vector3.right * myCat.speed * Time.deltaTime;
		myCat.shadowReference.GetComponent<Rigidbody2D>().transform.position += Vector3.right * myCat.speed * Time.deltaTime;
	
	}

	protected void MoveLeft(){

		myRigidBody2D.transform.position += Vector3.left * myCat.speed * Time.deltaTime;
		myCat.shadowReference.GetComponent<Rigidbody2D>().transform.position += Vector3.left * myCat.speed * Time.deltaTime;
	
	}

	protected void Jump(){

		myRigidBody2D.velocity = new Vector3(myRigidBody2D.velocity.x,0,0);

		myRigidBody2D.AddForce(new Vector3(0, myCat.jumpForce,0), ForceMode2D.Impulse);

		
	}

	void FixedUpdate(){
		if(!myCat.isAttacking && !myCat.isShooting && !myCat.isDying  && !myCat.freakoutMode && !myCat.beingLaunched && !myCat.receivingDamage && !myCat.knockedDown && !myCat.landing && !myCat.controlersDisabled){
			if (movedRight) {
				MoveRight();
			} else if (movedLeft) {
				MoveLeft();
			}
		}
	}


	protected virtual void OnCollisionEnter2D(Collision2D other){


		if(other.gameObject.tag == "Ground" || other.gameObject.tag == "Enemy"){
			if(!myCat.justJumped){
				CheckIfGrounded();
			}
		}

    }

	protected void CheckIfGrounded(){

		Vector3 position = transform.position;
		Vector2 direction = Vector2.down;
		Vector2 direction2 = new Vector2(-0.4f,-1);
		Vector2 direction3 = new Vector2(0.4f,-1);
		float distance = 0.5f;
		if (!myCat.FourLeggedCat) {
			distance = 1f;
		}

		RaycastHit2D hit = Physics2D.Raycast(position,direction,distance,LayerMask.GetMask("Enemies","Ground"));


		if(hit.collider != null){
			IsGrounded();
		}


	}

	public void IsGrounded ()
	{

		myCat.isJumping = false;
		myCat.isFalling = false;
		if(myCat.beingLaunched){

			myRigidBody2D.gravityScale = 1;
			myRigidBody2D.velocity = new Vector2(0,0);
			myCat.beingLaunched = false;
			myCat.invulnerable = false;
			myCat.landing = true;
			myCat.animator.SetBool("launched", false);
			myCat.animator.SetBool("landing", true);
			CameraController.instance.RestoreSpeedX();
			CameraController.instance.ReturnCameraToPlayerFollowing();
			myCat.shadow.GetComponent<SpriteRenderer>().enabled = true;
		}else if(myCat.knockedDown){

			myCat.animator.SetBool("knockedOnFloor", true);
			myCat.animator.SetBool("knockedDown", false);
		}else{
			myRigidBody2D.velocity = new Vector2(myRigidBody2D.velocity.x,0);
		}

		myCat.animator.SetBool("jumping", false);
	}

}
