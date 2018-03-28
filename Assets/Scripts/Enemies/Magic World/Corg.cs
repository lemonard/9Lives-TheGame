using UnityEngine;
using System.Collections;

public class Corg : Enemy {

	//Limits for the movement of the enemy 
	public Transform startThreshold; 
	public Transform EndThreshold;

	public float stunTime;

	public bool movementIsHorizontal; //Defines if the corg moves on the horizontal or vertical axis


	private float stunnedTimestamp;
	// Use this for initialization
	void Start () {
		stunnedTimestamp = 0;
		mySpriteRenderer = GetComponent<SpriteRenderer>();
		lookingRight = true;
		freakoutManager = FindObjectOfType<FreakoutManager>();
		myAnimator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

		if(!prepareForParry && !stunned && !dying){
			if(movementIsHorizontal){
				RoamX();
			} else {
				RoamY();
			}
		}

		if(stunned && (Time.time > stunnedTimestamp)){
			stunned = false;
		}

		//Death
		CheckDeath();

		//Toggle invulnerability off
		if (invulnerableTimeStamp < Time.time) {
			invulnerable = false;
			mySpriteRenderer.enabled = true;
		}

		//Flash
		if (invulnerable) {
			Flash ();
			lifeWhileInvulnerable = life;
		}

		//Take Damage
		CheckDamageReceived();
	}

	//Moves between the 2 limits on the x axis
	void RoamX(){

		//Turn left if passed right limit
		if(transform.position.x >= startThreshold.position.x){
			lookingRight = false;
			mySpriteRenderer.flipX = true;
		}

		//Turn right if passed left limit
		if(transform.position.x <= EndThreshold.position.x){
			lookingRight = true;
			mySpriteRenderer.flipX = false;
		}

		//Move right if is looking right
		if(lookingRight){
			GetComponent<Rigidbody2D>().transform.position += Vector3.right * speed * Time.deltaTime;
		} else { // Move left if is looking left
			GetComponent<Rigidbody2D>().transform.position += Vector3.left * speed * Time.deltaTime;
		}
		
	}

	//Moves between the 2 limits on the y axis
	void RoamY(){

		//Turn left if passed right limit
		if(transform.position.y >= startThreshold.position.y){
			lookingRight = false;
			mySpriteRenderer.flipX = true;
		}

		//Turn right if passed left limit
		if(transform.position.y <= EndThreshold.position.y){
			lookingRight = true;
			mySpriteRenderer.flipX = false;
		}

		//Move right if is looking right
		if(lookingRight){
			GetComponent<Rigidbody2D>().transform.position += Vector3.up * speed * Time.deltaTime;
		} else { // Move left if is looking left
			GetComponent<Rigidbody2D>().transform.position += Vector3.down * speed * Time.deltaTime;
		}
		
	}

	void OnTriggerEnter2D(Collider2D other){

		//Do damage to player that touches
		if(other.gameObject.tag == "Player"){

			Cat playerVariables = other.gameObject.GetComponent<Cat>();
            Health healthScript = other.gameObject.GetComponent<Health>();

			if(!playerVariables.invulnerable){
				playerVariables.life -= 1;
                healthScript.damage = true;
				playerVariables.receivedDamage = true;
			} 
		}
	}

	public override void ReceiveParry ()
	{
		base.ReceiveParry();
		stunnedTimestamp = Time.time + stunTime;
		//print("Recebi o parry e sou um Corg");

	}

	void OnBecameVisible(){
		freakoutManager.AddEnemie(transform.parent.gameObject);
	}

	void OnBecameInvisible(){
		freakoutManager.RemoveEnemie(transform.parent.gameObject);
	}

	protected void CheckDamageReceived()
	{
		if (receivedDamage && life > 0) {
			myAnimator.SetBool("damage",true);
			ToggleInvinsibility ();
		}
	}

	protected void DamageAnimationFinished()
	{
		myAnimator.SetBool("damage",false);
	}

	protected void CheckDeath()
	{
		if (life <= 0 && !dying) {
			myAnimator.SetBool("dying",true);
			//			source.PlayOneShot (dieSound, 1.0f);
			dying = true;
			Destroy (GetComponent<Rigidbody2D> ());
			Destroy (GetComponent<CircleCollider2D> ());

		}
	}
}
