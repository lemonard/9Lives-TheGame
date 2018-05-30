using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNearPlayerSensor : MonoBehaviour {

	private CombableEnemy enemy;

	// Use this for initialization
	void Start () {
		enemy = GetComponentInParent<CombableEnemy>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Player"){
			if((GameManager.instance.enemiesNearCat.Count < GameManager.instance.maxAmountOfEnemiesNearCat) && (GameManager.instance.enemiesNearCat.IndexOf(enemy.gameObject) == -1) ){
				GameManager.instance.AddToEnemyNearCatList(enemy.gameObject);
			}
		}	
	}

	void OnTriggerExit2D(Collider2D other){
		if(other.tag == "Player"){
			GameManager.instance.RemoveFromEnemyNearCatList(enemy.gameObject);
		}
	}
}
