﻿using UnityEngine;
using System.Collections;

public class HouseCat : Cat {

	public KeyCode enterPortalKey;
	public bool isNearPortal;

	public Portal nearestPortal;
	public string enterPortalGamepadButton;

	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		if(!controlersDisabled){
			if(!isDying){

				if(!isAttacking){

					if (Input.GetKey (moveRightKey) || (Input.GetAxis (moveHorizontalGamepadAxis) >= 0.5f)) {
						movedLeft = false;
						movedRight = true;

					} else if (Input.GetKey (moveLeftKey) || (Input.GetAxis (moveHorizontalGamepadAxis) <= -0.5f)) {
						movedLeft = true;
						movedRight = false;

					} else {
						ResetMovementVariables();
					}

					if((Input.GetKeyDown (jumpKey) || Input.GetButtonDown(jumpGamepadButton)) && !isNearPortal){
						if(!isFalling){

							if(!isJumping){
								Jump();
							} 
						}
					}

	//				if(isFalling)
	//				{
	//					animator.SetBool("jumping", false);
	//				}

					if((Input.GetKeyDown (enterPortalKey) || Input.GetButtonDown(enterPortalGamepadButton)) && isNearPortal ){
						EnterPortal();
					}

				}else{
					ResetMovementVariables();
				}

				if(myRigidBody2D.velocity.y < -1){
					isFalling = true;
				}

			}
		}else{
			ResetMovementVariables();
		}

		if (invulnerableTimeStamp < Time.time) {
			invulnerable = false;
			mySpriteRenderer.enabled = true;
		}

		if (invulnerable) {
			Flash ();
		}

		if (receivedDamage && life > 0) {
			ToggleInvinsibility ();
		}

		if(life <= 0 ){
			isDying = true;
			Destroy(gameObject);
			//animator.SetBool("dying",true);
		}
	}

	void FixedUpdate(){
		if(!isAttacking && !isDying){
			if (movedRight) {
				MoveRight ();
			} else if (movedLeft) {
				MoveLeft ();
			} else {
				Idle ();
			}
		}
	}


	void EnterPortal(){
		if(nearestPortal != null){
			DisableControls();
			StartCoroutine(nearestPortal.Enter());
		}
	}
}
