using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskedDog : Enemy {

	public Collider2D rightMaskCollider;
	public Collider2D leftMaskCollider;

	public float stunTime;

    private float stunnedTimestamp;

	private bool waiting;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<Cat>();
		canReceiveDamage = true;
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
        freakoutManager = FindObjectOfType<FreakoutManager>();
		lastPositionX = transform.position.x;
	}
	
	void Update () {

		if(!player.isDying){
			transform.position = new Vector3(lastPositionX,transform.position.y,transform.position.z);

			if (!dying)
	        {

	            if (!attacking && !stunned)
	            {

	                DefineDirectionToLook();
	                DefineMaskCollider();

	                if (playerInWalkingRange && !playerInAttackingRange)
	                {
	                    //Walk();
	                }
					else if (playerInWalkingRange && playerInAttackingRange)
	                {
	                   
	                }
	                else
	                {
	                    Idle();
	                }

	            }

				if(waiting){
					Idle();
				}

	       }
        }else{
			Idle();
        }

        if(wasTurned){
        	wasTurned = false;
        	StartStun();
        }

		if (stunned && (Time.time > stunnedTimestamp))
        {
            FinishStun();
        }

		if (life <= 0 && !dying)
        {
        	if(stunned){
        		if(lookingRight){
        			mySpriteRenderer.flipX = true;
        		}else{
					mySpriteRenderer.flipX = false;
        		}
        	}
            myAnimator.SetBool("dead", true);
            myAnimator.SetBool("idle", false);
            myAnimator.SetBool("attacking", false);
            myAnimator.SetBool("walking", false);
			PlayDeathSound();
            dying = true;
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<CircleCollider2D>());

        }

        //Toggle invulnerability off
        if (invulnerableTimeStamp < Time.time)
        {
            invulnerable = false;
            mySpriteRenderer.enabled = true;
        }

        //Flash
        if (invulnerable)
        {
            Flash();
        }

        //Take Damage
        if (receivedDamage && life > 0)
        {
            ToggleInvinsibility();
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

    void DefineMaskCollider(){
    	if(lookingRight){
    		rightMaskCollider.enabled = true;
    		leftMaskCollider.enabled = false;
    	}else{
			rightMaskCollider.enabled = false;
    		leftMaskCollider.enabled = true;
    	}
    }

    void StartStun(){
		stunnedTimestamp = Time.time + stunTime;
		stunned = true;
		myAnimator.SetBool("stunned", true);
    }


	void FinishStun()
    {
        stunned = false;
        myAnimator.SetBool("stunned", false);
        myAnimator.SetBool("idle", true);
        //transform.rotation = Quaternion.identity;
		attacking = false;

    }

	IEnumerator StartWaiting(){

    	waiting = true;
    	yield return new WaitForSeconds(2f);
    	waiting = false;

    }
}
