using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombableEnemy : Enemy {

	public int amountOfHitsToBeKnockedDown = 3;
	public int amountOfHitsTaken = 0;
	public bool knockedDown;
	public float knockedDownTime = 2f;
	public bool receivingDamage;

	protected float knockedDownTimeStamp = 0f;

	public Vector3 sourceOfDamagePosition;


	public float waitTime;
	protected bool waiting;
	protected Coroutine waitingCoroutine;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ReceiveCombo(){
		receivedDamage = false;
		amountOfHitsTaken++;
		receivingDamage = true;

		if(amountOfHitsTaken < amountOfHitsToBeKnockedDown){
			if(myAnimator.GetBool("damage")){
				myAnimator.Play("ReceiveDamage",0,0f);
			}else{
				myAnimator.SetBool("damage",true);
			}

		}else{
			if(!dying){
				KnockDown();
			}
		}



	}

	protected void FinishDamageAnimation(){
		
		myAnimator.SetBool("damage",false);
		receivingDamage = false;
	}

	public void KnockDown(){
		amountOfHitsTaken = 0;
		knockedDown = true;
		invulnerable = true;
		receivingDamage = false;
		//myAnimator.SetBool("knockedDown", true);
		if(sourceOfDamagePosition.x > transform.position.x){ //Enemy is on the right

			GetComponent<Rigidbody2D>().velocity = new Vector2( -5f , GetComponent<Rigidbody2D>().velocity.y + 3);
	
		}else{

			GetComponent<Rigidbody2D>().velocity = new Vector2( 5f , GetComponent<Rigidbody2D>().velocity.y + 3);	
	
		}
	}

	protected void FinishedKnockDownAnimation(){

		knockedDownTimeStamp = Time.time + knockedDownTime;
	}

	public void StandUp(){
		//myAnimator.SetBool("knockedDown", false);
		//myAnimator.SetBool("standUp", true);
	}

	protected void FinishStandUpAnimation(){
		invulnerable = false;
		knockedDown = false;
	}

	public IEnumerator StartWaiting(){

    	waiting = true;
    	yield return new WaitForSeconds(waitTime);
    	waiting = false;

    }

}
