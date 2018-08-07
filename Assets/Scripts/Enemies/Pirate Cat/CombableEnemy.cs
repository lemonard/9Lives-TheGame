using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombableEnemy : Enemy {

	public int amountOfHitsToBeKnockedDown = 3;
	public int amountOfHitsTaken = 0;
	public bool knockedDown;
	public float knockedDownTime = 2f;
	public bool receivingDamage;
	public GameObject shadow;
	public Transform shadowReference;
	public GameObject splashEffect;
	private GameObject currentSplashEffect;

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

	[SerializeField]
	private Transform shipBackRailReference;
	[SerializeField]
	private Transform shipBackRailFloorReference;

	public int shipHeight;

	public enum DeathType{
		Flying,
		Normal,
		BackOfShip,
		FrontOfShip
	}

	public DeathType deathType; 

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

		Destroy (gameObject.transform.parent.parent.transform.gameObject);
	}

	virtual public void CheckDeath(){


		if (life <= 0 && !dying)
        {
        	if(arenaEnemy){
				ArenaManager.instance.currentActiveArena.IncreaseAmountOfEnemiesDead();
				GameManager.instance.RemoveFromEnemyNearCatList(this.gameObject);
        	}

			invulnerable = true;
        	float random = Random.value;

			dying = true;

        	if(random <= 0.3){

        		deathType = DeathType.BackOfShip;
				knockedDown = true;
				myAnimator.SetBool("damage",true);
				myAnimator.SetBool("knockedDown", true);
				GetComponentInParent<Rigidbody2D>().velocity = Vector2.zero;

				shipBackRailReference = LevelReferenceManager.instance.GetShipBackRailReference(shipHeight);
				shipBackRailFloorReference = LevelReferenceManager.instance.GetShipBackRailFloorReference(shipHeight);

				DropItem ();
				if (mySpawner != null) {
					mySpawner.SetDeadEnemy (this.gameObject.GetInstanceID ());
				}

				if(sourceOfDamagePosition.x > transform.position.x){ //Enemy is on the right

					GetComponentInParent<Rigidbody2D>().velocity = new Vector2( -3f , GetComponentInParent<Rigidbody2D>().velocity.y + 10);
			
				}else{

					GetComponentInParent<Rigidbody2D>().velocity = new Vector2( 3f , GetComponentInParent<Rigidbody2D>().velocity.y + 10);	
			
				}

				Destroy(gameObject.transform.parent.parent.transform.gameObject, 6f);

        	}else if(random > 0.3 && random <= 0.6){
				deathType = DeathType.Flying;
				knockedDown = true;
				myAnimator.SetBool("damage",true);
				myAnimator.SetBool("knockedDown", true);

				DropItem ();
				if (mySpawner != null) {
					mySpawner.SetDeadEnemy (this.gameObject.GetInstanceID ());
				}

				GetComponentInParent<Rigidbody2D>().velocity = Vector2.zero;
				shadow.GetComponent<SpriteRenderer>().enabled = false;

				if(sourceOfDamagePosition.x > transform.position.x){ //Enemy is on the right

					GetComponentInParent<Rigidbody2D>().velocity = new Vector2( -8f , GetComponentInParent<Rigidbody2D>().velocity.y + 20);
			
				}else{

					GetComponentInParent<Rigidbody2D>().velocity = new Vector2( 8f , GetComponentInParent<Rigidbody2D>().velocity.y + 20);	
			
				}

				Destroy(gameObject.transform.parent.parent.transform.gameObject, 2f);

        	}else{
				deathType = DeathType.Normal;
				myAnimator.SetBool("dead", true);
	            myAnimator.SetBool("idle", false);
	            myAnimator.SetBool("attacking", false);
	            myAnimator.SetBool("walking", false);
				PlayDeathSound();
	            Destroy(GetComponent<Rigidbody2D>());
	            Destroy(GetComponent<Collider2D>());
        	}

        }

        if(dying){
        	if(deathType == DeathType.BackOfShip){
				shadow.transform.position += Vector3.up * 1.5f * Time.deltaTime;

				if(transform.position.y > shipBackRailReference.position.y){
					mySpriteRenderer.sortingOrder = -18;
				}

				if(shadow.transform.position.y > shipBackRailFloorReference.position.y){
					shadow.GetComponent<SpriteRenderer>().enabled = false;
				}

				if(mySpriteRenderer.sortingOrder == -18 && transform.position.y < shipBackRailFloorReference.position.y && currentSplashEffect == null){
					currentSplashEffect = Instantiate(splashEffect, transform.position, Quaternion.identity);
				}
        	}

        }
	}

}
