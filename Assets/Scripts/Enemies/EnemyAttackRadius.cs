using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackRadius : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Player"){
			Enemy enemy = gameObject.GetComponentInParent<Enemy>();
			enemy.playerInAttackingRange = true;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if(other.tag == "Player"){
			Enemy enemy = gameObject.GetComponentInParent<Enemy>();
			enemy.playerInAttackingRange = false;
		}
	}
}
