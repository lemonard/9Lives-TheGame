using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardDog : Enemy {

    public Transform rightFiringPoint;
    public Transform leftFiringPoint;
    public GameObject projectile;
    public GameObject shield;

    public float walkingRange;
    public float minimumRange;
    public float attackingRange;
   
    private bool isWalking;
    private bool doneOnce = false;
    private bool shieldActive = false;

    public float distanceToPlayer;

    private Cat player;

    // Use this for initialization
    void Start () {
        player = FindObjectOfType<Cat>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
        lookingRight = false;
        canReceiveDamage = false;
        freakoutManager = FindObjectOfType<FreakoutManager>();
    }
	
	// Update is called once per frame
	void Update () {
        distanceToPlayer = Mathf.Abs(Vector3.Distance(player.transform.position, transform.position));



        if (!dying)
        {

            if (!attacking)
            {

                DefineDirectionToLook();

                if (minimumRange < distanceToPlayer && distanceToPlayer <= walkingRange && !attacking)
                {
                    myAnimator.SetBool("attacking", false);
                    doneOnce = false;
                    Walk();
                }
                else if (distanceToPlayer <= attackingRange && !attacking && !isWalking)
                {
                   
                    if (Random.value < 0.3 && !attacking && !shieldActive)
                    {
                        CreateShield();
                        StartCoroutine(shieldCooldown());
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
        
    }

    void DefineDirectionToLook()
    {
        if (player.transform.position.x > transform.position.x)
        {
            lookingRight = true;
            mySpriteRenderer.flipX = true;
        }
        else
        {
            lookingRight = false;
            mySpriteRenderer.flipX = false;
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

        /*if (doneOnce == false)
        {
            FireProjectile();// this will function differently when there is an animation to tie it to.
            doneOnce = true;
        }*/

    }

    public void FireProjectile()
    {
        // this will function differently when there is an animation to tie it to.
        if (lookingRight)
        {
            Instantiate(projectile, rightFiringPoint.position, Quaternion.identity);
        }
        else
        {
            Instantiate(projectile, leftFiringPoint.position, Quaternion.identity);
        }
        
    }


    void FinishAttacking()
    {

        attacking = false;

        doneOnce = false;
        myAnimator.SetBool("attacking", false);
        Walk();
    }


    void CreateShield()
    {
        myAnimator.SetBool("attacking", false);
        myAnimator.SetBool("shieldspell", true);
        shieldActive = true;
        GameObject newShield = Instantiate(shield, transform.position, Quaternion.identity);
        newShield.transform.SetParent(this.gameObject.GetComponent<Transform>());
    }

    //ends shield animation, attached in animation
    void stopBarking()
    {
        myAnimator.SetBool("shieldspell", false);
        Walk();
    }

    void OnBecameVisible()
    {
        freakoutManager.AddEnemie(this.gameObject);
    }

    void OnBecameInvisible()
    {
        freakoutManager.RemoveEnemie(this.gameObject);
    }


    IEnumerator shieldCooldown()
    {
        yield return new WaitForSeconds(3);
        shieldActive = false;
    }

}