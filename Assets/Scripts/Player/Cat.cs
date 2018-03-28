using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Cat : MonoBehaviour {

	[SerializeField]
	float speed;
	[SerializeField]
	float jumpForce;

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
	protected bool isAttacking;

//	public LayerMask groundLayer;
    public bool isOnGround;

	public bool finishedLevel;

	public bool isNearStatue;
	public CatStatue nearestStatue;

	public FreakoutManager freakoutManager;

    public bool ready;                                          // Checks if the freak out bar is ready.

    private LayerMask mask = 1<<8;

	public int currencyAmount = 0;

    // Use this for initialization
    protected virtual void Start () {
		animator = GetComponent<Animator>();
		mySpriteRenderer = GetComponent<SpriteRenderer>();
		myRigidBody2D = GetComponent<Rigidbody2D>();
		myRigidBody2D.velocity = new Vector2(myRigidBody2D.velocity.x,0);
		myAudioSource = GetComponent<AudioSource> ();
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
		mySpriteRenderer.flipX = true;

	
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


			isJumping = false;
			isFalling = false;
			myRigidBody2D.velocity = new Vector2(myRigidBody2D.velocity.x,0);
//			animator.SetBool("flying", false);
			animator.SetBool("jumping", false);
//			animator.SetBool("falling",false);
	}

	public void ChangeLookingDirection(){

		if(isLookingRight){
			GetComponent<SpriteRenderer>().flipX = false;
		} else {
			GetComponent<SpriteRenderer>().flipX = true;
		}
	}

	void OnCollisionEnter2D(Collision2D other){
		if(other.gameObject.tag == "DeathPit"){
			FellFromStageDeath ();
		}

        // Increases the fill for the freakout bar
        if(other.gameObject.tag == "FOBPickUp")
        {
            // TODO:: if (other.name == "BigFill"){ freakoutManager.percentage *= 5;} // Add this when we want to make the pickup worth more percentage or less percentage.
            Destroy(other.gameObject);
            freakoutManager.IncreaseFBBar();
        }

        // Increases the fill for the freakout bar
        if (other.gameObject.tag == "HealthPickUp")
        {
            Destroy(other.gameObject);
            Health health = gameObject.GetComponent<Health>();
            health.heal = true;
        }

		if(other.gameObject.tag == "Ground" || other.gameObject.tag == "InvisiblePlatform" || other.gameObject.tag == "Enemy"){
			CheckIfGrounded();
		}
    }

	protected void ReturnToHub(){
		SceneManager.LoadScene(0);
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
		float distance = 0.5f;
		if (!FourLeggedCat) {
			distance = 1f;
		}

		RaycastHit2D hit = Physics2D.Raycast(position,direction,distance,LayerMask.GetMask("Enemies","Ground"));
		Debug.DrawRay(position, direction, Color.green);

		if(hit.collider != null){
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
		SceneManager.LoadScene (1);
	}

	protected void FellFromStageDeath()
	{
		SceneManager.LoadScene (1);
	}

}



