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
	public bool arenaEntrance;
	public Transform arenaEntrancePoint;
	public bool arenaEntranceSpotToTheRight;

	public bool arenaEnemy;


	protected bool playerIsPirate;
	protected PirateCat pirateCat;
	protected bool pirateIsKnockedDown;

	// Use this for initialization
	protected void CombableEnemyInitialize () {
		EnemyInitialize();

		if(player.GetComponent<PirateCat>()){
			playerIsPirate = true;
			pirateCat = player.GetComponent<PirateCat>();
		}

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
		myAnimator.SetBool("damage",true);
		myAnimator.SetBool("knockedDown", true);
		GetComponentInParent<Rigidbody2D>().velocity = Vector2.zero;

		if(sourceOfDamagePosition.x > transform.position.x){ //Enemy is on the right

			GetComponentInParent<Rigidbody2D>().velocity = new Vector2( -3f , GetComponentInParent<Rigidbody2D>().velocity.y + 3);
	
		}else{

			GetComponentInParent<Rigidbody2D>().velocity = new Vector2( 3f , GetComponentInParent<Rigidbody2D>().velocity.y + 3);	
	
		}
	}

	protected void FinishedKnockDownAnimation(){

		myAnimator.SetBool("damage",false);
		myAnimator.SetBool("knockedDown", false);
		knockedDownTimeStamp = Time.time + knockedDownTime;
	}

	public void StandUp(){
		knockedDownTimeStamp = 0;
		myAnimator.SetBool("knockedOnFloor", false);
		myAnimator.SetBool("knockedDown", false);
		myAnimator.SetBool("standUp", true);
	}

	protected void FinishStandUpAnimation(){
		invulnerable = false;
		knockedDown = false;
		myAnimator.SetBool("standUp", false);
	}

	public IEnumerator StartWaiting(){

    	waiting = true;
    	yield return new WaitForSeconds(waitTime);
    	waiting = false;

    }

	override public void Disappear(){
		DropItem ();
		if (mySpawner != null) {
			mySpawner.SetDeadEnemy (this.gameObject.GetInstanceID ());
		}

		Destroy (gameObject.transform.parent.gameObject);
	}

}
