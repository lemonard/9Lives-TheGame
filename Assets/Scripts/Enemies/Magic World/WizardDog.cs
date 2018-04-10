using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardDog : Enemy {

    public Transform rightFiringPoint;
    public Transform leftFiringPoint;
    public GameObject projectile;
    public GameObject shield;
	public AudioClip shieldSummonSound;

    public float projectileCooldown;
    public float shieldCooldown;

    public float walkingRange;
    public float minimumRange;
    public float attackingRange;
   
    private bool isWalking;
    private bool doneOnce = false;
    private bool shieldActive = false;

    public float distanceToPlayer;

    private Cat player;
	private bool waiting;

    // Use this for initialization
    void Start () {
        player = FindObjectOfType<Cat>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
        lookingRight = false;
        canReceiveDamage = false;
        freakoutManager = FindObjectOfType<FreakoutManager>();
		lastPositionX = transform.position.x;
    }
	
	// Update is called once per frame
	void Update () {

		if(!player.isDying){
	        distanceToPlayer = Mathf.Abs(Vector3.Distance(player.transform.position, transform.position));
			transform.position = new Vector3(lastPositionX,transform.position.y,transform.position.z);

	        if (!dying)
	        {

	            if (!attacking && !waiting)
	            {

	                DefineDirectionToLook();

	                if (minimumRange < distanceToPlayer && distanceToPlayer <= walkingRange && !attacking)
	                {
	                    myAnimator.SetBool("attacking", false);
	                    Walk();
	                }
	                else if (distanceToPlayer <= attackingRange && !attacking && !isWalking)
	                {
	                   
	                    if (Random.value < 0.3 && !attacking && !shieldActive)
	                    {
	                        CreateShield();
	                    }
	                    else
	                    {
	                        StartAttacking();
	                    }
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
       }
       else
       {
		Idle();
       }


        //Death
        if (life <= 0 && !dying)
        {

            //			source.PlayOneShot (dieSound, 1.0f);
            myAnimator.SetBool("dead", true);

            myAnimator.SetBool("idle", false);
            myAnimator.SetBool("attacking", false);
            myAnimator.SetBool("walking", false);
            myAnimator.SetBool("shieldspell", false);
			PlayDeathSound();
            dying = true;
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<CircleCollider2D>());

        }

        //Toggle invulnerability off
        if (invulnerableTimeStamp < Time.time)
        {
            myAnimator.SetBool("damaged", false);
            invulnerable = false;
            mySpriteRenderer.enabled = true;
        }

        //Flash
        if (invulnerable)
        {
            Flash();
            myAnimator.SetBool("damaged", true);
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
        myAnimator.SetBool("walking", true);
        myAnimator.SetBool("damaged", false);
        //myAnimator.SetBool("shieldspell", false);
    }



    void Idle()
    {
        myAnimator.SetBool("idle", true);

        myAnimator.SetBool("attacking", false);
        myAnimator.SetBool("walking", false);
        myAnimator.SetBool("damaged", false);
        //myAnimator.SetBool("shieldspell", false);
    }


    void StartAttacking()
    {
        attacking = true;
        myAnimator.SetBool("attacking", true);
        myAnimator.SetBool("walking", false);
        myAnimator.SetBool("idle", false);
        myAnimator.SetBool("damaged", false);


    }

    public void FireProjectile()
    {

 
        if (lookingRight)
        {
            GameObject shot = (GameObject)Instantiate(projectile, rightFiringPoint.position, Quaternion.identity);
			shot.GetComponent<EnemyProjectile>().goRight = true;
        }
        else
        {
            Instantiate(projectile, leftFiringPoint.position, Quaternion.identity);
        }
        
    }

    void FinishAttacking()
    {

        attacking = false;
        myAnimator.SetBool("attacking", false);
        StartCoroutine(ProjectileCooldown());
     
    }


    void CreateShield()
    {
        myAnimator.SetBool("attacking", false);
        myAnimator.SetBool("shieldspell", true);
        shieldActive = true;
        GameObject newShield = Instantiate(shield, transform.position, Quaternion.identity);
        newShield.transform.SetParent(this.gameObject.GetComponent<Transform>());
        newShield.GetComponent<MagicShield>().shieldDuration = shieldCooldown;
        AudioManager.instance.PlaySound(shieldSummonSound);
		StartCoroutine(ShieldCooldown());
    }

    //ends shield animation, attached in animation
    void stopBarking()
    {
        myAnimator.SetBool("shieldspell", false);
    }

    void OnBecameVisible()
    {
        freakoutManager.AddEnemie(this.gameObject);
    }

    void OnBecameInvisible()
    {
        freakoutManager.RemoveEnemie(this.gameObject);
    }

	IEnumerator ProjectileCooldown(){

    	waiting = true;
		yield return new WaitForSeconds(projectileCooldown);
    	waiting = false;

    }

    IEnumerator ShieldCooldown()
    {
		waiting = true;
        yield return new WaitForSeconds(shieldCooldown);
		waiting = false;
        shieldActive = false;
    }

}