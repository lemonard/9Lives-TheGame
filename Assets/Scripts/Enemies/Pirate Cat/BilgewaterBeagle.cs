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

   	private bool playerIsPirate;
   	private PirateCat pirateCat;
   	private bool pirateIsKnockedDown;

    // Use this for initialization
    void Start()
    {
        player = FindObjectOfType<Cat>();
        stunnedTimestamp = 0;
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
        lookingRight = true;
        canReceiveDamage = true;
        freakoutManager = FindObjectOfType<FreakoutManager>();
		lastPositionX = transform.position.x;

		if(player.GetComponent<PirateCat>()){
			playerIsPirate = true;
			pirateCat = player.GetComponent<PirateCat>();
		}
    }

    // Update is called once per frame
    void Update()
    {

    	if(playerIsPirate){
    		pirateIsKnockedDown = pirateCat.knockedDown;
    	}

		if(!player.isDying){
			
			transform.position = new Vector3(lastPositionX,transform.position.y,transform.position.z);

	        if (!dying)
	        {

	            if (!stunned && !prepareForParry && !attacking && !knockedDown && !waiting && !receivingDamage && !pirateIsKnockedDown)
	            {

	                DefineDirectionToLook();

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

	            }

	            if(knockedDown){
					if(knockedDownTimeStamp < Time.time && knockedDownTimeStamp != 0){
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

		lastPositionX = transform.position.x;
    }

    void Walk()
    {
        
        if (lookingRight)
        {
            GetComponent<Rigidbody2D>().transform.position += Vector3.right * speed * Time.deltaTime;
        }
        else
        { // Move left if is looking left
            GetComponent<Rigidbody2D>().transform.position += Vector3.left * speed * Time.deltaTime;
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
