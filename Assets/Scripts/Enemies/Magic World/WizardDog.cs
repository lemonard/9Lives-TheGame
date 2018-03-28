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

    public bool lookingRight;

    private bool isWalking;
    private bool doneOnce = false;

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
                   
                    if (Random.value > 0.7 && !attacking)
                    {
                        CreateShield();
                    }
                    else
                    {
                        StartAttacking();
                    }
                }
                /*else if (distanceToPlayer >= attackingRange)
                {
                    Debug.Log("This");
                    FinishAttacking();
                }*/
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

    }



    void Idle()
    {
        myAnimator.SetBool("idle", true);

        myAnimator.SetBool("attacking", false);
        myAnimator.SetBool("walking", false);
    }


    void StartAttacking()
    {
        attacking = true;
        //myAnimator.SetBool("attacking", true);
        myAnimator.SetBool("walking", false);
        myAnimator.SetBool("idle", false);


        if (doneOnce == false)
        {
            FireProjectile();// this will function differently when there is an animation to tie it to.
            doneOnce = true;
        }
        Debug.Log("Happening");
        attacking = false;
        Walk();
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
        //myAnimator.SetBool("attacking", false);
        Walk();
    }


    void CreateShield()
    {
       GameObject newShield = Instantiate(shield, transform.position, Quaternion.identity);
       newShield.transform.SetParent(this.gameObject.GetComponent<Transform>());
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