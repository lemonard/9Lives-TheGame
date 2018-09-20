using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLight : MonoBehaviour {


	private LightSearchingEnemy enemy;
	// Use this for initialization
	void Start () {
		enemy = GetComponentInParent<LightSearchingEnemy>();
	}
	
	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Player" && !enemy.alert){
			Debug.Log("Cat spotted");
			SpotTarget();
			enemy.TargetSpotted(other.gameObject);
		}
		

	}

	void SpotTarget(){
		
	}
}
