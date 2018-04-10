using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDefendingRange : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Player"){
			KnightDog enemy = gameObject.GetComponentInParent<KnightDog>();
			enemy.playerInDefendingRange = true;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if(other.tag == "Player"){
			KnightDog enemy = gameObject.GetComponentInParent<KnightDog>();
			enemy.playerInDefendingRange = false;
		}
	}
}
