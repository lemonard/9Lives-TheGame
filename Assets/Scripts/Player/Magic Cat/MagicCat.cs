using UnityEngine;
using System.Collections;

public class MagicCat : Cat {

	//	//Childs objects
	public Transform rightFiringPoint;
	public Transform leftFiringPoint;
	public GameObject projectile;
	public GameObject magicPulsePrefab;
	public Transform pulseOrigin;

	public ParticleSystem broomParticleRight;
	public ParticleSystem broomParticleLeft;

	public AudioClip magicShotSound;
	public AudioClip magicPulseSound;
	public AudioClip magicBroomSound;

	public string shootMagicGamepadButton;
	public string magicPulseGamepadButton;
	public string returnToHubGamepadButton;

    public Vector3 tempPos;

	public KeyCode shootKey;
	public KeyCode magicPulseKey;
	public KeyCode returnToHubKey;

	public bool canLevitate;
    public int levitateDelay = 3;
	public float levitateCooldown = 3;
    public bool levitate = false;
    public float doubleJump = 0;

	public bool isPulsing;
	private bool returningAfterFalling;

	private float levitateCooldownTimeStamp = 0;
	private Coroutine levitateCoroutine;
	// Use this for initialization

	public GameObject lastFallingCheckpoint;
	public GameObject returningParticlePrefab;

	private GameObject currentReturningParticle;

	private ParticleSystem.EmissionModule broomParticleRightEmissionModule;
	private ParticleSystem.MainModule broomParticleRightMainModule;
	private ParticleSystem.EmissionModule broomParticleLeftEmissionModule;
	private ParticleSystem.MainModule broomParticleLeftMainModule;

	private float initialBroomParticleRate;
	private ParticleSystem.MinMaxCurve initialBroomParticleSpeed;

	private bool lastLookingDirection;


	protected override void Start ()
	{
		base.Start ();
		canLevitate = true;

		broomParticleRightEmissionModule = broomParticleRight.emission;
		broomParticleRightMainModule = broomParticleRight.main;

		broomParticleLeftEmissionModule = broomParticleLeft.emission;
		broomParticleLeftMainModule = broomParticleLeft.main;

		initialBroomParticleRate = broomParticleRightEmissionModule.rateOverTime.constant;
		initialBroomParticleSpeed = broomParticleRightMainModule.startSpeed;

		lastLookingDirection = isLookingRight;
	}


	
	// Update is called once per frame
	void Update () {

		if(!controlersDisabled){
			if(Input.GetButtonDown(returnToHubGamepadButton) || Input.GetKeyDown(returnToHubKey)){
				ReturnToHub();
			}

			if((Input.GetButtonDown(freakoutGamepadButton) || Input.GetKeyDown(freakoutKey) ) && !isDying && !isAttacking && !isJumping && !isPulsing && ready){
				freakoutMode = true;
				freakoutManager.PlayFreakOutSound ();
				animator.SetBool("freakout",true);
			}

			if(!isDying && !freakoutMode){

				if(!isAttacking && !isPulsing){

	                if (Input.GetKey(moveRightKey) || (Input.GetAxis(moveHorizontalGamepadAxis) >= 0.5f))
	                {
	                    MoveRight();
	                }
	                else if (Input.GetKey(moveLeftKey) || (Input.GetAxis(moveHorizontalGamepadAxis) <= -0.5f))
	                {
	                    MoveLeft();
	                }
	                else
	                {
	                    Idle();
	                }

					if ((Input.GetKeyDown (downKey) && levitate) || Input.GetAxis (moveVerticalGamepadAxis) <= -0.5f && levitate) {
						CancelLevitate ();
					}

	//                if (Input.GetKey(jumpKey) && levitate || Input.GetAxis(moveVerticalGamepadAxis) >= 0.5f && levitate)
	//                {
	//                    MoveUp();
	//                }
	//
	//                if (Input.GetKey(downKey) && levitate || Input.GetAxis(moveVerticalGamepadAxis) <= -0.5f && levitate)
	//                {
	//                    MoveDown();
	//                }

					if(Input.GetKeyDown (jumpKey) || Input.GetButtonDown(jumpGamepadButton)){

						if(levitate){
							CancelLevitate();
							Jump ();
						}

						if (!levitate && canLevitate && (isJumping || isFalling)) {
							Levitate ();
						}

						if(!isFalling){

							if (!isJumping) {
								Jump ();
							}
						}


					}


	                if (Input.GetKeyDown (shootKey) || Input.GetButtonDown(shootMagicGamepadButton)){
						
						StartProjectile();
					}

					if((Input.GetKeyDown (magicPulseKey) || Input.GetButtonDown(magicPulseGamepadButton)) && !isJumping && !isFalling){ 
						StartMagicPulse();
					}
				}

				if(myRigidBody2D.velocity.y < -0.2f){
					isFalling = true;
				}

				if(levitate){
					if(lastLookingDirection != isLookingRight){
						if(isLookingRight){
							broomParticleRight.Stop();
							broomParticleLeft.Play();
						}else{
							broomParticleRight.Play();
							broomParticleLeft.Stop();
						}
					}
				}

				lastLookingDirection = isLookingRight;
			}
		}else{
			if(levitate){
				CancelLevitate();
			}
			Idle();
		}

		if(returningAfterFalling){
			ReturnToFallingCheckpoint();

		} 

		CheckInvulnerableTimeStamp ();

		if (invulnerable) {
			Flash ();
		}

		if (levitateCooldownTimeStamp < Time.time) {
			canLevitate = true;
		}

		if(isAttacking){
			if(animator.GetCurrentAnimatorStateInfo(0).IsName("MagicCatAttaking") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1){
				FinishProjectile();
			}
		}

		if(isPulsing){
			if(animator.GetCurrentAnimatorStateInfo(0).IsName("MagicCatPulse") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1){
				FinishMagicPulse();
			}
			if(animator.GetCurrentAnimatorStateInfo(0).IsName("MagicCatJump") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1){
				FinishMagicPulse();
			}
		}

		CheckIfDamageReceived ();

		CheckDeath ();
	}


