﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaCat : Cat {

	public Transform rightFiringPoint;
	public Transform leftFiringPoint;
	public GameObject projectile;
	public Collider2D rightAttackingPoint;
	public Collider2D leftAttackingPoint;

	public KeyCode attackKey;
	public KeyCode gunKey;
	public string gunGamepadButton;
	public string attackGamepadButton;

	public bool isShooting;

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

				if (!isAttacking && !isShooting) {


					if (Input.GetKey (moveRightKey) || (Input.GetAxis (moveHorizontalGamepadAxis) >= 0.5f)) {
						MoveRight ();
					} else if (Input.GetKey (moveLeftKey) || (Input.GetAxis (moveHorizontalGamepadAxis) <= -0.5f)) {
						MoveLeft ();
					} else {
						Idle ();
					}
					

					if ((Input.GetKeyDown (jumpKey) || Input.GetButtonDown (jumpGamepadButton))) {
						if (!isFalling) {

							if (!isJumping) {
								Jump ();
							} 
						}
					}

					if ( (Input.GetKeyDown (attackKey) || Input.GetButtonDown (attackGamepadButton)) && !isJumping && !isFalling) {
						
					}


					if ( (Input.GetKeyDown (attackKey) || Input.GetButtonDown (attackGamepadButton)) && (isJumping || isFalling) ) {

					}


				}

				if (myRigidBody2D.velocity.y < -1) {
					isFalling = true;
				}

			}

			CheckInvulnerableTimeStamp ();
		} else {
			Idle ();
		}


		if (invulnerable) {
			Flash ();
		}

		CheckIfDamageReceived ();
			
		CheckDeath ();

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
}