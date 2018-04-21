using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingCheckpoint : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Player"){
			MagicCat player = other.gameObject.GetComponent<MagicCat>();
			player.lastFallingCheckpoint = gameObject;
		}
	}
}
