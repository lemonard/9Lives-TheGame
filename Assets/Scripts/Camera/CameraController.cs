//Script by Florian Grundmann (IndieFlorianG)
//You can use and change this script as you like.
//Credit would be nice, but is not needed

using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public static CameraController instance;

	public bool follow = true; //Whether the camera should follow the player
	public Vector3 moveTo; //Target vector
	public GameObject target;
	private Cat player;
    private PussInBoots sebby;
	private Vector3 velocity = Vector3.zero;

	public float speedX = 12f; //Speed of the camera
	public float speedY = 4f;
	public float yTolerance = 1.3f; //Tolerance in y direction
	public float xTolerance = 0.3f; //Tolerance in x direction

	private float initialSpeedX;

	private bool movingRight = true; //Whether the player is on the way right

    public GameObject topBorder;
    public GameObject bottomBorder; //Border the player can't come bellow
	public GameObject leftBorder; //Border the player can't cross if he is moving right
	public GameObject leftTolerance; //Only if the player crosses this border he is viewed as moving left
	public GameObject rightBorder; //Border the player can't cross if he is moving left
	public GameObject rightTolerance; //Only if the player crosses this border he is viewed as moving right

	public Transform leftCameraLimit;
	public Transform rightCameraLimit;
	public Transform topCameraLimit;
	public Transform downCameraLimit;

	private float playerXOffset;
	private float playerYOffset;

	private float yOffset; //Offset of the camera
	private float xOffset; //Offset of the camera
	private bool animationMode;
	private bool noDampMode;

	public bool startLookingLeft;
	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {
		//Find the player
		player = FindObjectOfType<Cat>();
        sebby = FindObjectOfType<PussInBoots>();
        initialSpeedX = speedX;
        //Set the offsets
		playerXOffset = leftBorder.transform.localPosition.x;
		playerYOffset = - bottomBorder.transform.localPosition.y;

		yOffset = playerYOffset;
		xOffset = playerXOffset;

		target = player.gameObject;
		//Set camera to start position
		if(startLookingLeft){
			moveTo = new Vector3(player.transform.position.x + xOffset, player.transform.position.y + yOffset, transform.position.z);
			movingRight = false;
		}else{
			moveTo = new Vector3(player.transform.position.x - xOffset, player.transform.position.y + yOffset, transform.position.z);
		}
		//moveTo = new Vector3(player.transform.position.x - xOffset, transform.position.y, transform.position.z);
		transform.position = moveTo;
	}
	
	// Update is called once per frame
	void LateUpdate () {

		if (follow) {
			if(!animationMode && !noDampMode){
				//Update x and y position
				UpdateXDirection();
				UpdateYDirection();
			}else if(animationMode && !noDampMode){
				UpdateXDirectionAnimation();
				UpdateYDirectionAnimation();
			}else if(!animationMode && noDampMode){
				transform.position = new Vector3 (target.transform.position.x, target.transform.position.y, transform.position.z);
			}
		}
	}


	private void UpdateYDirection(){

		
		transform.position = Vector3.Lerp (transform.position, new Vector3 (transform.position.x, moveTo.y, transform.position.z), speedY * Time.deltaTime);
		
		//If the player is bellow the bottom border move instantly to him in y direction 
		if (target.transform.position.y < bottomBorder.transform.position.y) {
			moveTo = new Vector3 (moveTo.x, target.transform.position.y + yOffset, moveTo.z);
			transform.position = new Vector3 (transform.position.x, moveTo.y, transform.position.z);
		}

        //If the player is above the top border move instantly to him in y direction 
        if(topBorder){
			if (target.transform.position.y > topBorder.transform.position.y)
	        {
				moveTo = new Vector3(moveTo.x, target.transform.position.y - yOffset, moveTo.z);
	            transform.position = new Vector3(transform.position.x, moveTo.y, transform.position.z);
	        }
        }

        //If the player is standing on ground, set the target vector
        if (!player.isJumping) {
			if (target.transform.position.y - (moveTo.y - yOffset) > yTolerance) 
				moveTo = new Vector3 (moveTo.x, target.transform.position.y + yOffset, moveTo.z);
		}

        //If the player as Sebastian is climbing, set the target vector
        if(player.GetComponent<PussInBoots>()){
			if (player.GetComponent<PussInBoots>().isClimbing)
			{
				if (target.transform.position.y - (moveTo.y - yOffset) > yTolerance)
	            {
					moveTo = new Vector3(moveTo.x, target.transform.position.y + yOffset, moveTo.z);
	        	}
       		}
       	}


//		if(moveTo.y > topCameraLimit.position.y || moveTo.y < downCameraLimit.position.y){
//			moveTo = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
//		}


       
    }

	private void UpdateXDirection(){


		transform.position = Vector3.SmoothDamp (transform.position, new Vector3 (moveTo.x, transform.position.y, transform.position.z),ref velocity,  speedX * Time.deltaTime);


		if (movingRight) {
			//If player is far away from the left border (= If the direction changed shortly)
			//move to the player in x direction
			if (target.transform.position.x > leftBorder.transform.position.x + xTolerance) {
				moveTo = new Vector3 (target.transform.position.x - xOffset, moveTo.y, moveTo.z);

				//If the player just crossed the left border, move instantly to the player in x direction
			}else if (target.transform.position.x > leftBorder.transform.position.x) {
				moveTo = new Vector3 (target.transform.position.x - xOffset, moveTo.y, moveTo.z);
			
				//transform.position = new Vector3 (moveTo.x, transform.position.y, transform.position.z);
			}
			//If the player crossed the left tolerance border, change the direction
			if (target.transform.position.x < leftTolerance.transform.position.x) {
				movingRight = false;
			}
		} else {
			//If player is far away from the right border (= If the direction changed shortly)
			//move to the player in x direction
			if (target.transform.position.x < rightBorder.transform.position.x - xTolerance) {
				moveTo = new Vector3 (target.transform.position.x + xOffset, moveTo.y, moveTo.z);
				//dampSpeedX = turnSpeedX;
				//If the player just crossed the right border, move instantly to the player in x direction
			} else if (target.transform.position.x < rightBorder.transform.position.x) {
				moveTo = new Vector3 (target.transform.position.x + xOffset, moveTo.y, moveTo.z);

				//transform.position = new Vector3 (moveTo.x, transform.position.y, transform.position.z);
			}
			//If the player crossed the right tolerance border, change the direction
			if (target.transform.position.x > rightTolerance.transform.position.x) {
				movingRight = true;
			}
		}

//		if(moveTo.x > rightCameraLimit.position.x || moveTo.x < leftCameraLimit.position.x){
//			print("Oie");
//			moveTo = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
//		}
	}

	private void UpdateXDirectionAnimation(){

		transform.position = Vector3.SmoothDamp (transform.position, new Vector3 (moveTo.x, transform.position.y, transform.position.z),ref velocity,  speedX * Time.deltaTime);

		moveTo = new Vector3 (target.transform.position.x, moveTo.y, moveTo.z);
	}

	private void UpdateYDirectionAnimation(){
		transform.position = Vector3.Lerp (transform.position, new Vector3 (transform.position.x, moveTo.y, transform.position.z), speedY * Time.deltaTime);

		moveTo = new Vector3 (moveTo.x, target.transform.position.y, moveTo.z);

	}



	public void JumpTo(Vector3 targetPos){
		transform.position = targetPos + new Vector3 (xOffset, yOffset, transform.position.z);
	}

	public void ActivateAnimationMode(){
		animationMode = true;
	}

	public void DeactivateAnimationMode(){
		animationMode = false;
	}

	public void ChangeToTargetAndCenter(GameObject target){
		ActivateAnimationMode();
		this.target = target;
		yOffset = 0;
		xOffset = 0;
	}

	public void ChangeToTargetAndCenterNoDamp(GameObject target){
		noDampMode = true;
		this.target = target;
		yOffset = 0;
		xOffset = 0;
	}

	public void ReturnCameraToPlayerFollowing(){
		DeactivateAnimationMode();
		noDampMode = false;
		this.target = player.gameObject;
		yOffset = playerYOffset;
		xOffset = playerXOffset;
	}

	public void UpdateActiveCat(){
		player = FindObjectOfType<Cat>();
		target = player.gameObject;
	}

	public void SetSpeedX(float speed){
		speedX = speed;
	}

	public void RestoreSpeedX(){
		speedX = initialSpeedX;
	}
}
