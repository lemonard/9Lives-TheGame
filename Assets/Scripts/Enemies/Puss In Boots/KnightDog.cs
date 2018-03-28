using UnityEngine;
using System.Collections;

public class KnightDog : Enemy {

	public float walkingRange;
	public float defenseRange;
	public float attackingRange;

	public Collider2D leftAttackCollider;
	public Collider2D rightAttackCollider;

	public float stunTime;

	private float stunnedTimestamp;

	public bool isDefending;
	private bool isWalking;

	public float distanceToPlayer;

	private Cat player;


	// Use this for initialization
	void Start () {
		player = FindObjectOfType<Cat>();
		stunnedTimestamp = 0;
		mySpriteRenderer = GetComponent<SpriteRenderer>();
		myAnimator = GetComponent<Animator>();
		lookingRight = true;
		isDefending = false;
		canReceiveDamage = false;
        freakoutManager = FindObjectOfType<FreakoutManager>();
    }

 

    // Update is called once per frame
    void Update () {

		distanceToPlayer = Mathf.Abs(Vector3.Distance(player.transform.position, transform.position));

        if (!dying){

			if(!stunned && !prepareForParry && !attacking){ 

				DefineDirectionToLook();

                if (defenseRange < distanceToPlayer && distanceToPlayer <= walkingRange && !attacking)
                {
                    myAnimator.SetBool("attacking", false);
					isDefending = false;
                    Walk();
                }
				else if (attackingRange < distanceToPlayer && distanceToPlayer <= defenseRange)
				{
					Defend();
				}
                else if (distanceToPlayer <= attackingRange && !attacking && !isWalking && isDefending)
                {
					StartAttacking();
				}
                else if (distanceToPlayer >= attackingRange)
                {
                    FinishAttacking();
                }
                else
                {
					Idle();
				}

			}

		}

		if(stunned && (Time.time > stunnedTimestamp)){
			FinishStun();
		}

		//Death
		if (life <= 0 && !dying) {

//			source.PlayOneShot (dieSound, 1.0f);
			myAnimator.SetBool ("dead", true);

			myAnimator.SetBool("idle",false);
			myAnimator.SetBool("attacking",false);
			myAnimator.SetBool("walking",false);
			myAnimator.SetBool("prepareDefense",false);
			myAnimator.SetBool("defending",false);
			 

			dying = true;
 			Destroy (GetComponent<Rigidbody2D> ());
 			Destroy (GetComponent<CircleCollider2D> ());

		}

		//Toggle invulnerability off
		if (invulnerableTimeStamp < Time.time) {
			invulnerable = false;
			mySpriteRenderer.enabled = true;
		}

		//Flash
		if (invulnerable) {
			Flash ();
			 
		}

		//Take Damage
		if (receivedDamage && life > 0) {
			ToggleInvinsibility ();
		}

		if(prepareForParry){
			PrepareForParry();
		}
	}

	void DefineDirectionToLook(){
		if(player.transform.position.x > transform.position.x){
			lookingRight = true;
			mySpriteRenderer.flipX = false;
		} else {
			lookingRight = false;
			mySpriteRenderer.flipX = true;
		}
	}

	void Walk(){

		isDefending = false;

		if(lookingRight){
			GetComponent<Rigidbody2D>().transform.position += Vector3.right * speed * Time.deltaTime;
		} else { // Move left if is looking left
			GetComponent<Rigidbody2D>().transform.position += Vector3.left * speed * Time.deltaTime;
		}
		myAnimator.SetBool("idle",false);
		myAnimator.SetBool("walking",true);
		myAnimator.SetBool("defending",false);
		myAnimator.SetBool("prepareDefense",false);
	}

	void Defend(){

		myAnimator.SetBool("walking",false);

		if(!isDefending){
			StartDefending();
		}
	}

	void Idle(){
		myAnimator.SetBool("idle",true);

		myAnimator.SetBool("attacking",false);
		myAnimator.SetBool("walking",false);
		myAnimator.SetBool("prepareDefense",false);
		myAnimator.SetBool("defending",false);
	}

	void StartDefending(){

		myAnimator.SetBool("prepareDefense",true);
		myAnimator.SetBool("walking",false);

		isDefending = true;
	}

	public void DefenseStance(){

		myAnimator.SetBool("prepareDefense",false);
		myAnimator.SetBool("defending",true);
	}

	void StartAttacking(){

		isDefending = false;
		attacking = true;
		myAnimator.SetBool("attacking",true);
		myAnimator.SetBool("defending",false);
        myAnimator.SetBool("walking", false);

    }

	public void ActivateAttackCollider(){

		if(lookingRight){
			rightAttackCollider.enabled = true;
		} else {
			leftAttackCollider.enabled = true;
		}
	}

	void FinishAttacking(){

		attacking = false;
		rightAttackCollider.enabled = false;
		leftAttackCollider.enabled = false;
		myAnimator.SetBool("attacking",false);
		StartDefending();
	}

	void PrepareForParry(){
		prepareForParry = false;
		attacking = false;
		stunned = true;
		myAnimator.SetBool("parried",true);

		rightAttackCollider.enabled = false;
		leftAttackCollider.enabled = false;

	}

	public override void ReceiveParry ()
	{
		base.ReceiveParry();
		stunnedTimestamp = Time.time + stunTime;
		myAnimator.SetBool("parried",false);
		myAnimator.SetBool("stunned",true);

		myAnimator.SetBool("idle",false);
		myAnimator.SetBool("attacking",false);
		myAnimator.SetBool("walking",false);
		myAnimator.SetBool("prepareDefense",false);
		myAnimator.SetBool("defending",false);

		canReceiveDamage = true;
		attacking = false;

	}

	void FinishStun(){
		stunned = false;
		myAnimator.SetBool("stunned",false);
		myAnimator.SetBool("idle",true);
		canReceiveDamage = false;
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
}
