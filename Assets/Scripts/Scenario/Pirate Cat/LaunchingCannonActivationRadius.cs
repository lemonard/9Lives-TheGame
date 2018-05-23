using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchingCannonActivationRadius : MonoBehaviour {

	private LaunchingCannon myCannon;

	// Use this for initialization
	void Start () {
		myCannon = GetComponentInParent<LaunchingCannon>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){

		if(other.tag == "Player" && other.GetComponent<PirateCat>()){
			myCannon.targetCat = other.GetComponent<PirateCat>();
			myCannon.canActivate = true;
			other.GetComponent<PirateCat>().isNearCannon = true;
		}
	}

	void OnTriggerExit2D(Collider2D other){

		if(other.tag == "Player" && other.GetComponent<PirateCat>()){
			myCannon.canActivate = false;
			//myCannon.targetCat = null;
			other.GetComponent<PirateCat>().isNearCannon = false;
		}
	}
}
