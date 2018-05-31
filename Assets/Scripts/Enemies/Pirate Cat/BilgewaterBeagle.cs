using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BilgewaterBeagle : CombableEnemy {

	public float walkingRange;
    public float attackingRange;

    public Collider2D leftAttackCollider;
    public Collider2D rightAttackCollider;

    public float stunTime;

    private float stunnedTimestamp;

    private bool isWalking;

   	public float chanceToUseThreeAttacks = 0.4f;

   	private float yMovementSpeed;

    // Use this for initialization
    void Start()
    {
		CombableEnemyInitialize();

		yMovementSpeed = speed / 3;

    }

    // Update is called once per frame
    void Update()
    {

    	if(playerIsPirate){
    		pirateIsKnockedDown = pirateCat.knockedDown;
    	}

		if(!player.isDying){

	        if (!dying)
	        {

	            if (!stunned && !prepareForParry && !attacking && !knockedDown && !waiting && !receivingDamage && !pirateIsKnockedDown)
	            {

	                DefineDirectionToLook();

	                if(!arenaEnemy){

						if (playerInWalkingRange && !playerInAttackingRange)
		                {
							
		                    myAnimator.SetBool("attacking", false);
		                    Walk();
		                }
						else if (playerInWalkingRange && playerInAttackingRange)
		                {
		                    StartAttacking();
		                }
		                else
		                {
		                    Idle();
		                }
	                }else{

	                	if(!arenaEntrance){

							if (!playerInAttackingRange && (GameManager.instance.enemiesNearCat.Count < GameManager.instance.maxAmountOfEnemiesNearCat))
			                {
			                    myAnimator.SetBool("attacking", false);
			                    Walk();
							}else if(!playerInAttackingRange && GameManager.instance.enemiesNearCat.IndexOf(this.gameObject) != -1){
								myAnimator.SetBool("attacking", false);
			                    Walk();
			                } else if(playerInAttackingRange)
			                {
			                    StartAttacking();
			                }else{
			                	//CheckEnemiesNearCatAmount();
			                	Idle();
			                }
		                }else{
		                	if(arenaEntranceSpotToTheRight){

		                		if(transform.position.x < arenaEntrancePoint.position.x){
									myAnimator.SetBool("attacking", false);
			                		Walk();
		                		}else{
		                			arenaEntrance = false;
		                		}
								
		                	}else{

								if(transform.position.x > arenaEntrancePoint.position.x){
									myAnimator.SetBool("attacking", false);
			                		Walk();
		                		}else{
		                			arenaEntrance = false;
		                		}
		                	}
							
		                }
	                }
	            }

	            if(knockedDown){
					if(knockedDownTimeStamp < Time.time && knockedDownTimeStamp != 0){
	            		StandUp();
	            	}

					if(myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Bilgewater Beagle Hitting Ground") && myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && knockedDownTimeStamp == 0){
						StandUp();
					}
	            }

				if(waiting){
					Idle();
				}


	        }
        }else{
			Idle();
        }

        if (stunned && (Time.time > stunnedTimestamp))
        {
            FinishStun();
        }

        //Death
        if (life <= 0 && !dying)
        {
        	if(arenaEnemy){
				ArenaManager.instance.currentActiveArena.IncreaseAmountOfEnemiesDead();
				GameManager.instance.RemoveFromEnemyNearCatList(this.gameObject);
        	}
            myAnimator.SetBool("dead", true);
            myAnimator.SetBool("idle", false);
            myAnimator.SetBool("attacking", false);
            myAnimator.SetBool("walking", false);
			PlayDeathSound();
            dying = true;
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<Collider2D>());

        }

        //Take Damage
        if (receivedDamage && life > 0)
        {
        	Reset();
            ReceiveCombo();
        }

        if (prepareForParry)
        {
            PrepareForParry();
        }

		if(!knockedDown){
			shadow.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
		}else{
			shadow.transform.position = new Vector3(transform.position.x, shadow.transform.position.y, transform.position.z);
		}

    }

    void Walk()
    {
        
        if (lookingRight)
        {
            GetComponentInParent<Rigidbody2D>().transform.position += Vector3.right * speed * Time.deltaTime;

        }
        else
        { // Move left if is looking left
			GetComponentInParent<Rigidbody2D>().transform.position += Vector3.left * speed * Time.deltaTime;

        }

        if(pirateCat != null){
        	if(!pirateCat.isJumping){
		        if(pirateCat.transform.position.y > transform.position.y){ //Move Up
					transform.position += Vector3.up * yMovementSpeed * Time.deltaTime;
					shadow.transform.position += Vector3.up * yMovementSpeed * Time.deltaTime;
		        }else{ //Move Down
					transform.position += Vector3.down * yMovementSpeed * Time.deltaTime;
					shadow.transform.position += Vector3.down * yMovementSpeed * Time.deltaTime;
		        }
	        }
        }
        myAnimator.SetBool("idle", false);
        myAnimator.SetBool("attacking", false);
        myAnimator.SetBool("walking", true);
        attacking = false;

    }


    void Idle()
    {
        myAnimator.SetBool("idle", true);
        myAnimator.SetBool("attacking", false);
        myAnimator.SetBool("walking", false);
        attacking = false;

    }

    void CheckEnemiesNearCatAmount(){
		if(GameManager.instance.enemiesNearCat.Count < GameManager.instance.maxAmountOfEnemiesNearCat && (GameManager.instance.enemiesNearCat.IndexOf(gameObject) == -1)){
			GameManager.instance.AddToEnemyNearCatList(gameObject);
    	}
    }

    void StartAttacking()
    {
    	
    	float random = Random.value;

    	if(random < chanceToUseThreeAttacks ){
			myAnimator.SetBool("strongerAttack", true);
    	}else{
			myAnimator.SetBool("attacking", true);
    	}
        attacking = true;
   
		
        myAnimator.SetBool("walking", false);
        myAnimator.SetBool("idle", false);
    }

    public void ActivateAttackCollider()
    {

        if (lookingRight)
        {
            rightAttackCollider.enabled = true;
        }
        else
        {
            leftAttackCollider.enabled = true;
        }
    }

	public void DeactivateAttackCollider()
    {

        if (lookingRight)
        {
            rightAttackCollider.enabled = false;
        }
        else
        {
            leftAttackCollider.enabled = false;
        }
    }

    void FinishAttacking()
    {

        attacking = false;
        rightAttackCollider.enabled = false;
        leftAttackCollider.enabled = false;
        myAnimator.SetBool("attacking", false);
		myAnimator.SetBool("strongerAttack", false);
        Idle();
        if(waitingCoroutine != null){
        	StopCoroutine(waitingCoroutine);
        }
		waitingCoroutine = StartCoroutine(StartWaiting());
    }

    void PrepareForParry()
    {
        prepareForParry = false;
        attacking = false;
        stunned = true;
        myAnimator.SetBool("parried", true);

		rightAttackCollider.enabled = false;
		leftAttackCollider.enabled = false;

    }

    public override void ReceiveParry()
    {
        base.ReceiveParry();
        stunnedTimestamp = Time.time + stunTime;
		myAnimator.SetBool("stunned", true);
        myAnimator.SetBool("parried", false);


        myAnimator.SetBool("idle", false);
        myAnimator.SetBool("attacking", false);
        myAnimator.SetBool("walking", false);


    }

    void FinishStun()
    {
        stunned = false;
        myAnimator.SetBool("stunned", false);
        myAnimator.SetBool("idle", true);
		attacking = false;

    }

    void Reset(){
		myAnimator.SetBool("idle", false);
        myAnimator.SetBool("attacking", false);
        myAnimator.SetBool("walking", false);
		myAnimator.SetBool("strongerAttack", false);
		myAnimator.SetBool("parried", false);
		myAnimator.SetBool("stunned", false);

		attacking = false;
		stunned = false;

    }

    void OnBecameVisible()
    {
        freakoutManager.AddEnemie(this.gameObject);
    }

    void OnBecameInvisible()
    {
        freakoutManager.RemoveEnemie(this.gameObject);
    }
}