	protected override void OnCollisionEnter2D (Collision2D other)
	{
		if(other.gameObject.tag == "Ground" || other.gameObject.tag == "InvisiblePlatform" || other.gameObject.tag == "Enemy"){
			if(!levitate){
				CheckIfGrounded();
			}
		}
	}

    private void FixedUpdate()
    {

//		if(!levitate){
//		    if(isFalling){
//				myRigidBody2D.velocity += Vector2.up * Physics2D.gravity.y * (fallGravityMultiplier - 1) * Time.deltaTime; 
//			}else if(myRigidBody2D.velocity.y > 0 && !(Input.GetKey (jumpKey) || Input.GetButton(jumpGamepadButton))){
//				myRigidBody2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpGravityMultiplier - 1) * Time.deltaTime; 
//		    }
//	    }

   	}

	void Levitate(){

		if(isLookingRight){
			broomParticleLeft.Play();

		}else{
			broomParticleRight.Play();
		}
		levitate = true;
		animator.SetBool("levitate", true);
		myRigidBody2D.gravityScale = 0;
		myRigidBody2D.velocity = Vector2.zero;
		levitateCoroutine = StartCoroutine(LevitateOff());


	}
    
	void CancelLevitate(){

		broomParticleRightMainModule.startSpeed = initialBroomParticleSpeed;
		broomParticleRightEmissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(initialBroomParticleRate);

		broomParticleLeftMainModule.startSpeed = initialBroomParticleSpeed;
		broomParticleLeftEmissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(initialBroomParticleRate);

		broomParticleLeft.Stop();
		broomParticleRight.Stop();

		StopCoroutine (levitateCoroutine);
		canLevitate = false;
		levitateCooldownTimeStamp = Time.time + levitateCooldown;
		levitate = false;
		animator.SetBool("levitate", false);
		myRigidBody2D.gravityScale = 1;
		CheckIfGrounded();
		SkillCooldownIndicator.instance.StartCooldown();


	}

