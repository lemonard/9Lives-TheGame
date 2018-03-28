using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrawlerDog : Enemy {

	public float stunTime;
    private float stunnedTimestamp;

	public Collider2D leftAttackCollider;
    public Collider2D rightAttackCollider;

	public Cat player;

	public bool playerInWalkingRange;
	public bool playerInAttackingRange;

	private bool waiting;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<Cat>();
		canReceiveDamage = true;
        stunnedTimestamp = 0;
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
        freakoutManager = FindObjectOfType<FreakoutManager>();
	}
	
	// Update is called once per frame
	void Update () {

		if (!dying)
        {

            if (!stunned && !prepareForParry && !attacking && !waiting)
            {

                DefineDirectionToLook();

                if (playerInWalkingRange && !playerInAttackingRange)
                {
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

				if(waiting){
					Idle();
				}

            }

			

        }

        if (stunned && (Time.time > stunnedTimestamp))
        {
            FinishStun();
        }

		if (life <= 0 && !dying)
        {
            //			source.PlayOneShot (dieSound, 1.0f);
            myAnimator.SetBool("dead", true);
            myAnimator.SetBool("idle", false);
            myAnimator.SetBool("attacking", false);
            myAnimator.SetBool("walking", false);
           
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

        if (prepareForParry)
        {
            PrepareForParry();
        }
	}

	void FixedUpdate(){

		if(!stunned && !prepareForParry && attacking && !waiting){
				MoveWhileAttacking();
		}
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

	void DefineDirectionToLook()
    {
        if (player.transform.position.x > transform.position.x)
        {
            lookingRight = true;
            mySpriteRenderer.flipX = false;
        }
        else
        {
            lookingRight = false;
            mySpriteRenderer.flipX = true;
        }
    }

	void PrepareForParry()
    {

        prepareForParry = false;
        attacking = false;
        stunned = true;
		stunnedTimestamp = Time.time + stunTime;
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

	void Idle()
    {
        myAnimator.SetBool("idle", true);
        myAnimator.SetBool("attacking", false);
        myAnimator.SetBool("walking", false);
        attacking = false;

    }

    void StartAttacking()
    {

        attacking = true;
        myAnimator.SetBool("attacking", true);
        myAnimator.SetBool("walking", false);
        myAnimator.SetBool("idle", false);


    }

    void MoveWhileAttacking(){

    	print("Being called and attacking is: " + attacking);
		print("And stunned is: " + stunned);

		if (lookingRight)
        {
            GetComponent<Rigidbody2D>().transform.position += Vector3.right * speed * Time.deltaTime;
        }
        else
        { // Move left if is looking left
            GetComponent<Rigidbody2D>().transform.position += Vector3.left * speed * Time.deltaTime;
        }
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

    void FinishAttacking()
    {

        attacking = false;
        rightAttackCollider.enabled = false;
        leftAttackCollider.enabled = false;
        myAnimator.SetBool("attacking", false);
        Idle();
		StartCoroutine(StartWaiting());
    }

	void FinishStun()
    {
        stunned = false;
        myAnimator.SetBool("stunned", false);
        myAnimator.SetBool("idle", true);
		attacking = false;

    }

    void OnBecameVisible()
    {
        freakoutManager.AddEnemie(this.gameObject);
    }

    void OnBecameInvisible()
    {
        freakoutManager.RemoveEnemie(this.gameObject);
    }

    IEnumerator StartWaiting(){

    	waiting = true;
    	yield return new WaitForSeconds(2f);
    	waiting = false;

    }

}
