using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Cat : MonoBehaviour {

	[SerializeField]
	float speed;
	[SerializeField]
	float jumpForce;
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
	protected Animator animator;
	protected Rigidbody2D myRigidBody2D;
	protected SpriteRenderer mySpriteRenderer;
	protected AudioSource myAudioSource;

	//States
	public bool isLookingRight;
	public bool isDying;
	public bool receivedDamage;
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

    // Use this for initialization
    protected virtual void Start () {

    	//jumpTimeCounter = jumpTime;
		animator = GetComponent<Animator>();
		mySpriteRenderer = GetComponent<SpriteRenderer>();
		myRigidBody2D = GetComponent<Rigidbody2D>();
		myRigidBody2D.velocity = new Vector2(myRigidBody2D.velocity.x,0);
		myAudioSource = GetComponent<AudioSource> ();
		currentCheckpoint = startingPoint.position;
	}

    private void FixedUpdate()
    {
        CheckSlope();
    }

    protected void MoveRight(){

		animator.SetBool("walking",true);
		animator.SetBool("idle",false);
		isWalking = true;
		isLookingRight = true;

		myRigidBody2D.transform.position += Vector3.right * speed * Time.deltaTime;
		ChangeLookingDirection();

	
	}

	protected void MoveLeft(){

		animator.SetBool("walking",true);
		animator.SetBool("idle",false);
		isWalking = true;
		isLookingRight = false; 


		myRigidBody2D.transform.position += Vector3.left * speed * Time.deltaTime;
		ChangeLookingDirection();

	
	}
    protected void MoveUp()
    {
        myRigidBody2D.transform.position += Vector3.up * speed * Time.deltaTime;
    }
    protected void MoveDown()
    {
        myRigidBody2D.transform.position += Vector3.down * speed * Time.deltaTime;
    }

    protected void Idle(){

		animator.SetBool("walking",false);
		animator.SetBool("idle",true);
		isWalking = false;
	}

	protected void Jump(){

		animator.SetBool("jumping",true);

		myRigidBody2D.velocity = new Vector3(myRigidBody2D.velocity.x,0,0);

		myRigidBody2D.AddForce(new Vector3(0, jumpForce,0), ForceMode2D.Impulse);

		isJumping = true;
		isSliding = false;

		if(animator.GetBool("sliding")){
			animator.SetBool("sliding",false);
		}
		
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

	}

	public void IsGrounded(){

//		jumpTimeCounter = jumpTime;
//		finishedJump = true;
		isJumping = false;
		isFalling = false;
		myRigidBody2D.velocity = new Vector2(myRigidBody2D.velocity.x,0);
		animator.SetBool("jumping", false);
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
			CheckIfGrounded();
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
			animator.SetBool("damage",true);
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
			if(!freakoutMode){
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

	protected void DeathAnimationFinished()
	{
		//SceneManager.LoadScene (1);
		StartCoroutine(RestartLevel());
	}

	protected void FellFromStageDeath()
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

}



