using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipWaveMovement : MonoBehaviour {

	public float movementSpeed;
	public float upThreshold;
	public float downThreshold;
	public bool startGoingUp = true;

	public bool goingUp;

	private Vector3 velocity = Vector3.zero;

	// Use this for initialization
	void Start () {
		goingUp = startGoingUp;

		upThreshold = transform.position.y + 0.18f;
		downThreshold = transform.position.y - 0.18f;
	}
	
	// Update is called once per frame
	void Update () {
		if(goingUp){

			if(transform.position.y >= upThreshold){
				goingUp = false;
			}else{
				//transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, upThreshold + 0.2f, transform.position.z), movementSpeed  * Time.deltaTime);
				transform.position = Vector3.SmoothDamp (transform.position, new Vector3 (transform.position.x, upThreshold + 0.2f, transform.position.z),ref velocity,  movementSpeed * Time.deltaTime);
			}

		}else{

			if(transform.position.y <= downThreshold){
				goingUp = true;
			}else{
				//transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, downThreshold - 0.2f, transform.position.z), movementSpeed  * Time.deltaTime);
				transform.position = Vector3.SmoothDamp (transform.position, new Vector3 (transform.position.x, downThreshold - 0.2f, transform.position.z),ref velocity,  movementSpeed * Time.deltaTime);
			}

		}
	}
}
