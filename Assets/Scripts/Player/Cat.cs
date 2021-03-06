﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Cat : MonoBehaviour {


	public float speed;

	public float jumpForce;
	[SerializeField]
	protected float fallGravityMultiplier = 2.5f;
	[SerializeField]
	protected float lowJumpGravityMultiplier = 2f;

	public int life = 3;
	//Invulnerability variables
	public float invulnerableSeconds = 1;

	private int flashDelay = 2;
	protected int flashingCounter;
	protected bool toggleFlashing = false;
	protected float invulnerableTimeStamp;

	//Components
	public Animator animator;
	protected Rigidbody2D myRigidBody2D;
	protected SpriteRenderer mySpriteRenderer;
	protected AudioSource myAudioSource;
	protected MovementSoundManager movementSoundManager;

	//States
	public bool isLookingRight;
	public bool isDying;
	public bool receivedDamage;
	public bool bounceBackAfterReceivingDamage;
	public bool invulnerable;
    public bool freakoutMode;
	public bool FourLeggedCat;
	public bool controlersDisabled;

	public KeyCode moveRightKey;
	public KeyCode moveLeftKey;
	public KeyCode jumpKey;
    public KeyCode downKey;
	public KeyCode transformInHubKey;
	public KeyCode freakoutKey;

	public string moveHorizontalGamepadAxis;
	public string moveVerticalGamepadAxis;
	public string jumpGamepadButton;
	public string transformInHubGamepadButton;
	public string freakoutGamepadButton;


	public bool isJumping;
	protected bool isWalking;
	public bool isFalling;
	public bool isAttacking;
	public bool isSliding;

	public bool justJumped;

//	public LayerMask groundLayer;
    public bool isOnGround;

	public bool finishedLevel;

	public bool isNearStatue;
	public CatStatue nearestStatue;

	public FreakoutManager freakoutManager;
	public Transform startingPoint;

    public bool ready;                                          // Checks if the freak out bar is ready.

    private LayerMask mask = 1<<8;

	public int currencyAmount = 0;

	public Vector3 currentCheckpoint;

	public Vector3 sourceOfDamagePosition;

	protected bool movedLeft;
	protected bool movedRight;

    // Use this for initialization
    protected virtual void Start () {

    	//jumpTimeCounter = jumpTime;
		animator = GetComponent<Animator>();
		mySpriteRenderer = GetComponent<SpriteRenderer>();
		myRigidBody2D = GetComponent<Rigidbody2D>();
		myRigidBody2D.velocity = new Vector2(myRigidBody2D.velocity.x,0);
		myAudioSource = GetComponent<AudioSource> ();
		currentCheckpoint = startingPoint.position;
		movementSoundManager = GetComponentInChildren<MovementSoundManager>();
	}

    private void FixedUpdate()
    {
        CheckSlope();
    }

	protected virtual void MoveRight(){

		animator.SetBool("walking",true);
		animator.SetBool("idle",false);
		isWalking = true;
		isLookingRight = true;

		myRigidBody2D.transform.position += Vector3.right * speed * Time.deltaTime;
		ChangeLookingDirection();

		//movementSoundManager.StartStepsSound();
	}

	protected virtual void MoveLeft(){

		animator.SetBool("walking",true);
		animator.SetBool("idle",false);
		isWalking = true;
		isLookingRight = false; 


		myRigidBody2D.transform.position += Vector3.left * speed * Time.deltaTime;
		ChangeLookingDirection();

		//movementSoundManager.StartStepsSound();
	}
	protected virtual void MoveUp()
    {
        myRigidBody2D.transform.position += Vector3.up * speed * Time.deltaTime;

		
    }
	protected virtual void MoveDown()
    {
        myRigidBody2D.transform.position += Vector3.down * speed * Time.deltaTime;

    }

    protected virtual void Idle(){

		animator.SetBool("walking",false);
		animator.SetBool("idle",true);
		isWalking = false;
		//movementSoundManager.StopStepsSound();
	}

	protected virtual void Jump(){

		justJumped = true;
		animator.SetBool("jumping",true);
		StartCoroutine(SetJustJumpedOff());

		myRigidBody2D.velocity = new Vector3(myRigidBody2D.velocity.x,0,0);

		myRigidBody2D.AddForce(new Vector3(0, jumpForce,0), ForceMode2D.Impulse);

		isJumping = true;
		isSliding = false;

		if(animator.GetBool("sliding")){
			animator.SetBool("sliding",false);
		}

//		movementSoundManager.PlayJumpingSound();
	}


	public void Flash(){

		if(flashingCounter >= flashDelay){ 

			flashingCounter = 0;

			toggleFlashing = !toggleFlashing;

			if(toggleFlashing) {
				mySpriteRenderer.enabled = true;
			}
			else {
				mySpriteRenderer.enabled = false;
			}

		}
		else {
			flashingCounter++;
		}

	}

	public void ToggleInvinsibility(){
		receivedDamage = false;
		invulnerable = true;
		invulnerableTimeStamp = Time.time + invulnerableSeconds;
		if(bounceBackAfterReceivingDamage){
			StartCoroutine(BounceBackAfterDamage());
		}
		bounceBackAfterReceivingDamage = false;
	}

	public virtual void IsGrounded(){

//		jumpTimeCounter = jumpTime;
//		finishedJump = true;
		isJumping = false;
		isFalling = false;
		myRigidBody2D.velocity = new Vector2(myRigidBody2D.velocity.x,0);
		animator.SetBool("jumping", false);
		//movementSoundManager.PlayLandingSound();
	}

	public void ChangeLookingDirection(){

		if(isLookingRight){
			GetComponent<SpriteRenderer>().flipX = false;
		} else {
			GetComponent<SpriteRenderer>().flipX = true;
		}
	}

	protected virtual void OnCollisionEnter2D(Collision2D other){


		if(other.gameObject.tag == "Ground" || other.gameObject.tag == "InvisiblePlatform" || other.gameObject.tag == "Enemy"){
			if(!justJumped){
				CheckIfGrounded();
			}

		}

    }

    void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "DeathPit"){
			FellFromStageDeath ();
		}
    }


	protected void ReturnToHub(){
		SceneManager.LoadScene(0);
	}

	public void EnableControls(){
		controlersDisabled = false;
		invulnerable = true;
	}

	public void DisableControls(){
		controlersDisabled = true;
	}

	protected void Freakout(){
		freakoutManager.StartFreakout(this);
	}

	public void StopFreakout(){
		animator.SetBool("freakout",false);
	}

	//Raycast to see if Cat is on Ground
	protected void CheckIfGrounded(){

		Vector3 position = transform.position;
		Vector2 direction = Vector2.down;
		Vector2 direction2 = new Vector2(-0.4f,-1);
		Vector2 direction3 = new Vector2(0.4f,-1);
		float distance = 0.5f;
		if (!FourLeggedCat) {
			distance = 1f;
		}

		RaycastHit2D hit = Physics2D.Raycast(position,direction,distance,LayerMask.GetMask("Enemies","Ground"));


		RaycastHit2D hit2 = Physics2D.Raycast(position,direction2,distance,LayerMask.GetMask("Enemies","Ground"));
		Debug.DrawRay(position, direction2, Color.green);

		RaycastHit2D hit3 = Physics2D.Raycast(position,direction3,distance,LayerMask.GetMask("Enemies","Ground"));


		if(hit.collider != null || hit2.collider != null || hit3.collider != null){
			IsGrounded();
		}


	}

    //WIP
    void CheckSlope()
    {
        // Character is grounded, and no axis pressed: 
        if (!isFalling && !isJumping)
        {
            
            RaycastHit2D hit = Physics2D.Raycast(transform.position - new Vector3(0, 0.4f, 0), Vector2.down, 1, mask);
            //Debug.DrawRay(transform.position, Vector2.down, Color.green);

            // Check if we are on the slope
            if (hit && Mathf.Abs(hit.normal.x) > Mathf.Epsilon)
            {
                Debug.Log("On stairs");
                // We freeze all the rigidbody constraints and put velocity to 0
                //myRigidBody2D.constraints = RigidbodyConstraints2D.FreezeAll;
                myRigidBody2D.velocity = Vector2.zero;
            }
        }
        else
        {

            // if we are on air or moving - jumping, unfreeze all and freeze only rotation.
            myRigidBody2D.constraints = RigidbodyConstraints2D.None;
            myRigidBody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

	protected virtual void CheckIfDamageReceived()
	{

		if (receivedDamage && life > 0) {
			//animator.SetBool("damage",true);

			ToggleInvinsibility ();
		}
	}

	protected void DamageAnimationFinished()
	{
		animator.SetBool("damage",false);
	}

	protected virtual void CheckInvulnerableTimeStamp()
	{
		if (invulnerableTimeStamp < Time.time) {
			if(!freakoutMode && !controlersDisabled){
				invulnerable = false;
				mySpriteRenderer.enabled = true;
			}
		}
	}

	protected virtual void CheckDeath()
	{
		if(life <= 0 ){
			isDying = true;
			animator.SetBool("dying",true);
		}
	}

	protected virtual void DeathAnimationFinished()
	{
		//SceneManager.LoadScene (1);
		StartCoroutine(RestartLevel());
	}

	protected virtual void FellFromStageDeath()
	{
		//SceneManager.LoadScene (1);
		StartCoroutine(RestartLevel());
	}

	IEnumerator RestartLevel(){
		ScreenFade fade = (ScreenFade)FindObjectOfType<ScreenFade>();
		fade.FadeOut();
		yield return new WaitForSeconds(1);
		if(GetComponent<Health>()){
			GetComponent<Health>().FillHealth();
		}
		if(freakoutManager){
			freakoutManager.ResetBar();
		}
		transform.position = currentCheckpoint;
		animator.SetBool("dying",false);
		Idle();
		if(EnemyManager.instance){
			EnemyManager.instance.RespawnEnemies();
		}
		yield return new WaitForSeconds(2);
		fade.FadeIn();
		yield return new WaitForSeconds(1);
		myRigidBody2D.velocity = Vector2.zero;
		isDying = false;
	}

	IEnumerator BounceBackAfterDamage(){

			DisableControls();

			if(isJumping){
				myRigidBody2D.drag = 3f;
			}

			if(sourceOfDamagePosition.x > transform.position.x){ //Enemy is on the right
				if(isWalking && GetComponent<PussInBoots>()){
					myRigidBody2D.velocity = new Vector2( -4f , myRigidBody2D.velocity.y + 3);
				}else{
					myRigidBody2D.velocity = new Vector2( -2f , myRigidBody2D.velocity.y + 2);
				}
	
			}else{
				if(isWalking){
					myRigidBody2D.velocity = new Vector2( 4f , myRigidBody2D.velocity.y + 3);
				}else{
					myRigidBody2D.velocity = new Vector2( 2f , myRigidBody2D.velocity.y + 2);
				}
	
			}


		yield return new WaitForSeconds(0.5f);
		myRigidBody2D.drag = 0;
		EnableControls();
	}

	protected IEnumerator SetJustJumpedOff(){
		yield return new WaitForSeconds(0.2f);
		justJumped = false;
	}

	protected virtual void ResetMovementVariables(){
		movedLeft = false;
		movedRight = false;
	}
}

/*
	Jump that changes height for longer the player keeps the jump button pressed - Saving for maybe using in the future
	if(isFalling){
		myRigidBody2D.gravityScale = fallGravityMultiplier;
	}else if(myRigidBody2D.velocity.y > 0 && !(Input.GetKey (jumpKey) || Input.GetButton(jumpGamepadButton))){
		myRigidBody2D.gravityScale = lowJumpGravityMultiplier;
	}else{
		myRigidBody2D.gravityScale = 1;
	}
			    */



