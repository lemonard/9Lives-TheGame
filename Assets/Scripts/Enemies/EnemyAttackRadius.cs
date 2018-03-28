using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackRadius : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Player"){
			BrawlerDog brawlerDog = gameObject.GetComponentInParent<BrawlerDog>();
			brawlerDog.playerInAttackingRange = true;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if(other.tag == "Player"){
			BrawlerDog brawlerDog = gameObject.GetComponentInParent<BrawlerDog>();
			brawlerDog.playerInAttackingRange = false;
		}
	}
}
