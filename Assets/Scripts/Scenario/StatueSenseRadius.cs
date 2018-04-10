using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueSenseRadius : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Player"){
			EyeStatue statue = gameObject.GetComponentInParent<EyeStatue>();
			statue.playerInRange = true;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if(other.tag == "Player"){
			EyeStatue statue = gameObject.GetComponentInParent<EyeStatue>();
			statue.playerInRange = false;
		}
	}
}
