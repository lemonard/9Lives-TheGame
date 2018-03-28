using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantDog : Enemy
{

    public float walkingRange;
    public float attackingRange;

    public Collider2D leftAttackCollider;
    public Collider2D rightAttackCollider;

    public float stunTime;

    private bool lookingRight;

    private float stunnedTimestamp;

    private bool isWalking;

    public float distanceToPlayer;

    private Cat player;


    // Use this for initialization
    void Start()
    {
        player = FindObjectOfType<Cat>();
        stunnedTimestamp = 0;
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
        lookingRight = true;
        canReceiveDamage = false;
        freakoutManager = FindObjectOfType<FreakoutManager>();
    }

    // Update is called once per frame
    void Update()
    {

        distanceToPlayer = Mathf.Abs(Vector3.Distance(player.transform.position, transform.position));

        if (!dying)
        {

            if (!stunned && !prepareForParry && !attacking)
            {

                DefineDirectionToLook();

                if (attackingRange < distanceToPlayer && distanceToPlayer <= walkingRange)
                {
					
                    myAnimator.SetBool("attacking", false);
                    Walk();
                }
                else if (distanceToPlayer <= attackingRange && !attacking && !isWalking)
                {
                    StartAttacking();
                }
                else
                {
                    Idle();
                }

            }

        }

        if (stunned && (Time.time > stunnedTimestamp))
        {
            FinishStun();
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

        if (prepareForParry)
        {
            PrepareForParry();
        }
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

        attacking = true;
        myAnimator.SetBool("attacking", true);
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

    void FinishAttacking()
    {

        attacking = false;
        rightAttackCollider.enabled = false;
        leftAttackCollider.enabled = false;
        myAnimator.SetBool("attacking", false);
        Idle();
    }

    void PrepareForParry()
    {
        prepareForParry = false;
        attacking = false;
        stunned = true;
        myAnimator.SetBool("parried", true);

    }

    public override void ReceiveParry()
    {
        base.ReceiveParry();
        stunnedTimestamp = Time.time + stunTime;
        myAnimator.SetBool("parried", false);
        myAnimator.SetBool("stunned", true);

        myAnimator.SetBool("idle", false);
        myAnimator.SetBool("attacking", false);
        myAnimator.SetBool("walking", false);


        rightAttackCollider.enabled = false;
        leftAttackCollider.enabled = false;
        canReceiveDamage = true;

    }

    void FinishStun()
    {
        stunned = false;
        myAnimator.SetBool("stunned", false);
        myAnimator.SetBool("idle", true);
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
