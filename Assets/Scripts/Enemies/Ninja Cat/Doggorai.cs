using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doggorai : LightSearchingEnemy {

	public GameObject searchingLight;
	public float waitTime;

	public bool running;
	bool waiting;

	float currentSpeed;

	public Collider2D rightAttackCollider;
	public Collider2D leftAttackCollider;

	public Transform rightThreshold; 
	public Transform leftThreshold;

	private bool attackDelay;
	// Use this for initialization
	void Start () {
		EnemyInitialize();
		currentSpeed = speed;
		StartRoaming();
	}
	
	// Update is called once per frame
	void Update () {
		if(playerInAttackingRange && !attacking && !dying && !attackDelay){
			StartAttack();
		}

		if (invulnerableTimeStamp < Time.time)
        {
            invulnerable = false;
            mySpriteRenderer.enabled = true;
        }


		if(life <= 0){
        	Die();
        }

		if (receivedDamage && life > 0)
        {
            ToggleInvinsibility();
        }
        //Flash
        if (invulnerable)
        {
            Flash();
        }

	}

	void FixedUpdate(){
		HandleMovement();
	}

	void HandleMovement(){

		if(!attacking && !dying && !waiting && !attackDelay){
			if(!alert){
				Roam();
			}else{
				if(running){
					Run();
				}
			}
		}
	}

	void StartWaiting(){
		myAnimator.Play("Idle");
	}

	void StartRoaming(){
		myAnimator.Play("Walk");
	}

	void Roam(){


		//Move right if is looking right
		if(lookingRight){
			GetComponent<Rigidbody2D>().transform.position += Vector3.right * speed * Time.deltaTime;
		} else { // Move left if is looking left
			GetComponent<Rigidbody2D>().transform.position += Vector3.left * speed * Time.deltaTime;
		}

		//Turn left if passed right limit
		if(transform.position.x >= rightThreshold.position.x){
			StartCoroutine(Wait());

		}

		//Turn right if passed left limit
		if(transform.position.x <= leftThreshold.position.x){
			StartCoroutine(Wait());

		}


		
	}

	void Run(){


		if (target.transform.position.x > transform.position.x)
        {
            lookingRight = true;
            mySpriteRenderer.flipX = true;
			searchingLight.transform.localScale = new Vector3(-1,1,1);
        }
        else
        {
            lookingRight = false;
            mySpriteRenderer.flipX = false;
			searchingLight.transform.localScale = new Vector3(1,1,1);
        }

		if(lookingRight){
			GetComponent<Rigidbody2D>().transform.position += Vector3.right * runningSpeed * Time.deltaTime;
		} else { 
			GetComponent<Rigidbody2D>().transform.position += Vector3.left * runningSpeed * Time.deltaTime;
		}
	}

	void ChangeRoamingDirection(){

		if(lookingRight){
			lookingRight = false;
			mySpriteRenderer.flipX = false;
			searchingLight.transform.localScale = new Vector3(1,1,1);
		}else{
			lookingRight = true;
			mySpriteRenderer.flipX = true;
			searchingLight.transform.localScale = new Vector3(-1,1,1);
		}

	}



	public override void TargetSpotted (GameObject target)
	{

		alert = true;
		this.target = target;
		myAnimator.Play("Alert");
	}


	void AlertAnimationEnded(){
		myAnimator.Play("Run");
		running = true;
	}

	void StartAttack(){
		Debug.Log("Attack animation");
		attacking = true;
		myAnimator.Play("Attack");
	}

	void ActivateAttackCollider()
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

	void FinishAttack(){
		StartCoroutine(AttackDelay());
		attacking = false;

	}

	void Die(){


		PlayDeathSound();
        dying = true;
        Destroy(GetComponent<Collider2D>());
	}

	void OnBecameVisible()
    {
        freakoutManager.AddEnemie(this.gameObject);
    }

    void OnBecameInvisible()
    {
        freakoutManager.RemoveEnemie(this.gameObject);
    }


	IEnumerator Wait(){

		waiting = true;
		myAnimator.Play("Idle");
		yield return new WaitForSeconds(waitTime);
		ChangeRoamingDirection();
		StartRoaming();
		waiting = false;
	}

	IEnumerator AttackDelay(){

		attackDelay = true;
		myAnimator.Play("Idle");
		yield return new WaitForSeconds(2f);
		myAnimator.Play("Run");
		attackDelay = false;
	}
}
