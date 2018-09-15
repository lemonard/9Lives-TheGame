using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

	public Transform rightOrUpThreshold; 
	public Transform leftOrDownThreshold;

	public float speed = 0.5f;
	public bool movementIsHorizontal;

	public bool goingRight;
	public bool goingUp;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate(){
		if(movementIsHorizontal){
			MoveX();
		}else{
			MoveY();
		}

	}

	void MoveX(){

		//Turn left if passed right limit
		if(transform.position.x >= rightOrUpThreshold.position.x){
			goingRight = false;
	
		}

		//Turn right if passed left limit
		if(transform.position.x <= leftOrDownThreshold.position.x){
			goingRight = true;

		}

		//Move right if is looking right
		if(goingRight){
			GetComponent<Rigidbody2D>().transform.position += Vector3.right * speed * Time.deltaTime;
		} else { // Move left if is looking left
			GetComponent<Rigidbody2D>().transform.position += Vector3.left * speed * Time.deltaTime;
		}
		
	}

	//Moves between the 2 limits on the y axis
	void MoveY(){

		//Go down if passed up limit
		if(transform.position.y >= rightOrUpThreshold.position.y){
			goingUp = false;

		}

		//Go up if passed down limit
		if(transform.position.y <= leftOrDownThreshold.position.y){
			goingUp = true;

		}

		//Move right if is looking right
		if(goingUp){
			GetComponent<Rigidbody2D>().transform.position += Vector3.up * speed * Time.deltaTime;
		} else { // Move left if is looking left
			GetComponent<Rigidbody2D>().transform.position += Vector3.down * speed * Time.deltaTime;
		}
		
	}

	void OnCollisionEnter2D(Collision2D other){
		if(other.gameObject.tag == "Player"){
			other.transform.SetParent(gameObject.transform);
		}

	}

	void OnCollisionExit2D(Collision2D other){
		if(other.gameObject.tag == "Player"){
			other.transform.SetParent(null);
		}

	}

}