    IEnumerator LevitateOff()
    {

		yield return new WaitForSeconds(levitateDelay / 3f);
		broomParticleRightMainModule.startSpeed = new ParticleSystem.MinMaxCurve(2f,5f);
		broomParticleRightEmissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(initialBroomParticleRate / 2);

		broomParticleLeftMainModule.startSpeed = new ParticleSystem.MinMaxCurve(2f,5f);
		broomParticleLeftEmissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(initialBroomParticleRate / 2);

		yield return new WaitForSeconds(levitateDelay / 3f);
		broomParticleRightMainModule.startSpeed = new ParticleSystem.MinMaxCurve(1f,2f);
		broomParticleRightEmissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(initialBroomParticleRate / 4);

		broomParticleLeftMainModule.startSpeed = new ParticleSystem.MinMaxCurve(1f,2f);
		broomParticleLeftEmissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(initialBroomParticleRate / 4);

		yield return new WaitForSeconds(levitateDelay / 3f);

		broomParticleRight.Stop();
		broomParticleLeft.Stop();

		broomParticleRightMainModule.startSpeed = initialBroomParticleSpeed;
		broomParticleRightEmissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(initialBroomParticleRate);

		broomParticleLeftMainModule.startSpeed = initialBroomParticleSpeed;
		broomParticleLeftEmissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(initialBroomParticleRate);

		print ("Leviate off called");
		canLevitate = false;
		levitateCooldownTimeStamp = Time.time + levitateCooldown;
		levitate = false;
		animator.SetBool("levitate", false);
		myRigidBody2D.gravityScale = 1;
		CheckIfGrounded();
		SkillCooldownIndicator.instance.StartCooldown();
       
    }


//	void LateUpdate(){
//
//		transform.position = new Vector3(Mathf.Round(transform.position.x),Mathf.Round(transform.position.y),0);	
//	}

	protected override void FellFromStageDeath ()
	{
		DisableControls();
		myRigidBody2D.gravityScale = 0;
		myRigidBody2D.velocity = Vector3.zero;
		returningAfterFalling = true;
		GetComponent<BoxCollider2D>().enabled = false;
		GetComponent<SpriteRenderer>().enabled = false;
		currentReturningParticle = (GameObject)Instantiate(returningParticlePrefab,transform.position,Quaternion.identity,transform);
	}

	void ReturnToFallingCheckpoint(){

		if(transform.position.x == lastFallingCheckpoint.transform.position.x && transform.position.y == lastFallingCheckpoint.transform.position.y){
			EnableControls();
			returningAfterFalling = false;
			myRigidBody2D.gravityScale = 1;
			GetComponent<BoxCollider2D>().enabled = true;
			GetComponent<SpriteRenderer>().enabled = true;
			Destroy(currentReturningParticle);
			currentReturningParticle = null;
		}else{
			transform.position = Vector3.MoveTowards(transform.position, lastFallingCheckpoint.transform.position, speed * 2 * Time.deltaTime);
		}
	}

	private void StartProjectile(){

		
		isAttacking = true;
		animator.SetBool("projectile", true);

		myAudioSource.PlayOneShot(magicShotSound);

	}

	public void FireProjectile(){

		animator.SetBool("idle", false);
		if(isLookingRight){
			Instantiate(projectile,rightFiringPoint.position,Quaternion.identity);
		} else {
			Instantiate(projectile,leftFiringPoint.position,Quaternion.identity);
		}
	}

	public void FinishProjectile(){
		
		isAttacking = false;
		animator.SetBool("idle", true);
		animator.SetBool("projectile", false);
	}

	void StartMagicPulse(){

		isPulsing = true;
		animator.SetBool("magicPulse",true);
		animator.SetBool("idle",false);
		animator.SetBool("walking",false);

		myAudioSource.PlayOneShot(magicPulseSound);
	}

	public void CreateMagicPulse(){
		Instantiate(magicPulsePrefab,pulseOrigin.position,Quaternion.identity);
	}

	public void FinishMagicPulse(){

		animator.SetBool("magicPulse",false);
		animator.SetBool("walking",false);
		animator.SetBool("idle",true);

		isPulsing = false;
	}


}
